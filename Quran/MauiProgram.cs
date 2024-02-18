using CommunityToolkit.Maui;
using Microsoft.Extensions.Caching.Memory;
using Quran.Infrastructure;
using Quran.UseCase;
using Quran.UseCase.Abstraction;
using Quran.ViewModels;
using Quran.Views;

namespace Quran;

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
            });
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder.Services.AddScoped(sp =>
new HttpClient(new HttpClientHandler())
{
    BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/quran-api@1/")
});
        builder.Services.AddMemoryCache();

        builder.Services.AddSingleton<IVerseService, VerseService>();
        builder.Services.AddSingleton<IVerseRepository, VerseRepository>();

        builder.Services.AddSingleton<IChapterRepository, ChapterRepository>();
        builder.Services.AddSingleton<IChapterService, ChapterService>();

        builder.Services.AddSingleton<VersesViewModel>();
        builder.Services.AddSingleton<ChaptersViewModel>();

        builder.Services.AddSingleton<VersesPage>();
        builder.Services.AddSingleton<ChaptersPage>();

        return builder.Build();
    }
}
