// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Utilities.DrivesHelper
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PS3SaveEditor.Utilities
{
  public class DrivesHelper
  {
    private ComboBox driveBox;
    private List<game> gameList;
    private CheckBox chkShowAll;
    private Panel pnlNoSaves;
    private Button btnResign;
    private Button btnImport;
    private DrivesHelper.ClearDrivesDelegate ClearDrivesFunc;
    private DrivesHelper.AddItemDelegate AddItemFunc;

    public DrivesHelper(
      ComboBox cdDrives,
      List<game> m_games,
      CheckBox showAll,
      Panel noSaves,
      Button resign,
      Button import)
    {
      this.driveBox = cdDrives;
      this.gameList = m_games;
      this.chkShowAll = showAll;
      this.pnlNoSaves = noSaves;
      this.btnResign = resign;
      this.btnImport = import;
      this.ClearDrivesFunc = new DrivesHelper.ClearDrivesDelegate(this.ClearDrives);
      this.AddItemFunc = new DrivesHelper.AddItemDelegate(this.AddItem);
    }

    public DrivesHelper(ComboBox cdDrives, List<game> m_games, CheckBox showAll, Panel noSaves)
      : this(cdDrives, m_games, showAll, noSaves, new Button(), new Button())
    {
    }

    public void HandleDrive(object drive) => this.FillDrives();

    public void FillDrives()
    {
      this.driveBox.Invoke((Delegate) this.ClearDrivesFunc);
      string locationFromRegistry = Util.GetCheatsLocationFromRegistry();
      if (!string.IsNullOrEmpty(locationFromRegistry))
        this.driveBox.Invoke((Delegate) this.AddItemFunc, (object) locationFromRegistry);
      foreach (DriveInfo drive in DriveInfo.GetDrives())
      {
        bool flag1;
        if (Util.IsUnixOrMacOSX())
        {
          bool flag2 = (drive.Name.Contains("media") || drive.Name.Contains("Volumes")) && Directory.Exists(drive.ToString() + "/PS4");
          flag1 = drive.IsReady && drive.DriveType == DriveType.Removable | flag2;
        }
        else
          flag1 = drive.IsReady && drive.DriveType == DriveType.Removable;
        if (flag1)
        {
          if (drive != null)
            this.driveBox.Invoke((Delegate) this.AddItemFunc, (object) (Util.CurrentPlatform != Util.Platform.Windows ? string.Format("/{0}", (object) Path.GetFileName(drive.RootDirectory.FullName)) : drive.RootDirectory.FullName));
          else
            this.driveBox.Invoke((Delegate) this.AddItemFunc, new object[1]);
        }
      }
      this.driveBox.Invoke((Delegate) this.AddItemFunc, (object) PS3SaveEditor.Resources.Resources.colSelect);
      this.SetCbDrivesBoxSize(true);
    }

    private void SetCbDrivesBoxSize(bool useLongSize)
    {
      if (Util.IsUnixOrMacOSX())
      {
        this.driveBox.Location = new Point(Util.ScaleSize(65), Util.ScaleSize(2));
        this.driveBox.Width = Util.ScaleSize(165);
      }
      else if (useLongSize)
      {
        this.driveBox.Location = new Point(Util.ScaleSize(65), Util.ScaleSize(5));
        this.driveBox.Width = Util.ScaleSize(165);
      }
      else
      {
        this.driveBox.Location = new Point(Util.ScaleSize(185), Util.ScaleSize(5));
        this.driveBox.Width = Util.ScaleSize(45);
      }
    }

    private void AddItem(string item)
    {
      if (item != null)
      {
        int num = this.driveBox.Items.Add((object) item);
        string str1 = item;
        if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(str1))
          str1 = string.Format("/Volumes{0}", (object) str1);
        else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(str1))
          str1 = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) str1);
        if (Directory.Exists(Util.GetDataPath(str1)) && (uint) Directory.GetDirectories(Util.GetDataPath(str1)).Length > 0U)
        {
          this.pnlNoSaves.Visible = false;
          this.pnlNoSaves.SendToBack();
          if (this.driveBox.SelectedIndex < 0)
          {
            this.driveBox.SelectedIndex = num;
          }
          else
          {
            string str2 = this.driveBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(str2))
            {
              if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(str2))
                str2 = string.Format("/Volumes{0}", (object) str2);
              else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(str2))
                str2 = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) str2);
              if (!Directory.Exists(Util.GetDataPath(str2)) || (uint) Directory.GetDirectories(Util.GetDataPath(str2)).Length <= 0U)
                this.driveBox.SelectedIndex = num;
            }
          }
        }
        else if (this.driveBox.SelectedIndex < 0)
        {
          this.pnlNoSaves.Visible = true;
          this.pnlNoSaves.BringToFront();
          this.driveBox.SelectedIndex = num;
        }
        if (!this.chkShowAll.Enabled)
        {
          this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = true;
          this.chkShowAll.Checked = false;
        }
      }
      if (!string.IsNullOrEmpty(item) || this.gameList.Count <= 0)
        return;
      this.chkShowAll.Checked = true;
      this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = false;
    }

    private void ClearDrives()
    {
      this.driveBox.Items.Clear();
      if (this.driveBox.Items.Count > 0)
        this.driveBox.SelectedIndex = 0;
      else if (this.gameList.Count > 0)
      {
        this.chkShowAll.Checked = true;
        this.chkShowAll.Enabled = false;
      }
    }

    private delegate void ClearDrivesDelegate();

    private delegate void AddItemDelegate(string item);
  }
}
