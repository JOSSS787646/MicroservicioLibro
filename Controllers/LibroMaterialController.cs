using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uttt.Micro.Libro.Aplicacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uttt.Micro.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibroMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/LibroMaterial
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.CrearLibro data)
        {
            return await _mediator.Send(data);
        }

        // GET: api/LibroMaterial
        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        // GET: api/LibroMaterial/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibroUnico(Guid id)
        {
            return await _mediator.Send(new ConsultaFiltro.LibroUnico
            {
                LibroId = id
            });
        }

        // GET: api/LibroMaterial/autor/{autorId}
        [HttpGet("autor/{autorId}")]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetPorAutor(Guid autorId)
        {
            return await _mediator.Send(new ConsultaPorCriterio.PorAutor { AutorId = autorId });
        }

        // GET: api/LibroMaterial/titulo/{titulo}
        [HttpGet("titulo/{titulo}")]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetPorTitulo(string titulo)
        {
            return await _mediator.Send(new ConsultaPorCriterio.PorTitulo { Titulo = titulo });
        }

        // PUT: api/LibroMaterial/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.EditarLibro data)
        {
            data.LibroId = id;
            return await _mediator.Send(data);
        }

        // DELETE: api/LibroMaterial/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await _mediator.Send(new Eliminar.EliminarLibro { Id = id });
        }

        // DELETE: api/LibroMaterial/titulo/{titulo}
        [HttpDelete("titulo/{titulo}")]
        public async Task<ActionResult<Unit>> EliminarPorTitulo(string titulo)
        {
            return await _mediator.Send(new EliminarPorTitulo.Ejecuta { Titulo = titulo });
        }
    }
}
