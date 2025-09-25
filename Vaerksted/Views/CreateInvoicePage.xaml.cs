using Workshop.ViewModels;

namespace Workshop.Views;

public partial class CreateInvoicePage : ContentPage
{
    public CreateInvoicePage(CreateInvoiceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CreateInvoiceViewModel vm)
            await vm.EnsureDefaultOrderAsync();
    }
}
