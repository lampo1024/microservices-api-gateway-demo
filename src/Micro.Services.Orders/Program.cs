using Micro.Infrastructure.ServiceDiscovery;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsul(builder.Configuration.GetServiceConfig());
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/cart/add", () => "Hello from order service--cart add.");
app.MapGet("api/cart/list", () => "Hello from order service--cart list.");
app.MapGet("api/cart/delete/{id}", (int id) => $"Hello from order service--cart delete {id}.");
app.MapGet("api/health_check", () => Results.Ok("OK"));
app.MapGet("api/cart/upload", async (HttpRequest req) =>
{
    if (!req.HasFormContentType)
    {
        return Results.Ok(new { success = false, message = "Upload failed." });
    }

    var form = await req.ReadFormAsync();
    var file = form.Files["file"];
    if (file is null)
    {
        return Results.Ok(new { success = false, message = "Upload failed." });
    }

    var uploads = Path.Combine(@"d:\uploads", file.FileName);
    await using var fileStream = File.OpenWrite(uploads);
    await using var uploadStream = file.OpenReadStream();
    await uploadStream.CopyToAsync(fileStream);
    return Results.Ok(new
    {
        success = true,
        message = "Upload successful."
    });
}).Accepts<IFormFile>("multipart/form-data");
app.Run();