using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinomialHeap_cs
{
    public class BinomialHeap
    {
        private List<BinomialTree> heap;

        public BinomialHeap()
        {
            heap = new List<BinomialTree>();
        }

        public BinomialHeap(BinomialTree t)
        {
            heap = new List<BinomialTree>();
            heap.Add(t);
        }

        public List<BinomialTree> GetHeap()
        {
            return heap;
        }


        public int CountTrees()
        { return heap.Count; }

        // Returns amount of nodes in this heap
        // this is "lazy version"
        // Complexity O(logn) WHERE n = amount of nodes
        public int CountNodes()
        {
            int count = 0;
            foreach (BinomialTree tree in heap)
            {
                count += (int)Math.Pow(2, tree.degree);
            }
            return count;
        }

        


        // Inserts new key into the heap
        public void Insert(int key)
        {
            BinomialTree tmpTree = new BinomialTree(key);
            this.Insert(tmpTree);
        }


        // Inserts new tree into the heap
        public void Insert(BinomialTree tree)
        {
            BinomialHeap tmpHeap = new BinomialHeap(tree);

            this.Union(tmpHeap);
        }


        // Merges two heaps together into "this"
        // works like merge from MergeSort
        public void Union(BinomialHeap other)
        {
            if (other == null)
            {
                return;
            }

            List<BinomialTree> result = new List<BinomialTree>();           // result list for merged heaps
            List<BinomialTree> otherList = CopyList(other.GetHeap());       // Make a copy of other.heap to avoid destroying it on accident
            List<BinomialTree> thisList  = CopyList(this.GetHeap());        // for convenience only (this.heap will be replaced with "result" afterwards)

            // while there is still a tree in both heaps
            while (0 < thisList.Count && 0 < otherList.Count)
            {
                // the smaller tree should be added into result
                if (thisList.First().degree <= otherList.First().degree)
                {
                    result.Add(thisList.First());
                    thisList.RemoveAt(0);
                }
                else
                {
                    result.Add(otherList.First());
                    otherList.RemoveAt(0);
                }
            }

            // while there are still some trees left in this heap
            while (0 < thisList.Count)
            {
                result.Add(thisList.First());
                thisList.RemoveAt(0);
            }

            // while there are still some trees left in the other heap
            while (0 < otherList.Count)
            {
                result.Add(otherList.First());
                otherList.RemoveAt(0);
            }

            this.heap = result;             // now reassign this heap to the result
            this.MergeTreesOfSameHeight();  // and now merge all the trees of same height
        }



        // Linearly goes through the list of trees and checks three trees at the same time
        // and merges them if they are of same height
        private void MergeTreesOfSameHeight()
        {
            List<BinomialTree> result = new List<BinomialTree>();
            List<BinomialTree> trees = CopyList(this.GetHeap());


            // repeat while not done (while there are still trees to process)
            bool done = false;
            while (!done)
            {
                // depending on how many trees are left to process
                switch (trees.Count)
                {
                    // if none, then we are done
                    case 0:
                        done = true;
                        break;

                    // if one, then add it to result and we are done
                    case 1:
                        result.Add(trees.First());
                        done = true;
                        break;

                    // if two, then check if they are same height or not
                    case 2:
                        BinomialTree one = trees.First();
                        trees.RemoveAt(0);
                        BinomialTree two = trees.First();

                        // if they are different, then they should be in correct order already
                        if (one.degree != two.degree)
                        {
                            result.Add(one);
                            result.Add(two);
                        }
                        // if they are of same height
                        else
                        {
                            // insert theirs merge
                            result.Add(one.Merge(two));
                        }
                        done = true;
                        break;

                    // if more than two trees are present, then 
                    default:

                        // borrow all three trees
                        BinomialTree first = trees.First();
                        trees.RemoveAt(0);
                        BinomialTree second = trees.First();
                        trees.RemoveAt(0);
                        BinomialTree third = trees.First();
                        trees.RemoveAt(0);

                        // first compare the second and third
                        // if second and third are of same height, then
                        if (second.degree == third.degree)
                        {
                            // just add first into the result (because it's 100% smaller than second and third)
                            result.Add(first);
                            // return merged tree back into the list that we are processing
                            trees.Insert(0,second.Merge(third));
                        }
                        // if second and third are different
                        else
                        {
                            // compare first and second
                            if (first.degree == second.degree)
                            {
                                // create merged tree of first and second
                                BinomialTree mergedFirstSecond = first.Merge(second);

                                // if this new tree is of same height as the third tree
                                if (mergedFirstSecond.degree == third.degree)
                                {
                                    // merge them all together
                                    BinomialTree mergedFirstSecondThird = mergedFirstSecond.Merge(third);
                                    // and add it back to the list that we re processing, because we need to compare it with the next trees
                                    trees.Insert(0,mergedFirstSecondThird);
                                }
                                // if the new merged tree of First and Second are of different height than Third tree
                                else
                                {
                                    // then we can add this merged tree
                                    result.Add(mergedFirstSecond);
                                    // and return Third back into the list for further processing
                                    trees.Insert(0,third);
                                }
                            }
                            else // all three trees are different
                            {
                                // we can safely add the first
                                result.Add(first);
                                // and return second and third for further checks
                                trees.Insert(0,third);
                                trees.Insert(0,second);
                            }
                        }
                        break;
                }
            }
            this.heap = result;
        }



        // Helper method for copying contents of one list into another
        // (mainly because we want to avoid side effect e.g. destroying original list)
        private List<BinomialTree> CopyList(List<BinomialTree> treeList)
        {
            List<BinomialTree> result = new List<BinomialTree>();
            foreach (BinomialTree tree in treeList)
            {
                result.Add(tree);
            }

            return result;
        }


        // Just goes through all roots and remembers the root with smallest key
        // then returns reference to that tree (if heap didnt have any trees, then returns NULL)
        public BinomialTree Min()
        {
            if (heap.Count == 0)
            {
                return null;
            }
            else
            {
                BinomialTree minTree = new BinomialTree(Int32.MaxValue);
                foreach (BinomialTree tree in heap)
                {
                    if (tree.key < minTree.key)
                    {
                        minTree = tree;
                    }
                }

                return minTree;

            }
        }


        // Finds tree with min value
        // removes it from the heap,
        // then fixes the heap 
        // and lastly returns the tree.node with the min value (if there are no trees in the heap, then returns NULL)
        public BinomialTree ExtractMin()
        {
            BinomialTree min = Min();
            if (min == null)
            {
                return null;
            }
            else
            {
                // remember the left child
                BinomialTree child = min.child;

                // this is list of children of the deleted node
                List<BinomialTree> disJoined = new List<BinomialTree>();

                // while child isnt NULL
                while (child != null)
                {
                    // add the current child to the main heap as root of a tree
                    disJoined.Add(child);
                    
                    // move to the sibling
                    child = child.sibling;
                }
                heap.Remove(min);   // remove the node from the heap

                // for each child in the list of children remove the mutual pointers, and then each tree is added back to the heap
                foreach (BinomialTree tree in disJoined)
                {
                    tree.parent = null;
                    tree.sibling = null;
                    Insert(tree);
                }

                // the extracted node should not have any reference to the heap now
                min.degree = 0;
                min.child = null;
                min.sibling = null;
                min.parent = null;
                return min;
            }
        }
    }
    
}


