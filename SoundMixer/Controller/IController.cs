using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMixer.Controller
{
    interface IController
    {
        /// <summary>
        /// Set the volume.
        /// </summary>
        /// <param name="volume">The new volume between 0 (mute) and 100 (max).</param>
        /// <returns>True, if the volume could be set. False, otherwise.</returns>
        bool SetVolume(float volume);
    }
}
