using SQLite;

namespace CFTimer.Models;

public class Workout
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = string.Empty;
    public TimerType TimerType { get; set; }
    public int TotalRounds { get; set; } = 1;
    public long WorkDurationTicks { get; set; }
    public long RestDurationTicks { get; set; }
    public long PrepDurationTicks { get; set; } = TimeSpan.FromSeconds(10).Ticks;
    public long TotalDurationTicks { get; set; }
    public long TimeCapTicks { get; set; }
    public long IntervalDurationTicks { get; set; } = TimeSpan.FromMinutes(1).Ticks;
    public bool IsFavorite { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUsedAt { get; set; }
    public string Notes { get; set; } = string.Empty;

    [Ignore] public TimeSpan WorkDuration { get => TimeSpan.FromTicks(WorkDurationTicks); set => WorkDurationTicks = value.Ticks; }
    [Ignore] public TimeSpan RestDuration { get => TimeSpan.FromTicks(RestDurationTicks); set => RestDurationTicks = value.Ticks; }
    [Ignore] public TimeSpan PrepDuration { get => TimeSpan.FromTicks(PrepDurationTicks); set => PrepDurationTicks = value.Ticks; }
    [Ignore] public TimeSpan TotalDuration { get => TimeSpan.FromTicks(TotalDurationTicks); set => TotalDurationTicks = value.Ticks; }
    [Ignore] public TimeSpan TimeCap { get => TimeSpan.FromTicks(TimeCapTicks); set => TimeCapTicks = value.Ticks; }
    [Ignore] public TimeSpan IntervalDuration { get => TimeSpan.FromTicks(IntervalDurationTicks); set => IntervalDurationTicks = value.Ticks; }

    [Ignore]
    public string Summary => TimerType switch
    {
        TimerType.Stopwatch => "Stopwatch",
        TimerType.Countdown => FormatDuration(TotalDuration),
        TimerType.EMOM => $"{TotalRounds} rounds \u00d7 {FormatDuration(IntervalDuration)}",
        TimerType.AMRAP => FormatDuration(TotalDuration),
        TimerType.Tabata => $"{TotalRounds}R \u2022 {FormatDuration(WorkDuration)}/{FormatDuration(RestDuration)}",
        TimerType.ForTime => TimeCapTicks > 0 ? $"Cap {FormatDuration(TimeCap)}" : "No cap",
        TimerType.Interval => $"{TotalRounds}R \u2022 {FormatDuration(WorkDuration)}/{FormatDuration(RestDuration)}",
        _ => string.Empty
    };

    public Workout Clone()
    {
        return new Workout
        {
            Id = Guid.NewGuid().ToString(),
            Name = Name,
            TimerType = TimerType,
            TotalRounds = TotalRounds,
            WorkDurationTicks = WorkDurationTicks,
            RestDurationTicks = RestDurationTicks,
            PrepDurationTicks = PrepDurationTicks,
            TotalDurationTicks = TotalDurationTicks,
            TimeCapTicks = TimeCapTicks,
            IntervalDurationTicks = IntervalDurationTicks,
            IsFavorite = false,
            CreatedAt = DateTime.UtcNow,
            Notes = Notes
        };
    }

    private static string FormatDuration(TimeSpan ts) =>
        ts.TotalMinutes >= 1 ? $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}" : $"0:{ts.Seconds:D2}";
}
