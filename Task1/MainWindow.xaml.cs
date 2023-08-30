using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread thread1;
        private Thread thread2;
        private bool isThread1Paused = false;
        private bool isThread2Paused = false;
        private bool restartThread1 = false;
        private bool restartThread2 = false;
        int clickCount = 0;

        int low;
        int high;
        private Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
        public MainWindow()
        {
            InitializeComponent();
            Numbers.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            Fibonacci.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }
        static bool IsPrime(int number)
        {
            if (number <= 1)
                return false;

            if (number <= 3)
                return true;

            if (number % 2 == 0 || number % 3 == 0)
                return false;

            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }
            return true;
        }
        static List<int> GeneratePrimesInRange(int start, int end)
        {
            List<int> primeNumbers = new List<int>();

            for (int number = start; number <= end; number++)
            {
                if (IsPrime(number))
                {
                    primeNumbers.Add(number);
                }
            }

            return primeNumbers;
        }
        static List<int> GenerateFibonacciInRange(int start, int end)
        {
            List<int> fibonacciNumbers = new List<int>();
            int a = 0, b = 1;

            while (a <= end)
            {
                if (a >= start)
                {
                    fibonacciNumbers.Add(a);
                }

                int next = a + b;
                a = b;
                b = next;
            }

            return fibonacciNumbers;
        }

        private void getRange()
        {
            if (Low.Text == string.Empty) low = 2;
            else low = int.Parse(Low.Text.ToString());
            if (High.Text == string.Empty) high = 100;
            else high = int.Parse(High.Text.ToString());

        }
        private void thread1Run()
        {

            getRange();

            thread1 = new Thread(async () =>
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    List<int> primesInRange = GeneratePrimesInRange(low, high);

                    foreach (int prime in primesInRange)
                    {
                        Numbers.Text += ($"Thread Id{Thread.CurrentThread.GetHashCode()}: {prime}\n");
                        Numbers.ScrollToEnd();
                        await Task.Delay(1000);
                        while (isThread1Paused)
                        {
                            await Task.Delay(100);
                        }
                        if (restartThread1)
                        {
                            restartThread1 = false;
                            break;
                        }
                    }
                 
                });
            });
            threads[thread1.ManagedThreadId] = thread1;
            thread1.Start();
        }

        private void thread2Run()
        {

            getRange();
           
               
            thread2 = new Thread(async () =>
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    List<int> fibonacciNumbers = GenerateFibonacciInRange(low, high);

                    foreach (int number in fibonacciNumbers)
                    {
                        Fibonacci.Text += ($"Thread Id{Thread.CurrentThread.GetHashCode()}: {number}\n");
                        Fibonacci.ScrollToEnd();
                        await Task.Delay(1000);
                        while (isThread2Paused)
                        {
                            await Task.Delay(100);
                        }
                        if (restartThread2)
                        {
                            restartThread2 = false;
                            break;
                        }
                    }



                });
            });
            threads[thread2.ManagedThreadId] = thread2;
            thread2.Start();
        }
        private async void GenerateNumbers_Click(object sender, RoutedEventArgs e)
        { 
            Numbers.Text = string.Empty;
            Fibonacci.Text = string.Empty;
           
            if (clickCount > 0)
            {
                
                restartThread1 = true;
                
                restartThread2 = true;

            }
             clickCount++;
            thread1Run();
            thread2Run();
           
        }


        private void GenerateAgain_Click(object sender, RoutedEventArgs e)
        {
           
           
        }
        private void PauseTheard1_Click(object sender, RoutedEventArgs e)
       {
            isThread1Paused = true;
        }

        private void ResumeTheard1_Click(object sender, RoutedEventArgs e)
        {
            isThread1Paused = false;
        }

        private void PauseTheard2_Click(object sender, RoutedEventArgs e)
        {
            isThread2Paused = true;
        }

        private void ResumeTheard2_Click(object sender, RoutedEventArgs e)
        {
            isThread2Paused = false;
        }

       
    }
}

