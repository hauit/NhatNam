using Common.DatabaseExecution;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_Test.Properties;

namespace WPF_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //IDatabaseExecution<SqlConnection> sqlServer = new SQLDatabaseExecution();
            //sqlServer.Server
            //IServiceCollection services = new ServiceCollection();
            ////services.AddScoped<IDatabaseExecution<SqlConnection>, SQLDatabaseExecution>();
            //services.AddTransient<IDatabaseExecution<SqlConnection>> (provider => new SQLDatabaseExecution("alooo", string.Empty, string.Empty, string.Empty));
            //services.AddTransient<IDatabaseExecution<SQLiteConnection>, SQLiteDatabaseExecution>();
            //IServiceProvider serviceProvider = services.BuildServiceProvider();

            //IDatabaseExecution<SqlConnection> serviceA = serviceProvider.GetRequiredService<IDatabaseExecution<SqlConnection>>();
            //serviceA.LoadGridByStr<object>("select * from alooo");

            //var a = new DataExecutionWrapper<SQLiteConnection>(1);
            //a.LoadGridByStr<object>("select * from alooo");
            base.OnStartup(e);
        }
    }
}
