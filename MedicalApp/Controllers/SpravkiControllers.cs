using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalApi.Models;
using MedicalApi.Data;

[ApiController]
[Route("api/[controller]")]
public class SpravkiController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public SpravkiController(MedicalDbContext db)
    {
        _db = db;
    }

    // Сортировка по цене или по дате
    [HttpGet]
    public async Task<List<Spravka>> GetAll(string? sortBy = null, bool desc = false)
    {
        IQueryable<Spravka> query = _db.Spravki.AsNoTracking();

        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.Equals("price", StringComparison.OrdinalIgnoreCase))
            {
                query = desc ? query.OrderByDescending(s => s.Price) : query.OrderBy(s => s.Price);
            }
            else if (sortBy.Equals("date", StringComparison.OrdinalIgnoreCase))
            {
                query = desc ? query.OrderByDescending(s => s.Date) : query.OrderBy(s => s.Date);
            }
        }

        return await query.ToListAsync();
    }

    // Статистика для главврача: сколько справок выписано и на какую сумму
    [HttpGet("stats")]
    public async Task<ActionResult> GetStats()
    {
        int totalCount = await _db.Spravki.CountAsync();
        decimal totalAmount = await _db.Spravki.SumAsync(s => s.Price);

        return Ok(new
        {
            TotalCount = totalCount,
            TotalAmount = totalAmount
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Spravka>> GetById(int id)
    {
        Spravka? spravka = await _db.Spravki.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (spravka == null)
        {
            return NotFound("Справка не найдена.");
        }
        return spravka;
    }

    [HttpPost]
    public async Task<ActionResult<Spravka>> Create(Spravka spravka)
    {
        bool patientExists = await _db.Patients.AnyAsync(p => p.Id == spravka.PatientId);
        if (!patientExists)
        {
            return BadRequest("Пациент с указанным ID не найден.");
        }

        SpravkaType? spravkaType = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == spravka.SpravkaTypeId);
        if (spravkaType == null)
        {
            return BadRequest("Тип справки с указанным ID не найден.");
        }

        spravka.Id = 0; // Id генерирует база данных
        spravka.Name = spravkaType.Name;
        spravka.Price = spravkaType.Price;

        // Npgsql для timestamptz требует DateTime с Kind = Utc
        spravka.Date = spravka.Date == default
            ? DateTime.UtcNow.Date
            : DateTime.SpecifyKind(spravka.Date, DateTimeKind.Utc);

        _db.Spravki.Add(spravka);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = spravka.Id }, spravka);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Spravka>> Update(int id, Spravka updatedSpravka)
    {
        Spravka? existingSpravka = await _db.Spravki.FirstOrDefaultAsync(s => s.Id == id);
        if (existingSpravka == null)
        {
            return NotFound("Справка не найдена.");
        }

        bool patientExists = await _db.Patients.AnyAsync(p => p.Id == updatedSpravka.PatientId);
        if (!patientExists)
        {
            return BadRequest("Пациент с указанным ID не найден.");
        }

        SpravkaType? spravkaType = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == updatedSpravka.SpravkaTypeId);
        if (spravkaType == null)
        {
            return BadRequest("Тип справки с указанным ID не найден.");
        }

        existingSpravka.PatientId = updatedSpravka.PatientId;
        existingSpravka.SpravkaTypeId = updatedSpravka.SpravkaTypeId;
        existingSpravka.Name = spravkaType.Name;
        existingSpravka.Price = spravkaType.Price;
        if (updatedSpravka.Date != default)
        {
            existingSpravka.Date = DateTime.SpecifyKind(updatedSpravka.Date, DateTimeKind.Utc);
        }

        await _db.SaveChangesAsync();

        return Ok(existingSpravka);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        Spravka? spravka = await _db.Spravki.FirstOrDefaultAsync(s => s.Id == id);
        if (spravka == null)
        {
            return NotFound("Справка не найдена.");
        }

        _db.Spravki.Remove(spravka);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}