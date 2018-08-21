using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualAbstractInterface
{
    public class Abstract
    {
        class Test
        {
            public void Dosth()
            {
                Console.WriteLine("test");
            }
        }

        abstract class Abstracts:Test
        {
            public abstract void Dosth();
        }
    }
}
