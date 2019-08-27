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

			this.LoadConfigJson();
		}

		private void BasicBot_Load(object sender, EventArgs e)
		{
			;
		}

		private void LoadConfigJson()
		{
			string ConfigString = System.IO.File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()) + "/Config.json");
			JObject Configs = JObject.Parse(ConfigString);
			{
				JToken InputListToken = Configs.GetValue("InputList");
				JObject InputList = InputListToken.ToObject<JObject>();
				{
					foreach(var Pair in InputList)
					{
						_buttons.Add(Pair.Key, 0);
						_maps.Add(Pair.Key, new List<string>() { string.Empty });

						LB_Input.Items.Add("[" + Pair.Key + "]");
						var List = Pair.Value;
						foreach(string Button in List)
						{
							LB_Input.Items.Add(Button);
							_maps[Pair.Key].Add(Button);
						}
					}
				}

				JToken OutputListToken = Configs.GetValue("OutputList");
				JObject OutputList = OutputListToken.ToObject<JObject>();
				{
					foreach(var Pair in OutputList)
					{
						_addresses.Add(Pair.Key, Pair.Value.Value<int>());
						_outputs.Add(Pair.Key, 0);

						LB_Output.Items.Add(Pair.Key);
					}
				}
			}
		}

		#endregion

		#region Network

		private Socket _socket;
		private IPEndPoint _endPoint;
		private byte[] _buffer = new byte[1024];
		private int _bufferSize = 0;

		private Dictionary<string, int> _buttons = new Dictionary<string, int>();
		private Dictionary<string, List<string>> _maps = new Dictionary<string, List<string>>();
		private Dictionary<string, int> _addresses = new Dictionary<string, int>();
		private Dictionary<string, int> _outputs = new Dictionary<string, int>();

		private int _p1_X = -1;
		private int _p1_Y = -1;
		private int _p1_HP = -1;
		private int _p1_isAttacking = 0;
		private int _p1_wasHitting = 0;
		private int _p1_isHitting = 0;
		private int _p1_cannotControl = 0;

		private int _p2_X = -1;
		private int _p2_Y = -1;
		private int _p2_HP = -1;

		private int _timer = 0;
		private int _winner = 0;
		private int _roundState = 0;

		private void ConnectServer()
		{
			IPAddress ip = IPAddress.Parse(TB_IP.Text);
			int port = Int32.Parse(TB_Port.Text);

			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_endPoint = new IPEndPoint(ip, port);
			_socket.Connect(_endPoint);
		}

		private void ReadFeature()
		{
			foreach(var Pair in _addresses.ToList())
			{
				_outputs[Pair.Key] = this.GetRamValue(Pair.Value);
			}
		}

		private void ReceiveFromServer()
		{
			Array.Clear(_buffer, 0, _bufferSize);

			_bufferSize = _socket.Receive(_buffer);
			string data = Encoding.UTF8.GetString(_buffer, 0, _bufferSize);
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

		private void MakeBuffer()
		{
			Array.Clear(_buffer, 0, _bufferSize);

			Dictionary<string, int> data = new Dictionary<string, int>();
			foreach(var Pair in _outputs)
			{
				data.Add(Pair.Key, Pair.Value);
			}

			string json = JsonConvert.SerializeObject(data);
			_buffer = Encoding.UTF8.GetBytes(json);
		}

		private void SendToServer()
		{
			_socket.Send(_buffer, SocketFlags.None);
		}

		private void PrintBuffer()
		{
			Console.WriteLine(Encoding.UTF8.GetString(_buffer, 0, _bufferSize));
		}

		private void DisconnectServer()
		{
			_socket.Close();
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
				FramesLabel.Text = _frames.ToString();
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
				this.ReadFeature();

				GlobalWin.MainForm.PauseEmulator();

				this.ReceiveFromServer();
				this.MakeBuffer();
				this.SendToServer();
				this.PrintBuffer();

				GlobalWin.MainForm.UnpauseEmulator();

				this.PressButtons();
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
				_dataSize = 1;
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
			StartBot();
		}

		private void StopBtn_Click(object sender, EventArgs e)
		{
			StopBot();
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
		}

		private void SetMaxSpeed()
		{
			GlobalWin.MainForm.Unthrottle();
		}

		private void SetNormalSpeed()
		{
			GlobalWin.MainForm.Throttle();
		}
	}
}
