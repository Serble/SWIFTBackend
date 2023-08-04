using GeneralPurposeLib;
using SwiftBackend.Storage;
using SwiftBackend.Storage.MySQL;
using LogLevel = GeneralPurposeLib.LogLevel;

namespace SwiftBackend; 

internal static class Program {
    public const bool AllowTestUser = false;
    public static IStorageManager StorageManager = null!;  // Will never be null when accessed
    public const string Version = "0.0.1";
    
    public static void Main(string[] args) {
        Logger.Init(LogLevel.Debug);

        Config config = new(DefaultConfig.Config);
        GlobalConfig.Init(config);
        
        StorageManager = new MySqlStorage();
        
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}