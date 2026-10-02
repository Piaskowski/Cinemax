using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Db.Extentions;
using Cinemax.Domain.Common;
using Cinemax.Domain.Entities;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Domain.Entities.Notifications;
using Cinemax.Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cinemax.Db.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options,
        ICurrentUserService currentUserService) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IUnitOfWork
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TicketPrice> TicketPrices { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<EmailNotificationResource> EmailNotificationsResources { get; set; }
        public DbSet<MessageTemplate> MessageTemplates { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.ConfigureIdentityTables();
            mb.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is not null)
                return _currentTransaction;

            _currentTransaction = await Database.BeginTransactionAsync(ct);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is null)
                return;

            try
            {
                await SaveChangesAsync(ct);
                await _currentTransaction.CommitAsync(ct);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is null)
                return;

            try
            {
                await _currentTransaction.RollbackAsync(ct);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var email = _currentUserService.Email;

            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added && entry.Entity is IAuditable added)
                {
                    added.CreatedAt = now;
                    added.ModifiedAt = now;
                    added.ModifiedBy = email;
                }else if (entry.State == EntityState.Modified && entry.Entity is IAuditable modified)
                {
                    modified.ModifiedAt = now;
                    modified.ModifiedBy = email;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
