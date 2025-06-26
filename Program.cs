using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Extenciones;
using Uttt.Micro.Libro.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger y OpenAPI
builder.Services.AddOpenApi();     // Extensión personalizada
builder.Services.AddSwaggerGen();  // Swagger de Swashbuckle

// Configurar DbContext con cadena de conexión
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS - CORREGIDO para que se llame igual que en UseCors
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Servicios personalizados
builder.Services.AddCustonServices(builder.Configuration);

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi(); // OpenAPI personalizado

// Habilitar CORS correctamente
app.UseCors("PermitirReact");

// Redirección HTTPS (puedes comentarlo si no lo usas en desarrollo)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
