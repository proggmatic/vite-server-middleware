using Proggmatic.SpaServices.Vite;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSpaStaticFiles(); //-- new addition --//

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


//-- new addition --//
app.UseSpa(spa =>
{
    // spa.Options.SourcePath = "ClientApp";          // Optional. If this string is commented, "ClientApp" will be used
    spa.Options.PackageManagerCommand = "pnpm";    // Optional. If this string is commented, "npm" will be used. You may use Pnpm or Yarn instead of Npm.

    if (app.Environment.IsDevelopment())
    {
        spa.UseViteServer();

        // Or to build not by starting this application but manually uncomment next lines and comment line above
        // spa.ApplicationBuilder.UseFixSpaPathBaseBugMiddleware();     // Uncomment this, if you want to use non-root url for proxying (like http://localhost:8080/my-custom-path)
        // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
    }
});


app.Run();