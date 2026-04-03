using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CFTimer.Models;
using CFTimer.Services;

namespace CFTimer.ViewModels;

[QueryProperty(nameof(WorkoutId), "workoutId")]
public partial class WorkoutDetailViewModel : BaseViewModel
{
    private readonly DatabaseService _db;
    private readonly WorkoutSessionService _session;

    public WorkoutDetailViewModel(DatabaseService db, WorkoutSessionService session)
    {
        _db = db;
        _session = session;
    }

    [ObservableProperty] private string? _workoutId;
    [ObservableProperty] private Workout? _workout;
    [ObservableProperty] private string _summary = string.Empty;
    [ObservableProperty] private string _typeName = string.Empty;

    partial void OnWorkoutIdChanged(string? value)
    {
        if (!string.IsNullOrEmpty(value))
            _ = LoadWorkoutAsync(value);
    }

    private async Task LoadWorkoutAsync(string id)
    {
        var workout = await _db.GetWorkoutAsync(id);
        if (workout is null) return;

        Workout = workout;
        Title = workout.Name;
        TypeName = workout.TimerType.ToString();
        Summary = workout.Summary;
    }

    [RelayCommand]
    private async Task StartWorkout()
    {
        if (Workout is null) return;
        Workout.LastUsedAt = DateTime.UtcNow;
        await _db.SaveWorkoutAsync(Workout);
        _session.CurrentWorkout = Workout;
        await Shell.Current.GoToAsync("activeWorkout");
    }

    [RelayCommand]
    private async Task EditWorkout()
    {
        if (Workout is null) return;
        await Shell.Current.GoToAsync($"timerSetup?timerType={Workout.TimerType}&workoutId={Workout.Id}");
    }

    [RelayCommand]
    private async Task DuplicateWorkout()
    {
        if (Workout is null) return;
        var copy = Workout.Clone();
        copy.Name = Workout.Name + " (copy)";
        await _db.SaveWorkoutAsync(copy);
        await Shell.Current.DisplayAlert("Duplicated", $"\"{copy.Name}\" created!", "OK");
    }

    [RelayCommand]
    private async Task DeleteWorkout()
    {
        if (Workout is null) return;
        bool confirm = await Shell.Current.DisplayAlert("Delete", $"Delete \"{Workout.Name}\"?", "Delete", "Cancel");
        if (!confirm) return;
        await _db.DeleteWorkoutAsync(Workout.Id);
        await Shell.Current.GoToAsync("..");
    }
}
