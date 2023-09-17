using TlpArchitectureCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb") ??
    throw new NullReferenceException("MongoDb connection string is not set");

builder.Services.AddMongoDb(mongoDbConnectionString);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
