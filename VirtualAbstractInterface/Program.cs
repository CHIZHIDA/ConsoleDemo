using System;

namespace VirtualAbstractInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            SubClass subClass = new SubClass();
            SubClass2 subClass2 = new SubClass2();

            subClass2.Dosth();
            subClass.Dosth();
            
            Console.ReadKey();
        }
    }
}
