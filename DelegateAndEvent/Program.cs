using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DelegateAndEvent
{
    public delegate void GreetingDelegate(string name);

    public class GreetingManage
    {
        /// <summary>
        /// 在GreetingMange内部声明委托变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GreetingDelegate gmGreetingDelegate;

        //如果GreetingMange类中定义的委托。使用private的话没有意义，使用public的话可以随意进行
        //赋值操作，言重破坏了对象的封装性，所以这时就是Event的出场时机了。
        public event GreetingDelegate MakeGreet;

        public void GreetPeople(string name, GreetingDelegate MarkGreeting)
        {
            MarkGreeting(name);
        }

        public void DelegateGreetPeople(string name)
        {
            if (gmGreetingDelegate != null)
            {
                gmGreetingDelegate(name);
            }
        }
    }

    class Program
    {
        #region 使用枚举判断调用对应的方法
        public enum Language
        {
            Chinese, English
        }

        private static void EnumGreetPeople(string name, Language lang)
        {
            switch (lang)
            {
                case Language.Chinese:
                    ChineseGreeting(name);
                    break;
                case Language.English:
                    EnglishGreeting(name);
                    break;
            }
        }
        #endregion

        private static void GreetPeopple(string name, GreetingDelegate MakeGreeting)
        {
            MakeGreeting(name);
        }

        private static void EnglishGreeting(string name)
        {
            Console.WriteLine("Hello " + name);
        }

        private static void ChineseGreeting(string name)
        {
            Console.WriteLine("您好 " + name);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("枚举：");
            EnumGreetPeople("枚举", Language.Chinese);
            EnumGreetPeople("Enum", Language.English);
            Console.WriteLine();

            Console.WriteLine("委托：");
            GreetingDelegate delegate1, delegate2;
            delegate1 = ChineseGreeting;
            delegate2 = EnglishGreeting;
            GreetPeopple("委托", delegate1);
            GreetPeopple("delegate", delegate2);
            Console.WriteLine();

            Console.WriteLine("委托绑定一：");
            GreetingDelegate delegate3;
            delegate3 = ChineseGreeting;
            delegate3 += EnglishGreeting;
            GreetPeopple("binding", delegate3);
            Console.WriteLine();

            Console.WriteLine("委托绑定二：");
            delegate3("bundingtwo");
            Console.WriteLine();

            Console.WriteLine("委托解绑：");
            delegate3 -= EnglishGreeting;
            delegate3("unbunding");
            Console.WriteLine();

            Console.WriteLine("新类：");
            GreetingManage gm = new GreetingManage();
            gm.GreetPeople("gm类", ChineseGreeting);
            gm.GreetPeople("classgm", EnglishGreeting);
            Console.WriteLine();

            Console.WriteLine("委托新类：");
            GreetingManage gm2 = new GreetingManage();
            GreetingDelegate delegate4;
            delegate4 = ChineseGreeting;
            delegate4 += EnglishGreeting;
            gm2.GreetPeople("gmclasstwo", delegate4);
            Console.WriteLine();

            Console.WriteLine("新类定义的委托：");
            GreetingManage gm3 = new GreetingManage();
            gm3.gmGreetingDelegate = ChineseGreeting;
            gm3.gmGreetingDelegate += EnglishGreeting;
            gm3.gmGreetingDelegate("gmdelegate");
            Console.WriteLine();

            Console.WriteLine("模拟烧水过程，Observer设计模式：");
            Console.WriteLine();
            Heater heater = new Heater();
            Alarm alarm = new Alarm();
            Display display = new Display();
            heater.Boiled += alarm.MakeAlert;            //注册方法
            heater.Boiled += (new Alarm()).MakeAlert;    //给匿名对象注册方法
            heater.Boiled += new Heater.BoiledEventHandler(alarm.MakeAlert); //也可以这么注册
            heater.Boiled += Display.ShowMsg;            //注册静态方法
            heater.BoilWater();     //烧水，会自动调用注册过对象的方法
            Console.WriteLine();

            Console.WriteLine("不使用异步调用的通常情况：");
            Console.WriteLine("Client application start");
            Thread.CurrentThread.Name = "Main Thread";
            Calculator cal = new Calculator();
            int result = cal.Add(6, 8);
            Console.WriteLine("Result：{0}", result);
            //做其他事情，模拟需要执行3秒钟
            for (int i = 1; i <= 3; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(i));
                Console.WriteLine("{0}：Client excute {1} second(s).", Thread.CurrentThread.Name, i);
            }
            Console.WriteLine();

            Console.WriteLine("使用异步调用：");
            Console.WriteLine("Client application started!\n");
            //Thread.CurrentThread.Name = "Main Thread";
            AddDelegate del = new AddDelegate(cal.Add);
            string data = "And data you want to pass";

            AsyncCallback callback = new AsyncCallback(Calculator.OnAddComplete);
            IAsyncResult asyncResult = del.BeginInvoke(5, 6, callback, data);   //异步调用方法
            del.EndInvoke(asyncResult);
            // 做某些其它的事情，模拟需要执行3 秒钟
            for (int i = 1; i <= 3; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(i));
                Console.WriteLine("{0}: Client executed {1} second(s).", Thread.CurrentThread.Name, i);
            }
            Console.ReadLine();

            Console.ReadKey();
        }

        #region 模拟热水器烧水过程 Observer设计模式
        /// <summary>
        /// 热水器
        /// </summary>
        public class Heater
        {
            private int temperature;         //水温
            public string type = "mode s";  //型号
            public string area = "Japan";   //产地

            public delegate void BoiledEventHandler(object sender, BoilEventArgs e);

            public event BoiledEventHandler Boiled;   //声明事件

            //定义BoilEventArgs类，传递给Observer所感兴趣的信息
            public class BoilEventArgs : EventArgs
            {
                public readonly int temperature;
                public BoilEventArgs(int temperature)
                {
                    this.temperature = temperature;
                }
            }

            //可以提供继承Header的类重写，以便继承类拒绝其他对象怼它的监视
            protected virtual void OnBoiled(BoilEventArgs e)
            {
                if (Boiled != null)
                {
                    Boiled(this, e);     //调用所有注册对象的方法
                }
            }

            //烧水
            public void BoilWater()
            {
                for (int i = 0; i < 100; i++)
                {
                    temperature = i;
                    if (temperature > 95)
                    {
                        //建立BoilEventArgs对象。
                        BoilEventArgs e = new BoilEventArgs(temperature);
                        OnBoiled(e);        //调用OnBoiled方法
                    }
                }
            }
        }

        public class Alarm
        {
            /// <summary>
            /// 发出语音警报
            /// </summary>
            /// <param name="param"></param>
            public void MakeAlert(object sender, Heater.BoilEventArgs e)
            {
                Heater heater = (Heater)sender;

                //访问send的公共字段
                Console.WriteLine("Alarm:{0}-{1}:", heater.area, heater.type);
                Console.WriteLine("Alarm:叮咚，水已经{0}度了", e.temperature);
                Console.WriteLine();
            }
        }

        public class Display
        {
            /// <summary>
            /// 显示水温
            /// </summary>
            public static void ShowMsg(object sender, Heater.BoilEventArgs e)    //静态方法
            {
                Heater heater = (Heater)sender;
                Console.WriteLine("Display：{0} - {1}: ", heater.area, heater.type);
                Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", e.temperature);
                Console.WriteLine();
            }
        }
        #endregion

        #region 模拟计算
        public delegate int AddDelegate(int x, int y);
        public class Calculator
        {
            public int Add(int x, int y)
            {
                if (Thread.CurrentThread.IsThreadPoolThread)
                {
                    Thread.CurrentThread.Name = "Pool Thread";
                }

                Console.WriteLine("Method Invoke");

                //执行某些事情，模拟需要执行2秒
                for (int i = 1; i <= 2; i++)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(i));
                    Console.WriteLine("{0}：Add execute {1} second(s).", Thread.CurrentThread.Name, i);
                }

                Console.WriteLine("Method complete");
                return x + y;
            }

            public static void OnAddComplete(IAsyncResult asyncResult)
            {
                AsyncResult result = (AsyncResult)asyncResult;
                AddDelegate del = (AddDelegate)result.AsyncDelegate;
                string data = (string)asyncResult.AsyncState;
                int run = del.EndInvoke(asyncResult);
                Console.WriteLine("{0}：Result,{1};Data:{2}", Thread.CurrentThread.Name, run, data);
            }
        }
        #endregion
    }
}
