using someCrud.DI.repositories;
using someCrud.domain.services;
using Microsoft.EntityFrameworkCore;
using someCrud.configuration;
using someCrud.DI.repositories.sql;
using Mapster;
using someCrud.domain.models;
using someCrud.DI.models;
using MapsterMapper;

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
        builder.Services.AddScoped<INoteRepository, SqlNoteRepository>();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(
                builder.Configuration.GetConnectionString("database")
            );
        });

        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Note, NoteEntity>();
        config.NewConfig<NoteEntity, Note>();

        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IMapper, ServiceMapper>();

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

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        _instance = new AppWrapper(app);
    }
}