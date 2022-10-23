using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.UowRepository.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using src.Domain;

namespace EFCore.UowRepository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly ILogger<DepartamentoController> _logger;
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoController(ILogger<DepartamentoController> logger, IDepartamentoRepository repository)
        {
            _logger = logger;
            _departamentoRepository = repository;
        }

        //a parte comentada (referente ao from services), é uma maneira para ser usada para recuperar o repositório sem a necessidade da injeção de dependencia
        //departamento/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id /*, [FromServices]IDepartemantoRepository repository*/)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(id);
            return Ok(departamento);
        }
        [HttpPost]
        public IActionResult CreateDepartamento(Departamento departamento)
        {
             _departamentoRepository.Add(departamento);

            var saved = _departamentoRepository.Save();
            return Ok(departamento);
        }
    }
}
