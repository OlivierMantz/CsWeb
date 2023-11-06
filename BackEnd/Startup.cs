//using System.IO;
//using BackEnd.Data;
//using BackEnd.Repositories;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using BackEnd.Data;

//namespace BackEnd
//{
//    public class Startup
//    {
//        public IConfiguration Configuration { get; }
//        // private readonly IWebHostEnvironment _env;

//        public Startup(IConfiguration configuration, IWebHostEnvironment env)
//        {
//            Configuration = configuration;
//            // _env = env;
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("InMem"));
//            services.AddDbContext<PostContext>(opt => opt.UseInMemoryDatabase("InMem"));
//            services.AddScoped<IUserRepository, UserRepository>();
            
//            services.AddControllers();
//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppService", Version = "V1" });
//            });
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                app.UseSwagger();
//                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
                
//                app.UseRouting();

//                app.UseAuthorization();

//                app.UseEndpoints(endpoints =>
//                {
//                    endpoints.MapControllers();
//                    // endpoints.MapGrpcService<GrpcPlatformService>();

//                    // endpoints.MapGet("/protos/platforms.proto", async context =>
//                    // {
//                    //     await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
//                    //     
//                    // });
//                });


//                PrepDb.PrepPopulation(app);
//            }
//        }
//    }
//}