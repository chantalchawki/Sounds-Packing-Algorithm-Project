using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sounds_Packing_Algorithm_Project
{
    class PriorityQueue
    {
        public List<Tuple<int, int>> list;
        public int Count { get { return list.Count; } }

        public PriorityQueue()
        {
            list = new List<Tuple<int, int>>();
        }

        public PriorityQueue(int count)
        {
            list = new List<Tuple<int, int>>(count);
        }


        public void Enqueue(int x, int c)
        {
            Tuple<int, int> tup = new Tuple<int, int>(x, c);
            list.Add(tup);
            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (list[p].Item1 <= x) break;

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = tup;
        }

        public int Dequeue()
        {
            int q = list[Count - 1].Item2;
            int min = Peek();
            int root = list[Count - 1].Item1;

            
            list.RemoveAt(Count - 1);
            Tuple<int, int> tup = new Tuple<int, int>(root, q);
            int i = 0;
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1;
                int b = i * 2 + 2;
                int c = b < Count && list[b].Item1 < list[a].Item1 ? b : a;

                if (list[c].Item1 >= root) break;
                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = tup;
            return min;
        }

        public int Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0].Item1;
        }

        public int ReturnIndex()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0].Item2;
        }
        public void Clear()
        {
            list.Clear();
        }
    }
}
