using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using NetComm;


namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class Window1 : Window
    {


        FlowDocument mcFlowDoc = new FlowDocument();
        Paragraph para = new Paragraph();
        public static int typesendbyte = 0; //кб/мб/гб/проги
        public string sear = "", osname;
        public static int port = 0;
        NetComm.Host Server;
        public string strName,ipnomer;  //Имя клиента
        public string ID2;
        public byte[] Data2;
        public bool exit = true;

        public Window1()
        {
            InitializeComponent();
            port = MainWindow.port;
            strName = Environment.MachineName;
            
            RTBHandler("Текущий ip-адрес ");
            foreach (System.Net.IPAddress ip1 in System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList)
            {
                ipnomer += ip1.ToString()+" ";
                RTBHandler(ip1.ToString() + Environment.NewLine);

            }
            Server = new NetComm.Host(port); //Initialize the Server object, connection will use the 2020 port number
            Server.StartConnection(); //Starts listening for incoming clients

            Server.onConnection += new NetComm.Host.onConnectionEventHandler(Server_onConnection);
            Server.lostConnection += new NetComm.Host.lostConnectionEventHandler(Server_lostConnection);
            Server.DataReceived += new NetComm.Host.DataReceivedEventHandler(Server_DataReceived);
            string key = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            using (Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key))
            {
                string n1, n2;
                if (regKey != null)
                {
                    try
                    {
                        n1 = regKey.GetValue("ProductName").ToString();
                    }
                    catch
                    {
                        n1 = "не известной операционной системой";
                    }
                    try
                    {
                        n2 = regKey.GetValue("CSDVersion").ToString();
                    }
                    catch { n2 = ""; }
                    try
                    {
                        osname += n1 + " " + n2;
                            //regKey.GetValue("ProductName").ToString()+" "+regKey.GetValue("CSDVersion").ToString();
                    }
                    catch { }
                }
            }
        }



        public void RTBHandler(string message) //основной метод вывода в чат сообщений, надо переделать
        {
            para.Inlines.Add(new Run(message));
            mcFlowDoc.Blocks.Add(para);
            ChatBox.Document = mcFlowDoc;
            //? сообщения в чате, Входящий запрос Запрос Удовлетворен
        }

        private void ChatBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChatBox.ScrollToEnd();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (exit == true) { File.WriteAllText("port.txt", port.ToString(), Encoding.UTF8); Environment.Exit(0); }
        }

        public string SendMsg()
        {
            double vsego = 0;
            double b = 0;
            double free = 0;
            double a = 0;
            string Vol = "";
            double bite1 = 0;
            string bite2 = "";
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            Vol += "Имя сервера " + strName + ", под управлением " + osname  + ", ip-адрес " + ipnomer+ Environment.NewLine;
            switch (typesendbyte)
            {
                case 1: bite1 = 1024; bite2 = " Kb ";

                    foreach (DriveInfo MyDriveInfo in allDrives)
                    {
                        if (MyDriveInfo.IsReady == true)
                        {
                            vsego = MyDriveInfo.TotalSize;
                            b = vsego / bite1;
                            free = MyDriveInfo.AvailableFreeSpace;
                            a = free / bite1; //байты, /1024 килобайты /1024 мегабайты /1024гигабайты
                            //  Vol += MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + Environment.NewLine;
                            Vol += "Свободного места " + MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + "Всего места: " +
       b.ToString("#") + bite2 + Environment.NewLine;
                        }
                    } break;
                case 2: bite1 = 1048576; bite2 = " Mb ";

                    foreach (DriveInfo MyDriveInfo in allDrives)
                    {
                        if (MyDriveInfo.IsReady == true)
                        {
                            vsego = MyDriveInfo.TotalSize;
                            b = vsego / bite1;
                            free = MyDriveInfo.AvailableFreeSpace;
                            a = free / bite1; //байты, /1024 килобайты /1024 мегабайты /1024гигабайты
                            // Vol += MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + Environment.NewLine;
                            Vol += "Свободного места " + MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + "Всего места: " +
b.ToString("#") + bite2 + Environment.NewLine;
                        }
                    } break;
                case 3: bite1 = 1073741824; bite2 = " Gb ";

                    foreach (DriveInfo MyDriveInfo in allDrives)
                    {
                        if (MyDriveInfo.IsReady == true)
                        {
                            vsego = MyDriveInfo.TotalSize;
                            b = vsego / bite1;
                            free = MyDriveInfo.AvailableFreeSpace;
                            a = free / bite1; //байты, /1024 килобайты /1024 мегабайты /1024гигабайты
                            //Vol += MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + Environment.NewLine;
                            Vol += "Свободного места " + MyDriveInfo.Name + ": " + a.ToString("#") + bite2 + "Всего места: " +
b.ToString("#") + bite2 + Environment.NewLine;
                        }
                    } break;
                case 4:
                    if (sear != "")
                    {
                        string sear2 = "";
                        Microsoft.Win32.RegistryKey key2 =
                            Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
                        //HKEY_CURRENT_USER\\Software\\Microsoft\\Installer\\Products   HKEY_CLASSES_ROOT\\Installer\\Products     SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstal
                        string[] skeys2 = key2.GetSubKeyNames(); // Все подключи из key
                        int length2 = skeys2.Length;
                        // Проход по всем подключам
                        for (int j = 0; j < length2; j++)
                        {
                            // Получаем очередной подключ
                            Microsoft.Win32.RegistryKey appKey2 = key2.OpenSubKey(skeys2[j]);
                            string name;

                            try // Пробуем получить значение DisplayName
                            {
                                name = appKey2.GetValue("DisplayName").ToString() + "\n";
                            }
                            catch (Exception)
                            {
                                // Если не указано имя, то пропускаем ключ
                                continue;
                            }
                            sear2 += name;
                            appKey2.Close();
                        }
                        key2.Close();
                        string tt = sear2.ToLower();
                        string ss = sear.ToLower();
                        while (true)
                        {
                            if (tt.IndexOf(ss) > 1)
                            {
                                Vol = "ПО " + ss + " Установлено на компьютере";
                            }
                            else
                            {
                                Vol = "ПО " + ss + " Не установлено на компьютере";
                            }
                            break;
                        }
                        sear = "";

                        break;
                    }
                    else
                    {
                        break;
                    }
            }
            return Vol;
        }

        public void typeSend()
        {
            //куча ифов
            string typemessage = typesendtext.Text;
            switch (typemessage)
            {
                case "Сообщение 1": typesendbyte = 1; break;
                case "Сообщение 2": typesendbyte = 2; break;
                case "Сообщение 3": typesendbyte = 3; break;
                default: typesendbyte = 4;
                    sear = typemessage; break;
            }
            typesendtext.Clear();
        }

    
        public void Server_DataReceived(string ID, byte[] Data) //сообщения отправ
        {
            
            
                ID2 = ID;
                Data2 = Data;
                // Log.AppendText(ID + ": " + ConvertBytesToString(Data) + Environment.NewLine); //Updates the log when a new message arrived, converting the Data bytes to a string
                // Log.AppendText(ID + ": " + Encoding.GetEncoding(1251).GetString(Data) + Environment.NewLine);
                
                    this.RTBHandler(ID + ": " + Encoding.GetEncoding(1251).GetString(Data) + Environment.NewLine);
               
                typesendtext.Text = Encoding.GetEncoding(1251).GetString(Data);
                typeSend();
                //SendMsg();
                string messagorge = SendMsg();
                foreach (string clientID in Server.Users)
                {
                    //if (ID != clientID)
                    Server.SendData(clientID, Encoding.GetEncoding(1251).GetBytes(messagorge));
                }
            
        }


        public void Server_lostConnection(string id) //потеря конекта
        {
                    RTBHandler(id + " Отключился" + Environment.NewLine);
        }

        public void Server_onConnection(string id) //продключения 
        {
                    RTBHandler(id + " Подключился" + Environment.NewLine);
        }

      

        private void MenuItem_OnClick1(object o, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void MenuItem_OnClick2(object o, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_OnClick3(object o, RoutedEventArgs e)
        {
            Help help = new Help();
            help.start();
        }

        private void MenuItem_OnClick4(object o, RoutedEventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }
    }
}
