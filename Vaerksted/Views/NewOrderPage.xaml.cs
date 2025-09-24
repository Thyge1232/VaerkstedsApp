using Workshop.ViewModels;

namespace Workshop.Views;

public partial class NewOrderPage : ContentPage
{
    public NewOrderPage(NewOrderViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
