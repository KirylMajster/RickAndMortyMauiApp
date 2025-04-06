using Microsoft.Extensions.Logging;
using RickAndMortyMauiApp.Data;

namespace RickAndMortyMauiApp
{
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

           
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }

            return app;
        }
    }
}
