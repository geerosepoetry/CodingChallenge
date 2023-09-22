using CodingChallenge.Business;
using CodingChallenge.Contracts;
using System.Text.Json;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Coding Challenge API",
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
});
builder.Services.AddScoped<IListingServices, ListingServices>();
//Setup casing insensitive from the consuming api return object
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(exceptionHandler => exceptionHandler.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("An exception was thrown.");
    }));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
