// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.GlassPanelForm
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace BrightIdeasSoftware
{
  internal class GlassPanelForm : Form
  {
    internal IOverlay Overlay;
    private ObjectListView objectListView;
    private bool isDuringResizeSequence;
    private bool isGlassShown;
    private bool wasGlassShownBeforeResize;
    private Form myOwner;
    private Form mdiOwner;
    private List<Control> ancestors;
    private MdiClient mdiClient;

    public GlassPanelForm()
    {
      this.Name = nameof (GlassPanelForm);
      this.Text = nameof (GlassPanelForm);
      this.ClientSize = Util.ScaleSize(new Size(0, 0));
      this.ControlBox = false;
      this.FormBorderStyle = FormBorderStyle.None;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.Manual;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.FormBorderStyle = FormBorderStyle.None;
      this.SetStyle(ControlStyles.Selectable, false);
      this.Opacity = 0.5;
      this.BackColor = Color.FromArgb((int) byte.MaxValue, 254, 254, 254);
      this.TransparencyKey = this.BackColor;
      this.HideGlass();
      NativeMethods.ShowWithoutActivate((IWin32Window) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.Unbind();
      base.Dispose(disposing);
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ExStyle |= 32;
        createParams.ExStyle |= 128;
        return createParams;
      }
    }

    public void Bind(ObjectListView olv, IOverlay overlay)
    {
      if (this.objectListView != null)
        this.Unbind();
      this.objectListView = olv;
      this.Overlay = overlay;
      this.mdiClient = (MdiClient) null;
      this.mdiOwner = (Form) null;
      if (this.objectListView == null)
        return;
      this.objectListView.Disposed += new EventHandler(this.objectListView_Disposed);
      this.objectListView.LocationChanged += new EventHandler(this.objectListView_LocationChanged);
      this.objectListView.SizeChanged += new EventHandler(this.objectListView_SizeChanged);
      this.objectListView.VisibleChanged += new EventHandler(this.objectListView_VisibleChanged);
      this.objectListView.ParentChanged += new EventHandler(this.objectListView_ParentChanged);
      if (this.ancestors == null)
        this.ancestors = new List<Control>();
      for (Control parent = this.objectListView.Parent; parent != null; parent = parent.Parent)
        this.ancestors.Add(parent);
      foreach (Control ancestor in this.ancestors)
      {
        ancestor.ParentChanged += new EventHandler(this.objectListView_ParentChanged);
        if (ancestor is TabControl tabControl2)
          tabControl2.Selected += new TabControlEventHandler(this.tabControl_Selected);
      }
      this.Owner = this.objectListView.FindForm();
      this.myOwner = this.Owner;
      if (this.Owner != null)
      {
        this.Owner.LocationChanged += new EventHandler(this.Owner_LocationChanged);
        this.Owner.SizeChanged += new EventHandler(this.Owner_SizeChanged);
        this.Owner.ResizeBegin += new EventHandler(this.Owner_ResizeBegin);
        this.Owner.ResizeEnd += new EventHandler(this.Owner_ResizeEnd);
        if (this.Owner.TopMost)
          NativeMethods.MakeTopMost((IWin32Window) this);
        this.mdiOwner = this.Owner.MdiParent;
        if (this.mdiOwner != null)
        {
          this.mdiOwner.LocationChanged += new EventHandler(this.Owner_LocationChanged);
          this.mdiOwner.SizeChanged += new EventHandler(this.Owner_SizeChanged);
          this.mdiOwner.ResizeBegin += new EventHandler(this.Owner_ResizeBegin);
          this.mdiOwner.ResizeEnd += new EventHandler(this.Owner_ResizeEnd);
          foreach (Control control in (ArrangedElementCollection) this.mdiOwner.Controls)
          {
            this.mdiClient = control as MdiClient;
            if (this.mdiClient != null)
              break;
          }
          if (this.mdiClient != null)
            this.mdiClient.ClientSizeChanged += new EventHandler(this.myMdiClient_ClientSizeChanged);
        }
      }
      this.UpdateTransparency();
    }

    private void myMdiClient_ClientSizeChanged(object sender, EventArgs e)
    {
      this.RecalculateBounds();
      this.Invalidate();
    }

    public void HideGlass()
    {
      if (!this.isGlassShown)
        return;
      this.isGlassShown = false;
      this.Bounds = new Rectangle(-10000, -10000, 1, 1);
    }

    public void ShowGlass()
    {
      if (this.isGlassShown || this.isDuringResizeSequence)
        return;
      this.isGlassShown = true;
      this.RecalculateBounds();
    }

    public void Unbind()
    {
      if (this.objectListView != null)
      {
        this.objectListView.Disposed -= new EventHandler(this.objectListView_Disposed);
        this.objectListView.LocationChanged -= new EventHandler(this.objectListView_LocationChanged);
        this.objectListView.SizeChanged -= new EventHandler(this.objectListView_SizeChanged);
        this.objectListView.VisibleChanged -= new EventHandler(this.objectListView_VisibleChanged);
        this.objectListView.ParentChanged -= new EventHandler(this.objectListView_ParentChanged);
        this.objectListView = (ObjectListView) null;
      }
      if (this.ancestors != null)
      {
        foreach (Control ancestor in this.ancestors)
        {
          ancestor.ParentChanged -= new EventHandler(this.objectListView_ParentChanged);
          if (ancestor is TabControl tabControl4)
            tabControl4.Selected -= new TabControlEventHandler(this.tabControl_Selected);
        }
        this.ancestors = (List<Control>) null;
      }
      if (this.myOwner != null)
      {
        this.myOwner.LocationChanged -= new EventHandler(this.Owner_LocationChanged);
        this.myOwner.SizeChanged -= new EventHandler(this.Owner_SizeChanged);
        this.myOwner.ResizeBegin -= new EventHandler(this.Owner_ResizeBegin);
        this.myOwner.ResizeEnd -= new EventHandler(this.Owner_ResizeEnd);
        this.myOwner = (Form) null;
      }
      if (this.mdiOwner != null)
      {
        this.mdiOwner.LocationChanged -= new EventHandler(this.Owner_LocationChanged);
        this.mdiOwner.SizeChanged -= new EventHandler(this.Owner_SizeChanged);
        this.mdiOwner.ResizeBegin -= new EventHandler(this.Owner_ResizeBegin);
        this.mdiOwner.ResizeEnd -= new EventHandler(this.Owner_ResizeEnd);
        this.mdiOwner = (Form) null;
      }
      if (this.mdiClient == null)
        return;
      this.mdiClient.ClientSizeChanged -= new EventHandler(this.myMdiClient_ClientSizeChanged);
      this.mdiClient = (MdiClient) null;
    }

    private void objectListView_Disposed(object sender, EventArgs e) => this.Unbind();

    private void Owner_ResizeBegin(object sender, EventArgs e)
    {
      this.isDuringResizeSequence = true;
      this.wasGlassShownBeforeResize = this.isGlassShown;
    }

    private void Owner_ResizeEnd(object sender, EventArgs e)
    {
      this.isDuringResizeSequence = false;
      if (!this.wasGlassShownBeforeResize)
        return;
      this.ShowGlass();
    }

    private void Owner_LocationChanged(object sender, EventArgs e)
    {
      if (this.mdiOwner != null)
        this.HideGlass();
      else
        this.RecalculateBounds();
    }

    private void Owner_SizeChanged(object sender, EventArgs e) => this.HideGlass();

    private void objectListView_LocationChanged(object sender, EventArgs e)
    {
      if (!this.isGlassShown)
        return;
      this.RecalculateBounds();
    }

    private void objectListView_SizeChanged(object sender, EventArgs e)
    {
    }

    private void tabControl_Selected(object sender, TabControlEventArgs e) => this.HideGlass();

    private void objectListView_ParentChanged(object sender, EventArgs e)
    {
      ObjectListView objectListView = this.objectListView;
      IOverlay overlay = this.Overlay;
      this.Unbind();
      this.Bind(objectListView, overlay);
    }

    private void objectListView_VisibleChanged(object sender, EventArgs e)
    {
      if (this.objectListView.Visible)
        this.ShowGlass();
      else
        this.HideGlass();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (this.objectListView == null || this.Overlay == null)
        return;
      Graphics graphics = e.Graphics;
      graphics.TextRenderingHint = ObjectListView.TextRenderingHint;
      graphics.SmoothingMode = ObjectListView.SmoothingMode;
      if (this.mdiClient != null)
      {
        Rectangle client = this.objectListView.RectangleToClient(this.mdiClient.RectangleToScreen(this.mdiClient.ClientRectangle));
        graphics.SetClip(client, CombineMode.Intersect);
      }
      this.Overlay.Draw(this.objectListView, graphics, this.objectListView.ClientRectangle);
    }

    protected void RecalculateBounds()
    {
      if (!this.isGlassShown)
        return;
      Rectangle clientRectangle = this.objectListView.ClientRectangle;
      clientRectangle.X = 0;
      clientRectangle.Y = 0;
      this.Bounds = this.objectListView.RectangleToScreen(clientRectangle);
    }

    internal void UpdateTransparency()
    {
      if (!(this.Overlay is ITransparentOverlay overlay))
        this.Opacity = (double) this.objectListView.OverlayTransparency / (double) byte.MaxValue;
      else
        this.Opacity = (double) overlay.Transparency / (double) byte.MaxValue;
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 132)
        m.Result = (IntPtr) -1;
      base.WndProc(ref m);
    }
  }
}
