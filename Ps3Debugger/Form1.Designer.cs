namespace Ps3Debugger
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.TabControl = new System.Windows.Forms.TabControl();
            this.Ps3DebuggerTab = new System.Windows.Forms.TabPage();
            this.asmBox = new System.Windows.Forms.ListBox();
            this.Ps3Discon = new System.Windows.Forms.Button();
            this.Attach = new System.Windows.Forms.Button();
            this.asciiText = new System.Windows.Forms.RichTextBox();
            this.offsetsText = new System.Windows.Forms.RichTextBox();
            this.hexCode = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.JumpUp = new System.Windows.Forms.Button();
            this.JumpDown = new System.Windows.Forms.Button();
            this.lblms = new System.Windows.Forms.Label();
            this.startAutoDebug = new System.Windows.Forms.CheckBox();
            this.autoUpdateTime = new System.Windows.Forms.TextBox();
            this.startDebug = new System.Windows.Forms.Button();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.AutoUpdateset = new System.Windows.Forms.CheckBox();
            this.plusHundred = new System.Windows.Forms.Button();
            this.AutoUpdateget = new System.Windows.Forms.CheckBox();
            this.minusHundred = new System.Windows.Forms.Button();
            this.Options = new System.Windows.Forms.Button();
            this.jumpUint = new System.Windows.Forms.TextBox();
            this.lblJump = new System.Windows.Forms.Label();
            this.getOffset = new System.Windows.Forms.Button();
            this.lblBytes = new System.Windows.Forms.Label();
            this.setOffset = new System.Windows.Forms.Button();
            this.bytesLegthText = new System.Windows.Forms.TextBox();
            this.connectedLbl = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.dexHexCombo = new System.Windows.Forms.ComboBox();
            this.startHex = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.resultTxt = new System.Windows.Forms.TextBox();
            this.comboByteLength = new System.Windows.Forms.ComboBox();
            this.lblDebugging = new System.Windows.Forms.Label();
            this.offsetTxt = new System.Windows.Forms.TextBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.lblOffset = new System.Windows.Forms.Label();
            this.SearcherTab = new System.Windows.Forms.TabPage();
            this.TabCon = new System.Windows.Forms.TabControl();
            this.CodesTab = new System.Windows.Forms.TabPage();
            this.cbCodes = new System.Windows.Forms.RichTextBox();
            this.cbSaveAll = new System.Windows.Forms.Button();
            this.cbSaveAs = new System.Windows.Forms.Button();
            this.cbImport = new System.Windows.Forms.Button();
            this.cbSave = new System.Windows.Forms.Button();
            this.cbRemove = new System.Windows.Forms.Button();
            this.cbAdd = new System.Windows.Forms.Button();
            this.cbWrite = new System.Windows.Forms.Button();
            this.ConstantWrite = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.TextBox();
            this.cbList = new System.Windows.Forms.ListView();
            this.ColumnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SearchTab = new System.Windows.Forms.TabPage();
            this.rangesBox = new System.Windows.Forms.CheckBox();
            this.Play = new System.Windows.Forms.Button();
            this.Pause = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.saveSRes = new System.Windows.Forms.Button();
            this.loadSRes = new System.Windows.Forms.Button();
            this.schVal2 = new System.Windows.Forms.TextBox();
            this.DumpMem = new System.Windows.Forms.Button();
            this.compBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SchPWS = new System.Windows.Forms.CheckBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.schProg = new System.Windows.Forms.ProgressBar();
            this.SchHexCheck = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSchAlign = new System.Windows.Forms.ComboBox();
            this.SchRef = new System.Windows.Forms.Button();
            this.lvSch = new System.Windows.Forms.ListView();
            this.lvSchAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSchValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSchDec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSchAlign = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.schNSearch = new System.Windows.Forms.Button();
            this.schVal = new System.Windows.Forms.TextBox();
            this.schRange2 = new System.Windows.Forms.TextBox();
            this.schRange1 = new System.Windows.Forms.TextBox();
            this.schSearch = new System.Windows.Forms.Button();
            this.RangesTab = new System.Windows.Forms.TabPage();
            this.findRangeProgBar = new System.Windows.Forms.ProgressBar();
            this.findRanges = new System.Windows.Forms.Button();
            this.recRangeBox = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Label4 = new System.Windows.Forms.Label();
            this.RangeDown = new System.Windows.Forms.Button();
            this.RangeUp = new System.Windows.Forms.Button();
            this.RemoveRange = new System.Windows.Forms.Button();
            this.AddRange = new System.Windows.Forms.Button();
            this.SaveRange = new System.Windows.Forms.Button();
            this.ImportRange = new System.Windows.Forms.Button();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.rangeView = new System.Windows.Forms.ListView();
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PluginsTab = new System.Windows.Forms.TabPage();
            this.plugIcon = new System.Windows.Forms.PictureBox();
            this.descPlugDesc = new System.Windows.Forms.Label();
            this.descPlugVer = new System.Windows.Forms.Label();
            this.descPlugAuth = new System.Windows.Forms.Label();
            this.refPlugin = new System.Windows.Forms.Button();
            this.descPlugName = new System.Windows.Forms.Label();
            this.pluginList = new System.Windows.Forms.ListBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.autoUpdateSetTimer = new System.Windows.Forms.Timer(this.components);
            this.autoUpdateGetTimer = new System.Windows.Forms.Timer(this.components);
            this.AutoDebug = new System.Windows.Forms.Timer(this.components);
            this.DumpWorker = new System.ComponentModel.BackgroundWorker();
            this.RangeWorker = new System.ComponentModel.BackgroundWorker();
            this.Screentimer = new System.Windows.Forms.Timer(this.components);
            this.TabControl.SuspendLayout();
            this.Ps3DebuggerTab.SuspendLayout();
            this.SearcherTab.SuspendLayout();
            this.TabCon.SuspendLayout();
            this.CodesTab.SuspendLayout();
            this.SearchTab.SuspendLayout();
            this.RangesTab.SuspendLayout();
            this.PluginsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.Ps3DebuggerTab);
            this.TabControl.Controls.Add(this.SearcherTab);
            this.TabControl.Location = new System.Drawing.Point(1, 1);
            this.TabControl.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1471, 752);
            this.TabControl.TabIndex = 0;
            // 
            // Ps3DebuggerTab
            // 
            this.Ps3DebuggerTab.BackColor = System.Drawing.Color.Black;
            this.Ps3DebuggerTab.Controls.Add(this.asmBox);
            this.Ps3DebuggerTab.Controls.Add(this.Ps3Discon);
            this.Ps3DebuggerTab.Controls.Add(this.Attach);
            this.Ps3DebuggerTab.Controls.Add(this.asciiText);
            this.Ps3DebuggerTab.Controls.Add(this.offsetsText);
            this.Ps3DebuggerTab.Controls.Add(this.hexCode);
            this.Ps3DebuggerTab.Controls.Add(this.label12);
            this.Ps3DebuggerTab.Controls.Add(this.label10);
            this.Ps3DebuggerTab.Controls.Add(this.JumpUp);
            this.Ps3DebuggerTab.Controls.Add(this.JumpDown);
            this.Ps3DebuggerTab.Controls.Add(this.lblms);
            this.Ps3DebuggerTab.Controls.Add(this.startAutoDebug);
            this.Ps3DebuggerTab.Controls.Add(this.autoUpdateTime);
            this.Ps3DebuggerTab.Controls.Add(this.startDebug);
            this.Ps3DebuggerTab.Controls.Add(this.lblUpdate);
            this.Ps3DebuggerTab.Controls.Add(this.AutoUpdateset);
            this.Ps3DebuggerTab.Controls.Add(this.plusHundred);
            this.Ps3DebuggerTab.Controls.Add(this.AutoUpdateget);
            this.Ps3DebuggerTab.Controls.Add(this.minusHundred);
            this.Ps3DebuggerTab.Controls.Add(this.Options);
            this.Ps3DebuggerTab.Controls.Add(this.jumpUint);
            this.Ps3DebuggerTab.Controls.Add(this.lblJump);
            this.Ps3DebuggerTab.Controls.Add(this.getOffset);
            this.Ps3DebuggerTab.Controls.Add(this.lblBytes);
            this.Ps3DebuggerTab.Controls.Add(this.setOffset);
            this.Ps3DebuggerTab.Controls.Add(this.bytesLegthText);
            this.Ps3DebuggerTab.Controls.Add(this.connectedLbl);
            this.Ps3DebuggerTab.Controls.Add(this.lblLength);
            this.Ps3DebuggerTab.Controls.Add(this.dexHexCombo);
            this.Ps3DebuggerTab.Controls.Add(this.startHex);
            this.Ps3DebuggerTab.Controls.Add(this.lblAddress);
            this.Ps3DebuggerTab.Controls.Add(this.resultTxt);
            this.Ps3DebuggerTab.Controls.Add(this.comboByteLength);
            this.Ps3DebuggerTab.Controls.Add(this.lblDebugging);
            this.Ps3DebuggerTab.Controls.Add(this.offsetTxt);
            this.Ps3DebuggerTab.Controls.Add(this.btnConnection);
            this.Ps3DebuggerTab.Controls.Add(this.lblOffset);
            this.Ps3DebuggerTab.ForeColor = System.Drawing.Color.White;
            this.Ps3DebuggerTab.Location = new System.Drawing.Point(4, 22);
            this.Ps3DebuggerTab.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Ps3DebuggerTab.Name = "Ps3DebuggerTab";
            this.Ps3DebuggerTab.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Ps3DebuggerTab.Size = new System.Drawing.Size(1463, 726);
            this.Ps3DebuggerTab.TabIndex = 0;
            this.Ps3DebuggerTab.Text = "Ps3Debugger";
            // 
            // asmBox
            // 
            this.asmBox.BackColor = System.Drawing.Color.White;
            this.asmBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asmBox.FormattingEnabled = true;
            this.asmBox.ItemHeight = 16;
            this.asmBox.Items.AddRange(new object[] {
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop ",
            "nop ",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop ",
            "nop ",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop ",
            "nop ",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop ",
            "nop ",
            "nop ",
            "nop ",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop"});
            this.asmBox.Location = new System.Drawing.Point(1278, 0);
            this.asmBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.asmBox.Name = "asmBox";
            this.asmBox.Size = new System.Drawing.Size(188, 724);
            this.asmBox.TabIndex = 8;
            this.asmBox.Tag = "ASM";
            // 
            // Ps3Discon
            // 
            this.Ps3Discon.BackColor = System.Drawing.Color.Black;
            this.Ps3Discon.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.Ps3Discon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Ps3Discon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ps3Discon.ForeColor = System.Drawing.Color.White;
            this.Ps3Discon.Location = new System.Drawing.Point(18, 130);
            this.Ps3Discon.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Ps3Discon.Name = "Ps3Discon";
            this.Ps3Discon.Size = new System.Drawing.Size(155, 23);
            this.Ps3Discon.TabIndex = 22;
            this.Ps3Discon.Text = "Disconnect";
            this.Ps3Discon.UseVisualStyleBackColor = false;
            this.Ps3Discon.Click += new System.EventHandler(this.Ps3Discon_Click);
            // 
            // Attach
            // 
            this.Attach.BackColor = System.Drawing.Color.Black;
            this.Attach.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.Attach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Attach.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Attach.ForeColor = System.Drawing.Color.White;
            this.Attach.Location = new System.Drawing.Point(18, 98);
            this.Attach.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Attach.Name = "Attach";
            this.Attach.Size = new System.Drawing.Size(155, 27);
            this.Attach.TabIndex = 21;
            this.Attach.Text = "Attach";
            this.Attach.UseVisualStyleBackColor = false;
            this.Attach.Click += new System.EventHandler(this.Attach_Click);
            // 
            // asciiText
            // 
            this.asciiText.AcceptsTab = true;
            this.asciiText.AutoWordSelection = true;
            this.asciiText.BackColor = System.Drawing.Color.White;
            this.asciiText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.asciiText.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asciiText.ForeColor = System.Drawing.Color.Black;
            this.asciiText.Location = new System.Drawing.Point(962, 0);
            this.asciiText.Name = "asciiText";
            this.asciiText.ReadOnly = true;
            this.asciiText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.asciiText.Size = new System.Drawing.Size(313, 724);
            this.asciiText.TabIndex = 0;
            this.asciiText.Text = resources.GetString("asciiText.Text");
            this.asciiText.VScroll += new System.EventHandler(this.asciiText_VScroll);
            this.asciiText.TextChanged += new System.EventHandler(this.asciiText_TextChanged);
            // 
            // offsetsText
            // 
            this.offsetsText.BackColor = System.Drawing.Color.White;
            this.offsetsText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.offsetsText.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offsetsText.ForeColor = System.Drawing.Color.Black;
            this.offsetsText.Location = new System.Drawing.Point(225, 0);
            this.offsetsText.Name = "offsetsText";
            this.offsetsText.ReadOnly = true;
            this.offsetsText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.offsetsText.Size = new System.Drawing.Size(92, 724);
            this.offsetsText.TabIndex = 1;
            this.offsetsText.Text = resources.GetString("offsetsText.Text");
            this.offsetsText.TextChanged += new System.EventHandler(this.offsetsText_TextChanged);
            // 
            // hexCode
            // 
            this.hexCode.BackColor = System.Drawing.Color.White;
            this.hexCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hexCode.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexCode.ForeColor = System.Drawing.Color.Black;
            this.hexCode.Location = new System.Drawing.Point(319, 0);
            this.hexCode.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.hexCode.Name = "hexCode";
            this.hexCode.ReadOnly = true;
            this.hexCode.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.hexCode.Size = new System.Drawing.Size(642, 724);
            this.hexCode.TabIndex = 8;
            this.hexCode.Tag = "HEX";
            this.hexCode.Text = resources.GetString("hexCode.Text");
            this.hexCode.Click += new System.EventHandler(this.hexCode_Text_click);
            this.hexCode.TextChanged += new System.EventHandler(this.hexCode_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Black;
            this.label12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(41, 249);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 15);
            this.label12.TabIndex = 20;
            this.label12.Text = "Bytes";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(41, 195);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 15);
            this.label10.TabIndex = 18;
            this.label10.Text = "Address";
            // 
            // JumpUp
            // 
            this.JumpUp.BackColor = System.Drawing.Color.Black;
            this.JumpUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.JumpUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.JumpUp.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JumpUp.ForeColor = System.Drawing.Color.White;
            this.JumpUp.Location = new System.Drawing.Point(203, 8);
            this.JumpUp.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.JumpUp.Name = "JumpUp";
            this.JumpUp.Size = new System.Drawing.Size(17, 26);
            this.JumpUp.TabIndex = 17;
            this.JumpUp.Text = "+";
            this.JumpUp.UseVisualStyleBackColor = false;
            this.JumpUp.Click += new System.EventHandler(this.JumpUp_Click);
            // 
            // JumpDown
            // 
            this.JumpDown.BackColor = System.Drawing.Color.Black;
            this.JumpDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.JumpDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.JumpDown.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JumpDown.ForeColor = System.Drawing.Color.White;
            this.JumpDown.Location = new System.Drawing.Point(203, 42);
            this.JumpDown.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.JumpDown.Name = "JumpDown";
            this.JumpDown.Size = new System.Drawing.Size(17, 26);
            this.JumpDown.TabIndex = 16;
            this.JumpDown.Text = "-";
            this.JumpDown.UseVisualStyleBackColor = false;
            this.JumpDown.Click += new System.EventHandler(this.JumpDown_Click);
            // 
            // lblms
            // 
            this.lblms.AutoSize = true;
            this.lblms.BackColor = System.Drawing.Color.Black;
            this.lblms.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblms.ForeColor = System.Drawing.Color.White;
            this.lblms.Location = new System.Drawing.Point(127, 579);
            this.lblms.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblms.Name = "lblms";
            this.lblms.Size = new System.Drawing.Size(73, 15);
            this.lblms.TabIndex = 15;
            this.lblms.Text = "Miliseconds";
            this.lblms.Visible = false;
            // 
            // startAutoDebug
            // 
            this.startAutoDebug.AutoSize = true;
            this.startAutoDebug.BackColor = System.Drawing.Color.Black;
            this.startAutoDebug.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startAutoDebug.ForeColor = System.Drawing.Color.White;
            this.startAutoDebug.Location = new System.Drawing.Point(44, 670);
            this.startAutoDebug.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.startAutoDebug.Name = "startAutoDebug";
            this.startAutoDebug.Size = new System.Drawing.Size(94, 19);
            this.startAutoDebug.TabIndex = 0;
            this.startAutoDebug.Text = "Auto update";
            this.startAutoDebug.UseVisualStyleBackColor = false;
            this.startAutoDebug.CheckedChanged += new System.EventHandler(this.startAutoDebug_CheckedChanged);
            // 
            // autoUpdateTime
            // 
            this.autoUpdateTime.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.autoUpdateTime.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoUpdateTime.ForeColor = System.Drawing.Color.Chartreuse;
            this.autoUpdateTime.Location = new System.Drawing.Point(67, 575);
            this.autoUpdateTime.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.autoUpdateTime.Name = "autoUpdateTime";
            this.autoUpdateTime.Size = new System.Drawing.Size(49, 22);
            this.autoUpdateTime.TabIndex = 14;
            this.autoUpdateTime.Text = "1000";
            this.autoUpdateTime.Visible = false;
            // 
            // startDebug
            // 
            this.startDebug.BackColor = System.Drawing.Color.Black;
            this.startDebug.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.startDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startDebug.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDebug.ForeColor = System.Drawing.Color.White;
            this.startDebug.Location = new System.Drawing.Point(10, 624);
            this.startDebug.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.startDebug.Name = "startDebug";
            this.startDebug.Size = new System.Drawing.Size(210, 29);
            this.startDebug.TabIndex = 6;
            this.startDebug.Text = "Load / Refresh";
            this.startDebug.UseVisualStyleBackColor = false;
            this.startDebug.Click += new System.EventHandler(this.startDebug_Click);
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.BackColor = System.Drawing.Color.Black;
            this.lblUpdate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdate.ForeColor = System.Drawing.Color.White;
            this.lblUpdate.Location = new System.Drawing.Point(16, 579);
            this.lblUpdate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(47, 15);
            this.lblUpdate.TabIndex = 13;
            this.lblUpdate.Text = "Update";
            this.lblUpdate.Visible = false;
            // 
            // AutoUpdateset
            // 
            this.AutoUpdateset.AutoSize = true;
            this.AutoUpdateset.BackColor = System.Drawing.Color.Black;
            this.AutoUpdateset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoUpdateset.ForeColor = System.Drawing.Color.White;
            this.AutoUpdateset.Location = new System.Drawing.Point(35, 357);
            this.AutoUpdateset.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.AutoUpdateset.Name = "AutoUpdateset";
            this.AutoUpdateset.Size = new System.Drawing.Size(115, 19);
            this.AutoUpdateset.TabIndex = 7;
            this.AutoUpdateset.Text = "Auto update set";
            this.AutoUpdateset.UseVisualStyleBackColor = false;
            this.AutoUpdateset.CheckedChanged += new System.EventHandler(this.AutoUpdateset_CheckedChanged);
            // 
            // plusHundred
            // 
            this.plusHundred.BackColor = System.Drawing.Color.Black;
            this.plusHundred.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.plusHundred.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusHundred.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusHundred.ForeColor = System.Drawing.Color.White;
            this.plusHundred.Location = new System.Drawing.Point(166, 535);
            this.plusHundred.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.plusHundred.Name = "plusHundred";
            this.plusHundred.Size = new System.Drawing.Size(17, 26);
            this.plusHundred.TabIndex = 12;
            this.plusHundred.Text = "+";
            this.plusHundred.UseVisualStyleBackColor = false;
            this.plusHundred.Click += new System.EventHandler(this.plusHundred_Click);
            // 
            // AutoUpdateget
            // 
            this.AutoUpdateget.AutoSize = true;
            this.AutoUpdateget.BackColor = System.Drawing.Color.Black;
            this.AutoUpdateget.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoUpdateget.ForeColor = System.Drawing.Color.White;
            this.AutoUpdateget.Location = new System.Drawing.Point(35, 334);
            this.AutoUpdateget.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.AutoUpdateget.Name = "AutoUpdateget";
            this.AutoUpdateget.Size = new System.Drawing.Size(115, 19);
            this.AutoUpdateget.TabIndex = 6;
            this.AutoUpdateget.Text = "Auto update get";
            this.AutoUpdateget.UseVisualStyleBackColor = false;
            this.AutoUpdateget.CheckedChanged += new System.EventHandler(this.AutoUpdateget_CheckedChanged);
            // 
            // minusHundred
            // 
            this.minusHundred.BackColor = System.Drawing.Color.Black;
            this.minusHundred.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.minusHundred.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusHundred.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minusHundred.ForeColor = System.Drawing.Color.White;
            this.minusHundred.Location = new System.Drawing.Point(145, 535);
            this.minusHundred.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.minusHundred.Name = "minusHundred";
            this.minusHundred.Size = new System.Drawing.Size(17, 26);
            this.minusHundred.TabIndex = 6;
            this.minusHundred.Text = "-";
            this.minusHundred.UseVisualStyleBackColor = false;
            this.minusHundred.Click += new System.EventHandler(this.minusHundred_Click);
            // 
            // Options
            // 
            this.Options.BackColor = System.Drawing.Color.Black;
            this.Options.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.Options.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Options.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Options.ForeColor = System.Drawing.Color.White;
            this.Options.Location = new System.Drawing.Point(63, 32);
            this.Options.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(69, 23);
            this.Options.TabIndex = 15;
            this.Options.Text = "Options";
            this.Options.UseVisualStyleBackColor = false;
            this.Options.Click += new System.EventHandler(this.Options_Click);
            // 
            // jumpUint
            // 
            this.jumpUint.BackColor = System.Drawing.Color.Black;
            this.jumpUint.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumpUint.ForeColor = System.Drawing.Color.Chartreuse;
            this.jumpUint.Location = new System.Drawing.Point(67, 539);
            this.jumpUint.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.jumpUint.Name = "jumpUint";
            this.jumpUint.Size = new System.Drawing.Size(65, 22);
            this.jumpUint.TabIndex = 11;
            this.jumpUint.Text = "0x20";
            this.jumpUint.TextChanged += new System.EventHandler(this.jumpUint_TextChanged);
            // 
            // lblJump
            // 
            this.lblJump.AutoSize = true;
            this.lblJump.BackColor = System.Drawing.Color.Black;
            this.lblJump.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJump.ForeColor = System.Drawing.Color.White;
            this.lblJump.Location = new System.Drawing.Point(24, 542);
            this.lblJump.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblJump.Name = "lblJump";
            this.lblJump.Size = new System.Drawing.Size(39, 15);
            this.lblJump.TabIndex = 10;
            this.lblJump.Text = "Jump";
            // 
            // getOffset
            // 
            this.getOffset.BackColor = System.Drawing.Color.Black;
            this.getOffset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.getOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getOffset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getOffset.ForeColor = System.Drawing.Color.White;
            this.getOffset.Location = new System.Drawing.Point(49, 298);
            this.getOffset.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.getOffset.Name = "getOffset";
            this.getOffset.Size = new System.Drawing.Size(50, 28);
            this.getOffset.TabIndex = 5;
            this.getOffset.Text = "Get";
            this.getOffset.UseVisualStyleBackColor = false;
            this.getOffset.Click += new System.EventHandler(this.getOffset_Click);
            // 
            // lblBytes
            // 
            this.lblBytes.AutoSize = true;
            this.lblBytes.BackColor = System.Drawing.Color.Black;
            this.lblBytes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBytes.ForeColor = System.Drawing.Color.White;
            this.lblBytes.Location = new System.Drawing.Point(144, 505);
            this.lblBytes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBytes.Name = "lblBytes";
            this.lblBytes.Size = new System.Drawing.Size(37, 15);
            this.lblBytes.TabIndex = 9;
            this.lblBytes.Text = "Bytes";
            // 
            // setOffset
            // 
            this.setOffset.BackColor = System.Drawing.Color.Black;
            this.setOffset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.setOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setOffset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setOffset.ForeColor = System.Drawing.Color.White;
            this.setOffset.Location = new System.Drawing.Point(130, 298);
            this.setOffset.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.setOffset.Name = "setOffset";
            this.setOffset.Size = new System.Drawing.Size(47, 28);
            this.setOffset.TabIndex = 4;
            this.setOffset.Text = "Set";
            this.setOffset.UseVisualStyleBackColor = false;
            this.setOffset.Click += new System.EventHandler(this.setOffset_Click);
            // 
            // bytesLegthText
            // 
            this.bytesLegthText.BackColor = System.Drawing.Color.Black;
            this.bytesLegthText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bytesLegthText.ForeColor = System.Drawing.Color.Chartreuse;
            this.bytesLegthText.Location = new System.Drawing.Point(67, 501);
            this.bytesLegthText.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.bytesLegthText.Name = "bytesLegthText";
            this.bytesLegthText.Size = new System.Drawing.Size(65, 22);
            this.bytesLegthText.TabIndex = 8;
            this.bytesLegthText.Text = "640";
            // 
            // connectedLbl
            // 
            this.connectedLbl.AutoSize = true;
            this.connectedLbl.BackColor = System.Drawing.Color.Black;
            this.connectedLbl.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectedLbl.ForeColor = System.Drawing.Color.DodgerBlue;
            this.connectedLbl.Location = new System.Drawing.Point(9, 4);
            this.connectedLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.connectedLbl.Name = "connectedLbl";
            this.connectedLbl.Size = new System.Drawing.Size(116, 24);
            this.connectedLbl.TabIndex = 0;
            this.connectedLbl.Text = "Connection";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.BackColor = System.Drawing.Color.Black;
            this.lblLength.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLength.ForeColor = System.Drawing.Color.White;
            this.lblLength.Location = new System.Drawing.Point(17, 505);
            this.lblLength.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(46, 15);
            this.lblLength.TabIndex = 7;
            this.lblLength.Text = "Length";
            // 
            // dexHexCombo
            // 
            this.dexHexCombo.BackColor = System.Drawing.Color.Black;
            this.dexHexCombo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dexHexCombo.ForeColor = System.Drawing.Color.White;
            this.dexHexCombo.FormattingEnabled = true;
            this.dexHexCombo.Items.AddRange(new object[] {
            "dec",
            "hex"});
            this.dexHexCombo.Location = new System.Drawing.Point(131, 266);
            this.dexHexCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.dexHexCombo.Name = "dexHexCombo";
            this.dexHexCombo.Size = new System.Drawing.Size(89, 24);
            this.dexHexCombo.TabIndex = 3;
            // 
            // startHex
            // 
            this.startHex.BackColor = System.Drawing.Color.Black;
            this.startHex.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startHex.ForeColor = System.Drawing.Color.Chartreuse;
            this.startHex.Location = new System.Drawing.Point(69, 464);
            this.startHex.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.startHex.Name = "startHex";
            this.startHex.Size = new System.Drawing.Size(96, 22);
            this.startHex.TabIndex = 6;
            this.startHex.Text = "0x00010000";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.BackColor = System.Drawing.Color.Black;
            this.lblAddress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.ForeColor = System.Drawing.Color.White;
            this.lblAddress.Location = new System.Drawing.Point(10, 464);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(55, 15);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "Address";
            // 
            // resultTxt
            // 
            this.resultTxt.BackColor = System.Drawing.Color.Black;
            this.resultTxt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultTxt.ForeColor = System.Drawing.Color.Chartreuse;
            this.resultTxt.Location = new System.Drawing.Point(36, 268);
            this.resultTxt.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.resultTxt.Name = "resultTxt";
            this.resultTxt.Size = new System.Drawing.Size(83, 22);
            this.resultTxt.TabIndex = 2;
            this.resultTxt.Text = "0x00010000";
            // 
            // comboByteLength
            // 
            this.comboByteLength.BackColor = System.Drawing.Color.Black;
            this.comboByteLength.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboByteLength.ForeColor = System.Drawing.Color.White;
            this.comboByteLength.FormattingEnabled = true;
            this.comboByteLength.Items.AddRange(new object[] {
            "1 byte",
            "2 bytes",
            "4 bytes",
            "string"});
            this.comboByteLength.Location = new System.Drawing.Point(130, 214);
            this.comboByteLength.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.comboByteLength.Name = "comboByteLength";
            this.comboByteLength.Size = new System.Drawing.Size(89, 24);
            this.comboByteLength.TabIndex = 0;
            // 
            // lblDebugging
            // 
            this.lblDebugging.AutoSize = true;
            this.lblDebugging.BackColor = System.Drawing.Color.Transparent;
            this.lblDebugging.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebugging.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblDebugging.Location = new System.Drawing.Point(6, 420);
            this.lblDebugging.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDebugging.Name = "lblDebugging";
            this.lblDebugging.Size = new System.Drawing.Size(108, 24);
            this.lblDebugging.TabIndex = 2;
            this.lblDebugging.Text = "Debugging";
            // 
            // offsetTxt
            // 
            this.offsetTxt.BackColor = System.Drawing.Color.Black;
            this.offsetTxt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offsetTxt.ForeColor = System.Drawing.Color.Chartreuse;
            this.offsetTxt.Location = new System.Drawing.Point(35, 214);
            this.offsetTxt.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.offsetTxt.Name = "offsetTxt";
            this.offsetTxt.Size = new System.Drawing.Size(84, 22);
            this.offsetTxt.TabIndex = 0;
            this.offsetTxt.Text = "0x00010000";
            // 
            // btnConnection
            // 
            this.btnConnection.BackColor = System.Drawing.Color.Black;
            this.btnConnection.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnection.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnection.ForeColor = System.Drawing.Color.White;
            this.btnConnection.Location = new System.Drawing.Point(18, 63);
            this.btnConnection.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(155, 27);
            this.btnConnection.TabIndex = 0;
            this.btnConnection.Text = "Connect";
            this.btnConnection.UseVisualStyleBackColor = false;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.BackColor = System.Drawing.Color.Transparent;
            this.lblOffset.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffset.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblOffset.Location = new System.Drawing.Point(9, 171);
            this.lblOffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(67, 24);
            this.lblOffset.TabIndex = 1;
            this.lblOffset.Text = "Offset";
            // 
            // SearcherTab
            // 
            this.SearcherTab.Controls.Add(this.TabCon);
            this.SearcherTab.Location = new System.Drawing.Point(4, 22);
            this.SearcherTab.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.SearcherTab.Name = "SearcherTab";
            this.SearcherTab.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.SearcherTab.Size = new System.Drawing.Size(1463, 726);
            this.SearcherTab.TabIndex = 1;
            this.SearcherTab.Text = "Searcher";
            this.SearcherTab.UseVisualStyleBackColor = true;
            // 
            // TabCon
            // 
            this.TabCon.Controls.Add(this.CodesTab);
            this.TabCon.Controls.Add(this.SearchTab);
            this.TabCon.Controls.Add(this.RangesTab);
            this.TabCon.Controls.Add(this.PluginsTab);
            this.TabCon.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabCon.Location = new System.Drawing.Point(-4, 0);
            this.TabCon.Name = "TabCon";
            this.TabCon.SelectedIndex = 0;
            this.TabCon.Size = new System.Drawing.Size(1236, 713);
            this.TabCon.TabIndex = 1;
            this.TabCon.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TabCon_KeyUp);
            // 
            // CodesTab
            // 
            this.CodesTab.BackColor = System.Drawing.Color.Black;
            this.CodesTab.Controls.Add(this.cbCodes);
            this.CodesTab.Controls.Add(this.cbSaveAll);
            this.CodesTab.Controls.Add(this.cbSaveAs);
            this.CodesTab.Controls.Add(this.cbImport);
            this.CodesTab.Controls.Add(this.cbSave);
            this.CodesTab.Controls.Add(this.cbRemove);
            this.CodesTab.Controls.Add(this.cbAdd);
            this.CodesTab.Controls.Add(this.cbWrite);
            this.CodesTab.Controls.Add(this.ConstantWrite);
            this.CodesTab.Controls.Add(this.Label1);
            this.CodesTab.Controls.Add(this.cbName);
            this.CodesTab.Controls.Add(this.cbList);
            this.CodesTab.ForeColor = System.Drawing.Color.White;
            this.CodesTab.Location = new System.Drawing.Point(4, 26);
            this.CodesTab.Name = "CodesTab";
            this.CodesTab.Padding = new System.Windows.Forms.Padding(3);
            this.CodesTab.Size = new System.Drawing.Size(1228, 683);
            this.CodesTab.TabIndex = 0;
            this.CodesTab.Text = "Codes";
            // 
            // cbCodes
            // 
            this.cbCodes.BackColor = System.Drawing.Color.White;
            this.cbCodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbCodes.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCodes.ForeColor = System.Drawing.Color.Black;
            this.cbCodes.Location = new System.Drawing.Point(525, 158);
            this.cbCodes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbCodes.Name = "cbCodes";
            this.cbCodes.Size = new System.Drawing.Size(616, 336);
            this.cbCodes.TabIndex = 44;
            this.cbCodes.Text = "";
            this.cbCodes.TextChanged += new System.EventHandler(this.cbCodes_TextChanged);
            this.cbCodes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbCodes_KeyDown);
            this.cbCodes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbCodes_KeyUp);
            // 
            // cbSaveAll
            // 
            this.cbSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSaveAll.Location = new System.Drawing.Point(161, 550);
            this.cbSaveAll.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbSaveAll.Name = "cbSaveAll";
            this.cbSaveAll.Size = new System.Drawing.Size(94, 28);
            this.cbSaveAll.TabIndex = 43;
            this.cbSaveAll.Text = "Save All";
            this.cbSaveAll.UseVisualStyleBackColor = true;
            this.cbSaveAll.Click += new System.EventHandler(this.cbSaveAll_Click);
            // 
            // cbSaveAs
            // 
            this.cbSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSaveAs.Location = new System.Drawing.Point(66, 550);
            this.cbSaveAs.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbSaveAs.Name = "cbSaveAs";
            this.cbSaveAs.Size = new System.Drawing.Size(94, 28);
            this.cbSaveAs.TabIndex = 42;
            this.cbSaveAs.Text = "Save As";
            this.cbSaveAs.UseVisualStyleBackColor = true;
            this.cbSaveAs.Click += new System.EventHandler(this.cbSaveAs_Click);
            // 
            // cbImport
            // 
            this.cbImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbImport.Location = new System.Drawing.Point(256, 515);
            this.cbImport.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbImport.Name = "cbImport";
            this.cbImport.Size = new System.Drawing.Size(94, 28);
            this.cbImport.TabIndex = 41;
            this.cbImport.Text = "Import";
            this.cbImport.UseVisualStyleBackColor = true;
            this.cbImport.Click += new System.EventHandler(this.cbImport_Click);
            // 
            // cbSave
            // 
            this.cbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSave.Location = new System.Drawing.Point(256, 550);
            this.cbSave.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbSave.Name = "cbSave";
            this.cbSave.Size = new System.Drawing.Size(94, 28);
            this.cbSave.TabIndex = 40;
            this.cbSave.Text = "Save";
            this.cbSave.UseVisualStyleBackColor = true;
            this.cbSave.Click += new System.EventHandler(this.cbSave_Click);
            // 
            // cbRemove
            // 
            this.cbRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRemove.Location = new System.Drawing.Point(161, 515);
            this.cbRemove.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbRemove.Name = "cbRemove";
            this.cbRemove.Size = new System.Drawing.Size(94, 28);
            this.cbRemove.TabIndex = 39;
            this.cbRemove.Text = "Remove";
            this.cbRemove.UseVisualStyleBackColor = true;
            this.cbRemove.Click += new System.EventHandler(this.cbRemove_Click);
            // 
            // cbAdd
            // 
            this.cbAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAdd.Location = new System.Drawing.Point(66, 515);
            this.cbAdd.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbAdd.Name = "cbAdd";
            this.cbAdd.Size = new System.Drawing.Size(94, 28);
            this.cbAdd.TabIndex = 38;
            this.cbAdd.Text = "Add";
            this.cbAdd.UseVisualStyleBackColor = true;
            this.cbAdd.Click += new System.EventHandler(this.cbAdd_Click);
            // 
            // cbWrite
            // 
            this.cbWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbWrite.Location = new System.Drawing.Point(692, 506);
            this.cbWrite.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbWrite.Name = "cbWrite";
            this.cbWrite.Size = new System.Drawing.Size(291, 37);
            this.cbWrite.TabIndex = 37;
            this.cbWrite.Text = "Write";
            this.cbWrite.UseVisualStyleBackColor = true;
            this.cbWrite.Click += new System.EventHandler(this.cbWrite_Click);
            // 
            // ConstantWrite
            // 
            this.ConstantWrite.Location = new System.Drawing.Point(528, 126);
            this.ConstantWrite.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ConstantWrite.Name = "ConstantWrite";
            this.ConstantWrite.Size = new System.Drawing.Size(257, 23);
            this.ConstantWrite.TabIndex = 36;
            this.ConstantWrite.Text = "Constant Write";
            this.ConstantWrite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ConstantWrite.UseVisualStyleBackColor = true;
            this.ConstantWrite.CheckedChanged += new System.EventHandler(this.ConstantWrite_CheckedChanged);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(525, 62);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(260, 26);
            this.Label1.TabIndex = 35;
            this.Label1.Text = "Name of code";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cbName
            // 
            this.cbName.BackColor = System.Drawing.Color.White;
            this.cbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbName.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbName.Location = new System.Drawing.Point(528, 94);
            this.cbName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(257, 20);
            this.cbName.TabIndex = 34;
            this.cbName.TextChanged += new System.EventHandler(this.cbName_TextChanged);
            // 
            // cbList
            // 
            this.cbList.BackColor = System.Drawing.Color.White;
            this.cbList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader4});
            this.cbList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbList.ForeColor = System.Drawing.Color.Black;
            this.cbList.FullRowSelect = true;
            this.cbList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.cbList.HideSelection = false;
            this.cbList.LabelWrap = false;
            this.cbList.Location = new System.Drawing.Point(10, 9);
            this.cbList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(436, 494);
            this.cbList.TabIndex = 33;
            this.cbList.UseCompatibleStateImageBehavior = false;
            this.cbList.View = System.Windows.Forms.View.Details;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            this.cbList.DoubleClick += new System.EventHandler(this.cbList_DoubleClick);
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Codes";
            this.ColumnHeader4.Width = 150;
            // 
            // SearchTab
            // 
            this.SearchTab.BackColor = System.Drawing.Color.Black;
            this.SearchTab.Controls.Add(this.rangesBox);
            this.SearchTab.Controls.Add(this.Play);
            this.SearchTab.Controls.Add(this.Pause);
            this.SearchTab.Controls.Add(this.StatusLabel);
            this.SearchTab.Controls.Add(this.Label2);
            this.SearchTab.Controls.Add(this.saveSRes);
            this.SearchTab.Controls.Add(this.loadSRes);
            this.SearchTab.Controls.Add(this.schVal2);
            this.SearchTab.Controls.Add(this.DumpMem);
            this.SearchTab.Controls.Add(this.compBox);
            this.SearchTab.Controls.Add(this.label8);
            this.SearchTab.Controls.Add(this.SchPWS);
            this.SearchTab.Controls.Add(this.Label3);
            this.SearchTab.Controls.Add(this.label7);
            this.SearchTab.Controls.Add(this.schProg);
            this.SearchTab.Controls.Add(this.SchHexCheck);
            this.SearchTab.Controls.Add(this.label9);
            this.SearchTab.Controls.Add(this.cbSchAlign);
            this.SearchTab.Controls.Add(this.SchRef);
            this.SearchTab.Controls.Add(this.lvSch);
            this.SearchTab.Controls.Add(this.schNSearch);
            this.SearchTab.Controls.Add(this.schVal);
            this.SearchTab.Controls.Add(this.schRange2);
            this.SearchTab.Controls.Add(this.schRange1);
            this.SearchTab.Controls.Add(this.schSearch);
            this.SearchTab.ForeColor = System.Drawing.Color.White;
            this.SearchTab.Location = new System.Drawing.Point(4, 26);
            this.SearchTab.Name = "SearchTab";
            this.SearchTab.Size = new System.Drawing.Size(1228, 683);
            this.SearchTab.TabIndex = 2;
            this.SearchTab.Text = "Search";
            // 
            // rangesBox
            // 
            this.rangesBox.AutoSize = true;
            this.rangesBox.Location = new System.Drawing.Point(541, 89);
            this.rangesBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.rangesBox.Name = "rangesBox";
            this.rangesBox.Size = new System.Drawing.Size(92, 21);
            this.rangesBox.TabIndex = 74;
            this.rangesBox.Text = "Set Ranges";
            this.rangesBox.UseVisualStyleBackColor = true;
            this.rangesBox.CheckedChanged += new System.EventHandler(this.rangesBox_CheckedChanged);
            // 
            // Play
            // 
            this.Play.BackgroundImage = global::Ps3Debugger.Properties.Resources.start_button;
            this.Play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Play.Location = new System.Drawing.Point(757, 16);
            this.Play.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(56, 34);
            this.Play.TabIndex = 73;
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // Pause
            // 
            this.Pause.BackgroundImage = global::Ps3Debugger.Properties.Resources.pause_button2;
            this.Pause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pause.Location = new System.Drawing.Point(757, 62);
            this.Pause.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(56, 34);
            this.Pause.TabIndex = 72;
            this.Pause.UseVisualStyleBackColor = true;
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(636, 258);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(46, 17);
            this.StatusLabel.TabIndex = 69;
            this.StatusLabel.Text = "Status:";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(109, 30);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(67, 20);
            this.Label2.TabIndex = 68;
            this.Label2.Text = "Value";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveSRes
            // 
            this.saveSRes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveSRes.Location = new System.Drawing.Point(801, 179);
            this.saveSRes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.saveSRes.Name = "saveSRes";
            this.saveSRes.Size = new System.Drawing.Size(135, 24);
            this.saveSRes.TabIndex = 67;
            this.saveSRes.Text = "Save Scan Results";
            this.saveSRes.UseVisualStyleBackColor = true;
            this.saveSRes.Click += new System.EventHandler(this.saveSRes_Click);
            // 
            // loadSRes
            // 
            this.loadSRes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadSRes.Location = new System.Drawing.Point(294, 179);
            this.loadSRes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.loadSRes.Name = "loadSRes";
            this.loadSRes.Size = new System.Drawing.Size(266, 24);
            this.loadSRes.TabIndex = 66;
            this.loadSRes.Text = "Load Scan Results";
            this.loadSRes.UseVisualStyleBackColor = true;
            this.loadSRes.Click += new System.EventHandler(this.loadSRes_Click);
            // 
            // schVal2
            // 
            this.schVal2.BackColor = System.Drawing.Color.Black;
            this.schVal2.ForeColor = System.Drawing.Color.White;
            this.schVal2.Location = new System.Drawing.Point(178, 56);
            this.schVal2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schVal2.Name = "schVal2";
            this.schVal2.Size = new System.Drawing.Size(335, 25);
            this.schVal2.TabIndex = 65;
            this.schVal2.Text = "00000001";
            this.schVal2.Visible = false;
            // 
            // DumpMem
            // 
            this.DumpMem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DumpMem.Location = new System.Drawing.Point(57, 179);
            this.DumpMem.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.DumpMem.Name = "DumpMem";
            this.DumpMem.Size = new System.Drawing.Size(131, 24);
            this.DumpMem.TabIndex = 64;
            this.DumpMem.Text = "Dump Memory";
            this.DumpMem.UseVisualStyleBackColor = true;
            this.DumpMem.Click += new System.EventHandler(this.DumpMem_Click);
            // 
            // compBox
            // 
            this.compBox.BackColor = System.Drawing.Color.Black;
            this.compBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.compBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.compBox.FormattingEnabled = true;
            this.compBox.Items.AddRange(new object[] {
            "Equal",
            "Not Equal",
            "Less Than",
            "Less Than Or Equal",
            "Greater Than",
            "Greater Than Or Equal",
            "Value Between"});
            this.compBox.Location = new System.Drawing.Point(181, 88);
            this.compBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.compBox.Name = "compBox";
            this.compBox.Size = new System.Drawing.Size(182, 25);
            this.compBox.TabIndex = 63;
            this.compBox.SelectedIndexChanged += new System.EventHandler(this.compBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(109, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 20);
            this.label8.TabIndex = 62;
            this.label8.Text = "Search";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SchPWS
            // 
            this.SchPWS.AutoSize = true;
            this.SchPWS.Location = new System.Drawing.Point(541, 128);
            this.SchPWS.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.SchPWS.Name = "SchPWS";
            this.SchPWS.Size = new System.Drawing.Size(154, 21);
            this.SchPWS.TabIndex = 61;
            this.SchPWS.Text = "Pause When Scanning";
            this.SchPWS.UseVisualStyleBackColor = true;
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(379, 119);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(41, 28);
            this.Label3.TabIndex = 60;
            this.Label3.Text = "Stop";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(380, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 28);
            this.label7.TabIndex = 59;
            this.label7.Text = "Start";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // schProg
            // 
            this.schProg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(210)))));
            this.schProg.Location = new System.Drawing.Point(25, 233);
            this.schProg.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schProg.Name = "schProg";
            this.schProg.Size = new System.Drawing.Size(1113, 22);
            this.schProg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.schProg.TabIndex = 58;
            // 
            // SchHexCheck
            // 
            this.SchHexCheck.AutoSize = true;
            this.SchHexCheck.Checked = true;
            this.SchHexCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SchHexCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SchHexCheck.Location = new System.Drawing.Point(58, 31);
            this.SchHexCheck.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.SchHexCheck.Name = "SchHexCheck";
            this.SchHexCheck.Size = new System.Drawing.Size(45, 17);
            this.SchHexCheck.TabIndex = 57;
            this.SchHexCheck.Text = "Hex";
            this.SchHexCheck.UseVisualStyleBackColor = true;
            this.SchHexCheck.CheckedChanged += new System.EventHandler(this.SchHexCheck_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(109, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 20);
            this.label9.TabIndex = 56;
            this.label9.Text = "Type";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbSchAlign
            // 
            this.cbSchAlign.BackColor = System.Drawing.Color.Black;
            this.cbSchAlign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSchAlign.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbSchAlign.FormattingEnabled = true;
            this.cbSchAlign.Items.AddRange(new object[] {
            "1 byte",
            "2 bytes",
            "4 bytes",
            "8 bytes",
            "X bytes",
            "Text",
            "Float byte"});
            this.cbSchAlign.Location = new System.Drawing.Point(181, 122);
            this.cbSchAlign.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbSchAlign.Name = "cbSchAlign";
            this.cbSchAlign.Size = new System.Drawing.Size(182, 25);
            this.cbSchAlign.TabIndex = 55;
            this.cbSchAlign.SelectedIndexChanged += new System.EventHandler(this.cbSchAlign_SelectedIndexChanged);
            // 
            // SchRef
            // 
            this.SchRef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SchRef.Location = new System.Drawing.Point(294, 206);
            this.SchRef.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.SchRef.Name = "SchRef";
            this.SchRef.Size = new System.Drawing.Size(266, 24);
            this.SchRef.TabIndex = 54;
            this.SchRef.Text = "Refresh Results From PS3";
            this.SchRef.UseVisualStyleBackColor = true;
            this.SchRef.Click += new System.EventHandler(this.SchRef_Click);
            // 
            // lvSch
            // 
            this.lvSch.BackColor = System.Drawing.Color.Black;
            this.lvSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvSchAddr,
            this.lvSchValue,
            this.lvSchDec,
            this.lvSchAlign});
            this.lvSch.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.lvSch.FullRowSelect = true;
            this.lvSch.HideSelection = false;
            this.lvSch.LabelEdit = true;
            this.lvSch.LabelWrap = false;
            this.lvSch.Location = new System.Drawing.Point(0, 257);
            this.lvSch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lvSch.Name = "lvSch";
            this.lvSch.Size = new System.Drawing.Size(1203, 426);
            this.lvSch.TabIndex = 53;
            this.lvSch.UseCompatibleStateImageBehavior = false;
            this.lvSch.View = System.Windows.Forms.View.Details;
            this.lvSch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvSch_KeyUp);
            // 
            // lvSchAddr
            // 
            this.lvSchAddr.Text = "Address";
            this.lvSchAddr.Width = 80;
            // 
            // lvSchValue
            // 
            this.lvSchValue.Text = "Hex Value";
            this.lvSchValue.Width = 120;
            // 
            // lvSchDec
            // 
            this.lvSchDec.Text = "Dec Value";
            this.lvSchDec.Width = 130;
            // 
            // lvSchAlign
            // 
            this.lvSchAlign.Text = "Alignment";
            this.lvSchAlign.Width = 80;
            // 
            // schNSearch
            // 
            this.schNSearch.Enabled = false;
            this.schNSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schNSearch.Location = new System.Drawing.Point(802, 206);
            this.schNSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schNSearch.Name = "schNSearch";
            this.schNSearch.Size = new System.Drawing.Size(134, 24);
            this.schNSearch.TabIndex = 52;
            this.schNSearch.Text = "Next Scan";
            this.schNSearch.UseVisualStyleBackColor = true;
            this.schNSearch.Click += new System.EventHandler(this.schNSearch_Click);
            // 
            // schVal
            // 
            this.schVal.BackColor = System.Drawing.Color.Black;
            this.schVal.ForeColor = System.Drawing.Color.White;
            this.schVal.Location = new System.Drawing.Point(178, 26);
            this.schVal.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schVal.Name = "schVal";
            this.schVal.Size = new System.Drawing.Size(335, 25);
            this.schVal.TabIndex = 51;
            this.schVal.Text = "00000064";
            // 
            // schRange2
            // 
            this.schRange2.BackColor = System.Drawing.Color.Black;
            this.schRange2.ForeColor = System.Drawing.Color.White;
            this.schRange2.Location = new System.Drawing.Point(427, 122);
            this.schRange2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schRange2.Name = "schRange2";
            this.schRange2.Size = new System.Drawing.Size(99, 25);
            this.schRange2.TabIndex = 50;
            this.schRange2.Text = "00020000";
            // 
            // schRange1
            // 
            this.schRange1.BackColor = System.Drawing.Color.Black;
            this.schRange1.ForeColor = System.Drawing.Color.White;
            this.schRange1.Location = new System.Drawing.Point(427, 88);
            this.schRange1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schRange1.Name = "schRange1";
            this.schRange1.Size = new System.Drawing.Size(99, 25);
            this.schRange1.TabIndex = 49;
            this.schRange1.Text = "00001000";
            // 
            // schSearch
            // 
            this.schSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schSearch.Location = new System.Drawing.Point(57, 206);
            this.schSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.schSearch.Name = "schSearch";
            this.schSearch.Size = new System.Drawing.Size(131, 24);
            this.schSearch.TabIndex = 48;
            this.schSearch.Text = "Initial Scan";
            this.schSearch.UseVisualStyleBackColor = true;
            this.schSearch.Click += new System.EventHandler(this.schSearch_Click);
            // 
            // RangesTab
            // 
            this.RangesTab.BackColor = System.Drawing.Color.Black;
            this.RangesTab.Controls.Add(this.findRangeProgBar);
            this.RangesTab.Controls.Add(this.findRanges);
            this.RangesTab.Controls.Add(this.recRangeBox);
            this.RangesTab.Controls.Add(this.Label4);
            this.RangesTab.Controls.Add(this.RangeDown);
            this.RangesTab.Controls.Add(this.RangeUp);
            this.RangesTab.Controls.Add(this.RemoveRange);
            this.RangesTab.Controls.Add(this.AddRange);
            this.RangesTab.Controls.Add(this.SaveRange);
            this.RangesTab.Controls.Add(this.ImportRange);
            this.RangesTab.Controls.Add(this.Label5);
            this.RangesTab.Controls.Add(this.Label6);
            this.RangesTab.Controls.Add(this.rangeView);
            this.RangesTab.ForeColor = System.Drawing.Color.White;
            this.RangesTab.Location = new System.Drawing.Point(4, 26);
            this.RangesTab.Name = "RangesTab";
            this.RangesTab.Padding = new System.Windows.Forms.Padding(3);
            this.RangesTab.Size = new System.Drawing.Size(1228, 683);
            this.RangesTab.TabIndex = 1;
            this.RangesTab.Text = "Ranges";
            // 
            // findRangeProgBar
            // 
            this.findRangeProgBar.Location = new System.Drawing.Point(676, 128);
            this.findRangeProgBar.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.findRangeProgBar.Name = "findRangeProgBar";
            this.findRangeProgBar.Size = new System.Drawing.Size(300, 28);
            this.findRangeProgBar.TabIndex = 26;
            // 
            // findRanges
            // 
            this.findRanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findRanges.Location = new System.Drawing.Point(992, 128);
            this.findRanges.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.findRanges.Name = "findRanges";
            this.findRanges.Size = new System.Drawing.Size(94, 28);
            this.findRanges.TabIndex = 25;
            this.findRanges.Text = "Find Ranges";
            this.findRanges.UseVisualStyleBackColor = true;
            this.findRanges.Click += new System.EventHandler(this.findRanges_Click);
            // 
            // recRangeBox
            // 
            this.recRangeBox.BackColor = System.Drawing.Color.Black;
            this.recRangeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.recRangeBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1});
            this.recRangeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.recRangeBox.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.recRangeBox.LabelWrap = false;
            this.recRangeBox.Location = new System.Drawing.Point(676, 186);
            this.recRangeBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.recRangeBox.MultiSelect = false;
            this.recRangeBox.Name = "recRangeBox";
            this.recRangeBox.Size = new System.Drawing.Size(410, 301);
            this.recRangeBox.TabIndex = 24;
            this.recRangeBox.TabStop = false;
            this.recRangeBox.UseCompatibleStateImageBehavior = false;
            this.recRangeBox.View = System.Windows.Forms.View.Details;
            this.recRangeBox.DoubleClick += new System.EventHandler(this.recRangeBox_DoubleClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "File";
            this.ColumnHeader1.Width = 204;
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(732, 160);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(282, 20);
            this.Label4.TabIndex = 23;
            this.Label4.Text = "Recent Ranges";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RangeDown
            // 
            this.RangeDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RangeDown.Location = new System.Drawing.Point(918, 499);
            this.RangeDown.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.RangeDown.Name = "RangeDown";
            this.RangeDown.Size = new System.Drawing.Size(121, 24);
            this.RangeDown.TabIndex = 22;
            this.RangeDown.Text = "Down";
            this.RangeDown.UseVisualStyleBackColor = true;
            this.RangeDown.Click += new System.EventHandler(this.RangeDown_Click);
            // 
            // RangeUp
            // 
            this.RangeUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RangeUp.Location = new System.Drawing.Point(757, 499);
            this.RangeUp.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.RangeUp.Name = "RangeUp";
            this.RangeUp.Size = new System.Drawing.Size(121, 24);
            this.RangeUp.TabIndex = 21;
            this.RangeUp.Text = "Up";
            this.RangeUp.UseVisualStyleBackColor = true;
            this.RangeUp.Click += new System.EventHandler(this.RangeUp_Click);
            // 
            // RemoveRange
            // 
            this.RemoveRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveRange.Location = new System.Drawing.Point(918, 593);
            this.RemoveRange.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.RemoveRange.Name = "RemoveRange";
            this.RemoveRange.Size = new System.Drawing.Size(121, 24);
            this.RemoveRange.TabIndex = 20;
            this.RemoveRange.Text = "Remove";
            this.RemoveRange.UseVisualStyleBackColor = true;
            this.RemoveRange.Click += new System.EventHandler(this.RemoveRange_Click);
            // 
            // AddRange
            // 
            this.AddRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddRange.Location = new System.Drawing.Point(757, 593);
            this.AddRange.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.AddRange.Name = "AddRange";
            this.AddRange.Size = new System.Drawing.Size(121, 24);
            this.AddRange.TabIndex = 19;
            this.AddRange.Text = "Add";
            this.AddRange.UseVisualStyleBackColor = true;
            this.AddRange.Click += new System.EventHandler(this.AddRange_Click);
            // 
            // SaveRange
            // 
            this.SaveRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveRange.Location = new System.Drawing.Point(918, 547);
            this.SaveRange.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.SaveRange.Name = "SaveRange";
            this.SaveRange.Size = new System.Drawing.Size(121, 24);
            this.SaveRange.TabIndex = 18;
            this.SaveRange.Text = "Save";
            this.SaveRange.UseVisualStyleBackColor = true;
            this.SaveRange.Click += new System.EventHandler(this.SaveRange_Click);
            // 
            // ImportRange
            // 
            this.ImportRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportRange.Location = new System.Drawing.Point(757, 547);
            this.ImportRange.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ImportRange.Name = "ImportRange";
            this.ImportRange.Size = new System.Drawing.Size(121, 24);
            this.ImportRange.TabIndex = 17;
            this.ImportRange.Text = "Import";
            this.ImportRange.UseVisualStyleBackColor = true;
            this.ImportRange.Click += new System.EventHandler(this.ImportRange_Click);
            // 
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(317, 70);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(584, 19);
            this.Label5.TabIndex = 16;
            this.Label5.Text = "Be Sure To Order From Least To Greatest";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label6
            // 
            this.Label6.Location = new System.Drawing.Point(320, 51);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(581, 19);
            this.Label6.TabIndex = 15;
            this.Label6.Text = "Memory Range Of Game";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rangeView
            // 
            this.rangeView.BackColor = System.Drawing.Color.Black;
            this.rangeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rangeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader2,
            this.ColumnHeader3});
            this.rangeView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.rangeView.FullRowSelect = true;
            this.rangeView.HideSelection = false;
            this.rangeView.LabelWrap = false;
            this.rangeView.Location = new System.Drawing.Point(26, 104);
            this.rangeView.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.rangeView.MultiSelect = false;
            this.rangeView.Name = "rangeView";
            this.rangeView.Size = new System.Drawing.Size(553, 496);
            this.rangeView.TabIndex = 14;
            this.rangeView.UseCompatibleStateImageBehavior = false;
            this.rangeView.View = System.Windows.Forms.View.Details;
            this.rangeView.SelectedIndexChanged += new System.EventHandler(this.rangeView_SelectedIndexChanged);
            this.rangeView.DoubleClick += new System.EventHandler(this.rangeView_DoubleClick);
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Start Address";
            this.ColumnHeader2.Width = 110;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "End Address";
            this.ColumnHeader3.Width = 110;
            // 
            // PluginsTab
            // 
            this.PluginsTab.BackColor = System.Drawing.Color.Black;
            this.PluginsTab.Controls.Add(this.plugIcon);
            this.PluginsTab.Controls.Add(this.descPlugDesc);
            this.PluginsTab.Controls.Add(this.descPlugVer);
            this.PluginsTab.Controls.Add(this.descPlugAuth);
            this.PluginsTab.Controls.Add(this.refPlugin);
            this.PluginsTab.Controls.Add(this.descPlugName);
            this.PluginsTab.Controls.Add(this.pluginList);
            this.PluginsTab.ForeColor = System.Drawing.Color.Black;
            this.PluginsTab.Location = new System.Drawing.Point(4, 26);
            this.PluginsTab.Name = "PluginsTab";
            this.PluginsTab.Size = new System.Drawing.Size(1228, 683);
            this.PluginsTab.TabIndex = 3;
            this.PluginsTab.Text = "Plugins";
            // 
            // plugIcon
            // 
            this.plugIcon.InitialImage = ((System.Drawing.Image)(resources.GetObject("plugIcon.InitialImage")));
            this.plugIcon.Location = new System.Drawing.Point(434, 297);
            this.plugIcon.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.plugIcon.Name = "plugIcon";
            this.plugIcon.Size = new System.Drawing.Size(655, 340);
            this.plugIcon.TabIndex = 18;
            this.plugIcon.TabStop = false;
            // 
            // descPlugDesc
            // 
            this.descPlugDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugDesc.Location = new System.Drawing.Point(434, 143);
            this.descPlugDesc.Name = "descPlugDesc";
            this.descPlugDesc.Size = new System.Drawing.Size(655, 148);
            this.descPlugDesc.TabIndex = 16;
            this.descPlugDesc.Text = "Plugin Description";
            // 
            // descPlugVer
            // 
            this.descPlugVer.AutoSize = true;
            this.descPlugVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugVer.ForeColor = System.Drawing.Color.White;
            this.descPlugVer.Location = new System.Drawing.Point(467, 121);
            this.descPlugVer.Name = "descPlugVer";
            this.descPlugVer.Size = new System.Drawing.Size(74, 13);
            this.descPlugVer.TabIndex = 15;
            this.descPlugVer.Text = "Plugin Version";
            // 
            // descPlugAuth
            // 
            this.descPlugAuth.AutoSize = true;
            this.descPlugAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugAuth.ForeColor = System.Drawing.Color.White;
            this.descPlugAuth.Location = new System.Drawing.Point(450, 98);
            this.descPlugAuth.Name = "descPlugAuth";
            this.descPlugAuth.Size = new System.Drawing.Size(100, 13);
            this.descPlugAuth.TabIndex = 14;
            this.descPlugAuth.Text = "by Plugin Author";
            // 
            // refPlugin
            // 
            this.refPlugin.BackColor = System.Drawing.Color.Black;
            this.refPlugin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refPlugin.ForeColor = System.Drawing.Color.White;
            this.refPlugin.Location = new System.Drawing.Point(717, 6);
            this.refPlugin.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.refPlugin.Name = "refPlugin";
            this.refPlugin.Size = new System.Drawing.Size(255, 65);
            this.refPlugin.TabIndex = 17;
            this.refPlugin.Text = "Load Plugins";
            this.refPlugin.UseVisualStyleBackColor = false;
            this.refPlugin.Click += new System.EventHandler(this.refPlugin_Click);
            // 
            // descPlugName
            // 
            this.descPlugName.AutoSize = true;
            this.descPlugName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugName.ForeColor = System.Drawing.Color.White;
            this.descPlugName.Location = new System.Drawing.Point(431, 76);
            this.descPlugName.Name = "descPlugName";
            this.descPlugName.Size = new System.Drawing.Size(78, 13);
            this.descPlugName.TabIndex = 13;
            this.descPlugName.Text = "Plugin Name";
            // 
            // pluginList
            // 
            this.pluginList.BackColor = System.Drawing.Color.Black;
            this.pluginList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pluginList.ForeColor = System.Drawing.Color.White;
            this.pluginList.FormattingEnabled = true;
            this.pluginList.ItemHeight = 17;
            this.pluginList.Location = new System.Drawing.Point(14, 57);
            this.pluginList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.pluginList.Name = "pluginList";
            this.pluginList.Size = new System.Drawing.Size(362, 580);
            this.pluginList.TabIndex = 12;
            this.pluginList.SelectedIndexChanged += new System.EventHandler(this.pluginList_SelectedIndexChanged);
            this.pluginList.DoubleClick += new System.EventHandler(this.pluginList_DoubleClick);
            // 
            // autoUpdateSetTimer
            // 
            this.autoUpdateSetTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // autoUpdateGetTimer
            // 
            this.autoUpdateGetTimer.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // AutoDebug
            // 
            this.AutoDebug.Tick += new System.EventHandler(this.AutoDebug_Tick);
            // 
            // DumpWorker
            // 
            this.DumpWorker.WorkerReportsProgress = true;
            this.DumpWorker.WorkerSupportsCancellation = true;
            this.DumpWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DumpWorker_DoWork);
            this.DumpWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DumpWorker_ProgressChanged);
            this.DumpWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DumpWorker_WorkerCompleted);
            // 
            // RangeWorker
            // 
            this.RangeWorker.WorkerReportsProgress = true;
            this.RangeWorker.WorkerSupportsCancellation = true;
            this.RangeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RangeWorker_DoWork);
            this.RangeWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.RangeWorker_ProgressChanged);
            this.RangeWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RangeWorker_WorkerCompleted);
            // 
            // Screentimer
            // 
            this.Screentimer.Tick += new System.EventHandler(this.Screentimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1473, 750);
            this.Controls.Add(this.TabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debugger";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.TabControl.ResumeLayout(false);
            this.Ps3DebuggerTab.ResumeLayout(false);
            this.Ps3DebuggerTab.PerformLayout();
            this.SearcherTab.ResumeLayout(false);
            this.TabCon.ResumeLayout(false);
            this.CodesTab.ResumeLayout(false);
            this.CodesTab.PerformLayout();
            this.SearchTab.ResumeLayout(false);
            this.SearchTab.PerformLayout();
            this.RangesTab.ResumeLayout(false);
            this.PluginsTab.ResumeLayout(false);
            this.PluginsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage Ps3DebuggerTab;
        private System.Windows.Forms.TabPage SearcherTab;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.Label connectedLbl;
        private System.Windows.Forms.Button startDebug;
        private System.Windows.Forms.CheckBox startAutoDebug;
        private System.Windows.Forms.Label lblms;
        private System.Windows.Forms.TextBox autoUpdateTime;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.Button plusHundred;
        private System.Windows.Forms.Button minusHundred;
        private System.Windows.Forms.TextBox jumpUint;
        private System.Windows.Forms.Label lblJump;
        private System.Windows.Forms.Label lblBytes;
        private System.Windows.Forms.TextBox bytesLegthText;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.TextBox startHex;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblDebugging;
        private System.Windows.Forms.CheckBox AutoUpdateset;
        private System.Windows.Forms.CheckBox AutoUpdateget;
        private System.Windows.Forms.Button getOffset;
        private System.Windows.Forms.Button setOffset;
        private System.Windows.Forms.ComboBox dexHexCombo;
        private System.Windows.Forms.TextBox resultTxt;
        private System.Windows.Forms.ComboBox comboByteLength;
        private System.Windows.Forms.TextBox offsetTxt;
        private System.Windows.Forms.Label lblOffset;       
        private System.Windows.Forms.Timer autoUpdateSetTimer;
        private System.Windows.Forms.Timer autoUpdateGetTimer;
        private System.Windows.Forms.Timer AutoDebug;
        internal System.Windows.Forms.TabControl TabCon;
        internal System.Windows.Forms.TabPage CodesTab;
        private System.Windows.Forms.RichTextBox cbCodes;
        private System.Windows.Forms.Button cbSaveAll;
        private System.Windows.Forms.Button cbSaveAs;
        private System.Windows.Forms.Button cbImport;
        private System.Windows.Forms.Button cbSave;
        private System.Windows.Forms.Button cbRemove;
        private System.Windows.Forms.Button cbAdd;
        private System.Windows.Forms.Button cbWrite;
        private System.Windows.Forms.CheckBox ConstantWrite;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox cbName;
        private System.Windows.Forms.ListView cbList;
        private System.Windows.Forms.ColumnHeader ColumnHeader4;
        internal System.Windows.Forms.TabPage SearchTab;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Button saveSRes;
        private System.Windows.Forms.Button loadSRes;
        private System.Windows.Forms.TextBox schVal2;
        private System.Windows.Forms.Button DumpMem;
        private System.Windows.Forms.ComboBox compBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox SchPWS;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar schProg;
        private System.Windows.Forms.CheckBox SchHexCheck;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbSchAlign;
        private System.Windows.Forms.Button SchRef;
        public System.Windows.Forms.ListView lvSch;
        private System.Windows.Forms.ColumnHeader lvSchAddr;
        private System.Windows.Forms.ColumnHeader lvSchValue;
        private System.Windows.Forms.ColumnHeader lvSchDec;
        private System.Windows.Forms.ColumnHeader lvSchAlign;
        private System.Windows.Forms.Button schNSearch;
        private System.Windows.Forms.TextBox schVal;
        private System.Windows.Forms.TextBox schRange2;
        private System.Windows.Forms.TextBox schRange1;
        private System.Windows.Forms.Button schSearch;
        internal System.Windows.Forms.TabPage RangesTab;
        private System.Windows.Forms.ProgressBar findRangeProgBar;
        private System.Windows.Forms.Button findRanges;
        private System.Windows.Forms.ListView recRangeBox;
        private System.Windows.Forms.ColumnHeader ColumnHeader1;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.Button RangeDown;
        private System.Windows.Forms.Button RangeUp;
        private System.Windows.Forms.Button RemoveRange;
        private System.Windows.Forms.Button AddRange;
        private System.Windows.Forms.Button SaveRange;
        private System.Windows.Forms.Button ImportRange;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.ListView rangeView;
        private System.Windows.Forms.ColumnHeader ColumnHeader2;
        private System.Windows.Forms.ColumnHeader ColumnHeader3;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button Options;
        private System.ComponentModel.BackgroundWorker DumpWorker;
        private System.Windows.Forms.Button Play;
        public System.Windows.Forms.Button Pause;
        private System.Windows.Forms.RichTextBox hexCode;
        private System.Windows.Forms.RichTextBox offsetsText;
        private System.Windows.Forms.RichTextBox asciiText;
        private System.Windows.Forms.Button JumpUp;
        private System.Windows.Forms.Button JumpDown;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage PluginsTab;
        private System.ComponentModel.BackgroundWorker RangeWorker;
        private System.Windows.Forms.PictureBox plugIcon;
        private System.Windows.Forms.Label descPlugDesc;
        private System.Windows.Forms.Label descPlugVer;
        private System.Windows.Forms.Label descPlugAuth;
        private System.Windows.Forms.Button refPlugin;
        private System.Windows.Forms.Label descPlugName;
        private System.Windows.Forms.ListBox pluginList;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer Screentimer;
        private System.Windows.Forms.Button Attach;
        private System.Windows.Forms.Button Ps3Discon;
        private System.Windows.Forms.CheckBox rangesBox;
        private System.Windows.Forms.ListBox asmBox;
    }
}

