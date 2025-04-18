using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Net.NetworkInformation;
using System.Xml;
using System.IO;
using System.Security;

namespace KirbStomp.Scripts.Classes.Sound
{
    class SoundManager
    {
        private Dictionary<string, SoundEffectInstance> _actionSounds;
        private Dictionary<string, float> _volumePercentages;
        // Singleton
        private static SoundManager inst;
        public SoundManager()
        {
            _actionSounds = new Dictionary<string, SoundEffectInstance>();
            _volumePercentages = new Dictionary<string, float>()
            {
                { "Walk", 0.2f },
                { "Landing", 0.3f },
                { "Hit", 0.2f }
            };
        }

        public static SoundManager Get()
        {
            if (inst == null)
            {
                inst = new SoundManager();
            }
            return inst;
        }

        public void LoadContent()
        {
            string[] sounds = { "Attack", "Special", "Jump", "Landing", "Walk", "SpecialUp", "Hit" };
            foreach (var sound in sounds)
            {
                SoundEffect soundEffect = Game1.Get().Content.Load<SoundEffect>(sound);
                SoundEffectInstance soundEffectInst = soundEffect.CreateInstance();
                if (_volumePercentages.TryGetValue(sound, out float volumePercent))
                {
                    soundEffectInst.Volume = volumePercent;
                }
                _actionSounds[sound] = soundEffectInst;
            } 
        }

        public void PlaySound(string sound)
        {
            if (_actionSounds.TryGetValue(sound, out SoundEffectInstance soundInst))
            {
                soundInst.Play();
            }
        }

    }
}
