using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace СОБЗИК
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4()
        {
            InitializeComponent();
            try
            {
                textport.Text = File.ReadAllText("port.txt");
            }
            catch
            {
            }
        }
        public static int port = 0;

        private void serverform_Click(object sender, RoutedEventArgs e)
        {
            if (textport.Text == "")
            {
                MessageBox.Show("Поле порт является обязательным для заполнения");
            }
            else
            {
                port = Convert.ToInt32(textport.Text);
                Start_window.Visibility = Visibility.Hidden;
                Server_window.Visibility = Visibility.Visible;
            /*    Window4 server = new Window4();
                server.Show();
                this.Hide();*/
            }
        }

        private void clientform_Click(object sender, RoutedEventArgs e)
        {
            if (textport.Text == "")
            {
                MessageBox.Show("Поле порт является обязательным для заполнения");
            }
            else
            {
                port = Convert.ToInt32(textport.Text);
                Start_window.Visibility = Visibility.Hidden;
                Client_window.Visibility = Visibility.Visible;
            /*    Window2 client = new Window2();
                client.Show();
                this.Hide();*/
            }
        }

        private void Window_Closing_1(object sender, CancelEventArgs cancelEventArgs)
        {
            File.WriteAllText("port.txt", textport.Text, Encoding.UTF8);
            Environment.Exit(0);
        }

        private void Window_Closing_2(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("port.txt", textport.Text, Encoding.UTF8);
            Environment.Exit(0);
        }


        private void MenuItem_OnClick2(object sender, RoutedEventArgs e)
        {
            try
            {
                Help help = new Help();
                help.start();
                //Process.Start(@"Help.chm"); 
            }
            catch { }
        }

        private void MenuItem_OnClick3(object sender, RoutedEventArgs e)
        {
            //как будет работать с разных гридов
            Start_window.Visibility = Visibility.Hidden;
            About_window.Visibility = Visibility.Visible;
            /*   AboutBox1 ab = new AboutBox1();
               ab.ShowDialog();*/
        }

        private void textport_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)//разрешенные клавиши
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.Back:
                case Key.Delete:
                    e.Handled = false;
                    break;
                default:
                    e.Handled = true;
                    break;
            }
            string iptime = textport.Text;
            iptime = iptime.Trim().Replace(" ", string.Empty); //убирает все пробелы
            textport.Text = iptime;
            textport.SelectionStart = textport.Text.Length;
        }
    }
}
