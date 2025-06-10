using OrderFetcher.Application.Interfaces;
using OrderFetcher.Application.Services;
using OrderFetcher.Infrastructure;
using OrderFetcher.Infrastructure.Repositories;
using OrderFetcher.Infrastructure.Services;
using OrderFetcher.WebUI.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IOrderRepository, MockOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderFileProcessor, OrderFileProcessor>();
builder.Services.AddScoped<IEmailParser, EmailParser>();

var app = builder.Build();

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