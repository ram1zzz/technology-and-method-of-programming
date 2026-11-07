using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class SpravkiController : ControllerBase
{
    private static List<Spravka> _spravki = new List<Spravka>();

    // Сортировка по цене или по дате
    [HttpGet]
    public List<Spravka> GetAll(string? sortBy = null, bool desc = false)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            return _spravki;
        }

        if (sortBy.Equals("price", StringComparison.OrdinalIgnoreCase))
        {
            return desc ? _spravki.OrderByDescending(s => s.Price).ToList() : _spravki.OrderBy(s => s.Price).ToList();
        }

        if (sortBy.Equals("date", StringComparison.OrdinalIgnoreCase))
        {
            return desc ? _spravki.OrderByDescending(s => s.Date).ToList() : _spravki.OrderBy(s => s.Date).ToList();
        }

        return _spravki;
    }

    // Статистика для главврача: сколько справок выписано и на какую сумму
    [HttpGet("stats")]
    public ActionResult GetStats()
    {
        int totalCount = _spravki.Count;
        decimal totalAmount = _spravki.Sum(s => s.Price);

        return Ok(new
        {
            TotalCount = totalCount,
            TotalAmount = totalAmount
        });
    }

    [HttpGet("{id:int}")]
    public ActionResult<Spravka> GetById(int id)
    {
        Spravka? spravka = _spravki.FirstOrDefault(s => s.Id == id);
        if (spravka == null)
        {
            return NotFound("Справка не найдена.");
        }
        return spravka;
    }

    [HttpPost]
    public ActionResult<Spravka> Create(Spravka spravka)
    {
        Patient? patient = PatientsController.Patients.FirstOrDefault(p => p.Id == spravka.PatientId);
        if (patient == null)
        {
            return BadRequest("Пациент с указанным ID не найден.");
        }

        SpravkaType? spravkaType = SpravkaTypesController.Types.FirstOrDefault(t => t.Id == spravka.SpravkaTypeId);
        if (spravkaType == null)
        {
            return BadRequest("Тип справки с указанным ID не найден.");
        }

        spravka.Name = spravkaType.Name;
        spravka.Price = spravkaType.Price;

        if (spravka.Date == default)
        {
            spravka.Date = DateTime.Today;
        }

        spravka.Id = _spravki.Count > 0 ? _spravki.Max(s => s.Id) + 1 : 1;
        _spravki.Add(spravka);

        return CreatedAtAction(nameof(GetById), new { id = spravka.Id }, spravka);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Spravka> Update(int id, Spravka updatedSpravka)
    {
        Spravka? existingSpravka = _spravki.FirstOrDefault(s => s.Id == id);
        if (existingSpravka == null)
        {
            return NotFound("Справка не найдена.");
        }

        Patient? patient = PatientsController.Patients.FirstOrDefault(p => p.Id == updatedSpravka.PatientId);
        if (patient == null)
        {
            return BadRequest("Пациент с указанным ID не найден.");
        }

        SpravkaType? spravkaType = SpravkaTypesController.Types.FirstOrDefault(t => t.Id == updatedSpravka.SpravkaTypeId);
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
            existingSpravka.Date = updatedSpravka.Date;
        }

        return existingSpravka;
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        Spravka? spravka = _spravki.FirstOrDefault(s => s.Id == id);
        if (spravka == null)
        {
            return NotFound("Справка не найдена.");
        }

        _spravki.Remove(spravka);
        return NoContent();
    }
}
