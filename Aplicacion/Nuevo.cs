using FluentValidation;
using MediatR;
using Uttt.Micro.Libro.Modelo;
using Uttt.Micro.Libro.Persistencia;
using System.Threading;
using System.Threading.Tasks;

namespace Uttt.Micro.Libro.Aplicacion
{
    // ---------------------------
    // CREAR
    // ---------------------------
    public class Nuevo
    {
        public class CrearLibro : IRequest
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class CrearLibroValidacion : AbstractValidator<CrearLibro>
        {
            public CrearLibroValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<CrearLibro>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(CrearLibro request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                _contexto.LibreriasMateriales.Add(libro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar el libro");
            }
        }
    }

    // ---------------------------
    // EDITAR
    // ---------------------------
    public class Editar
    {
        public class EditarLibro : IRequest
        {
            public Guid LibroId { get; set; }
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class EditarLibroValidacion : AbstractValidator<EditarLibro>
        {
            public EditarLibroValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<EditarLibro>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(EditarLibro request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriasMateriales.FindAsync(request.LibroId);
                if (libro == null)
                {
                    throw new Exception("No se encontró el libro");
                }

                libro.Titulo = request.Titulo ?? libro.Titulo;
                libro.FechaPublicacion = request.FechaPublicacion ?? libro.FechaPublicacion;
                libro.AutorLibro = request.AutorLibro ?? libro.AutorLibro;

                var resultado = await _contexto.SaveChangesAsync();
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }

    // ---------------------------
    // ELIMINAR
    // ---------------------------
    public class Eliminar
    {
        public class EliminarLibro : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<EliminarLibro>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(EliminarLibro request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriasMateriales.FindAsync(request.Id);
                if (libro == null)
                {
                    throw new Exception("No se encontró el libro");
                }

                _contexto.Remove(libro);
                var resultado = await _contexto.SaveChangesAsync();
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el libro");
            }
        }
    }
}
