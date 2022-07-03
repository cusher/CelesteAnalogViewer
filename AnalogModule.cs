using System;

namespace Celeste.Mod.AnalogViewer
{
    public class AnalogModule : EverestModule
    {
        public static AnalogModule Instance;

        public override Type SettingsType => typeof(AnalogViewerSettings);
        public static AnalogViewerSettings Settings => (AnalogViewerSettings)Instance._Settings;

        public AnalogModule()
        {
            Instance = this;
        }

        public override void Load()
        {
            Everest.Events.Level.OnLoadLevel += modLoadLevel;
        }

        private void modLoadLevel(Level level, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            level.Add(new AnalogDisplay());
        }

        public override void Unload()
        {
            Everest.Events.Level.OnLoadLevel -= modLoadLevel;
        }
    }
}
