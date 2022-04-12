using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinomialHeap_cs
{
    public class Printer
    {


        public static void Print(BinomialHeap BH)
        {
            foreach (BinomialTree binomialTree in BH.GetHeap())
            {
                Console.WriteLine("-");
                PrintTree(binomialTree);
            }
        }


        private static void PrintTree(BinomialTree BT)
        {
            BinomialTree current = BT;
            List<BinomialTree> stack = new List<BinomialTree>();
            while (current != null)
            {
                stack.Add(current);
                current = current.sibling;
            }
            
            while (stack.Count != 0)
            {
                Console.Write(" ");
                Console.Write(stack[0].key);
                PrintTree(stack[0].child);
                Console.WriteLine();
                stack.RemoveAt(0);
            }
        }
        
    }
}
