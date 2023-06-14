using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BackEnd.Models.Context;
using BackEnd.Services;
using BackEnd.Repositories;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllersWithViews();
                    services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("UserList"));
                    services.AddDbContext<PostContext>(opt => opt.UseInMemoryDatabase("PostList"));
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddSwaggerGen();
                });

                webBuilder.Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI(options =>
                            {
                                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                                options.RoutePrefix = string.Empty; // Set the route prefix to an empty string
                            });
                        }
                    }

                    app.UseHttpsRedirection();
                    app.UseRouting();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                });
            })
            .Build();

        host.Run();
    }
}