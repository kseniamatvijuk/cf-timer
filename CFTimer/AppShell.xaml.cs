using CFTimer.Views;

namespace CFTimer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("timerSetup", typeof(TimerSetupPage));
        Routing.RegisterRoute("activeWorkout", typeof(ActiveWorkoutPage));
        Routing.RegisterRoute("workoutDetail", typeof(WorkoutDetailPage));
    }
}
