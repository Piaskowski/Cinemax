
using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Notifications.Commands;
using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Application.Elements.Orders.Commands;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Reservations.Repositories;
using Cinemax.Application.Elements.Screenings.Commands;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Elements.Seats.Repositories;
using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Db.Context;
using Cinemax.Db.Extentions;
using Cinemax.Db.Repositories;
using Cinemax.Db.Repositories.Notifications;
using Cinemax.Mailing.MailKit;
using Cinemax.Scheduler.Quartz;
using Cinemax.Shared.Settings;
using Cinemax.Worker.Services;
using MediatR;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddAppDbContextServices(config);
        services.AddScoped<IUnitOfWork>(provider =>
                provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<ICinemaHallRepository, CinemaHallRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<IScreeningRepository, ScreeningRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddTransient<IEmailSender, MailKitEmailSender>();
        services.AddScoped<IEmailNotificationRepository, EmailNotificationRepository>();
        services.AddScoped<IEmailMessageRepository, EmailMessageRepository>();
        services.AddTransient<ICurrentUserService, WorkerCurrentUserService>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        services.AddTransient<IRequestHandler<SendPendingEmailNotificationsCommand>, SendPendingEmailNotificationsCommandHandler>();
        services.AddTransient<IRequestHandler<UpdateOrdersStatusCommand>, UpdateOrdersStatusCommandHandler>();
        services.AddTransient<IRequestHandler<UpdateScreeningsStatusCommand>, UpdateScreeningsStatusCommandHandler>();

        //services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UpdateScreeningsStatusCommand).Assembly));
        services.AddQuartzService(config);

        services.Configure<MailingSettings>(
            config.GetSection("MailingSettings"));

    }).Build();

host.Run();
