using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config
        .SetBasePath(context.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddJsonFile("ocelot.json", false, true)
        .AddEnvironmentVariables();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddOcelot(builder.Configuration)
    .AddConsul()
    .AddConfigStoredInConsul()
    //.AddCacheManager(x =>
    //{
    //    x.WithDictionaryHandle();
    //})
    .AddPolly();

var app = builder.Build();
app.UseOcelot().Wait();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();