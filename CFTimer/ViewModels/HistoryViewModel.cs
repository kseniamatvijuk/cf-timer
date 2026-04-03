using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CFTimer.Models;
using CFTimer.Services;

namespace CFTimer.ViewModels;

public partial class HistoryViewModel : BaseViewModel
{
    private readonly DatabaseService _db;

    public HistoryViewModel(DatabaseService db)
    {
        _db = db;
        Title = "History";
    }

    public ObservableCollection<HistoryEntry> Entries { get; } = [];

    [ObservableProperty] private bool _isEmpty = true;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var entries = await _db.GetHistoryAsync();
            Entries.Clear();
            foreach (var e in entries)
                Entries.Add(e);
            IsEmpty = Entries.Count == 0;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ClearHistory()
    {
        bool confirm = await Shell.Current.DisplayAlert("Clear History", "Delete all workout history?", "Clear", "Cancel");
        if (!confirm) return;

        await _db.ClearHistoryAsync();
        Entries.Clear();
        IsEmpty = true;
    }

    [RelayCommand]
    private async Task DeleteEntry(HistoryEntry entry)
    {
        await _db.DeleteHistoryEntryAsync(entry.Id);
        Entries.Remove(entry);
        IsEmpty = Entries.Count == 0;
    }
}
