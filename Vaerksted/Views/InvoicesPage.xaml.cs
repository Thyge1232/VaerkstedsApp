using Workshop.ViewModels;

namespace Workshop.Views;

public partial class InvoicesPage : ContentPage
{
    private readonly InvoicesViewModel _vm;

    public InvoicesPage(InvoicesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm == null) return;
        await _vm.LoadAsync();
    }
}
