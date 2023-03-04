using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmesContext _context;
    private IMapper _mapper; //Mapeando AutoMapper

    public FilmeController(FilmesContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Recupera filme por ID
    /// </summary>
    /// <param name="id, take">Objeto com os campos necessários para busca de um filme em lista</param>
    /// <returns>IActionResult</returns>
    [HttpGet]
    public IEnumerable<ReadFilmeDTO> RecuperaListaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        return _mapper.Map<List<ReadFilmeDTO>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Recupera filme por ID
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para busca de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        var filmeDTO = _mapper.Map<ReadFilmeDTO>(filme);

        return Ok(filme);
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDTO filmeDTO)
    {
        //Mapeando AutoMapper
        Filme filme = _mapper.Map<Filme>(filmeDTO);

        _context.Filmes.Add(filme);
        _context.SaveChanges();

        return CreatedAtAction(nameof(RecuperaFilmePorId), new {id = filme.Id}, filme);
    }

    /// <summary>
    /// Atualiza filmes
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para busca de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a atualização seja feita com sucesso</response>
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDTO updateDTO)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        _mapper.Map(updateDTO, filme);
        _context.SaveChanges();

        //204 - Retorno utilizado em casos de PUT/POST/DELETE
        return NoContent();
    }

    /// <summary>
    /// Atualiza filmes parcialmente
    /// </summary>
    /// <param name="id, patchDocument">Objeto com os campos necessários para busca de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a atualização seja feita com sucesso</response>
    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDTO> patchDocument)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDTO>(filme);

        patchDocument.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        //204 - Retorno utilizado em casos de PUT/POST/DELETE
        return Ok(filme);
    }

    /// <summary>
    /// Deleta filmes
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para busca de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a deleção seja feita com sucesso</response>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();

        return NoContent();
    }
}
