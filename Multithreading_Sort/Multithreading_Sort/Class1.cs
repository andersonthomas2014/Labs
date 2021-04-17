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

        //создадим пустой объект, который зачем-то нужен для критической секции
        private object locker = new object();
        
        
        //Это банальная функция, которая меняет пару элементов массива местами
        private void Swap(ref int e1, ref int e2)
        {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }
        /*Пузырьковая сортировка, как ты могла догадаться, принимает массив, 
        тот самый семафор для синхронизации старта и семафор для синхронизации вывода, значение того самого чекбокса*/
        public void Bubble_Sort(int[] array, Semaphore starter,Semaphore output, bool locked)
        {
            /*Этот поток запускается вторым, можешь глянуть в MainWindow, 
            поэтому он освобождает место для первого ждущего и начинает работу*/
            starter.Release();
            
            //Здесь ничего интересного, кроме критической секции, просто пузырьковая сортировка
            int len = array.Length;
            for (int i = 1; i < len; i++)
            {
                //Перебираем все элементы и если очередной элемент больше следующего, меняем их местами
                for (int j = 0; j < len - i; j++)
                {
                    //А вот здесь проверяем чекбокс, включена критическая секция или нет
                    if (locked)
                    {
                        /* раз включена, то вот следующий if внутри lock, становится критической операцией
                         То есть операционная система даже если очень захочет, не сможет взять и переключиться на тот поток в этой области
                        Зачем? А представь, что он наткнулся на элемент к-ый > следующего, но ОСь возьмёт и переключится на другой поток
                        Перед тем, как вызвать Swap, то есть поменять местами
                        В том потоке эти элементы уже как-нибудь изменят своё место, а потом ОСь снова вернётся сюда, 
                        и поменяет их ещё разок, когда это будет уже неактуально*/
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
            // Здесь освобождается один из семафоров для синхронизации вывода
            output.Release();
            return;
            
        }
        public void Selection_Max_Sort(int[] array, Semaphore starter, Semaphore output, bool locked)
        {
            //Аналогично
            starter.WaitOne();

            int max, temp;
            int length = array.Length;
            /*Тоже ниче интересного, сортировка выбором максимума, ищет максимум - кидает в конец,
            конец соответственно каждый раз сдвигается ближе к началу*/
            for (int i = 0; i < length - 1; i++)
            {
                max = i;
                /*Здесь тоже проверяется чекбокс if(locked), и добавлен Thread.Sleep(2), который оставляет спать этот поток на 2 мс
                 ОСь переключается на другой; Главная фишка здесь в том, чтобы не разорвать момент принятия решения, то есть здесь нахождение максимума
                В пузырьковой это просто нахождение элемента больше следующего, и момента действия, т.е. перемены местами
                 */
                if (locked) {
                    lock (locker)
                    {
                        //Поиск максимума
                        for (int j = 0; j < length - i; j++)
                        {


                            if (array[j] > array[max])
                            {

                                max = j;
                            }


                        }

                        Thread.Sleep(2);
                        //Меняются местами
                        if (max != i)
                        {
                            temp = array[length - i - 1];
                            array[length - i - 1] = array[max];
                            array[max] = temp;
                        }
                    }
                } else {
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
