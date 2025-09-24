using Workshop.ViewModels;

namespace Workshop.Views;

public partial class CreateInvoicePage : ContentPage
{
    public CreateInvoicePage(CreateInvoiceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
