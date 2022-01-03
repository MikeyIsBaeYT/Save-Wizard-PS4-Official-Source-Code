// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SimpleSaveUploader
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class SimpleSaveUploader : Form
  {
    private SimpleSaveUploader.CloseDelegate CloseForm;
    private game m_game;
    private bool appClosing = false;
    private IContainer components = (IContainer) null;
    private SaveUploadDownloder saveUploadDownloder1;

    public SimpleSaveUploader(game gameItem, string profile, List<string> files)
    {
      this.m_game = gameItem;
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = PS3SaveEditor.Resources.Resources.titleSimpleEditUploader;
      this.CenterToScreen();
      this.BackColor = Color.FromArgb(80, 29, 11);
      this.saveUploadDownloder1.BackColor = Color.FromArgb(200, 100, 10);
      this.saveUploadDownloder1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.saveUploadDownloder1.Files = files.ToArray();
      this.saveUploadDownloder1.Action = "patch";
      this.saveUploadDownloder1.OutputFolder = Path.GetDirectoryName(gameItem.LocalSaveFolder);
      this.saveUploadDownloder1.Game = gameItem;
      this.CloseForm = new SimpleSaveUploader.CloseDelegate(this.CloseFormSafe);
      this.Load += new EventHandler(this.SimpleSaveUploader_Load);
      this.saveUploadDownloder1.DownloadFinish += new SaveUploadDownloder.DownloadFinishEventHandler(this.saveUploadDownloder1_DownloadFinish);
      this.saveUploadDownloder1.UploadFinish += new SaveUploadDownloder.UploadFinishEventHandler(this.saveUploadDownloder1_UploadFinish);
      this.saveUploadDownloder1.UploadStart += new SaveUploadDownloder.UploadStartEventHandler(this.saveUploadDownloder1_UploadStart);
      this.saveUploadDownloder1.DownloadStart += new SaveUploadDownloder.DownloadStartEventHandler(this.saveUploadDownloder1_DownloadStart);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private List<string> PrepareZipFile(List<string> files)
    {
      List<string> stringList = new List<string>();
      List<string> containerFiles = this.m_game.GetContainerFiles();
      string file = this.m_game.LocalSaveFolder.Substring(0, this.m_game.LocalSaveFolder.Length - 4);
      string hash = Util.GetHash(file);
      bool cache = Util.GetCache(hash);
      string contents = this.m_game.ToString(true, files);
      if (cache)
      {
        containerFiles.Remove(file);
        contents = contents.Replace("<name>" + Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder) + "</name>", "<name>" + Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder) + "</name><md5>" + hash + "</md5>");
      }
      stringList.AddRange((IEnumerable<string>) containerFiles);
      string path = Path.Combine(Util.GetTempFolder(), "ps4_list.xml");
      File.WriteAllText(path, contents);
      stringList.Add(path);
      ZipUtil.GetAsZipFile(containerFiles.ToArray(), (ZipUtil.OnZipProgress) null);
      return stringList;
    }

    private void saveUploadDownloder1_DownloadStart(object sender, EventArgs e) => this.saveUploadDownloder1.SetStatus(PS3SaveEditor.Resources.Resources.msgDownloadPatch);

    private void saveUploadDownloder1_UploadStart(object sender, EventArgs e) => this.saveUploadDownloder1.SetStatus(PS3SaveEditor.Resources.Resources.msgUploadPatch);

    private void saveUploadDownloder1_UploadFinish(object sender, EventArgs e) => this.saveUploadDownloder1.SetStatus(PS3SaveEditor.Resources.Resources.msgWait);

    private void saveUploadDownloder1_DownloadFinish(object sender, DownloadFinishEventArgs e)
    {
      if (!e.Status && e.Error != "Abort" && !string.IsNullOrEmpty(e.Error))
      {
        int num = (int) Util.ShowMessage(e.Error, PS3SaveEditor.Resources.Resources.msgError);
      }
      this.CloseThis(e.Status);
    }

    private void SimpleSaveUploader_Load(object sender, EventArgs e) => this.saveUploadDownloder1.Start();

    private void CloseThis(bool status)
    {
      if (this.IsDisposed)
        return;
      this.Invoke((Delegate) this.CloseForm, (object) status);
    }

    private void CloseFormSafe(bool bStatus)
    {
      if (bStatus)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Abort;
      this.appClosing = true;
      this.Close();
    }

    private void SimpleSaveUploader_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.appClosing && e.CloseReason == CloseReason.UserClosing && this.DialogResult != DialogResult.OK)
      {
        if (Util.ShowMessage("Are you sure you want to abort?", PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.saveUploadDownloder1.AbortEvent.Set();
          this.DialogResult = DialogResult.Abort;
          this.appClosing = true;
          e.Cancel = true;
          return;
        }
      }
      if (this.appClosing)
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.saveUploadDownloder1 = new SaveUploadDownloder();
      this.SuspendLayout();
      this.saveUploadDownloder1.Action = (string) null;
      this.saveUploadDownloder1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.saveUploadDownloder1.BackColor = Color.FromArgb(102, 102, 102);
      this.saveUploadDownloder1.FilePath = (string) null;
      this.saveUploadDownloder1.Files = (string[]) null;
      this.saveUploadDownloder1.Game = (game) null;
      this.saveUploadDownloder1.IsUpload = false;
      this.saveUploadDownloder1.ListResult = (string) null;
      this.saveUploadDownloder1.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(12));
      this.saveUploadDownloder1.Name = "saveUploadDownloder1";
      this.saveUploadDownloder1.OrderedEntries = (List<string>) null;
      this.saveUploadDownloder1.OutputFolder = (string) null;
      this.saveUploadDownloder1.Profile = (string) null;
      this.saveUploadDownloder1.SaveId = (string) null;
      this.saveUploadDownloder1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(472, 175));
      this.Controls.Add((Control) this.saveUploadDownloder1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SimpleSaveUploader);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Simple Save Patcher";
      this.FormClosing += new FormClosingEventHandler(this.SimpleSaveUploader_FormClosing);
      this.ResumeLayout(false);
    }

    private delegate void CloseDelegate(bool bStatus);
  }
}
