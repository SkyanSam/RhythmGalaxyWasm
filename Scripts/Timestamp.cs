/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy
{
    public struct TimestampInfo
    {
        public double time;
        public delegate void callbackType();
        public callbackType callback;
        public TimestampInfo(double time, callbackType callback)
        {
            this.time = time;
            this.callback = callback;
        }
    }
    public class TimestampManager
    {
        public static List<TimestampInfo> timestampInfos = new List<TimestampInfo>();
        public static void Add(double time, TimestampInfo.callbackType callback) 
        {
            timestampInfos.Add(new TimestampInfo(time, callback));
        }
        public static void Start()
        {
            timestampInfos = new List<TimestampInfo>();
        }
        public static void Update()
        {
            foreach (var info in timestampInfos)
            {
                if (info.time <= LevelManager.GetTime())
                {
                    info.callback(); // consider adding a yield seconds statement here of some sort
                    timestampInfos.Remove(info);
                }
            }
        }
    }
}
*/