using Bookstore.Application.Common;
using Bookstore.Application.DTOs;
using Bookstore.Application.Interfaces;
using Bookstore.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _service;

        public BooksController(IBookRepository service)
        {
            _service = service;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            if (pagination.Page <= 0 || pagination.PageSize <= 0)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetAll(pagination);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            if (!result.Value!.Items.Any())
                return NotFound("No existen libros registrados");

            Response.Headers.Append("X-Total-Count", result.Value.TotalRecords.ToString());

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            var result = await _service.AddAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}
