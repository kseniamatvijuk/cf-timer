namespace CFTimer.Models;

public class PhaseStep
{
    public TimerPhase Phase { get; set; }
    public TimeSpan Duration { get; set; }
    public int RoundNumber { get; set; }
    public bool IsCountUp { get; set; }
}
