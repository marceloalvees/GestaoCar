using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebInterface
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add application services and repositories
            services.AddControllersWithViews();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IDealershipRepository, DealershipRepository>();
            services.AddScoped<IDealershipService, DealershipService>();
            services.AddScoped<ICepService, CepService>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IExcelService, ExcelService>();

            // Add Entity Framework DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add Identity


            // Configure Cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
