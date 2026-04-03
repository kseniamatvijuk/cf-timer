namespace CFTimer.Models;

public enum TimerPhase
{
    Idle,
    Prep,
    Work,
    Rest,
    Finished
}

public enum TimerState
{
    Idle,
    Running,
    Paused,
    Finished
}
