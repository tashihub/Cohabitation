using Cohabitation.Models;
using Cohabitation.Repositories;
using Microsoft.Extensions.Logging;

namespace Cohabitation;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("LibreFranklin-Regular.ttf", "Strong");
                fonts.AddFont("Roboto-Black.ttf", "Regular");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<SettingRepository>();
        builder.Services.AddSingleton<TransactionRepository>();

        return builder.Build();
	}
}
