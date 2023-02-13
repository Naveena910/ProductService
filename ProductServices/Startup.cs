using Contracts.IRepository;
using Contracts.IServicees;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Services;
using Services.Client;
using Services.Helpers;
using System.Text;

namespace ProductServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) 
        {

           services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
         //  services.AddScoped(IGenericRepository, (GenericRepository));
        services.AddHttpClient();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartRepository,CartRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IWishListRepository, WishListRepository>();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddScoped<UserClient>();
            services.AddAutoMapper(typeof(Mappers));
            services.AddHttpClient();
          /*  services.AddHttpClient<UserClient>(a =>
            {
                a.BaseAddress = new Uri("http://localhost:5170");
            });*/


            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer    (Configuration.GetConnectionString("ProductDb")));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                /* app.UseExceptionHandler(appBuilder =>
                 {
                     appBuilder.Run(async c =>
                     {
                         c.Response.StatusCode = 500;
                         await c.Response.WriteAsync("Something went wrong, please try again later!");
                     });
                 });
             }*/

                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs");
                });
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}
