using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entidades = await context.Actores.ToListAsync();
            var dtos = mapper.Map<List<ActorDTO>>(entidades);
            return dtos;
        }


        [HttpGet("{id:int}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> GetById(int id)
        {
            var entidad = await context.Actores.FirstOrDefaultAsync(actor => actor.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ActorDTO>(entidad);
            return dto;
        }


        [HttpPost]

        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actor)
        {
            var entidad = mapper.Map<Actor>(actor);
            if (actor.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actor.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actor.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        actor.Foto.ContentType);
                }
            }
            context.Add(entidad);
            await context.SaveChangesAsync();
            var actorDTO = mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("obtenerActor", new { id = actorDTO.Id }, actorDTO);
        }


        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actor)
        {
            var entidad = mapper.Map<Actor>(actor);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Actor { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
