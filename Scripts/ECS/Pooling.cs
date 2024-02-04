using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy.ECS
{
    public static class Pooling
    {
        public interface Poolable
        {
            public bool queueForPooling { get; set; }
        }
        public static int Add<T>(T poolable, ref List<T> list) where T : Poolable
        {
            var index = Find(list);
            if (index >= 0) 
            {
                list.RemoveAt(index);
                list.Insert(index, poolable);
                //list[index] = (T2)poolable;
                return index;
            }
            else 
            {
                list.Add(poolable);
                return list.Count - 1;
            }
        }
        public static int Find<T>(List<T> list) where T : Poolable
        {
            return list.FindIndex(0, list.Count, p => p.queueForPooling);
        }
        public static void Remove<T>(int index, ref List<T> list) where T : Poolable
        {
            
            list[index].queueForPooling = true;
            //list.RemoveAt(index);
            //list.Insert(index, new T());
            //list[index].queueForPooling = true;
        }
    }
}
