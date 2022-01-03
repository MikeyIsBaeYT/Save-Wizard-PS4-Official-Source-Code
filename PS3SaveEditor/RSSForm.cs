// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.RSSForm
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Rss;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace PS3SaveEditor
{
  public class RSSForm : Form
  {
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private ListBox lstRSSFeeds;
    private Button btnOk;
    private Panel panel2;
    private LinkLabel lnkTitle;
    private Label lblTitle;
    private HtmlPanel htmlPanel1;

    public RSSForm(RssChannel channel)
    {
      string registryValue = Util.GetRegistryValue("Language");
      if (registryValue != null)
        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(registryValue);
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.BackColor = Color.FromArgb(80, 29, 11);
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lstRSSFeeds.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstRSSFeeds.DrawItem += new DrawItemEventHandler(this.lstRSSFeeds_DrawItem);
      this.CenterToScreen();
      this.Text = Util.PRODUCT_NAME;
      this.Load += new EventHandler(this.RSSForm_Load);
      this.btnOk.Text = PS3SaveEditor.Resources.Resources.btnOK;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.LostFocus += new EventHandler(this.RSSForm_LostFocus);
      this.lstRSSFeeds.SelectedIndexChanged += new EventHandler(this.lstRSSFeeds_SelectedIndexChanged);
      this.lnkTitle.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkTitle_LinkClicked);
      if (Util.CurrentPlatform == Util.Platform.Linux)
        this.htmlPanel1.Scroll += new ScrollEventHandler(this.HtmlPanel_Scroll);
      try
      {
        if (channel.Items.Count > 0)
        {
          this.lstRSSFeeds.DataSource = (object) channel.Items;
          this.lstRSSFeeds.Refresh();
        }
        else
          this.lstRSSFeeds.DataSource = (object) null;
      }
      catch (Exception ex)
      {
      }
    }

    private void lstRSSFeeds_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.FromArgb(0, 175, (int) byte.MaxValue));
        e.Graphics.DrawString(this.lstRSSFeeds.Items[e.Index].ToString(), e.Font, Brushes.White, (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString(this.lstRSSFeeds.Items[e.Index].ToString(), e.Font, Brushes.Black, (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void lnkTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo(this.lnkTitle.Links[0].LinkData as string));

    private void RSSForm_LostFocus(object sender, EventArgs e) => this.Focus();

    private void lstRSSFeeds_SelectedIndexChanged(object sender, EventArgs e)
    {
      RssItem selectedItem = (RssItem) this.lstRSSFeeds.SelectedItem;
      if (selectedItem.Link != (Uri) null)
      {
        this.lnkTitle.Text = selectedItem.Title;
        this.lnkTitle.Links.Clear();
        this.lnkTitle.Links.Add(0, selectedItem.Title.Length, (object) selectedItem.Link.ToString());
        this.lnkTitle.Visible = true;
        this.lblTitle.Visible = false;
      }
      else
      {
        this.lblTitle.Text = selectedItem.Title;
        this.lnkTitle.Visible = false;
        this.lblTitle.Visible = true;
      }
      string str = Util.ScaleSize(14).ToString() + "px";
      this.htmlPanel1.Text = "<style>*{font-family: '" + Util.GetFontFamily() + "'font-size:" + str + ";color:#000;} body{padding:4px;} p,div{margin:0px;}</style><body>" + selectedItem.Description + " </body>";
    }

    private void btnOk_Click(object sender, EventArgs e) => this.Close();

    private void RSSForm_Load(object sender, EventArgs e)
    {
      if (this.lstRSSFeeds.DataSource == null)
      {
        this.Close();
      }
      else
      {
        this.Show();
        if (this.WindowState == FormWindowState.Minimized)
          this.WindowState = FormWindowState.Normal;
        this.Activate();
      }
    }

    private void RSSForm_ResizeEnd(object sender, EventArgs e)
    {
    }

    private async void HtmlPanel_Scroll(object sender, ScrollEventArgs e)
    {
      await Task.Delay(20);
      this.htmlPanel1.ClearSelection();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.lblTitle = new Label();
      this.lnkTitle = new LinkLabel();
      this.btnOk = new Button();
      this.lstRSSFeeds = new ListBox();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.BackColor = Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.lstRSSFeeds);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(604, 420));
      this.panel1.TabIndex = 0;
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.BackColor = Color.White;
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.lblTitle);
      this.panel2.Controls.Add((Control) this.lnkTitle);
      this.panel2.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(97));
      this.panel2.Name = "panel2";
      this.panel2.Size = Util.ScaleSize(new Size(581, 275));
      this.panel2.TabIndex = 2;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(12f), FontStyle.Bold);
      this.lblTitle.ForeColor = Color.Black;
      this.lblTitle.Location = new Point(Util.ScaleSize(9), Util.ScaleSize(10));
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = Util.ScaleSize(new Size(46, 24));
      this.lblTitle.TabIndex = 4;
      this.lblTitle.Text = "      ";
      this.lnkTitle.AutoSize = true;
      this.lnkTitle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(12f), FontStyle.Bold);
      this.lnkTitle.ForeColor = Color.White;
      this.lnkTitle.Location = new Point(Util.ScaleSize(9), Util.ScaleSize(10));
      this.lnkTitle.Name = "lnkTitle";
      this.lnkTitle.Size = Util.ScaleSize(new Size(40, 24));
      this.lnkTitle.TabIndex = 2;
      this.lnkTitle.TabStop = true;
      this.lnkTitle.Text = "     ";
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnOk.Location = new Point(Util.ScaleSize(263), Util.ScaleSize(389));
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = false;
      this.lstRSSFeeds.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lstRSSFeeds.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.lstRSSFeeds.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(9f), FontStyle.Regular);
      this.lstRSSFeeds.FormattingEnabled = true;
      this.lstRSSFeeds.IntegralHeight = false;
      this.lstRSSFeeds.ItemHeight = Util.ScaleSize(16);
      this.lstRSSFeeds.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.lstRSSFeeds.Name = "lstRSSFeeds";
      this.lstRSSFeeds.Size = Util.ScaleSize(new Size(581, 82));
      this.lstRSSFeeds.ScrollAlwaysVisible = true;
      this.lstRSSFeeds.TabIndex = 0;
      this.htmlPanel1 = new HtmlPanel();
      this.htmlPanel1.BackColor = Color.White;
      this.htmlPanel1.Location = new Point(Util.ScaleSize(-1), Util.ScaleSize(40));
      this.htmlPanel1.Name = "htmlPanel1";
      this.htmlPanel1.Text = "<p>Rss Feed</p>";
      this.htmlPanel1.Size = Util.ScaleSize(new Size(581, 234));
      this.panel2.Controls.Add((Control) this.htmlPanel1);
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(624, 442));
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RSSForm);
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = nameof (RSSForm);
      this.ResizeEnd += new EventHandler(this.RSSForm_ResizeEnd);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
