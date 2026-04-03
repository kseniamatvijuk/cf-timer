namespace CFTimer.Services;

public class PreferencesService
{
    public bool SoundEnabled
    {
        get => Preferences.Get(nameof(SoundEnabled), true);
        set => Preferences.Set(nameof(SoundEnabled), value);
    }

    public bool VibrationEnabled
    {
        get => Preferences.Get(nameof(VibrationEnabled), true);
        set => Preferences.Set(nameof(VibrationEnabled), value);
    }

    public bool CountdownEnabled
    {
        get => Preferences.Get(nameof(CountdownEnabled), true);
        set => Preferences.Set(nameof(CountdownEnabled), value);
    }

    public bool KeepScreenAwake
    {
        get => Preferences.Get(nameof(KeepScreenAwake), true);
        set => Preferences.Set(nameof(KeepScreenAwake), value);
    }

    public int DefaultPrepSeconds
    {
        get => Preferences.Get(nameof(DefaultPrepSeconds), 10);
        set => Preferences.Set(nameof(DefaultPrepSeconds), value);
    }
}
