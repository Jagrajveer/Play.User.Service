using Play.Common.Service.MassTransit;
using Play.Common.Service.MongoDb;
using Play.User.Service.Entities;

var builder = WebApplication.CreateBuilder(args);

const string AllowedOriginSettings = "AllowedHost";

// Add services to the container.

builder.Services
    .AddMongo()
    .AddMongoRepository<User>("users");
builder.Services.AddMassTransitWithRabbitMq();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyBuilder =>
{
    policyBuilder.WithOrigins(builder.Configuration[AllowedOriginSettings]);
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
