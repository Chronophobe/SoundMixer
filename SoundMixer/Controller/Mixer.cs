using SoundMixer.Serial;
using System.Collections.Generic;

namespace SoundMixer.Controller
{
    class Mixer
    {
        private IList<IController> controllers;

        public Mixer()
        {
            this.controllers = new List<IController>();
            this.controllers.Add(new MasterController());
        }

        public Mixer(IList<string> processes)
        {
            this.controllers = new List<IController>();
            this.controllers.Add(new MasterController());
            foreach (var process in processes)
            {
                this.controllers.Add(new ProcessController(process));
            }
        }

        public bool SetVolume(Command command)
        {
            if (this.controllers.Count > command.Id)
            {
                return this.controllers[command.Id].SetVolume(command.Volume);
            }
            return false;
        }
    }
}
