using CommunityToolkit.Mvvm.ComponentModel;
using CFTimer.Services;

namespace CFTimer.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly PreferencesService _preferences;

    public SettingsViewModel(PreferencesService preferences)
    {
        _preferences = preferences;
        Title = "Settings";

        _soundEnabled = _preferences.SoundEnabled;
        _vibrationEnabled = _preferences.VibrationEnabled;
        _countdownEnabled = _preferences.CountdownEnabled;
        _keepScreenAwake = _preferences.KeepScreenAwake;
        _defaultPrepSeconds = _preferences.DefaultPrepSeconds;
    }

    [ObservableProperty] private bool _soundEnabled;
    [ObservableProperty] private bool _vibrationEnabled;
    [ObservableProperty] private bool _countdownEnabled;
    [ObservableProperty] private bool _keepScreenAwake;
    [ObservableProperty] private int _defaultPrepSeconds;

    partial void OnSoundEnabledChanged(bool value) => _preferences.SoundEnabled = value;
    partial void OnVibrationEnabledChanged(bool value) => _preferences.VibrationEnabled = value;
    partial void OnCountdownEnabledChanged(bool value) => _preferences.CountdownEnabled = value;
    partial void OnKeepScreenAwakeChanged(bool value) => _preferences.KeepScreenAwake = value;
    partial void OnDefaultPrepSecondsChanged(int value) => _preferences.DefaultPrepSeconds = value;

    public string AppVersion => AppInfo.VersionString;
}
