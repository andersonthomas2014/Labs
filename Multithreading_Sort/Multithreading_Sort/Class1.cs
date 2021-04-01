using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Multithreading_Sort
{
    class MThread_Sorter
    {
        public MThread_Sorter()
        {

            
        }

        
        private object locker = new object();
        
        

        private void Swap(ref int e1, ref int e2)
        {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }
        public void Bubble_Sort(int[] array, Semaphore starter,Semaphore output, bool locked)
        {
            starter.Release();
            
            int len = array.Length;
            for (int i = 1; i < len; i++)
            {
                for (int j = 0; j < len - i; j++)
                {
                    if (locked)
                    {

                        lock (locker)
                        {
                            if (array[j] > array[j + 1])
                            {
                                Swap(ref array[j], ref array[j + 1]);
                            }
                        }
                    }
                    else
                    {
                        if (array[j] > array[j + 1])
                        {
                            
                            Swap(ref array[j], ref array[j + 1]);
                        }
                    }
                }
            }
            output.Release();
            return;
            
        }
        public void Selection_Max_Sort(int[] array, Semaphore starter, Semaphore output, bool locked)
        {
            starter.WaitOne();

            int max, temp;
            int length = array.Length;

            for (int i = 0; i < length - 1; i++)
            {
                max = i;
                lock (locker)
                {
                    for (int j = 0; j < length - i; j++)
                    {


                        if (array[j] > array[max])
                        {

                            max = j;
                        }


                    }

                    Thread.Sleep(2);
                    if (max != i)
                    {
                        temp = array[length - i - 1];
                        array[length - i - 1] = array[max];
                        array[max] = temp;
                    }
                }
            }

            output.Release();
            return;
        }
    }
}
