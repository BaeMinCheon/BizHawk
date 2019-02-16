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
		}

		private void BasicBot_Load(object sender, EventArgs e)
		{
			;
		}

		#endregion

		#region Network
		private Socket _socket;
		private IPEndPoint _endPoint;
		private byte[] _buffer = new byte[1];
		private int _bufferSize = 0;
		private int _keyMove = 0;
		private int _keyControl = 0;
		private string[] _mapMove = { string.Empty, "P1 Left", "P1 Up", "P1 Right", "P1 Down" };
		private string[] _mapControl = { string.Empty, "P1 X", "P1 Y", "P1 A", "P1 B" };

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
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);
			_socket.Connect(_endPoint);
		}

		private void ReadFeature()
		{
			_p1_X = _currentDomain.PeekUshort(0x000022, _isBigEndian);
			_p1_Y = 192 - _currentDomain.PeekUshort(0x000C0A, _isBigEndian);
			_p1_HP = _currentDomain.PeekUshort(0x000D12, _isBigEndian);
			if (_currentDomain.PeekUshort(0x000CB2, _isBigEndian) >= 256)
			{
				_p1_isAttacking = 1;
			}
			else
			{
				_p1_isAttacking = 0;
			}
			if((_p1_wasHitting == 1) && (_p1_Y > 0))
			{
				_p1_isHitting = 1;
			}
			else if(_currentDomain.PeekUshort(0x000C58, _isBigEndian) >= 256)
			{
				_p1_wasHitting = 1;
				_p1_isHitting = 1;
			}
			else
			{
				_p1_wasHitting = 0;
				_p1_isHitting = 0;
			}
			_p1_cannotControl = ((_p1_isAttacking + _p1_isHitting) > 0) ? 1 : 0;

			_p2_X = _currentDomain.PeekUshort(0x000026, _isBigEndian);
			_p2_Y = 192 - _currentDomain.PeekUshort(0x000E0A, _isBigEndian);
			_p2_HP = _currentDomain.PeekUshort(0x000F12, _isBigEndian);

			_timer = _currentDomain.PeekUshort(0x001AC8, _isBigEndian);
			_winner = _currentDomain.PeekUshort(0x001ACE, _isBigEndian);
			switch(_roundState)
			{
				// before round
				case 0:
					if (_timer == 152)
					{
						_roundState = 1;
					}
					else
					{
						_timer = 0;
					}
					break;

				// in round
				case 1:
					if (_winner >= 256)
					{
						_roundState = 0;
					}
					break;
			}
		}

		private void MakeBuffer()
		{
			Array.Clear(_buffer, 0, _bufferSize);

			int p1_isLeft = (_p1_X > _p2_X) ? 1 : 0;
			int gap_x = Math.Abs(_p1_X - _p2_X);
			int gap_y = Math.Abs(_p1_Y - _p2_Y);
			int gap_hp_for_p1 = _p1_HP - _p2_HP;
			int p1_canInputMove = (_p1_cannotControl > 0) ? 0 : ((_p1_Y > 0) ? 0 : 1);
			int p1_canInputAction = (_p1_cannotControl > 0) ? 0 : 1;

			string msg = string.Empty;

			msg += p1_isLeft;
			msg += ".";
			msg += gap_x;
			msg += ".";
			msg += gap_y;
			msg += ".";
			msg += gap_hp_for_p1;
			msg += ".";
			msg += p1_canInputMove;
			msg += ".";
			msg += p1_canInputAction;

			_buffer = Encoding.UTF8.GetBytes(msg);
		}

		private void SendToServer()
		{
			_socket.Send(_buffer, SocketFlags.None);
		}

		private void ReceiveFromServer()
		{
			_bufferSize = _socket.Receive(_buffer);
			string[] keys = Encoding.UTF8.GetString(_buffer, 0, _bufferSize).Split('.');
			_keyMove = int.Parse(keys[0]);
			_keyControl = int.Parse(keys[1]);
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

				if(_timer == 0)
				{
					if (_frames > 100)
					{
						Global.LuaAndAdaptor.SetButton("P1 Start", true);
						_frames = 0;
					}
					else
					{
						Global.LuaAndAdaptor.SetButton("P1 Start", false);
					}
					++_frames;
				}
				else
				{
					if (_p1_cannotControl == 0)
					{
						GlobalWin.MainForm.PauseEmulator();

						this.MakeBuffer();
						this.ReceiveFromServer();
						this.SendToServer();
						this.PrintBuffer();

						GlobalWin.MainForm.UnpauseEmulator();
					}
					this.PressButtons();
				}
			}
		}

		private bool IsCanInput()
		{
			return true;
		}

		private void PressButtons()
		{
			if(_keyMove > 0)
			{
				Global.LuaAndAdaptor.SetButton(_mapMove[_keyMove], true);
			}
			if(_keyControl > 0)
			{
				Global.LuaAndAdaptor.SetButton(_mapControl[_keyControl], true);
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
