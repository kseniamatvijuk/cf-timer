namespace CFTimer.Services;

public class AudioService
{
    private readonly PreferencesService _preferences;

    public AudioService(PreferencesService preferences)
    {
        _preferences = preferences;
    }

    public void PlayCountdownBeep()
    {
        if (!_preferences.SoundEnabled) return;
        // MVP: simple vibration as audio cue
        VibrateShort();
    }

    public void PlayPhaseChange()
    {
        if (!_preferences.SoundEnabled) return;
        VibrateMedium();
    }

    public void PlayWorkoutComplete()
    {
        if (!_preferences.SoundEnabled) return;
        VibrateLong();
    }

    public void PlayLastSeconds()
    {
        if (!_preferences.SoundEnabled) return;
        VibrateShort();
    }

    private void VibrateShort()
    {
        if (!_preferences.VibrationEnabled) return;
        try { Vibration.Vibrate(TimeSpan.FromMilliseconds(100)); } catch { /* ignore on unsupported platforms */ }
    }

    private void VibrateMedium()
    {
        if (!_preferences.VibrationEnabled) return;
        try { Vibration.Vibrate(TimeSpan.FromMilliseconds(300)); } catch { }
    }

    private void VibrateLong()
    {
        if (!_preferences.VibrationEnabled) return;
        try { Vibration.Vibrate(TimeSpan.FromMilliseconds(600)); } catch { }
    }
}
