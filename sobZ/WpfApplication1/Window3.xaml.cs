using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public static string[] ipcol = new string[100];

        public Window3()
        {
            InitializeComponent();
        }

         

        private void ydalit_Click(object sender, RoutedEventArgs e)//удаление выбранного элемента
        {
            if (listb2.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент");
            }
            else
            {
                DialogResult result = (DialogResult)MessageBox.Show(
                    "Вы действительно хотите удалить " + listb2.SelectedItem.ToString() + " из списка?", "Удаление элемента", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {

                    for (int i = 0; i < ipcol.Length; i++)
                    {
                        if (ipcol[i] == listb2.SelectedItem.ToString()) // toString зачем то требует 
                        {
                            ipcol[i] = null;
                            break;
                        }
                    }
                } listb2.Items.RemoveAt(listb2.SelectedIndex);
            }
        }

        private void dobavit_Click(object sender, RoutedEventArgs e) //добавление вписанного элемента
        {
            string ipadress = newIPbox.Text;
            ipadress = ipadress.Replace("  ", string.Empty);
            ipadress = ipadress.Trim().Replace(" ", string.Empty);
            ipadress = ipadress.Replace(",", ".");
            if (ipadress != "")
            {
                listb2.Items.Add(ipadress);
                for (int i = 0; i < ipcol.Length; i++)
                {
                    if (ipcol[i] == null)
                    {
                        ipcol[i] = ipadress;
                        break;
                    }
                }
                newIPbox.Clear();
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)//загрузка элементов из главной формы
        {
            ipcol = Window2.ipcolletion;
            for (int i = 0; i < ipcol.Length; i++)
            {
                listb2.Items.Add(ipcol[i]);
            }
        }

        private void Grid_Unloaded_1(object sender, RoutedEventArgs e)//выгрузка элементов в главную форму
        {
            Window2.ipcolletion = ipcol;
        }
    }
}
