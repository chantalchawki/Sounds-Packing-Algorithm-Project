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
            arr = new List<Tuple<int, int>>(L.Count);
            for (int i = 0 ; i < L.Count ; i++)
            {
                arr.Add(L[i]);
            }
            count = 0;
        }
        public void getlist ( ref List < Tuple < int , int > > L)
        {
            L.Clear();
            L = new List<Tuple<int, int>>(arr.Count);
            for (int i = 0 ; i < arr.Count ; i++)
            {
                L.Add(arr[i]);
            }
        }
        public void MergeSort()
        {
            MergeSortUsingThreadingFirstStep(0, arr.Count - 1);
        }
        private void MergeSortUsingThreadingSecondStep (  int l, int mid, int r) 
        {

            List<Tuple<int, int>> a = new List<Tuple<int, int>>();
            int newindex = 0, i = l, j = mid + 1;
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
            while (i <= mid)
            {
                a.Add(arr[i]);
                i++;
                newindex++;
            }
            while (j <= r)
            {
                a.Add(arr[j]);
                j++;
                newindex++;
            }
            for (i = 0; i < a.Count; i++ )
            {
                arr[l++] = a[i];
            }
        }

        private void MergeSortUsingThreadingFirstStep( int l, int r) 
        {
            // base case until we have only one element to sort
            if (l >= r)
                return;
            // get the mid to divide the arr into two halves
            int mid = (l + r) / 2;
            // check if there is less than 4 threads run then we can send our array to be sorted by another thread
            if (count < 4)
            {
                // increase our thread counter by 1
                ++count;
                // send the left part of current array to be sorted by thread
                Thread t1 = new Thread(delegate() { MergeSortUsingThreadingFirstStep(l, mid); });
                // start the thread of the left array
                t1.Start();
                // sort the right part with the current thread
                MergeSortUsingThreadingFirstStep(mid + 1, r);
                // wait until the left thread to finish
                t1.Join();
                // kill the first thread to use it later
                t1.Abort();
                // decrease the thread counter 
                --count ;
            }
            else
            {
                // by entering here that mean we have no thread to use so we must sort by out cuurent thread
                MergeSortUsingThreadingFirstStep(l, mid);
                MergeSortUsingThreadingFirstStep(mid + 1, r);
            }
            // merge the two partes of the array 
            MergeSortUsingThreadingSecondStep(l, mid, r);
        }
    }
}
