using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui;
using Workshop.Data;
using Workshop.ViewModels;
using Workshop.Views;

namespace Workshop;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
			
        // --- DI: Database ---
        builder.Services.AddSingleton<AppDatabase>(sp =>
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "workshop.db3");
            var db = new AppDatabase(dbPath);
            db.InitializeAsync().Wait(); // Initialize database ved opstart
            return db;
        });

        // --- DI: ViewModels ---
        builder.Services.AddTransient<NewOrderViewModel>();
        builder.Services.AddTransient<CalendarViewModel>();
        builder.Services.AddTransient<CreateInvoiceViewModel>();
        builder.Services.AddTransient<InvoicesViewModel>();

        // --- DI: Pages ---
        builder.Services.AddTransient<NewOrderPage>();
        builder.Services.AddTransient<CalendarPage>();
        builder.Services.AddTransient<CreateInvoicePage>();
        builder.Services.AddTransient<InvoicesPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}