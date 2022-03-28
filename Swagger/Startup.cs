// var builder = WebApplication.CreateBuilder(args);

using System.Reflection;

namespace Swagger;

/// <summary>
/// 
/// </summary>
public class Startup
{
    /// <summary>
    /// 配置文件接口
    /// </summary>
    /// <value></value>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// 运行时将调用此方法。 使用此方法将服务添加到容器。添加中间件
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSwaggerGen(options =>
        {
            // 添加文档信息
            options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "WebApiDemo",
                Version = "v1"
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    /// <summary>
    /// 运行时将调用此方法。 使用此方法来配置HTTP请求管道。
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        //使用静态资源，如css,js文件
        app.UseStaticFiles();
        //路由
        app.UseRouting();
        if (!env.IsProduction())
        {
            // 启用Swagger中间件
            app.UseSwagger();
            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiDemo");
                c.RoutePrefix = string.Empty;
            });
        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}");
        });
    }
}
