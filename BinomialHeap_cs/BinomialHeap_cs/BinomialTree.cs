using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinomialHeap_cs
{
    public class BinomialTree
    {
        public int key;
        public int degree;
        public BinomialTree parent;
        public BinomialTree child;
        public BinomialTree sibling;

        public BinomialTree(int key)
        {
            this.key = key;
            this.degree = 0;
        }


        /*
         * Merging Binomial Trees
         * both trees must be of same height
         * practically - just link one of the trees as a child of the other (parent has root.key < other.root.key)
         * returns the merged tree
         */
        public BinomialTree Merge(BinomialTree other)
        {
            if (other == null)
            {
                throw new ArgumentNullException();
            }

            if (this.degree != other.degree)
            {
                throw new ArgumentException("Trying to merge trees of different height");
            }


            if (this.key <= other.key)
            {
                this.BinomialLink(other);
                return this;
            }
            else
            {
                other.BinomialLink(this);
                return other;
            }
        }


        /*
         * "this" Tree is parent,
         * "other" is new child
         */
        private void BinomialLink(BinomialTree other)
        {
            BinomialTree tmp = this.child;

            this.child = other;
            other.parent = this;
            other.sibling = tmp;
            this.degree += 1;
        }
        
        
    }
}
