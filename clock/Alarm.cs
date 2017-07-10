using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clock
{
    class Alarm
    {
        private static String wavFile = System.IO.Directory.GetCurrentDirectory() + "\\..\\chime.wav";
        private System.Media.SoundPlayer player = new System.Media.SoundPlayer(wavFile);

        private bool isPlayed = false;

        public void RingOnce()
        {
            if (isPlayed)
            {
                return;
            }

            isPlayed = true;
            player.Play();
        }
    }
}
