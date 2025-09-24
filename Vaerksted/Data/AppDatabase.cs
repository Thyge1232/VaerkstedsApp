using SQLite;
using Workshop.Models;

namespace Workshop.Data;

public class AppDatabase
{
    public SQLiteAsyncConnection Conn { get; }

    public AppDatabase(string dbPath)
    {
        Conn = new SQLiteAsyncConnection(dbPath);

        // Initialiser tabeller asynkront
        Task.Run(async () =>
        {
            try
            {
                await Conn.CreateTableAsync<Order>();
                await Conn.CreateTableAsync<Invoice>();
                await Conn.CreateTableAsync<InvoiceItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex}");
            }
        });
    }
}