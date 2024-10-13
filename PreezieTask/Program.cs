using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container, including controllers
builder.Services.AddControllers();

// Add CORS policy to allow requests from the React frontend (running on http://localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Allow frontend to access
               .AllowAnyHeader()
               .AllowAnyMethod(); // Allow all headers and methods
    });
});

// Register Swagger services for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register TodoService as a singleton to manage todo-related logic and data
builder.Services.AddSingleton<ITodoService, TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Enable Swagger middleware only in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
        c.RoutePrefix = string.Empty; // Swagger will be available at the root (http://localhost:5000)
    });
}

app.UseRouting();
app.UseCors(); // Enable CORS

app.MapControllers();

app.Run();
