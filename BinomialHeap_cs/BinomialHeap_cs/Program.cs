using System;
using System.Collections.Generic;

namespace BinomialHeap_cs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BinomialHeap h = new BinomialHeap();

            int howMany = 16;

            for (int i = 0; i < howMany; i++)
            {
                Console.WriteLine("-----------------\n" +
                                  "Inserting: " + i);
                h.Insert(i);
                Printer.Print(h);
                Console.WriteLine("\n");
            }

            for (int i = 0; i < howMany; i++)
            {
                BinomialTree min = h.ExtractMin();
                Console.WriteLine("-----------------\n"+
                                  "Extract min = " + min.key);
                Printer.Print(h);
                Console.WriteLine("\n");
            }

        }
    }
}
