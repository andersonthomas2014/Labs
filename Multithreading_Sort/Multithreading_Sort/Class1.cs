using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Multithreading_Sort
{
    class MThread_Sorter
    {
        private int[] array;
        private Semaphore starter, output;
        private bool locked;
        private object locker = new object();
        public MThread_Sorter()
        {

            
        }
        public MThread_Sorter(int[] array, Semaphore starter, Semaphore output, bool locked)
        {
            this.array = array;
            this.starter = starter; this.output = output;
            this.locked = locked;
        }
        
       
        
        

        private void Swap(ref int e1, ref int e2)
        {
            int temp = e1;
            e1 = e2;
            e2 = temp;
        }
       //Пузырьковая сортировка
        public void Bubble_Sort()
        {
            //Этот поток запускается в основном потоке вторым, и освобождает место в семафоре первому
            starter.Release();
            
            int len = array.Length;
            for (int i = 1; i < len; i++)
            {
                for (int j = 0; j < len - i; j++)
                {
                    //Критическая секция сработает только при установленном чекбоксе
                    if (locked)
                    {
                        //Критическая секция
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
            //Синхронизация вывода
            output.Release();
            return;
            
        }
        //Сортировка выбором максимума
        public void Selection_Max_Sort(int[] array, Semaphore starter, Semaphore output, bool locked)
        {
            //Этот поток запускается первым и ожидает пока запустится второй
            starter.WaitOne();

            int max, temp;
            int length = array.Length;

            for (int i = 0; i < length - 1; i++)
            {
                max = i;
                //Здесь - начало критической секции
                lock (locker)
                {
                    for (int j = 0; j < length - i; j++)
                    {


                        if (array[j] > array[max])
                        {

                            max = j;
                        }


                    }
                    //В момент между принятием решения (поиском max) и выполнением замены(Swap), можно прийти к тому
                    //Что первый метод отработает с теми же аргументами, которые должен поменять второй
                    //И при выполнении замены во втором, данные уже будут неактуальны(элемент с индексом max)
                    //Thread.Sleep вносит задержку в работе этого потока, для явного результата поломки одновременной сортировки
                    //(Срабатывает не всегда и только сразу после запуска, из-за высокого быстродействия ЦП и кэш-памяти)
                    Thread.Sleep(2);
                    if (max != i)
                    {
                        Swap(ref array[length - i - 1], ref array[max]);
                    }
                }
            }
            //Синхронизация вывода
            output.Release();
            return;
        }
    }
}
