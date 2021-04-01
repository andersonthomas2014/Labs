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
            SortedArray.Text = null;

            bool locking = (bool) Locker.IsChecked;
            Semaphore starter = new Semaphore(0, 2);
            Semaphore bubble_output, selection_max_output;
            bubble_output = new Semaphore(0, 1);
            selection_max_output = new Semaphore(0, 1);

            MThread_Sorter Sorter = new MThread_Sorter();
            Thread Bubble_Sort_Thread = new Thread(() => Sorter.Bubble_Sort(array, starter, bubble_output, locking));
            Thread Max_Sort_Thread = new Thread(() => Sorter.Selection_Max_Sort(array, starter, selection_max_output, locking));
            
            Max_Sort_Thread.Start();
            Bubble_Sort_Thread.Start();


            //starter.Release(2);
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
