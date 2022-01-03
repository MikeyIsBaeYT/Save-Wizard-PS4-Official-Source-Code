// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SingleInstanceApplication
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Microsoft.VisualBasic.ApplicationServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class SingleInstanceApplication : WindowsFormsApplicationBase
  {
    public SingleInstanceApplication(AuthenticationMode mode)
      : base(mode)
    {
      this.InitializeAppProperties();
    }

    public SingleInstanceApplication() => this.InitializeAppProperties();

    protected virtual void InitializeAppProperties()
    {
      this.IsSingleInstance = true;
      this.EnableVisualStyles = true;
    }

    public virtual void Run(Form mainForm)
    {
      this.MainForm = mainForm;
      this.Run(this.CommandLineArgs);
    }

    private void Run(ReadOnlyCollection<string> commandLineArgs) => this.Run((string[]) new ArrayList((ICollection) commandLineArgs).ToArray(typeof (string)));
  }
}
