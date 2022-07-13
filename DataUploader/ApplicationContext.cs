using DataUploader.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NLog;
using Microsoft.EntityFrameworkCore;

namespace DataUploader
{
    internal class ApplicationContext : DbContext 
    {
        private DbSet<Product> Products { get; set; } = null!;
        private DbSet<Category> Categories { get; set; } = null!;

        private string? dbHost;
        private string? dbName;
        private string? dbUsername;
        private string? dbPassword;

        private bool useTrustedConnection = false;

        internal ApplicationContext() 
        {
            SetConfigurationStrings();

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString;
            if (useTrustedConnection)
            {
                connectionString = $"Server={dbHost};Database={dbName};Trusted_Connection=True";
            }
            else 
            {
                connectionString = $"Server={dbHost};Database={dbName};User Id={dbUsername}; Password={dbPassword}";
            }
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        private void SetConfigurationStrings() 
        {
            dbHost = ConfigurationManager.AppSettings.Get("DataBaseHost");
            dbName = ConfigurationManager.AppSettings.Get("DataBaseName");
            dbUsername = ConfigurationManager.AppSettings.Get("DataBaseUserName");
            dbPassword = ConfigurationManager.AppSettings.Get("DataBasePassword");
            useTrustedConnection = bool.Parse(ConfigurationManager.AppSettings.Get("UseTrustedConnection"));
        }
    }  
}
