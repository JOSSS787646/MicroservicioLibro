using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Extenciones;
using Uttt.Micro.Libro.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger y OpenAPI
builder.Services.AddOpenApi();     // Extensi�n personalizada
builder.Services.AddSwaggerGen();  // Swagger de Swashbuckle

// ?? Cadena de conexi�n directamente en el c�digo
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS para permitir TODO
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Servicios personalizados (como validadores, etc.)
builder.Services.AddCustonServices(builder.Configuration);

var app = builder.Build();

// Swagger solo en entorno de desarrollo
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi(); // OpenAPI personalizado
//}

// Habilitar CORS
app.UseCors("PermitirReact");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();