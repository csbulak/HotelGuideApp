using Report.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ istemci yapýlandýrmasý
builder.Services.AddSingleton<RabbitMqService>(provider =>
{
    var hostName = builder.Configuration["RabbitMQ:HostName"];
    var reportQueueName = builder.Configuration["RabbitMQ:ReportQueueName"];
    var reportStatusQueueName = builder.Configuration["RabbitMQ:ReportStatusQueueName"];

    return new RabbitMqService(hostName, reportQueueName, reportStatusQueueName);
});

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
