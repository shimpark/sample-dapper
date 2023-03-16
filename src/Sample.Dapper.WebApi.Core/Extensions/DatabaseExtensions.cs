using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Dapper.WebApi.Core.Extensions;

public class MigrationManager : IStartupFilter
{
    /// <summary>
    /// Extends the provided <paramref name="next" /> and returns an <see cref="T:System.Action" /> of the same type.
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>
    /// A modified <see cref="T:System.Action" />.
    /// </returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using var scope = app.ApplicationServices.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<DatabaseExtensions>();
            databaseService.CreateDatabase("SampleDapper");
            next(app);
        };
    }
}

public class DatabaseExtensions
{
    private readonly IConfiguration _configuration;
    public DatabaseExtensions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// Creates the database.
    /// </summary>
    /// <param name="dbName">Name of the database.</param>
    public void CreateDatabase(string dbName)
    {
        var query = "SELECT * FROM sys.databases WHERE name = @name";
        var parameters = new DynamicParameters();
        parameters.Add("name", dbName);
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        var records = connection.Query(query, parameters);
        if (!records.Any())
        {
            connection.Execute($"CREATE DATABASE {dbName}");
            connection.Execute(@"USE [SampleDapper] 
                                CREATE TABLE [dbo].[User](
	                            [EntityId] [int] IDENTITY(1,1) NOT NULL,
	                            [Name] [varchar](100) NOT NULL,
	                            [Email] [varchar](100) NOT NULL,
	                            [PhoneNumber] [varchar](50) NOT NULL,
	                            [CreatedAt] [datetime2](7) NULL,
	                            [UpdatedAt] [datetime2](7) NULL
                            ) ON [PRIMARY]");
        }
    }
}