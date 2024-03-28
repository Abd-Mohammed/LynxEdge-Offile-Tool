using Application;
using Infrastructure;
using LynxEdge.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration)
	.AddControllersWithViews();

builder.WebHost.ConfigureKestrel(cfg =>
{
	cfg.Limits.MaxRequestHeadersTotalSize = 40960;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<LynxEdgeMiddleware>();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Profiles}/{action=Index}/{id?}");

app.Run();


// configure/ response-compression / handling error page/ clean the smell of code. 
  