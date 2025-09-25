using Workshop.ViewModels;

namespace Workshop.Views;

public partial class CalendarPage : ContentPage
{
    private readonly CalendarViewModel _vm;

    public CalendarPage(CalendarViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm != null)
        {
            await _vm.LoadForDateAsync();
        }
    }
}
