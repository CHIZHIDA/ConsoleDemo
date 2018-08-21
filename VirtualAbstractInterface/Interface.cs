using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualAbstractInterface
{
    interface Interface
    {
        void Eat();
    }

    class Cat:Virtual,Interface
    {
        public override void Dosth()
        {
            base.Dosth();
        }

        public void Eat()
        {
            Console.WriteLine("Interface Eat");
        }
    }
}
