using Azure.Storage.Blobs;
using BeardMan.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Management.Storage.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BeardMan
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AutoRentContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("AutoRent")));
            services.AddTransient<BranchesLogic>();
            services.AddTransient<CarsLogic>();
            services.AddTransient<CarTypesLogic>();
            services.AddTransient<HomeLogic>();
            services.AddTransient<RentsLogic>();
            services.AddTransient<UsersLogic>();
            services.AddTransient<BlobsLogic>();
            services.AddTransient<AutoRentContext>();

            JwtHelper jwtHelper = new JwtHelper(Configuration.GetValue<string>("JWT:key"));
            services.AddSingleton(jwtHelper);

            services.AddSingleton(x =>
             new BlobServiceClient(connectionString: Configuration.GetValue<string>(key: "Azure")));

            services.AddSingleton<IBlobService, BlobService>();

            services.AddAuthentication(options => jwtHelper.SetAuthenticaionOptions(options))
                .AddJwtBearer(options => jwtHelper.SetBearerOptions(options));

            services.AddControllers();

            services.AddCors(setup => setup.AddPolicy("EntireWorld", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddCors(setup => setup.AddPolicy("LocalhostDevelopment", policy => policy.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:5000").AllowAnyMethod().AllowAnyHeader()));
            //services.AddCors(setup => setup.AddPolicy("BeardMan", policy => policy.WithOrigins("https://www.baerdmanwebsites").AllowAnyMethod().AllowAnyHeader()));

            services.AddSpaStaticFiles(config => config.RootPath = "wwwroot");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("EntireWorld");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles();
            app.UseSpa(config => config.Options.SourcePath = "wwwroot");
        }
    }
}
