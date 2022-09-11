using Middleware.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseMiddleware<SimpleMiddleware>();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("<div> Hello World from the middleware 1 </div>");
    await next.Invoke();
    await context.Response.WriteAsync("<div> Returning from the middleware 1 </div>");
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("<div> Hello World from the middleware 2 </div>");
    await next.Invoke();
    await context.Response.WriteAsync("<div> Returning from the middleware 2 </div>");
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("<div> Hello World from the middleware 3 </div>");
});

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

app.MapGet("/hello/{name:alpha}", (string name) => $"Hello {name}!");
app.Run();
