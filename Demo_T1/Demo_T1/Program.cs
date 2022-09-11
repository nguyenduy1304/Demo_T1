using Demo_T1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.FileProviders;
using Demo_T1.Models;

var builder = WebApplication.CreateBuilder(args);

//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    Args = args,
//    // Examine Hosting environment: logging value
//    EnvironmentName = Environments.Staging,
//    WebRootPath = "wwwroot-custom"
//});


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();

builder.Services.Configure<PositionOptions>(
    builder.Configuration.GetSection(PositionOptions.Position));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ITransientService, SomeService>();
builder.Services.AddScoped<IScopedService, SomeService>();
builder.Services.AddSingleton<ISingletonService, SomeService>();

builder.Services.AddDirectoryBrowser();

builder.Services.AddHttpClient();

//////Static file authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


///////Exception handler lambda

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler(exceptionHandlerApp =>
//    {
//        exceptionHandlerApp.Run(async context =>
//        {
//            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

//            // using static System.Net.Mime.MediaTypeNames;
//            context.Response.ContentType = Text.Plain;

//            await context.Response.WriteAsync("An exception was thrown.");

//            var exceptionHandlerPathFeature =
//                context.Features.Get<IExceptionHandlerPathFeature>();

//            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
//            {
//                await context.Response.WriteAsync(" The file was not found.");
//            }

//            if (exceptionHandlerPathFeature?.Path == "/")
//            {
//                await context.Response.WriteAsync(" Page: Home.");
//            }
//        });
//    });

//    app.UseHsts();
//}

app.UseStatusCodePages(async statusCodeContext =>
{
    // using static System.Net.Mime.MediaTypeNames;
    statusCodeContext.HttpContext.Response.ContentType = Text.Plain;

    await statusCodeContext.HttpContext.Response.WriteAsync(
        $"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

//var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "images"));
//var requestPath = "/MyImages";

// Enable displaying browser links.
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = fileProvider,
//    RequestPath = requestPath
//});

//app.UseDirectoryBrowser(new DirectoryBrowserOptions
//{
//    FileProvider = fileProvider,
//    RequestPath = requestPath
//});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();