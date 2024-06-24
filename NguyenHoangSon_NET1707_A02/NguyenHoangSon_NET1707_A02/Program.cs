using AutoMapper;
using FHS.BusinessLogic.Services;
using FHS.BusinessLogic.Tools;
using FHS.DataAccess.Contracts;
using FHS.DataAccess.Entities;
using FHS.DataAccess.Entities.InitData;
using FHS.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NguyenHoangSon_NET1707_A02.Hubs;
using NguyenHoangSon_NET1707_A02.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});
builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddDbContext<FuminiHotelManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
    

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBaseRepository<BookingDetail>, BaseRepository<BookingDetail>>();
builder.Services.AddScoped<IBaseRepository<BookingReservation>, BaseRepository<BookingReservation>>();
builder.Services.AddScoped<IBaseRepository<Customer>, BaseRepository<Customer>>();
builder.Services.AddScoped<IBaseRepository<RoomInformation>, BaseRepository<RoomInformation>>();
builder.Services.AddScoped<IBaseRepository<RoomType>, BaseRepository<RoomType>>();

builder.Services.AddScoped<BookingDetailService>();
builder.Services.AddScoped<BookingReservationService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<RoomInformationService>();
builder.Services.AddScoped<RoomTypeService>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

//app.MapGet("/", async context =>
//{
//    string username = context.Session.GetString("Username");

//    if (string.IsNullOrEmpty(username))
//    {
//        context.Response.Redirect("/Auths/Login");
//    }
//    else
//    {
//        context.Response.Redirect("/BookingReservations/Index");
//    }

//    await Task.CompletedTask;
//});

app.MapRazorPages();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use session
app.UseSession();
app.UseAuthenticationMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapHub<SignalRServer>("/signalRServer");
});

//app.MapHub<SignalRServer>("/signalRServer");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<FuminiHotelManagementContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.Run();
