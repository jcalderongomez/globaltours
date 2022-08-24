using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entidades;
using Core.Especificacion;
using Core.Interfaces;
using Infraestructura.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LugaresController : ControllerBase
    {
        private readonly IRepositorio<Lugar> _lugarRepo;
        private readonly IRepositorio<Pais> _paisRepo;
        private readonly IRepositorio<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;

        public LugaresController(IRepositorio<Lugar> lugarRepo,
                                    IRepositorio<Pais> paisRepo,
                                    IRepositorio<Categoria> categoriaRepo,
                                    IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
            _paisRepo = paisRepo;
            _lugarRepo = lugarRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<LugarDto>>> GetLugares(){
            var espec = new LugaresConPaisCategoriaEspecificacion();
            var lugares= await _lugarRepo.ObterneTodosEspec(espec);
            return Ok(_mapper.Map<IReadOnlyList<Lugar>, IReadOnlyList<LugarDto>>(lugares));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LugarDto>> GetLugar(int id){
            var espec = new LugaresConPaisCategoriaEspecificacion(id);
            var lugar = await _lugarRepo.ObtenerEspec(espec);
            return _mapper.Map<Lugar,LugarDto>(lugar);
        }

        [HttpGet("paises")]
        public async Task<ActionResult<List<Pais>>> GetPaises(){
            return Ok(await _paisRepo.ObtenerTodosAsync());
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<List<Categoria>>> GetCategorias(){
            return Ok(await _categoriaRepo.ObtenerTodosAsync());
        }
    }
}