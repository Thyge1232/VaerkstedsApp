using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.ViewModels;

public partial class InvoicesViewModel : ObservableObject
{
    private readonly AppDatabase _db;

    public InvoicesViewModel(AppDatabase db)
    {
        _db = db;
    }

    // Shown in the list UI
    public ObservableCollection<InvoiceRow> Items { get; } = new();

    [ObservableProperty] private string? searchText;

    // Reload when the search text changes
    partial void OnSearchTextChanged(string? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var invoices = await _db.Conn.Table<Invoice>().ToListAsync();

        // Build rows with customer name + totals
        var rowTasks = invoices.Select(async inv =>
        {
            var orderTask = _db.Conn.FindAsync<Order>(inv.OrderId);
            var itemsTask = _db.Conn.Table<InvoiceItem>()
                                    .Where(i => i.InvoiceId == inv.Id)
                                    .ToListAsync();

            await Task.WhenAll(orderTask, itemsTask);

            var mats = itemsTask.Result.Sum(i => i.UnitPrice * i.Quantity);
            var labor = inv.Hours * inv.HourPrice;

            return new InvoiceRow
            {
                InvoiceId = inv.Id,
                OrderId = inv.OrderId,
                CustomerName = orderTask.Result?.CustomerName ?? "",
                MaterialsTotal = mats,
                LaborTotal = labor,
                GrandTotal = mats + labor
            };
        });

        var rows = await Task.WhenAll(rowTasks);

        // filter by invoice id, order id, or customer name
        var q = SearchText?.Trim();
        if (!string.IsNullOrEmpty(q))
        {
            rows = rows.Where(r =>
                    r.InvoiceId.ToString().Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    r.OrderId.ToString().Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (r.CustomerName?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToArray();
        }

        Items.Clear();
        foreach (var r in rows) Items.Add(r);
    }

    // What is shown in the list
    public class InvoiceRow
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public double MaterialsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double GrandTotal { get; set; }
    }
}
