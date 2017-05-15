using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

public class Help
{

    protected Process[] prc;
    Process proc = new Process();
    public void start()
    {
        try
        {
            prc = Process.GetProcessesByName("hh");
            int i = 0;
            while (i != prc.Length)
            {
                prc[i].Kill();
                i++;
            }
            proc.StartInfo.FileName = (@"Help.chm");
            proc.StartInfo.Arguments = "";
            proc.Start();
        }
        catch
        {
            MessageBox.Show("Справка по техническим причинам не доступна");
        }
    }

}