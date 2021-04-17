using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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



namespace Multithreading_Sort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int min = 0, max = 100;

        public int[] array;
         
        


        public MainWindow()
        {
            InitializeComponent();
            Sort.Click += Sort_Click;
            Generation.Click += Generation_Click;

        }

      

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            //Окно для отсортированного массива очищаем
            SortedArray.Text = null;

            //Здесь проверяем чекбокс критической секции
            bool locking = (bool) Locker.IsChecked;
            //Создаём семафор для синхронизации начала работы двух потоков, и пару семафоров для ожидания завершения каждого потока
            //Изначально свободных мест - 0, а всего может быть - 2
            Semaphore starter = new Semaphore(0, 2);
            Semaphore bubble_output, selection_max_output;
            //Здесь изначально - 0, всего соответственно по одному месту
            bubble_output = new Semaphore(0, 1);
            selection_max_output = new Semaphore(0, 1);

            //Создадим объект нашего класса многопоточного сортировщика
            MThread_Sorter Sorter = new MThread_Sorter();
            //Здесь создаём потоки и отдаём им функции которые они должны выполнить, через лямбды о которых я рассказывал
            Thread Bubble_Sort_Thread = new Thread(() => Sorter.Bubble_Sort(array, starter, bubble_output, locking));
            Thread Max_Sort_Thread = new Thread(() => Sorter.Selection_Max_Sort(array, starter, selection_max_output, locking));
            
            //Запускаем их
            Max_Sort_Thread.Start();
            Bubble_Sort_Thread.Start();


            //starter.Release(2);
            //Здесь ожидаем оба потока, прежде чем выводить результат
            bubble_output.WaitOne();
            selection_max_output.WaitOne();
           

            

            for (int i = 0; i < array.Length; i++)
            {
                SortedArray.Text += array[i] + " ";
            }

        }

       

        private void Generation_Click(object sender, RoutedEventArgs e)
        {
            GeneratedArray.Text = null;

            Random random = new Random();
            array = new int[Convert.ToInt32(Number.Text)];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(min, max);
                GeneratedArray.Text += array[i] + " ";
            }
        }

        
        
       

       
    }
}
