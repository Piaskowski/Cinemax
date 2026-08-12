using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Common;
using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Elements.Identity.Commands;
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Reservations.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Elements.Seats.Repositories;
using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Db.Context;
using Cinemax.Db.Extentions;
using Cinemax.Db.Repositories;
using Cinemax.Db.Seeders;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Server.Extensions.Auth;
using Cinemax.Server.Middlewares;
using Cinemax.Server.Services;
using Cinemax.Shared.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Database Context
builder.Services.AddAppDbContextServices(builder.Configuration);

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Authentication
builder.Services.Configure<AuthSettings>(
    builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddJwtBearerAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// MadiatR Commands and FluentValidation
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(CreateAccountCommand).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Repositories
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICinemaHallRepository, CinemaHallRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IScreeningRepository, ScreeningRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Other Interfaces
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IUnitOfWork>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddTransient<ErrorHandlingMiddleware>();

// Settings
builder.Services.Configure<HomeSettings>(
    builder.Configuration.GetSection("HomeSettings"));

var app = builder.Build();

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

// Seed
using (var scope = app.Services.CreateScope())
{
    await UserRoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Blazor WASM
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
