namespace Celeste.Mod.AnalogViewer
{
    public class AnalogViewerSettings : EverestModuleSettings
    {
        public bool Enabled { get; set; } = true;

        [SettingSubText("This is the dead zone method used by the game.")]
        public bool ShowIndepdentAxes { get; set; } = true;

        [SettingSubText("This is the raw analog stick position.")]
        public bool ShowNoDeadZone { get; set; } = true;

        [SettingSubText("This gives more accurate angles while still having a dead zone.")]
        public bool ShowCircularDeadZone { get; set; } = true;
    }
}
