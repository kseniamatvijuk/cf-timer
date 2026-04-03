using System.Diagnostics;
using CFTimer.Models;

namespace CFTimer.Services;

public class TimerEngine
{
    private readonly AudioService _audio;
    private readonly List<PhaseStep> _phases = [];
    private int _currentPhaseIndex = -1;
    private readonly Stopwatch _phaseStopwatch = new();
    private readonly Stopwatch _totalStopwatch = new();
    private IDispatcherTimer? _uiTimer;
    private TimeSpan _currentPhaseDuration;
    private int _lastAnnouncedSecond = -1;

    public TimerEngine(AudioService audio)
    {
        _audio = audio;
    }

    public TimerState State { get; private set; } = TimerState.Idle;
    public TimerPhase CurrentPhase { get; private set; } = TimerPhase.Idle;
    public TimeSpan DisplayTime { get; private set; }
    public TimeSpan TotalElapsed => _totalStopwatch.Elapsed;
    public int CurrentRound { get; private set; }
    public int TotalRounds { get; private set; }
    public string PhaseLabel { get; private set; } = string.Empty;
    public string NextPhaseLabel { get; private set; } = string.Empty;
    public double Progress { get; private set; }
    public bool IsCountUp { get; private set; }

    public event Action? Tick;
    public event Action? PhaseChanged;
    public event Action? WorkoutCompleted;

    public void Configure(Workout workout)
    {
        Reset();
        _phases.Clear();

        var prepTime = workout.PrepDuration;
        if (prepTime > TimeSpan.Zero)
            _phases.Add(new PhaseStep { Phase = TimerPhase.Prep, Duration = prepTime, RoundNumber = 0 });

        switch (workout.TimerType)
        {
            case TimerType.Stopwatch:
                _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = TimeSpan.MaxValue, IsCountUp = true, RoundNumber = 1 });
                TotalRounds = 0;
                break;

            case TimerType.Countdown:
                _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = workout.TotalDuration, RoundNumber = 1 });
                TotalRounds = 1;
                break;

            case TimerType.EMOM:
                TotalRounds = workout.TotalRounds;
                for (int i = 1; i <= workout.TotalRounds; i++)
                    _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = workout.IntervalDuration, RoundNumber = i });
                break;

            case TimerType.AMRAP:
                _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = workout.TotalDuration, RoundNumber = 1 });
                TotalRounds = 1;
                break;

            case TimerType.Tabata:
                TotalRounds = workout.TotalRounds;
                for (int i = 1; i <= workout.TotalRounds; i++)
                {
                    _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = workout.WorkDuration, RoundNumber = i });
                    if (i < workout.TotalRounds)
                        _phases.Add(new PhaseStep { Phase = TimerPhase.Rest, Duration = workout.RestDuration, RoundNumber = i });
                }
                break;

            case TimerType.ForTime:
                var cap = workout.TimeCap > TimeSpan.Zero ? workout.TimeCap : TimeSpan.MaxValue;
                _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = cap, IsCountUp = true, RoundNumber = 1 });
                TotalRounds = 1;
                break;

            case TimerType.Interval:
                TotalRounds = workout.TotalRounds;
                for (int i = 1; i <= workout.TotalRounds; i++)
                {
                    _phases.Add(new PhaseStep { Phase = TimerPhase.Work, Duration = workout.WorkDuration, RoundNumber = i });
                    if (i < workout.TotalRounds && workout.RestDuration > TimeSpan.Zero)
                        _phases.Add(new PhaseStep { Phase = TimerPhase.Rest, Duration = workout.RestDuration, RoundNumber = i });
                }
                break;
        }
    }

    public void Start()
    {
        if (_phases.Count == 0) return;
        if (State == TimerState.Running) return;

        if (State == TimerState.Paused)
        {
            Resume();
            return;
        }

        _currentPhaseIndex = -1;
        _totalStopwatch.Reset();
        _totalStopwatch.Start();
        MoveToNextPhase();
        StartUiTimer();
        State = TimerState.Running;
    }

    public void Pause()
    {
        if (State != TimerState.Running) return;
        _phaseStopwatch.Stop();
        _totalStopwatch.Stop();
        StopUiTimer();
        State = TimerState.Paused;
        Tick?.Invoke();
    }

    public void Resume()
    {
        if (State != TimerState.Paused) return;
        _phaseStopwatch.Start();
        _totalStopwatch.Start();
        StartUiTimer();
        State = TimerState.Running;
    }

    public void Reset()
    {
        StopUiTimer();
        _phaseStopwatch.Reset();
        _totalStopwatch.Reset();
        _currentPhaseIndex = -1;
        State = TimerState.Idle;
        CurrentPhase = TimerPhase.Idle;
        DisplayTime = TimeSpan.Zero;
        CurrentRound = 0;
        PhaseLabel = string.Empty;
        NextPhaseLabel = string.Empty;
        Progress = 0;
        _lastAnnouncedSecond = -1;
        Tick?.Invoke();
    }

    public void Skip()
    {
        if (State != TimerState.Running && State != TimerState.Paused) return;
        MoveToNextPhase();
    }

    public void Finish()
    {
        StopUiTimer();
        _phaseStopwatch.Stop();
        _totalStopwatch.Stop();
        State = TimerState.Finished;
        CurrentPhase = TimerPhase.Finished;
        PhaseLabel = "FINISHED";
        NextPhaseLabel = string.Empty;
        _audio.PlayWorkoutComplete();
        WorkoutCompleted?.Invoke();
        Tick?.Invoke();
    }

    private void MoveToNextPhase()
    {
        _currentPhaseIndex++;

        if (_currentPhaseIndex >= _phases.Count)
        {
            Finish();
            return;
        }

        var step = _phases[_currentPhaseIndex];
        CurrentPhase = step.Phase;
        _currentPhaseDuration = step.Duration;
        CurrentRound = step.RoundNumber;
        IsCountUp = step.IsCountUp;
        _phaseStopwatch.Restart();
        _lastAnnouncedSecond = -1;

        PhaseLabel = step.Phase switch
        {
            TimerPhase.Prep => "GET READY",
            TimerPhase.Work => "WORK",
            TimerPhase.Rest => "REST",
            _ => string.Empty
        };

        NextPhaseLabel = GetNextPhaseLabel();

        _audio.PlayPhaseChange();
        PhaseChanged?.Invoke();
        UpdateDisplay();
    }

    private string GetNextPhaseLabel()
    {
        if (_currentPhaseIndex + 1 >= _phases.Count) return "Finish";
        var next = _phases[_currentPhaseIndex + 1];
        return next.Phase switch
        {
            TimerPhase.Work => $"Work - Round {next.RoundNumber}",
            TimerPhase.Rest => "Rest",
            _ => string.Empty
        };
    }

    private void StartUiTimer()
    {
        if (_uiTimer is not null) return;
        _uiTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_uiTimer is null) return;
        _uiTimer.Interval = TimeSpan.FromMilliseconds(100);
        _uiTimer.Tick += OnUiTimerTick;
        _uiTimer.Start();
    }

    private void StopUiTimer()
    {
        if (_uiTimer is null) return;
        _uiTimer.Stop();
        _uiTimer.Tick -= OnUiTimerTick;
        _uiTimer = null;
    }

    private void OnUiTimerTick(object? sender, EventArgs e)
    {
        if (State != TimerState.Running) return;

        var elapsed = _phaseStopwatch.Elapsed;

        if (!IsCountUp && _currentPhaseDuration != TimeSpan.MaxValue && elapsed >= _currentPhaseDuration)
        {
            MoveToNextPhase();
            return;
        }

        UpdateDisplay();

        // 3-2-1 countdown beeps
        if (!IsCountUp && _currentPhaseDuration != TimeSpan.MaxValue)
        {
            var remaining = _currentPhaseDuration - elapsed;
            var sec = (int)Math.Ceiling(remaining.TotalSeconds);
            if (sec <= 3 && sec > 0 && sec != _lastAnnouncedSecond)
            {
                _lastAnnouncedSecond = sec;
                _audio.PlayLastSeconds();
            }
        }

        Tick?.Invoke();
    }

    private void UpdateDisplay()
    {
        var elapsed = _phaseStopwatch.Elapsed;

        if (IsCountUp)
        {
            DisplayTime = elapsed;
            Progress = 0;
        }
        else if (_currentPhaseDuration == TimeSpan.MaxValue)
        {
            DisplayTime = elapsed;
            Progress = 0;
        }
        else
        {
            var remaining = _currentPhaseDuration - elapsed;
            if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;
            DisplayTime = remaining;
            Progress = _currentPhaseDuration.TotalSeconds > 0
                ? elapsed.TotalSeconds / _currentPhaseDuration.TotalSeconds
                : 0;
        }
    }
}
