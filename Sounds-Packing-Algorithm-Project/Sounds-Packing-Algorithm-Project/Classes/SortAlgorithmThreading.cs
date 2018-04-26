using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace Sounds_Packing_Algorithm_Project.Classes
{
    class SortAlgorithmThreading
    {
        public List < Tuple < int , int > > arr;
        int count;
        public SortAlgorithmThreading(List<Tuple<int, int>> L)
        {
            //O(1)
            arr = new List<Tuple<int, int>>(L.Count);
            //O(Size)
            for (int i = 0 ; i < L.Count ; i++)
            {
                arr.Add(L[i]);
            }
            //O(1)
            count = 0;
        }
        public void getlist ( ref List < Tuple < int , int > > L)
        {
            //O(Size)
            L.Clear();
            //O(1)
            L = new List<Tuple<int, int>>(arr.Count);
            //O(size)
            for (int i = 0 ; i < arr.Count ; i++)
            {
                //O(1)
                L.Add(arr[i]);
            }
        }
        public void MergeSort()
        {
            //O(size*Log(size) )
            MergeSortUsingThreadingFirstStep(0, arr.Count - 1);
        }
        private void MergeSortUsingThreadingSecondStep (  int l, int mid, int r) 
        {
            //O(1)
            List<Tuple<int, int>> a = new List<Tuple<int, int>>();
            //O(1)
            int newindex = 0, i = l, j = mid + 1;
            //O(mid*2)
            while (i <= mid && j <= r)
            {

                if (arr[i].Item1 <  arr[j].Item1 )
                {
                    a.Add (arr[i]);
                    i++;
                }
                else if (arr[i].Item1 == arr[j].Item1 && arr[i].Item2 <= arr[j].Item2 )
                {
                    a.Add(arr[i]);
                    i++;
                }
                else
                {
                    a.Add(arr[j]);
                    j++;
                }

            }
            //O(mid)
            while (i <= mid)
            {
                a.Add(arr[i]);
                i++;
                newindex++;
            }
            //O(mid)
            while (j <= r)
            {
                a.Add(arr[j]);
                j++;
                newindex++;
            }
            //O(mid*2)
            for (i = 0; i < a.Count; i++ )
            {
                arr[l++] = a[i];
            }
        }

        private void MergeSortUsingThreadingFirstStep( int l, int r) 
        {
            // base case until we have only one element to sort
            //O(1)
            if (l >= r)
                return;
            // get the mid to divide the arr into two halves
            //O(1)
            int mid = (l + r) / 2;
            // check if there is less than 4 threads run then we can send our array to be sorted by another thread
            if (count < 4)
            {
                // increase our thread counter by 1
                //O(1)
                ++count;
                // send the left part of current array to be sorted by thread
                //O(mid * log (mid ))
                Thread t1 = new Thread(delegate() { MergeSortUsingThreadingFirstStep(l, mid); });
                // start the thread of the left array
                t1.Start();
                // sort the right part with the current thread
                //O(mid * log (mid ) )
                MergeSortUsingThreadingFirstStep(mid + 1, r);
                // wait until the left thread to finish
                t1.Join();
                // kill the first thread to use it later
                t1.Abort();
                // decrease the thread counter 
                //O(1)
                --count ;
            }
            else
            {
                // by entering here that mean we have no thread to use so we must sort by out cuurent thread
                //O(mid * log (mid ) )
                MergeSortUsingThreadingFirstStep(l, mid);
                //O(mid * log (mid ) )
                MergeSortUsingThreadingFirstStep(mid + 1, r);
            }
            // merge the two partes of the array
            //O(mid*2)
            MergeSortUsingThreadingSecondStep(l, mid, r);
        }
    }
}
