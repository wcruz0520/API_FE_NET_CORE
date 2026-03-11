using API_FACTURACION_NET_CORE.Application.Services;
using API_FACTURACION_NET_CORE.Application.Services.Invoices;
using API_FACTURACION_NET_CORE.Infrastructure.Data;
using API_FACTURACION_NET_CORE.Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddHostedService<InvoiceProcessorWorker>();
builder.Services.AddScoped<InvoiceValidationService>();
builder.Services.AddScoped<InvoiceXmlGeneratorService>();
builder.Services.AddScoped<InvoiceXmlSignerService>();
builder.Services.Configure<SriServiceSettings>(builder.Configuration.GetSection("SRI"));
builder.Services.AddHttpClient<SriClientService>();


builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// esta linea es para crear la clave hasheada
//var hash = BCrypt.Net.BCrypt.HashPassword("admin123");
//Console.WriteLine(hash);

app.Run();
