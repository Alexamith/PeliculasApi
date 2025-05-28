using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneroDTO>> GetById(int id)
        {
            var entidad = await context.Generos.FirstOrDefaultAsync(genero => genero.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<GeneroDTO>(entidad);
            return dto;
        }


    }
}
