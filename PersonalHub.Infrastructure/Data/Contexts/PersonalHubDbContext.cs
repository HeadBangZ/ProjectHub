﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonalHub.Domain.User.Entities;
using PersonalHub.Domain.Workspace.Entities;
using PersonalHub.Infrastructure.Data.Configurations;

namespace PersonalHub.Infrastructure.Data.Contexts
{
    public class PersonalHubDbContext : IdentityDbContext<ApiUser>
    {
        private readonly IConfiguration _configuration;

        public PersonalHubDbContext(DbContextOptions<PersonalHubDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<ApiUser> Users => Set<ApiUser>();
        public DbSet<Feature> Features => Set<Feature>();
        public DbSet<Bug> Bugs => Set<Bug>();
        public DbSet<Activity> Activities => Set<Activity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration(_configuration));
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration(_configuration));

            // ApiUser
            modelBuilder.Entity<ApiUser>()
                .ComplexProperty(u => u.Information).IsRequired();

            // Features
            modelBuilder.Entity<Feature>()
                .HasMany(f => f.Activities)
                .WithOne()
                .HasForeignKey(a => a.FeatureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feature>()
                .Property(f => f.Importance)
                .HasConversion<string>();

            // Bugs
            modelBuilder.Entity<Bug>()
                .Property(b => b.Severity)
                .HasConversion<string>();

            // Activites
            modelBuilder.Entity<Activity>()
                .Property(a => a.FeatureId);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonalHubDbContext).Assembly);
        }
    }
}


