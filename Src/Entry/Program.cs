using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Base.DataBaseAndIdentity.DBContext;
using Entry.Registry;
using Microsoft.IdentityModel.JsonWebTokens;

Console.OutputEncoding = Encoding.UTF8;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var configuration = builder.Configuration;

service.RegisterRequireServices(configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var connect = await context.Database.CanConnectAsync();
    if (!connect)
    {
        throw new Exception("Can not connect to database !!");
    }
    Console.WriteLine("Connected to database !!");
}

// Configure the HTTP request pipeline.
app.UseRouting()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseOpenApi()
    .UseSwaggerUi(options =>
    {
        options.Path = string.Empty;
        options.DefaultModelsExpandDepth = -1;
    });

app.MapControllers();

await app.RunAsync(CancellationToken.None);
