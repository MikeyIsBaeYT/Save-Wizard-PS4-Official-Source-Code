// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CustomMsgBox
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class CustomMsgBox
  {
    public static DialogResult Show(
      string text = "",
      string title = "",
      MessageBoxButtons button = MessageBoxButtons.OK,
      MessageBoxIcon icon = MessageBoxIcon.None,
      MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
    {
      return CustomMsgBox.Show((Form) null, text, title, button, icon, defaultButton);
    }

    public static DialogResult Show(string text) => CustomMsgBox.Show((Form) null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

    public static DialogResult Show(
      Form parent = null,
      string text = "",
      string title = "",
      MessageBoxButtons button = MessageBoxButtons.OK,
      MessageBoxIcon icon = MessageBoxIcon.None,
      MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
    {
      using (Form form = new Form())
      {
        form.Icon = CustomMsgBox.MsgBoxIconToFormIcon(icon);
        form.Font = Util.GetFontForPlatform(form.Font);
        form.Text = title;
        form.FormBorderStyle = FormBorderStyle.FixedSingle;
        if (parent == null)
        {
          form.StartPosition = FormStartPosition.CenterScreen;
        }
        else
        {
          form.StartPosition = FormStartPosition.Manual;
          form.Location = new Point(parent.Location.X + (parent.Width / 2 - Util.ScaleSize(140)), parent.Location.Y + (parent.Height / 2 - Util.ScaleSize(75)));
        }
        form.Width = Util.ScaleSize(280);
        form.MaximizeBox = false;
        form.MinimizeBox = false;
        Label label = new Label();
        label.Text = text;
        label.Width = Util.ScaleSize(250);
        label.Padding = new Padding(Util.ScaleSize(11), Util.ScaleSize(5), Util.ScaleSize(11), Util.ScaleSize(5));
        label.TextAlign = ContentAlignment.MiddleCenter;
        bool flag = Util.ScaleSize(label.PreferredWidth) > Util.ScaleSize(280);
        int num1 = Util.ScaleSize(label.PreferredWidth) / Util.ScaleSize(280);
        label.AutoSize = false;
        int num2 = 0;
        if (flag)
          num2 = Util.ScaleSize(14) * num1;
        label.Height = Util.ScaleSize(50) + num2;
        if (Util.IsUnixOrMacOSX())
          form.Height = Util.ScaleSize(120) + num2;
        else
          form.Height = Util.ScaleSize(150) + num2;
        form.Controls.Add((Control) label);
        switch (button)
        {
          case MessageBoxButtons.OK:
            Button button1 = new Button();
            button1.Text = "OK";
            button1.SetBounds(Util.ScaleSize(100), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button1.DialogResult = DialogResult.OK;
            form.Controls.Add((Control) button1);
            break;
          case MessageBoxButtons.OKCancel:
            Button button2 = new Button();
            Button button3 = new Button();
            button2.Text = "OK";
            button2.SetBounds(Util.ScaleSize(50), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button2.DialogResult = DialogResult.OK;
            button3.Text = "Cancel";
            button3.SetBounds(Util.ScaleSize(155), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(230));
            button3.DialogResult = DialogResult.Cancel;
            if (defaultButton == MessageBoxDefaultButton.Button1)
            {
              form.Controls.Add((Control) button2);
              form.Controls.Add((Control) button3);
              form.AcceptButton = (IButtonControl) button2;
              break;
            }
            form.Controls.Add((Control) button3);
            form.Controls.Add((Control) button2);
            form.AcceptButton = (IButtonControl) button3;
            break;
          case MessageBoxButtons.AbortRetryIgnore:
            Button button4 = new Button();
            Button button5 = new Button();
            Button button6 = new Button();
            button4.Text = "Abort";
            button4.SetBounds(Util.ScaleSize(20), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button4.DialogResult = DialogResult.Yes;
            button5.Text = "Retry";
            button5.SetBounds(Util.ScaleSize(100), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button5.DialogResult = DialogResult.No;
            button6.Text = "Ignore";
            button6.SetBounds(Util.ScaleSize(180), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button6.DialogResult = DialogResult.Cancel;
            switch (defaultButton)
            {
              case MessageBoxDefaultButton.Button1:
                form.Controls.Add((Control) button4);
                form.Controls.Add((Control) button5);
                form.Controls.Add((Control) button6);
                form.AcceptButton = (IButtonControl) button4;
                break;
              case MessageBoxDefaultButton.Button2:
                form.Controls.Add((Control) button5);
                form.Controls.Add((Control) button4);
                form.Controls.Add((Control) button6);
                form.AcceptButton = (IButtonControl) button5;
                break;
              default:
                form.Controls.Add((Control) button6);
                form.Controls.Add((Control) button5);
                form.Controls.Add((Control) button4);
                form.AcceptButton = (IButtonControl) button6;
                break;
            }
            break;
          case MessageBoxButtons.YesNoCancel:
            Button button7 = new Button();
            Button button8 = new Button();
            Button button9 = new Button();
            button7.Text = "Yes";
            button7.SetBounds(Util.ScaleSize(20), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button7.DialogResult = DialogResult.Yes;
            button8.Text = "No";
            button8.SetBounds(Util.ScaleSize(100), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button8.DialogResult = DialogResult.No;
            button9.Text = "Cancel";
            button9.SetBounds(Util.ScaleSize(180), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button9.DialogResult = DialogResult.Cancel;
            switch (defaultButton)
            {
              case MessageBoxDefaultButton.Button1:
                form.Controls.Add((Control) button7);
                form.Controls.Add((Control) button8);
                form.Controls.Add((Control) button9);
                form.AcceptButton = (IButtonControl) button7;
                break;
              case MessageBoxDefaultButton.Button2:
                form.Controls.Add((Control) button8);
                form.Controls.Add((Control) button7);
                form.Controls.Add((Control) button9);
                form.AcceptButton = (IButtonControl) button8;
                break;
              default:
                form.Controls.Add((Control) button7);
                form.Controls.Add((Control) button8);
                form.Controls.Add((Control) button9);
                form.AcceptButton = (IButtonControl) button9;
                break;
            }
            break;
          case MessageBoxButtons.YesNo:
            Button button10 = new Button();
            Button button11 = new Button();
            button10.Text = "Yes";
            button10.SetBounds(Util.ScaleSize(50), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button10.DialogResult = DialogResult.Yes;
            button11.Text = "No";
            button11.SetBounds(Util.ScaleSize(155), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button11.DialogResult = DialogResult.No;
            if (defaultButton == MessageBoxDefaultButton.Button1)
            {
              form.Controls.Add((Control) button10);
              form.Controls.Add((Control) button11);
              form.AcceptButton = (IButtonControl) button10;
              break;
            }
            form.Controls.Add((Control) button11);
            form.Controls.Add((Control) button10);
            form.AcceptButton = (IButtonControl) button11;
            break;
          case MessageBoxButtons.RetryCancel:
            Button button12 = new Button();
            Button button13 = new Button();
            button12.Text = "Retry";
            button12.SetBounds(Util.ScaleSize(50), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button12.DialogResult = DialogResult.Retry;
            button13.Text = "Cancel";
            button13.SetBounds(Util.ScaleSize(155), Util.ScaleSize(60 + num2), Util.ScaleSize(75), Util.ScaleSize(23));
            button13.DialogResult = DialogResult.Cancel;
            if (defaultButton == MessageBoxDefaultButton.Button1)
            {
              form.Controls.Add((Control) button12);
              form.Controls.Add((Control) button13);
              form.AcceptButton = (IButtonControl) button12;
              break;
            }
            form.Controls.Add((Control) button13);
            form.Controls.Add((Control) button12);
            form.AcceptButton = (IButtonControl) button13;
            break;
        }
        return form.ShowDialog();
      }
    }

    private static Icon MsgBoxIconToFormIcon(MessageBoxIcon msgBoxIcon)
    {
      if (msgBoxIcon == MessageBoxIcon.Asterisk)
        return SystemIcons.Asterisk;
      if (msgBoxIcon == MessageBoxIcon.Hand)
        return SystemIcons.Error;
      if (msgBoxIcon == MessageBoxIcon.Exclamation)
        return SystemIcons.Exclamation;
      if (msgBoxIcon == MessageBoxIcon.Hand)
        return SystemIcons.Hand;
      if (msgBoxIcon == MessageBoxIcon.Asterisk)
        return SystemIcons.Information;
      if (msgBoxIcon == MessageBoxIcon.Question)
        return SystemIcons.Question;
      if (msgBoxIcon == MessageBoxIcon.Hand)
        return SystemIcons.Shield;
      return msgBoxIcon == MessageBoxIcon.Exclamation ? SystemIcons.Warning : (Icon) null;
    }
  }
}
