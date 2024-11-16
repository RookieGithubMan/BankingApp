using Banking.UI.EntityFramework.Dao;
using BankingApplication.UI.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Banking.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var configuration = builder.Configuration
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

            var bankingDatabaseConnectionString = configuration.GetConnectionString("BankingDatabaseConnectionString");
            builder.Services.AddDbContext<BankingDBContext>(options => options.UseSqlServer(bankingDatabaseConnectionString));

            builder.Services.AddSingleton(configuration);
            builder.Services.AddScoped<IAccountDao, AccountDao>();
            builder.Services.AddScoped<ITransactionDao, TransactionDao>();
            builder.Services.AddScoped<IInitiateTransactionDao, InitiateTransactionDao>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
