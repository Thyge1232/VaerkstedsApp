using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Workshop.Models;
using Workshop.Data;

namespace Workshop.ViewModels;

public partial class NewOrderViewModel : ObservableObject
{
    private readonly AppDatabase _db;

    public NewOrderViewModel(AppDatabase db)
    {
        _db = db;
    }

    [ObservableProperty] private string customerName = string.Empty;
    [ObservableProperty] private string customerAddress = string.Empty;
    [ObservableProperty] private string carMake = string.Empty;
    [ObservableProperty] private string carModel = string.Empty;
    [ObservableProperty] private string plateNumber = string.Empty;
    [ObservableProperty] private DateTime dropOffDate = DateTime.Today; 
    [ObservableProperty] private TimeSpan dropOffTime = new(9, 0, 0); 
    [ObservableProperty] private string workDescription = string.Empty;

    [RelayCommand]
    private async Task SaveOrderAsync()
    {
        var order = new Order
        {
            CustomerName = CustomerName,
            CustomerAddress = CustomerAddress,
            CarMake = CarMake,
            CarModel = CarModel,
            PlateNumber = PlateNumber,
            DropOffDateTime = DropOffDate + DropOffTime,
            WorkDescription = WorkDescription
        };

        await _db.Conn.InsertAsync(order);
    }
}