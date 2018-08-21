using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualAbstractInterface
{
    class SubClass:Virtual
    {
        public override void Dosth()
        {
            base.Dosth();
            Console.WriteLine("subclass");
        }
    }

    class SubClass2 :Virtual
    {
        public override void Dosth()
        {
            //base.Dosth();
            Console.WriteLine("subclass2");
        }
    }

    class Virtual
    {
        public Virtual()
        {

        }

        public virtual void Dosth()
        {
            Console.WriteLine("parent class");
        }
    }
}
