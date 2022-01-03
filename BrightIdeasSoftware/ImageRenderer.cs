// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ImageRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ImageRenderer : BaseRenderer
  {
    private bool isPaused = true;
    private System.Threading.Timer tickler;
    private Stopwatch stopwatch;

    public ImageRenderer() => this.stopwatch = new Stopwatch();

    public ImageRenderer(bool startAnimations)
      : this()
    {
      this.Paused = !startAnimations;
    }

    protected override void Dispose(bool disposing)
    {
      this.Paused = true;
      base.Dispose(disposing);
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Paused
    {
      get => this.isPaused;
      set
      {
        if (this.isPaused == value)
          return;
        this.isPaused = value;
        if (this.isPaused)
        {
          this.StopTickler();
          this.stopwatch.Stop();
        }
        else
        {
          this.Tickler.Change(1, -1);
          this.stopwatch.Start();
        }
      }
    }

    private void StopTickler()
    {
      if (this.tickler == null)
        return;
      this.tickler.Dispose();
      this.tickler = (System.Threading.Timer) null;
    }

    protected System.Threading.Timer Tickler
    {
      get
      {
        if (this.tickler == null)
          this.tickler = new System.Threading.Timer(new TimerCallback(this.OnTimer), (object) null, -1, -1);
        return this.tickler;
      }
    }

    public void Pause() => this.Paused = true;

    public void Unpause() => this.Paused = false;

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      if (this.Aspect == null || this.Aspect == DBNull.Value)
        return;
      r = this.ApplyCellPadding(r);
      if (this.Aspect is byte[])
        this.DrawAlignedImage(g, r, this.GetImageFromAspect());
      else if (!(this.Aspect is ICollection aspect2))
        this.DrawAlignedImage(g, r, this.GetImageFromAspect());
      else
        this.DrawImages(g, r, aspect2);
    }

    protected Image GetImageFromAspect()
    {
      if (this.OLVSubItem != null && this.OLVSubItem.ImageSelector is Image)
        return this.OLVSubItem.AnimationState == null ? (Image) this.OLVSubItem.ImageSelector : this.OLVSubItem.AnimationState.image;
      Image image = (Image) null;
      if (this.Aspect is byte[])
      {
        using (MemoryStream memoryStream = new MemoryStream((byte[]) this.Aspect))
        {
          try
          {
            image = Image.FromStream((Stream) memoryStream);
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      else if (this.Aspect is int)
      {
        image = this.GetImage(this.Aspect);
      }
      else
      {
        string aspect = this.Aspect as string;
        if (!string.IsNullOrEmpty(aspect))
        {
          try
          {
            image = Image.FromFile(aspect);
          }
          catch (FileNotFoundException ex)
          {
            image = this.GetImage(this.Aspect);
          }
          catch (OutOfMemoryException ex)
          {
            image = this.GetImage(this.Aspect);
          }
        }
      }
      if (this.OLVSubItem != null && ImageRenderer.AnimationState.IsAnimation(image))
        this.OLVSubItem.AnimationState = new ImageRenderer.AnimationState(image);
      if (this.OLVSubItem != null)
        this.OLVSubItem.ImageSelector = (object) image;
      return image;
    }

    public void OnTimer(object state)
    {
      if (this.ListView == null || this.Paused)
        return;
      if (this.ListView.InvokeRequired)
        this.ListView.Invoke((Delegate) (() => this.OnTimer(state)));
      else
        this.OnTimerInThread();
    }

    protected void OnTimerInThread()
    {
      if (this.ListView == null || this.Paused || this.ListView.IsDisposed)
        return;
      if (this.ListView.View != View.Details || this.Column == null || this.Column.Index < 0)
      {
        this.Tickler.Change(1000, -1);
      }
      else
      {
        long elapsedMilliseconds = this.stopwatch.ElapsedMilliseconds;
        int index1 = this.Column.Index;
        long val1 = elapsedMilliseconds + 1000L;
        Rectangle rectangle = new Rectangle();
        for (int index2 = 0; index2 < this.ListView.GetItemCount(); ++index2)
        {
          OLVListSubItem subItem = this.ListView.GetItem(index2).GetSubItem(index1);
          ImageRenderer.AnimationState animationState = subItem.AnimationState;
          if (animationState != null && animationState.IsValid)
          {
            if (elapsedMilliseconds >= animationState.currentFrameExpiresAt)
            {
              animationState.AdvanceFrame(elapsedMilliseconds);
              rectangle = !rectangle.IsEmpty ? Rectangle.Union(rectangle, subItem.Bounds) : subItem.Bounds;
            }
            val1 = Math.Min(val1, animationState.currentFrameExpiresAt);
          }
        }
        if (!rectangle.IsEmpty)
          this.ListView.Invalidate(rectangle);
        this.Tickler.Change(val1 - elapsedMilliseconds, -1L);
      }
    }

    internal class AnimationState
    {
      private const int PropertyTagTypeShort = 3;
      private const int PropertyTagTypeLong = 4;
      private const int PropertyTagFrameDelay = 20736;
      private const int PropertyTagLoopCount = 20737;
      internal int currentFrame;
      internal long currentFrameExpiresAt;
      internal Image image;
      internal List<int> imageDuration;
      internal int frameCount;

      public static bool IsAnimation(Image image) => image != null && new List<Guid>((IEnumerable<Guid>) image.FrameDimensionsList).Contains(FrameDimension.Time.Guid);

      public AnimationState() => this.imageDuration = new List<int>();

      public AnimationState(Image image)
        : this()
      {
        if (!ImageRenderer.AnimationState.IsAnimation(image))
          return;
        this.image = image;
        this.frameCount = this.image.GetFrameCount(FrameDimension.Time);
        foreach (PropertyItem propertyItem in this.image.PropertyItems)
        {
          if (propertyItem.Id == 20736)
          {
            for (int index = 0; index < propertyItem.Len; index += 4)
              this.imageDuration.Add((((int) propertyItem.Value[index + 3] << 24) + ((int) propertyItem.Value[index + 2] << 16) + ((int) propertyItem.Value[index + 1] << 8) + (int) propertyItem.Value[index]) * 10);
            break;
          }
        }
      }

      public bool IsValid => this.image != null && this.frameCount > 0;

      public void AdvanceFrame(long millisecondsNow)
      {
        this.currentFrame = (this.currentFrame + 1) % this.frameCount;
        this.currentFrameExpiresAt = millisecondsNow + (long) this.imageDuration[this.currentFrame];
        this.image.SelectActiveFrame(FrameDimension.Time, this.currentFrame);
      }
    }
  }
}
