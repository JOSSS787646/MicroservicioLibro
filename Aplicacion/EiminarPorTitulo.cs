using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uttt.Micro.Libro.Persistencia;

namespace Uttt.Micro.Libro.Aplicacion
{
    public class EliminarPorTitulo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriasMateriales
                    .Where(x => x.Titulo.ToLower() == request.Titulo.ToLower())
                    .ToListAsync();

                if (libros == null || libros.Count == 0)
                {
                    throw new Exception("No se encontraron libros con ese título");
                }

                _contexto.LibreriasMateriales.RemoveRange(libros);
                var result = await _contexto.SaveChangesAsync();

                if (result > 0) return Unit.Value;

                throw new Exception("No se pudo eliminar el/los libros");
            }
        }
    }
}
