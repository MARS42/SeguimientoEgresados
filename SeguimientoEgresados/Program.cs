using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Filters;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Services;

const string policy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(services =>
{
    services.Filters.Add(new VerificarSesion());
});

builder.Services.AddDbContext<SeguimientoEgresadosContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SegegContext"));
});

builder.Services.AddScoped<IGoogleSheetsService, GoogleSheetsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policy, poliBuilder =>
    {
        //poliBuilder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").
        poliBuilder.SetIsOriginAllowed(origin => new Uri(origin).Host == "*").
            AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseCors(policy);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Inicio/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");*/

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name : "areas",
        pattern : "{area:exists}/{controller=Inicio}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Inicio}/{action=Index}"
    );
});

app.Run();