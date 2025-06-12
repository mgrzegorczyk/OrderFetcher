using Microsoft.EntityFrameworkCore;
using OrderFetcher.Application.Interfaces;
using OrderFetcher.Application.Services;
using OrderFetcher.Infrastructure;
using OrderFetcher.Infrastructure.Repositories;
using OrderFetcher.Infrastructure.Services;
using OrderFetcher.WebUI.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderFileProcessor, OrderFileProcessor>();
builder.Services.AddScoped<IEmailParser, EmailParser>();
builder.Services.AddScoped<IOrderGPTMapper, OrderGPTMapper>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();