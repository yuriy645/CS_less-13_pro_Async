using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2_string_callbackk
{// Создайте консольное приложение, в котором организуйте асинхронный вызов метода.
 // Используя конструкцию BeginInvoke передайте в поток некоторую информацию(возможно, в
 // формате строки). Организуйте обработку переданных данных в callback методе.
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Старт первичного потока: Id {0}", Thread.CurrentThread.ManagedThreadId);

            Action action = new Action(Method);
            //AsyncCallback callback = new AsyncCallback(CallBack);
            action.BeginInvoke(CallBack, "Hello world!");// запуск Method во вторичном потоке с методом CallBack с принимаемым параметром

            Console.WriteLine("Первичный поток продолжает работать.");
            Console.ReadKey();
        }

        static void Method()
        {
            Console.WriteLine("\nСтарт первичного потока: Id {0}", Thread.CurrentThread.ManagedThreadId);

            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(20);
                Console.Write(".");
            }

            Console.WriteLine("\nМетод во вторичном потоке завершен.\n");
        }

        static void CallBack(IAsyncResult asyncResult)
        {
            Console.WriteLine("\nСтарт Callback метода во вторичном потоке:", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Стока, которую выдал Callback метод: " + asyncResult.AsyncState);
        }
    }
}
