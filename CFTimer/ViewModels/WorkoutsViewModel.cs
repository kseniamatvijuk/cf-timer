using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CFTimer.Models;
using CFTimer.Services;

namespace CFTimer.ViewModels;

public partial class WorkoutsViewModel : BaseViewModel
{
    private readonly DatabaseService _db;
    private readonly WorkoutSessionService _session;
    private List<Workout> _allWorkouts = [];

    public WorkoutsViewModel(DatabaseService db, WorkoutSessionService session)
    {
        _db = db;
        _session = session;
        Title = "Workouts";
    }

    public ObservableCollection<Workout> Workouts { get; } = [];

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private string _selectedFilter = "All";

    public string[] Filters { get; } = ["All", "EMOM", "AMRAP", "Tabata", "Interval", "Countdown", "ForTime", "Stopwatch"];

    partial void OnSearchTextChanged(string value) => ApplyFilters();
    partial void OnSelectedFilterChanged(string value) => ApplyFilters();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            _allWorkouts = await _db.GetWorkoutsAsync();
            ApplyFilters();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allWorkouts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(w => w.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        if (SelectedFilter != "All" && Enum.TryParse<TimerType>(SelectedFilter, out var type))
            filtered = filtered.Where(w => w.TimerType == type);

        Workouts.Clear();
        foreach (var w in filtered)
            Workouts.Add(w);
    }

    [RelayCommand]
    private async Task ToggleFavorite(Workout workout)
    {
        workout.IsFavorite = !workout.IsFavorite;
        await _db.SaveWorkoutAsync(workout);
        ApplyFilters();
    }

    [RelayCommand]
    private async Task DeleteWorkout(Workout workout)
    {
        bool confirm = await Shell.Current.DisplayAlert("Delete", $"Delete \"{workout.Name}\"?", "Delete", "Cancel");
        if (!confirm) return;

        await _db.DeleteWorkoutAsync(workout.Id);
        _allWorkouts.Remove(workout);
        Workouts.Remove(workout);
    }

    [RelayCommand]
    private async Task DuplicateWorkout(Workout workout)
    {
        var copy = workout.Clone();
        copy.Name = workout.Name + " (copy)";
        await _db.SaveWorkoutAsync(copy);
        _allWorkouts.Add(copy);
        ApplyFilters();
    }

    [RelayCommand]
    private async Task OpenWorkout(Workout workout)
    {
        await Shell.Current.GoToAsync($"workoutDetail?workoutId={workout.Id}");
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
