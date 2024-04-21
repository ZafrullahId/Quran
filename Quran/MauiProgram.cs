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
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Lateef-Bold.ttf", "LateefBold");
                fonts.AddFont("Lateef-ExtraBold.ttf", "LateefExtraBold");
                fonts.AddFont("Lateef-ExtraLight.ttf", "LateefExtraLight");
                fonts.AddFont("Lateef-Light.ttf", "LateefLight");
                fonts.AddFont("Lateef-Medium.ttf", "LateefMedium");
                fonts.AddFont("Lateef-Regular.ttf", "LateefRegular");
                fonts.AddFont("Lateef-SemiBold.ttf", "LateefSemiBold");
                fonts.AddFont("Amiri-Bold.ttf", "AmiriBold");
                fonts.AddFont("Amiri-BoldItalic.ttf", "AmiriBoldItalic");
                fonts.AddFont("Amiri-Italic.ttf", "AmiriItalic");
                fonts.AddFont("Amiri-Regular.ttf", "AmiriRegular");
                fonts.AddFont("AlmendraSC-Regular.ttf", "AlmendraSCRegular");
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
        builder.Services.AddSingleton<IAudioService, AudioService>();

        builder.Services.AddSingleton<VersesViewModel>();
        builder.Services.AddSingleton<ChapterViewModel>();
        builder.Services.AddSingleton<ChapterAudioPlayerViewModel>();
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<SearchViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<SearchPage>();
        builder.Services.AddSingleton<VersesPage>();
        builder.Services.AddTransient<ChaptersPage>();
        builder.Services.AddSingleton<ChapterAudioPlayerPage>();

        return builder.Build();
    }
}
