using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Modelo;
using Uttt.Micro.Libro.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Uttt.Micro.Libro.Aplicacion
{
    public class ConsultaPorCriterio
    {
        public class PorAutor : IRequest<List<LibroMaterialDto>>
        {
            public Guid AutorId { get; set; }
        }

        public class PorTitulo : IRequest<List<LibroMaterialDto>>
        {
            public string Titulo { get; set; }
        }

        public class Manejador :
            IRequestHandler<PorAutor, List<LibroMaterialDto>>,
            IRequestHandler<PorTitulo, List<LibroMaterialDto>>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<LibroMaterialDto>> Handle(PorAutor request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriasMateriales
                    .Where(x => x.AutorLibro == request.AutorId)
                    .ToListAsync();

                return _mapper.Map<List<LibreriaMaterial>, List<LibroMaterialDto>>(libros);
            }

            public async Task<List<LibroMaterialDto>> Handle(PorTitulo request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriasMateriales
                    .Where(x => x.Titulo.ToLower().Contains(request.Titulo.ToLower()))
                    .ToListAsync();

                return _mapper.Map<List<LibreriaMaterial>, List<LibroMaterialDto>>(libros);
            }
        }
    }
}
