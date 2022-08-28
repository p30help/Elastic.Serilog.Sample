using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Before run project don't forget to run command [docker-compose up] in cmd

// Serilog services
builder.Host.UseSerilog((ctx, config) =>
{
    config.Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        IndexFormat = $"sample-serilog-logs-{ctx.HostingEnvironment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.Now.ToString("yyyy-MM")}",
        AutoRegisterTemplate = true,
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
    .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
    .ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddControllers();
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

// Track all requests and responses in serilog
//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
