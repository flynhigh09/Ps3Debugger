using System;
using Microsoft.VisualBasic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using PS3Lib;
using System.Reflection;
using System.IO.Compression;
using RTF;
using Ps3Debugger;
using System.Globalization;
using System.IO;
using CWPS3Portable;

namespace Ps3Debugger
{
    public partial class Form1 : Form
    {
        #region Global Variables // Debugger
        public static PS3API PS3 = new PS3API(SelectAPI.TargetManager);
        public static Form1 Instance = (Form1)null;
        public static bool PluginAllowColoring = false;
        public static bool DefaultPluginAllowColoring = true;
        public static bool isClosing = false;
        public static int Target = 0;
        public static bool connected = false;
        public static bool attached = false;
        public static int CodesCount = 0;
        public static int ConstantLoop = 0;
        public static bool bComment = false;
        public static int cbListIndex = 0;
        public static uint Api = 0;
        public bool Checkinout;
        public bool Connection = false;
        public static string VersionNum = "1.01";
        public static string IPAddrStr = "";
        public static uint ProcessID;
        public static uint[] processIDs;
        public static string MemStatus;
        public static uint[] MemArray;
        public static uint TotalResults = 0U;
        public static float TotalTime = 0.0f;

        // Debugger
        private byte[] dispBytes;
        private byte[] displayedBytes;
        Color invForeColor;
        ulong curAddr = 0x00010000;
        int rowCount = 640;//992
        int bytesPerColumn = 32; //32
        int asmbytesPerColumn = 4;
        private static int oldASMSelIndex = -1;
        static CW CodeWiz = new CW();
        static bool _relBCNDBool = false;
        private static bool relBCNDBool
        {

            get { return _relBCNDBool; }
            set
            {

                _relBCNDBool = value;
                CodeWiz.isBranchCompareRelative = _relBCNDBool;
            }
        }

        private Thread TargetInfo;
        private const bool threadIsRunning = false;

        #region Global Netcheat Variables
        public const int MaxCodes = 1000; /* Max number of codes */

        /* Search variables */
        public const int MaxRes = 10000; /* Max number of results */
        public static ulong SchResCnt = 0; /* Number of results */
        public static int NextSAlign = 0; /* Initial Search sets this to the current alignment */
        public static bool ValHex = true; /* Determines whether the value is in hex or not */
        public static bool NewSearch = true; /* When true, Initial Scan will show */
        public static int CancelSearch = 0; /* When 1 the search will cancel, 2 = stop */
        public static ulong GlobAlign = 0; /* Alignment */
        public static string dFileName = "a.txt"; /* Dump file when searching */
        public static int compMode = 0; /* Comparison type */

        /* Pre-parsed code struct */
        public struct CodeData
        {
            public byte[] val;
            public ulong addr;
            public char type;
            public byte[] jbool; /* Joker boolean value */
            public int jsize; /* Numbero of lines to execute with it true; joker */
        }
        /* Code struct */
        public struct CodeDB
        {          /* Structure for a single code */
            public bool state;          /* Determines whether to write constantly or not */
            public string name;         /* Name of Code */
            public string codes;        /* Holds codes string */
            public CodeData[] CData;    /* Holds codes in parsed format */
            public string filename;     /* For use with the 'Save' button */
        };
        /* Search result struct - NOT USED */
        public struct CodeRes
        {    /* Structure for a single search result */
            public bool state;     /* Determines whether to write constantly or not */
            public ulong addr;    /* Address of the code */
            public byte[] val;     /* Value of the code */
            public int align;      /* Alignment of the code */
        };
        /* List result struct */
        public struct ListRes
        {
            public string Addr;
            public string HexVal;
            public string DecVal;
            public string AlignStr;
        }
        /* Codes Array */
        public static CodeDB[] Codes = new CodeDB[MaxCodes];
        /* Search Types */
        public const int compEq = 0;        /* Equal To */
        public const int compNEq = 1;       /* Not Equal To */
        public const int compLT = 2;        /* Less Than */
        public const int compLTE = 3;       /* Less Than Or Equal To */
        public const int compGT = 4;        /* Greater Than */
        public const int compGTE = 5;       /* Greater Than Or Equal To */
        public const int compVBet = 6;      /* Value Between */
        public const int compINC = 7;       /* Increased Value */
        public const int compDEC = 8;       /* Decreased Value */
        public const int compChg = 9;       /* Changed Value */
        public const int compUChg = 10;     /* Unchanged Value */

        public const int compANEq = 20;     /* And Equal (used with E joker type) */
        /* Input Box Argument Structure */
        public struct IBArg
        {
            public string label;
            public string defStr;
            public string retStr;
        };

        /* ForeColor and BackColor */
        public static Color ncBackColor = Color.Black;
        public static Color ncForeColor = Color.FromArgb(200, 25, 125);
        /* Keybind arrays */
        public static Keys[] keyBinds = new Keys[8];
        public static string[] keyNames = new string[] 
        {
            "Connect And Attach", "Disconnect",
            "Initial Scan", "Next Scan", "Stop", "Refresh Results",
            "Toggle Constant Write", "Write"
        };
        /* API dll */
        public static int apiDLL = 0;
        /* Settings file path */
        public static string settFile = "";
        /* String array that holds each range import */
        public static string[] rangeImports = new string[0];
        /* Int array that defines the order of the recent ranges */
        public static int[] rangeOrder = new int[0];

        /* Plugin form related arrays */
        static PluginForm[] pluginForm = new PluginForm[0];
        public static bool[] pluginFormActive = new bool[0];
        public static int setplugWindow = -1;

        /* Delete block struct */
        public struct deleteArr
        {
            public int start;
            public int size;
        }

        /* Constant writing thread */
        public static System.Threading.Thread tConstWrite = new System.Threading.Thread(new System.Threading.ThreadStart(codes.BeginConstWriting));
        #endregion
        #endregion

        #region Interface Functions
        /* API related functions */
        public static void apiSetMem(ulong addr, byte[] val) //Set the memory
        {
            if (val != null && connected)
                PS3.SetMemory((uint)addr, val);
            //PS3TMAPI.ProcessSetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, addr, val);
        }
        public static bool apiGetMem(ulong addr, ref byte[] val) //Gets the memory as a byte array
        {
            bool ret = false;
            if (val != null && connected)
            {
                if (apiDLL == 0)
                    ret = (PS3Lib.PS3TMAPI.ProcessGetMemory(0, PS3Lib.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID, 0, addr, ref val) ==
                        PS3TMAPI.SNRESULT.SN_S_OK);
                else
                    ret = (PS3.CCAPI.GetMemory(addr, val) >= 0);
            }
            return ret;
        }
        public static byte[] apiGetMem2(ulong addr, byte[] val) //Gets the memory as a byte array
        {
            bool ret = false;
            if (val != null && connected)
            {
                if (apiDLL == 0)
                    ret = (PS3Lib.PS3TMAPI.ProcessGetMemory(0, PS3Lib.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID, 0, addr, ref val) ==
                        PS3Lib.PS3TMAPI.SNRESULT.SN_S_OK);
                else
                    ret = (PS3.CCAPI.GetMemory(addr, val) >= 0);              
            }
            return BitConverter.GetBytes(ret);
        }
        /* Finds the listindex of the listview cbList */
        public int FindLI()
        {
            int x = 0;

            //Potentially let the list update the selected Code
            Application.DoEvents();

            //Finds the list index (selected Code)
            for (x = 0; x <= cbList.Items.Count - 1; x++)
            {
                if (cbList.Items[x].Selected == true)
                    return x;
            }

            return -1;
        }
        /* Updates the code name, state, and codes */
        private void UpdateCB(int Index)
        {
            if (Index < 0 || Index >= MaxCodes)
                return;

            //Update the textboxes to the new code
            cbName.Text = Codes[Index].name;
            cbCodes.Text = Codes[Index].codes;
            ConstantWrite.Checked = Codes[Index].state;
        }
        private string BrowseForFile(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (filter != null)
                openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;
            else
                return "";
        }
        /* Sorts the recRangeBox according to the rangeOrder */
        void UpdateRecRangeBox()
        {
            recRangeBox.Items.Clear();
            foreach (int val in rangeOrder)
                recRangeBox.Items.Add("");

            for (int impRan = 0; impRan < rangeOrder.Length; impRan++)
            {
                string str = rangeImports[impRan];
                if (str != "")
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(str);
                    ListViewItem lvi = new ListViewItem(new string[] { fi.Name });
                    lvi.Text = fi.Name;
                    lvi.Tag = impRan.ToString();
                    lvi.ToolTipText = fi.FullName;
                    lvi.BackColor = ncBackColor;
                    lvi.ForeColor = ncForeColor;
                    recRangeBox.Items[rangeOrder[impRan]] = lvi;
                }
            }
        }
        public void UpdateMemArray()
        {
            misc.MemArray = new uint[rangeView.Items.Count * 2];
            for (int x = 0; x < rangeView.Items.Count; x++)
            {
                misc.MemArray[x * 2] = uint.Parse(rangeView.Items[x].SubItems[0].Text,
                    System.Globalization.NumberStyles.HexNumber);
                misc.MemArray[(x * 2) + 1] = uint.Parse(rangeView.Items[x].SubItems[1].Text,
                    System.Globalization.NumberStyles.HexNumber);
            }
        }
        /* Saves the options to the ncps3.ini file */
        public static void SaveOptions()
        {
            using (System.IO.StreamWriter fd = new System.IO.StreamWriter(settFile, false))
            {
                //KeyBinds
                for (int x = 0; x < keyBinds.Length; x++)
                {
                    string key = keyBinds[x].GetHashCode().ToString();
                    fd.WriteLine(key);
                }
                //Colors
                fd.WriteLine(ncBackColor.Name);
                fd.WriteLine(ncForeColor.Name);
                //Recently opened ranges order
                string range = "";
                foreach (int val in rangeOrder)
                    range += val.ToString() + ";";
                if (range == "")
                    range = ";";
                fd.WriteLine(range);
                //Recently opened ranges paths
                range = "";
                foreach (string str in rangeImports)
                    range += str + ";";
                if (range == "")
                    range = ";";
                fd.WriteLine(range);
                //API
                fd.WriteLine(apiDLL.ToString());
            }
        }
        string ParseValFromStr(string val)
        {
            val = val.ToLower();
            val = val.Replace(" ", "");
            val = val.Replace("zero", "0");
            val = val.Replace("one", "1");
            val = val.Replace("two", "2");
            val = val.Replace("three", "3");
            val = val.Replace("four", "4");
            val = val.Replace("five", "5");
            val = val.Replace("six", "6");
            val = val.Replace("seven", "7");
            val = val.Replace("eight", "8");
            val = val.Replace("nine", "9");
            val = val.Replace("see", "C");
            val = val.Replace("be", "B");
            return val;
        }
        #endregion
       
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                Checkinout = true;
                Opacity = 0;
                Screentimer.Enabled = true;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (e.Cancel == true)
                return;
            if (Opacity > 0)
            {
                Checkinout = false;
                Screentimer.Enabled = true;
                e.Cancel = true;
            }
        }
        private void Screentimer_Tick(object sender, EventArgs e)
        {
            if (Checkinout == false)
            {
                Opacity -= (Screentimer.Interval / 750.0);
                if (Opacity > 0)
                    Screentimer.Enabled = true;
                else
                {
                    Screentimer.Enabled = false;
                    Close();
                }
            }
            else
            {
                Opacity += (Screentimer.Interval / 750.0);
                Screentimer.Enabled = (Opacity < 1.0);
                Checkinout = (Opacity < 1.0);
            }
        }
        public Form1()
        {
            Hide();
            Thread splashthread = new Thread((SplashScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            InitializeComponent();

            hexCode.KeyDown += new KeyEventHandler(rtbKeyDown);
            hexCode.TextChanged += new EventHandler(rtbTextChanged);
            hexCode.MouseUp += new MouseEventHandler(rtbMouseUp);
            hexCode.MouseDown += new MouseEventHandler(rtbOnMouseDown);
            hexCode.KeyPress += new KeyPressEventHandler(rtbKeyPress);

            asciiText.KeyDown += new KeyEventHandler(rtbKeyDown);
            asciiText.TextChanged += new EventHandler(rtbTextChanged);
            asciiText.MouseUp += new MouseEventHandler(rtbMouseUp);
            asciiText.MouseDown += new MouseEventHandler(rtbOnMouseDown);
            asciiText.KeyPress += new KeyPressEventHandler(rtbKeyPress);

            comboByteLength.SelectedIndex = 0;
            dexHexCombo.SelectedIndex = 0;
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            TabCon.KeyUp += new KeyEventHandler(Form1_KeyUp);
            TabCon.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SplashScreen.UdpateStatusText("Loading Items!!");

            CodeWiz.DeclareInstructions();
            CodeWiz.DeclareHelpStr();

            /* Add psuedo ops */
            CodeWiz.customPseudos = new CW.pInstruction[3];
            CodeWiz.customPseudos[0].asm = @"ori %rD, %rA, 0";
            CodeWiz.customPseudos[0].format = @"%rD, %rA";
            CodeWiz.customPseudos[0].name = "mr";
            CodeWiz.customPseudos[0].regs = new string[] { @"%rD", @"%rA" };

            CodeWiz.customPseudos[1].asm = @"hexcode 0x4E800420";
            CodeWiz.customPseudos[1].format = @"";
            CodeWiz.customPseudos[1].name = "bctr";
            CodeWiz.customPseudos[1].regs = new string[0];

            CodeWiz.customPseudos[2].asm = @"hexcode 0x4E800421";
            CodeWiz.customPseudos[2].format = @"";
            CodeWiz.customPseudos[2].name = "bctrl";
            CodeWiz.customPseudos[2].regs = new string[0];

            CodeWiz.customPseudos[2].asm = @"hexcode 0x44000002";
            CodeWiz.customPseudos[2].format = @"";
            CodeWiz.customPseudos[2].name = "sc";
            CodeWiz.customPseudos[2].regs = new string[0];

            asmBox.LostFocus += new EventHandler(listBoxLoseFocus);
            asmBox.KeyDown += new KeyEventHandler(listBoxKeyDown);

            Thread.Sleep(500);
            SplashScreen.UdpateStatusText("Almost Done!!");
            Thread.Sleep(500);
            Show();
            SplashScreen.CloseSplashScreen();
            Activate();

            Text = "Debugger " + VersionNum + " by flynhigh09";
            int x = 0;
            settFile = Application.StartupPath + "\\ps3.ini";
            if (System.IO.File.Exists(settFile))
            {
                string[] settLines = System.IO.File.ReadAllLines(settFile);
                try
                {
                    for (x = 0; x < keyBinds.Length; x++)
                        keyBinds[x] = (Keys)int.Parse(settLines[x]);

                    ncBackColor = Color.FromArgb(int.Parse(settLines[x], System.Globalization.NumberStyles.HexNumber)); BackColor = ncBackColor; x++;
                    ncForeColor = Color.FromArgb(int.Parse(settLines[x], System.Globalization.NumberStyles.HexNumber)); ForeColor = ncForeColor; x++;

                    string[] strRangeOrder = settLines[x].Split(';');
                    Array.Resize(ref rangeOrder, strRangeOrder.Length - 1);
                    for (int valRO = 0; valRO < rangeOrder.Length; valRO++)
                        if (strRangeOrder[valRO] != "")
                            rangeOrder[valRO] = int.Parse(strRangeOrder[valRO]);
                    x++;
                    rangeImports = settLines[x].Split(';');

                    Array.Resize(ref rangeImports, rangeImports.Length - 1);
                    UpdateRecRangeBox();
                    x++;
                    apiDLL = int.Parse(settLines[x]);
                    x++;
                }
                catch
                {
                }
            }
            PS3.ChangeAPI((apiDLL == 0) ? SelectAPI.TargetManager : SelectAPI.ControlConsole);
            if (apiDLL == 0)
                PS3.PS3TMAPI_NET();
            else
            {
                SchPWS.Visible = false;
            }
            refPlugin_Click(null, null);

            cbList.Items.Add("NEW CHEAT CODE");
            cbList.Items[0].ForeColor = ncForeColor;
            cbList.Items[0].BackColor = ncBackColor;
            Codes[CodesCount].name = "NEW CHEAT CODE";
            Codes[CodesCount].state = false;
            cbSchAlign.SelectedIndex = 2;
            compBox.SelectedIndex = 0;
            dFileName = Application.StartupPath + "\\dump.txt";
            cbList.Items[0].Selected = true;
            cbList.Items[0].Selected = false;

            string[] a = { "00000000", "FFFFFFFC" };
            ListViewItem b = new ListViewItem(a);
            rangeView.Items.Add(b);
            UpdateMemArray();

            int ctrl = 0;
            for (ctrl = 0; ctrl < Controls.Count; ctrl++)
            {
                Controls[ctrl].BackColor = ncBackColor;
                Controls[ctrl].ForeColor = ncForeColor;
            }

            //Update all the controls on the tabs
            for (ctrl = 0; ctrl < TabControl.TabPages.Count; ctrl++)
            {
                TabControl.TabPages[ctrl].BackColor = ncBackColor;
                TabControl.TabPages[ctrl].ForeColor = ncForeColor;
                //Color each control in the tab too
                for (int tabCtrl = 0; tabCtrl < TabControl.TabPages[ctrl].Controls.Count; tabCtrl++)
                {
                    TabControl.TabPages[ctrl].Controls[tabCtrl].BackColor = ncBackColor;
                    TabControl.TabPages[ctrl].Controls[tabCtrl].ForeColor = ncForeColor;
                }
            }
            //Update all the controls on the tabs
            for (ctrl = 0; ctrl < TabCon.TabPages.Count; ctrl++)
            {
                TabCon.TabPages[ctrl].BackColor = ncBackColor;
                TabCon.TabPages[ctrl].ForeColor = ncForeColor;
                //Color each control in the tab too
                for (int tabCtrl = 0; tabCtrl < TabCon.TabPages[ctrl].Controls.Count; tabCtrl++)
                {
                    TabCon.TabPages[ctrl].Controls[tabCtrl].BackColor = ncBackColor;
                    TabCon.TabPages[ctrl].Controls[tabCtrl].ForeColor = ncForeColor;
                }
            }
        }
        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            isClosing = true;
            ConstantLoop = 2;
            if (PS3.GetCurrentAPI() == SelectAPI.ControlConsole)
                PS3.DisconnectTarget();
            connectedLbl.Text = "Disconnected";
            System.IO.File.Delete(dFileName);
            foreach (PluginForm pluginForm in Form1.pluginForm)
            {
                try
                {
                    pluginForm.Dispose();
                    pluginForm.Close();
                }
                catch
                {
                }
            }
           if (!(refPlugin.Text == "Close Plugins"))
                return;
          Global.Plugins.ClosePlugins();
        }

        #region >< Debugger
        #region "API Stuff"          
        public class CustomRTB : RichTextBox
        {
          [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetScrollPos(IntPtr hWnd, int nBar);
          [DllImport("user32.dll")]
            private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
            private const int SB_HORZ = 0x0;
            private const int SB_VERT = 0x1;
            public int HorizontalPosition
            {
                get { return GetScrollPos((IntPtr)Handle, SB_HORZ); }
                set { SetScrollPos((IntPtr)Handle, SB_HORZ, value, true); }
            }
            public int VerticalPosition
            {
                get { return GetScrollPos((IntPtr)this.Handle, SB_VERT); }
                set { SetScrollPos((IntPtr)this.Handle, SB_VERT, value, true); }
            }
        }
        #endregion

        private void startAutoDebug_CheckedChanged(object sender, EventArgs e)
        {
            AutoDebug.Start();
        }
        private void Attach_Click(object sender, EventArgs e)
        {
            try
            {
                if (PS3.AttachProcess())
                {
                    connectedLbl.Text = "Attached";
                    ConstantLoop = 1;
                    Attach.Enabled = false;
                    attached = true;

                  dispBytes = PS3.GetBytes((uint)curAddr, 4 * rowCount);
                }
                else
                {
                    connectedLbl.Text = "Error attaching process; no game started?";
                }
            }
            catch (Exception ex)
            {
                connectedLbl.Text = "Error attaching process; no game started?";
            }
        }
        private void Ps3Discon_Click(object sender, EventArgs e)
        {
            try
            {
                PS3.DisconnectTarget();
                ConstantLoop = 2;
                connectedLbl.Text = "Disconnected";
                processIDs = (uint[])null;
                attached = false;
                connected = false;
                Attach.Enabled = true;
                btnConnection.Enabled = true;
            }
            catch (Exception ex)
            {
     
                throw;
            }        
        }
        private void btnConnection_Click(object sender, EventArgs e)
        {
           connectedLbl.Text = "Connecting..";
            try
            {
                if (apiDLL == 0) //TMAPI
                {
                    if (PS3.ConnectTarget())
                    {                   
                        connected = true;                      
                        connectedLbl.Text = "Connected";
                    }
                }
                else //CCAPI
                {
                    IBArg[] ibArg = new IBArg[1];
                    ibArg[0].defStr = (IPAddrStr == "") ? "192.168.1.0" : IPAddrStr;
                    ibArg[0].label = "PS3 IP Address";
                    ibArg = CallIBox(ibArg);

                    if (ibArg == null)
                    {
                        connectedLbl.Text = "Cancelled Connecting";
                        return;
                    }

                    IPAddrStr = ibArg[0].retStr;
                    if (ibArg[0].retStr != "")
                    {
                        if (PS3.ConnectTarget(ibArg[0].retStr))
                        {
                            PS3.AttachProcess();
                            connected = true;
                            connectedLbl.Text = "Connected";
                        }
                    }
                }
                if (connected == false)
                {
                    connectedLbl.Text = "Failed to connect to PS3";
                    connected = false;
                }
            }
            catch
            {
                connectedLbl.Text = "Failed to connect to PS3";
                connected = false;
            }
        }
        private void setOffset_Click(object sender, EventArgs e)
        {
            int fromBase = 10;
            if (dexHexCombo.SelectedIndex == 0)
            {
                fromBase = 10;
            }
            else
            {
                fromBase = 0x10;
            }
            try
            {
                switch (comboByteLength.SelectedIndex)
                {
                    case 0:
                        PS3.Extension.WriteSByte(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), Convert.ToSByte(resultTxt.Text, fromBase));                      
                        return;
                    case 1:
                        PS3.Extension.WriteInt16(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), Convert.ToInt16(resultTxt.Text, fromBase));
                        return;
                    case 2:
                        PS3.Extension.WriteInt32(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), Convert.ToInt32(resultTxt.Text, fromBase));
                        return;
                    case 3:
                        PS3.Extension.WriteString(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""),16), resultTxt.Text);
                        hexCode.ForeColor = Color.Orange;
                        return;
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Unexpected error ocurred. Please check your Input.");
            }
        }             
        private void getOffset_Click(object sender, EventArgs e)
        {
            switch (comboByteLength.SelectedIndex)
            {
                case 0:
                    resultTxt.Text = Convert.ToString(PS3.Extension.ReadByte(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16)));
                    return;
                case 1:
                    resultTxt.Text = Convert.ToString(PS3.Extension.ReadUInt16(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16)));
                    return;
                case 2:
                    resultTxt.Text = Convert.ToString(PS3.Extension.ReadUInt32(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16)));
                    return;
                case 3:
                    resultTxt.Text = PS3.Extension.ReadString(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16));
                    return;
            }
        }
        private void minusHundred_Click(object sender, EventArgs e)
        {
            uint num = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16) - Convert.ToUInt32(jumpUint.Text.Replace("0x", ""), 16);
            startHex.Text = "0x" + Convert.ToString(Convert.ToInt64(num)).ToUpper();
         // debugShit();
            UpdateRTBs(true, true, true, true, true);
        }
        private void plusHundred_Click(object sender, EventArgs e)
        {
            uint num = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16) + Convert.ToUInt32(jumpUint.Text.Replace("0x", ""), 16);
            startHex.Text = "0x" + Convert.ToString(Convert.ToInt64(num)).ToUpper();
          //debugShit();
            UpdateRTBs(true, true, true, true, true);
        }
        private void AutoUpdateget_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoUpdateget.Checked)
            {
                autoUpdateSetTimer.Stop();
                autoUpdateGetTimer.Start();
            }
            else
            {
                autoUpdateGetTimer.Stop();
            }
        }
        private void AutoUpdateset_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoUpdateset.Checked)
            {
                autoUpdateGetTimer.Stop();
                autoUpdateSetTimer.Start();
            }
            else
            {
                autoUpdateSetTimer.Stop();
            }
        }
        private void startDebug_Click(object sender, EventArgs e)
        {
           // rtfBuilder();
              debugShit();
          //  UpdateRTBs(true, true, true, true, true);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int fromBase = 10;
            if (dexHexCombo.SelectedIndex == 0)
            {
                fromBase = 10;
            }
            else
            {
                fromBase = 16;
            }
            try
            {
                switch (comboByteLength.SelectedIndex)
                {
                    case 0:
                        PS3.Extension.WriteSByte(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), Convert.ToSByte(resultTxt.Text, fromBase));
                        return;
                    case 1:
                        PS3.Extension.WriteInt16(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), Convert.ToInt16(resultTxt.Text, fromBase));
                        return;
                    case 2:
                        PS3.Extension.WriteInt32(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), Convert.ToInt32(resultTxt.Text, fromBase));
                        return;
                    case 3:
                        PS3.Extension.WriteString(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16), resultTxt.Text);
                        return;
                }
                autoUpdateSetTimer.Start();
            }
            catch (Exception e1)
            {
                autoUpdateSetTimer.Stop();
                AutoUpdateget.Checked = false;
                MessageBox.Show("Unexpected error ocurred. Please check your Input.");
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            switch (comboByteLength.SelectedIndex)
            {
                case 0:
                    resultTxt.Text = Convert.ToString(PS3.Extension.ReadByte(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16)));
                    return;
                case 1:
                    resultTxt.Text = Convert.ToString(PS3.Extension.ReadUInt16(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16)));
                    return;
                case 2:
                    resultTxt.Text = Convert.ToString(PS3.Extension.ReadUInt32(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16)));
                    return;
                case 3:
                    resultTxt.Text = PS3.Extension.ReadString(Convert.ToUInt32(offsetTxt.Text.Replace("0x", ""), 16));//,0x10;
                    return;
            }
            autoUpdateGetTimer.Start();
        }
        private void AutoDebug_Tick(object sender, EventArgs e)
        {
            if (startAutoDebug.Checked)
            {
             // debugShit();
                UpdateRTBs(true, true, true, true, true);
            }
            else
            AutoDebug.Stop();
        }
        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }
        void UpdateRTBs(bool updateAddr, bool updateBytes, bool updateText, bool updateASM, bool color)
        {
            rowCount = Convert.ToInt32(bytesLegthText.Text);
            curAddr = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16);
            displayedBytes = PS3.GetBytes((uint)curAddr, 4 * rowCount);

            int[] oldStarts = { offsetsText.SelectionStart, hexCode.SelectionStart, asciiText.SelectionStart };
            int[] oldLens = { offsetsText.SelectionLength, hexCode.SelectionLength, asciiText.SelectionLength };
            bool[] isFocused = { offsetsText.Focused, hexCode.Focused, asciiText.Focused };
            if (updateAddr)
            {
                offsetsText.Enabled = false;
                string txt = "";
                for (int x = 0; x < rowCount; x++)
                    txt += (curAddr + (ulong)(x * bytesPerColumn)).ToString("X8") + "\n";
                offsetsText.Text = txt.Trim('\n');
                offsetsText.SelectAll();
                offsetsText.SelectionAlignment = HorizontalAlignment.Center;
                offsetsText.SelectionLength = 1;

                offsetsText.Enabled = true;
                if (isFocused[0])
                    offsetsText.Focus();
                offsetsText.SelectionStart = oldStarts[0];
                offsetsText.SelectionLength = oldLens[0];
            }
            if (updateBytes)
            {
                hexCode.Enabled = false;
                //hexCode.Rtf = rtfBuilder();
                hexCode.Text = FormatByteArray(displayedBytes);
                hexCode.SelectAll();
                hexCode.SelectionAlignment = HorizontalAlignment.Center;
                hexCode.SelectionColor = Color.SpringGreen;

                //Color changed values
                if (color)
                {
                    byte max = 255;
                    Color changed = Color.FromArgb(max - ForeColor.R, max - ForeColor.G, max - ForeColor.B);
                    invForeColor = changed;
                    for (int cnt = 0; cnt < dispBytes.Length; cnt++)
                    {
                        if (dispBytes[cnt] != displayedBytes[cnt])
                        {
                            int off = cnt * 2;
                            off += off / (bytesPerColumn * 2);
                            hexCode.SelectionStart = off;
                            hexCode.SelectionLength = 2;
                            hexCode.SelectionColor = changed;
                        }
                    }
                }
                hexCode.SelectionLength = 1;
                hexCode.Enabled = true;
                if (isFocused[1])
                    hexCode.Focus();
                hexCode.SelectionStart = oldStarts[1];
                hexCode.SelectionLength = oldLens[1];
                dispBytes = displayedBytes;               
            }
            if (updateText)
            {
                byte max = 255;
                byte low = 0;
                Color changed = Color.FromArgb(max - ForeColor.R, max - ForeColor.G, max - ForeColor.B); invForeColor = changed;

                asciiText.Enabled = false;           
                asciiText.Text = FormatText(displayedBytes);
                asciiText.SelectAll();
                asciiText.SelectionAlignment = HorizontalAlignment.Center;
                asciiText.SelectionLength = 1;
                asciiText.SelectionColor = changed;
                asciiText.Enabled = true;
                if (isFocused[2])
                    asciiText.Focus();
                asciiText.SelectionStart = oldStarts[2];
                asciiText.SelectionLength = oldLens[2];
            }
            if (updateASM)
            {
                while (asmBox.Items.Count < rowCount)
                    asmBox.Items.Add("illegal");
                byte[] mem = new byte[4];
                for (int x = 0; x < rowCount; x++)
                {
                    ulong newAddr = curAddr + (ulong)(x * asmbytesPerColumn);
                    Array.Copy(displayedBytes, x * asmbytesPerColumn, mem, 0, 4);
                    string ncCode = "0 " + newAddr.ToString("X8") + " " + BitConverter.ToString(mem).Replace("-", "") + "\n";
                    CodeWiz.ASMDisMode = 1; //Disables labels                    
                    string[] disCodes = CodeWiz.ASMDisassemble(ncCode).Replace("hexcode 0x00000000", "illegal").Split('\n');
                    string disCode = ParseCode(disCodes[disCodes.Length - 2]);
                    asmBox.Items[x] = disCode;
                    asmBox.ForeColor = Color.DarkRed;
                }
            }
            RefreshAll();
        }
       /*void UpdateWatchList()
        {
            foreach (DataGridViewRow row in watchList.Rows)
            {
                //Make sure it isn't being edited
                //Would be annoying to have it update on you
                if (!row.Cells[2].IsInEditMode)
                {
                    ulong addr = Convert.ToUInt64(row.Cells[0].Value.ToString(), 16);
                    string oldStr = row.Cells[2].Value.ToString();
                    byte[] data;

                    switch (row.Cells[1].Value.ToString())
                    {
                        case "char":
                            data = PS3.GetBytes((uint)addr, 1);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "short":
                            data = PS3.GetBytes((uint)addr, 2);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "int":
                            data = PS3.GetBytes((uint)addr, 4);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "long":
                            data = PS3.GetBytes((uint)addr, 8);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "string":
                            byte b;
                            string res = "";
                            while ((b = PS3.GetBytes((uint)addr, 1)[0]) != 0)
                            {
                                if (res.Length > 255)
                                    break;
                                res += ((char)b).ToString();
                                addr++;
                            }
                            row.Cells[2].Value = res;
                            break;
                        case "float":
                            data = PS3.GetBytes((uint)addr, 4);
                            row.Cells[2].Value = HexToFloat(data).ToString("0.000000");
                            break;
                    }

                    if (oldStr != row.Cells[2].Value.ToString())
                        row.DefaultCellStyle.ForeColor = invForeColor;
                    else
                        row.DefaultCellStyle.ForeColor = ForeColor;
                }
            }
        }*/
        private void debugShit()
        {
            offsetsText.Text = "";
            string str = startHex.Text.Substring(startHex.Text.Length - 1, 1);
            if (!"0".Equals(str))
            {
                startHex.Text = startHex.Text.Remove(startHex.Text.Length - 1, 1) + "0";
            }
            try
            {
               rowCount = Convert.ToInt32(bytesLegthText.Text);
               curAddr = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16);//,0x16

               byte[] displayedBytes = PS3.GetBytes((uint)curAddr, 4 * rowCount);
               byte[] bytes = PS3.GetBytes((uint) curAddr, Convert.ToInt32(bytesLegthText.Text));

                RTFBuilderbase builderbase = new RTFBuilder();
                builderbase.Font(RTFFont.Arial);
                builderbase.FontSize(20f);
                for (int i = 0; i <= displayedBytes.Length -1; i++)
                {
                    builderbase.Font(RTFFont.MSSansSerif);
                    builderbase.FontSize(20f);
                    if (dispBytes != null)
                    {
                        if (displayedBytes.Length == dispBytes.Length)
                        {
                            if (displayedBytes[i] == dispBytes[i])
                            {
                                builderbase.Append(displayedBytes[i].ToString("X2") + " ");
                            }
                            else
                            {
                                builderbase.ForeColor(KnownColor.Firebrick).Append(displayedBytes[i].ToString("X2") + " ");
                            }
                        }
                        else
                        {
                            builderbase.Append(displayedBytes[i].ToString("X2") + " ");
                        }
                    }
                    else
                    {
                        builderbase.Append(displayedBytes[i].ToString("X2") + " ");
                    }
                }
                dispBytes = displayedBytes;
                string str2 = builderbase.ToString();
                hexCode.Rtf = str2;
               // string ascii = FormatText(displayedBytes);
                string str3 = SpliceText(Encoding.Default.GetString(displayedBytes).Replace(Constants.vbNullChar, ".").Replace("\\a", ".").Replace(Constants.vbVerticalTab, ".").Replace(Constants.vbCr, ".").Replace(" ", ".").Replace(Constants.vbTab, ".").Replace(Constants.vbLf, ".").Replace(Constants.vbBack, ".").Replace(Constants.vbFormFeed, "."), 0x10);
                asciiText.Text = str3;
                int num4 = (hexCode.Text.Length / 0x30) + 1;
                for (int j = 0; j <= num4 - 1; j++)
                {                                                                                        //0x10
                    offsetsText.Text = offsetsText.Text + "0x" + Convert.ToString(Convert.ToInt64(curAddr), 16).ToUpper() + Environment.NewLine;
                    curAddr += 0x10;
                }
            }
            catch
            {
                startAutoDebug.Enabled = false;
                startAutoDebug.Checked = false;
                MessageBox.Show("FAILURE TO FIND Address!");
            }
        }
        private void JumpUp_Click(object sender, EventArgs e)
        {
            uint num = Convert.ToUInt32(startHex.Text.Replace("0x", ""),16) + Convert.ToUInt32(jumpUint.Text.Replace("0x", ""), 16);
            startHex.Text = "0x" + Convert.ToString(Convert.ToInt64(num)).ToUpper();
           // debugShit();
            UpdateRTBs(true, true,true,true,true);
        }
        private void JumpDown_Click(object sender, EventArgs e)
        {
            uint num = Convert.ToUInt32(startHex.Text.Replace("0x", ""),16) - Convert.ToUInt32(jumpUint.Text.Replace("0x", ""), 16);
            startHex.Text = "0x" + Convert.ToString(Convert.ToInt64(num)).ToUpper();
         //   debugShit();
            UpdateRTBs(true, true, true, true, true);
        }
        private void hexCode_TextChanged(object sender, EventArgs e)
        {
            hexCode.ForeColor = Color.OrangeRed;
        }
        [DllImport("user32.dll")]
        public static extern int GetScrollPos1(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private void asciiText_VScroll(object sender, EventArgs e)
        {
            int num = CustomRTB.GetScrollPos(asciiText.Handle, 1) << 0x10;
            uint num2 = Convert.ToUInt32(4 | num);
            SendMessage(hexCode.Handle, 0x115, new IntPtr(Convert.ToInt64(num2)), new IntPtr(0));
            SendMessage(offsetsText.Handle, 0x115, new IntPtr(Convert.ToInt64(num2)), new IntPtr(0));
        }
        private void hexCode_Text_click(object sender, EventArgs e)
        {
            dexHexCombo.SelectedIndex = 1;
            int index = hexCode.SelectionStart / 0x30;
            int num2 = (hexCode.SelectionStart - (index * 0x30)) / 3;
            string str = offsetsText.Lines.ElementAt(index);
            string str2 = num2.ToString("X");
            str = str.Remove(str.Length - 1, 1) + str2;
            offsetTxt.Text = str;
        }
        private void jumpUint_TextChanged(object sender, EventArgs e)
        {

        }
        private void offsetsText_TextChanged(object sender, EventArgs e)
        {

        }
        private void asciiText_TextChanged(object sender, EventArgs e)
        {

        }

        #region KeyHandlers
        void HandleRTBMove(RichTextBox rtb, int off)
        {
            if ((rtb.SelectionStart + off) == rtb.Text.Length)
                rtb.SelectionStart--;
            if (rtb.Text[(rtb.SelectionStart + off)] == '\n' && (rtb.SelectionStart + off) == (rtb.Text.Length - 1))
                rtb.SelectionStart--;
            if (rtb.Text[(rtb.SelectionStart + off)] == '\n' && (rtb.SelectionStart + off) < (rtb.Text.Length - 1))
                rtb.SelectionStart++;
        }
        void RefreshAll()
        {
            hexCode.Refresh();
            asciiText.Refresh();
            asmBox.Refresh();
            offsetsText.Refresh();
        }
        /*Parses out any pseudo ops (hard-coded)*/
        string ParseCode(string code)
        {
            if (code == null || code == "" || code == "nop" || code == "illegal")
                return code;
            string[] words = code.Split(' ');
            if (words[0] == "ori" && words[3] == "0x0000")
            {
                return "mr " + words[1] + " " + words[2].Replace(",", "");
            }
            else if (code == "hexcode 0x4E800420")
                return "bctr";
            else if (code == "hexcode 0x4E800421")
                return "bctrl";
            else if (code == "hexcode 0x44000002")
                return "sc";
            return code;
        }
        byte[] FloatToBA(float fltStr)
        {
            try
            {
                byte[] flt = BitConverter.GetBytes(fltStr);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(flt);
                return flt;
            }
            catch { }
            return new byte[0];
        }
        public static byte[] StringBAToBA(string str)
        {
            if (str == null || (str.Length % 2) == 1)
                return new byte[0];

            byte[] ret = new byte[str.Length / 2];
            for (int x = 0; x < str.Length; x += 2)
                ret[x / 2] = byte.Parse(misc.sMid(str, x, 2), NumberStyles.HexNumber);

            return ret;
        }
        Single HexToFloat(byte[] hexBA)
        {
            string hex = "";
            foreach (byte b in hexBA)
               hex += b.ToString("X2");
            try
            {
                return BitConverter.ToSingle((BitConverter.GetBytes(int.Parse(hex, NumberStyles.HexNumber))), 0);
            }
            catch {}
            return 0;
        }
        string FormatText(byte[] ba)
        {
            string retstr = "";     
            for (int y = 0; y < ba.Length; y += bytesPerColumn)
            {
                for (int x = 0; x < bytesPerColumn; x++)
                {
                    char chr = (char)ba[x + y];
                    if (chr < 0x20 || chr > 0x80)
                        chr = '.';
                    retstr += chr.ToString();
                }
                retstr += "\n";
            }
          return retstr.Trim('\n');
        }
        string rtfBuilder()
        {
            RTFBuilderbase builderbase = new RTFBuilder();
            builderbase.Font(RTFFont.Arial);
            builderbase.FontSize(20f);
            for (int i = 0; i <= displayedBytes.Length; i++)
            {
                builderbase.Font(RTFFont.MSSansSerif);
                builderbase.FontSize(20f);
                if (dispBytes != null)
                {
                    if (displayedBytes.Length == dispBytes.Length)
                    {
                        if (displayedBytes[i] == dispBytes[i])
                        {
                            builderbase.Append(displayedBytes[i].ToString("X2") + " ");
                        }
                        else
                        {
                            builderbase.ForeColor(KnownColor.Firebrick).Append(displayedBytes[i].ToString("X2") + " ");
                        }
                    }
                    else
                    {
                        builderbase.Append(displayedBytes[i].ToString("X2") + " ");
                    }
                }
                else
                {
                    builderbase.Append(displayedBytes[i].ToString("X2") + " ");
                }
                dispBytes = displayedBytes;
            }
            return builderbase.ToString();
        }
        string FormatByteArray(byte[] ba)
        {
            string retbyte = "";
            for (int y = 0; y < ba.Length; y += bytesPerColumn)
            {
                for (int x = 0; x < bytesPerColumn; x++)
                    retbyte += ba[x + y].ToString("X2");
                retbyte += "\n";
            }
            return retbyte.Trim('\n').ToUpper();
        }
        void CalculateChange(bool isText, char newChar, RichTextBox rtb)
        {
            curAddr = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16);

            int off = rtb.SelectionStart;
            bool isFocused = rtb.Focused;

            //Apply changes to window
            if (!isText && newChar > (int)'F')
                newChar -= (char)0x20;
            char[] chr = rtb.Text.ToCharArray();
            chr[off] = newChar;
            string text = "";
            foreach (char c in chr)
                text += c.ToString();

            rtb.Enabled = false;
            rtb.Text = text;
            rtb.SelectAll();
            rtb.SelectionAlignment = HorizontalAlignment.Center;
            rtb.SelectionLength = 1;
            rtb.Enabled = true;
            if (isFocused)
                rtb.Focus();
            rtb.SelectionStart = off;

            //Apply changes to memory
            if (!isText)
            {
                int offLine = off - (off / (bytesPerColumn * 2 + 1));
                string byteStr = "";
                if ((offLine % 2) == 1) //End of byte
                    byteStr = chr[off - 1].ToString() + chr[off].ToString();
                else
                    byteStr = chr[off].ToString() + chr[off + 1].ToString();
                ulong addr = curAddr + (ulong)(offLine / 2);
                PS3.SetMemory((uint)addr, StringBAToBA(byteStr));
            }
            else
            {
                int byteOff = off - (off / (bytesPerColumn + 1));
                ulong addr = curAddr + (ulong)byteOff;
                PS3.SetMemory((uint)addr, new byte[] { (byte)newChar });
            }
            UpdateRTBs(true, true, true, true, true);
        }
        void rtbMouseUp(object sender, MouseEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;

            if (rtb.SelectionLength < 1)
                rtb.SelectionLength = 1;
        }
        void rtbOnMouseDown(object sender, MouseEventArgs e)
        {
            (sender as RichTextBox).AutoWordSelection = true;
            (sender as RichTextBox).AutoWordSelection = false;
        }
        void rtbKeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            HandleRTBMove(rtb, 0);
            e.Handled = true;

            int colSize = bytesPerColumn;
            bool isText = rtb.Tag.ToString() == "TEXT";
            if (isText)
                colSize /= 2;

            if (e.KeyCode == Keys.C && e.Control)
                CopySelected();
            if (e.Control)
                return;

            char newChar = char.ConvertFromUtf32((int)e.KeyCode)[0];
            if (newChar >= 0x61 && newChar <= (int)'z' && !e.Shift)
                newChar -= (char)0x20;
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 || ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)) && !isText)
            {
                CalculateChange(isText, newChar, rtb);

                if (rtb.SelectionStart == (rtb.Text.Length - 1))
                {
                    curAddr += 4;
                    rtb.SelectionStart -= bytesPerColumn * 2 + 1;
                    UpdateRTBs(true, true, true, true, true);
                }
                rtb.SelectionStart++;
                HandleRTBMove(rtb, 0);
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                if (rtb.SelectionStart > 0)
                {
                    rtb.SelectionStart--;
                    if (rtb.Text[rtb.SelectionStart] == '\n')
                        rtb.SelectionStart--;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (rtb.SelectionStart < (rtb.Text.Length - 1))
                {
                    rtb.SelectionStart++;
                    if (rtb.Text[rtb.SelectionStart] == '\n')
                        rtb.SelectionStart++;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (rtb.SelectionStart > (colSize * 2))
                {
                    rtb.SelectionStart -= (colSize * 2 + 1);
                }
                else
                {
                    curAddr -= (ulong)bytesPerColumn;
                    UpdateRTBs(true, true, true, true, true);
                    Application.DoEvents();
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (rtb.SelectionStart < (rtb.Text.Length - (colSize * 2 + 1)))
                {
                    rtb.SelectionStart += (colSize * 2 + 1);
                }
                else
                {
                    curAddr += (ulong)bytesPerColumn;
                    UpdateRTBs(true, true, true, true, true);
                    Application.DoEvents();
                }
            }
            else if (isText && (newChar > 0x20 && newChar < 0x80))
            {
                CalculateChange(isText, newChar, rtb);

                if (rtb.SelectionStart == (rtb.Text.Length - 1))
                {
                    curAddr += 4;
                    rtb.SelectionStart -= bytesPerColumn + 1;
                    UpdateRTBs(true, true, true, true, true);
                }
                rtb.SelectionStart++;
                HandleRTBMove(rtb, 0);
            }
          (sender as RichTextBox).SelectionLength = 1;
        }
        void rtbTextChanged(object sender, EventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            HandleRTBMove(rtb, 0);
            rtb.SelectionLength = 1;
        }
        void rtbKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        void listBoxLoseFocus(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            oldASMSelIndex = lb.SelectedIndex;
            lb.SelectedIndex = -1;
        }
        void listBoxKeyDown(object sender, KeyEventArgs e)
        {
            curAddr = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16);
            if (e.KeyCode == Keys.Down && asmBox.SelectedIndex == (rowCount - 1))
            {            
                curAddr += (ulong) bytesPerColumn;
                UpdateRTBs(true, true, true, true, true);
            }
            else if (e.KeyCode == Keys.Up && asmBox.SelectedIndex == 0)
            {
                curAddr -= (ulong)bytesPerColumn;
                UpdateRTBs(true, true, true, true, true);
            }
            else if (e.KeyCode == Keys.C && e.Control)
             CopySelected();
        }
        void CopySelected()
        {
            string res = "";
            curAddr = Convert.ToUInt32(startHex.Text.Replace("0x", ""), 16);

            if (offsetsText.Focused)
            {
                res = misc.sMid(offsetsText.Text, offsetsText.SelectionStart, offsetsText.SelectionLength);
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (hexCode.Focused)
            {
                int off = hexCode.SelectionStart, len = hexCode.SelectionLength;
                int byteOff = off - (off / (bytesPerColumn * 2));
                if ((byteOff % 2) != 0)
                {
                    off--;
                    len++;
                }
                if ((len / (bytesPerColumn * 2)) % 2 == 0)
                    len++;
                byteOff = off - (off / (bytesPerColumn * 2));
                ulong addr = curAddr + (ulong)(byteOff / 2);

                res = "0 " + addr.ToString("X8") + " " + misc.sMid(hexCode.Text, off, len).Replace("\n", "");
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (asciiText.Focused)
            {
                int byteOff = asciiText.SelectionStart - (asciiText.SelectionStart / (bytesPerColumn + 1));
                ulong addr = curAddr + (ulong)(byteOff);
                res = "1 " + addr.ToString("X8") + " " + misc.sMid(asciiText.Text, asciiText.SelectionStart, asciiText.SelectionLength).Replace("\n", "");

                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (asmBox.Focused)
            {
                res = asmBox.Items[asmBox.SelectedIndex].ToString();
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
        }
        #endregion

        #endregion

        #region NetCheat
        private void TabCon_KeyUp(object sender, KeyEventArgs e)
        {
           if (!ProcessKeyBinds(e.KeyData))
              return;
          e.SuppressKeyPress = true;
        }
        private bool ProcessKeyBinds(Keys data)
        {
            int num = -1;
            for (int index = 0; index < keyBinds.Length; ++index)
            {
                if (keyBinds[index].Equals((object)data))
                  num = index;
            }
            if (num < 0)
                return false;
            switch (num)
            {
                case 0:
                    btnConnection_Click((object)null, (EventArgs)null);
                    btnConnection_Click((object)null, (EventArgs)null);
                    if (connected)
                    {
                        Attach_Click((object)null, (EventArgs)null);
                        break;
                    }
                    else
                        break;
                case 1:
                    Ps3Discon_Click((object)null, (EventArgs)null);
                    break;
                case 2:
                    if (schSearch.Text == "Stop")
                        schSearch_Click((object)null, (EventArgs)null);
                    if (schNSearch.Text == "Cancel")
                        schNSearch_Click((object)null, (EventArgs)null);
                    schSearch.Text = "Initial Scan";
                    schSearch_Click((object)null, (EventArgs)null);
                    break;
                case 3:
                    if (schSearch.Text == "Stop")
                        schSearch_Click((object)null, (EventArgs)null);
                    if (schNSearch.Text == "Cancel")
                        schNSearch_Click((object)null, (EventArgs)null);
                    schNSearch.Text = "Next Scan";
                    schNSearch_Click((object)null, (EventArgs)null);
                    break;
                case 4:
                    if (schSearch.Text == "Stop")
                        schSearch_Click((object)null, (EventArgs)null);
                    if (schNSearch.Text == "Cancel")
                    {
                        schNSearch_Click((object)null, (EventArgs)null);
                        break;
                    }
                    else
                        break;
                case 5:
                    RefreshSearchResults(0);
                    break;
                case 6:
                    if (cbListIndex >= 0 && cbListIndex < cbList.Items.Count)
                    {
                        cbList_DoubleClick((object)null, (EventArgs)null);
                        break;
                    }
                    else
                        break;
                case 7:
                    if (cbListIndex >= 0 && cbListIndex < cbList.Items.Count)
                    {
                        cbWrite_Click((object)null, (EventArgs)null);
                        break;
                    }
                    else
                        break;
            }
            return true;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde)
            {
                if (!ProcessKeyBinds(e.KeyData))
                    return;
             e.SuppressKeyPress = true;
            }          
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void CopyResults()
        {
            int x = 0, max = lvSch.Items.Count - 1;
            String text = "", res = "";

            for (x = 0; x <= max; x++)
            {

                while (x <= max && lvSch.Items[x].Selected == false)
                    x++;

                if (x > max)
                    break;

                CodeRes a = fileio.ReadDump(dFileName, x, (int)GlobAlign);

                int align = (int)GlobAlign;
                if ((int)GlobAlign == -2)
                    align = a.val.Length;

                if (align > 0 && align <= 8)
                {
                    a.val = BitConverter.GetBytes(uint.Parse(lvSch.Items[x].SubItems[1].Text, System.Globalization.NumberStyles.HexNumber));
                    if (a.val == null)
                        return;
                    if (BitConverter.IsLittleEndian) Array.Reverse(a.val);
                }

                switch (align)
                {
                    case 1:
                        text = "0 " + a.addr.ToString("X8") + " ";
                        text += a.val[3].ToString("X2") + "000000";
                        break;
                    case 2:
                        text = "1 " + a.addr.ToString("X8") + " ";
                        text = text + a.val[2].ToString("X2") + a.val[3].ToString("X2") + "0000";
                        break;
                    case 4:
                        text = "2 " + a.addr.ToString("X8") + " ";
                        text = text + (a.val[0].ToString("X2") + a.val[1].ToString("X2") + a.val[2].ToString("X2") + a.val[3].ToString("X2")).PadLeft(8, '0');
                        break;
                    case 8:
                        text = "2 " + a.addr.ToString("X8") + " ";
                        text = text + (a.val[0].ToString("X2") + a.val[1].ToString("X2") + a.val[2].ToString("X2") + a.val[3].ToString("X2")).PadLeft(8, '0') + "\n";
                        text = text + "2 " + (a.addr + 4).ToString("X8") + " ";
                        text = text + (a.val[4].ToString("X2") + a.val[5].ToString("X2") + a.val[6].ToString("X2") + a.val[7].ToString("X2")).PadLeft(8, '0');
                        break;
                    default:
                        text = "2 " + a.addr.ToString("X8") + " ";
                        break;
                }
                res = res + text + "\n";
            }
            if (res != null)
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
        }
        private void RefreshFromDump()
        {
            Form1.SchResCnt = 0;
            Form1.GlobAlign = 0;
            bool update = !schNSearch.Enabled;
            RefreshSearchResults(0);
            if (Form1.SchResCnt != 0 && update)
            {
                /* Setup CompBox */
                compBox.Items.Add("Increased By");
                compBox.Items.Add("Decreased By");
                compBox.Items.Add("Changed Value");
                compBox.Items.Add("Unchanged Value");
                NewSearch = false;
                schSearch.Text = "New Scan";
                schNSearch.Enabled = true;
            }
        }
        private void DeleteSearchResult()
        {
            int[] selectedItems = new int[0];
            int x = 0;
            /* Add selected items to int array */
            for (x = 0; x < lvSch.Items.Count; x++)
            {
                if (lvSch.Items[x].Selected)
                {
                    Array.Resize(ref selectedItems, selectedItems.Length + 1);
                    selectedItems[selectedItems.Length - 1] = x;
                }
            }
            /* Delete selected items */
            if (selectedItems.Length == 0)
                return;
            fileio.DeleteDumpBlock(dFileName, selectedItems);
            RefreshSearchResults(0);
        }
        public void RefreshSearchResults(int mode)
        {
            ulong x = 0;
            bool reloadDump = false;
            ListView.ListViewItemCollection items = new ListView.ListViewItemCollection(lvSch);
            //lvSch.Items.

            switch (mode)
            {
                case 0: /* Refresh by reloading everything from the dump */
                    lvSch.Items.Clear();

                    if (Form1.SchResCnt == 0)
                        reloadDump = true;

                    if (reloadDump)
                    {
                        lvSch.BeginUpdate();
                        while (true)
                        {
                            String Addr = "";

                            CodeRes[] retRes = fileio.ReadDumpArray(dFileName, (int)x, (MaxRes - 1) + (int)x, (int)GlobAlign);
                            if (retRes == null)
                                return;

                            GlobAlign = (ulong)retRes[0].align;
                            NextSAlign = (int)retRes[0].align;

                            for (int z = 0; z < retRes.Length; z++)
                            {

                                if (retRes[z].val == null)
                                    goto nextZ;

                                ListRes a = misc.GetlvVals((int)GlobAlign, retRes[z].val, 0);

                                Addr = (retRes[z].addr).ToString("X8");

                                string[] row = { Addr, a.HexVal, a.DecVal, a.AlignStr };
                                var listViewItem = new ListViewItem(row);
                                //lvSch.Items.Add(listViewItem);
                                items.Add(listViewItem);

                            nextZ:
                                if ((x % 1000) == 0)
                                {
                                    StatusLabel.Text = "Results: " + x.ToString();
                                    Application.DoEvents();
                                }
                                if (retRes[z].val == null && reloadDump)
                                {
                                    Form1.SchResCnt = x;
                                    StatusLabel.Text = "Results: " + x.ToString();
                                    lvSch.EndUpdate();
                                    NewSearch = false;
                                    schNSearch.Enabled = true;
                                    return;
                                }
                                x++;
                            }
                        }
                    }
                    else
                    {
                        lvSch.BeginUpdate();
                        //for (x = 0; x < Form1.SchResCnt; x++)
                        while (x < Form1.SchResCnt)
                        {
                            String Addr = "";

                            CodeRes[] retRes = fileio.ReadDumpArray(dFileName, (int)x, (MaxRes - 1) + (int)x, (int)GlobAlign);
                            if (retRes == null)
                                return;

                            if ((int)x < retRes.Length)
                            {
                                GlobAlign = (ulong)retRes[x].align;
                                NextSAlign = (int)retRes[x].align;
                            }

                            int z = 0;
                            for (z = 0; z < retRes.Length; z++)
                            {
                                if (retRes[z].val == null)
                                    goto nextZ;

                                ListRes a = misc.GetlvVals((int)GlobAlign, retRes[z].val, 0);

                                Addr = (retRes[z].addr).ToString("X8");

                                string[] row = { Addr, a.HexVal, a.DecVal, a.AlignStr };
                                var listViewItem = new ListViewItem(row);
                                items.Add(listViewItem);

                            nextZ:
                                if ((z % 1000) == 0)
                                    Application.DoEvents();
                            }
                            x += (ulong)z;
                        }
                        lvSch.EndUpdate();
                    }

                    break;
                case 1: /* Refresh everything by grabbing the values from the PS3 */
                    lvSch.BeginUpdate();
                    for (x = 0; x < Form1.SchResCnt; x++)
                    {
                        String Addr = "";
                        int align = (int)GlobAlign;

                        CodeRes[] retRes = fileio.ReadDumpArray(dFileName, (int)x, MaxRes + (int)x, (int)GlobAlign);
                        if (retRes == null)
                            return;

                        for (int z = 0; z < retRes.Length; z++)
                        {

                            if (x >= Form1.SchResCnt)
                                break;

                            if ((int)GlobAlign == -1 || (int)GlobAlign == -2)
                                align = retRes[z].val.Length;

                            byte[] ret = new byte[align];
                            apiGetMem(retRes[z].addr, ref ret);

                            ListRes a = new ListRes();
                            a = misc.GetlvVals((int)GlobAlign, ret, 0);

                            Addr = (retRes[z].addr).ToString("X8");

                            if ((int)x < lvSch.Items.Count)
                            {
                                items[(int)x].SubItems[0].Text = Addr;
                                items[(int)x].SubItems[1].Text = a.HexVal;
                                items[(int)x].SubItems[2].Text = a.DecVal;
                                items[(int)x].SubItems[3].Text = a.AlignStr;
                            }
                            else
                            {
                                string[] row = { Addr, a.HexVal, a.DecVal, a.AlignStr };
                                var listViewItem = new ListViewItem(row);
                                items.Add(listViewItem);
                            }


                            x++;
                            if ((x % 500) == 0)
                                Application.DoEvents();
                        }
                    }
                    lvSch.EndUpdate();
                    break;
            }
        }
        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbListIndex = FindLI();
            if (cbListIndex < 0)
                cbListIndex = 0;

            UpdateCB(cbListIndex);
            cbCodes.BackColor = BackColor;
            cbCodes.ForeColor = ForeColor;
            cbCodes.SelectionStart = 0;
            cbCodes.SelectionLength = cbCodes.Text.Length;
            cbCodes.SelectionColor = ForeColor;
            cbCodes.SelectionLength = 0;
        }
        private void cbAdd_Click(object sender, EventArgs e)
        {
            cbList.Items.Add("NEW CODE");
            CodesCount = cbList.Items.Count - 1;
            cbList.Items[CodesCount].ForeColor = ncForeColor;
            cbList.Items[CodesCount].BackColor = ncBackColor;
            Codes[CodesCount].name = "NEW CODE";
            Codes[CodesCount].state = false;
            Codes[CodesCount].codes = "";
        }
        private void cbRemove_Click(object sender, EventArgs e)
        {
            int ind = cbListIndex, x = 0;
            if (ind < 0)
                return;
            cbList.Items[ind].Remove();
            for (x = ind; x <= (cbList.Items.Count - 1); x++)
            {
                Codes[x].codes = Codes[x + 1].codes;
                Codes[x].name = Codes[x + 1].name;
                Codes[x].state = Codes[x + 1].state;
            }
            Codes[x].codes = "NEW CODE";
            Codes[x].name = "";
            Codes[x].state = false;

            if (cbListIndex >= (cbList.Items.Count - 1))
                cbListIndex = cbList.Items.Count - 1;
            UpdateCB(cbListIndex);
        }
        private void cbImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "List files (*.ncl)|*.ncl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                CodeDB[] ret = fileio.OpenFile(fd.FileName);
                if (ret == null)
                    return;
                int cnt = 0;
                if (cbListIndex >= 0)
                    cbList.Items[cbListIndex].Selected = false;
                for (int x = 0; x < ret.Length; x++)
                {
                    if (ret[x].name == null)
                        break;
                    ret[x].filename = fd.FileName;
                    if (ret[x].state)
                        cbList.Items.Add("+ " + ret[x].name);
                    else
                        cbList.Items.Add(ret[x].name);
                    cnt = cbList.Items.Count - 1;
                    cbList.Items[cnt].ForeColor = Color.FromArgb(0, 130, 210);
                    cbList.Items[cnt].BackColor = Color.Black;
                    Codes[cnt] = ret[x];
                }
                CodesCount = cnt;
                cbList.Items[cnt].Selected = true;
            }
        }
        private void cbSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "List files (*.ncl)|*.ncl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileio.SaveFile(fd.FileName, Codes[cbListIndex]);
                Codes[cbListIndex].filename = fd.FileName;
            }
        }
        private void cbSaveAll_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "List files (*.ncl)|*.ncl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileio.SaveFileAll(fd.FileName);
            }
        }
        private void cbSave_Click(object sender, EventArgs e)
        {
            fileio.SaveFile(Codes[cbListIndex].filename, Codes[cbListIndex]);
        }
        private void cbWrite_Click(object sender, EventArgs e)
        {
            codes.WriteToPS32(Codes[cbListIndex]);
        }
        /* Toggles whether the code is constant writing or not */
        private void cbName_TextChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;
            Codes[ind].name = cbName.Text;
            if (Codes[ind].state == true)
            {
                cbList.Items[ind].Text = "+ " + cbName.Text;
            }
            else
            {
                cbList.Items[ind].Text = cbName.Text;
            }
        }
        private void cbList_DoubleClick(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;
            if (Codes[ind].state == true)
            {
                cbList.Items[ind].Text = Codes[ind].name;
                Codes[ind].state = false;
            }
            else
            {
                cbList.Items[ind].Text = "+ " + Codes[ind].name;
                Codes[ind].state = true;
                ConstantLoop = 1;
            }
            Application.DoEvents();
            UpdateCB(ind);
        }
        private void ConstantWrite_CheckedChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;
            if (ConstantWrite.Checked)
                cbList.Items[ind].Text = "+ " + Codes[ind].name;
            else
                cbList.Items[ind].Text = Codes[ind].name;
            Codes[ind].state = ConstantWrite.Checked;
            ConstantLoop = 1;
        }
        private void cbCodes_TextChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;
            CodesCount = cbList.Items.Count - 1;
            Codes[ind].codes = cbCodes.Text;
            codes.UpdateCData(Codes[ind].codes, ind);
        }
        private void cbCodes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Shift == false && e.Control)
            {
                cbCodes.SelectionStart = 0;
                cbCodes.SelectionLength = cbCodes.Text.Length;
            }
        }
        private void cbCodes_KeyUp(object sender, KeyEventArgs e)
        {

        }
        private void lvSch_KeyUp(object sender, KeyEventArgs e)
        {
            //Refresh
            if (e.KeyCode == Keys.R && e.Control)
            {
                RefreshFromDump();
            }
            //Copy
            else if (e.KeyValue == 67)
            {
                CopyResults();
            }
        }
        bool hexSchHexVal = true;
        bool textSchHexVal = false;
        private void SearchBtnCode(ulong startRange, ulong stopRange, bool islast = true)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (rangesBox.Checked)
                rangeView.Enabled = false;
            rangesBox.Enabled = false;
            schRange1.Enabled = false;
            schRange2.Enabled = false;
            schRange1.Text = startRange.ToString("X8");
            schRange2.Text = stopRange.ToString("X8");
            int align = 4;
            if (schSearch.Text == "Initial Scan" && System.IO.File.Exists(dFileName))
                System.IO.File.Delete(dFileName);
            if (schSearch.Text == "Stop" || schSearch.Text == "Skip")
            {
                for (int index = 0; index < 4; ++index)
                {
                    if (compBox.Items.Count > 7)
                        compBox.Items.RemoveAt(compBox.Items.Count - 1);
                }
                if (compBox.SelectedIndex < 0)
                    compBox.SelectedIndex = 0;
                CancelSearch = 2;
                if (!SchPWS.Checked || !this.SchPWS.Visible)
                    return;
                PS3Lib.PS3TMAPI.ProcessContinue(0, TMAPI.Parameters.ProcessID);
            }
            else if (schRange1.Text.Length != 8 || this.schRange2.Text.Length != 8)
            {
                MessageBox.Show("Error: range addresses are not of proper length 8.");
            }
            else
            {
                if (SchPWS.Checked && SchPWS.Visible && schSearch.Text != "New Scan")
                {
                    PS3Lib.PS3TMAPI.ProcessAttach(0, PS3Lib.PS3TMAPI.UnitType.PPU, TMAPI.Parameters.ProcessID);
                }
                switch (cbSchAlign.SelectedItem.ToString())
                {
                    case "1 byte":
                        align = 1;
                        break;
                    case "2 bytes":
                        align = 2;
                        break;
                    case "4 bytes":
                        align = 4;
                        break;
                    case "8 bytes":
                        align = 8;
                        break;
                    case "X bytes":
                        align = -2;
                        break;
                    case "Text":
                        align = -1;
                        break;
                    case "Float byte":
                        align = 4;
                        break;
                }
                string str1 = "";
                string str2 = "";
                if (align > 0 || align == -2)
                {
                    if (ValHex)
                    {
                        if (align > 0)
                        {
                            str1 = schVal.Text.PadLeft(align * 2, '0');
                            schVal.Text = str1;
                            if (schVal2.Visible)
                            {
                                str2 = schVal2.Text.PadLeft(align * 2, '0');
                                schVal2.Text = str2;
                            }
                        }
                        else if (align == -2)
                        {
                            str1 = schVal.Text;
                            if (schVal2.Visible)
                                str2 = schVal2.Text;
                        }
                    }
                    else
                    {
                        str1 = ulong.Parse(schVal.Text).ToString("X").PadLeft(align * 2, '0');
                        if (schVal2.Visible)
                            str2 = ulong.Parse(schVal2.Text).ToString("X").PadLeft(align * 2, '0');
                    }
                }
                byte[] numArray = (byte[])null;
                byte[] c = (byte[])null;
                if (align > 0)
                {
                    numArray = new byte[align];
                }
                else
                {
                    switch (align)
                    {
                        case 10:
                            numArray = new byte[schVal.Text.Length];
                            break;
                        case -2:
                            numArray = new byte[schVal.Text.Length / 2];
                            break;
                        case -1:
                            numArray = new byte[schVal.Text.Length];
                            break;
                    }
                }
                switch (align)
                {
                    case 10:
                        numArray = misc.StringBAToBA(schVal.Text);
                        break;
                    case -2:
                        numArray = misc.StringBAToBA(schVal.Text);
                        break;
                    case -1:
                        numArray = misc.StringToByteArray(schVal.Text);
                        break;
                    case 8:
                        numArray = BitConverter.GetBytes(long.Parse(misc.ReverseE(str1.PadLeft(align * 2, '0'), align * 2), NumberStyles.HexNumber));
                        break;
                    default:
                        if (align > 0)
                        {
                            numArray = misc.StringBAToBA(str1);
                            break;
                        }
                        else
                            break;
                }
                ulong num3 = stopRange - startRange;
                if (schVal2.Visible)
                    c = align == -2 ? misc.StringBAToBA(str2) : misc.StringBAToBA(str2);

                if ((long)num3 <= 0L)
                {
                   MessageBox.Show("Error: range addresses are incompatible.");
                }
                else if (compMode >= compBox.Items.Count)
                {
                    compMode = 0;
                    compBox.SelectedIndex = 0;
                }
                else if (NewSearch)
                {
                    compBox.Items.Add((object)"Increased By");
                    compBox.Items.Add((object)"Decreased By");
                    compBox.Items.Add((object)"Changed Value");
                    compBox.Items.Add((object)"Unchanged Value");
                    if (islast)
                        NewSearch = false;
                    schSearch.Text = !rangesBox.Checked || rangeView.SelectedItems.Count <= 1 ? "Stop" : "Skip";
                    ulong num5 = 65536UL;
                    ulong num6 = (ulong)misc.ParseRealDif(startRange, stopRange, num5);
                    schProg.Value = 0;
                    schProg.Maximum = (int)num6;
                    SchResCnt = 0UL;
                    int length = schVal.Text.Length;
                    if (!SchHexCheck.Checked)
                        length = numArray.Length;
                    NextSAlign = align;
                    if (stopRange - startRange < num5)
                        num5 = stopRange - startRange;
                    Application.DoEvents();
                    using (StreamWriter fStream = new StreamWriter(dFileName, true))
                    {
                        lvSch.BeginUpdate();
                        ulong addr = startRange;
                        while (num6 > 0UL)
                        {
                            ulong num7 = addr;
                            ulong sStart = misc.ParseSchAddr(addr);
                            if (sStart < stopRange)
                            {
                                if ((long)sStart == 0L)
                                {
                                    if ((long)num7 != 0L)
                                        break;
                                }
                                ulong num8 = 0UL;
                                if (stopRange - sStart < num5)
                                    num5 = stopRange - sStart;
                                if (align > 0)
                                {
                                    num8 = InitSearch(sStart, numArray, c, align, num5, fStream);
                                }
                                else
                                {
                                    switch (align)
                                    {
                                        case -2:
                                         num8 = InitSearchText(sStart, numArray, c, length, num5, fStream);
                                         break;

                                       case -1:
                                        num8 = InitSearchText(SchHexCheck.Checked, sStart, numArray, c, length * 2, num5, fStream);
                                         break;
                                    }
                                }
                                if (num8 > 0UL)
                                {
                                    lvSch.EndUpdate();
                                    Application.DoEvents();
                                    lvSch.BeginUpdate();
                                }
                                SchResCnt += num8;
                                if (CancelSearch == 1)
                                {
                                    NewSearch = true;
                                    schSearch.Text = "Initial Scan";
                                    schNSearch.Enabled = false;
                                    SchRef.Enabled = false;
                                    schProg.Maximum = 0;
                                    schProg.Value = 0;
                                    SchResCnt = 0UL;
                                    lvSch.Items.Clear();
                                    CancelSearch = 0;
                                    lvSch.EndUpdate();
                                    return;
                                }
                                else if (CancelSearch != 2)
                                {
                                    if (schProg.Value + 1 < schProg.Maximum)
                                        ++schProg.Value;
                                    --num6;
                                    Application.DoEvents();
                                    addr = sStart + num5;
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                    }
                    TotalResults += (uint)SchResCnt;
                    StatusLabel.Text = "Results found: " + TotalResults.ToString();
                    stopwatch.Stop();
                    TotalTime += (float)stopwatch.ElapsedMilliseconds / 1000f;
                    Label stripStatusLabel = StatusLabel;
                    string str3 = stripStatusLabel.Text + ", Search time: " + TotalTime.ToString("F") + " seconds";
                    stripStatusLabel.Text = str3;
                    schSearch.Text = "New Scan";
                    if (islast)
                    {
                        if ((int)TotalResults == 0)
                        {
                            schSearch.Text = "Initial Scan";
                            NewSearch = true;
                            if (System.IO.File.Exists(dFileName))
                                System.IO.File.Delete(dFileName);
                            lvSch.Items.Clear();
                            rangesBox.Enabled = true;
                            rangeView.Enabled = true;
                            if (!rangesBox.Checked)
                            {
                                schRange1.Enabled = true;
                                schRange2.Enabled = true;
                            }
                            SchRef.Enabled = false;
                            schNSearch.Enabled = false;
                            schProg.Maximum = 0;
                            schProg.Value = 0;
                            lvSch.EndUpdate();
                            return;
                        }
                        else
                        {
                            TotalResults = 0U;
                            TotalTime = 0.0f;
                        }
                    }
                    schProg.Maximum = 0;
                    if (islast)
                        NewSearch = false;
                    schNSearch.Enabled = true;
                    SchRef.Enabled = true;
                    schProg.Value = 0;
                    GlobAlign = (ulong)align;
                    if (SchPWS.Checked && SchPWS.Visible)
                    {
                        PS3TMAPI.ProcessContinue(0, TMAPI.Parameters.ProcessID);
                    }
                    CancelSearch = 0;
                    lvSch.EndUpdate();
                }
                else
                {
                    if (compBox.Items[compBox.Items.Count - 1].ToString() == "Unchanged Value")
                    {
                        for (int index = 0; index < 4; ++index)
                        {
                            if (compBox.Items.Count > 7)
                                compBox.Items.RemoveAt(compBox.Items.Count - 1);
                        }
                        if (compBox.SelectedIndex < 0)
                            compBox.SelectedIndex = 0;
                    }
                    if (islast)
                    {
                        NewSearch = true;
                        if (System.IO.File.Exists(dFileName))
                            System.IO.File.Delete(dFileName);
                    }
                    lvSch.Items.Clear();
                    schSearch.Text = "Initial Scan";
                    rangesBox.Enabled = true;
                    rangeView.Enabled = true;
                    if (!rangesBox.Checked)
                    {
                        schRange1.Enabled = true;
                        schRange2.Enabled = true;
                    }
                  SchRef.Enabled = false;
                  schNSearch.Enabled = false;
                }
            }
        }
        private void SearchMode(int mode)
        {
            if (mode != 5 && SchHexCheck.Text == "MCase")
            {
                SchHexCheck.Text = "Hex";
                textSchHexVal = SchHexCheck.Checked;
                SchHexCheck.Checked = hexSchHexVal;
                SchHexCheck.Font = Font;
            }
            switch (mode)
            {
                case 0: //1 byte
                    SchHexCheck.Visible = true;
                    break;
                case 1: //2 bytes
                    SchHexCheck.Visible = true;
                    break;
                case 2: //4 bytes
                    SchHexCheck.Visible = true;
                    break;
                case 3: //8 bytes
                    SchHexCheck.Visible = true;
                    break;
                case 4: //X bytes
                    SchHexCheck.Visible = true;
                    break;
                case 5: //Text
                    hexSchHexVal = SchHexCheck.Checked;
                    SchHexCheck.Checked = textSchHexVal;
                    SchHexCheck.Visible = true;
                    SchHexCheck.Text = "MCase";
                    SchHexCheck.Font = new Font(Font.FontFamily, 6.75f, FontStyle.Regular);
                    break;
                case 6: //Float
                    SchHexCheck.Visible = true;

                    break;
            }
        }
        public ulong InitSearch(ulong sStart, byte[] sVal, byte[] c, int align, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];
            apiGetMem(sStart, ref ret);
            while (sCount < sSize)
            {
                byte[] argB = new byte[align];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sVal, argB, c, compMode))
                {
                    ListRes a = misc.GetlvVals(align, ret, (int)sCount);
                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr };
                    var listViewItem = new ListViewItem(row);
                    lvSch.Items.Add(listViewItem);
                    fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sVal, " ") + " " + align);

                    ResCnt++;
                }
                sCount += (ulong)align;
            }
            return ResCnt;
        }
        public ulong InitSearchText(ulong sStart, byte[] sText, byte[] c, int len, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];
            apiGetMem(sStart, ref ret);
            while ((sCount + (ulong)sText.Length) <= sSize)
            {
                if (CancelSearch == 1)
                    return 0;
                byte[] argB = new byte[len / 2];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sText, argB, c, compMode))
                {
                    byte[] newB = new byte[len / 2];
                    Array.Copy(ret, (int)sCount, newB, 0, newB.Length);
                    ListRes a = misc.GetlvVals((int)NextSAlign, newB, 0);
                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr };
                    var listViewItem = new ListViewItem(row);
                    lvSch.Items.Add(listViewItem);
                    if ((int)NextSAlign == -1)
                        fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sText, " ") + " " + (len / 2) + " -1");
                    else if ((int)NextSAlign == -2)
                        fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sText, " ") + " " + (len / 2) + " -2");
                    ResCnt++;
                }
                sCount++;
            }
            return ResCnt;
        }
        public ulong InitSearchText(bool matchCase, ulong sStart, byte[] sText, byte[] c, int len, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];
            apiGetMem(sStart, ref ret);

            while ((sCount + (ulong)sText.Length) <= sSize)
            {
                if (CancelSearch == 1)
                    return 0;
                byte[] argB = new byte[len / 2];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sText, argB, c, compMode, matchCase))
                {
                    byte[] newB = new byte[len / 2];
                    Array.Copy(ret, (int)sCount, newB, 0, newB.Length);
                    ListRes a = misc.GetlvVals((int)NextSAlign, newB, 0);
                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr };
                    var listViewItem = new ListViewItem(row);
                    lvSch.Items.Add(listViewItem);
                    if ((int)NextSAlign == -1)
                        fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sText, " ") + " " + (len / 2) + " -1");
                    else if ((int)NextSAlign == -2)
                        fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sText, " ") + " " + (len / 2) + " -2");
                    ResCnt++;
                }
                sCount++;
            }
            return ResCnt;
        }
        public ulong FloatSearch(ulong sStart, byte[] sVal, byte[] c, int align, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];
            apiGetMem(sStart, ref ret);
            while (sCount < sSize)
            {
                byte[] argB = new byte[align];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sVal, argB, c, compMode))
                {
                    ListRes a = misc.GetlvVals(align, ret, (int)sCount);
                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr };
                    var listViewItem = new ListViewItem(row);
                    lvSch.Items.Add(listViewItem);
                    fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sVal, " ") + " " + align);

                    ResCnt++;
                }
                sCount += (ulong)align;
            }
            return ResCnt;
        }
        public ulong NextSearch(byte[] sVal, byte[] c, int align)
        {
            ulong cnt = 0, ResCnt = 0;
            byte[] ret = new byte[align];
            schProg.Maximum = (int)SchResCnt + 1;
            schProg.Value = 0;
            ulong maxRes2 = SchResCnt;
            ulong x = 0;
            if (SchResCnt >= MaxRes)
                maxRes2 = MaxCodes;
            CodeRes[] tempSchRes = new Form1.CodeRes[maxRes2];
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dFileName + "2", true))
            {
                for (cnt = 0; cnt < SchResCnt; cnt++)
                {
                    tempSchRes = fileio.ReadDumpArray(dFileName, (int)cnt, (int)maxRes2 + (int)cnt, align);
                    for (x = 0; x < maxRes2; x++)
                    {
                        if (cnt >= SchResCnt)
                            break;
                        cnt++;
                        if (cnt == (SchResCnt - 1))
                            cnt = SchResCnt - 1;
                        if (CancelSearch != 0)
                        {
                            schNSearch.Text = "Next Scan";
                            schProg.Maximum = 0;
                            schProg.Value = 0;
                            lvSch.Items.Clear();
                        }
                        if (CancelSearch == 1)
                            return 0;
                        if (CancelSearch == 2)
                            return 0;
                        apiGetMem(tempSchRes[x].addr, ref ret);
                        schProg.Value++;
                        Application.DoEvents();
                        if (compMode == compINC || compMode == compDEC || compMode == compChg || compMode == compUChg)
                            c = tempSchRes[x].val;
                        if (misc.ArrayCompare(sVal, ret, c, compMode))
                        {
                            file.WriteLine(tempSchRes[x].addr + " " + misc.ByteAToStringInt(ret, " ") + " " + align);
                            ResCnt++;
                        }
                    }
                }
            }
            System.IO.File.Delete(dFileName);
            System.IO.File.Copy(dFileName + "2", dFileName);
            System.IO.File.Delete(dFileName + "2");
            return ResCnt;
        }
        public ulong NextSearchText(byte[] sVal, byte[] cVal, int align)
        {
            ulong cnt = 0, ResCnt = 0;
            schProg.Maximum = (int)SchResCnt;
            schProg.Value = 0;
            ulong maxRes2 = SchResCnt;
            ulong x = 0;
            if (SchResCnt >= MaxRes)
                maxRes2 = MaxCodes;
            CodeRes[] tempSchRes = new Form1.CodeRes[maxRes2];
            for (cnt = 0; cnt < SchResCnt; cnt++)
            {
                tempSchRes = fileio.ReadDumpArray(dFileName, (int)cnt, (int)maxRes2 + (int)cnt, align);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dFileName + "2", true))
                {
                    for (x = 0; x < maxRes2; x++)
                    {
                        cnt++;
                        byte[] ret = new byte[sVal.Length];

                        if (CancelSearch == 1)
                        {
                            schNSearch.Text = "Next Scan";
                            schProg.Maximum = 0;
                            schProg.Value = 0;

                            return 0;
                        }
                       apiGetMem(tempSchRes[x].addr, ref ret);
                        schProg.Value++;
                        Application.DoEvents();
                        if (misc.ArrayCompare(sVal, ret, cVal, compMode))
                        {
                            string end = " -1";
                            if (NextSAlign == -2)
                                end = " -2";
                            file.WriteLine(tempSchRes[x].addr + " " + misc.ByteAToStringInt(ret, " ") + " " + sVal.Length + end);
                            ResCnt++;
                        }
                    }
                }
                System.IO.File.Delete(dFileName);
                System.IO.File.Copy(dFileName + "2", dFileName);
                System.IO.File.Delete(dFileName + "2");
            }
            return ResCnt;
        }
        private void rangesBox_CheckedChanged(object sender, EventArgs e)
        {
            if (rangesBox.Checked)
            {
                if (rangeView.Items.Count > 0)
                {
                    if (rangeView.SelectedItems.Count > 0)
                    {
                        schRange1.Text = rangeView.SelectedItems[0].SubItems[0].Text;
                        schRange2.Text = rangeView.SelectedItems[0].SubItems[1].Text;
                        schRange1.Enabled = false;
                        schRange2.Enabled = false;
                    }
                    else
                    {
                        rangesBox.Checked = false;
                        MessageBox.Show("You haven't selected any range");
                    }
                }
                else
                {
                    rangesBox.Checked = false;
                    MessageBox.Show("Your rangelist is empty. Import ranges, or find them again");
                }
            }
            else
            {
                schRange1.Enabled = true;
                schRange2.Enabled = true;
            }
        }
        private void schSearch_Click(object sender, EventArgs e)
        {
            if (rangesBox.Checked)
            {
                for (int index = 0; index < rangeView.SelectedItems.Count; ++index)
                {
                    bool islast = index == rangeView.SelectedItems.Count - 1;
                    SearchBtnCode(ulong.Parse(rangeView.SelectedItems[index].SubItems[0].Text, NumberStyles.HexNumber), ulong.Parse(rangeView.SelectedItems[index].SubItems[1].Text, NumberStyles.HexNumber), islast);
                }
            }
            else
          SearchBtnCode(ulong.Parse(schRange1.Text, NumberStyles.HexNumber), ulong.Parse(schRange2.Text, NumberStyles.HexNumber), true);
        }
        private void schNSearch_Click(object sender, EventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            byte[] sVal = null;
            byte[] c = null;
            if (schNSearch.Text == "Cancel")
            {
                CancelSearch = 1;
                if (SchPWS.Checked)
                    PS3Lib.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
                return;
            }
            if (NewSearch == false)
            {
                NewSearch = true;
                schNSearch.Text = "Cancel";
            }
            else
            {
                NewSearch = false;
                schNSearch.Text = "Next Scan";
                return;
            }
            if (NextSAlign > 0 || NextSAlign == -2)
            {
                int align = NextSAlign;
                String ValStr = "", ValStr2 = "";
                int oldA = align;
                if (align == -2)
                    align = schVal.Text.Length / 2;
                if (ValHex)
                {
                    ValStr = schVal.Text.PadLeft(align * 2, '0');
                    schVal.Text = ValStr;
                    if (schVal2.Visible)
                    {
                        ValStr2 = schVal2.Text.PadLeft(align * 2, '0');
                        schVal2.Text = ValStr2;
                    }
                }
                else
                {
                    ValStr = int.Parse(schVal.Text).ToString("X8");
                }
                if (ValStr == "")
                {
                    MessageBox.Show("Error: please perform an initial search first.");
                    return;
                }
                sVal = misc.StringBAToBA(ValStr);
                if ((int)NextSAlign == -2)
                    align = ValStr2.Length;
                if (schVal2.Visible)
                    c = misc.StringBAToBA(ValStr2);
                align = oldA;
            }
            if (SchPWS.Checked)
                PS3TMAPI.ProcessAttach(0, PS3TMAPI.UnitType.PPU, TMAPI.Parameters.ProcessID);
            ulong a = 0;
            if (NextSAlign == -1)
            {
                sVal = misc.StringToByteArray(schVal.Text);
                a = NextSearchText(sVal, c, NextSAlign);
            }
            else if (NextSAlign == -2)
            {
                byte[] newB = new byte[schVal.Text.Length / 2];
                Array.Copy(sVal, 0, newB, 0, schVal.Text.Length / 2);
                a = NextSearchText(newB, c, NextSAlign);
            }
            else if (NextSAlign > 0)
                a = NextSearch(sVal, c, NextSAlign);
            if (CancelSearch == 0)
            {
                SchResCnt = a;
                StatusLabel.Text = "Results found: " + SchResCnt.ToString();
                RefreshSearchResults(0);
            }
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            StatusLabel.Text += ", Search time: " + ((Single)elapsedMs / (Single)1000).ToString("F") + " seconds";
            schNSearch.Text = "Next Scan";
            NewSearch = false;
            schProg.Maximum = 0;
            schProg.Value = 0;
            if (SchPWS.Checked)
                PS3Lib.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
            CancelSearch = 0;
            lvSch.EndUpdate();
        }
        private void compBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            compMode = compBox.SelectedIndex;
            if (compMode == 6)
            {
                schVal2.Visible = true;
                schVal2.Size = schVal.Size;
                schVal2.Left = schVal.Size.Width;
                if (cbSchAlign.SelectedIndex == 5)
                    cbSchAlign.SelectedIndex = 0;
            }
            else
                schVal2.Visible = false;
        }
        private void cbSchAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchMode(cbSchAlign.SelectedIndex);
            /* Sizes */
            Size size_0 = new Size(30, 20);
            Size size_1 = new Size(60, 20);
            Size size_2 = new Size(120, 20);
            Size size_3 = new Size(240, 20);
            Size size_4 = new Size(180, 20);
            Size size_5 = new Size(394, 20);
            Size size_6 = new Size(180, 20);

            switch (cbSchAlign.SelectedIndex)
            {
                case 0: //1 byte
                    schVal.Size = size_0;
                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_0.Width + 0 + schVal.Left;
                        schVal2.Size = size_0;
                    }
                    break;
                case 1: //2 bytes
                    schVal.Size = size_1;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_1.Width + 0 + schVal.Left;
                        schVal2.Size = size_1;
                    }
                    break;
                case 2: //4 bytes
                    schVal.Size = size_2;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_2.Width + 0 + schVal.Left;
                        schVal2.Size = size_2;
                    }
                    break;
                case 3: //8 bytes
                    schVal.Size = size_3;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_3.Width + 0 + schVal.Left;
                        schVal2.Size = size_3;
                    }
                    break;
                case 4: //X bytes
                    schVal.Size = size_4;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_4.Width + 0 + schVal.Left;
                        schVal2.Size = size_4;
                    }
                    break;
                case 5: //Text
                    schVal.Size = size_5;
                    schVal2.Visible = false;
                    if (compMode == 6)
                    {
                        compMode = 0;
                        compBox.SelectedIndex = 0;
                    }
                    break;
                case 6: //Float
                    schVal.Size = size_6;
                    schVal2.Visible = false;
                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_6.Width + 0 + schVal.Left;
                        schVal2.Size = size_6;
                    }
                    break;
            }
        }
        private void SchHexCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (SchHexCheck.Text == "Hex")
            {
                ValHex = SchHexCheck.Checked;
                if (schVal.Text.Length == 0)
                    return;
                if (ValHex && schVal.Text.Length <= 8)
                    schVal.Text = int.Parse(schVal.Text).ToString("X8"); //Dec to Hex
                else if (ValHex && schVal.Text.Length > 8)
                    schVal.Text = Int64.Parse(schVal.Text).ToString("X16"); //Dec to Hex
                else if (schVal.Text.Length <= 8)
                    schVal.Text = Convert.ToInt32(schVal.Text, 16).ToString(); //Hex to Dec
                else
                    schVal.Text = Convert.ToInt64(schVal.Text, 16).ToString(); //Hex to Dec
                if (schVal2.Visible)
                {
                    if (ValHex && schVal2.Text.Length <= 8)
                        schVal2.Text = int.Parse(schVal2.Text).ToString("X8"); //Dec to Hex
                    else if (ValHex && schVal2.Text.Length > 8)
                        schVal2.Text = Int64.Parse(schVal2.Text).ToString("X16"); //Dec to Hex
                    else if (schVal2.Text.Length <= 8)
                        schVal2.Text = Convert.ToInt32(schVal2.Text, 16).ToString(); //Hex to Dec
                    else
                        schVal2.Text = Convert.ToInt64(schVal2.Text, 16).ToString(); //Hex to Dec
                }
            }
        }
        private void DumpMem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DumpWorker.IsBusy)
                {
                    DumpWorker.RunWorkerAsync();
                }
                else if (DumpWorker.IsBusy)
                {
                    DumpWorker.CancelAsync();
                    MessageBox.Show("Try Again");
                }
            }
            catch
            {
                /* Getoffsets.Enabled = false;
                 Status.Text = "";
                 TargetInfo = new Thread((InfoWorker));
                 progressBar2.Visible = true;
                 threadIsRunning = true;
                 TargetInfo.Start();*/
            }
            /*ulong rStart = 0, rStop = 0, cnt = 0, incVal = 0x10000;
            int rDif = 0;
            string file = "";
            bool NextSearch = false;
            if (schRange1.Text.Length != 8 || schRange2.Text.Length != 8)
                return;
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
                file = fd.FileName;
            if (file == "")
                return;
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            rStart = ulong.Parse(schRange1.Text, System.Globalization.NumberStyles.HexNumber);
            rStop = ulong.Parse(schRange2.Text, System.Globalization.NumberStyles.HexNumber);
            rDif = (int)misc.ParseRealDif(rStart, rStop, incVal);
            schProg.Maximum = rDif;
            schProg.Value = 0;
            Label1.Text = "No, I am not going to add a cancel button. Just wait through it.";
            schSearch.Enabled = false;
            SchRef.Enabled = false;
            DumpMem.Enabled = false;
            NextSearch = schNSearch.Enabled;
            schNSearch.Enabled = false;

            System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Append,
                System.IO.FileAccess.Write);
            for (cnt = rStart; cnt < rStop; cnt += incVal)
            {
                if (cnt != misc.ParseSchAddr(cnt))
                {
                    int maCnt = 0;
                    uint size = 0;
                    while (maCnt < misc.MemArray.Length && cnt > misc.MemArray[maCnt])
                        maCnt++;
                    if (maCnt < misc.MemArray.Length)
                    {
                        size = (uint)misc.MemArray[maCnt] - (uint)misc.MemArray[maCnt - 1];
                        fs.Write(new byte[size], 0, (int)size);
                        cnt = misc.MemArray[maCnt];
                    }
                    int newVal = misc.ParseRealDif((ulong)rStart, cnt, incVal);
                    if ((int)newVal <= schProg.Maximum)
                        schProg.Value = (int)newVal;
                }
                else
                {
                    if ((cnt + incVal) > rStop)
                        incVal = (rStop - cnt);

                    byte[] retByte = new byte[incVal];

                    apiGetMem(cnt, ref retByte);
                    fs.Write(retByte, 0, retByte.Length);

                    if ((schProg.Value + 1) <= schProg.Maximum)
                        schProg.Value++;
                }
                Application.DoEvents();
            }
            fs.Close();
            schSearch.Enabled = true;
            SchRef.Enabled = true;
            DumpMem.Enabled = true;
            schNSearch.Enabled = NextSearch;
            schProg.Value = 0;
            schProg.Maximum = 0;
            Label1.Text = "Dump complete";*/
        }
        private void DumpWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            schProg.Value = e.ProgressPercentage;
            StatusLabel.Text = "Status: Processing.." + schProg.Value.ToString() + "%";
        }
        private void DumpWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusLabel.Text = "Status : Dump Finished";
            schProg.Visible = false;
            schProg.Maximum = 0;
            schProg.Value = 0;
        }
        private void DumpWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate
                {
                    ulong rStart = 0, rStop = 0, cnt = 0, incVal = 0x10000; int rDif = 0;
                    string file = ""; SaveFileDialog fd = new SaveFileDialog();
                    fd.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                    fd.RestoreDirectory = true;

                    if (schRange1.Text.Length != 8 || schRange2.Text.Length != 8)
                        return;

                    if (fd.ShowDialog() == DialogResult.OK)
                        file = fd.FileName;
                    if (file == "")
                        return;
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
                    rStart = ulong.Parse(schRange1.Text, NumberStyles.HexNumber);
                    rStop = ulong.Parse(schRange2.Text, NumberStyles.HexNumber);
                    rDif = (int)misc.ParseRealDif(rStart, rStop, incVal);
                    schProg.Maximum = rDif;
                    schProg.Value = 0;
                    System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    for (cnt = rStart; cnt < rStop; cnt += incVal)
                    {
                        if (cnt != misc.ParseSchAddr(cnt))
                        {
                            int maCnt = 0;
                            uint size = 0;
                            while (maCnt < MemArray.Length && cnt > MemArray[maCnt])
                                maCnt++;
                            if (maCnt < MemArray.Length)
                            {
                                size = (uint)MemArray[maCnt] - (uint)MemArray[maCnt - 1];
                                fs.Write(new byte[size], 0, (int)size);
                                cnt = MemArray[maCnt];
                            }
                            int newVal = misc.ParseRealDif((ulong)rStart, cnt, incVal);
                            if ((int)newVal <= schProg.Maximum)
                                schProg.Value = (int)newVal;
                        }
                        else
                        {
                            if ((cnt + incVal) > rStop)
                                incVal = (rStop - cnt);
                            byte[] retByte = new byte[incVal];
                            PS3.GetMemory((uint)cnt, retByte);
                            fs.Write(retByte, 0, retByte.Length);
                            if ((schProg.Value + 1) <= schProg.Maximum)
                              schProg.Value++;
                        }
                        Application.DoEvents();
                    }
                    fs.Close();
                }));
            }
        }
        private void InfoWorker()
        {
            while (threadIsRunning)
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        ulong rStart = 0, rStop = 0, cnt = 0, incVal = 0x10000;
                        int rDif = 0;
                        string file = "";
                        SaveFileDialog fd = new SaveFileDialog();
                        fd.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                        fd.RestoreDirectory = true;

                        if (schRange1.Text.Length != 8 || schRange2.Text.Length != 8)
                            return;
                        if (fd.ShowDialog() == DialogResult.OK)
                            file = fd.FileName;
                        if (file == "")
                            return;
                        if (System.IO.File.Exists(file))
                            System.IO.File.Delete(file);

                        rStart = ulong.Parse(schRange1.Text, NumberStyles.HexNumber);
                        rStop = ulong.Parse(schRange2.Text, NumberStyles.HexNumber);
                        rDif = (int)misc.ParseRealDif(rStart, rStop, incVal);
                        schProg.Maximum = rDif;
                        schProg.Value = 0;

                        System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Append, System.IO.FileAccess.Write);

                        for (cnt = rStart; cnt < rStop; cnt += incVal)
                        {
                            if (cnt != misc.ParseSchAddr(cnt))
                            {
                                int maCnt = 0;
                                uint size = 0;
                                while (maCnt < MemArray.Length && cnt > MemArray[maCnt])
                                    maCnt++;

                                if (maCnt < MemArray.Length)
                                {
                                    size = (uint)MemArray[maCnt] - (uint)MemArray[maCnt - 1];
                                    fs.Write(new byte[size], 0, (int)size);
                                    cnt = MemArray[maCnt];
                                }

                                int newVal = misc.ParseRealDif((ulong)rStart, cnt, incVal);
                                if ((int)newVal <= schProg.Maximum)
                                    schProg.Value = (int)newVal;
                            }
                            else
                            {
                                if ((cnt + incVal) > rStop)
                                    incVal = (rStop - cnt);
                                byte[] retByte = new byte[incVal];

                                PS3.GetMemory((uint)cnt, retByte);
                                fs.Write(retByte, 0, retByte.Length);

                                if ((schProg.Value + 1) <= schProg.Maximum)
                                    schProg.Value++;
                            }
                            Application.DoEvents();
                        }
                        fs.Close();

                        schProg.Value = 0;
                        schProg.Maximum = 0;
                        StatusLabel.Text = "Dump complete";
                        schProg.Enabled = true;
                    }));
                    Thread.Sleep(500);
                }
                TargetInfo.Abort();
            }
        }
        private void loadSRes_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName != dFileName)
                    System.IO.File.Copy(fd.FileName, dFileName, true);
                RefreshFromDump();
                MessageBox.Show("Results loaded!");
            }
        }
        private void SchRef_Click(object sender, EventArgs e)
        {
            RefreshSearchResults(1);
        }
        private void saveSRes_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
                System.IO.File.Copy(dFileName, fd.FileName, true);
            MessageBox.Show("Results saved!");
        }
        private void Options_Click(object sender, EventArgs e)
        {
            OptionForm oForm = new OptionForm();
            oForm.Show();
        }
        bool findRangesCancel = false;
        private void findRanges_Click(object sender, EventArgs e)
        {
           /* try
            {
                if (!RangeWorker.IsBusy)
                {
                    RangeWorker.RunWorkerAsync();
                }
                else if (RangeWorker.IsBusy)
                {
                    RangeWorker.CancelAsync();
                    MessageBox.Show("Try Again");
                }
            }
            catch
            {
                 /*TargetInfo = new Thread((InfoWorker));
                 threadIsRunning = true;
                 TargetInfo.Start();*/
            //}
            if (findRanges.Text == "Stop")
            {
                findRangesCancel = true;
                return;
            }
            rangeView.Items.Clear();
            ulong findAddr = 0;
            ulong blockSize = 0x10000;
            findRanges.Text = "Stop";
            bool inMemRange = false;
            findRangeProgBar.Maximum = 4096;
            ulong blockStart = 0;
            for (findAddr = 0; findAddr < 0xFFFFFFFC; findAddr += blockSize)
            {
                if (findRangesCancel)
                    break;
                if ((findAddr % 0x100000) == 0)
                {
                    Label4.Text = "Scanning memory at 0x" + findAddr.ToString("X8");
                    findRangeProgBar.Increment(1);
                    Application.DoEvents();
                }
                byte[] ret = new byte[1];
                bool validRegion = apiGetMem(findAddr, ref ret);
                //Start new mem block
                if (validRegion && !inMemRange)
                {
                    blockStart = findAddr;
                    inMemRange = true;
                }
                //Add block
                else if (!validRegion && inMemRange)
                {
                    string[] str = new string[2];
                    str[0] = blockStart.ToString("X8");
                    str[1] = findAddr.ToString("X8");
                    ListViewItem strLV = new ListViewItem(str);
                    rangeView.Items.Add(strLV);
                    inMemRange = false;
                }
            }
            findRangesCancel = false;
            findRanges.Text = "Find Ranges";
            findRangeProgBar.Value = 0;
            UpdateMemArray();
            if (misc.MemArray != null || misc.MemArray.Length > 0)
            MessageBox.Show("Find Ranges Completed!\nUnderstand that the range finder searches in blocks of 0x10000.");
        }
        private void RangeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (findRanges.Text == "Stop")
            {
                findRangesCancel = true;
                return;
            }
            rangeView.Items.Clear();
            ulong findAddr = 0;
            ulong blockSize = 0x10000;
            findRanges.Text = "Stop";
            bool inMemRange = false;
            findRangeProgBar.Maximum = 4096;
            ulong blockStart = 0;
            for (findAddr = 0; findAddr < 0xFFFFFFFC; findAddr += blockSize)
            {
                if (findRangesCancel)
                    break;
                if ((findAddr % 0x100000) == 0)
                {
                    Label4.Text = "Scanning memory at 0x" + findAddr.ToString("X8");
                    findRangeProgBar.Increment(1);
                    Application.DoEvents();
                }
                byte[] ret = new byte[1];
                bool validRegion = apiGetMem(findAddr, ref ret);

                //Start new mem block
                if (validRegion && !inMemRange)
                {
                    blockStart = findAddr;
                    inMemRange = true;
                }

                //Add block
                else if (!validRegion && inMemRange)
                {
                    string[] str = new string[2];
                    str[0] = blockStart.ToString("X8");
                    str[1] = findAddr.ToString("X8");
                    ListViewItem strLV = new ListViewItem(str);
                    rangeView.Items.Add(strLV);
                    inMemRange = false;
                }
            }
            findRangesCancel = false;
            findRanges.Text = "Find Ranges";
            findRangeProgBar.Value = 0;
            UpdateMemArray();
            if (misc.MemArray != null || misc.MemArray.Length > 0)
          MessageBox.Show("Find Ranges Completed!\nUnderstand that the range finder searches in blocks of 0x10000.");
        }
        private void RangeWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            findRangeProgBar.Value = e.ProgressPercentage;
            Label4.Text = Label4.Text = "Scanning memory at 0x" + schProg.Value.ToString() + "%";
        }
        private void RangeWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Label4.Text = "Scanning: Finished";
            findRangeProgBar.Maximum = 0;
            findRangeProgBar.Value = 0;
        }
        public int rangeListIndex = -1;
        private void rangeView_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int x = 0; x < rangeView.Items.Count; x++)
            {
                if (rangeView.Items[x].Selected)
                {
                    rangeListIndex = x;
                    return;
                }
            }
        }
        private void rangeView_DoubleClick(object sender, EventArgs e)
        {
            IBArg[] a = new IBArg[2];
            a[0].defStr = rangeView.Items[rangeListIndex].SubItems[0].Text;
            a[0].label = "Start Address";

            a[1].defStr = rangeView.Items[rangeListIndex].SubItems[1].Text;
            a[1].label = "End Address";
            a = CallIBox(a);
            if (a == null)
                return;
            if (a[0].retStr.Length == 8)
                rangeView.Items[rangeListIndex].SubItems[0].Text = a[0].retStr;
            else
                rangeView.Items[rangeListIndex].SubItems[0].Text = a[0].defStr;

            if (a[1].retStr.Length == 8)
                rangeView.Items[rangeListIndex].SubItems[1].Text = a[1].retStr;
            else
                rangeView.Items[rangeListIndex].SubItems[1].Text = a[1].defStr;

            //Update range array
            UpdateMemArray();
        }
        public IBArg[] CallIBox(IBArg[] a)
        {
            InputBox ib = new InputBox();
            ib.Arg = a;
            ib.fmHeight = Height;
            ib.fmWidth = Width;
            ib.fmLeft = Left;
            ib.fmTop = Top;
            ib.TopMost = true;
            ib.fmForeColor = ForeColor;
            ib.fmBackColor = BackColor;
            ib.Show();
            while (ib.ret == 0)
            {
                a = ib.Arg;
                Application.DoEvents();
            }
            a = ib.Arg;
            if (ib.ret == 1)
                return a;
            else if (ib.ret == 2)
                return null;
            return null;
        }
        private void AddRange_Click(object sender, EventArgs e)
        {
            string[] rV = { "00000000", "00000000" };
            ListViewItem a = new ListViewItem(rV);
            rangeView.Items.Add(a);
            UpdateMemArray();
        }
        private void RemoveRange_Click(object sender, EventArgs e)
        {
            if (rangeListIndex < 0)
                return;
            rangeView.Items.RemoveAt(rangeListIndex);
            UpdateMemArray();
            if (rangeListIndex >= rangeView.Items.Count)
                rangeListIndex = (rangeView.Items.Count - 1);
            if (rangeListIndex < 0)
                return;
            rangeView.Items[rangeListIndex].Selected = true;
        }
        private void ImportRange_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Memory Range files (*.ncm)|*.ncm|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //Check if file is already added to the recent range imports
                bool added = false;
                foreach (string rI in rangeImports)
                {
                    if (rI == fd.FileName)
                    {
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    //File path
                    Array.Resize(ref rangeImports, rangeImports.Length + 1);
                    rangeImports[rangeImports.Length - 1] = fd.FileName;
                    //Order
                    Array.Resize(ref rangeOrder, rangeOrder.Length + 1);
                    for (int rO = 0; rO < rangeOrder.Length - 1; rO++)
                        rangeOrder[rO]++;
                    rangeOrder[rangeOrder.Length - 1] = 0;
                    //Add to recRangeBox
                    System.IO.FileInfo fi = new System.IO.FileInfo(fd.FileName);
                    ListViewItem lvi = new ListViewItem(new string[] { fi.Name });
                    lvi.Tag = rangeOrder.Length - 1;
                    lvi.ToolTipText = fi.FullName;
                    recRangeBox.Items.Insert(0, lvi);

                    SaveOptions();
                }
                ListView a = new ListView();
                a = fileio.OpenRangeFile(fd.FileName);

                if (a == null)
                    return;
                rangeView.Items.Clear();
                string[] str = new string[2];
                for (int x = 0; x < a.Items.Count; x++)
                {
                    str[0] = a.Items[x].SubItems[0].Text;
                    str[1] = a.Items[x].SubItems[1].Text;
                    ListViewItem strLV = new ListViewItem(str);
                    rangeView.Items.Add(strLV);
                }
                UpdateMemArray();
                Text = "Debugger " + VersionNum + " by Flynhigh09(" + new System.IO.FileInfo(fd.FileName).Name + ")";
            }
        }
        private void SaveRange_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Memory Range files (*.ncm)|*.ncm|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileio.SaveRangeFile(fd.FileName, rangeView);
                //Check if file is already added to the recent range imports
                bool added = false;
                foreach (string rI in rangeImports)
                {
                    if (rI == fd.FileName)
                    {
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    //File path
                    Array.Resize(ref rangeImports, rangeImports.Length + 1);
                    rangeImports[rangeImports.Length - 1] = fd.FileName;
                    //Order
                    Array.Resize(ref rangeOrder, rangeOrder.Length + 1);
                    for (int rO = 0; rO < rangeOrder.Length - 1; rO++)
                        rangeOrder[rO]++;
                    rangeOrder[rangeOrder.Length - 1] = 0;
                    //Add to recRangeBox
                    System.IO.FileInfo fi = new System.IO.FileInfo(fd.FileName);
                    ListViewItem lvi = new ListViewItem(new string[] { fi.Name });
                    lvi.Tag = rangeOrder.Length - 1;
                    lvi.ToolTipText = fi.FullName;
                    recRangeBox.Items.Insert(0, lvi);
                    SaveOptions();
                }
            }
        }
        private void RangeUp_Click(object sender, EventArgs e)
        {
            if (rangeListIndex <= 0)
                return;
            ListViewItem selected = rangeView.Items[rangeListIndex];
            rangeView.Items.RemoveAt(rangeListIndex);
            rangeView.Items.Insert(rangeListIndex - 1, selected);
        }
        private void RangeDown_Click(object sender, EventArgs e)
        {
            if (rangeListIndex < 0 || rangeListIndex >= rangeView.Items.Count)
                return;
            ListViewItem selected = rangeView.Items[rangeListIndex];
            rangeView.Items.RemoveAt(rangeListIndex);
            rangeView.Items.Insert(rangeListIndex + 1, selected);
        }
        private void recRangeBox_DoubleClick(object sender, EventArgs e)
        {
            if (recRangeBox.SelectedIndices.Count >= 0 && recRangeBox.SelectedIndices[0] >= 0)
            {
                int ind = recRangeBox.SelectedIndices[0];
                string path = recRangeBox.Items[ind].ToolTipText;

                if (System.IO.File.Exists(path))
                {
                    ListView a = new ListView();
                    a = fileio.OpenRangeFile(path);
                    if (a == null)
                        return;
                    rangeView.Items.Clear();
                    string[] str = new string[2];
                    for (int x = 0; x < a.Items.Count; x++)
                    {
                        str[0] = a.Items[x].SubItems[0].Text;
                        str[1] = a.Items[x].SubItems[1].Text;
                        ListViewItem strLV = new ListViewItem(str);
                        rangeView.Items.Add(strLV);
                    }
                    UpdateMemArray();
                    Text = "Debugger " + VersionNum + " by Flynhigh09 (" + recRangeBox.Items[ind].Text + ")";
                    int roInd = int.Parse(recRangeBox.Items[ind].Tag.ToString());
                    if (ind != 0)
                    {
                        for (int xRO = 0; xRO < ind; xRO++)
                        {
                            int newInd = int.Parse(recRangeBox.Items[xRO].Tag.ToString());
                            rangeOrder[newInd]++;
                        }
                        rangeOrder[roInd] = 0;
                        SaveOptions();
                        UpdateRecRangeBox();
                    }
                }
                else
                {
                    if (MessageBox.Show(this, "Would you like to remove the reference to it?", "File Doesn't Exist!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //Delete reference to file
                        int index = 0;
                        //Find index of file
                        for (index = 0; index < rangeOrder.Length; index++)
                        {
                            if (rangeImports[index] == path)
                                break;
                        }
                        int[] newRangeOrder = new int[rangeOrder.Length - 1];
                        string[] newRangeImports = new string[newRangeOrder.Length];
                        int y = 0;
                        for (int x = 0; x < rangeOrder.Length; x++)
                        {
                            if (x != index)
                            {
                                newRangeOrder[y] = rangeOrder[x];
                                newRangeImports[y] = rangeImports[x];
                                if (y >= rangeOrder[index])
                                    newRangeOrder[y]--;
                                y++;
                            }
                        }
                        rangeOrder = newRangeOrder;
                        rangeImports = newRangeImports;
                        SaveOptions();
                        recRangeBox.Items.RemoveAt(ind);
                    }
                }
            }
        }
        private void Play_Click(object sender, EventArgs e)
        {
            if (apiDLL == 0)
                PS3Lib.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            if (apiDLL == 0)
                PS3Lib.PS3TMAPI.ProcessAttach(0, PS3Lib.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID);
        }
        #endregion

        #region  Plugins Hnadles
        public static void HandlePluginControls(Control.ControlCollection plgCtrl)
        {
            foreach (Control ctrl in plgCtrl)
            {
                if (ctrl is GroupBox || ctrl is Panel || ctrl is TabControl || ctrl is TabPage ||
                    ctrl is UserControl || ctrl is ListBox || ctrl is ListView)
                    HandlePluginControls(ctrl.Controls);

                ctrl.BackColor = ncBackColor;
                ctrl.ForeColor = ncForeColor;
            }
        }
        private void refPlugin_Click(object sender, EventArgs e)
        {
            if (refPlugin.Text == "Close Plugins")
            {
                foreach (PluginForm pF in pluginForm)
                {
                    pF.Close();
                    //pF = null;
                }
                Global.Plugins.ClosePlugins();
                //TabCon.SelectedIndex = 0;
                pluginList.Items.Clear();
                refPlugin.Text = "Load Plugins";
                codes.ConstCodes = new codes.ConstCode[0];
            }
            else
            {
                int x = 0;
                //Close any open plugins
                Global.Plugins.ClosePlugins();
                pluginList.Items.Clear();

                if (System.IO.Directory.Exists(Application.StartupPath + @"\Plugins") == false)
                    return;

                //Delete any excess PluginInterface.dll's (result of a build and not a copy)
                foreach (string file in System.IO.Directory.GetFiles(Application.StartupPath + @"\Plugins", "PluginInterface.dll", System.IO.SearchOption.AllDirectories))
                    System.IO.File.Delete(file);

                //Call the find plugins routine, to search in our Plugins Folder
                Global.Plugins.FindPlugins(Application.StartupPath + @"\Plugins");

                //Load plugins
                pluginForm = Global.Plugins.GetPlugin(ncBackColor, ncForeColor);
                if (pluginForm != null)
                {
                    Array.Resize(ref pluginFormActive, pluginForm.Length);
                    for (x = 0; x < pluginForm.Length; x++)
                    {
                        pluginForm[x].Tag = x;
                        pluginForm[x].FormClosing += new FormClosingEventHandler(HandlePlugin_Closing);
                        pluginList.Items.Add(pluginForm[x].plugText);
                    }
                }
                RangesTab.BackColor = ncForeColor;
                SearchTab.BackColor = ncForeColor;
                CodesTab.BackColor = ncForeColor;
                Ps3DebuggerTab.BackColor = ncForeColor;
                SearcherTab.BackColor = ncForeColor;
                Application.DoEvents();
                SearcherTab.BackColor = ncBackColor;
                Ps3DebuggerTab.BackColor = ncBackColor;
                RangesTab.BackColor = ncBackColor;
                SearchTab.BackColor = ncBackColor;
                CodesTab.BackColor = ncBackColor;
                if (pluginForm.Length != 0)
                    pluginList.SelectedIndex = 0;
                refPlugin.Text = "Close Plugins";
            }
        }
        private void HandlePlugin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int ind = (int)((PluginForm)sender).Tag;
            if (pluginFormActive[ind])
            {
                e.Cancel = true;
                pluginList.SelectedIndex = ind;
                pluginList_DoubleClick(null, null);
            }
        }
        private void pluginList_DoubleClick(object sender, EventArgs e)
        {
            int ind = pluginList.SelectedIndex;
            if (pluginFormActive[ind]) //Already on
            {
                pluginForm[ind].Visible = false;
                pluginForm[ind].WindowState = FormWindowState.Minimized;
                pluginList.Items[ind] = pluginForm[ind].plugText;
                pluginFormActive[pluginList.SelectedIndex] = false;
            }
            else //Turn on
            {
                pluginFormActive[pluginList.SelectedIndex] = true;
                pluginForm[ind].WindowState = FormWindowState.Normal;
                pluginList.Items[ind] = "+ " + pluginForm[ind].plugText;
                pluginForm[pluginList.SelectedIndex].Show();
                pluginForm[ind].Visible = true;
            }
        }
        private void pluginList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = pluginList.SelectedIndex;
            if (ind >= 0 && pluginForm[ind] != null)
            {
                //Get the selected Plugin
                Types.AvailablePlugin selectedPlugin = Global.Plugins.AvailablePlugins.GetIndex(ind);

                if (selectedPlugin != null && pluginForm[ind].Controls.Count == 0)
                {
                    pluginForm[ind].Controls.Clear();

                    //Set the dockstyle of the plugin to fill, to fill up the space provided
                    selectedPlugin.Instance.MainInterface.Dock = DockStyle.Fill;

                    //Finally, add the usercontrol to the tab... Tadah!
                    pluginForm[ind].Controls.Add(selectedPlugin.Instance.MainInterface);
                    pluginForm[ind].Controls[0].Resize += new EventHandler(pluginForm[ind].Plugin_Resize);

                    //TabCon.TabPages[TabCon.SelectedIndex].Controls[0].Dock = DockStyle.None;

                    pluginForm[ind].Controls[0].BackColor = ncBackColor;
                    pluginForm[ind].Controls[0].ForeColor = ncForeColor;
                    //Color each control in the tab too
                    //for (int tabCtrl = 0; tabCtrl < TabCon.TabPages[TabCon.SelectedIndex].Controls[0].Controls.Count; tabCtrl++)
                    HandlePluginControls(pluginForm[ind].Controls[0].Controls);
                }
                if (pluginForm[ind].Controls.Count > 0)
                {
                    descPlugAuth.Text = "by " + selectedPlugin.Instance.Author;
                    descPlugName.Text = selectedPlugin.Instance.Name;
                    descPlugVer.Text = selectedPlugin.Instance.Version;
                    descPlugDesc.Text = selectedPlugin.Instance.Description;
                    /*if (selectedPlugin.Instance.MainIcon != null)
                        plugIcon.Image = (Bitmap)selectedPlugin.Instance.MainIcon.BackgroundImage.Clone();
                    else
                       plugIcon.Image = (Bitmap)plugIcon.InitialImage.Clone();*/
                }
            }
        }
         #endregion

        #region PS3
        static uint[] ProcessIDs;
        public static void SetMemory(uint Address, byte[] Bytes, uint thread = 0)
        {
            PS3TMAPI.ProcessSetMemory(0, PS3TMAPI.UnitType.PPU, ProcessID, thread, Address, Bytes);
        }
        public static bool GetMemory(ulong addr, ref byte[] val)
        {
            bool ret = false;
            ret = (PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID, 0, addr, ref val) ==
            PS3TMAPI.SNRESULT.SN_S_OK);
            return ret;
        }
        public static byte[] GetMemory(uint Address, int length, uint thread = 0)
        {
            byte[] x = new byte[length];
            PS3Lib.PS3TMAPI.ProcessGetMemory(0, PS3Lib.PS3TMAPI.UnitType.PPU, ProcessID, thread, Address, ref x);
            return x;
        }
        private static byte[] buff = new byte[0x20];
        public static float ReadFloat(uint offset)
        {
            PS3.GetMemory(offset, buff);
            Array.Reverse(buff, 0, 4);
            return BitConverter.ToSingle(buff, 0);
        }
        public static int ReadInt(uint address)
        {
            byte[] buff = GetMemory(address, 4, 0);
            Array.Reverse(buff);
            int val = BitConverter.ToInt32(buff, 0);
            return val;
        }
        public static uint ReadUInt(uint address)
        {
            byte[] buff = GetMemory(address, 4, 0);
            Array.Reverse(buff);
            uint val = BitConverter.ToUInt32(buff, 0);
            return val;
        }
        public static short ReadShort(uint address)
        {
            byte[] buff = GetMemory(address, 2, 0);
            Array.Reverse(buff);
            short val = BitConverter.ToInt16(buff, 0);
            return val;
        }
        public static byte ReadByte(uint address)
        {
            byte[] buff = GetMemory(address, 1, 0);
            return buff[0];
        }
        public static sbyte ReadSByte(uint address)
        {
            return (sbyte)GetMemory(address, 1)[0];
        }
        public static sbyte[] ReadSBytes(uint address, int length)
        {
            byte[] memory = GetMemory(address, length);
            sbyte[] numArray = new sbyte[length];
            for (int index = 0; index < length; ++index)
                numArray[index] = (sbyte)memory[index];
            return numArray;
        }
        private float[] ReadSingle(UInt32 address, Int32 length)
        {
            Byte[] memory = new byte[length * 4];
            PS3.GetMemory(address, memory);
            ReverseBytes(memory);
            float[] numArray = new float[length];
            for (Int32 i = 0; i < length; i++)
            {
                numArray[i] = BitConverter.ToSingle(memory, ((length - 1) - i) * 4);
            }
            return numArray;
        }
        public static float ReadSingle(uint address)
        {
            byte[] memory = GetMemory(address, 4);
            Array.Reverse((Array)memory, 0, 4);
            return BitConverter.ToSingle(memory, 0);
        }
        public static string ReadString(uint address)
        {
            int length = 0;
            for (int i = 0; i < 5000; i++)
            {
                byte buffer = GetMemory(address + (uint)i, 1)[0];
                if (buffer == (byte)0x00) { length = i; break; }
            }
            byte[] buff = GetMemory(address, length, 0);
            return Encoding.ASCII.GetString(buff);
        }
        public static void WriteFloat(uint addr, float f)
        {
            byte[] Float = BitConverter.GetBytes(f);
            Array.Reverse(Float);
            PS3.SetMemory(addr, Float);
        }
        public static void WriteFloatArray(uint Offset, float[] Array)
        {
            byte[] buffer = new byte[Array.Length * 4];
            for (int Lenght = 0; Lenght < Array.Length; Lenght++)
            {
                ReverseBytes(BitConverter.GetBytes(Array[Lenght])).CopyTo(buffer, Lenght * 4);
            }
            PS3.SetMemory(Offset, buffer);
        }
        public static float[] ReadFloatLength(uint Offset, int Length)
        {
            byte[] buffer = new byte[Length * 4];
            PS3.GetMemory(Offset, buffer);
            ReverseBytes(buffer);
            float[] Array = new float[Length];
            for (int i = 0; i < Length; i++)
            {
                Array[i] = BitConverter.ToSingle(buffer, (Length - 1 - i) * 4);
            }
            return Array;
        }
        public static void Writefloat(uint offset, float input)
        {
            byte[] buff = new byte[4];
            BitConverter.GetBytes(input).CopyTo(buff, 0);
            Array.Reverse(buff, 0, 4);
            PS3.SetMemory(offset, buff);
        }
        public static void WriteFloat2(uint offset, float[] input)
        {
            byte[] buff = new byte[4];
            PS3.SetMemory(offset, buff);
        }
        public static void WriteByte(uint address, byte val)
        {
            SetMemory(address, new byte[] { val });
        }
        public static void WriteInt(uint address, int val)
        {
            SetMemory(address, ReverseBytes(BitConverter.GetBytes(val)), 0);
        }
        public static void WriteUInt(uint address, uint val)
        {
            SetMemory(address, ReverseBytes(BitConverter.GetBytes(val)), 0);
        }
        public static void WriteString(uint address, string txt)
        {
            SetMemory(address, Encoding.ASCII.GetBytes(txt + "\0"), 0);
        }
        public static void WriteBytes(uint address, byte[] input)
        {
            PS3.SetMemory(address, input);
        }
        public static void WriteSByte(uint address, sbyte input)
        {
            byte[] Bytes = new byte[1] { (byte)input };
            PS3.SetMemory(address, Bytes);
        }
        public static void WriteSBytes(uint address, sbyte[] input)
        {
            int length = input.Length;
            byte[] Bytes = new byte[length];
            for (int index = 0; index < length; ++index)
                Bytes[index] = (byte)input[index];
            PS3.SetMemory(address, Bytes);
        }
        public static void WriteSingle(uint address, float input)
        {
            byte[] Bytes = new byte[4];
            BitConverter.GetBytes(input).CopyTo(Bytes, 0);
            Array.Reverse(Bytes, 0, 4);
            PS3.SetMemory(address, Bytes);
        }
        public static void WriteSingles(uint address, float[] input)
        {
            int length = input.Length;
            byte[] Bytes = new byte[length * 4];
            for (int index = 0; index < length; ++index)
                ReverseBytes(BitConverter.GetBytes(input[index])).CopyTo(Bytes, index * 4);
            PS3.SetMemory(address, Bytes);
        }
        public static string char_to_wchar(string text)
        {
            string str = text;
            for (int index = 0; index < text.Length; ++index)
                str = str.Insert(index * 2, "\0");
            return str;
        }
        public static bool WriteBytesToggle(uint Offset, byte[] On, byte[] Off)
        {
            bool flag = (int)ReadByte(Offset) == (int)On[0];
            WriteBytes(Offset, !flag ? On : Off);
            return flag;
        }
        public static void WriteDouble(uint address, double input)
        {
            byte[] Bytes = new byte[8];
            BitConverter.GetBytes(input).CopyTo((Array)Bytes, 0);
            Array.Reverse((Array)Bytes, 0, 8);
            PS3.SetMemory(address, Bytes);
        }
        public static void WriteDouble(uint address, double[] input)
        {
            int length = input.Length;
            byte[] Bytes = new byte[length * 8];
            for (int index = 0; index < length; ++index)
                ReverseBytes(BitConverter.GetBytes(input[index])).CopyTo((Array)Bytes, index * 8);
            PS3.SetMemory(address, Bytes);
        }
        private static void WriteSingle(UInt32 address, float[] input)
        {
            Int32 length = input.Length;
            Byte[] array = new Byte[length * 4];
            for (Int32 i = 0; i < length; i++)
            {
                ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (i * 4));
            }
            PS3.SetMemory(address, array);
        }
        public static Byte[] ReverseBytes(Byte[] toReverse)
        {
            Array.Reverse(toReverse);
            return toReverse;
        }
        #endregion
    }
}