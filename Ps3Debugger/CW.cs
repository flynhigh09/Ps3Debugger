using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ps3Debugger;

namespace CWPS3Portable
{
  public class CW
  {
    private static bool[] importLoaded = new bool[0];
    private string re = "(@(?:\"[^\"]*\")+|\"(?:[^\"\\n\\\\]+|\\\\.)*\"|'(?:[^'\\n\\\\]+|\\\\.)*')|//.*|/\\*(?s:.*?)\\*/";
    public CW.pInstruction[] customPseudos = new CW.pInstruction[0];
    public CW.PPCInstr[] ASMDef = new CW.PPCInstr[100];
    public CW.RegCol[] RegColArr = new CW.RegCol[75];
    public int[] InsPlaceArr = new int[26];
    public string[] helpCom = new string[8];
    public string[] helpReg = new string[14];
    public string[] helpTerm = new string[78];
    public string debugString = "";
    public bool isBranchCompareRelative = false;
    private uint CompAddress = 0U;
    private uint FirstCompAddr = 0U;
    private uint CompHook = 0U;
    public CW.ASMLabel[] labels = (CW.ASMLabel[]) null;
    private int HookMode = 0;
    private int ValState = 0;
    private int vstHex = 0;
    private int vstDec = 1;
    private int vstLab = 2;
    public string[] importPaths = new string[0];
    private int impOffset = 0;
    private uint DisASMAddr = 0U;
    private CW.ASMLabel[] DisASMLabel = new CW.ASMLabel[0];
    private bool addr = true;
    public int ASMDisMode = 0;
    public const int typeNAN = 0;
    public const int typeBNC = 1;
    public const int typeNOP = 2;
    public const int typeOFI = 3;
    public const int typeIMM = 4;
    public const int typeSPR = 5;
    public const int typeBNCMP = 6;
    public const int typeCND = 7;
    public const int typeIMM5 = 8;
    public const int typeFNAN = 9;
    public const int typeFOFI = 10;
    public const int typeFCND = 11;
    private string importDef;
    public int ParseRegsImm;

    private string[] InitAssembly(string ASMBox, string DefBox, string GlobalFileName)
    {
      int num1 = 0;
      if (ASMBox.IndexOf("\r\n") >= 0)
        num1 = 1;
      importPaths = new string[0];
      importLoaded = new bool[0];
      impOffset = 0;
      CompAddress = 0U;
      FirstCompAddr = 0U;
      HookMode = 0;
      if (DefBox != null)
        importDef = DefBox + "\n";
      if (ASMBox == "")
        return new string[0];
      debugString = "";
      string pattern = "(@(?:\"[^\"]*\")+|\"(?:[^\"\\n\\\\]+|\\\\.)*\"|'(?:[^'\\n\\\\]+|\\\\.)*')|//.*|/\\*(?s:.*?)\\*/";
      RichTextBox richTextBox1 = new RichTextBox();
      string str1 = "";
      richTextBox1.Text = Regex.Replace(ASMBox + Environment.NewLine, pattern, "$1");
      foreach (string line in richTextBox1.Lines)
        str1 = str1 + ParseASMText(line) + Environment.NewLine;
      richTextBox1.Text = str1;
      richTextBox1.Find("a");
      richTextBox1.Text = GetImports(richTextBox1.Text, GlobalFileName);
      richTextBox1.Text = Regex.Replace(richTextBox1.Text, pattern, "$1");
      string str2 = "";
      foreach (string line in richTextBox1.Lines)
        str2 = str2 + ParseASMText(line) + Environment.NewLine;
      richTextBox1.Text = str2;
      string str3 = richTextBox1.Text.ToLower();
      bool flag = false;
      for (int index1 = 0; index1 < customPseudos.Length; ++index1)
      {
        string str4 = " ";
        int index2 = str3.IndexOf(customPseudos[index1].name.ToLower() + str4, 0);
        if (customPseudos[index1].regs.Length <= 0 && index2 < 0)
        {
          str4 = "\n";
          index2 = str3.IndexOf(customPseudos[index1].name.ToLower() + str4, 0);
          if (index2 < 0)
          {
            str4 = "\r";
            index2 = str3.IndexOf(customPseudos[index1].name.ToLower() + str4, 0);
          }
        }
        if (index2 != 0)
        {
          index2 = str3.IndexOf("\r" + customPseudos[index1].name.ToLower() + str4, 0);
          if (index2 < 0)
            index2 = str3.IndexOf("\n" + customPseudos[index1].name.ToLower() + str4, 0);
        }
        for (; index2 >= 0 || flag; index2 = (str3 = richTextBox1.Text.ToLower()).IndexOf("\r" + customPseudos[index1].name.ToLower() + " ", index2 + 1))
        {
          if (flag)
          {
            index1 = 0;
            index2 = str3.IndexOf(customPseudos[index1].name.ToLower() + " ", 0);
            if (index2 != 0)
              index2 = str3.IndexOf(customPseudos[index1].name.ToLower() + " ", 0);
            if (index2 < 0)
            {
              flag = false;
              break;
            }
          }
          flag = true;
          int num2 = richTextBox1.Find("\r", index2 + 1, RichTextBoxFinds.None);
          string[] strArray1;
          if (index2 != 0)
            strArray1 = misc.sMid(richTextBox1.Text, index2 + num1, num2 - index2).Split(' ');
          else
            strArray1 = misc.sMid(richTextBox1.Text, index2, num2 - index2).Split(' ');
          string[] strArray2 = new string[customPseudos[index1].regs.Length];
          if (strArray1.Length - 1 != strArray2.Length && strArray2.Length != 1)
          {
            flag = false;
          }
          else
          {
            for (int index3 = 0; index3 < strArray2.Length; ++index3)
              strArray2[index3] = strArray1[index3 + 1].Replace(",", "").Replace("\n", "");
            string str5 = customPseudos[index1].asm;
            for (int index3 = 0; index3 < customPseudos[index1].regs.Length; ++index3)
              str5 = str5.Replace(customPseudos[index1].regs[index3], strArray2[index3]);
            if (index2 == 0)
              richTextBox1.Text = richTextBox1.Text.Replace(misc.sMid(richTextBox1.Text, index2, num2 - index2), str5 + "\n");
            else
              richTextBox1.Text = richTextBox1.Text.Replace(misc.sMid(richTextBox1.Text, index2 + num1, num2 - index2), str5 + "\n");
          }
        }
      }
      richTextBox1.Text = Regex.Replace(richTextBox1.Text, pattern, "$1");
      RichTextBox richTextBox2 = richTextBox1;
      string str6 = richTextBox2.Text + "\n/*\n" + importDef + "\n*/\n";
      richTextBox2.Text = str6;
      labels = ProcessLabels(richTextBox1.Lines);
      if (labels == null)
        return new string[0];
      richTextBox1.Text = Regex.Replace(richTextBox1.Text, pattern, "$1");
      return richTextBox1.Lines;
    }
    public byte[][] ASMAssembleToByte(string ASMBox, string DefBox)
    {
      byte[][] array = new byte[1][];
      int index1 = 0;
      foreach (string line in this.InitAssembly(ASMBox, DefBox, Application.StartupPath))
      {
        string str = this.ASMToHex(this.ParseASMText(line));
        if (str != null && (str.IndexOf(" ") < 0 && str != ""))
        {
          if (array[index1] == null)
            array[index1] = new byte[0];
          byte[] numArray = misc.StringBAToBA(str);
          Array.Resize<byte>(ref array[index1], array[index1].Length + numArray.Length);
          Array.Copy((Array) numArray, 0, (Array) array[index1], array[index1].Length - numArray.Length, numArray.Length);
          CompAddress += 4U;
        }
        else if (str != null && str != "" && array[index1] != null)
        {
          ++index1;
          Array.Resize<byte[]>(ref array, array.Length + 1);
        }
      }
      if (importDef != "" && importDef != "\n" && importDef != "\r" && importDef != "\r\n")
      {
        string str1 = Regex.Replace(importDef, re, "$1");
        char[] chArray = new char[1]{'\n'};
        foreach (string line in str1.Split(chArray))
        {
          string str2 = (string) null;
          if (line != "")
            str2 = ASMToHex(ParseASMText(line));
          if (str2 != null && str2 != "")
          {
            if (str2.IndexOf(" ") < 0)
            {
              if (array[index1] == null)
                array[index1] = new byte[0];
              byte[] numArray = misc.StringToByteArr(str2);
              Array.Resize<byte>(ref array[index1], array[index1].Length + numArray.Length);
              Array.Copy((Array) numArray, 0, (Array) array[index1], array[index1].Length - numArray.Length, numArray.Length);
              CompAddress += 4U;
            }
            else
            {
              ++index1;
              Array.Resize<byte[]>(ref array, array.Length + 1);
            }
          }
        }
      }
      if (this.HookMode != 0)
      {
        string str1 = "b";
        if (this.HookMode == 1)
          str1 = "bl";
        this.CompAddress = this.CompHook;
        string str2 = misc.sRight(this.ASMToHex(str1 + " 0x" + (this.FirstCompAddr - 4U).ToString("X8")), 8);
        int index2 = index1 + 1;
        Array.Resize<byte[]>(ref array, array.Length + 1);
        array[index2] = misc.StringToByteArr(str2);
      }
      return array;
    }
    public string ASMAssembleToNC(string ASMBox, string DefBox)
    {
      string[] strArray = InitAssembly(ASMBox, DefBox, Application.StartupPath);
      string str1 = "";
      for (int index = 0; index < strArray.Length; ++index)
      {
        while (index < strArray.Length && strArray[index] == "")
          ++index;
        if (index < strArray.Length)
        {
          str1 = ASMToHex(ParseASMText(strArray[index]));
          if (str1 != null && str1 != "")
          {
             str1 = str1; //str1 += str1;
             CompAddress += 4U; //4u
          }
        }
        else
          break;
      }
      if (importDef != "" && importDef != "\n" && importDef != "\r" && importDef != "\r\n")
      {
        bool flag = false;
        string str2 = Regex.Replace(importDef, re, "$1");
        char[] chArray = new char[1]{'\n'};
        foreach (string line in str2.Split(chArray))
        {
          string str3 = (string) null;
          if (line != "")
            str3 = ASMToHex(ParseASMText(line));
          if (str3 != null && str3 != "")
          {
            if (!flag && (int) str3[1] == 32)
            {
              str1 = str1 + Environment.NewLine + Environment.NewLine;
              flag = true;
            }
            str1 += str3; // str1 += str3;
            CompAddress += 4U; //4u
          }
        }
      }
      if (HookMode != 0)
      {
        string str2 = "b";
        if (HookMode == 1)
          str2 = "bl";
        CompAddress = CompHook;
        string str3 = misc.sRight(ASMToHex(str2 + " 0x" + (this.FirstCompAddr - 4U).ToString("X8")), 8);
        str1 = str1 + Environment.NewLine + "0 " + CompHook.ToString("X8") + " " + str3 + Environment.NewLine;
      }
      return str1.Trim(Environment.NewLine.ToCharArray()) + Environment.NewLine;
    }
    public string GetImports(string rtb, string filePath)
    {
      TextBox rtb1 = new TextBox();
      rtb1.Text = rtb;
      if (filePath == "")
        return rtb;
      TextBox[] rtb2 = ReadImport(rtb1, new FileInfo(filePath).Directory.FullName);
      bool flag = true;
      while (flag)
      {
        int skip = 0;
        flag = false;
        impOffset = rtb2.Length;
        rtb2 = ParseImports(rtb2, skip);
        for (int index = impOffset; index < rtb2.Length; ++index)
        {
          if (Regex.IsMatch(rtb2[index].Text, "import"))
            flag = true;
        }
        int num = impOffset + 1;
      }
      for (int index = 0; index < rtb2.Length; ++index)
      {
        TextBox textBox = rtb1;
        string str = textBox.Text + "\n" + rtb2[index].Text;
        textBox.Text = str;
      }
      rtb1.Text += "\n";
      return rtb1.Text;
    }
    public TextBox[] ParseImports(TextBox[] rtb, int skip)
    {
      TextBox[] array = (TextBox[]) rtb.Clone();
      for (int index = skip; index < rtb.Length; ++index)
      {
        int num = 0;
        while (num >= 0)
        {
          num = rtb[index].Text.IndexOf("import ", num + 1);
          if (num >= 0)
          {
            int length = array.Length;
            TextBox[] textBoxArray = ReadImport(rtb[index], (string) rtb[index].Tag);
            if (textBoxArray != null && textBoxArray.Length != 0)
            {
              Array.Resize<TextBox>(ref array, array.Length + textBoxArray.Length);
              Array.Copy(textBoxArray, 0, array, array.Length - textBoxArray.Length, textBoxArray.Length);
            }
          }
        }
      }
      return array;
    }
    public TextBox[] ReadImport(TextBox rtb, string GlobalFileName)
    {
      string str1 = rtb.Text.ToLower();
      TextBox[] array = new TextBox[0];
      if (GlobalFileName == null)
        return (TextBox[]) null;
      int num1 = str1.IndexOf("import");
      while (num1 >= 0)
      {
        if (num1 >= 0 && num1 < rtb.Text.Length)
        {
          int num2 = str1.IndexOf("\r", num1);
          if (num2 < 0)
            num2 = str1.IndexOf("\n", num1);
          if (num2 < 0)
            num2 = rtb.Text.Length;
          string str2 = "";
          string[] strArray = misc.sMid(rtb.Text, num1, num2 - num1).Split(' ');
          for (int index = 1; index < strArray.Length; ++index)
            str2 = str2 + strArray[index] + " ";
          string fileName = str2.Trim(' ');
          if (!fileName.StartsWith("Map"))
            ;
          if (fileName.IndexOf(':') < 0)
            fileName = GlobalFileName + "\\" + fileName;
          string fullName = new FileInfo(fileName).FullName;
          if (!isLoaded(fullName, importPaths.Length))
          {
            Array.Resize<string>(ref importPaths, importPaths.Length + 1);
            importPaths[importPaths.Length - 1] = fullName;
          }
          num1 = str1.IndexOf("import", num1 + 1);
        }
      }
      for (int ind = impOffset; ind < importPaths.Length; ++ind)
      {
        if (importPaths[ind].IndexOf(':') >= 0 && !isLoaded(importPaths[ind], ind))
        {
          if (File.Exists(importPaths[ind]))
          {
            Array.Resize<TextBox>(ref array, array.Length + 1);
            array[array.Length - 1] = new TextBox();
            string[] strArray = misc.LoadCWP3(importPaths[ind]);
            if (strArray.Length >= 1)
              array[array.Length - 1].Text = misc.OpenFile(strArray[0]);
            if (strArray.Length >= 2)
            {
              CW cw = this;
              string str2 = cw.importDef + misc.OpenFile(strArray[1]) + "\n";
              cw.importDef = str2;
            }
            CW.importLoaded[ind] = true;
            array[array.Length - 1].Tag = (object) new FileInfo(importPaths[ind]).Directory.FullName;
          }
          else
          {
            CW cw = this;
            string str2 = cw.debugString + "Imported file " + importPaths[ind] + " doesn't exist!" + Environment.NewLine;
            cw.debugString = str2;
          }
        }
        else if (GlobalFileName != "" && !isLoaded(importPaths[ind], ind))
        {
          string fullName = new FileInfo(GlobalFileName + "\\" + importPaths[ind]).FullName;
          if (File.Exists(fullName) && !isLoaded(fullName, ind))
          {
            Array.Resize<TextBox>(ref array, array.Length + 1);
            array[array.Length - 1] = new TextBox();
            string[] strArray = misc.LoadCWP3(fullName);
            if (strArray.Length >= 1)
              array[array.Length - 1].Text = misc.OpenFile(strArray[0]);
            if (strArray.Length >= 2)
            {
              CW cw = this;
              string str2 = cw.importDef + misc.OpenFile(strArray[1]) + "\n";
              cw.importDef = str2;
            }
            CW.importLoaded[ind] = true;
            array[array.Length - 1].Tag = (object) new FileInfo(fullName).Directory.FullName;
          }
          else if (File.Exists(fullName))
          {
            CW cw = this;
            string str2 = cw.debugString + "Imported file " + fullName + " doesn't exist!" + Environment.NewLine;
            cw.debugString = str2;
          }
        }
        else if (GlobalFileName == "")
        {
          CW cw = this;
          string str2 = cw.debugString + "You must save the file before importing other files!" + Environment.NewLine;
          cw.debugString = str2;
        }
      }
      return array;
    }
    public bool isLoaded(string file, int ind)
    {
      if (this.importPaths.Length <= 0)
        return false;
      Array.Resize<bool>(ref CW.importLoaded, this.importPaths.Length + 1);
      if (CW.importLoaded[ind])
        return true;
      --ind;
      for (int index = ind; index >= 0; --index)
      {
        if (new FileInfo(this.importPaths[index]).FullName == new FileInfo(file).FullName)
          return true;
      }
      return false;
    }

    public string ASMToHex(string argASM)
    {
      string[] str1 = argASM.Trim().Split(' ');
      uint retVal = 0U;
      if (str1[0] == "" || str1[0] == null)
        return (string) null;
      if (str1[0].ToLower() == "address")
      {
        if ((int) CompAddress == 0)
        {
          FirstCompAddr = (uint) ParseVal(str1[1], 1);
          CompAddress = FirstCompAddr;
        }
        else
          CompAddress = (uint) ParseVal(str1[1], 1);
        return Environment.NewLine + "0 " + CompAddress.ToString("X8") + " ";
      }
      if (str1[0].ToLower() == "hook")
      {
        CompHook = (uint) ParseVal(str1[1], 1);
        HookMode = 2;
        return (string) null;
      }
      if (str1[0].ToLower() == "hookl")
      {
        CompHook = (uint) ParseVal(str1[1], 1);
        HookMode = 1;
        return (string) null;
      }
      if (str1[0].ToLower() == "setreg")
      {
        if (str1.Length != 3)
        {
          CW cw = this;
          string str2 = cw.debugString + Environment.NewLine + "Too few arguments for setreg: \"" + argASM + "\"";
          cw.debugString = str2;
          return (string) null;
        }
        long num = ParseVal(str1[2], 1);
        string str3 = misc.sLeft(num.ToString("X8"), 4);
        string str4 = misc.sRight(num.ToString("X8"), 4);
        string str5 = ASMToHex("lis " + str1[1] + " 0x" + str3) + ASMToHex("ori " + str1[1] + " " + str1[1] + " 0x" + str4);
        CompAddress += 4U;
        return str5;
      }
      if (str1[0].ToLower() == "hexcode")
        return ParseVal(str1[1], 1).ToString("X8");
      if (str1[0].ToLower() == "import")
        return (string) null;
      if (str1[0].ToLower() == "float")
        return BitConverter.ToUInt32(BitConverter.GetBytes(float.Parse(str1[1])), 0).ToString("X8");
      if (str1[0].ToLower() == "string")
      {
        string str2 = str1[1];
        for (int index = 2; index < str1.Length; ++index)
          str2 = str2 + " " + str1[index];
        byte[] numArray = misc.StringToByteArr(str2);
        string str3 = "";
        foreach (byte num in numArray)
          str3 += num.ToString("X2");
        string str4 = str3.PadRight(str3.Length + (4 - str3.Length % 4) * 2, '0');
        this.CompAddress -= (uint) (str4.Length / 2);
        return str4;
      }
      for (int insStart = GetInsStart((int) str1[0][0]); insStart < ASMDef.Length; ++insStart)
      {
        if (str1[0] == ASMDef[insStart].name && IsCorrectSize(ASMDef[insStart].order, str1.Length - 1, ASMDef[insStart].type))
        {
          if (ASMDef[insStart].order == null)
            return ASMDef[insStart].opCode.ToString("X8");
          int[] regs = ParseRegs(str1, ASMDef[insStart].order.Length);
          if (regs == null)
          {
            CW cw = this;
            string str2 = cw.debugString + Environment.NewLine + "Error: Missing argument(s) in \"" + argASM + "\"";
            cw.debugString = str2;
            return (string) null;
          }
          retVal = ASMDef[insStart].opCode;
          switch (ASMDef[insStart].type)
          {
            case 0:
            case 5:
            case 9:
              for (int index = 0; index < ASMDef[insStart].shifts.Length; ++index)
                retVal |= (uint) (regs[ASMDef[insStart].order[index]] << ASMDef[insStart].shifts[index]);
              goto label_58;
            case 1:
              int index1;
              for (index1 = 0; index1 < ASMDef[insStart].shifts.Length - 1; ++index1)
                retVal |= (uint) (regs[ASMDef[insStart].order[index1]] << ASMDef[insStart].shifts[index1]);
              int num1 = (int) (((long) regs[ASMDef[insStart].order[index1]] - (long)CompAddress + 4L) / 4L) << ASMDef[insStart].shifts[index1];
              retVal |= (uint) (num1 << 6) >> 6;
              goto label_58;
            case 3:
            case 4:
            case 7:
            case 10:
            case 11:
              int index2;
              for (index2 = 0; index2 < ASMDef[insStart].shifts.Length - 1; ++index2)
                retVal |= (uint) (regs[ASMDef[insStart].order[index2]] << ASMDef[insStart].shifts[index2]);
              retVal |= (uint) (ushort) (regs[ASMDef[insStart].order[index2]] << ASMDef[insStart].shifts[index2]);
              goto label_58;
            case 6:
              int index3;
              for (index3 = 0; index3 < ASMDef[insStart].shifts.Length - 1; ++index3)
                retVal |= (uint) (regs[ASMDef[insStart].order[index3]] << ASMDef[insStart].shifts[index3]);
              uint num2 = ValState == vstLab ? CompAddress - 4U : 0U;
              int num3 = (int) (((long) regs[ASMDef[insStart].order[index3]] - (long) num2) / 4L) << ASMDef[insStart].shifts[index3];
              if (!this.isBranchCompareRelative)
                num3 -= (int) CompAddress;
              retVal |= (uint) (ushort) num3;
              goto label_58;
            case 8:
              if (ASMDef[insStart].name.ToLower() == "srdi" && regs[ASMDef[insStart].order[2]] < 32)
              {
                CW cw = this;
                string str2 = cw.debugString + "srdi cannot have a shift less than 32!" + Environment.NewLine;
                cw.debugString = str2;
              }
              retVal |= AssembleRotate(ASMDef[insStart].name, regs, insStart, retVal);
              goto label_58;
            default:
              goto label_58;
          }
        }
      }
label_58:
      if ((int) retVal != 0)
        return retVal.ToString("X8");
      if ((int) str1[0].ToLower()[str1[0].Length - 1] != 58)
      {
        CW cw = this;
        string str2 = cw.debugString + Environment.NewLine + "\"" + argASM + "\" is either missing argument(s) or is not valid";
        cw.debugString = str2;
      }
      return (string)null;
    }
    public uint AssembleRotate(string op, int[] regs, int asmDefIndex, uint retVal)
    {
      int index;
      for (index = 0; index < this.ASMDef[asmDefIndex].shifts.Length - 1; ++index)
        retVal |= (uint) (regs[this.ASMDef[asmDefIndex].order[index]] << this.ASMDef[asmDefIndex].shifts[index]);
      retVal |= (uint) (ushort) (regs[this.ASMDef[asmDefIndex].order[index]] << this.ASMDef[asmDefIndex].shifts[index]);
      int num = regs[this.ASMDef[asmDefIndex].order[index]];
      switch (op)
      {
        case "slwi":
          retVal |= (uint) (ushort) (31 - regs[this.ASMDef[asmDefIndex].order[index]] << 1);
          break;
        case "srwi":
          retVal |= (uint) (ushort) (32 - regs[this.ASMDef[asmDefIndex].order[index]] << 11);
          retVal |= 62U;
          break;
        case "sldi":
          retVal = this.AssembleRLDICR((ushort) regs[this.ASMDef[asmDefIndex].order[0]], (ushort) regs[this.ASMDef[asmDefIndex].order[1]], (uint) regs[this.ASMDef[asmDefIndex].order[2]]);
          break;
        case "srdi":
          retVal = this.AssembleRLDICL((ushort) regs[this.ASMDef[asmDefIndex].order[0]], (ushort) regs[this.ASMDef[asmDefIndex].order[1]], (uint) regs[this.ASMDef[asmDefIndex].order[2]]);
          break;
      }
      return retVal;
    }
    private uint AssembleRLDICR(ushort rA, ushort rS, uint n)
    {
      uint num1 = (uint) (2013265924 | (int) rS << 16) | (uint) rA << 21;
      uint num2 = 31U - n;
      uint num3;
      if (n > 31U)
      {
        num3 = num1 | 2U;
        num2 = 63U - n;
      }
      else
        num3 = num1 | 32U;
      return num3 | (uint) (ushort) (num2 << 6) | (uint) (ushort) (n << 11);
    }
    private uint AssembleRLDICL(ushort rA, ushort rS, uint n)
    {
      uint num1 = 2013265920U;
      if (n < 32U)
      {
        CW cw = this;
        string str = cw.debugString + "srdi cannot have a shift less than 32!" + Environment.NewLine;
        cw.debugString = str;
      }
      uint num2 = num1 | (uint) rS << 16 | (uint) rA << 21;
      uint num3 = n - 32U;
      uint num4;
      if (n > 32U)
      {
        n = 64U - n;
        num4 = num2 | (uint) (ushort) (num3 << 6) | (uint) (ushort) (n << 11) | 32U;
      }
      else
        num4 = num2 | (uint) (ushort) n | 2U;
      return num4;
    }
    public bool IsCorrectSize(int[] order, int regNum, int type)
    {
      if (order == null && regNum == 0)
        return true;
      if (order == null)
        return false;
      switch (type)
      {
        case 3:
        case 10:
          if (regNum + 1 == order.Length)
            return true;
          break;
        default:
          if (regNum == order.Length)
            return true;
          break;
      }
      return false;
    }
    private string ParseASMText(string line)
    {
      string[] strArray = line.Trim(' ', '\t').Replace('\t', ' ').Split(' ');
      string str1 = "";
      foreach (string str2 in strArray)
      {
        if (str2 != "")
          str1 = str1 + str2 + " ";
      }
      return str1.Trim(' ');
    }
    public long ParseVal(string str, int mode)
    {
      ValState = 0;
      if (str == "" || str == null)
        return 0L;
      str = str.Replace(",", "");
      CW.ASMLabel asmLabel = isLabel(str);
      if (asmLabel.name != null)
      {
        ValState = vstLab;
        return (long) asmLabel.address;
      }
      string str1 = str;
      str = str.ToLower();
      if (str.IndexOf('x') >= 0 || str.IndexOf('$') >= 0)
      {
        ValState = vstHex;
        str = str.Replace("0x", "");
        str = str.Replace("$", "");
        bool flag = str.StartsWith("-");
        if (flag)
          str = str.Replace("-", "");
        try
        {
          uint num = mode != 0 ? (uint) long.Parse(str, NumberStyles.HexNumber) : (uint) (ushort) long.Parse(str, NumberStyles.HexNumber);
          if (flag)
            return (long) -num;
          return (long) num;
        }
        catch
        {
          CW cw = this;
          string str2 = cw.debugString + Environment.NewLine + "Error: Hexadecimal value: 0x" + str.ToUpper() + " is not a valid hexadecimal value";
          cw.debugString = str2;
          return 0L;
        }
      }
      else
      {
        this.ValState = this.vstDec;
        try
        {
          bool flag = str.StartsWith("-");
          if (flag)
            str = str.Replace("-", "");
          uint num = mode != 0 ? (uint) long.Parse(str) : (uint) (ushort) long.Parse(str);
          if (flag)
            return (long) -num;
          return (long) num;
        }
        catch
        {
          CW cw = this;
          string str2 = cw.debugString + Environment.NewLine + "Error: Label " + str1 + " hasn't been declared (or invalid decimal value)";
          cw.debugString = str2;
          return 0L;
        }
      }
    }
    public int[] ParseRegs(string[] str, int size)
    {
      if (str.Length < 2)
        return (int[]) null;
      int[] numArray = new int[size];
      for (int index = 0; index < str.Length - 1; ++index)
      {
        string str1 = str[index + 1].Replace(",", "");
        string str2 = str1.ToLower();
        if (this.isLabel(str1).name != null)
          numArray[index] = (int) this.ParseVal(str[index + 1], 1);
        else if (str1.IndexOf('(') >= 0)
        {
          numArray[index] = this.ParseOffImm(str2);
          numArray[index + 1] = this.ParseRegsImm;
        }
        else if (str2.IndexOf("xer") >= 0 || str2.IndexOf("lr") >= 0 || str2.IndexOf("ctr") >= 0)
        {
          numArray[index] = this.SPRegToDec(str2);
        }
        else
        {
          int num = str2.IndexOf('r') >= 0 ? 0 : (str2.IndexOf('f') != 0 ? 1 : (str2.Length > 3 ? 1 : 0));
          numArray[index] = num != 0 ? (int) ParseVal(str1, 1) : RegToDec(str2);
        }
      }
      return numArray;
    }
    public int ParseOffImm(string str)
    {
      string[] strArray = str.Split('(');
      this.ParseRegsImm = (int) this.ParseVal(strArray[0], 0);
      return this.RegToDec(strArray[1].Replace(")", ""));
    }
    public int RegToDec(string reg)
    {
      string str1 = reg;
      if ((int) reg[0] == 58)
        return 0;
      reg = reg.ToLower();
      reg = reg.Replace("c", "");
      reg = reg.Replace("r", "");
      reg = reg.Replace(",", "");
      reg = reg.Replace("%", "");
      reg = reg.Replace("f", "");
      try
      {
        int num = int.Parse(reg.Replace("$", ""));
        if (num >= 0 && num <= 31)
          return num;
        CW cw = this;
        string str2 = cw.debugString + "Register " + str1 + " is not a valid register!";
        cw.debugString = str2;
        return 0;
      }
      catch
      {
        CW cw = this;
        string str2 = cw.debugString + "Register " + str1 + " is not a valid register!";
        cw.debugString = str2;
        return 0;
      }
    }
    public int SPRegToDec(string reg)
    {
      switch (reg.ToLower())
      {
        case "xer":
          return 1;
        case "lr":
          return 8;
        case "ctr":
          return 9;
        default:
          return 0;
      }
    }

    public CW.ASMLabel[] ProcessLabels(string[] strArray)
    {
      uint num1 = 0U;
      CW.ASMLabel[] array = new CW.ASMLabel[0];
      for (int index1 = 0; index1 < strArray.Length; ++index1)
      {
        string[] strArray1 = ParseASMText(strArray[index1]).Split(' ');
        int num2 = strArray[index1].IndexOf(':');
        if (strArray1[0].ToLower() == "address")
          num1 = (uint) ParseVal(strArray1[1], 1);
        else if (strArray1[0].ToLower() == "hexcode")
          num1 += 4U;
        else if (strArray1[0].ToLower() == "setreg")
          num1 += 8U;
        else if (strArray1[0].ToLower() == "float")
          num1 += 4U;
        else if (strArray1[0].ToLower() == "string")
        {
          string str = strArray1[1];
          for (int index2 = 2; index2 < strArray1.Length; ++index2)
            str = str + " " + strArray1[index2];
          int num3 = 4 - str.Length % 4;
          if (num3 != 4)
            num1 += (uint) (num3 + str.Length);
          else
            num1 += (uint) str.Length;
        }
        else if (num2 > 0 && (int) strArray[index1][num2 - 1] != 32 && (int) strArray[index1][num2 - 1] != 44)
        {
          if (strArray1.Length > 1 && strArray1[0].IndexOf(":") < 0)
          {
            CW cw = this;
            string str = cw.debugString + Environment.NewLine + "Error: Label at (" + strArray[index1] + ") accidently declared instead of called";
            cw.debugString = str;
            return (CW.ASMLabel[]) null;
          }
          Array.Resize<CW.ASMLabel>(ref array, array.Length + 1);
          array[array.Length - 1].address = num1;
          array[array.Length - 1].name = strArray1[0].Replace(":", "");
          for (int index2 = 0; index2 < array.Length; ++index2)
          {
            if (array[index2].name == array[array.Length - 1].name && index2 != array.Length - 1)
            {
              CW cw = this;
              string str = cw.debugString + Environment.NewLine + "Error: Label (" + array[array.Length - 1].name + ") duplication";
              cw.debugString = str;
              return (CW.ASMLabel[]) null;
            }
          }
        }
        else if (IsInstruction(strArray1[0]))
          num1 += 4U;
      }
      return array;
    }
    public CW.ASMLabel isLabel(string label)
    {
      if (labels == null)
        return new CW.ASMLabel();
      label = label.Replace(":", "").ToLower();
      foreach (CW.ASMLabel asmLabel in labels)
      {
        if (label == asmLabel.name.ToLower())
          return asmLabel;
      }
      return new CW.ASMLabel();
    }

    public bool IsInstruction(string asm)
    {
      if (asm == "")
        return false;
      for (int insStart = GetInsStart((int) asm[0]); insStart < ASMDef.Length && ASMDef[insStart].name != null; ++insStart)
      {
        if (asm == ASMDef[insStart].name)
          return true;
      }
      return false;
    }
    public string ASMDisassemble(string hexCode)
    {
      string str1 = "";
      string[] strArray1 = Regex.Replace(hexCode, re, "$1").Split('\r');
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        if (strArray1[index1] != "")
        {
          if (strArray1[index1].Length <= 8 && strArray1[index1] != "\n")
            this.addr = false;
          strArray1[index1] = strArray1[index1].Replace("\n", "");
          string[] strArray2 = strArray1[index1].Split(' ');
          string text = strArray2[strArray2.Length - 1];
          if (text != "")
          {
            if (this.addr)
            {
              string s = misc.sMid(strArray2[1], 1, 8);
              if ((int) this.DisASMAddr != (int) uint.Parse(s, NumberStyles.HexNumber))
              {
                this.DisASMAddr = uint.Parse(s, NumberStyles.HexNumber);
                str1 = str1 + "address 0x" + s + "\n\n";
              }
            }
            int index2 = 0;
            while (index2 < text.Length)
            {
              string str2 = misc.sMid(text, index2, 8);
              uint val;
              try
              {
                val = Convert.ToUInt32(str2, 16);
              }
              catch (Exception ex)
              {
                goto label_18;
              }
              int insFromHex = this.GetInsFromHex(val);
              if (this.addr)
                this.DisASMAddr += 4U;
              if (insFromHex < 0)
              {
                str1 = str1 + "hexcode 0x" + text + "\n";
              }
              else
              {
                string str3 = this.DisASMReg(insFromHex, val);
                str1 = str1 + this.ASMDef[insFromHex].name + " " + str3 + "\n";
              }
label_18:
              index2 += 8;
            }
          }
        }
      }
      if (this.ASMDisMode == 0)
      {
        RichTextBox richTextBox = new RichTextBox();
        richTextBox.Text = str1;
        this.DisASMAddr = 0U;
        bool[] flagArray = new bool[this.DisASMLabel.Length];
        bool flag = false;
        for (int index1 = 0; index1 < richTextBox.Lines.Length; ++index1)
        {
          if (richTextBox.Lines[index1] != "")
          {
            string[] strArray2 = richTextBox.Lines[index1].Split(' ');
            if (strArray2[0] == "address")
            {
              this.DisASMAddr = uint.Parse(strArray2[1].Replace("0x", "").Replace("$", "").Replace(" ", ""), NumberStyles.HexNumber);
              flag = true;
            }
            for (int index2 = 0; index2 < this.DisASMLabel.Length; ++index2)
            {
              if ((int) this.DisASMAddr == (int) this.DisASMLabel[index2].address)
              {
                int startIndex = 0;
                if (flag)
                {
                  for (int index3 = 0; index3 <= index1; ++index3)
                    startIndex += richTextBox.Lines[index3].Length + 1;
                }
                else
                {
                  for (int index3 = 0; index3 < index1; ++index3)
                    startIndex += richTextBox.Lines[index3].Length + 1;
                }
                richTextBox.Text = richTextBox.Text.Insert(startIndex, this.DisASMLabel[index2].name + ":\n");
                ++index1;
                flagArray[index2] = true;
                break;
              }
            }
            if (strArray2[0] != "hook" && strArray2[0] != "hookl")
              this.DisASMAddr += 4U;
            if (strArray2[0] == "setreg")
              this.DisASMAddr += 4U;
            flag = false;
          }
        }
        string str2 = richTextBox.Text;
        richTextBox.Dispose();
        if (!this.addr)
          str2 = "address 0x00000000\n\n" + str2;
        str1 = str2.Replace("blr", "blr\n");
        for (int index = 0; index < flagArray.Length; ++index)
        {
          if (!flagArray[index])
            str1 = str1 + "\naddress 0x" + this.DisASMLabel[index].address.ToString("X8") + "\n" + this.DisASMLabel[index].name + ":\n";
        }
      }
      else
      {
        for (int index = 0; index < this.DisASMLabel.Length; ++index)
          str1 = str1.Replace(":" + this.DisASMLabel[index].name, "0x" + this.DisASMLabel[index].address.ToString("X8"));
      }
      this.DisASMAddr = 0U;
      this.DisASMLabel = new CW.ASMLabel[0];
      this.addr = true;
      return str1;
    }
    public int GetInsFromHex(uint val)
    {
      int index = 0;
      int num = 0;
      for (; this.ASMDef[index].name != null; ++index)
      {
        uint val1 = val << this.ASMDef[index].opShift[0] >> this.ASMDef[index].opShift[0] + this.ASMDef[index].opShift[1] << this.ASMDef[index].opShift[1] & ~this.ASMDef[index].opCode;
        if ((int) (val - this.ASMDef[index].opCode) == (int) val1 && ((int) val & (int) this.ASMDef[index].opCode) == (int) this.ASMDef[index].opCode)
        {
          if (this.ASMDef[index].name == "addi" && this.GetStrFromBit(16, 5, val1) == "0")
            num = 1;
          else if (this.ASMDef[index].name == "addis" && this.GetStrFromBit(16, 5, val1) == "0")
            num = 1;
          else if (this.ASMDef[index].name == "cmpw" && this.GetStrFromBit(0, 8, val1) != "0")
            num = 1;
          else if (this.ASMDef[index].name == "std" && this.GetStrFromBit(0, 1, val1) != "0")
            num = 1;
          else if (this.ASMDef[index].name == "slwi" && this.GetStrFromBit(1, 5, val1) == "31")
            num = 1;
          else if (this.ASMDef[index].type == 2 && (int) val != (int) this.ASMDef[index].opCode)
            num = 1;
          if (num == 0)
            return index;
          num = 0;
        }
      }
      return -1;
    }
    public string DisASMReg(int index, uint val)
    {
      if (this.ASMDef[index].shifts == null)
        return "";
      string str1 = "";
      string[] strArray = (string[]) null;
      string preReg = "r";
      if (this.ASMDef[index].type == 9 || this.ASMDef[index].type == 10)
        preReg = "f";
      switch (this.ASMDef[index].type)
      {
        case 0:
        case 9:
          strArray = new string[this.ASMDef[index].shifts.Length];
          for (int index1 = 0; index1 < this.ASMDef[index].shifts.Length; ++index1)
            strArray[this.ASMDef[index].order[index1]] = preReg + this.GetStrFromBit(this.ASMDef[index].shifts[index1], 5, val);
          break;
        case 1:
          strArray = new string[this.ASMDef[index].shifts.Length];
          uint num1 = uint.Parse(this.GetStrFromBit(2, 23, val)) << 2;
          uint num2 = this.DisASMAddr;
          uint num3 = (num1 < 25165824U ? num2 + Convert.ToUInt32(num1) : num2 + Convert.ToUInt32(num1 - 33554432U)) - 4U;
          string str2 = "";
          if (this.addr)
          {
            str2 = "loc_" + num3.ToString("X");
            int num4 = 0;
            for (int index1 = 0; index1 < this.DisASMLabel.Length; ++index1)
            {
              if ((int) this.DisASMLabel[index1].address == (int) num3)
              {
                num4 = 1;
                break;
              }
            }
            if (num4 == 0)
            {
              Array.Resize<CW.ASMLabel>(ref this.DisASMLabel, this.DisASMLabel.Length + 1);
              this.DisASMLabel[this.DisASMLabel.Length - 1].address = num3;
              this.DisASMLabel[this.DisASMLabel.Length - 1].name = str2;
            }
          }
          int index2;
          for (index2 = 0; index2 < this.ASMDef[index].shifts.Length - 1; ++index2)
            strArray[this.ASMDef[index].order[index2]] = "cr" + this.GetStrFromBit(this.ASMDef[index].shifts[index2], 3, val);
          strArray[this.ASMDef[index].order[index2]] = !this.addr ? "0x" + (num3 + 4U).ToString("X8") : ":" + str2;
          break;
        case 2:
          return "";
        case 3:
        case 10:
          strArray = new string[this.ASMDef[index].shifts.Length - 1];
          int index3;
          for (index3 = 0; index3 < this.ASMDef[index].shifts.Length - 1; ++index3)
            strArray[this.ASMDef[index].order[index3]] = preReg + this.GetStrFromBit(this.ASMDef[index].shifts[index3], 5, val);
          strArray[this.ASMDef[index].order[index3 - 1]] = strArray[this.ASMDef[index].order[index3 - 1]].Replace("f", "r");
          if (this.ASMDef[index].name == "stdu")
            --val;
          strArray[this.ASMDef[index].order[index3 - 1]] = "0x" + misc.sRight(val.ToString("X"), 4) + "(" + strArray[this.ASMDef[index].order[index3 - 1]] + ")";
          break;
        case 4:
          strArray = new string[this.ASMDef[index].shifts.Length];
          int index4;
          for (index4 = 0; index4 < this.ASMDef[index].shifts.Length - 1; ++index4)
            strArray[this.ASMDef[index].order[index4]] = preReg + this.GetStrFromBit(this.ASMDef[index].shifts[index4], 5, val);
          strArray[this.ASMDef[index].order[index4]] = "0x" + misc.sRight(val.ToString("X"), 4);
          break;
        case 5:
          strArray = new string[this.ASMDef[index].shifts.Length];
          strArray[this.ASMDef[index].order[0]] = this.StrToSPR(this.GetStrFromBit(this.ASMDef[index].shifts[0], 4, val));
          for (int index1 = 1; index1 < this.ASMDef[index].shifts.Length; ++index1)
            strArray[this.ASMDef[index].order[index1]] = preReg + this.GetStrFromBit(this.ASMDef[index].shifts[index1], 5, val);
          break;
        case 6:
          int num5 = 4 * int.Parse(this.GetStrFromBit(2, 14, val));
          if (int.Parse(this.GetStrFromBit(this.ASMDef[index + 1].shifts[0], 3, val)) != 0)
            ++index;
          strArray = new string[this.ASMDef[index].shifts.Length];
          int index5;
          for (index5 = 0; index5 < this.ASMDef[index].shifts.Length - 1; ++index5)
            strArray[this.ASMDef[index].order[index5]] = "cr" + this.GetStrFromBit(this.ASMDef[index].shifts[index5], 3, val);
          strArray[this.ASMDef[index].order[index5]] = this.isBranchCompareRelative ? ((int) (short) num5 >= 0 ? "0x" + num5.ToString("X") : "-0x" + ((int) -(short) num5).ToString("X")) : "0x" + ((long) (this.DisASMAddr - 4U) + (long) (short) num5).ToString("X8");
          break;
        case 7:
          int num6 = int.Parse(GetStrFromBit(ASMDef[index + 1].shifts[0], 3, val));
          if (num6 != 0)
            ++index;
          strArray = new string[ASMDef[index].shifts.Length];
          int num7 = 0;
          if (num6 != 0)
          {
            strArray[ASMDef[index].order[0]] = "cr" + GetStrFromBit(ASMDef[index].shifts[0], 3, val);
            num7 = 1;
          }
          if ((int) ASMDef[index].name[ASMDef[index].name.Length - 1] == 105)
          {
            int index1;
            for (index1 = num7; index1 < ASMDef[index].shifts.Length - 1; ++index1)
              strArray[ASMDef[index].order[index1]] = preReg + GetStrFromBit(ASMDef[index].shifts[index1], 5, val);
            strArray[ASMDef[index].order[index1]] = "0x" + misc.sRight(val.ToString("X"), 4);
            break;
          }
          for (int index1 = num7; index1 < ASMDef[index].shifts.Length; ++index1)
            strArray[ASMDef[index].order[index1]] = "r" + GetStrFromBit(ASMDef[index].shifts[index1], 5, val);
          break;
        case 8:
          strArray = DisassembleRotate(index, preReg, val);
          break;
        case 11:
          int num8 = int.Parse(GetStrFromBit(ASMDef[index + 1].shifts[0], 3, val));
          if (num8 != 0)
            ++index;
          strArray = new string[ASMDef[index].shifts.Length];
          int num9 = 0;
          if (num8 != 0)
          {
            strArray[ASMDef[index].order[0]] = "cr" + GetStrFromBit(ASMDef[index].shifts[0], 3, val);
            num9 = 1;
          }
          for (int index1 = num9; index1 < ASMDef[index].shifts.Length; ++index1)
            strArray[ASMDef[index].order[index1]] = "f" + GetStrFromBit(ASMDef[index].shifts[index1], 5, val);
          break;
      }
      int index6;
      for (index6 = 0; index6 < strArray.Length - 1; ++index6)
        str1 = str1 + strArray[index6] + ", ";
      return str1 + strArray[index6];
    }
    public string[] DisassembleRotate(int index, string preReg, uint val)
    {
      string str = this.ASMDef[index].name;
      string[] strArray = new string[this.ASMDef[index].shifts.Length];
      int index1;
      for (index1 = 0; index1 < this.ASMDef[index].shifts.Length - 1; ++index1)
        strArray[this.ASMDef[index].order[index1]] = preReg + this.GetStrFromBit(this.ASMDef[index].shifts[index1], 5, val);
      switch (str)
      {
        case "slwi":
        case "srwi":
          strArray[this.ASMDef[index].order[index1]] = this.GetStrFromBit(this.ASMDef[index].shifts[index1], 5, val);
          break;
        case "sldi":
          strArray[this.ASMDef[index].order[index1]] = !(this.GetStrFromBit(1, 1, val) == "0") ? (int.Parse(this.GetStrFromBit(this.ASMDef[index].shifts[index1], 5, val)) + 32).ToString() : this.GetStrFromBit(this.ASMDef[index].shifts[index1], 5, val);
          break;
        case "srdi":
          strArray[this.ASMDef[index].order[index1]] = !(this.GetStrFromBit(1, 1, val) == "1") ? (64 - int.Parse(this.GetStrFromBit(this.ASMDef[index].shifts[this.ASMDef[index].order[index1]], 5, val))).ToString() : "32";
          break;
      }
      return strArray;
    }

    public string GetStrFromBit(int bitOff, int len, uint val)
    {
      return Convert.ToInt32(this.Reverse(misc.sMid(this.Reverse(Convert.ToString((long) val, 2).PadLeft(32, '0')), bitOff, len)), 2).ToString();
    }
    public string Reverse(string s)
    {
      char[] chArray = s.ToCharArray();
      Array.Reverse((Array) chArray);
      return new string(chArray);
    }
    public string StrToSPR(string val)
    {
      switch (val)
      {
        case "1":
          return "XER";
        case "8":
          return "LR";
        case "9":
          return "CTR";
        default:
          return "";
      }
    }
    public int GetRegHelpIndex(string reg)
    {
      switch (reg.ToLower())
      {
        case "r0":
          return 0;
        case "r1":
          return 1;
        case "r2":
          return 2;
        case "r3":
          return 3;
        case "r4":
        case "r5":
        case "r6":
        case "r7":
        case "r8":
        case "r9":
        case "r10":
          return 4;
        case "r11":
        case "r12":
          return 5;
        case "r13":
          return 6;
        case "r14":
        case "r15":
        case "r16":
        case "r17":
        case "r18":
        case "r19":
        case "r20":
        case "r21":
        case "r22":
        case "r23":
        case "r24":
        case "r25":
        case "r26":
        case "r27":
        case "r28":
        case "r29":
        case "r30":
        case "r31":
          return 7;
        case "f0":
        case "f1":
        case "f2":
        case "f3":
        case "f4":
        case "f5":
        case "f6":
        case "f7":
        case "f8":
        case "f9":
        case "f10":
        case "f11":
        case "f12":
        case "f13":
          return 8;
        case "f14":
        case "f15":
        case "f16":
        case "f17":
        case "f18":
        case "f19":
        case "f20":
        case "f21":
        case "f22":
        case "f23":
        case "f24":
        case "f25":
        case "f26":
        case "f27":
        case "f28":
        case "f29":
        case "f30":
        case "f31":
          return 9;
        case "cr0":
        case "cr1":
        case "cr2":
        case "cr3":
        case "cr4":
        case "cr5":
        case "cr6":
        case "cr7":
          return 10;
        case "xer":
          return 11;
        case "lr":
          return 12;
        case "ctr":
          return 13;
        default:
          return 0;
      }
    }

    public void DeclareInstructions()
    {
      ulong index1 = 0UL;
      this.RegColArr[index1].reg = "r" + index1.ToString();
      this.RegColArr[index1].col = Brushes.Firebrick;
      this.RegColArr[index1].title = "Register Zero";
      ulong index2 = index1 + 1UL;
      this.RegColArr[index2].reg = "r" + index2.ToString();
      this.RegColArr[index2].col = Brushes.BurlyWood;
      this.RegColArr[index2].title = "Stack Pointer";
      ulong index3 = index2 + 1UL;
      this.RegColArr[index3].reg = "r" + index3.ToString();
      this.RegColArr[index3].col = Brushes.Chocolate;
      this.RegColArr[index3].title = "Environment Pointer";
      ulong index4 = index3 + 1UL;
      this.RegColArr[index4].reg = "r" + index4.ToString();
      this.RegColArr[index4].col = Brushes.Violet;
      this.RegColArr[index4].title = "Argument 1 and Return Value";
      ulong index5 = index4 + 1UL;
      this.RegColArr[index5].reg = "r" + index5.ToString();
      this.RegColArr[index5].col = Brushes.Turquoise;
      this.RegColArr[index5].title = "Argument 2";
      ulong index6 = index5 + 1UL;
      this.RegColArr[index6].reg = "r" + index6.ToString();
      this.RegColArr[index6].col = Brushes.Turquoise;
      this.RegColArr[index6].title = "Argument 3";
      ulong index7 = index6 + 1UL;
      this.RegColArr[index7].reg = "r" + index7.ToString();
      this.RegColArr[index7].col = Brushes.Turquoise;
      this.RegColArr[index7].title = "Argument 4";
      ulong index8 = index7 + 1UL;
      this.RegColArr[index8].reg = "r" + index8.ToString();
      this.RegColArr[index8].col = Brushes.Turquoise;
      this.RegColArr[index8].title = "Argument 5";
      ulong index9 = index8 + 1UL;
      this.RegColArr[index9].reg = "r" + index9.ToString();
      this.RegColArr[index9].col = Brushes.Turquoise;
      this.RegColArr[index9].title = "Argument 6";
      ulong index10 = index9 + 1UL;
      this.RegColArr[index10].reg = "r" + index10.ToString();
      this.RegColArr[index10].col = Brushes.Turquoise;
      this.RegColArr[index10].title = "Argument 7";
      ulong index11 = index10 + 1UL;
      this.RegColArr[index11].reg = "r" + index11.ToString();
      this.RegColArr[index11].col = Brushes.Turquoise;
      this.RegColArr[index11].title = "Argument 8";
      ulong index12 = index11 + 1UL;
      this.RegColArr[index12].reg = "r" + index12.ToString();
      this.RegColArr[index12].col = Brushes.Wheat;
      this.RegColArr[index12].title = "Unknown";
      ulong index13 = index12 + 1UL;
      this.RegColArr[index13].reg = "r" + index13.ToString();
      this.RegColArr[index13].col = Brushes.Wheat;
      this.RegColArr[index13].title = "Unknown";
      ulong index14 = index13 + 1UL;
      this.RegColArr[index14].reg = "r" + index14.ToString();
      this.RegColArr[index14].col = Brushes.Brown;
      this.RegColArr[index14].title = "Data Area Pointer?";
      ulong index15 = index14 + 1UL;
      this.RegColArr[index15].reg = "r" + index15.ToString();
      this.RegColArr[index15].col = Brushes.Crimson;
      this.RegColArr[index15].title = "Saved Register";
      ulong index16 = index15 + 1UL;
      this.RegColArr[index16].reg = "r" + index16.ToString();
      this.RegColArr[index16].col = Brushes.Crimson;
      this.RegColArr[index16].title = "Saved Register";
      ulong index17 = index16 + 1UL;
      this.RegColArr[index17].reg = "r" + index17.ToString();
      this.RegColArr[index17].col = Brushes.Crimson;
      this.RegColArr[index17].title = "Saved Register";
      ulong index18 = index17 + 1UL;
      this.RegColArr[index18].reg = "r" + index18.ToString();
      this.RegColArr[index18].col = Brushes.Crimson;
      this.RegColArr[index18].title = "Saved Register";
      ulong index19 = index18 + 1UL;
      this.RegColArr[index19].reg = "r" + index19.ToString();
      this.RegColArr[index19].col = Brushes.Crimson;
      this.RegColArr[index19].title = "Saved Register";
      ulong index20 = index19 + 1UL;
      this.RegColArr[index20].reg = "r" + index20.ToString();
      this.RegColArr[index20].col = Brushes.Crimson;
      this.RegColArr[index20].title = "Saved Register";
      ulong index21 = index20 + 1UL;
      this.RegColArr[index21].reg = "r" + index21.ToString();
      this.RegColArr[index21].col = Brushes.Crimson;
      this.RegColArr[index21].title = "Saved Register";
      ulong index22 = index21 + 1UL;
      this.RegColArr[index22].reg = "r" + index22.ToString();
      this.RegColArr[index22].col = Brushes.Crimson;
      this.RegColArr[index22].title = "Saved Register";
      ulong index23 = index22 + 1UL;
      this.RegColArr[index23].reg = "r" + index23.ToString();
      this.RegColArr[index23].col = Brushes.Crimson;
      this.RegColArr[index23].title = "Saved Register";
      ulong index24 = index23 + 1UL;
      this.RegColArr[index24].reg = "r" + index24.ToString();
      this.RegColArr[index24].col = Brushes.Crimson;
      this.RegColArr[index24].title = "Saved Register";
      ulong index25 = index24 + 1UL;
      this.RegColArr[index25].reg = "r" + index25.ToString();
      this.RegColArr[index25].col = Brushes.Crimson;
      this.RegColArr[index25].title = "Saved Register";
      ulong index26 = index25 + 1UL;
      this.RegColArr[index26].reg = "r" + index26.ToString();
      this.RegColArr[index26].col = Brushes.Crimson;
      this.RegColArr[index26].title = "Saved Register";
      ulong index27 = index26 + 1UL;
      this.RegColArr[index27].reg = "r" + index27.ToString();
      this.RegColArr[index27].col = Brushes.Crimson;
      this.RegColArr[index27].title = "Saved Register";
      ulong index28 = index27 + 1UL;
      this.RegColArr[index28].reg = "r" + index28.ToString();
      this.RegColArr[index28].col = Brushes.Crimson;
      this.RegColArr[index28].title = "Saved Register";
      ulong index29 = index28 + 1UL;
      this.RegColArr[index29].reg = "r" + index29.ToString();
      this.RegColArr[index29].col = Brushes.Crimson;
      this.RegColArr[index29].title = "Saved Register";
      ulong index30 = index29 + 1UL;
      this.RegColArr[index30].reg = "r" + index30.ToString();
      this.RegColArr[index30].col = Brushes.Crimson;
      this.RegColArr[index30].title = "Saved Register";
      ulong index31 = index30 + 1UL;
      this.RegColArr[index31].reg = "r" + index31.ToString();
      this.RegColArr[index31].col = Brushes.Crimson;
      this.RegColArr[index31].title = "Saved Register";
      ulong index32 = index31 + 1UL;
      this.RegColArr[index32].reg = "r" + index32.ToString();
      this.RegColArr[index32].col = Brushes.Crimson;
      this.RegColArr[index32].title = "Saved Register";
      ulong index33 = index32 + 1UL;
      this.RegColArr[index33].reg = "cr0";
      this.RegColArr[index33].col = Brushes.LightSeaGreen;
      this.RegColArr[index33].title = "Compare Register 0";
      ulong index34 = index33 + 1UL;
      this.RegColArr[index34].reg = "cr1";
      this.RegColArr[index34].col = Brushes.LightSeaGreen;
      this.RegColArr[index34].title = "Compare Register 1";
      ulong index35 = index34 + 1UL;
      this.RegColArr[index35].reg = "cr2";
      this.RegColArr[index35].col = Brushes.LightSeaGreen;
      this.RegColArr[index35].title = "Compare Register 2";
      ulong index36 = index35 + 1UL;
      this.RegColArr[index36].reg = "cr3";
      this.RegColArr[index36].col = Brushes.LightSeaGreen;
      this.RegColArr[index36].title = "Compare Register 3";
      ulong index37 = index36 + 1UL;
      this.RegColArr[index37].reg = "cr4";
      this.RegColArr[index37].col = Brushes.LightSeaGreen;
      this.RegColArr[index37].title = "Compare Register 4";
      ulong index38 = index37 + 1UL;
      this.RegColArr[index38].reg = "cr5";
      this.RegColArr[index38].col = Brushes.LightSeaGreen;
      this.RegColArr[index38].title = "Compare Register 5";
      ulong index39 = index38 + 1UL;
      this.RegColArr[index39].reg = "cr6";
      this.RegColArr[index39].col = Brushes.LightSeaGreen;
      this.RegColArr[index39].title = "Compare Register 6";
      ulong index40 = index39 + 1UL;
      this.RegColArr[index40].reg = "cr7";
      this.RegColArr[index40].col = Brushes.LightSeaGreen;
      this.RegColArr[index40].title = "Compare Register 7";
      ulong index41 = index40 + 1UL;
      this.RegColArr[index41].reg = "xer";
      this.RegColArr[index41].col = Brushes.Cornsilk;
      this.RegColArr[index41].title = "Fixed-Point Exception Register";
      ulong index42 = index41 + 1UL;
      this.RegColArr[index42].reg = "lr";
      this.RegColArr[index42].col = Brushes.Cornsilk;
      this.RegColArr[index42].title = "Link Register";
      ulong index43 = index42 + 1UL;
      this.RegColArr[index43].reg = "ctr";
      this.RegColArr[index43].col = Brushes.Cornsilk;
      this.RegColArr[index43].title = "Count Register";
      ulong index44 = index43 + 1UL;
      int num1 = 0;
      this.RegColArr[index44].reg = "f" + num1.ToString();
      this.RegColArr[index44].col = Brushes.Tan;
      this.RegColArr[index44].title = "Floating Point Register 0";
      ulong index45 = index44 + 1UL;
      int num2 = num1 + 1;
      this.RegColArr[index45].reg = "f" + num2.ToString();
      this.RegColArr[index45].col = Brushes.Tan;
      this.RegColArr[index45].title = "Float Argument 1 and Return Value";
      ulong index46 = index45 + 1UL;
      int num3 = num2 + 1;
      this.RegColArr[index46].reg = "f" + num3.ToString();
      this.RegColArr[index46].col = Brushes.Tan;
      this.RegColArr[index46].title = "Float Argument 2";
      ulong index47 = index46 + 1UL;
      int num4 = num3 + 1;
      this.RegColArr[index47].reg = "f" + num4.ToString();
      this.RegColArr[index47].col = Brushes.Tan;
      this.RegColArr[index47].title = "Float Argument 3";
      ulong index48 = index47 + 1UL;
      int num5 = num4 + 1;
      this.RegColArr[index48].reg = "f" + num5.ToString();
      this.RegColArr[index48].col = Brushes.Tan;
      this.RegColArr[index48].title = "Float Argument 4";
      ulong index49 = index48 + 1UL;
      int num6 = num5 + 1;
      this.RegColArr[index49].reg = "f" + num6.ToString();
      this.RegColArr[index49].col = Brushes.Tan;
      this.RegColArr[index49].title = "Float Argument 5";
      ulong index50 = index49 + 1UL;
      int num7 = num6 + 1;
      this.RegColArr[index50].reg = "f" + num7.ToString();
      this.RegColArr[index50].col = Brushes.Tan;
      this.RegColArr[index50].title = "Float Argument 6";
      ulong index51 = index50 + 1UL;
      int num8 = num7 + 1;
      this.RegColArr[index51].reg = "f" + num8.ToString();
      this.RegColArr[index51].col = Brushes.Tan;
      this.RegColArr[index51].title = "Float Argument 7";
      ulong index52 = index51 + 1UL;
      int num9 = num8 + 1;
      this.RegColArr[index52].reg = "f" + num9.ToString();
      this.RegColArr[index52].col = Brushes.Tan;
      this.RegColArr[index52].title = "Float Argument 8";
      ulong index53 = index52 + 1UL;
      int num10 = num9 + 1;
      this.RegColArr[index53].reg = "f" + num10.ToString();
      this.RegColArr[index53].col = Brushes.Tan;
      this.RegColArr[index53].title = "Floating Point Register 9";
      ulong index54 = index53 + 1UL;
      int num11 = num10 + 1;
      this.RegColArr[index54].reg = "f" + num11.ToString();
      this.RegColArr[index54].col = Brushes.Tan;
      this.RegColArr[index54].title = "Floating Point Register 10";
      ulong index55 = index54 + 1UL;
      int num12 = num11 + 1;
      this.RegColArr[index55].reg = "f" + num12.ToString();
      this.RegColArr[index55].col = Brushes.Tan;
      this.RegColArr[index55].title = "Floating Point Register 11";
      ulong index56 = index55 + 1UL;
      int num13 = num12 + 1;
      this.RegColArr[index56].reg = "f" + num13.ToString();
      this.RegColArr[index56].col = Brushes.Tan;
      this.RegColArr[index56].title = "Floating Point Register 12";
      ulong index57 = index56 + 1UL;
      int num14 = num13 + 1;
      this.RegColArr[index57].reg = "f" + num14.ToString();
      this.RegColArr[index57].col = Brushes.Tan;
      this.RegColArr[index57].title = "Floating Point Register 13";
      ulong index58 = index57 + 1UL;
      int num15 = num14 + 1;
      this.RegColArr[index58].reg = "f" + num15.ToString();
      this.RegColArr[index58].col = Brushes.Tan;
      this.RegColArr[index58].title = "Floating Point Register 14";
      ulong index59 = index58 + 1UL;
      int num16 = num15 + 1;
      this.RegColArr[index59].reg = "f" + num16.ToString();
      this.RegColArr[index59].col = Brushes.Tan;
      this.RegColArr[index59].title = "Floating Point Register 15";
      ulong index60 = index59 + 1UL;
      int num17 = num16 + 1;
      this.RegColArr[index60].reg = "f" + num17.ToString();
      this.RegColArr[index60].col = Brushes.Tan;
      this.RegColArr[index60].title = "Floating Point Register 16";
      ulong index61 = index60 + 1UL;
      int num18 = num17 + 1;
      this.RegColArr[index61].reg = "f" + num18.ToString();
      this.RegColArr[index61].col = Brushes.Tan;
      this.RegColArr[index61].title = "Floating Point Register 17";
      ulong index62 = index61 + 1UL;
      int num19 = num18 + 1;
      this.RegColArr[index62].reg = "f" + num19.ToString();
      this.RegColArr[index62].col = Brushes.Tan;
      this.RegColArr[index62].title = "Floating Point Register 18";
      ulong index63 = index62 + 1UL;
      int num20 = num19 + 1;
      this.RegColArr[index63].reg = "f" + num20.ToString();
      this.RegColArr[index63].col = Brushes.Tan;
      this.RegColArr[index63].title = "Floating Point Register 19";
      ulong index64 = index63 + 1UL;
      int num21 = num20 + 1;
      this.RegColArr[index64].reg = "f" + num21.ToString();
      this.RegColArr[index64].col = Brushes.Tan;
      this.RegColArr[index64].title = "Floating Point Register 20";
      ulong index65 = index64 + 1UL;
      int num22 = num21 + 1;
      this.RegColArr[index65].reg = "f" + num22.ToString();
      this.RegColArr[index65].col = Brushes.Tan;
      this.RegColArr[index65].title = "Floating Point Register 21";
      ulong index66 = index65 + 1UL;
      int num23 = num22 + 1;
      this.RegColArr[index66].reg = "f" + num23.ToString();
      this.RegColArr[index66].col = Brushes.Tan;
      this.RegColArr[index66].title = "Floating Point Register 22";
      ulong index67 = index66 + 1UL;
      int num24 = num23 + 1;
      this.RegColArr[index67].reg = "f" + num24.ToString();
      this.RegColArr[index67].col = Brushes.Tan;
      this.RegColArr[index67].title = "Floating Point Register 23";
      ulong index68 = index67 + 1UL;
      int num25 = num24 + 1;
      this.RegColArr[index68].reg = "f" + num25.ToString();
      this.RegColArr[index68].col = Brushes.Tan;
      this.RegColArr[index68].title = "Floating Point Register 24";
      ulong index69 = index68 + 1UL;
      int num26 = num25 + 1;
      this.RegColArr[index69].reg = "f" + num26.ToString();
      this.RegColArr[index69].col = Brushes.Tan;
      this.RegColArr[index69].title = "Floating Point Register 25";
      ulong index70 = index69 + 1UL;
      int num27 = num26 + 1;
      this.RegColArr[index70].reg = "f" + num27.ToString();
      this.RegColArr[index70].col = Brushes.Tan;
      this.RegColArr[index70].title = "Floating Point Register 26";
      ulong index71 = index70 + 1UL;
      int num28 = num27 + 1;
      this.RegColArr[index71].reg = "f" + num28.ToString();
      this.RegColArr[index71].col = Brushes.Tan;
      this.RegColArr[index71].title = "Floating Point Register 27";
      ulong index72 = index71 + 1UL;
      int num29 = num28 + 1;
      this.RegColArr[index72].reg = "f" + num29.ToString();
      this.RegColArr[index72].col = Brushes.Tan;
      this.RegColArr[index72].title = "Floating Point Register 28";
      ulong index73 = index72 + 1UL;
      int num30 = num29 + 1;
      this.RegColArr[index73].reg = "f" + num30.ToString();
      this.RegColArr[index73].col = Brushes.Tan;
      this.RegColArr[index73].title = "Floating Point Register 29";
      ulong index74 = index73 + 1UL;
      int num31 = num30 + 1;
      this.RegColArr[index74].reg = "f" + num31.ToString();
      this.RegColArr[index74].col = Brushes.Tan;
      this.RegColArr[index74].title = "Floating Point Register 30";
      ulong index75 = index74 + 1UL;
      int num32 = num31 + 1;
      this.RegColArr[index75].reg = "f" + num32.ToString();
      this.RegColArr[index75].col = Brushes.Tan;
      this.RegColArr[index75].title = "Floating Point Register 31";
      ulong num33 = index75 + 1UL;
      int num34 = num32 + 1;
      ulong index76 = 0UL;
      string newLine = Environment.NewLine;
      string str1 = "Stores (~rA + rB + 1) into rD" + newLine + "subf rD, rA, rB :: rD = rB - rA or rD = ~rA + rB + 1" + newLine + newLine + "Example:" + newLine + "subf r4, r4, r3" + newLine;
      this.ASMDef[index76].name = "subf";
      this.ASMDef[index76].opCode = 2080374864U;
      this.ASMDef[index76].opShift = new int[2]
      {
        6,
        9
      };
      this.ASMDef[index76].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index76].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index76].type = 0;
      this.ASMDef[index76].help = str1;
      this.ASMDef[index76].title = "Subtract From (subf)";
      ulong index77 = index76 + 1UL;
      this.InsPlaceArr[0] = (int) index77;
      string str2 = "Adds the register rA with the 16-bit signed value IMM that has been shifted left 16 bits and stores it into the lower part of register rD" + newLine + "addis rD, rA, 0xIMM :: rD = (rA + IMM) << 16" + newLine + newLine + "Example:" + newLine + "addis r4, r3, 0x1FF0" + newLine;
      this.ASMDef[index77].name = "addis";
      this.ASMDef[index77].opCode = 1006632960U;
      this.ASMDef[index77].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index77].shifts = new int[3];
      this.ASMDef[index77].shifts[0] = 21;
      this.ASMDef[index77].shifts[1] = 16;
      this.ASMDef[index77].shifts[2] = 0;
      this.ASMDef[index77].order = new int[3];
      this.ASMDef[index77].order[0] = 0;
      this.ASMDef[index77].order[1] = 1;
      this.ASMDef[index77].order[2] = 2;
      this.ASMDef[index77].type = 4;
      this.ASMDef[index77].help = str2;
      this.ASMDef[index77].title = "Add Immediate Shifted (addis)";
      ulong index78 = index77 + 1UL;
      string str3 = "Adds the register rA with the 16-bit signed value IMM and stores it into the lower part of register rD. It also does something with the XER register that I don't understand..." + newLine + "addic rD, rA, 0xIMM :: rD = (rA + IMM)" + newLine + newLine + "Example:" + newLine + "addic r4, r3, 0x1FF0" + newLine;
      this.ASMDef[index78].name = "addic";
      this.ASMDef[index78].opCode = 805306368U;
      this.ASMDef[index78].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index78].shifts = new int[3];
      this.ASMDef[index78].shifts[0] = 21;
      this.ASMDef[index78].shifts[1] = 16;
      this.ASMDef[index78].shifts[2] = 0;
      this.ASMDef[index78].order = new int[3];
      this.ASMDef[index78].order[0] = 0;
      this.ASMDef[index78].order[1] = 1;
      this.ASMDef[index78].order[2] = 2;
      this.ASMDef[index78].type = 4;
      this.ASMDef[index78].help = str3;
      this.ASMDef[index78].title = "Add Immediate And Carry (addic)";
      ulong index79 = index78 + 1UL;
      string str4 = "Adds the register rA with the 16-bit signed value IMM and stores it into the upper part of register rD" + newLine + "addi rD, rA, 0xIMM :: rD = rA + IMM" + newLine + newLine + "Example:" + newLine + "addi r4, r3, 0x1FF0" + newLine;
      this.ASMDef[index79].name = "addi";
      this.ASMDef[index79].opCode = 939524096U;
      this.ASMDef[index79].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index79].shifts = new int[3];
      this.ASMDef[index79].shifts = new int[3];
      this.ASMDef[index79].shifts[0] = 21;
      this.ASMDef[index79].shifts[1] = 16;
      this.ASMDef[index79].shifts[2] = 0;
      this.ASMDef[index79].order = new int[3];
      this.ASMDef[index79].order[0] = 0;
      this.ASMDef[index79].order[1] = 1;
      this.ASMDef[index79].order[2] = 2;
      this.ASMDef[index79].type = 4;
      this.ASMDef[index79].help = str4;
      this.ASMDef[index79].title = "Add Immediate (addi)";
      ulong index80 = index79 + 1UL;
      string str5 = "Adds the register rA with the register rB and stores it into the register rD" + newLine + "add rD, rA, rB :: rD = rA + rB" + newLine + newLine + "Example:" + newLine + "add r3, r3, r4" + newLine;
      this.ASMDef[index80].name = "add";
      this.ASMDef[index80].opCode = 2080375316U;
      this.ASMDef[index80].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index80].shifts = new int[3];
      this.ASMDef[index80].shifts[0] = 21;
      this.ASMDef[index80].shifts[1] = 16;
      this.ASMDef[index80].shifts[2] = 11;
      this.ASMDef[index80].order = new int[3];
      this.ASMDef[index80].order[0] = 0;
      this.ASMDef[index80].order[1] = 1;
      this.ASMDef[index80].order[2] = 2;
      this.ASMDef[index80].type = 0;
      this.ASMDef[index80].help = str5;
      this.ASMDef[index80].title = "Add (add)";
      ulong index81 = index80 + 1UL;
      string str6 = "Ands the register rA with the register rB and stores it into the register rD" + newLine + "and rD, rA, rB :: rD = rA & rB" + newLine + newLine + "Example:" + newLine + "and r3, r3, r4" + newLine;
      this.ASMDef[index81].name = "and";
      this.ASMDef[index81].opCode = 2080374840U;
      this.ASMDef[index81].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index81].shifts = new int[3];
      this.ASMDef[index81].shifts[0] = 21;
      this.ASMDef[index81].shifts[1] = 16;
      this.ASMDef[index81].shifts[2] = 11;
      this.ASMDef[index81].order = new int[3];
      this.ASMDef[index81].order[0] = 1;
      this.ASMDef[index81].order[1] = 0;
      this.ASMDef[index81].order[2] = 2;
      this.ASMDef[index81].type = 0;
      this.ASMDef[index81].help = str6;
      this.ASMDef[index81].title = "Logical And (and)";
      ulong index82 = index81 + 1UL;
      string str7 = "Ands the register rA with IMM and stores it into the register rD" + newLine + "andi rD, rA, IMM :: rD = rA & IMM" + newLine + newLine + "Example:" + newLine + "andi r3, r4, 0x1234" + newLine;
      this.ASMDef[index82].name = "andi";
      this.ASMDef[index82].opCode = 1879048192U;
      this.ASMDef[index82].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index82].shifts = new int[3];
      this.ASMDef[index82].shifts[0] = 21;
      this.ASMDef[index82].shifts[1] = 16;
      this.ASMDef[index82].order = new int[3];
      this.ASMDef[index82].order[0] = 1;
      this.ASMDef[index82].order[1] = 0;
      this.ASMDef[index82].order[2] = 2;
      this.ASMDef[index82].type = 4;
      this.ASMDef[index82].help = str7;
      this.ASMDef[index82].title = "Logical And Immediate (andi)";
      ulong index83 = index82 + 1UL;
      string str8 = "Ands the register rA with IMM that has been shifted left 16 bits and stores it into the register rD" + newLine + "andis rD, rA, IMM :: rD = (rA & IMM) << 16" + newLine + newLine + "Example:" + newLine + "andis r3, r4, 0x1234" + newLine;
      this.ASMDef[index83].name = "andis";
      this.ASMDef[index83].opCode = 1946157056U;
      this.ASMDef[index83].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index83].shifts = new int[3];
      this.ASMDef[index83].shifts[0] = 21;
      this.ASMDef[index83].shifts[1] = 16;
      this.ASMDef[index83].order = new int[3];
      this.ASMDef[index83].order[0] = 1;
      this.ASMDef[index83].order[1] = 0;
      this.ASMDef[index83].order[2] = 2;
      this.ASMDef[index83].type = 4;
      this.ASMDef[index83].help = str8;
      this.ASMDef[index83].title = "Logical And Immediate Shifted (andis)";
      ulong index84 = index83 + 1UL;
      this.InsPlaceArr[1] = (int) index84;
      string str9 = "Branches to the address held in the special register LR" + newLine + "blr :: PC = LR" + newLine + newLine + "Example:" + newLine + "blr" + newLine;
      this.ASMDef[index84].name = "blr";
      this.ASMDef[index84].opCode = 1317011488U;
      this.ASMDef[index84].opShift = new int[2]
      {
        9,
        6
      };
      this.ASMDef[index84].type = 2;
      this.ASMDef[index84].help = str9;
      this.ASMDef[index84].title = "Branch To Link Register (blr)";
      ulong index85 = index84 + 1UL;
      string str10 = "Branches to the unsigned value IMM and sets special register LR to the address after it" + newLine + "bl IMM :: LR = CURRENT_ADDR + 4; PC = IMM" + newLine + newLine + "Example:" + newLine + "bl 0x0024A7FC" + newLine;
      this.ASMDef[index85].name = "bl";
      this.ASMDef[index85].opCode = 1207959553U;
      this.ASMDef[index85].opShift = new int[2]
      {
        6,
        1
      };
      this.ASMDef[index85].shifts = new int[1];
      this.ASMDef[index85].shifts[0] = 2;
      this.ASMDef[index85].order = new int[1];
      this.ASMDef[index85].order[0] = 0;
      this.ASMDef[index85].type = 1;
      this.ASMDef[index85].help = str10;
      this.ASMDef[index85].title = "Branch And Link (bl)";
      ulong index86 = index85 + 1UL;
      string str11 = "Branches to the value (IMM + CURRENT_ADDR) if CR is set equal" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "beq CR, IMM :: if (CR == 1) then PC = CURRENT_ADDR + IMM" + newLine + newLine + "Example:" + newLine + "beq cr2, 0x5C" + newLine;
      this.ASMDef[index86].name = "beq";
      this.ASMDef[index86].opCode = 1099038720U;
      this.ASMDef[index86].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index86].shifts = new int[1];
      this.ASMDef[index86].shifts[0] = 2;
      this.ASMDef[index86].order = new int[1];
      this.ASMDef[index86].order[0] = 0;
      this.ASMDef[index86].type = 6;
      this.ASMDef[index86].help = str11;
      this.ASMDef[index86].title = "Branch If Equal (beq)";
      ulong index87 = index86 + 1UL;
      this.ASMDef[index87].name = "beq";
      this.ASMDef[index87].opCode = 1099038720U;
      this.ASMDef[index87].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index87].shifts = new int[2];
      this.ASMDef[index87].shifts[0] = 18;
      this.ASMDef[index87].shifts[1] = 2;
      this.ASMDef[index87].order = new int[2];
      this.ASMDef[index87].order[0] = 0;
      this.ASMDef[index87].order[1] = 1;
      this.ASMDef[index87].type = 6;
      this.ASMDef[index87].help = str11;
      this.ASMDef[index87].title = "Branch If Equal (beq)";
      ulong index88 = index87 + 1UL;
      string str12 = "Branches to the value (IMM + CURRENT_ADDR) if CR is not set equal" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "bne CR, IMM :: if (CR != 1) then PC = 0xCURRENT_ADDR + IMM (DEC)" + newLine + newLine + "Example:" + newLine + "bne cr2, 0x54" + newLine;
      this.ASMDef[index88].name = "bne";
      this.ASMDef[index88].opCode = 1082261504U;
      this.ASMDef[index88].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index88].shifts = new int[1];
      this.ASMDef[index88].shifts[0] = 2;
      this.ASMDef[index88].order = new int[1];
      this.ASMDef[index88].order[0] = 0;
      this.ASMDef[index88].type = 6;
      this.ASMDef[index88].help = str12;
      this.ASMDef[index88].title = "Branch If Not Equal (bne)";
      ulong index89 = index88 + 1UL;
      this.ASMDef[index89].name = "bne";
      this.ASMDef[index89].opCode = 1082261504U;
      this.ASMDef[index89].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index89].shifts = new int[2];
      this.ASMDef[index89].shifts[0] = 18;
      this.ASMDef[index89].shifts[1] = 2;
      this.ASMDef[index89].order = new int[2];
      this.ASMDef[index89].order[0] = 0;
      this.ASMDef[index89].order[1] = 1;
      this.ASMDef[index89].type = 6;
      this.ASMDef[index89].help = str12;
      this.ASMDef[index89].title = "Branch If Not Equal (bne)";
      ulong index90 = index89 + 1UL;
      string str13 = "Branches to the value (IMM + CURRENT_ADDR) if CR is set less than or equal" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "ble CR, IMM :: if (CR == 1 || CR == 4) then PC = 0xCURRENT_ADDR + 0xIMM" + newLine + newLine + "Example:" + newLine + "ble cr2, 0x5C" + newLine;
      this.ASMDef[index90].name = "ble";
      this.ASMDef[index90].opCode = 1082195968U;
      this.ASMDef[index90].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index90].shifts = new int[1];
      this.ASMDef[index90].shifts[0] = 2;
      this.ASMDef[index90].order = new int[1];
      this.ASMDef[index90].order[0] = 0;
      this.ASMDef[index90].type = 6;
      this.ASMDef[index90].help = str13;
      this.ASMDef[index90].title = "Branch If Less Than Or Equal (ble)";
      ulong index91 = index90 + 1UL;
      this.ASMDef[index91].name = "ble";
      this.ASMDef[index91].opCode = 1082195968U;
      this.ASMDef[index91].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index91].shifts = new int[2];
      this.ASMDef[index91].shifts[0] = 18;
      this.ASMDef[index91].shifts[1] = 2;
      this.ASMDef[index91].order = new int[2];
      this.ASMDef[index91].order[0] = 0;
      this.ASMDef[index91].order[1] = 1;
      this.ASMDef[index91].type = 6;
      this.ASMDef[index91].help = str13;
      this.ASMDef[index91].title = "Branch If Less Than Or Equal (ble)";
      ulong index92 = index91 + 1UL;
      string str14 = "Branches to the value (IMM + CURRENT_ADDR) if CR is set greater than or equal" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "bge CR, IMM :: if (CR == 1 || CR == 2) then PC = 0xCURRENT_ADDR + 0xIMM" + newLine + newLine + "Example:" + newLine + "bge cr2, 0x5C" + newLine;
      this.ASMDef[index92].name = "bge";
      this.ASMDef[index92].opCode = 1082130432U;
      this.ASMDef[index92].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index92].shifts = new int[1];
      this.ASMDef[index92].shifts[0] = 2;
      this.ASMDef[index92].order = new int[1];
      this.ASMDef[index92].order[0] = 0;
      this.ASMDef[index92].type = 6;
      this.ASMDef[index92].help = str14;
      this.ASMDef[index92].title = "Branch If Greater Than Or Equal (bge)";
      ulong index93 = index92 + 1UL;
      this.ASMDef[index93].name = "bge";
      this.ASMDef[index93].opCode = 1082130432U;
      this.ASMDef[index93].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index93].shifts = new int[2];
      this.ASMDef[index93].shifts[0] = 18;
      this.ASMDef[index93].shifts[1] = 2;
      this.ASMDef[index93].order = new int[2];
      this.ASMDef[index93].order[0] = 0;
      this.ASMDef[index93].order[1] = 1;
      this.ASMDef[index93].type = 6;
      this.ASMDef[index93].help = str14;
      this.ASMDef[index93].title = "Branch If Greater Than Or Equal (bge)";
      ulong index94 = index93 + 1UL;
      string str15 = "Branches to the value (IMM + CURRENT_ADDR) if CR is set greater than" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "bgt CR, IMM :: if (CR == 2) then PC = 0xCURRENT_ADDR + 0xIMM" + newLine + newLine + "Example:" + newLine + "bgt cr2, 0x5C" + newLine;
      this.ASMDef[index94].name = "bgt";
      this.ASMDef[index94].opCode = 1098973184U;
      this.ASMDef[index94].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index94].shifts = new int[1];
      this.ASMDef[index94].shifts[0] = 2;
      this.ASMDef[index94].order = new int[1];
      this.ASMDef[index94].order[0] = 0;
      this.ASMDef[index94].type = 6;
      this.ASMDef[index94].help = str15;
      this.ASMDef[index94].title = "Branch If Greater Than (bgt)";
      ulong index95 = index94 + 1UL;
      this.ASMDef[index95].name = "bgt";
      this.ASMDef[index95].opCode = 1098973184U;
      this.ASMDef[index95].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index95].shifts = new int[2];
      this.ASMDef[index95].shifts[0] = 18;
      this.ASMDef[index95].shifts[1] = 2;
      this.ASMDef[index95].order = new int[2];
      this.ASMDef[index95].order[0] = 0;
      this.ASMDef[index95].order[1] = 1;
      this.ASMDef[index95].type = 6;
      this.ASMDef[index95].help = str15;
      this.ASMDef[index95].title = "Branch If Greater Than (bgt)";
      ulong index96 = index95 + 1UL;
      string str16 = "Branches to the value (IMM + CURRENT_ADDR) if CR is set less than" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "blt CR, IMM :: if (CR == 4) then PC = 0xCURRENT_ADDR + 0xIMM" + newLine + newLine + "Example:" + newLine + "blt cr2, 0x5C" + newLine;
      this.ASMDef[index96].name = "blt";
      this.ASMDef[index96].opCode = 1098907648U;
      this.ASMDef[index96].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index96].shifts = new int[1];
      this.ASMDef[index96].shifts[0] = 2;
      this.ASMDef[index96].order = new int[1];
      this.ASMDef[index96].order[0] = 0;
      this.ASMDef[index96].type = 6;
      this.ASMDef[index96].help = str16;
      this.ASMDef[index96].title = "Branch If Less Than (blt)";
      ulong index97 = index96 + 1UL;
      this.ASMDef[index97].name = "blt";
      this.ASMDef[index97].opCode = 1098907648U;
      this.ASMDef[index97].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index97].shifts = new int[2];
      this.ASMDef[index97].shifts[0] = 18;
      this.ASMDef[index97].shifts[1] = 2;
      this.ASMDef[index97].order = new int[2];
      this.ASMDef[index97].order[0] = 0;
      this.ASMDef[index97].order[1] = 1;
      this.ASMDef[index97].type = 6;
      this.ASMDef[index97].help = str16;
      this.ASMDef[index97].title = "Branch If Less Than (blt)";
      ulong index98 = index97 + 1UL;
      string str17 = "Branches to the unsigned value IMM" + newLine + "b IMM :: PC = IMM" + newLine + newLine + "Example:" + newLine + "b 0x0024A7FC" + newLine;
      this.ASMDef[index98].name = "b";
      this.ASMDef[index98].opCode = 1207959552U;
      this.ASMDef[index98].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index98].shifts = new int[1];
      this.ASMDef[index98].shifts[0] = 2;
      this.ASMDef[index98].order = new int[1];
      this.ASMDef[index98].order[0] = 0;
      this.ASMDef[index98].type = 1;
      this.ASMDef[index98].help = str17;
      this.ASMDef[index98].title = "Branch (b)";
      ulong index99 = index98 + 1UL;
      this.InsPlaceArr[2] = (int) index99;
      string str18 = "Compares (signed 32 bits) rA with the signed value IMM and stores result in BF" + newLine + "If there is no BF specified, then it will be defaulted to cr0" + newLine + "cmpwi rA, 0xIMM; cmpwi cr1, rA, 0xIMM" + newLine + "    if (rA == IMM) then BF = 1; if (rA > IMM) then BF = 2; if (rA < IMM) then BF = 4" + newLine + newLine + "Example:" + newLine + "cmpwi cr2, r4, 0xA7FC" + newLine;
      this.ASMDef[index99].name = "cmpwi";
      this.ASMDef[index99].opCode = 738197504U;
      this.ASMDef[index99].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index99].shifts = new int[2];
      this.ASMDef[index99].shifts[0] = 16;
      this.ASMDef[index99].order = new int[2];
      this.ASMDef[index99].order[0] = 0;
      this.ASMDef[index99].order[1] = 1;
      this.ASMDef[index99].type = 7;
      this.ASMDef[index99].help = str18;
      this.ASMDef[index99].title = "Compare Word Immediate (cmpwi)";
      ulong index100 = index99 + 1UL;
      this.ASMDef[index100].name = "cmpwi";
      this.ASMDef[index100].opCode = 738197504U;
      this.ASMDef[index100].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index100].shifts = new int[3];
      this.ASMDef[index100].shifts[0] = 23;
      this.ASMDef[index100].shifts[1] = 16;
      this.ASMDef[index100].order = new int[3];
      this.ASMDef[index100].order[0] = 0;
      this.ASMDef[index100].order[1] = 1;
      this.ASMDef[index100].order[2] = 2;
      this.ASMDef[index100].type = 7;
      this.ASMDef[index100].help = str18;
      this.ASMDef[index100].title = "Compare Word Immediate (cmpwi)";
      ulong index101 = index100 + 1UL;
      string str19 = "Compares (unsigned 32 bits) rA with the unsigned value IMM and stores result in BF" + newLine + "If there is no BF specified, then it will be defaulted to cr0" + newLine + "cmplwi rA, 0xIMM; cmplwi cr1, rA, 0xIMM" + newLine + "    if (rA == IMM) then BF = 1; if (rA > IMM) then BF = 2; if (rA < IMM) then BF = 4" + newLine + newLine + "Example:" + newLine + "cmplwi cr2, r4, 0xA7FC" + newLine;
      this.ASMDef[index101].name = "cmplwi";
      this.ASMDef[index101].opCode = 671088640U;
      this.ASMDef[index101].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index101].shifts = new int[2];
      this.ASMDef[index101].shifts[0] = 16;
      this.ASMDef[index101].order = new int[2];
      this.ASMDef[index101].order[0] = 0;
      this.ASMDef[index101].order[1] = 1;
      this.ASMDef[index101].type = 7;
      this.ASMDef[index101].help = str19;
      this.ASMDef[index101].title = "Compare Logical Word Immediate (cmplwi)";
      ulong index102 = index101 + 1UL;
      this.ASMDef[index102].name = "cmplwi";
      this.ASMDef[index102].opCode = 671088640U;
      this.ASMDef[index102].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index102].shifts = new int[3];
      this.ASMDef[index102].shifts[0] = 23;
      this.ASMDef[index102].shifts[1] = 16;
      this.ASMDef[index102].order = new int[3];
      this.ASMDef[index102].order[0] = 0;
      this.ASMDef[index102].order[1] = 1;
      this.ASMDef[index102].order[2] = 2;
      this.ASMDef[index102].type = 7;
      this.ASMDef[index102].help = str19;
      this.ASMDef[index102].title = "Compare Logical Word Immediate (cmplwi)";
      ulong index103 = index102 + 1UL;
      string str20 = "Compares (signed 64 bits) rA with rB and stores result in BF" + newLine + "If there is no BF specified, then it will be defaulted to cr0" + newLine + "cmpd rA, rB; cmpw cr1, rA, rB" + newLine + "    if (rA == rB) then BF = 1; if (rA > rB) then BF = 2; if (rA < rB) then BF = 4" + newLine + newLine + "Example:" + newLine + "cmpd cr2, r4, r3" + newLine;
      this.ASMDef[index103].name = "cmpd";
      this.ASMDef[index103].opCode = 2082471936U;
      this.ASMDef[index103].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index103].shifts = new int[2];
      this.ASMDef[index103].shifts[0] = 16;
      this.ASMDef[index103].shifts[1] = 11;
      this.ASMDef[index103].order = new int[2];
      this.ASMDef[index103].order[0] = 0;
      this.ASMDef[index103].order[1] = 1;
      this.ASMDef[index103].type = 7;
      this.ASMDef[index103].help = str20;
      this.ASMDef[index103].title = "Compare Doubleword (cmpd)";
      ulong index104 = index103 + 1UL;
      this.ASMDef[index104].name = "cmpd";
      this.ASMDef[index104].opCode = 2082471936U;
      this.ASMDef[index104].opShift = new int[2]
      {
        11,
        0
      };
      this.ASMDef[index104].shifts = new int[3];
      this.ASMDef[index104].shifts[0] = 23;
      this.ASMDef[index104].shifts[1] = 16;
      this.ASMDef[index104].shifts[2] = 11;
      this.ASMDef[index104].order = new int[3];
      this.ASMDef[index104].order[0] = 0;
      this.ASMDef[index104].order[1] = 1;
      this.ASMDef[index104].order[2] = 2;
      this.ASMDef[index104].type = 7;
      this.ASMDef[index104].help = str20;
      this.ASMDef[index104].title = "Compare Doubleword (cmpd)";
      ulong index105 = index104 + 1UL;
      string str21 = "Compares (signed 32 bits) rA with rB and stores result in BF" + newLine + "If there is no BF specified, then it will be defaulted to cr0" + newLine + "cmpw rA, rB; cmpw cr1, rA, rB" + newLine + "    if (rA == rB) then BF = 1; if (rA > rB) then BF = 2; if (rA < rB) then BF = 4" + newLine + newLine + "Example:" + newLine + "cmpw cr2, r4, r3" + newLine;
      this.ASMDef[index105].name = "cmpw";
      this.ASMDef[index105].opCode = 2080374784U;
      this.ASMDef[index105].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index105].shifts = new int[2];
      this.ASMDef[index105].shifts[0] = 16;
      this.ASMDef[index105].shifts[1] = 11;
      this.ASMDef[index105].order = new int[2];
      this.ASMDef[index105].order[0] = 0;
      this.ASMDef[index105].order[1] = 1;
      this.ASMDef[index105].type = 7;
      this.ASMDef[index105].help = str21;
      this.ASMDef[index105].title = "Compare Word (cmpw)";
      ulong index106 = index105 + 1UL;
      this.ASMDef[index106].name = "cmpw";
      this.ASMDef[index106].opCode = 2080374784U;
      this.ASMDef[index106].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index106].shifts = new int[3];
      this.ASMDef[index106].shifts[0] = 23;
      this.ASMDef[index106].shifts[1] = 16;
      this.ASMDef[index106].shifts[2] = 11;
      this.ASMDef[index106].order = new int[3];
      this.ASMDef[index106].order[0] = 0;
      this.ASMDef[index106].order[1] = 1;
      this.ASMDef[index106].order[2] = 2;
      this.ASMDef[index106].type = 7;
      this.ASMDef[index106].help = str21;
      this.ASMDef[index106].title = "Compare Word (cmpw)";
      ulong index107 = index106 + 1UL;
      string str22 = "Compares (unsigned 32 bits) the rA with rB and stores result in BF" + newLine + "If there is no BF specified, then it will be defaulted to cr0" + newLine + "cmplw rA, rB; cmplw cr1, rA, rB" + newLine + "if (rA == rB) then BF = 1; if (rA > rB) then BF = 2; if (rA < rB) then BF = 4" + newLine + newLine + "Example:" + newLine + "cmplw cr2, r4, r3" + newLine;
      this.ASMDef[index107].name = "cmplw";
      this.ASMDef[index107].opCode = 2080374848U;
      this.ASMDef[index107].opShift = new int[2]
      {
        6,
        7
      };
      this.ASMDef[index107].shifts = new int[2];
      this.ASMDef[index107].shifts[0] = 16;
      this.ASMDef[index107].shifts[1] = 11;
      this.ASMDef[index107].order = new int[2];
      this.ASMDef[index107].order[0] = 0;
      this.ASMDef[index107].order[1] = 1;
      this.ASMDef[index107].type = 7;
      this.ASMDef[index107].help = str22;
      this.ASMDef[index107].title = "Compare Logical Word (cmplw)";
      ulong index108 = index107 + 1UL;
      this.ASMDef[index108].name = "cmplw";
      this.ASMDef[index108].opCode = 2080374848U;
      this.ASMDef[index108].opShift = new int[2]
      {
        6,
        7
      };
      this.ASMDef[index108].shifts = new int[3];
      this.ASMDef[index108].shifts[0] = 23;
      this.ASMDef[index108].shifts[1] = 16;
      this.ASMDef[index108].shifts[2] = 11;
      this.ASMDef[index108].order = new int[3];
      this.ASMDef[index108].order[0] = 0;
      this.ASMDef[index108].order[1] = 1;
      this.ASMDef[index108].order[2] = 2;
      this.ASMDef[index108].type = 7;
      this.ASMDef[index108].help = str22;
      this.ASMDef[index108].title = "Compare Logical Word (cmplw)";
      ulong index109 = index108 + 1UL;
      this.InsPlaceArr[3] = (int) index109;
      string str23 = "The 64 bit contents of rA are divided by the 64 bit contents of rB and the resulting word is stored into rD" + newLine + "divw rD, rA, rB :: rD = (int)((long)rA / (long)rB)" + newLine + newLine + "Example:" + newLine + "divw r3, r4, r3" + newLine;
      this.ASMDef[index109].name = "divw";
      this.ASMDef[index109].opCode = 2080375766U;
      this.ASMDef[index109].opShift = new int[2]
      {
        6,
        9
      };
      this.ASMDef[index109].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index109].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index109].type = 0;
      this.ASMDef[index109].help = str23;
      this.ASMDef[index109].title = "Divide Word (divw)";
      ulong index110 = index109 + 1UL;
      this.InsPlaceArr[5] = (int) index110;
      string str24 = "Adds frA with frB and stores the result into frD" + newLine + "fadd frD, frA, frB :: frD = (frA + frB)" + newLine + newLine + "Example:" + newLine + "fadd f1, f2, f3" + newLine;
      this.ASMDef[index110].name = "fadd";
      this.ASMDef[index110].opCode = 4227858474U;
      this.ASMDef[index110].opShift = new int[2]
      {
        6,
        6
      };
      this.ASMDef[index110].shifts = new int[3];
      this.ASMDef[index110].order = new int[3];
      this.ASMDef[index110].order[0] = 0;
      this.ASMDef[index110].order[1] = 1;
      this.ASMDef[index110].order[2] = 2;
      this.ASMDef[index110].shifts[0] = 21;
      this.ASMDef[index110].shifts[1] = 16;
      this.ASMDef[index110].shifts[2] = 11;
      this.ASMDef[index110].type = 9;
      this.ASMDef[index110].help = str24;
      this.ASMDef[index110].title = "Floating Point Double Add (fadd)";
      ulong index111 = index110 + 1UL;
      string str25 = "Adds frA with frB and stores the result into frD" + newLine + "fadds frD, frA, frB :: frD = (frA + frB)" + newLine + newLine + "Example:" + newLine + "fadds f1, f2, f3" + newLine;
      this.ASMDef[index111].name = "fadds";
      this.ASMDef[index111].opCode = 3959423018U;
      this.ASMDef[index111].opShift = new int[2]
      {
        6,
        6
      };
      this.ASMDef[index111].shifts = new int[3];
      this.ASMDef[index111].order = new int[3];
      this.ASMDef[index111].order[0] = 0;
      this.ASMDef[index111].order[1] = 1;
      this.ASMDef[index111].order[2] = 2;
      this.ASMDef[index111].shifts[0] = 21;
      this.ASMDef[index111].shifts[1] = 16;
      this.ASMDef[index111].shifts[2] = 11;
      this.ASMDef[index111].type = 9;
      this.ASMDef[index111].help = str25;
      this.ASMDef[index111].title = "Floating Point Single Add (fadds)";
      ulong index112 = index111 + 1UL;
      string str26 = "The integer held in frA is converted to a double and stored in frD" + newLine + "fcfid frD, frA :: frD = (Double)frA" + newLine + newLine + "Example:" + newLine + "fcfid f1, f2" + newLine;
      this.ASMDef[index112].name = "fcfid";
      this.ASMDef[index112].opCode = 4227860124U;
      this.ASMDef[index112].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index112].shifts = new int[2];
      this.ASMDef[index112].order = new int[2];
      this.ASMDef[index112].order[0] = 0;
      this.ASMDef[index112].order[1] = 1;
      this.ASMDef[index112].shifts[0] = 21;
      this.ASMDef[index112].shifts[1] = 11;
      this.ASMDef[index112].type = 9;
      this.ASMDef[index112].help = str26;
      this.ASMDef[index112].title = "Floating Point Convert to Double From Integer (fcfid)";
      ulong index113 = index112 + 1UL;
      string str27 = "Divides frA by frB and stores the result into frD" + newLine + "fdivs frD, frA, frB :: frD = (frA / frB)" + newLine + newLine + "Example:" + newLine + "fdivs f1, f2, f3" + newLine;
      this.ASMDef[index113].name = "fdivs";
      this.ASMDef[index113].opCode = 3959423012U;
      this.ASMDef[index113].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index113].shifts = new int[3];
      this.ASMDef[index113].order = new int[3];
      this.ASMDef[index113].order[0] = 0;
      this.ASMDef[index113].order[1] = 1;
      this.ASMDef[index113].order[2] = 2;
      this.ASMDef[index113].shifts[0] = 21;
      this.ASMDef[index113].shifts[1] = 16;
      this.ASMDef[index113].shifts[2] = 11;
      this.ASMDef[index113].type = 9;
      this.ASMDef[index113].help = str27;
      this.ASMDef[index113].title = "Floating Point Single Divide (fdivs)";
      ulong index114 = index113 + 1UL;
      string str28 = "frA is multiplied with frC, frB is added to the result and stored in frD" + newLine + "fmadd frD, frA, frC, frB :: frD = (frA * frC) + frB" + newLine + newLine + "Example:" + newLine + "fmadd f1, f2, f5, f3" + newLine;
      this.ASMDef[index114].name = "fmadd";
      this.ASMDef[index114].opCode = 4227858490U;
      this.ASMDef[index114].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index114].shifts = new int[4]
      {
        21,
        16,
        11,
        6
      };
      this.ASMDef[index114].order = new int[4]
      {
        0,
        1,
        3,
        2
      };
      this.ASMDef[index114].type = 9;
      this.ASMDef[index114].help = str28;
      this.ASMDef[index114].title = "Floating Point Double Multiply Add (fmadd)";
      ulong index115 = index114 + 1UL;
      string str29 = "frA is multiplied with frC, frB is added to the result and stored in frD" + newLine + "fmadds frD, frA, frC, frB :: frD = (frA * frC) + frB" + newLine + newLine + "Example:" + newLine + "fmadds f1, f2, f5, f3" + newLine;
      this.ASMDef[index115].name = "fmadds";
      this.ASMDef[index115].opCode = 3959423034U;
      this.ASMDef[index115].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index115].shifts = new int[4]
      {
        21,
        16,
        11,
        6
      };
      this.ASMDef[index115].order = new int[4]
      {
        0,
        1,
        3,
        2
      };
      this.ASMDef[index115].type = 9;
      this.ASMDef[index115].help = str29;
      this.ASMDef[index115].title = "Floating Point Single Multiply Add (fmadds)";
      ulong index116 = index115 + 1UL;
      string str30 = "Multiplies frA with frB and stores the result into frD" + newLine + "fmul frD, frA, frB :: frD = (frA * frB)" + newLine + newLine + "Example:" + newLine + "fmul f1, f2, f3" + newLine;
      this.ASMDef[index116].name = "fmul";
      this.ASMDef[index116].opCode = 4227858482U;
      this.ASMDef[index116].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index116].shifts = new int[3];
      this.ASMDef[index116].order = new int[3];
      this.ASMDef[index116].order[0] = 0;
      this.ASMDef[index116].order[1] = 1;
      this.ASMDef[index116].order[2] = 2;
      this.ASMDef[index116].shifts[0] = 21;
      this.ASMDef[index116].shifts[1] = 16;
      this.ASMDef[index116].shifts[2] = 6;
      this.ASMDef[index116].type = 9;
      this.ASMDef[index116].help = str30;
      this.ASMDef[index116].title = "Floating Point Double Multiply (fmul)";
      ulong index117 = index116 + 1UL;
      string str31 = "Moves the contents of frA to frD" + newLine + "fmr frD, frA :: frD = frA" + newLine + newLine + "Example:" + newLine + "fmr f1, f2" + newLine;
      this.ASMDef[index117].name = "fmr";
      this.ASMDef[index117].opCode = 4227858576U;
      this.ASMDef[index117].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index117].shifts = new int[2];
      this.ASMDef[index117].order = new int[2];
      this.ASMDef[index117].order[0] = 0;
      this.ASMDef[index117].order[1] = 1;
      this.ASMDef[index117].shifts[0] = 21;
      this.ASMDef[index117].shifts[1] = 11;
      this.ASMDef[index117].type = 9;
      this.ASMDef[index117].help = str31;
      this.ASMDef[index117].title = "Move Floating Point Register (fmr)";
      ulong index118 = index117 + 1UL;
      string str32 = "Multiplies frA with frB and stores the result into frD" + newLine + "fmuls frD, frA, frB :: frD = (frA * frB)" + newLine + newLine + "Example:" + newLine + "fmuls f1, f2, f3" + newLine;
      this.ASMDef[index118].name = "fmuls";
      this.ASMDef[index118].opCode = 3959423026U;
      this.ASMDef[index118].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index118].shifts = new int[3];
      this.ASMDef[index118].order = new int[3];
      this.ASMDef[index118].order[0] = 0;
      this.ASMDef[index118].order[1] = 1;
      this.ASMDef[index118].order[2] = 2;
      this.ASMDef[index118].shifts[0] = 21;
      this.ASMDef[index118].shifts[1] = 16;
      this.ASMDef[index118].shifts[2] = 6;
      this.ASMDef[index118].type = 9;
      this.ASMDef[index118].help = str32;
      this.ASMDef[index118].title = "Floating Point Single Multiply (fmuls)";
      ulong index119 = index118 + 1UL;
      string str33 = "Rounds frA to Single and store the result in frD" + newLine + "frsp frD, frA :: frD = (Single)frA" + newLine + newLine + "Example:" + newLine + "frsp f1, f2" + newLine;
      this.ASMDef[index119].name = "frsp";
      this.ASMDef[index119].opCode = 4227858456U;
      this.ASMDef[index119].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index119].shifts = new int[2];
      this.ASMDef[index119].order = new int[2];
      this.ASMDef[index119].order[0] = 0;
      this.ASMDef[index119].order[1] = 1;
      this.ASMDef[index119].shifts[0] = 21;
      this.ASMDef[index119].shifts[1] = 11;
      this.ASMDef[index119].type = 9;
      this.ASMDef[index119].help = str33;
      this.ASMDef[index119].title = "Floating Point Round to Single (frsp)";
      ulong index120 = index119 + 1UL;
      string str34 = "Takes the square root of frA and stores the result in frD" + newLine + "fsqrt frD, frA :: frD = sqrt(frA)" + newLine + newLine + "Example:" + newLine + "fsqrt f1, f2" + newLine;
      this.ASMDef[index120].name = "fsqrt";
      this.ASMDef[index120].opCode = 4227858476U;
      this.ASMDef[index120].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index120].shifts = new int[2];
      this.ASMDef[index120].order = new int[2];
      this.ASMDef[index120].order[0] = 0;
      this.ASMDef[index120].order[1] = 1;
      this.ASMDef[index120].shifts[0] = 21;
      this.ASMDef[index120].shifts[1] = 11;
      this.ASMDef[index120].type = 9;
      this.ASMDef[index120].help = str34;
      this.ASMDef[index120].title = "Floating Point Square Root (fsqrt)";
      ulong index121 = index120 + 1UL;
      string str35 = "Subtracts frB from frA and stores the result into frD" + newLine + "fsub frD, frA, frB :: frD = (frA - frB)" + newLine + newLine + "Example:" + newLine + "fsub f1, f2, f3" + newLine;
      this.ASMDef[index121].name = "fsub";
      this.ASMDef[index121].opCode = 4227858472U;
      this.ASMDef[index121].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index121].shifts = new int[3];
      this.ASMDef[index121].order = new int[3];
      this.ASMDef[index121].order[0] = 0;
      this.ASMDef[index121].order[1] = 1;
      this.ASMDef[index121].order[2] = 2;
      this.ASMDef[index121].shifts[0] = 21;
      this.ASMDef[index121].shifts[1] = 16;
      this.ASMDef[index121].shifts[2] = 11;
      this.ASMDef[index121].type = 9;
      this.ASMDef[index121].help = str35;
      this.ASMDef[index121].title = "Floating Point Double Subtract (fsub)";
      ulong index122 = index121 + 1UL;
      string str36 = "Subtracts frB from frA and stores the result into frD" + newLine + "fsubs frD, frA, frB :: frD = (frA - frB)" + newLine + newLine + "Example:" + newLine + "fsubs f1, f2, f3" + newLine;
      this.ASMDef[index122].name = "fsubs";
      this.ASMDef[index122].opCode = 3959423016U;
      this.ASMDef[index122].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index122].shifts = new int[3];
      this.ASMDef[index122].order = new int[3];
      this.ASMDef[index122].order[0] = 0;
      this.ASMDef[index122].order[1] = 1;
      this.ASMDef[index122].order[2] = 2;
      this.ASMDef[index122].shifts[0] = 21;
      this.ASMDef[index122].shifts[1] = 16;
      this.ASMDef[index122].shifts[2] = 11;
      this.ASMDef[index122].type = 9;
      this.ASMDef[index122].help = str36;
      this.ASMDef[index122].title = "Floating Point Single Subtract (fsubs)";
      ulong index123 = index122 + 1UL;
      string str37 = "Divides frA by frB and stores the result into frD" + newLine + "fdiv frD, frA, frB :: frD = (frA / frB)" + newLine + newLine + "Example:" + newLine + "fdiv f1, f2, f3" + newLine;
      this.ASMDef[index123].name = "fdiv";
      this.ASMDef[index123].opCode = 4227858468U;
      this.ASMDef[index123].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index123].shifts = new int[3];
      this.ASMDef[index123].order = new int[3];
      this.ASMDef[index123].order[0] = 0;
      this.ASMDef[index123].order[1] = 1;
      this.ASMDef[index123].order[2] = 2;
      this.ASMDef[index123].shifts[0] = 21;
      this.ASMDef[index123].shifts[1] = 16;
      this.ASMDef[index123].shifts[2] = 11;
      this.ASMDef[index123].type = 9;
      this.ASMDef[index123].help = str37;
      this.ASMDef[index123].title = "Floating Point Double Divide (fdiv)";
      ulong index124 = index123 + 1UL;
      string str38 = "Compares frA with frB (unordered) and places the result in BF and FPCC" + newLine + "If there is no CR specified, then it will be defaulted to cr0" + newLine + "fcmpu frA, frB; cmpwi cr1, frA, frB" + newLine + "    if (frA is NaN || frB is Nan) then BF = 1; if (frA == frB) then BF = 2; if (rA > IMM) then BF = 4; if (rA < IMM) then BF = 8" + newLine + newLine + "Example:" + newLine + "fcmpu cr2, f1, f2" + newLine;
      this.ASMDef[index124].name = "fcmpu";
      this.ASMDef[index124].opCode = 4227858432U;
      this.ASMDef[index124].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index124].shifts = new int[2]
      {
        16,
        11
      };
      this.ASMDef[index124].order = new int[2]
      {
        0,
        1
      };
      this.ASMDef[index124].type = 11;
      this.ASMDef[index124].help = str38;
      this.ASMDef[index124].title = "Floating Point Compare Unordered (fcmpu)";
      ulong index125 = index124 + 1UL;
      this.ASMDef[index125].name = "fcmpu";
      this.ASMDef[index125].opCode = 4227858432U;
      this.ASMDef[index125].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index125].shifts = new int[3]
      {
        23,
        16,
        11
      };
      this.ASMDef[index125].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index125].type = 11;
      this.ASMDef[index125].help = str38;
      this.ASMDef[index125].title = "Floating Point Compare Unordered (fcmpu)";
      ulong index126 = index125 + 1UL;
      this.InsPlaceArr[11] = (int) index126;
      string str39 = "Loads the byte from the address (IMM + rA) and stores it into lower 8 bits of rD\nThe remaining bits are cleared" + newLine + "lbz rD, IMM(rA) :: rD = (char)MEM(IMM + rA)" + newLine + newLine + "Example:" + newLine + "lbz r3, 0x4058(r4)" + newLine;
      this.ASMDef[index126].name = "lbz";
      this.ASMDef[index126].opCode = 2281701376U;
      this.ASMDef[index126].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index126].shifts = new int[3];
      this.ASMDef[index126].order = new int[3];
      this.ASMDef[index126].order[0] = 0;
      this.ASMDef[index126].order[1] = 1;
      this.ASMDef[index126].order[2] = 2;
      this.ASMDef[index126].shifts[0] = 21;
      this.ASMDef[index126].shifts[1] = 16;
      this.ASMDef[index126].shifts[2] = 0;
      this.ASMDef[index126].type = 3;
      this.ASMDef[index126].help = str39;
      this.ASMDef[index126].title = "Load Byte And Zero (lbz)";
      ulong index127 = index126 + 1UL;
      string str40 = "Loads the byte from the address (rA + rB) and stores it into lower 8 bits of rD\nThe remaining bits are cleared" + newLine + "lbzx rD, rA, rB :: rD = (char)MEM(rA + rB)" + newLine + newLine + "Example:" + newLine + "lbzx r3, r4, r5" + newLine;
      this.ASMDef[index127].name = "lbzx";
      this.ASMDef[index127].opCode = 2080374958U;
      this.ASMDef[index127].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index127].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index127].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index127].type = 0;
      this.ASMDef[index127].help = str40;
      this.ASMDef[index127].title = "Load Byte And Zero Indexed (lbzx)";
      ulong index128 = index127 + 1UL;
      string str41 = "Loads the double-word from the address (rA + rB) and stores it into rD" + newLine + "ldx rD, rA, rB :: rD = (long)MEM(rA + rB)" + newLine + newLine + "Example:" + newLine + "ldx r14, r3, r4" + newLine;
      this.ASMDef[index128].name = "ldx";
      this.ASMDef[index128].opCode = 2080374826U;
      this.ASMDef[index128].opShift = new int[2]
      {
        6,
        8
      };
      this.ASMDef[index128].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index128].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index128].type = 0;
      this.ASMDef[index128].help = str41;
      this.ASMDef[index128].title = "Load Doubleword Indexed (ldx)";
      ulong index129 = index128 + 1UL;
      string str42 = "Loads the double-word from the address (IMM + rA) and stores it into rD" + newLine + "ld rD, IMM(rA) :: rD = (long)MEM(IMM + rA)" + newLine + newLine + "Example:" + newLine + "ld r14, 0x0020(r1)" + newLine;
      this.ASMDef[index129].name = "ld";
      this.ASMDef[index129].opCode = 3892314112U;
      this.ASMDef[index129].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index129].shifts = new int[3];
      this.ASMDef[index129].order = new int[3];
      this.ASMDef[index129].order[0] = 0;
      this.ASMDef[index129].order[1] = 1;
      this.ASMDef[index129].order[2] = 2;
      this.ASMDef[index129].shifts[0] = 21;
      this.ASMDef[index129].shifts[1] = 16;
      this.ASMDef[index129].shifts[2] = 0;
      this.ASMDef[index129].type = 3;
      this.ASMDef[index129].help = str42;
      this.ASMDef[index129].title = "Load Doubleword (ld)";
      ulong index130 = index129 + 1UL;
      string str43 = "Loads the single from (IMM + rB) and converts it to double and stores it in frA" + newLine + "lfs frA, IMM(rB) :: frA = (double)((single)MEM(IMM + rB))" + newLine + newLine + "Example:" + newLine + "lfs f1, 0x0054(r5)" + newLine;
      this.ASMDef[index130].name = "lfs";
      this.ASMDef[index130].opCode = 3221225472U;
      this.ASMDef[index130].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index130].shifts = new int[3];
      this.ASMDef[index130].order = new int[3];
      this.ASMDef[index130].order[0] = 0;
      this.ASMDef[index130].order[1] = 1;
      this.ASMDef[index130].order[2] = 2;
      this.ASMDef[index130].shifts[0] = 21;
      this.ASMDef[index130].shifts[1] = 16;
      this.ASMDef[index130].shifts[2] = 0;
      this.ASMDef[index130].type = 10;
      this.ASMDef[index130].help = str43;
      this.ASMDef[index130].title = "Load Floating Point Single (lfs)";
      ulong index131 = index130 + 1UL;
      string str44 = "Loads the double from (IMM + rB) and stores it in frA" + newLine + "lfd frA, IMM(rB) :: rD = (double)MEM(IMM + rB)" + newLine + newLine + "Example:" + newLine + "lfd f1, 0x0054(r5)" + newLine;
      this.ASMDef[index131].name = "lfd";
      this.ASMDef[index131].opCode = 3355443200U;
      this.ASMDef[index131].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index131].shifts = new int[3]
      {
        21,
        16,
        0
      };
      this.ASMDef[index131].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index131].type = 10;
      this.ASMDef[index131].help = str44;
      this.ASMDef[index131].title = "Load Floating Point Doubleword (lfd)";
      ulong index132 = index131 + 1UL;
      string str45 = "Loads the halfword from the address (IMM + rA) and stores it into the lower 16 bits of rD\nThe remaining bits are cleared" + newLine + "lhz rD, IMM(rA) :: rD = (short)MEM(IMM + rA)" + newLine + newLine + "Example:" + newLine + "lhz r14, 0x0020(r1)" + newLine;
      this.ASMDef[index132].name = "lhz";
      this.ASMDef[index132].opCode = 2684354560U;
      this.ASMDef[index132].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index132].shifts = new int[3];
      this.ASMDef[index132].order = new int[3];
      this.ASMDef[index132].order[0] = 0;
      this.ASMDef[index132].order[1] = 1;
      this.ASMDef[index132].order[2] = 2;
      this.ASMDef[index132].shifts[0] = 21;
      this.ASMDef[index132].shifts[1] = 16;
      this.ASMDef[index132].shifts[2] = 0;
      this.ASMDef[index132].type = 3;
      this.ASMDef[index132].help = str45;
      this.ASMDef[index132].title = "Load Halfword And Zero (lhz)";
      ulong index133 = index132 + 1UL;
      string str46 = "Loads the halfword from the address (rA + rB) and stores it into lower 16 bits of rD\nThe remaining bits are cleared" + newLine + "lhzx rD, rA, rB :: rD = (short)MEM(rA + rB)" + newLine + newLine + "Example:" + newLine + "lhzx r3, r4, r5" + newLine;
      this.ASMDef[index133].name = "lhzx";
      this.ASMDef[index133].opCode = 2080375342U;
      this.ASMDef[index133].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index133].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index133].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index133].type = 0;
      this.ASMDef[index133].help = str46;
      this.ASMDef[index133].title = "Load Halfword And Zero Indexed (lhzx)";
      ulong index134 = index133 + 1UL;
      string str47 = "Sets the lower part of the register rD to the 16-bit signed value IMM" + newLine + "Result is similar to addis" + newLine + "lis rD, IMM :: rD = (IMM << 16)" + newLine + newLine + "Example:" + newLine + "lis r3, 0x1234" + newLine;
      this.ASMDef[index134].name = "lis";
      this.ASMDef[index134].opCode = 1006632960U;
      this.ASMDef[index134].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index134].shifts = new int[2];
      this.ASMDef[index134].shifts[0] = 21;
      this.ASMDef[index134].order = new int[2];
      this.ASMDef[index134].order[0] = 0;
      this.ASMDef[index134].order[1] = 1;
      this.ASMDef[index134].type = 4;
      this.ASMDef[index134].help = str47;
      this.ASMDef[index134].title = "Load Immediate Shifted (lis)";
      ulong index135 = index134 + 1UL;
      string str48 = "Sets the upper part of the register rD to the 16-bit signed value IMM" + newLine + "Result is similar to addi" + newLine + "li rD, IMM :: rD = IMM" + newLine + newLine + "Example:" + newLine + "li r3, 0x1234" + newLine;
      this.ASMDef[index135].name = "li";
      this.ASMDef[index135].opCode = 939524096U;
      this.ASMDef[index135].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index135].shifts = new int[2];
      this.ASMDef[index135].shifts[0] = 21;
      this.ASMDef[index135].order = new int[2];
      this.ASMDef[index135].order[0] = 0;
      this.ASMDef[index135].order[1] = 1;
      this.ASMDef[index135].type = 4;
      this.ASMDef[index135].help = str48;
      this.ASMDef[index135].title = "Load Immediate (li)";
      ulong index136 = index135 + 1UL;
      string str49 = "Loads the word from the address (IMM + rA) and stores it into rD" + newLine + "lwzx rD, rA, rB :: rD = (int)MEM(rA + rB)" + newLine + newLine + "Example:" + newLine + "lwzx r3, r4, r3" + newLine;
      this.ASMDef[index136].name = "lwzx";
      this.ASMDef[index136].opCode = 2080374830U;
      this.ASMDef[index136].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index136].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index136].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index136].type = 0;
      this.ASMDef[index136].help = str49;
      this.ASMDef[index136].title = "Load Word And Zero Indexed (lwzx)";
      ulong index137 = index136 + 1UL;
      string str50 = "Loads the word from the address (IMM + rA) and stores it into rD" + newLine + "lwz rD, IMM(rA) :: rD = (int)MEM(IMM + rA)" + newLine + newLine + "Example:" + newLine + "lwz r3, 0x4058(r4)" + newLine;
      this.ASMDef[index137].name = "lwz";
      this.ASMDef[index137].opCode = 0;
      this.ASMDef[index137].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index137].shifts = new int[3];
      this.ASMDef[index137].order = new int[3];
      this.ASMDef[index137].order[0] = 0;
      this.ASMDef[index137].order[1] = 1;
      this.ASMDef[index137].order[2] = 2;
      this.ASMDef[index137].shifts[0] = 21;
      this.ASMDef[index137].shifts[1] = 16;
      this.ASMDef[index137].shifts[2] = 0;
      this.ASMDef[index137].type = 3;
      this.ASMDef[index137].help = str50;
      this.ASMDef[index137].title = "Load Word And Zero (lwz)";
      ulong index138 = index137 + 1UL;
      this.InsPlaceArr[12] = (int) index138;
      string str51 = "The contents of SPR are placed into rD" + newLine + "mfspr rD, SPR :: rD = SPR" + newLine + newLine + "Example:" + newLine + "mfspr r0, LR" + newLine;
      this.ASMDef[index138].name = "mfspr";
      this.ASMDef[index138].opCode = 2080375462U;
      this.ASMDef[index138].opShift = new int[2]
      {
        6,
        14
      };
      this.ASMDef[index138].shifts = new int[2];
      this.ASMDef[index138].shifts[0] = 16;
      this.ASMDef[index138].shifts[1] = 21;
      this.ASMDef[index138].order = new int[2];
      this.ASMDef[index138].order[0] = 1;
      this.ASMDef[index138].order[1] = 0;
      this.ASMDef[index138].type = 5;
      this.ASMDef[index138].help = str51;
      this.ASMDef[index138].title = "Move From Special Purpose Register (mfspr)";
      ulong index139 = index138 + 1UL;
      string str52 = "The contents of rA are placed into SPR" + newLine + "mtspr SPR, rA :: SPR = rA" + newLine + newLine + "Example:" + newLine + "mtspr LR, r0" + newLine;
      this.ASMDef[index139].name = "mtspr";
      this.ASMDef[index139].opCode = 2080375718U;
      this.ASMDef[index139].opShift = new int[2]
      {
        6,
        14
      };
      this.ASMDef[index139].shifts = new int[2];
      this.ASMDef[index139].shifts[0] = 16;
      this.ASMDef[index139].shifts[1] = 21;
      this.ASMDef[index139].order = new int[2];
      this.ASMDef[index139].order[0] = 0;
      this.ASMDef[index139].order[1] = 1;
      this.ASMDef[index139].type = 5;
      this.ASMDef[index139].help = str52;
      this.ASMDef[index139].title = "Move To Special Purpose Register (mtspr)";
      ulong index140 = index139 + 1UL;
      string str53 = "Multiplies the 32 bits of rA and rB and stores the resulting 64 bits into rD" + newLine + "mullw rD, rA, rB :: rD = (long)(rA * rB)" + newLine + newLine + "Example:" + newLine + "mullw r3, r3, r3" + newLine;
      this.ASMDef[index140].name = "mullw";
      this.ASMDef[index140].opCode = 2080375254U;
      this.ASMDef[index140].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index140].shifts = new int[3];
      this.ASMDef[index140].shifts[0] = 21;
      this.ASMDef[index140].shifts[1] = 16;
      this.ASMDef[index140].shifts[2] = 11;
      this.ASMDef[index140].order = new int[3];
      this.ASMDef[index140].order[0] = 0;
      this.ASMDef[index140].order[1] = 1;
      this.ASMDef[index140].order[2] = 2;
      this.ASMDef[index140].type = 0;
      this.ASMDef[index140].help = str53;
      this.ASMDef[index140].title = "Multiply Low Word (mullw)";
      ulong index141 = index140 + 1UL;
      string str54 = "Multiplies the 64 bits of rA and IMM and stores the resulting 64 bits into rD" + newLine + "mulli rD, rA, IMM :: rD = (long)(rA * IMM)" + newLine + newLine + "Example:" + newLine + "mulli r3, r3, 3" + newLine;
      this.ASMDef[index141].name = "mulli";
      this.ASMDef[index141].opCode = 469762048U;
      this.ASMDef[index141].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index141].shifts = new int[3];
      this.ASMDef[index141].shifts[0] = 21;
      this.ASMDef[index141].shifts[1] = 16;
      this.ASMDef[index141].order = new int[3];
      this.ASMDef[index141].order[0] = 0;
      this.ASMDef[index141].order[1] = 1;
      this.ASMDef[index141].order[2] = 2;
      this.ASMDef[index141].type = 4;
      this.ASMDef[index141].help = str54;
      this.ASMDef[index141].title = "Multiply Low Immediate (mulli)";
      ulong index142 = index141 + 1UL;
      this.InsPlaceArr[13] = (int) index142;
      string str55 = "Does nothing (disassembled as ori, r0, r0, 0)" + newLine + "Operation:" + newLine + "nop :: r0 = r0 | 0" + newLine + newLine + "Example:" + newLine + "nop" + newLine;
      this.ASMDef[index142].name = "nop";
      this.ASMDef[index142].opCode = 1610612736U;
      this.ASMDef[index142].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index142].type = 2;
      this.ASMDef[index142].help = str55;
      this.ASMDef[index142].title = "No Operation (nop)";
      ulong index143 = index142 + 1UL;
      this.InsPlaceArr[14] = (int) index143;
      string str56 = "Logical or's rA with IMM and stores result in rD" + newLine + "ori rD, rA, IMM :: rD = rA | IMM" + newLine + newLine + "Example:" + newLine + "ori r3, r3, 0x1300" + newLine;
      this.ASMDef[index143].name = "ori";
      this.ASMDef[index143].opCode = 1610612736U;
      this.ASMDef[index143].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index143].shifts = new int[3];
      this.ASMDef[index143].shifts[0] = 21;
      this.ASMDef[index143].shifts[1] = 16;
      this.ASMDef[index143].order = new int[3];
      this.ASMDef[index143].order[0] = 1;
      this.ASMDef[index143].order[1] = 0;
      this.ASMDef[index143].order[2] = 2;
      this.ASMDef[index143].type = 4;
      this.ASMDef[index143].help = str56;
      this.ASMDef[index143].title = "Logical Or Immediate (ori)";
      ulong index144 = index143 + 1UL;
      string str57 = "Logical or's rA with rB and stores result in rD" + newLine + "or rD, rA, rB :: rD = rA | rB" + newLine + newLine + "Example:" + newLine + "or r3, r3, r4" + newLine;
      this.ASMDef[index144].name = "or";
      this.ASMDef[index144].opCode = 2080375672U;
      this.ASMDef[index144].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index144].shifts = new int[3];
      this.ASMDef[index144].shifts[0] = 21;
      this.ASMDef[index144].shifts[1] = 16;
      this.ASMDef[index144].shifts[2] = 11;
      this.ASMDef[index144].order = new int[3];
      this.ASMDef[index144].order[0] = 1;
      this.ASMDef[index144].order[1] = 0;
      this.ASMDef[index144].order[2] = 2;
      this.ASMDef[index144].type = 0;
      this.ASMDef[index144].help = str57;
      this.ASMDef[index144].title = "Logical Or (or)";
      ulong index145 = index144 + 1UL;
      string str58 = "Logical or's rA with IMM that has been shifted over 16 bits and stores result in rD" + newLine + "oris rD, rA, rB :: rD = (rA | IMM) << 16" + newLine + newLine + "Example:" + newLine + "oris r3, r4, 0x1234" + newLine;
      this.ASMDef[index145].name = "oris";
      this.ASMDef[index145].opCode = 1677721600U;
      this.ASMDef[index145].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index145].shifts = new int[3];
      this.ASMDef[index145].shifts[0] = 21;
      this.ASMDef[index145].shifts[1] = 16;
      this.ASMDef[index145].order = new int[3];
      this.ASMDef[index145].order[0] = 1;
      this.ASMDef[index145].order[1] = 0;
      this.ASMDef[index145].order[2] = 2;
      this.ASMDef[index145].type = 4;
      this.ASMDef[index145].help = str58;
      this.ASMDef[index145].title = "Logical Or Immediate Shifted (oris)";
      ulong index146 = index145 + 1UL;
      this.InsPlaceArr[18] = (int) index146;
      string str59 = "Shifts the double-word (64 bits) rA left by IMM bits and stores the result in rD" + newLine + "sldi rD, rA, rB :: rD = (long)(rA << IMM)" + newLine + newLine + "Example:" + newLine + "sldi r3, r27, 2" + newLine;
      this.ASMDef[index146].name = "sldi";
      this.ASMDef[index146].opCode = 2013265924U;
      this.ASMDef[index146].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index146].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index146].order = new int[3]
      {
        1,
        0,
        2
      };
      this.ASMDef[index146].type = 8;
      this.ASMDef[index146].help = str59;
      this.ASMDef[index146].title = "Shift Left Double-Word Immediate (sldi)";
      ulong index147 = index146 + 1UL;
      string str60 = "Shifts the double-word (64 bits) rA right by IMM bits and stores the result in rD. Then" + newLine + "srdi rD, rA, rB :: rD = (long)(rA >> IMM)" + newLine + newLine + "Example:" + newLine + "srdi r3, r27, 2" + newLine;
      this.ASMDef[index147].name = "srdi";
      this.ASMDef[index147].opCode = 2013265920U;
      this.ASMDef[index147].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index147].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index147].order = new int[3]
      {
        1,
        0,
        2
      };
      this.ASMDef[index147].type = 8;
      this.ASMDef[index147].help = str60;
      this.ASMDef[index147].title = "Shift Right Double-Word Immediate (srdi)";
      ulong index148 = index147 + 1UL;
      string str61 = "Shifts the word (32 bits) rA left by IMM bits and stores the result in rD" + newLine + "slwi rD, rA, rB :: rD = (int)(rA << IMM)" + newLine + newLine + "Example:" + newLine + "slwi r3, r27, 2" + newLine;
      this.ASMDef[index148].name = "slwi";
      this.ASMDef[index148].opCode = 1409286144U;
      this.ASMDef[index148].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index148].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index148].order = new int[3]
      {
        1,
        0,
        2
      };
      this.ASMDef[index148].type = 8;
      this.ASMDef[index148].help = str61;
      this.ASMDef[index148].title = "Shift Left Word Immediate (slwi)";
      ulong index149 = index148 + 1UL;
      string str62 = "Shifts the word (32 bits) rA right by IMM bits and stores the result in rD" + newLine + "srwi rD, rA, rB :: rD = (int)(rA >> IMM)" + newLine + newLine + "Example:" + newLine + "srwi r3, r4, 2" + newLine;
      this.ASMDef[index149].name = "srwi";
      this.ASMDef[index149].opCode = 1409286144U;
      this.ASMDef[index149].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index149].shifts = new int[3]
      {
        21,
        16,
        6
      };
      this.ASMDef[index149].order = new int[3]
      {
        1,
        0,
        2
      };
      this.ASMDef[index149].type = 8;
      this.ASMDef[index149].help = str62;
      this.ASMDef[index149].title = "Shift Right Word Immediate (srwi)";
      ulong index150 = index149 + 1UL;
      string str63 = "Shifts the word (32 bits) rA left by the last 5 bits of rB and stores the result in rD" + newLine + "slw rD, rA, rB :: rD = (int)(rA << rB)" + newLine + newLine + "Example:" + newLine + "slw r3, r4, r3" + newLine;
      this.ASMDef[index150].name = "slw";
      this.ASMDef[index150].opCode = 2080374832U;
      this.ASMDef[index150].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index150].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index150].order = new int[3]
      {
        1,
        0,
        2
      };
      this.ASMDef[index150].type = 0;
      this.ASMDef[index150].help = str63;
      this.ASMDef[index150].title = "Shift Left Word (slw)";
      ulong index151 = index150 + 1UL;
      string str64 = "Shifts the word (32 bits) rA right by the last 5 bits of rB and stores the result in rD" + newLine + "srw rD, rA, rB :: rD = (int)(rA >> rB)" + newLine + newLine + "Example:" + newLine + "srw r3, r4, r3" + newLine;
      this.ASMDef[index151].name = "srw";
      this.ASMDef[index151].opCode = 2080375856U;
      this.ASMDef[index151].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index151].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index151].order = new int[3]
      {
        1,
        0,
        2
      };
      this.ASMDef[index151].type = 0;
      this.ASMDef[index151].help = str64;
      this.ASMDef[index151].title = "Shift Right Word (srw)";
      ulong index152 = index151 + 1UL;
      string str65 = "Stores the byte rS at the address (IMM + rB)" + newLine + "stb rS, IMM(rB) :: MEM(IMM + rB) = (char)rS" + newLine + newLine + "Example:" + newLine + "stb r4, 0x0054(r5)" + newLine;
      this.ASMDef[index152].name = "stb";
      this.ASMDef[index152].opCode = 2550136832U;
      this.ASMDef[index152].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index152].shifts = new int[3];
      this.ASMDef[index152].order = new int[3];
      this.ASMDef[index152].order[0] = 0;
      this.ASMDef[index152].order[1] = 1;
      this.ASMDef[index152].order[2] = 2;
      this.ASMDef[index152].shifts[0] = 21;
      this.ASMDef[index152].shifts[1] = 16;
      this.ASMDef[index152].shifts[2] = 0;
      this.ASMDef[index152].type = 3;
      this.ASMDef[index152].help = str65;
      this.ASMDef[index152].title = "Store Byte (stb)";
      ulong index153 = index152 + 1UL;
      string str66 = "Stores the byte rS at the address (rA + rB)" + newLine + "stbx rS, rA, rB :: MEM(rA + rB) = (char)rS" + newLine + newLine + "Example:" + newLine + "stbx r4, r5, r3" + newLine;
      this.ASMDef[index153].name = "stbx";
      this.ASMDef[index153].opCode = 2080375214U;
      this.ASMDef[index153].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index153].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index153].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index153].type = 0;
      this.ASMDef[index153].help = str66;
      this.ASMDef[index153].title = "Store Byte Indexed (stbx)";
      ulong index154 = index153 + 1UL;
      string str67 = "Stores the doubleword (64 bits) rS at the address (IMM + rB). rB is then set to (IMM + rB)" + newLine + "stdu rS, IMM(rB) :: MEM(IMM + rB) = (long)rS; rB += IMM" + newLine + newLine + "Example:" + newLine + "stdu r4, 0x0054(r5)" + newLine;
      this.ASMDef[index154].name = "stdu";
      this.ASMDef[index154].opCode = 4160749569U;
      this.ASMDef[index154].opShift = new int[2]
      {
        6,
        1
      };
      this.ASMDef[index154].shifts = new int[3];
      this.ASMDef[index154].order = new int[3];
      this.ASMDef[index154].order[0] = 0;
      this.ASMDef[index154].order[1] = 1;
      this.ASMDef[index154].order[2] = 2;
      this.ASMDef[index154].shifts[0] = 21;
      this.ASMDef[index154].shifts[1] = 16;
      this.ASMDef[index154].shifts[2] = 0;
      this.ASMDef[index154].type = 3;
      this.ASMDef[index154].help = str67;
      this.ASMDef[index154].title = "Store Doubleword And Update (stdu)";
      ulong index155 = index154 + 1UL;
      string str68 = "Stores the word (64 bits) rS at the address (rA + rB)" + newLine + "stdx rS, rA, rB :: MEM(rA + rB) = (long)rS" + newLine + newLine + "Example:" + newLine + "stdx r3, r4, r3" + newLine;
      this.ASMDef[index155].name = "stdx";
      this.ASMDef[index155].opCode = 2080375082U;
      this.ASMDef[index155].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index155].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index155].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index155].type = 0;
      this.ASMDef[index155].help = str68;
      this.ASMDef[index155].title = "Store Doubleword Indexed (stdx)";
      ulong index156 = index155 + 1UL;
      string str69 = "Stores the doubleword (64 bits) rS at the address (IMM + rB)" + newLine + "std rS, IMM(rB) :: MEM(IMM + rB) = (long)rS" + newLine + newLine + "Example:" + newLine + "std r4, 0x0054(r5)" + newLine;
      this.ASMDef[index156].name = "std";
      this.ASMDef[index156].opCode = 4160749568U;
      this.ASMDef[index156].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index156].shifts = new int[3];
      this.ASMDef[index156].order = new int[3];
      this.ASMDef[index156].order[0] = 0;
      this.ASMDef[index156].order[1] = 1;
      this.ASMDef[index156].order[2] = 2;
      this.ASMDef[index156].shifts[0] = 21;
      this.ASMDef[index156].shifts[1] = 16;
      this.ASMDef[index156].shifts[2] = 0;
      this.ASMDef[index156].type = 3;
      this.ASMDef[index156].help = str69;
      this.ASMDef[index156].title = "Store Doubleword (std)";
      ulong index157 = index156 + 1UL;
      string str70 = "Converts the float in frS to a single and stores it at (IMM + rB)" + newLine + "stfs frS, IMM(rB) :: MEM(IMM + rB) = (Single)frS" + newLine + newLine + "Example:" + newLine + "stfs f1, 0x0054(r5)" + newLine;
      this.ASMDef[index157].name = "stfs";
      this.ASMDef[index157].opCode = 3489660928U;
      this.ASMDef[index157].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index157].shifts = new int[3];
      this.ASMDef[index157].order = new int[3];
      this.ASMDef[index157].order[0] = 0;
      this.ASMDef[index157].order[1] = 1;
      this.ASMDef[index157].order[2] = 2;
      this.ASMDef[index157].shifts[0] = 21;
      this.ASMDef[index157].shifts[1] = 16;
      this.ASMDef[index157].shifts[2] = 0;
      this.ASMDef[index157].type = 10;
      this.ASMDef[index157].help = str70;
      this.ASMDef[index157].title = "Store Floating Point Single (stfs)";
      ulong index158 = index157 + 1UL;
      string str71 = "Store the double from (IMM + rB) and stores it in frA" + newLine + "stfd frA, IMM(rB) :: MEM(IMM + rB) = (double)frA" + newLine + newLine + "Example:" + newLine + "stfd f1, 0x0054(r5)" + newLine;
      this.ASMDef[index158].name = "stfd";
      this.ASMDef[index158].opCode = 3623878656U;
      this.ASMDef[index158].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index158].shifts = new int[3]
      {
        21,
        16,
        0
      };
      this.ASMDef[index158].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index158].type = 10;
      this.ASMDef[index158].help = str71;
      this.ASMDef[index158].title = "Store Floating Point Doubleword (stfd)";
      ulong index159 = index158 + 1UL;
      string str72 = "Stores the lower 16 bits of rS at the address (IMM + rA)" + newLine + "sth rS, IMM(rA) :: MEM(IMM + rA) = (short)rS" + newLine + newLine + "Example:" + newLine + "sth r14, 0x0020(r1)" + newLine;
      this.ASMDef[index159].name = "sth";
      this.ASMDef[index159].opCode = 2952790016U;
      this.ASMDef[index159].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index159].shifts = new int[3];
      this.ASMDef[index159].order = new int[3];
      this.ASMDef[index159].order[0] = 0;
      this.ASMDef[index159].order[1] = 1;
      this.ASMDef[index159].order[2] = 2;
      this.ASMDef[index159].shifts[0] = 21;
      this.ASMDef[index159].shifts[1] = 16;
      this.ASMDef[index159].shifts[2] = 0;
      this.ASMDef[index159].type = 3;
      this.ASMDef[index159].help = str72;
      this.ASMDef[index159].title = "Store Halfword (sth)";
      ulong index160 = index159 + 1UL;
      string str73 = "Stores the lower 16 bits of rS at the address (rA + rB)" + newLine + "sthx rS, rA, rB :: MEM(rA + rB) = (short)rS" + newLine + newLine + "Example:" + newLine + "sthx r14, r1, r4" + newLine;
      this.ASMDef[index160].name = "sthx";
      this.ASMDef[index160].opCode = 2080375598U;
      this.ASMDef[index160].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index160].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index160].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index160].type = 0;
      this.ASMDef[index160].help = str73;
      this.ASMDef[index160].title = "Store Halfword Indexed (sthx)";
      ulong index161 = index160 + 1UL;
      string str74 = "Stores the word (32 bits) rS at the address (rA + rB)" + newLine + "stwx rS, rA, rB :: MEM(rA + rB) = (int)rS" + newLine + newLine + "Example:" + newLine + "stwx r3, r4, r3" + newLine;
      this.ASMDef[index161].name = "stwx";
      this.ASMDef[index161].opCode = 2080375086U;
      this.ASMDef[index161].opShift = new int[2]
      {
        6,
        10
      };
      this.ASMDef[index161].shifts = new int[3]
      {
        21,
        16,
        11
      };
      this.ASMDef[index161].order = new int[3]
      {
        0,
        1,
        2
      };
      this.ASMDef[index161].type = 0;
      this.ASMDef[index161].help = str74;
      this.ASMDef[index161].title = "Store Word Indexed (stwx)";
      ulong index162 = index161 + 1UL;
      string str75 = "Stores the word (32 bits) rS at the address (IMM + rB)" + newLine + "stw rS, IMM(rB) :: MEM(IMM + rB) = (int)rS" + newLine + newLine + "Example:" + newLine + "stw r4, 0x0054(r5)" + newLine;
      this.ASMDef[index162].name = "stw";
      this.ASMDef[index162].opCode = 2415919104U;
      this.ASMDef[index162].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index162].shifts = new int[3];
      this.ASMDef[index162].order = new int[3];
      this.ASMDef[index162].order[0] = 0;
      this.ASMDef[index162].order[1] = 1;
      this.ASMDef[index162].order[2] = 2;
      this.ASMDef[index162].shifts[0] = 21;
      this.ASMDef[index162].shifts[1] = 16;
      this.ASMDef[index162].shifts[2] = 0;
      this.ASMDef[index162].type = 3;
      this.ASMDef[index162].help = str75;
      this.ASMDef[index162].title = "Store Word (stw)";
      ulong index163 = index162 + 1UL;
      string str76 = "Stores the word (32 bits) rS at the address (IMM + rB). rB is then set to (IMM + rB)" + newLine + "stwu rS, IMM(rB) :: MEM(IMM + rB) = (int)rS; rB += IMM" + newLine + newLine + "Example:" + newLine + "stwu r4, 0x0054(r5)" + newLine;
      this.ASMDef[index163].name = "stwu";
      this.ASMDef[index163].opCode = 2483027968U;
      this.ASMDef[index163].opShift = new int[2]
      {
        6,
        0
      };
      this.ASMDef[index163].shifts = new int[3];
      this.ASMDef[index163].order = new int[3];
      this.ASMDef[index163].order[0] = 0;
      this.ASMDef[index163].order[1] = 1;
      this.ASMDef[index163].order[2] = 2;
      this.ASMDef[index163].shifts[0] = 21;
      this.ASMDef[index163].shifts[1] = 16;
      this.ASMDef[index163].shifts[2] = 0;
      this.ASMDef[index163].type = 3;
      this.ASMDef[index163].help = str76;
      this.ASMDef[index163].title = "Store Word And Update (stwu)";
      num33 = index163 + 1UL;
    }
    public int GetInsStart(int charOff)
    {
      charOff -= 65;
      if (charOff > InsPlaceArr.Length)
        charOff -= 32;
      if (charOff < 0 || charOff >= InsPlaceArr.Length)
        return 0;
      return InsPlaceArr[charOff];
    }
    public void DeclareHelpStr()
    {
      int num1 = 0;
      string str1 = "\n";
      int num2 = 0;
      string[] strArray1;
      IntPtr index1;
      (strArray1 = this.helpCom)[(int) (index1 = (IntPtr) num2)] = strArray1[(int) index1] + "Set the current address of the subroutine" + str1;
      string[] strArray2;
      IntPtr index2;
      (strArray2 = this.helpCom)[(int) (index2 = (IntPtr) num2)] = strArray2[(int)index2] + "The address will automatically be incremented by 4 on each instruction" + str1;
      string[] strArray3;
      IntPtr index3;
      (strArray3 = this.helpCom)[(int) (index3 = (IntPtr) num2)] = strArray3[(int)index3] + str1 + "address 0x003D44C8" + str1;
      int num3 = num2 + 1;
      string[] strArray4;
      IntPtr index4;
      (strArray4 = this.helpCom)[(int) (index4 = (IntPtr) num3)] = strArray4[(int)index4] + "Set the current hook address of the subroutine (b). At the end of the compiled subroutine there will be a line with the address of the hook and a b to the address" + str1;
      string[] strArray5;
      IntPtr index5;
      (strArray5 = this.helpCom)[(int) (index5 = (IntPtr) num3)] = strArray5[(int)index5] + "There can only be one hook address at this point" + str1;
      string[] strArray6;
      IntPtr index6;
      (strArray6 = this.helpCom)[(int) (index6 = (IntPtr) num3)] = strArray6[(int)index6] + str1 + "hook 0x0174DC68" + str1;
      int num4 = num3 + 1;
      string[] strArray7;
      IntPtr index7;
      (strArray7 = this.helpCom)[(int) (index7 = (IntPtr) num4)] = strArray7[(int)index7] + "Set the current hook address of the subroutine (bl). At the end of the compiled subroutine there will be a line with the address of the hook and a bl to the address" + str1;
      string[] strArray8;
      IntPtr index8;
      (strArray8 = this.helpCom)[(int) (index8 = (IntPtr) num4)] = strArray8[(int)index8] + "There can only be one hook address at this point" + str1;
      string[] strArray9;
      IntPtr index9;
      (strArray9 = this.helpCom)[(int) (index9 = (IntPtr) num4)] = strArray9[(int)index9] + str1 + "hookl 0x0174DC68" + str1;
      int num5 = num4 + 1;
      string[] strArray10;
      IntPtr index10;
      (strArray10 = this.helpCom)[(int) (index10 = (IntPtr) num5)] = strArray10[(int)index10] + "Sets rD to the 32 bit unsigned IMM" + str1;
      string[] strArray11;
      IntPtr index11;
      (strArray11 = this.helpCom)[(int) (index11 = (IntPtr) num5)] = strArray11[(int)index11] + "setreg rD, IMM :: rD = IMM" + str1;
      string[] strArray12;
      IntPtr index12;
      (strArray12 = this.helpCom)[(int) (index12 = (IntPtr) num5)] = strArray12[(int)index12] + str1 + "setreg r3, 0x12345678  ->" + str1;
      string[] strArray13;
      int index13;
      string str2 = (strArray13 = this.helpCom)[(index13 = num5)] + "\tlis r3, 0x1234" + str1 + "\tori r3, r3, 0x5678" + str1;
      strArray13[index13] = str2;
      int num6 = num5 + 1;
      string[] strArray14;
      IntPtr index14;
      (strArray14 = this.helpCom)[(int) (index14 = (IntPtr) num6)] = strArray14[(int)index14] + "Inserts the value specified into the assembled code" + str1;
      string[] strArray15;
      IntPtr index15;
      (strArray15 = this.helpCom)[(int) (index15 = (IntPtr) num6)] = strArray15[(int)index15] + "Useful for unsupported instructions" + str1;
      string[] strArray16;
      IntPtr index16;
      (strArray16 = this.helpCom)[(int) (index16 = (IntPtr) num6)] = strArray16[(int)index16] + str1 + "hexcode 0x12345678  ->" + str1;
      string[] strArray17;
      IntPtr index17;
      (strArray17 = this.helpCom)[(int) (index17 = (IntPtr) num6)] = strArray17[(int)index17] + "\t2 00000000 12345678" + str1;
      int num7 = num6 + 1;
      string[] strArray18;
      IntPtr index18;
      (strArray18 = this.helpCom)[(int) (index18 = (IntPtr) num7)] = strArray18[(int)index18] + "Inserts the CWP3 file(s) specified into the assembly at compile time" + str1;
      string[] strArray19;
      IntPtr index19;
      (strArray19 = this.helpCom)[(int) (index19 = (IntPtr) num7)] = strArray19[(int)index19] + "If a full path is not specified, the path will be that of the current cwp3 file" + str1;
      string[] strArray20;
      int index20;
      string str3 = (strArray20 = helpCom)[(index20 = num7)] + str1 + "import Math.cwp3" + str1 + "import C:\\CodeWizard PS3\\Math.cwp3" + str1;
      strArray20[index20] = str3;
      int num8 = num7 + 1;
      string[] strArray21;
      IntPtr index21;
      (strArray21 = this.helpCom)[(int) (index21 = (IntPtr) num8)] = strArray21[(int)index21] + "Inserts the float value specified into the assembled code as hex" + str1;
      string[] strArray22;
      IntPtr index22;
      (strArray22 = this.helpCom)[(int) (index22 = (IntPtr) num8)] = strArray22[(int)index22] + str1 + "float 3.14159  ->" + str1;
      string[] strArray23;
      IntPtr index23;
      (strArray23 = this.helpCom)[(int) (index23 = (IntPtr) num8)] = strArray23[(int)index23] + "\t2 00000000 40490FD0" + str1;
      int num9 = num8 + 1;
      string[] strArray24;
      IntPtr index24;
      (strArray24 = this.helpCom)[(int) (index24 = (IntPtr) num9)] = strArray24[(int)index24] + "Inserts the string specified into the assembled code as hex" + str1;
      string[] strArray25;
      IntPtr index25;
      (strArray25 = this.helpCom)[(int) (index25 = (IntPtr) num9)] = strArray25[(int)index25] + str1 + "string ABCDE F  ->" + str1;
      string[] strArray26;
      IntPtr index26;
      (strArray26 = this.helpCom)[(int) (index26 = (IntPtr) num9)] = strArray26[(int)index26] + "\t2 00000000 41424344" + str1;
      string[] strArray27;
      IntPtr index27;
      (strArray27 = this.helpCom)[(int) (index27 = (IntPtr) num9)] = strArray27[(int)index27] + "\t2 00000004 45204600" + str1;
      num1 = num9 + 1;
      int num10 = 0;
      string[] strArray28;
      IntPtr index28;
      (strArray28 = this.helpReg)[(int) (index28 = (IntPtr) num10)] = strArray28[(int)index28] + "More information on PPC registers: http://www.csd.uwo.ca/~mburrel/stuff/ppc-asm.html" + str1;
      string[] strArray29;
      IntPtr index29;
      (strArray29 = this.helpReg)[(int) (index29 = (IntPtr) num10)] = strArray29[(int)index29] + "Used to hold the old link register when building the stack" + str1;
      string[] strArray30;
      IntPtr index30;
      (strArray30 = this.helpReg)[(int) (index30 = (IntPtr) num10)] = strArray30[(int)index30] + str1 + "It is best not to modify this register unless dealing with the stack" + str1;
      int num11 = num10 + 1;
      string[] strArray31;
      IntPtr index31;
      (strArray31 = this.helpReg)[(int) (index31 = (IntPtr) num11)] = strArray31[(int)index31] + "More information on PPC registers: http://www.csd.uwo.ca/~mburrel/stuff/ppc-asm.html" + str1;
      string[] strArray32;
      IntPtr index32;
      (strArray32 = this.helpReg)[(int) (index32 = (IntPtr) num11)] = strArray32[(int)index32] + "Points to the beginning of the stack" + str1;
      string[] strArray33;
      IntPtr index33;
      (strArray33 = this.helpReg)[(int) (index33 = (IntPtr) num11)] = strArray33[(int)index33] + str1 + "Only modify when dealing with the stack" + str1;
      int num12 = num11 + 1;
      string[] strArray34;
      IntPtr index34;
      (strArray34 = this.helpReg)[(int) (index34 = (IntPtr) num12)] = strArray34[(int)index34] + "More information on Register 2: http://physinfo.ulb.ac.be/divers_html/powerpc_programming_info/intro_to_ppc/ppc4_runtime4.html" + str1;
      string[] strArray35;
      IntPtr index35;
      (strArray35 = this.helpReg)[(int) (index35 = (IntPtr) num12)] = strArray35[(int)index35] + "A collection of pointers that the code uses to locate its static data" + str1;
      string[] strArray36;
      IntPtr index36;
      (strArray36 = this.helpReg)[(int) (index36 = (IntPtr) num12)] = strArray36[(int)index36] + str1 + "Do not modify this register" + str1;
      int num13 = num12 + 1;
      string[] strArray37;
      IntPtr index37;
      (strArray37 = this.helpReg)[(int) (index37 = (IntPtr) num13)] = strArray37[(int)index37] + "More information on PPC registers: http://www.csd.uwo.ca/~mburrel/stuff/ppc-asm.html" + str1;
      string[] strArray38;
      IntPtr index38;
      (strArray38 = this.helpReg)[(int) (index38 = (IntPtr) num13)] = strArray38[(int)index38] + "Commonly used as the return value of a function, and also the first argument" + str1;
      string[] strArray39;
      IntPtr index39;
      (strArray39 = this.helpReg)[(int) (index39 = (IntPtr) num13)] = strArray39[(int)index39] + str1 + "Free to be used" + str1;
      int num14 = num13 + 1;
      string[] strArray40;
      IntPtr index40;
      (strArray40 = this.helpReg)[(int) (index40 = (IntPtr) num14)] = strArray40[(int)index40] + "More information on PPC registers: http://www.csd.uwo.ca/~mburrel/stuff/ppc-asm.html" + str1;
      string[] strArray41;
      IntPtr index41;
      (strArray41 = this.helpReg)[(int) (index41 = (IntPtr) num14)] = strArray41[(int)index41] + "Commonly used as function arguments 2 through 8" + str1;
      string[] strArray42;
      IntPtr index42;
      (strArray42 = this.helpReg)[(int) (index42 = (IntPtr) num14)] = strArray42[(int)index42] + str1 + "Free to be used" + str1;
      int num15 = num14 + 1;
      string[] strArray43;
      IntPtr index43;
      (strArray43 = this.helpReg)[(int) (index43 = (IntPtr) num15)] = strArray43[(int)index43] + "More information on PPC registers: http://wiibrew.org/wiki/Assembler_Tutorial#Application_Binary_Interface_.28SVR4_ABI.29" + str1;
      string[] strArray44;
      IntPtr index44;
      (strArray44 = this.helpReg)[(int) (index44 = (IntPtr) num15)] = strArray44[(int)index44] + "Used with general purpose operations" + str1;
      string[] strArray45;
      IntPtr index45;
      (strArray45 = this.helpReg)[(int) (index45 = (IntPtr) num15)] = strArray45[(int)index45] + str1 + "Free to be used" + str1;
      int num16 = num15 + 1;
      string[] strArray46;
      IntPtr index46;
      (strArray46 = this.helpReg)[(int) (index46 = (IntPtr) num16)] = strArray46[(int)index46] + "More information on PPC registers: http://wiibrew.org/wiki/Assembler_Tutorial#Application_Binary_Interface_.28SVR4_ABI.29" + str1;
      string[] strArray47;
      IntPtr index47;
      (strArray47 = this.helpReg)[(int) (index47 = (IntPtr) num16)] = strArray47[(int)index47] + "Points to a small data area" + str1;
      string[] strArray48;
      IntPtr index48;
      (strArray48 = this.helpReg)[(int) (index48 = (IntPtr) num16)] = strArray48[(int)index48] + str1 + "Free to be used, however, I haven't yet seen it used in function" + str1;
      int num17 = num16 + 1;
      string[] strArray49;
      IntPtr index49;
      (strArray49 = this.helpReg)[(int) (index49 = (IntPtr) num17)] = strArray49[(int)index49] + "More information on PPC registers: http://www.csd.uwo.ca/~mburrel/stuff/ppc-asm.html" + str1;
      string[] strArray50;
      IntPtr index50;
      (strArray50 = this.helpReg)[(int) (index50 = (IntPtr) num17)] = strArray50[(int)index50] + "Used with general purpose operations" + str1;
      string[] strArray51;
      IntPtr index51;
      (strArray51 = this.helpReg)[(int) (index51 = (IntPtr) num17)] = strArray51[(int)index51] + str1 + "Free to be used, BUT, they must be preserved in the stack" + str1;
      int num18 = num17 + 1;
      string[] strArray52;
      IntPtr index52;
      (strArray52 = this.helpReg)[(int) (index52 = (IntPtr) num18)] = strArray52[(int)index52] + "Used with floating point operations" + str1;
      string[] strArray53;
      IntPtr index53;
      (strArray53 = this.helpReg)[(int) (index53 = (IntPtr) num18)] = strArray53[(int)index53] + str1 + "Free to be used" + str1;
      int num19 = num18 + 1;
      string[] strArray54;
      IntPtr index54;
      (strArray54 = this.helpReg)[(int) (index54 = (IntPtr) num19)] = strArray54[(int)index54] + "Used with floating point operations" + str1;
      string[] strArray55;
      IntPtr index55;
      (strArray55 = this.helpReg)[(int) (index55 = (IntPtr) num19)] = strArray55[(int)index55] + str1 + "Free to be used, BUT, they must be preserved in the stack" + str1;
      int num20 = num19 + 1;
      string[] strArray56;
      IntPtr index56;
      (strArray56 = this.helpReg)[(int) (index56 = (IntPtr) num20)] = strArray56[(int)index56] + "Hold results of comparisons" + str1;
      string[] strArray57;
      IntPtr index57;
      (strArray57 = this.helpReg)[(int) (index57 = (IntPtr) num20)] = strArray57[(int)index57] + str1 + "Free to be used, BUT, they must be preserved in the stack" + str1;
      string[] strArray58;
      IntPtr index58;
      (strArray58 = this.helpReg)[(int) (index58 = (IntPtr) num20)] = strArray58[(int)index58] + "See the instructions cmpwi, cmplwi, cmpw, and cmplw for usage" + str1;
      int num21 = num20 + 1;
      string[] strArray59;
      IntPtr index59;
      (strArray59 = this.helpReg)[(int) (index59 = (IntPtr) num21)] = strArray59[(int)index59] + "Overflows and Exception stuff" + str1;
      string[] strArray60;
      IntPtr index60;
      (strArray60 = this.helpReg)[(int) (index60 = (IntPtr) num21)] = strArray60[(int)index60] + "For more info search for the section \"XER Register (XER)\" at http://www.cebix.net/downloads/bebox/pem32b.pdf" + str1;
      string[] strArray61;
      IntPtr index61;
      (strArray61 = this.helpReg)[(int) (index61 = (IntPtr) num21)] = strArray61[(int)index61] + "Do not modify this special register" + str1;
      int num22 = num21 + 1;
      string[] strArray62;
      IntPtr index62;
      (strArray62 = this.helpReg)[(int) (index62 = (IntPtr) num22)] = strArray62[(int)index62] + "Holds return address when a linking branch is called" + str1;
      string[] strArray63;
      IntPtr index63;
      (strArray63 = this.helpReg)[(int) (index63 = (IntPtr) num22)] = strArray63[(int)index63] + str1 + "To be used exclusively for function calling" + str1;
      int num23 = num22 + 1;
      string[] strArray64;
      IntPtr index64;
      (strArray64 = this.helpReg)[(int) (index64 = (IntPtr) num23)] = strArray64[(int)index64] + "Holds a loop count that can be decremented during execution of branch instructions" + str1;
      string[] strArray65;
      IntPtr index65;
      (strArray65 = this.helpReg)[(int) (index65 = (IntPtr) num23)] = strArray65[(int)index65] + str1 + "For more info search for the section \"Count Register (CTR)\" at http://www.cebix.net/downloads/bebox/pem32b.pdf" + str1;
      num1 = num23 + 1;
      int index66 = 0;
      this.helpTerm[index66] = "rA - Register A" + str1;
      string[] strArray66;
      IntPtr index67;
      (strArray66 = this.helpTerm)[(int) (index67 = (IntPtr) index66)] = strArray66[(int)index67] + "First non-destination register in an assembly code" + str1;
      int index68 = index66 + 1;
      this.helpTerm[index68] = "rB - Register B" + str1;
      string[] strArray67;
      IntPtr index69;
      (strArray67 = this.helpTerm)[(int) (index69 = (IntPtr) index68)] = strArray67[(int)index69] + "Second non-destination register in an assembly code" + str1;
      int index70 = index68 + 1;
      this.helpTerm[index70] = "rD - Destination Register" + str1;
      string[] strArray68;
      IntPtr index71;
      (strArray68 = this.helpTerm)[(int) (index71 = (IntPtr) index70)] = strArray68[(int)index71] + "First destination register in an assembly code" + str1;
      int index72 = index70 + 1;
      this.helpTerm[index72] = "rS - Source Register" + str1;
      string[] strArray69;
      IntPtr index73;
      (strArray69 = this.helpTerm)[(int) (index73 = (IntPtr) index72)] = strArray69[(int)index73] + "First source register in an assembly code" + str1;
      int index74 = index72 + 1;
      this.helpTerm[index74] = "BF - Conditional Register" + str1;
      string[] strArray70;
      IntPtr index75;
      (strArray70 = this.helpTerm)[(int) (index75 = (IntPtr) index74)] = strArray70[(int)index75] + "Conditional register used in the following instructions:" + str1;
      string[] strArray71;
      IntPtr index76;
      (strArray71 = this.helpTerm)[(int) (index76 = (IntPtr) index74)] = strArray71[(int)index76] + "beq, bne, ble, blt, bgt, bge, cmpwi, cmplwi, cmpw, cmplw" + str1;
      int index77 = index74 + 1;
      this.helpTerm[index77] = "IMM - Immediate Value" + str1;
      string[] strArray72;
      IntPtr index78;
      (strArray72 = this.helpTerm)[(int) (index78 = (IntPtr) index77)] = strArray72[(int)index78] + "A constant value with a bit length defined by the instruction" + str1;
      int index79 = index77 + 1;
      this.helpTerm[index79] = "Signed" + str1;
      string[] strArray73;
      IntPtr index80;
      (strArray73 = this.helpTerm)[(int) (index80 = (IntPtr) index79)] = strArray73[(int)index80] + "Defines that the value can be negative" + str1;
      int index81 = index79 + 1;
      this.helpTerm[index81] = "Unsigned" + str1;
      string[] strArray74;
      IntPtr index82;
      (strArray74 = this.helpTerm)[(int) (index82 = (IntPtr) index81)] = strArray74[(int)index82] + "Defines that the value cannot be negative" + str1;
      string[] strArray75;
      IntPtr index83;
      (strArray75 = this.helpTerm)[(int) (index83 = (IntPtr) index81)] = strArray75[(int)index83] + "An unsigned 32-bit value of 0xFFFFFFFF is equal to 4294967295";
      string[] strArray76;
      IntPtr index84;
      (strArray76 = this.helpTerm)[(int) (index84 = (IntPtr) index81)] = strArray76[(int)index84] + " while a signed 32-bit value of 0xFFFFFFFF is equal to -1" + str1;
      num1 = index81 + 1;
    }

    public struct pInstruction
    {
      public string name;
      public string[] regs;
      public string format;
      public string asm;
    }
    public struct RegCol
    {
      public string reg;
      public Brush col;
      public string title;
    }
    public struct PPCInstr
    {
      public string name;
      public uint opCode;
      public int[] opShift;
      public int[] shifts;
      public int[] order;
      public int type;
      public string help;
      public string title;
    }
    public struct ASMLabel
    {
      public uint address;
      public string name;
    }
  }
}