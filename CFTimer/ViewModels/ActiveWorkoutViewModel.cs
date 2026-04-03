using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CFTimer.Models;
using CFTimer.Services;

namespace CFTimer.ViewModels;

public partial class ActiveWorkoutViewModel : BaseViewModel
{
    private readonly TimerEngine _timer;
    private readonly DatabaseService _db;
    private readonly WorkoutSessionService _session;
    private readonly PreferencesService _preferences;
    private Workout? _workout;

    public ActiveWorkoutViewModel(TimerEngine timer, DatabaseService db, WorkoutSessionService session, PreferencesService preferences)
    {
        _timer = timer;
        _db = db;
        _session = session;
        _preferences = preferences;
    }

    [ObservableProperty] private string _displayTime = "00:00";
    [ObservableProperty] private string _phaseLabel = "READY";
    [ObservableProperty] private string _nextPhaseLabel = string.Empty;
    [ObservableProperty] private string _roundDisplay = string.Empty;
    [ObservableProperty] private string _totalElapsed = "00:00";
    [ObservableProperty] private string _workoutName = string.Empty;
    [ObservableProperty] private double _progress;
    [ObservableProperty] private bool _isRunning;
    [ObservableProperty] private bool _isPaused;
    [ObservableProperty] private bool _isFinished;
    [ObservableProperty] private bool _showFinishButton;
    [ObservableProperty] private Color _phaseColor = Colors.White;
    [ObservableProperty] private Color _phaseTextColor = Colors.Black;

    public void Initialize()
    {
        _workout = _session.CurrentWorkout;
        if (_workout is null) return;

        WorkoutName = _workout.Name;
        ShowFinishButton = _workout.TimerType is TimerType.ForTime or TimerType.Stopwatch;

        _timer.Tick += OnTimerTick;
        _timer.PhaseChanged += OnPhaseChanged;
        _timer.WorkoutCompleted += OnWorkoutCompleted;
        _timer.Configure(_workout);

        if (_preferences.KeepScreenAwake)
            DeviceDisplay.KeepScreenOn = true;
    }

    public void Cleanup()
    {
        _timer.Tick -= OnTimerTick;
        _timer.PhaseChanged -= OnPhaseChanged;
        _timer.WorkoutCompleted -= OnWorkoutCompleted;
        _timer.Reset();
        DeviceDisplay.KeepScreenOn = false;
    }

    [RelayCommand]
    private void Start()
    {
        _timer.Start();
        IsRunning = true;
        IsPaused = false;
        IsFinished = false;
    }

    [RelayCommand]
    private void PauseResume()
    {
        if (_timer.State == TimerState.Running)
        {
            _timer.Pause();
            IsRunning = false;
            IsPaused = true;
        }
        else if (_timer.State == TimerState.Paused)
        {
            _timer.Resume();
            IsRunning = true;
            IsPaused = false;
        }
    }

    [RelayCommand]
    private void ResetTimer()
    {
        _timer.Reset();
        IsRunning = false;
        IsPaused = false;
        IsFinished = false;
        DisplayTime = "00:00";
        PhaseLabel = "READY";
        NextPhaseLabel = string.Empty;
        RoundDisplay = string.Empty;
        TotalElapsed = "00:00";
        Progress = 0;
        PhaseColor = Colors.White;
        PhaseTextColor = Colors.Black;
    }

    [RelayCommand]
    private void Skip()
    {
        _timer.Skip();
    }

    [RelayCommand]
    private void FinishWorkout()
    {
        _timer.Finish();
    }

    [RelayCommand]
    private async Task GoBack()
    {
        Cleanup();
        await Shell.Current.GoToAsync("..");
    }

    private void OnTimerTick()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DisplayTime = FormatTime(_timer.DisplayTime);
            TotalElapsed = FormatTime(_timer.TotalElapsed);
            Progress = _timer.Progress;
        });
    }

    private void OnPhaseChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            PhaseLabel = _timer.PhaseLabel;
            NextPhaseLabel = _timer.NextPhaseLabel;

            if (_timer.TotalRounds > 0)
                RoundDisplay = $"Round {_timer.CurrentRound} / {_timer.TotalRounds}";
            else
                RoundDisplay = string.Empty;

            UpdatePhaseColors();
        });
    }

    private async void OnWorkoutCompleted()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            IsRunning = false;
            IsPaused = false;
            IsFinished = true;
            PhaseLabel = "FINISHED!";
            PhaseColor = Color.FromArgb("#F0F9FF");
            PhaseTextColor = Color.FromArgb("#0F172A");

            if (_workout is not null)
            {
                var entry = new HistoryEntry
                {
                    WorkoutId = _workout.Id,
                    WorkoutName = _workout.Name,
                    TimerType = _workout.TimerType,
                    Duration = _timer.TotalElapsed,
                    CompletedAt = DateTime.UtcNow
                };
                await _db.SaveHistoryEntryAsync(entry);
            }
        });
    }

    private void UpdatePhaseColors()
    {
        (PhaseColor, PhaseTextColor) = _timer.CurrentPhase switch
        {
            TimerPhase.Work => (Color.FromArgb("#DCFCE7"), Color.FromArgb("#166534")),
            TimerPhase.Rest => (Color.FromArgb("#DBEAFE"), Color.FromArgb("#1E40AF")),
            TimerPhase.Prep => (Color.FromArgb("#FEF3C7"), Color.FromArgb("#92400E")),
            _ => (Colors.White, Colors.Black)
        };
    }

    private static string FormatTime(TimeSpan ts)
    {
        if (ts.TotalHours >= 1)
            return $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        return $"{(int)ts.TotalMinutes:D2}:{ts.Seconds:D2}";
    }
}
