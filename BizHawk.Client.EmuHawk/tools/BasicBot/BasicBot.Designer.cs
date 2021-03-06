﻿namespace BizHawk.Client.EmuHawk
{
	partial class BasicBot
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicBot));
            this.BotMenu = new MenuStripEx();
            this.FileSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RecentSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MemoryDomainsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.DataSizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._1ByteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._2ByteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._4ByteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BigEndianMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.TurboWhileBottingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.StartFromSlotBox = new System.Windows.Forms.ComboBox();
            this.ControlGroupBox = new System.Windows.Forms.GroupBox();
            this.LoadConfigBtn = new System.Windows.Forms.Button();
            this.StatsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ClearStatsContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GB_Server = new System.Windows.Forms.GroupBox();
            this.TB_Port = new System.Windows.Forms.TextBox();
            this.TB_IP = new System.Windows.Forms.TextBox();
            this.L_Port = new System.Windows.Forms.Label();
            this.L_IP = new System.Windows.Forms.Label();
            this.GB_Information = new System.Windows.Forms.GroupBox();
            this.OutputLstBx = new System.Windows.Forms.ListBox();
            this.OutputLbl = new System.Windows.Forms.Label();
            this.InputLbl = new System.Windows.Forms.Label();
            this.InputLstBx = new System.Windows.Forms.ListBox();
            this.BotMenu.SuspendLayout();
            this.ControlGroupBox.SuspendLayout();
            this.StatsContextMenu.SuspendLayout();
            this.GB_Server.SuspendLayout();
            this.GB_Information.SuspendLayout();
            this.SuspendLayout();
            // 
            // BotMenu
            // 
            this.BotMenu.ClickThrough = true;
            this.BotMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileSubMenu,
            this.OptionsSubMenu});
            this.BotMenu.Location = new System.Drawing.Point(0, 0);
            this.BotMenu.Name = "BotMenu";
            this.BotMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.BotMenu.Size = new System.Drawing.Size(562, 24);
            this.BotMenu.TabIndex = 0;
            this.BotMenu.Text = "menuStrip1";
            // 
            // FileSubMenu
            // 
            this.FileSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMenuItem,
            this.OpenMenuItem,
            this.SaveMenuItem,
            this.SaveAsMenuItem,
            this.RecentSubMenu,
            this.toolStripSeparator1,
            this.ExitMenuItem});
            this.FileSubMenu.Name = "FileSubMenu";
            this.FileSubMenu.Size = new System.Drawing.Size(37, 20);
            this.FileSubMenu.Text = "&File";
            // 
            // NewMenuItem
            // 
            this.NewMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.NewFile;
            this.NewMenuItem.Name = "NewMenuItem";
            this.NewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NewMenuItem.Size = new System.Drawing.Size(199, 22);
            this.NewMenuItem.Text = "&New";
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.OpenFile;
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenMenuItem.Size = new System.Drawing.Size(199, 22);
            this.OpenMenuItem.Text = "&Open...";
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.SaveAs;
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveMenuItem.Size = new System.Drawing.Size(199, 22);
            this.SaveMenuItem.Text = "&Save";
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Name = "SaveAsMenuItem";
            this.SaveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsMenuItem.Size = new System.Drawing.Size(199, 22);
            this.SaveAsMenuItem.Text = "Save &As...";
            // 
            // RecentSubMenu
            // 
            this.RecentSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2});
            this.RecentSubMenu.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Recent;
            this.RecentSubMenu.Name = "RecentSubMenu";
            this.RecentSubMenu.Size = new System.Drawing.Size(199, 22);
            this.RecentSubMenu.Text = "Recent";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(57, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(196, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.ShortcutKeyDisplayString = "Alt+F4";
            this.ExitMenuItem.Size = new System.Drawing.Size(199, 22);
            this.ExitMenuItem.Text = "E&xit";
            // 
            // OptionsSubMenu
            // 
            this.OptionsSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MemoryDomainsMenuItem,
            this.DataSizeMenuItem,
            this.BigEndianMenuItem,
            this.toolStripSeparator4,
            this.TurboWhileBottingMenuItem});
            this.OptionsSubMenu.Name = "OptionsSubMenu";
            this.OptionsSubMenu.Size = new System.Drawing.Size(61, 20);
            this.OptionsSubMenu.Text = "&Options";
            // 
            // MemoryDomainsMenuItem
            // 
            this.MemoryDomainsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3});
            this.MemoryDomainsMenuItem.Name = "MemoryDomainsMenuItem";
            this.MemoryDomainsMenuItem.Size = new System.Drawing.Size(182, 22);
            this.MemoryDomainsMenuItem.Text = "Memory Domains";
            this.MemoryDomainsMenuItem.DropDownOpened += new System.EventHandler(this.MemoryDomainsMenuItem_DropDownOpened);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(57, 6);
            // 
            // DataSizeMenuItem
            // 
            this.DataSizeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._1ByteMenuItem,
            this._2ByteMenuItem,
            this._4ByteMenuItem});
            this.DataSizeMenuItem.Name = "DataSizeMenuItem";
            this.DataSizeMenuItem.Size = new System.Drawing.Size(182, 22);
            this.DataSizeMenuItem.Text = "Data Size";
            this.DataSizeMenuItem.DropDownOpened += new System.EventHandler(this.DataSizeMenuItem_DropDownOpened);
            // 
            // _1ByteMenuItem
            // 
            this._1ByteMenuItem.Name = "_1ByteMenuItem";
            this._1ByteMenuItem.Size = new System.Drawing.Size(113, 22);
            this._1ByteMenuItem.Text = "1 Byte";
            this._1ByteMenuItem.Click += new System.EventHandler(this._1ByteMenuItem_Click);
            // 
            // _2ByteMenuItem
            // 
            this._2ByteMenuItem.Name = "_2ByteMenuItem";
            this._2ByteMenuItem.Size = new System.Drawing.Size(113, 22);
            this._2ByteMenuItem.Text = "2 Bytes";
            this._2ByteMenuItem.Click += new System.EventHandler(this._2ByteMenuItem_Click);
            // 
            // _4ByteMenuItem
            // 
            this._4ByteMenuItem.Name = "_4ByteMenuItem";
            this._4ByteMenuItem.Size = new System.Drawing.Size(113, 22);
            this._4ByteMenuItem.Text = "4 Bytes";
            this._4ByteMenuItem.Click += new System.EventHandler(this._4ByteMenuItem_Click);
            // 
            // BigEndianMenuItem
            // 
            this.BigEndianMenuItem.Name = "BigEndianMenuItem";
            this.BigEndianMenuItem.Size = new System.Drawing.Size(182, 22);
            this.BigEndianMenuItem.Text = "Big Endian";
            this.BigEndianMenuItem.Click += new System.EventHandler(this.BigEndianMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(179, 6);
            // 
            // TurboWhileBottingMenuItem
            // 
            this.TurboWhileBottingMenuItem.Name = "TurboWhileBottingMenuItem";
            this.TurboWhileBottingMenuItem.Size = new System.Drawing.Size(182, 22);
            this.TurboWhileBottingMenuItem.Text = "Turbo While Botting";
            // 
            // RunBtn
            // 
            this.RunBtn.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Play;
            this.RunBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RunBtn.Location = new System.Drawing.Point(10, 68);
            this.RunBtn.Name = "RunBtn";
            this.RunBtn.Size = new System.Drawing.Size(87, 21);
            this.RunBtn.TabIndex = 2001;
            this.RunBtn.Text = "&Run";
            this.RunBtn.UseVisualStyleBackColor = true;
            this.RunBtn.Click += new System.EventHandler(this.RunBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Stop;
            this.StopBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StopBtn.Location = new System.Drawing.Point(103, 68);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(87, 21);
            this.StopBtn.TabIndex = 2002;
            this.StopBtn.Text = "&Stop";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Visible = false;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "Start From:";
            // 
            // StartFromSlotBox
            // 
            this.StartFromSlotBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StartFromSlotBox.FormattingEnabled = true;
            this.StartFromSlotBox.Items.AddRange(new object[] {
            "Slot 0",
            "Slot 1",
            "Slot 2",
            "Slot 3",
            "Slot 4",
            "Slot 5",
            "Slot 6",
            "Slot 7",
            "Slot 8",
            "Slot 9"});
            this.StartFromSlotBox.Location = new System.Drawing.Point(81, 14);
            this.StartFromSlotBox.Name = "StartFromSlotBox";
            this.StartFromSlotBox.Size = new System.Drawing.Size(87, 20);
            this.StartFromSlotBox.TabIndex = 2000;
            // 
            // ControlGroupBox
            // 
            this.ControlGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlGroupBox.Controls.Add(this.LoadConfigBtn);
            this.ControlGroupBox.Controls.Add(this.StopBtn);
            this.ControlGroupBox.Controls.Add(this.RunBtn);
            this.ControlGroupBox.Controls.Add(this.StartFromSlotBox);
            this.ControlGroupBox.Controls.Add(this.label8);
            this.ControlGroupBox.Location = new System.Drawing.Point(12, 27);
            this.ControlGroupBox.Name = "ControlGroupBox";
            this.ControlGroupBox.Size = new System.Drawing.Size(268, 138);
            this.ControlGroupBox.TabIndex = 2004;
            this.ControlGroupBox.TabStop = false;
            this.ControlGroupBox.Text = "Control";
            // 
            // LoadConfigBtn
            // 
            this.LoadConfigBtn.Location = new System.Drawing.Point(10, 39);
            this.LoadConfigBtn.Name = "LoadConfigBtn";
            this.LoadConfigBtn.Size = new System.Drawing.Size(180, 23);
            this.LoadConfigBtn.TabIndex = 2004;
            this.LoadConfigBtn.Text = "Load Config File";
            this.LoadConfigBtn.UseVisualStyleBackColor = true;
            this.LoadConfigBtn.Click += new System.EventHandler(this.LoadConfigBtn_Click);
            // 
            // StatsContextMenu
            // 
            this.StatsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearStatsContextMenuItem});
            this.StatsContextMenu.Name = "StatsContextMenu";
            this.StatsContextMenu.Size = new System.Drawing.Size(102, 26);
            // 
            // ClearStatsContextMenuItem
            // 
            this.ClearStatsContextMenuItem.Name = "ClearStatsContextMenuItem";
            this.ClearStatsContextMenuItem.Size = new System.Drawing.Size(101, 22);
            this.ClearStatsContextMenuItem.Text = "&Clear";
            this.ClearStatsContextMenuItem.Click += new System.EventHandler(this.ClearStatsContextMenuItem_Click);
            // 
            // GB_Server
            // 
            this.GB_Server.Controls.Add(this.TB_Port);
            this.GB_Server.Controls.Add(this.TB_IP);
            this.GB_Server.Controls.Add(this.L_Port);
            this.GB_Server.Controls.Add(this.L_IP);
            this.GB_Server.Location = new System.Drawing.Point(286, 27);
            this.GB_Server.Name = "GB_Server";
            this.GB_Server.Size = new System.Drawing.Size(264, 138);
            this.GB_Server.TabIndex = 2006;
            this.GB_Server.TabStop = false;
            this.GB_Server.Text = "Server";
            // 
            // TB_Port
            // 
            this.TB_Port.Location = new System.Drawing.Point(78, 79);
            this.TB_Port.Name = "TB_Port";
            this.TB_Port.Size = new System.Drawing.Size(155, 21);
            this.TB_Port.TabIndex = 3;
            this.TB_Port.Text = "7000";
            // 
            // TB_IP
            // 
            this.TB_IP.Location = new System.Drawing.Point(78, 39);
            this.TB_IP.Name = "TB_IP";
            this.TB_IP.Size = new System.Drawing.Size(155, 21);
            this.TB_IP.TabIndex = 2;
            this.TB_IP.Text = "127.0.0.1";
            // 
            // L_Port
            // 
            this.L_Port.AutoSize = true;
            this.L_Port.Location = new System.Drawing.Point(24, 86);
            this.L_Port.Name = "L_Port";
            this.L_Port.Size = new System.Drawing.Size(27, 12);
            this.L_Port.TabIndex = 1;
            this.L_Port.Text = "Port";
            // 
            // L_IP
            // 
            this.L_IP.AutoSize = true;
            this.L_IP.Location = new System.Drawing.Point(24, 44);
            this.L_IP.Name = "L_IP";
            this.L_IP.Size = new System.Drawing.Size(16, 12);
            this.L_IP.TabIndex = 0;
            this.L_IP.Text = "IP";
            // 
            // GB_Information
            // 
            this.GB_Information.Controls.Add(this.OutputLstBx);
            this.GB_Information.Controls.Add(this.OutputLbl);
            this.GB_Information.Controls.Add(this.InputLbl);
            this.GB_Information.Controls.Add(this.InputLstBx);
            this.GB_Information.Location = new System.Drawing.Point(12, 171);
            this.GB_Information.Name = "GB_Information";
            this.GB_Information.Size = new System.Drawing.Size(538, 148);
            this.GB_Information.TabIndex = 2007;
            this.GB_Information.TabStop = false;
            this.GB_Information.Text = "Information";
            // 
            // OutputLstBx
            // 
            this.OutputLstBx.FormattingEnabled = true;
            this.OutputLstBx.ItemHeight = 12;
            this.OutputLstBx.Location = new System.Drawing.Point(274, 44);
            this.OutputLstBx.Name = "OutputLstBx";
            this.OutputLstBx.Size = new System.Drawing.Size(244, 88);
            this.OutputLstBx.TabIndex = 3;
            // 
            // OutputLbl
            // 
            this.OutputLbl.AutoSize = true;
            this.OutputLbl.Location = new System.Drawing.Point(272, 21);
            this.OutputLbl.Name = "OutputLbl";
            this.OutputLbl.Size = new System.Drawing.Size(65, 12);
            this.OutputLbl.TabIndex = 2;
            this.OutputLbl.Text = "Output List";
            // 
            // InputLbl
            // 
            this.InputLbl.AutoSize = true;
            this.InputLbl.Location = new System.Drawing.Point(17, 21);
            this.InputLbl.Name = "InputLbl";
            this.InputLbl.Size = new System.Drawing.Size(56, 12);
            this.InputLbl.TabIndex = 1;
            this.InputLbl.Text = "Input List";
            // 
            // InputLstBx
            // 
            this.InputLstBx.FormattingEnabled = true;
            this.InputLstBx.ItemHeight = 12;
            this.InputLstBx.Location = new System.Drawing.Point(15, 44);
            this.InputLstBx.Name = "InputLstBx";
            this.InputLstBx.Size = new System.Drawing.Size(244, 88);
            this.InputLstBx.TabIndex = 0;
            // 
            // BasicBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(562, 331);
            this.Controls.Add(this.GB_Information);
            this.Controls.Add(this.GB_Server);
            this.Controls.Add(this.ControlGroupBox);
            this.Controls.Add(this.BotMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.BotMenu;
            this.Name = "BasicBot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Basic Bot";
            this.Load += new System.EventHandler(this.BasicBot_Load);
            this.BotMenu.ResumeLayout(false);
            this.BotMenu.PerformLayout();
            this.ControlGroupBox.ResumeLayout(false);
            this.ControlGroupBox.PerformLayout();
            this.StatsContextMenu.ResumeLayout(false);
            this.GB_Server.ResumeLayout(false);
            this.GB_Server.PerformLayout();
            this.GB_Information.ResumeLayout(false);
            this.GB_Information.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private MenuStripEx BotMenu;
		private System.Windows.Forms.ToolStripMenuItem FileSubMenu;
		private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
		private System.Windows.Forms.Button RunBtn;
		private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RecentSubMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem OptionsSubMenu;
		private System.Windows.Forms.Button StopBtn;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox StartFromSlotBox;
		private System.Windows.Forms.ToolStripMenuItem SaveAsMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem NewMenuItem;
		private System.Windows.Forms.GroupBox ControlGroupBox;
		private System.Windows.Forms.ToolStripMenuItem TurboWhileBottingMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MemoryDomainsMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ContextMenuStrip StatsContextMenu;
		private System.Windows.Forms.ToolStripMenuItem ClearStatsContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem BigEndianMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem DataSizeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _1ByteMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _2ByteMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _4ByteMenuItem;
		private System.Windows.Forms.GroupBox GB_Server;
		private System.Windows.Forms.Label L_Port;
		private System.Windows.Forms.Label L_IP;
		private System.Windows.Forms.TextBox TB_Port;
		private System.Windows.Forms.TextBox TB_IP;
		private System.Windows.Forms.GroupBox GB_Information;
		private System.Windows.Forms.ListBox InputLstBx;
		private System.Windows.Forms.ListBox OutputLstBx;
		private System.Windows.Forms.Label OutputLbl;
		private System.Windows.Forms.Label InputLbl;
		private System.Windows.Forms.Button LoadConfigBtn;
	}
}
