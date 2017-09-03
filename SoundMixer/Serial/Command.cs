using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMixer.Serial
{
    class Command
    {
        public int Id { get; private set; }

        /// <summary>
        /// Volume between 0 (mute) and 100 (maximum)
        /// </summary>
        public int Volume { get; private set; }

        public Command(int id, int volume)
        {
            this.Id = id;
            this.Volume = volume;
        }
    }
}
