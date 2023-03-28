using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Repositories;
using AcmeTube.Application.Services;
using AcmeTube.Data.Repositories;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Internal;
using Npgsql;

namespace AcmeTube.Data.Contexts
{
    public sealed class MainContext : DbContext, IUnitOfWork, IHealthCheck
    {
        private readonly IIdentityService _identityService;
        private readonly ISystemClock _systemClock;

        private IUserRepository _userRepository;
        private IChannelRepository _channelRepository;
        private IVideoRepository _videoRepository;

        public MainContext(DbContextOptions<MainContext> options, IIdentityService identityService, ISystemClock systemClock)
            : base(options)
        {
            // Database.Log = (sql) => Debug.Write(sql);
            //Configuration.LazyLoadingEnabled = true;
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.AutoDetectChangesEnabled = true;

            _identityService = identityService;
            _systemClock = systemClock;
        }


        public IUserRepository UserRepository => _userRepository ??= new UserRepository(this);

        public IChannelRepository ChannelRepository => _channelRepository ??= new ChannelRepository(this);

        public IVideoRepository VideoRepository => _videoRepository ??= new VideoRepository(this);

        public IDbConnection CreateConnection() => new NpgsqlConnection(base.Database.GetDbConnection().ConnectionString);
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTimeOffset>()
                .HaveColumnType("timestamptz");

            base.ConfigureConventions(configurationBuilder);
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) { }

        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            var identity = _identityService.GetIdentity();

            foreach (var entityEntry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        // this.Entry(entityEntry).Property("CreatedAt").CurrentValue = _systemClock.UtcNow;

                        entityEntry.Entity.CreatedAt = _systemClock.UtcNow;
                        
                        if (identity.IsAuthenticated)
                        {
                            entityEntry.Entity.CreatedBy = identity.Id;
                        }
                        
                        break;
                    case EntityState.Modified:
                        entityEntry.Entity.UpdatedAt = _systemClock.UtcNow;
                        
                        if (identity.IsAuthenticated)
                        {
                            entityEntry.Entity.UpdatedBy = identity.Id;
                        }
                        
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public void RollbackTransaction() { }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                return await this.Database.CanConnectAsync(cancellationToken)
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Unhealthy();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
