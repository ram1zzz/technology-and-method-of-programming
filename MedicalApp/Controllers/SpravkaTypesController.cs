using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalApi.Models;
using MedicalApi.Data;


[ApiController]
[Route("api/[controller]")]
public class SpravkaTypesController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public SpravkaTypesController(MedicalDbContext db)
    {
        _db = db;
    }

    // GET: api/SpravkaTypes
    [HttpGet]
    public async Task<List<SpravkaType>> GetAll()
    {
        return await _db.SpravkaTypes.AsNoTracking().ToListAsync();
    }

    // GET: api/SpravkaTypes/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SpravkaType>> GetById(int id)
    {
        SpravkaType? type = await _db.SpravkaTypes.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (type == null)
        {
            return NotFound("Вид справки не найден.");
        }
        return type;
    }

    // POST: api/SpravkaTypes
    [HttpPost]
    public async Task<ActionResult<SpravkaType>> Create(SpravkaType type)
    {
        if (type.Price <= 0)
        {
            return BadRequest("Цена вида справки должна быть больше 0.");
        }

        if (string.IsNullOrWhiteSpace(type.Name))
        {
            return BadRequest("Название справки не может быть пустым.");
        }

        if (type.Name.Length < 2)
        {
            return BadRequest("Название справки должно содержать хотя бы 2 символа.");
        }

        type.Id = 0; // Id генерирует база данных

        _db.SpravkaTypes.Add(type);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
    }

    // PUT: api/SpravkaTypes/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<SpravkaType>> Update(int id, SpravkaType updatedType)
    {
        SpravkaType? existingType = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (existingType == null)
        {
            return NotFound("Вид справки не найден.");
        }

        if (updatedType.Price <= 0)
        {
            return BadRequest("Цена вида справки должна быть больше 0.");
        }

        if (string.IsNullOrWhiteSpace(updatedType.Name))
        {
            return BadRequest("Название справки не может быть пустым.");
        }

        if (updatedType.Name.Length < 2)
        {
            return BadRequest("Название справки должно содержать хотя бы 2 символа.");
        }

        existingType.Name = updatedType.Name;
        existingType.Price = updatedType.Price;
        existingType.ValidityDays = updatedType.ValidityDays;
        existingType.NeedsDoctorExam = updatedType.NeedsDoctorExam;

        await _db.SaveChangesAsync();

        return Ok(existingType);
    }

    // DELETE: api/SpravkaTypes/1
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        SpravkaType? type = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (type == null)
        {
            return NotFound("Вид справки не найден.");
        }

        _db.SpravkaTypes.Remove(type);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}