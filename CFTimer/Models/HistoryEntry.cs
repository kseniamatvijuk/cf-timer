using SQLite;

namespace CFTimer.Models;

public class HistoryEntry
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? WorkoutId { get; set; }
    public string WorkoutName { get; set; } = string.Empty;
    public TimerType TimerType { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public long DurationTicks { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool Completed { get; set; } = true;

    [Ignore]
    public TimeSpan Duration
    {
        get => TimeSpan.FromTicks(DurationTicks);
        set => DurationTicks = value.Ticks;
    }

    [Ignore]
    public string DurationFormatted => Duration.TotalHours >= 1
        ? $"{(int)Duration.TotalHours}:{Duration.Minutes:D2}:{Duration.Seconds:D2}"
        : $"{(int)Duration.TotalMinutes}:{Duration.Seconds:D2}";
}
