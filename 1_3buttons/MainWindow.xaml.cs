using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1_3buttons
{//Создайте приложение WindowsForms. На главной форме приложения разместите 3 кнопки с
//названиями: IsComplete, End, Callback.Организуйте обработчики нажатия на кнопки таким
//образом, чтобы они инициировали асинхронное выполнение некоторого метода(метод
//определите сами, можно воспользоваться чем-то вроде Add или более абстрактного Compute).
//Для каждой из кнопок завершение асинхронного метода должно отслеживаться
//соответствующим образом:

// IsComplete – с использованием значения свойства IsComplete
// End – просто применяя EndInvoke
// Callback – с использованием callback метода
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public double Add(double x, double y)
        {
            Thread.Sleep(4000);
            return x + y;
        }
        bool CheckInput(out double x, out double y)
        {
            bool xChecked = double.TryParse(textBox1.Text, out x);
            bool yChecked = double.TryParse(textBox2.Text, out y);
            
            if (xChecked && yChecked)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Аргументы должны быть числами");
                return false;
            }
        }

        private void IsCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInput(out double x, out double y))
            {
                var myDel = new Func<double, double, double>(Add);               //прицепили к делегату метод Add
                IAsyncResult asyncResult = myDel.BeginInvoke(x, y, null, null); // передали входные параметры в Add и запустили во вторичном потоке
                while (!asyncResult.IsCompleted)
                {
                    Thread.Sleep(100); //имитация деятельности основного потока
                }

                MessageBox.Show("Асинхронно запущщеный метод Add закончил работу в своём вторичном потоке, смотрите результат в 3-м поле");

                textBox3.Text = myDel.EndInvoke(asyncResult).ToString(); // только получение результата вторичного потока

            }

        }

        private void EndInvoke_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInput(out double x, out double y))
            {

                var myDel = new Func<double, double, double>(Add);
                IAsyncResult asyncResult = myDel.BeginInvoke(x, y, null, null);

                textBox3.Text = myDel.EndInvoke(asyncResult).ToString(); // здесь .EndInvoke
                                                                         // 1) остановило основной поток с помощью спрятанного WaitOne() 
                                                                         // 2) подождало завершения вторичного потока
                                                                         // 3) "разбудило" основной поток

                MessageBox.Show("Асинхронно запущщеный метод Add закончил работу в своём вторичном потоке, смотрите результат в 3-м поле");

            }
        }

        SynchronizationContext sync;
        private void Callback_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInput(out double x, out double y))
            {
                var function = new Func<double, double, double>(Add);

                sync = SynchronizationContext.Current; //получаем контекст синхронизации для текущего потока

               // IAsyncResult func =
                 function.BeginInvoke(x, y, CallBack, null);// асинхронный запуск Add и CallBack 

                MessageBox.Show("Асинхронный метод запущен");
            } 
        }
        public void CallBack(IAsyncResult asyncResult)
        {
            AsyncResult async = asyncResult as AsyncResult; // входное значение безопасно приводим к производному типу(даункаст)
            // а вот в AsyncResult есть object AsyncDelegate, в котором хранится ссылка на экземпляр делегата function

            var delegateAdd = (Func<double, double, double>)async.AsyncDelegate;

            var result = delegateAdd.EndInvoke(asyncResult); // это всеравно, что вызвали бы function.EndInvoke(asyncResult) в Callback_Click 
                                                             // получение результата Add в CallBack методе

            //MessageBox.Show(result.ToString());
            //TextBox3.Text = result.ToString();
            sync.Post(delegate { textBox3.Text = result.ToString(); }, null);

            MessageBox.Show(result.ToString());
        }

    }
}
