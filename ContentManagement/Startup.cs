//using ContentManagement.Extension;
//using ContentManagement.Extensions;
//using Infrastructure.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ContentManagement
//{
//    public class Startup
//    {
//        private readonly IConfiguration _configuration;

//        public Startup(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            #region Api Versioning
//            // Add API Versioning to the Project
//            services.AddApiVersioning(config =>
//            {
//                // Specify the default API Version as 1.0
//                config.DefaultApiVersion = new ApiVersion(1, 0);
//                // If the client hasn't specified the API version in the request, use the default API version number 
//                config.AssumeDefaultVersionWhenUnspecified = true;
//                // Advertise the API versions supported for the particular endpoint
//                config.ReportApiVersions = true;
//            });

//            services.AddVersionedApiExplorer(options =>
//            {
//                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
//                // note: the specified format code will format the version as "'v'major[.minor][-status]"  
//                options.GroupNameFormat = "'v'VVV";

//                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
//                // can also be used to control the format of the API version in route templates  
//                options.SubstituteApiVersionInUrl = true;
//            });
//            #endregion

//            #region Azure Cache
//            services.AddStackExchangeRedisCache(opt =>
//            {
//                opt.InstanceName = string.Empty;
//                opt.Configuration = _configuration.GetConnectionString("azureCache");
//            });
//            #endregion
//            services.AddCors(setup =>
//            {
//                setup.AddPolicy("cors_policy", policy =>
//                {
//                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//                });
//            });
//            services.AddControllers();
//            services.AddSwaggerService();
//            services.AddDbContext<RailDbContext>(builder => builder.UseSqlServer(_configuration.GetConnectionString("rail")));
//            services.ConfigureApplicationService(_configuration);
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

//            app.UseSwaggerService();

//            app.UseHttpsRedirection();

//            app.UseCors("cors_policy");

//            app.UseRouting();

//            app.UseAuthorization();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }
//}
