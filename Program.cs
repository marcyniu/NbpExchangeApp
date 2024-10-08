using NbpExchangeApp.Components;
using NbpExchangeApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<NbpApiService>();
builder.Services.AddMemoryCache();

// Register your services here
builder.Services.AddScoped<NbpExchangeRatesService>();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
