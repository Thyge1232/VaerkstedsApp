using SQLite;
using Workshop.Models;

namespace Workshop.Data;

public class AppDatabase
{
    public SQLiteAsyncConnection Conn { get; }

    public AppDatabase(string dbPath)
    {
        Conn = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitializeAsync()
    {
        try
        {
            await Conn.CreateTableAsync<Order>().ConfigureAwait(false);
            await Conn.CreateTableAsync<Invoice>().ConfigureAwait(false);
            await Conn.CreateTableAsync<InvoiceItem>().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex}"); // Check for all of the crashes
            throw;
        }
    }
}