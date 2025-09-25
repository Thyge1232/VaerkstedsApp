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

    
    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadForDateAsync();
    }

    [RelayCommand]
    public async Task LoadForDateAsync() // Fixet for at fix crash ved opstart
    {
        var start = SelectedDate.Date;
        var end = start.AddDays(1);

        var list = await _db.Conn.Table<Order>()
            .Where(o => o.DropOffDateTime >= start && o.DropOffDateTime < end)
            .ToListAsync();

        Orders.Clear();
        foreach (var o in list) Orders.Add(o);
    }
}
