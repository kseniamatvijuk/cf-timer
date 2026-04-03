using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CFTimer.Models;
using CFTimer.Services;

namespace CFTimer.ViewModels;

[QueryProperty(nameof(TimerTypeName), "timerType")]
[QueryProperty(nameof(WorkoutId), "workoutId")]
public partial class TimerSetupViewModel : BaseViewModel
{
    private readonly DatabaseService _db;
    private readonly WorkoutSessionService _session;
    private readonly PreferencesService _preferences;

    public TimerSetupViewModel(DatabaseService db, WorkoutSessionService session, PreferencesService preferences)
    {
        _db = db;
        _session = session;
        _preferences = preferences;
    }

    [ObservableProperty] private string? _timerTypeName;
    [ObservableProperty] private string? _workoutId;
    [ObservableProperty] private TimerType _selectedTimerType;
    [ObservableProperty] private string _workoutName = string.Empty;
    [ObservableProperty] private int _totalRounds = 8;
    [ObservableProperty] private int _workMinutes = 0;
    [ObservableProperty] private int _workSeconds = 20;
    [ObservableProperty] private int _restMinutes = 0;
    [ObservableProperty] private int _restSeconds = 10;
    [ObservableProperty] private int _prepSeconds = 10;
    [ObservableProperty] private int _totalMinutes = 10;
    [ObservableProperty] private int _totalSeconds = 0;
    [ObservableProperty] private int _intervalMinutes = 1;
    [ObservableProperty] private int _intervalSeconds = 0;
    [ObservableProperty] private int _timeCapMinutes = 0;
    [ObservableProperty] private int _timeCapSeconds = 0;
    [ObservableProperty] private string _notes = string.Empty;
    [ObservableProperty] private bool _isEditing;

    // Visibility helpers
    public bool ShowRounds => SelectedTimerType is TimerType.EMOM or TimerType.Tabata or TimerType.Interval;
    public bool ShowWorkRest => SelectedTimerType is TimerType.Tabata or TimerType.Interval;
    public bool ShowTotalDuration => SelectedTimerType is TimerType.Countdown or TimerType.AMRAP;
    public bool ShowInterval => SelectedTimerType == TimerType.EMOM;
    public bool ShowTimeCap => SelectedTimerType == TimerType.ForTime;
    public bool ShowPrep => SelectedTimerType != TimerType.Stopwatch;

    partial void OnTimerTypeNameChanged(string? value)
    {
        if (Enum.TryParse<TimerType>(value, true, out var type))
        {
            SelectedTimerType = type;
            ApplyDefaults();
        }
    }

    partial void OnWorkoutIdChanged(string? value)
    {
        if (!string.IsNullOrEmpty(value))
            _ = LoadWorkoutAsync(value);
    }

    partial void OnSelectedTimerTypeChanged(TimerType value)
    {
        Title = $"{value} Setup";
        OnPropertyChanged(nameof(ShowRounds));
        OnPropertyChanged(nameof(ShowWorkRest));
        OnPropertyChanged(nameof(ShowTotalDuration));
        OnPropertyChanged(nameof(ShowInterval));
        OnPropertyChanged(nameof(ShowTimeCap));
        OnPropertyChanged(nameof(ShowPrep));
    }

    private void ApplyDefaults()
    {
        PrepSeconds = _preferences.DefaultPrepSeconds;

        switch (SelectedTimerType)
        {
            case TimerType.Tabata:
                TotalRounds = 8;
                WorkMinutes = 0; WorkSeconds = 20;
                RestMinutes = 0; RestSeconds = 10;
                break;
            case TimerType.EMOM:
                TotalRounds = 10;
                IntervalMinutes = 1; IntervalSeconds = 0;
                break;
            case TimerType.AMRAP:
                TotalMinutes = 10; TotalSeconds = 0;
                break;
            case TimerType.Countdown:
                TotalMinutes = 5; TotalSeconds = 0;
                break;
            case TimerType.Interval:
                TotalRounds = 5;
                WorkMinutes = 0; WorkSeconds = 40;
                RestMinutes = 0; RestSeconds = 20;
                break;
            case TimerType.ForTime:
                TimeCapMinutes = 20; TimeCapSeconds = 0;
                break;
        }
    }

    private async Task LoadWorkoutAsync(string id)
    {
        var workout = await _db.GetWorkoutAsync(id);
        if (workout is null) return;

        IsEditing = true;
        SelectedTimerType = workout.TimerType;
        WorkoutName = workout.Name;
        TotalRounds = workout.TotalRounds;
        WorkMinutes = (int)workout.WorkDuration.TotalMinutes;
        WorkSeconds = workout.WorkDuration.Seconds;
        RestMinutes = (int)workout.RestDuration.TotalMinutes;
        RestSeconds = workout.RestDuration.Seconds;
        PrepSeconds = (int)workout.PrepDuration.TotalSeconds;
        TotalMinutes = (int)workout.TotalDuration.TotalMinutes;
        TotalSeconds = workout.TotalDuration.Seconds;
        IntervalMinutes = (int)workout.IntervalDuration.TotalMinutes;
        IntervalSeconds = workout.IntervalDuration.Seconds;
        TimeCapMinutes = (int)workout.TimeCap.TotalMinutes;
        TimeCapSeconds = workout.TimeCap.Seconds;
        Notes = workout.Notes;
    }

    private Workout BuildWorkout()
    {
        return new Workout
        {
            Id = IsEditing && WorkoutId is not null ? WorkoutId : Guid.NewGuid().ToString(),
            Name = string.IsNullOrWhiteSpace(WorkoutName) ? SelectedTimerType.ToString() : WorkoutName,
            TimerType = SelectedTimerType,
            TotalRounds = TotalRounds,
            WorkDuration = TimeSpan.FromMinutes(WorkMinutes) + TimeSpan.FromSeconds(WorkSeconds),
            RestDuration = TimeSpan.FromMinutes(RestMinutes) + TimeSpan.FromSeconds(RestSeconds),
            PrepDuration = TimeSpan.FromSeconds(PrepSeconds),
            TotalDuration = TimeSpan.FromMinutes(TotalMinutes) + TimeSpan.FromSeconds(TotalSeconds),
            TimeCap = TimeSpan.FromMinutes(TimeCapMinutes) + TimeSpan.FromSeconds(TimeCapSeconds),
            IntervalDuration = TimeSpan.FromMinutes(IntervalMinutes) + TimeSpan.FromSeconds(IntervalSeconds),
            Notes = Notes,
            CreatedAt = IsEditing ? DateTime.UtcNow : DateTime.UtcNow,
            LastUsedAt = DateTime.UtcNow
        };
    }

    [RelayCommand]
    private async Task StartWorkout()
    {
        var workout = BuildWorkout();
        _session.CurrentWorkout = workout;
        await Shell.Current.GoToAsync("activeWorkout");
    }

    [RelayCommand]
    private async Task SaveWorkout()
    {
        var workout = BuildWorkout();
        await _db.SaveWorkoutAsync(workout);
        await Shell.Current.DisplayAlert("Saved", $"Workout \"{workout.Name}\" saved!", "OK");
    }

    [RelayCommand]
    private async Task SaveAndStart()
    {
        var workout = BuildWorkout();
        await _db.SaveWorkoutAsync(workout);
        _session.CurrentWorkout = workout;
        await Shell.Current.GoToAsync("activeWorkout");
    }
}
