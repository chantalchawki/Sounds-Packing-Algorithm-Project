using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sounds_Packing_Algorithm_Project
{
    class PriorityQueue
    {
            //O(1)
        public List<Tuple<int, int>> list;
        //O(1)
        public int Count { get { return list.Count; } }
        //O(1)
        public PriorityQueue()
        {
            //O(1)
            list = new List<Tuple<int, int>>();
        }
        //O(1)
        public PriorityQueue(int count)
        {//O(1)
            list = new List<Tuple<int, int>>(count);
        }

        //O(log (size ) )
        public void Enqueue(int x, int c)
        {
            //O(1)
            Tuple<int, int> tup = new Tuple<int, int>(x, c);
            //O(1)
            list.Add(tup);
            //O(1)
            int i = Count - 1;
            //O(Log (Size))
            while (i > 0)
            {
                //O(1)
                int p = (i - 1) / 2;
                //O(1)
                if (list[p].Item1 <= x) 
                    break;
                //O(1)
                list[i] = list[p];
                //O(1)
                i = p;
            }
            //O(1)
            if (Count > 0) 
                list[i] = tup;
        }
        //O(Log (size ) )
        public int Dequeue()
        {
            //O(1)
            int q = list[Count - 1].Item2;
            //O(1)
            int min = Peek();
            //O(1)
            int root = list[Count - 1].Item1;

            //O(1) removing from the end of the list
            list.RemoveAt(Count - 1);
            //O(1)
            Tuple<int, int> tup = new Tuple<int, int>(root, q);
            //O(1)
            int i = 0;
            //O(Log (Size))
            while (i * 2 + 1 < Count)
            {
                //O(1)
                int a = i * 2 + 1;
                //O(1)
                int b = i * 2 + 2;
                //O(1)
                int c = b < Count && list[b].Item1 < list[a].Item1 ? b : a;
                //O(1)
                if (list[c].Item1 >= root) break;
                //O(1)
                list[i] = list[c];
                //O(1)
                i = c;
            }
            //O(1)
            if (Count > 0) list[i] = tup;
            //O(1)
            return min;
        }
        //O(1)
        public int Peek()
        {
            //O(1)
            if (Count == 0) 
                throw new InvalidOperationException("Queue is empty.");
            //O(1)
            return list[0].Item1;
        }
        //O(1)
        public int ReturnIndex()
        {
            //O(1)
            if (Count == 0) 
                throw new InvalidOperationException("Queue is empty.");
            //O(1)
            return list[0].Item2;
        }
        public void Clear()
        {
            //O(Size)
            list.Clear();
        }
    }
}
