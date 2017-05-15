using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;


// http://www.codeproject.com/Articles/118485/C-VB-NET-Multi-user-Communication-Library-TCP 
// http://translate.googleusercontent.com/translate_c?act=url&depth=1&hl=ru&ie=UTF8&prev=_t&rurl=translate.google.ru&sl=auto&tl=ru&u=http://www.codeproject.com/Articles/118485/C-VB-NET-Multi-user-Communication-Library-TCP&usg=ALkJrhj_5FbR6uUbZDlCw2nE2YBpYu7Hgw

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
   

    public partial class Window2 : Window
    {
         System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
         System.Windows.Threading.DispatcherTimer timer2 = new System.Windows.Threading.DispatcherTimer();
        public int ii = 0,t1,t2,t3,iii=0,oi;
        public string NameC = Environment.MachineName;
        public int cet = 0;
        public string strName; //Имя клиента
        // public bool IsConn;  хз зачем она
        public static string[] ipcolletion = new string[100]; //сколько нужно интересно?)
        public bool cnn = false;
        private FlowDocument mcFlowDoc = new FlowDocument();
        private Paragraph para = new Paragraph();
        public static int port = 0;
        NetComm.Client client; //The client object used for the communication

        public void RTBHandler(string message)
        {
            para.Inlines.Add(new Run(message));
            mcFlowDoc.Blocks.Add(para);
            ChatBox.Document = mcFlowDoc;
        }

        public Window2()
        {
           
           
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);

            timer2.Tick += new EventHandler(TimerTick2);
            timer2.Interval = new TimeSpan(0, 0, 1);

            InitializeComponent();
            client = new NetComm.Client(); //Initialize the client object

            port = MainWindow.port;
            mesto();
 
            client.Connected += new NetComm.Client.ConnectedEventHandler(client_Connected);
            client.Disconnected += new NetComm.Client.DisconnectedEventHandler(client_Disconnected);
            client.DataReceived += new NetComm.Client.DataReceivedEventHandler(client_DataReceived);

            try
            {
                string[] NewFile = File.ReadAllLines("ipadress.txt");
                int Count = 0;
                foreach (string str in NewFile)
                {
                    if (str != "")
                    {
                        ipcolletion[Count] = (str);
                        Count++;
                    }
                }
                runIP();
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки адресов");
            }
        }

     


        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e) //хотелось бы переместить 
        {
            try
            {
                File.WriteAllLines("ipadress.txt", ipcolletion, Encoding.UTF8);
                File.WriteAllText("port.txt", port.ToString(), Encoding.UTF8);
                Environment.Exit(0);
            }
            catch (ObjectDisposedException)
            {
            }
            catch
            {
                Environment.Exit(0);
            }
            Environment.Exit(0);
        }
        
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            
            if (listBox1.SelectedItem != null)
            {
                try
                {
                    client.Connect(listBox1.SelectedItem.ToString(), port, NameC.ToString());
                    cnn = true;
                }
                catch
                {
                    cnn = false;
                    RTBHandler("Ошибка подключения "+Environment.NewLine);
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ip");
            }
        }
        
        private void ChatBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChatBox.ScrollToEnd();
        }

        public void mesto()
        {
            foreach (System.Net.IPAddress ip1 in System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList)
            //ip
            {
               RTBHandler( ip1.ToString() + Environment.NewLine);
            }
            double bite1 = 1073741824;
            string bite2 = " Gb ";
            double free = 0, vsego = 0;
            double a = 0, b = 0;
            string Vol = "";
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo MyDriveInfo in allDrives)
            {
                if (MyDriveInfo.IsReady == true)
                {
                    vsego = MyDriveInfo.TotalSize;
                    free = MyDriveInfo.AvailableFreeSpace;
                    a = free / bite1;
                    b = vsego / bite1;
                    // a = ((free / 1024) / 1024) / 1024; //байты, /1024 килобайты /1024 мегабайты /1024гигабайты
                    Vol += "Свободного места " + MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + "Всего места: " +
                           b.ToString("#") + bite2 + Environment.NewLine;
                }
            }
            RTBHandler( Vol.ToString() + "\n");
        }

        private void CloseConnect(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client.isConnected) client.Disconnect();
            }
            catch
            {

                MessageBox.Show("Ошибка отключения ");
            }
        }

        public void runIP()
        {
            for (int i = 0; i < ipcolletion.Length; i++)
            {
                listBox1.Items.Add(ipcolletion[i]);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) // с одим массивом
        {
            if (searchbox.Text != "" && searchbox.Text != " " && searchbox.Text != "  ")
            {
                string tt = "";
                Microsoft.Win32.RegistryKey key =
                    Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                        "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
                //HKEY_CURRENT_USER\\Software\\Microsoft\\Installer\\Products   HKEY_CLASSES_ROOT\\Installer\\Products     SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstal
                string[] skeys = key.GetSubKeyNames(); // Все подключи из key
                int length = skeys.Length;
                // Проход по всем подключам
                for (int i = 0; i < length; i++)
                {
                    // Получаем очередной подключ
                    Microsoft.Win32.RegistryKey appKey = key.OpenSubKey(skeys[i]);
                    string name;

                    try // Пробуем получить значение DisplayName
                    {
                        name = appKey.GetValue("DisplayName").ToString() + "\n";
                    }
                    catch (Exception)
                    {
                        // Если не указано имя, то пропускаем ключ
                        continue;
                    }
                    tt += name;
                    appKey.Close();
                }
                key.Close();
                tt = tt.ToLower();
                string ss = searchbox.Text.ToLower();
                while (true)
                {
                    if (tt.IndexOf(ss) > 1)
                    {
                        MessageBox.Show("ПО " + ss + " Установлено на компьютере"); //coll.Text = "est!";
                    }
                    else
                    {
                        MessageBox.Show("ПО " + ss + " Не установлено на компьютере"); // coll.Text = "not!";
                    }
                    break;
                }
                // RTBHandler(tt);
                searchbox.Clear();
            }
            else
            {
                MessageBox.Show("Введите название программного обеспечения");
            }
        }

        private void gotoform_Click(object sender, RoutedEventArgs e)
        {
            Window3 form2 = new Window3();
            form2.ShowDialog();
            listBox1.Items.Clear();
            for (int i = 0; i < ipcolletion.Length; i++)
            {
                listBox1.Items.Add(ipcolletion[i]);
            }
        }

        void client_DataReceived(byte[] Data, string ID) //обработчик сообщений
        {
            cet++;
                RTBHandler(Encoding.GetEncoding(1251).GetString(Data) + Environment.NewLine);}

        void client_Disconnected() //отключение
        {
            if (cnn == true)
            {
                RTBHandler("Отключен от сервера!" + Environment.NewLine);
                cnn = false;
            }
            
        }

        void client_Connected() // подклюбчение
        {
                RTBHandler("Соединение установлено!" + Environment.NewLine);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string textRange = "0";
            if (msg1.IsChecked == true)
            {
                textRange = "Сообщение 1"; //кб
            }
            if (msg2.IsChecked == true)
            {
                textRange = "Сообщение 2"; //мб 
            }
            if (msg3.IsChecked == true)
            {
                textRange = "Сообщение 3"; //гб
            }
            if (searchbox.Text != "")
            {
                textRange = searchbox.Text; //проги
            }
            client.SendData(Encoding.GetEncoding(1251).GetBytes(textRange));
            searchbox.Clear(); //Clears the chatmessage textbox text
        }



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ii = 0;
            type_timer();

            if (listBox1.SelectedItem != null)
            {
                timer.Start();
                client.Connect(listBox1.SelectedItem.ToString(), port, NameC.ToString());
                cnn = true;
            }
            else
            {
                MessageBox.Show("Вы не выбрали ip");
                timer.Stop();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            ii++;
              if (ii == t1)
              {
                  SendButton_Click(sender,e);
              }
            if (ii == t2)
            {
                timer.Stop();
                CloseConnect(sender, null);
            }
        }
        
        public void type_timer()
        {
            if (time1.IsChecked == true)
            {
                t1 = 2;
                t2 = 4;
            }
            if (time2.IsChecked == true)
            {
                t1 = 3;
                t2 = 6;
            }
            if (time3.IsChecked == true)
            {
                t1 = 5;
                t2 = 10;
            }
        }

        private void ButtonBase_OnClick3(object sender, RoutedEventArgs e)//clear
        {
          mcFlowDoc = new FlowDocument();
          para = new Paragraph(); ChatBox.Document.Blocks.Clear();
        }


        private void but_click_all(object sender, RoutedEventArgs e)
        {
            ii = 0;
            type_timer();

            t3 = t2 + 1;
                timer2.Start();
                give1.IsEnabled = false;
                give2.IsEnabled = false;
                client.Connect(ipcolletion[iii], port, NameC.ToString());
        }

        private void TimerTick2(object sender, EventArgs e)
        {
            ii++;
            if (ii == t1)
            {
                SendButton_Click(sender, e);
            }
            if (ii == t2)
            {
               // timer.Stop();
                CloseConnect(sender, null);
            }
            if (ii == t3|| ii>t3)
            {
                iii++;
                if ( ipcolletion[iii] != null)
                {
                    //RTBHandler("!" + ipcolletion[iii] + Environment.NewLine);
                    try
                    {
                        client.Connect(ipcolletion[iii], port, NameC.ToString()); 
                        ii = 0;
                        cnn = true;
                    }
                    catch 
                    {
                        RTBHandler("Ошибка подключения " + Environment.NewLine);
                        iii++;
                    }
                }
                else
                {
                    while ( ipcolletion[iii] != null)
                    {
                        if (iii == 99)
                        {
                            timer2.Stop();
                            iii = 0;
                            give1.IsEnabled = true;
                            give2.IsEnabled = true;
                            break;
                        }
                        iii++;
                    }
                }
            }
            if (iii == 99)
            {
                timer2.Stop(); give1.IsEnabled = true;
                give2.IsEnabled = true;
                iii = 0;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string tt = "";
            Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            //HKEY_CURRENT_USER\\Software\\Microsoft\\Installer\\Products   HKEY_CLASSES_ROOT\\Installer\\Products     SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstal
            string[] skeys = key.GetSubKeyNames(); // Все подключи из key
            int length = skeys.Length;
            // Проход по всем подключам
            for (int i = 0; i < length; i++)
            {
                // Получаем очередной подключ
                Microsoft.Win32.RegistryKey appKey = key.OpenSubKey(skeys[i]);
                string name;
                try // Пробуем получить значение DisplayName
                {
                    name = appKey.GetValue("DisplayName").ToString() + "\n";
                }
                catch (Exception)
                {
                    // Если не указано имя, то пропускаем ключ
                    continue;
                }
                tt += name;
                appKey.Close();
            }
            key.Close();
            RTBHandler(tt);
        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (tab1.IsSelected == true)
            {
                give1.IsEnabled = true;
                give2.IsEnabled = true;
            }
            
            if (tab2.IsSelected == true)
            {
                give1.IsEnabled = false;
                give2.IsEnabled = false;
            }
            
        }

        private void MenuItem_OnClick1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
            
        }

        private void MenuItem_OnClick2(object sender, RoutedEventArgs e)
        {
           Environment.Exit(0);
        }

        private void MenuItem_OnClick3(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.start();
        }

        private void MenuItem_OnClick4(object sender, RoutedEventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }
    }
}

