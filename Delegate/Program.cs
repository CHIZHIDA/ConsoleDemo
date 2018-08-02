using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Delegate
{
    class Program
    {
        private enum InvokeFunctionType
        {
            invoke = 1,         //同步
            begininvoke1,       //异步委托写法一
            begininvoke2,       //异步委托写法二
            callbackfunction,   //异步回调函数
            threadwait          //多线程等待
        }

        private static void TestThread(string threadName)
        {
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine("thread begin：thread name is:{2},the current thread id is:{0},current time is:{1},",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("HH:mm:ss:fff"), threadName);
            long sum = 0;
            for (int i = 0; i < 99999999; i++)
            {
                sum += i;
            }
            Console.WriteLine("thread   end：thread name is:{2},the current thread id is:{0},current time is:{1},",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("HH:mm:ss:fff"), threadName);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine();

                InvokeFunctionType type = new InvokeFunctionType();
                string[] arrayType = Enum.GetNames(type.GetType());

                for (int i = 0; i < Enum.GetNames(type.GetType()).Length; i++)
                {
                    Console.WriteLine(i + 1 + "." + arrayType[i]);
                }
                Console.WriteLine("0.clear the screen");
                Console.WriteLine("-1.exit");
                Console.Write("Please input invoke function：");

                int itemp = 0;
                string select = Console.ReadLine();
                int.TryParse(select, out itemp);

                switch (itemp)
                {
                    case (int)InvokeFunctionType.invoke:
                        Invoke();
                        break;
                    case (int)InvokeFunctionType.begininvoke1:
                        BeginInvoke1();
                        break;
                    case (int)InvokeFunctionType.begininvoke2:
                        BeginInvoke2();
                        break;
                    case (int)InvokeFunctionType.callbackfunction:
                        AsyncCallbackFunction();
                        break;
                    case (int)InvokeFunctionType.threadwait:
                        ThreadWait();
                        break;
                    case 0:
                        Console.Clear();
                        break;
                    case -1: return;
                    default:
                        Console.WriteLine("input error,please reselect");
                        break;
                }
            }
        }

        /// <summary>
        /// 同步调用(Invoke)
        /// </summary>
        private static void Invoke()
        {
            Action<string> myFunc = TestThread;
            for (int i = 0; i < 5; i++)
            {
                string name = string.Format("Click{0}", i);
                myFunc.Invoke(name);
            }
        }

        /// <summary>
        /// 异步调用(BeginInvoke)写法一
        /// </summary>
        private static void BeginInvoke1()
        {
            //写法一：利用Action<>内置委托，调用的时候赋值
            Action<string> myFunc = TestThread;
            for (int i = 0; i < 5; i++)
            {
                string name = string.Format("Click{0}", i);
                myFunc.BeginInvoke(name, null, null);
            }
        }

        /// <summary>
        /// 异步调用(BeginInvoke)写法二
        /// </summary>
        private static void BeginInvoke2()
        {
            for (int i = 0; i < 5; i++)
            {
                string name = string.Format("Click{0}", i);
                Action myFunc = () =>
                {
                    TestThread(name);
                };
                myFunc.BeginInvoke(null, null);
            }
        }

        private static void AsyncCallbackFunction()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine("----------------- button1_Click begin main thread id is：{0}  --------------------------",
                Thread.CurrentThread.ManagedThreadId);

            Action<string> myFunc = TestThread;
            IAsyncResult asyncResult = null;
            //参数说明：前面几个参数都是方法的参数值，倒数第二个为异步调用的回调函数，倒数第一个为传给回调函数的参数
            for (int i = 0; i < 1; i++)
            {
                string name = string.Format("button1_Click{0}", i);
                asyncResult = myFunc.BeginInvoke(name, t =>
                {
                    Console.WriteLine("i am thread id is {0} callback", Thread.CurrentThread.ManagedThreadId);
                    //用 t.AsyncState 来获取回调传进来的参数
                    Console.WriteLine("the paramater pass in is：{0}", t.AsyncState);

                    //测试一下异步返回值的结果
                    Console.WriteLine("async return values result is：{0}", t.Equals(asyncResult));
                }, "maru");
            }

            //等待的方式1：会有时间上的误差
            //while (!asyncResult.IsCompleted)
            //{
            //    Console.WriteLine("waiting");
            //}

            // 等待的方式二:
            //asyncResult.AsyncWaitHandle.WaitOne();//一直等待
            //asyncResult.AsyncWaitHandle.WaitOne(-1);//一直等待
            //asyncResult.AsyncWaitHandle.WaitOne(1000);//等待1000毫秒，超时就不等待了

            //等待的方式三：
            //myFunc.EndInvoke(asyncResult);

            watch.Stop();
            Console.WriteLine("----------------- button1_Click end main thread id is：{0} the total time taken is：{1}--------------------------",
                Thread.CurrentThread.ManagedThreadId, watch.ElapsedMilliseconds);

        }

        private static void ThreadWait()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine("----------------- button1_Click begin main thread id is：{0}  --------------------------",
                Thread.CurrentThread.ManagedThreadId);

            List<IAsyncResult> list = new List<IAsyncResult>();

            for (int i = 0; i < 5; i++)
            {
                string name = string.Format("button1_Click{0}", i);
                Action myFunc = () =>
                {
                    TestThread(name);
                };
                var asyncResult = myFunc.BeginInvoke(null, null);
                list.Add(asyncResult);
            }

            //下面是线程等待
            foreach (var item in list)
            {
                item.AsyncWaitHandle.WaitOne(-1);
            }

            watch.Stop();
            Console.WriteLine("----------------- button1_Click end main thread id is：{0}  the total time taken is：{1}--------------------------",
                Thread.CurrentThread.ManagedThreadId, watch.ElapsedMilliseconds);
        }
    }
}
