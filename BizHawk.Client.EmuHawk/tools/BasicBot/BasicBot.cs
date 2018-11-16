using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk.ToolExtensions;

using BizHawk.Emulation.Common;
using BizHawk.Client.Common;

using System.Net;
using System.Net.Sockets;

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
		private bool _dontUpdateValues = false;
		private MemoryDomain _currentDomain;
		private bool _isBigEndian;
		private int _dataSize;

		private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private IPEndPoint _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);

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

			this.ConnectServer();
		}

		~BasicBot()
		{
			this.DisconnectServer();
		}

		private void BasicBot_Load(object sender, EventArgs e)
		{
			;
		}

		#endregion

		#region Network
		private byte[] _buffer = new byte[1];
		private int _bufferSize = 0;

		private void ConnectServer()
		{
			_socket.Connect(_endPoint);
		}

		private void SendToServer()
		{
			_socket.Send(_buffer, SocketFlags.None);
		}

		private void ReceiveFromServer()
		{
			_bufferSize = _socket.Receive(_buffer);
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
				if (_Emulator.Frame >= _frames)
				{
					_frames += 60;

					this.ReadMemory();

					this.SendToServer();
					this.ReceiveFromServer();
					this.PrintBuffer();
				}

				this.PressButtons();
			}
		}

		private void ReadMemory()
		{
			Array.Clear(_buffer, 0, _bufferSize);

			string msg = string.Empty;

			msg += _currentDomain.PeekUshort(0x000CB4, _isBigEndian);
			msg += ".";
			msg += _currentDomain.PeekUshort(0x000CB6, _isBigEndian);

			_buffer = Encoding.UTF8.GetBytes(msg);
		}

		private void PressButtons()
		{
			;
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

		private int GetRamvalue(int addr)
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

			_dontUpdateValues = true;
			GlobalWin.MainForm.LoadQuickSave(SelectedSlot, false, true);
			_dontUpdateValues = false;

			_targetFrame = _Emulator.Frame + 0;

			GlobalWin.MainForm.UnpauseEmulator();
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
