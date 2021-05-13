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
    //Задача: 1.Отсортировать массив двумя разными методами одновременно и постараться поломать сортировку, затем восстановить используя критическую секцию
    //        2.Синхронизировать одновременное начало работы методов и вывод результата с помощью семафоров 
    public partial class MainWindow : Window
    {
        const int max = 100;

        private int[] array;
         
        


        public MainWindow()
        {
            InitializeComponent();
            Sort.Click += Sort_Click;
            Generation.Click += Generation_Click;

        }

      

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            SortedArray.Text = null;
            //Переменная, активирующая критическую секцию по чекбоксу
            bool locking = (bool) Locker.IsChecked;
            //Первый семафор - синхронизация одновременной работы, следующие два - синхронизация вывода
            Semaphore starter = new Semaphore(0, 1);
            Semaphore bubble_output, selection_max_output;
       
            bubble_output = new Semaphore(0, 1);
            selection_max_output = new Semaphore(0, 1);

            //Класс-сортировщик, в котором инкапсулированы параметры для многопоточной сортировки
            MThread_Sorter Sorter = new MThread_Sorter(array, starter, bubble_output, locking);
            //Два новых потока, первый принимает функцию и использует параметры, инкапсулированные в класс
            //Второй принимает функцию и параметры внутри анонимного метода, через лямбда-выражение
            Thread Bubble_Sort_Thread = new Thread(Sorter.Bubble_Sort);
            Thread Max_Sort_Thread = new Thread(() => Sorter.Selection_Max_Sort(array, starter, selection_max_output, locking));
            

            Max_Sort_Thread.Start();
            Bubble_Sort_Thread.Start();


            //Ожидание завершения выполнения обоих потоков
            bubble_output.WaitOne();
            selection_max_output.WaitOne();
           

            
            //Вывод отсортированного массива
            for (int i = 0; i < array.Length; i++)
            {
                SortedArray.Text += array[i] + " ";
            }

        }

       
        //Генерация значений массива
        private void Generation_Click(object sender, RoutedEventArgs e)
        {
            GeneratedArray.Text = null;

            Random random = new Random();
            array = new int[Convert.ToInt32(Number.Text)];
            
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(max);
                GeneratedArray.Text += array[i] + " ";
            }
        }

        
        
       

       
    }
}
