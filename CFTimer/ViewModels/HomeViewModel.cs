using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CFTimer.Models;
using CFTimer.Services;

namespace CFTimer.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly DatabaseService _db;
    private readonly WorkoutSessionService _session;

    public HomeViewModel(DatabaseService db, WorkoutSessionService session)
    {
        _db = db;
        _session = session;
        Title = "CFTimer";
    }

    public ObservableCollection<Workout> RecentWorkouts { get; } = [];
    public ObservableCollection<Workout> FavoriteWorkouts { get; } = [];

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var workouts = await _db.GetWorkoutsAsync();

            RecentWorkouts.Clear();
            FavoriteWorkouts.Clear();

            foreach (var w in workouts.Where(w => w.LastUsedAt.HasValue).OrderByDescending(w => w.LastUsedAt).Take(5))
                RecentWorkouts.Add(w);

            foreach (var w in workouts.Where(w => w.IsFavorite).Take(5))
                FavoriteWorkouts.Add(w);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToTimerSetup(string timerType)
    {
        await Shell.Current.GoToAsync($"timerSetup?timerType={timerType}");
    }

    [RelayCommand]
    private async Task StartWorkout(Workout workout)
    {
        workout.LastUsedAt = DateTime.UtcNow;
        await _db.SaveWorkoutAsync(workout);
        _session.CurrentWorkout = workout;
        await Shell.Current.GoToAsync("activeWorkout");
    }

    [RelayCommand]
    private async Task CreateWorkout()
    {
        await Shell.Current.GoToAsync("timerSetup");
    }
}
