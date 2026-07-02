using someCrud.DI.repositories.memory;
using someCrud.DI.repositories;
using someCrud.domain.services;

namespace someCrud.bootstrap.rpc;

public class AppWrapper
{
    private static AppWrapper? _instance = null;

    public static AppWrapper instance
    {
        get { 
            if (_instance == null) throw new Exception("Not initialized.");
            return _instance; 
        }
    }

    public readonly WebApplication app;

    private AppWrapper(WebApplication app)
    {
        this.app = app;
    }

    public static void init(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        builder.Services.AddScoped<NoteService>();
        builder.Services.AddScoped<INoteRepository, InMemoryNoteRepository>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddLogging(builder => builder.AddConsole());


        WebApplication app = builder.Build();
        app.MapControllers();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        _instance = new AppWrapper(app);
    }
}