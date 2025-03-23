using Cohabitation.Models;
using Cohabitation.Repositories;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Cohabitation;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseSkiaSharp() //グラフ作成用
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
        builder.UseLiveCharts(); // ここが大事！
        return builder.Build();
	}
}
