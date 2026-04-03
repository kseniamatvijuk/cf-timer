using SQLite;
using CFTimer.Models;

namespace CFTimer.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "cftimer.db3");
    }

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database is not null) return _database;

        _database = new SQLiteAsyncConnection(_dbPath);
        await _database.CreateTableAsync<Workout>();
        await _database.CreateTableAsync<HistoryEntry>();
        return _database;
    }

    public async Task<List<Workout>> GetWorkoutsAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Workout>().OrderByDescending(w => w.CreatedAt).ToListAsync();
    }

    public async Task<Workout?> GetWorkoutAsync(string id)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Workout>().FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task SaveWorkoutAsync(Workout workout)
    {
        var db = await GetDatabaseAsync();
        var existing = await db.Table<Workout>().FirstOrDefaultAsync(w => w.Id == workout.Id);
        if (existing is not null)
            await db.UpdateAsync(workout);
        else
            await db.InsertAsync(workout);
    }

    public async Task DeleteWorkoutAsync(string id)
    {
        var db = await GetDatabaseAsync();
        await db.Table<Workout>().DeleteAsync(w => w.Id == id);
    }

    public async Task<List<HistoryEntry>> GetHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<HistoryEntry>().OrderByDescending(h => h.CompletedAt).ToListAsync();
    }

    public async Task SaveHistoryEntryAsync(HistoryEntry entry)
    {
        var db = await GetDatabaseAsync();
        await db.InsertAsync(entry);
    }

    public async Task DeleteHistoryEntryAsync(string id)
    {
        var db = await GetDatabaseAsync();
        await db.Table<HistoryEntry>().DeleteAsync(h => h.Id == id);
    }

    public async Task ClearHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAllAsync<HistoryEntry>();
    }
}
