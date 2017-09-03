using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMixer
{
    class RangeConverter
    {
        public static long linearize(long val, long[] in_min, long[] in_max, long[] out_min, long[] out_max)
        {
            for (int i = 0; i < in_min.Length; i++)
            {
                if (val <= in_max[i])
                {
                    return RangeConverter.map(val, in_min[i], in_max[i], out_min[i], out_max[i]);
                }
            }
            return val;
        }

        public static long map(long val, long in_min, long in_max, long out_min, long out_max)
        {
            return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
