using System;
using System.Windows.Media;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Audio; 

namespace MathClock
{
    public static class SoundEffects
    {
        private static bool initialized = false;
        private static SoundEffect alarm;

        // Must be called before using static methods.
        public static void Initialize()
        {
            if (SoundEffects.initialized)
                return;

            StreamResourceInfo info = App.GetResourceStream(new Uri("Audio/alarm.wav", UriKind.Relative));
            alarm = SoundEffect.FromStream(info.Stream);

            // Adds an Update delegate, to simulate the XNA update method.
            CompositionTarget.Rendering += delegate(object sender, EventArgs e)
            {
                Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            };

            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            initialized = true;
        }

        public static SoundEffect Alarm
        {
            get
            {
                // If not initialized, returns null.
                if (!SoundEffects.initialized)
                    return null;

                return alarm;
            }
        }
    }
}
