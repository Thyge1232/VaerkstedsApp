using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    private readonly AppDatabase _db;

    public ObservableCollection<Order> Orders { get; } = new();

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    public CalendarViewModel(AppDatabase db)
    {
        _db = db;
    }

    // auto-trigger when date changes
    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadForDateAsync();
    }

    [RelayCommand]
    public async Task LoadForDateAsync()
    {
        var list = await _db.Conn.Table<Order>()
            .Where(o => o.DropOffDateTime.Date == SelectedDate.Date)
            .ToListAsync();

        Orders.Clear();
        foreach (var o in list) Orders.Add(o);
    }
}
