using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using BizHawk.Client.EmuHawk.ToolExtensions;
using BizHawk.Emulation.Common;
using BizHawk.Client.Common;

using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BizHawk.Client.EmuHawk
{
	public partial class BasicBot : ToolFormBase , IToolFormAutoConfig
	{
		private const string DialogTitle = "Basic Bot";
		private bool _isBotting = false;
		private long _frames = 0;
		private int _targetFrame = 0;
		private bool _oldCountingSetting = false;
		private string _lastRom = "";
		private MemoryDomain _currentDomain;
		private bool _isBigEndian;
		private int _dataSize;

		#region Services and Settings

		[RequiredService]
		private IEmulator _Emulator { get; set; }
		// Unused, due to the use of MainForm to loadstate, but this needs to be kept here in order to establish an IStatable dependency
		[RequiredService]
		private IStatable _StatableCore { get; set; }
		[RequiredService]
		private IMemoryDomains _MemoryDomains { get; set; }

		#endregion

		#region Initialize

		public BasicBot()
		{
			this.InitializeComponent();
			this.Text = DialogTitle;
			RunBtn.Enabled = false;
		}

		private void BasicBot_Load(object sender, EventArgs e)
		{
			;
		}

		private void LoadConfig()
		{
			OpenFileDialog Dialog = new OpenFileDialog();
			if (Dialog.ShowDialog() == DialogResult.OK)
			{
				InputLstBx.Items.Clear();
				OutputLstBx.Items.Clear();
				_buttons.Clear();
				_maps.Clear();
				_addresses.Clear();
				_outputs.Clear();

				string ConfigString = System.IO.File.ReadAllText(Dialog.FileName);
				JObject Configs = JObject.Parse(ConfigString);
				{
					JToken InputListToken = Configs.GetValue("InputList");
					JObject InputList = InputListToken.ToObject<JObject>();
					{
						foreach (var Pair in InputList)
						{
							_buttons.Add(Pair.Key, 0);
							_maps.Add(Pair.Key, new List<string>() { string.Empty });

							InputLstBx.Items.Add("[" + Pair.Key + "]");
							var List = Pair.Value;
							foreach (string Button in List)
							{
								InputLstBx.Items.Add(Button);
								_maps[Pair.Key].Add(Button);
							}
						}
					}

					JToken OutputListToken = Configs.GetValue("OutputList");
					JObject OutputList = OutputListToken.ToObject<JObject>();
					{
						foreach (var Pair in OutputList)
						{
							_addresses.Add(Pair.Key, Convert.ToInt32(Pair.Value.ToString(), 16));
							_outputs.Add(Pair.Key, 0);

							OutputLstBx.Items.Add(Pair.Key + " : " + Pair.Value);
						}
					}
				}

				RunBtn.Enabled = true;
			}
		}

		#endregion

		#region Network

		private Socket _socket;
		private IPEndPoint _endPoint;
		private byte[] _packet = new byte[1024];

		private Dictionary<string, int> _buttons = new Dictionary<string, int>();
		private Dictionary<string, List<string>> _maps = new Dictionary<string, List<string>>();
		private Dictionary<string, int> _addresses = new Dictionary<string, int>();
		private Dictionary<string, int> _outputs = new Dictionary<string, int>();

		private void ConnectServer()
		{
			IPAddress ip = IPAddress.Parse(TB_IP.Text);
			int port = Int32.Parse(TB_Port.Text);

			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_endPoint = new IPEndPoint(ip, port);
			_socket.Connect(_endPoint);

			TB_IP.Enabled = false;
			TB_Port.Enabled = false;
		}

		private void DisconnectServer()
		{
			_socket.Close();

			TB_IP.Enabled = true;
			TB_Port.Enabled = true;
		}

		private void ReadMemory()
		{
			foreach(var Pair in _addresses.ToList())
			{
				_outputs[Pair.Key] = this.GetRamValue(Pair.Value);
			}
		}

		private void MakePacket()
		{
			Array.Clear(_packet, 0, _packet.Length);

			Dictionary<string, int> data = new Dictionary<string, int>();
			foreach (var Pair in _outputs)
			{
				data.Add(Pair.Key, Pair.Value);
			}

			string json = JsonConvert.SerializeObject(data);
			_packet = Encoding.UTF8.GetBytes(json);
		}

		private void SendPacket()
		{
			_socket.Send(_packet, SocketFlags.None);
		}

		private void ReceiveAction()
		{
			Array.Clear(_packet, 0, _packet.Length);

			int PacketSize = _socket.Receive(_packet);
			string data = Encoding.UTF8.GetString(_packet, 0, PacketSize);
			var keys = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);

			foreach(var Pair in _buttons.ToList())
			{
				try
				{
					_buttons[Pair.Key] = keys[Pair.Key];
				}
				catch
				{
					_buttons[Pair.Key] = 0;
				}
			}
		}

		private void PrintPacket()
		{
			Console.WriteLine(Encoding.UTF8.GetString(_packet, 0, _packet.Length));
		}

		#endregion

		#region UI Bindings

		private string SelectedSlot
		{
			get
			{
				if (StartFromSlotBox.SelectedItem == null)
				{
					return "QuickSave0";
				}
				else
				{
					return "QuickSave" + StartFromSlotBox.SelectedItem.ToString().Last();
				}
			}
		}

		private long Frames
		{
			get
			{
				return _frames;
			}
			set
			{
				_frames = value;
			}
		}

		public string FromSlot
		{
			get
			{
				return StartFromSlotBox.SelectedItem != null 
					? StartFromSlotBox.SelectedItem.ToString()
					: "";
			}
			set
			{
				var item = StartFromSlotBox.Items.
					OfType<object>()
					.FirstOrDefault(o => o.ToString() == value);

				if (item != null)
				{
					StartFromSlotBox.SelectedItem = item;
				}
				else
				{
					StartFromSlotBox.SelectedItem = null;
				}
			}
		}

		#endregion

		#region IToolForm Implementation

		public bool UpdateBefore { get { return true; } }

		public void NewUpdate(ToolFormUpdateType type)
		{
			;
		}

		public void UpdateValues()
		{
			Update(fast: false);
		}

		public void FastUpdate()
		{
			Update(fast: true);
		}

		private void Update(bool fast)
		{
			if(_isBotting)
			{
				try
				{
					this.ReadMemory();

					this.ReceiveAction();
					this.MakePacket();
					this.SendPacket();
					this.PrintPacket();

					this.PressButtons();
				}
				catch
				{
					this.StopBot();
				}
			}
		}

		private bool IsCanInput()
		{
			return true;
		}

		private void PressButtons()
		{
			foreach(var Pair in _buttons.ToList())
			{
				if((0 < Pair.Value) && (Pair.Value < _maps[Pair.Key].Count))
				{
					Global.LuaAndAdaptor.SetButton(_maps[Pair.Key][Pair.Value], true);
				}
			}
		}

		public void Restart()
		{
			if (_currentDomain == null ||
				_MemoryDomains.Contains(_currentDomain))
			{
				_currentDomain = _MemoryDomains.MainMemory;
				_isBigEndian = _currentDomain.EndianType == MemoryDomain.Endian.Big;
				_dataSize = 2;
			}

			if (_isBotting)
			{
				StopBot();
			}

			if (_lastRom != GlobalWin.MainForm.CurrentlyOpenRom)
			{
				_lastRom = GlobalWin.MainForm.CurrentlyOpenRom;
			}
		}

		public bool AskSaveChanges()
		{
			return true;
		}

		#endregion

		#region Control Events

		#region Options Menu

		private void MemoryDomainsMenuItem_DropDownOpened(object sender, EventArgs e)
		{
			MemoryDomainsMenuItem.DropDownItems.Clear();
			MemoryDomainsMenuItem.DropDownItems.AddRange(
				_MemoryDomains.MenuItems(SetMemoryDomain, _currentDomain.Name)
				.ToArray());
		}

		private void BigEndianMenuItem_Click(object sender, EventArgs e)
		{
			_isBigEndian ^= true;
		}

		private void DataSizeMenuItem_DropDownOpened(object sender, EventArgs e)
		{
			_1ByteMenuItem.Checked = _dataSize == 1;
			_2ByteMenuItem.Checked = _dataSize == 2;
			_4ByteMenuItem.Checked = _dataSize == 4;
		}

		private void _1ByteMenuItem_Click(object sender, EventArgs e)
		{
			_dataSize = 1;
		}

		private void _2ByteMenuItem_Click(object sender, EventArgs e)
		{
			_dataSize = 2;
		}

		private void _4ByteMenuItem_Click(object sender, EventArgs e)
		{
			_dataSize = 4;
		}

		#endregion

		private void RunBtn_Click(object sender, EventArgs e)
		{
			try
			{
				this.StartBot();
			}
			catch
			{
				this.StopBot();
			}
		}

		private void StopBtn_Click(object sender, EventArgs e)
		{
			try
			{
				this.StopBot();
			}
			catch
			{
				this.Close();
			}
		}

		private void ClearStatsContextMenuItem_Click(object sender, EventArgs e)
		{
			Frames = 0;
		}

		#endregion

		private void SetMemoryDomain(string name)
		{
			_currentDomain = _MemoryDomains[name];
			_isBigEndian = _MemoryDomains[name].EndianType == MemoryDomain.Endian.Big;
		}

		private int GetRamValue(int addr)
		{
			int val;
			switch (_dataSize)
			{
				default:
				case 1:
					val = _currentDomain.PeekByte(addr);
					break;
				case 2:
					val = _currentDomain.PeekUshort(addr, _isBigEndian);
					break;
				case 4:
					val = (int)_currentDomain.PeekUint(addr, _isBigEndian);
					break;
			}

			return val;
		}

		private void StartBot()
		{
			if (!CanStart())
			{
				MessageBox.Show("Unable to run with current settings");
				return;
			}

			_isBotting = true;
			StartFromSlotBox.Enabled = false;
			RunBtn.Visible = false;
			StopBtn.Visible = true;

			if (Global.MovieSession.Movie.IsRecording)
			{
				_oldCountingSetting = Global.MovieSession.Movie.IsCountingRerecords;
				Global.MovieSession.Movie.IsCountingRerecords = false;
			}

			GlobalWin.MainForm.LoadQuickSave(SelectedSlot, false, true);

			_targetFrame = _Emulator.Frame + 0;

			GlobalWin.MainForm.UnpauseEmulator();

			this.ConnectServer();
		}

		private bool CanStart()
		{
			return true;
		}

		private void StopBot()
		{
			RunBtn.Visible = true;
			StopBtn.Visible = false;
			_isBotting = false;
			_targetFrame = 0;
			StartFromSlotBox.Enabled = true;
			_targetFrame = 0;

			if (Global.MovieSession.Movie.IsRecording)
			{
				Global.MovieSession.Movie.IsCountingRerecords = _oldCountingSetting;
			}

			GlobalWin.MainForm.PauseEmulator();

			this.DisconnectServer();
			this.Close();
		}

		private void SetMaxSpeed()
		{
			GlobalWin.MainForm.Unthrottle();
		}

		private void SetNormalSpeed()
		{
			GlobalWin.MainForm.Throttle();
		}

		private void LoadConfigBtn_Click(object sender, EventArgs e)
		{
			this.LoadConfig();
		}
	}
}
