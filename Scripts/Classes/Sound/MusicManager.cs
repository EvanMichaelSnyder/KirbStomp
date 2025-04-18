using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Net.NetworkInformation;

namespace KirbStomp.Scripts.Classes.Sound
{
    class MusicManager
    {
        private Song _song;
        private float _volumePercent;
        private bool isSongPlaying;
        // Singleton
        private static MusicManager inst;

        public MusicManager()
        {
            _song = null;
            _volumePercent = 0.25f;
            isSongPlaying = false;
        }

        public static MusicManager Get()
        {
            if (inst == null)
            {
                inst = new MusicManager();
            }
            return inst;
        }

        public void LoadMusic()
        {
            _song = Game1.Get().Content.Load<Song>("BattlefieldTheme");
            this.SetVolume();
        }

        public void PlayMusic()
        {
            if (_song == null)
            {
                throw new Exception("song" + _song + " does not exist");
            }
            if (!isSongPlaying)
            {
                isSongPlaying = true;
                MediaPlayer.Play(_song);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void StopMusic()
        {
            if (isSongPlaying)
            {
                MediaPlayer.Stop();
                MediaPlayer.IsRepeating = false;
            }
        }

        public void SetVolume()
        {
            MediaPlayer.Volume *= _volumePercent;
        }

    }
}