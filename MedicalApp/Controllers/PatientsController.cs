using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;
using Microsoft.EntityFrameworkCore;
using MedicalApi.Data;


[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public PatientsController(MedicalDbContext db)
    {
        _db = db;
    }


    [HttpGet]
    public async Task<List<Patient>> GetAll()
    {
        return await _db.Patients.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        // ИСПРАВЛЕНО: Ищем пациента в статическом списке Patients
        Patient? patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
        {
            return NotFound();
        }

        return patient;
    }

    [HttpPost]
   public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.Name))
        {
            return BadRequest("Имя пациента не может быть пустым.");
        }
        if (patient.Name.Length > 20)
        {
            return BadRequest("Имя пациента не может превышать 20 символов.");
        }
        if (patient.Name.Length <= 1)
        {
            return BadRequest("Имя пациента должно содержать хотя бы 2 символа.");
        }

       _db.Patients.Add(patient);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Patient>> Update(int id, Patient patient)
    {
        Patient? existingPatient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (existingPatient == null)
        {
            return NotFound("Пациент не найден.");
        }

        if (string.IsNullOrWhiteSpace(patient.Name))
        {
            return BadRequest("Имя пациента не может быть пустым.");
        }
        if (patient.Name.Length > 100)
        {
            return BadRequest("Имя пациента не может превышать 100 символов.");
        }
        if (patient.Name.Length <= 1)
        {
            return BadRequest("Имя пациента должно содержать хотя бы 2 символа.");
        }

        existingPatient.Name = patient.Name;
        existingPatient.BirthDate = patient.BirthDate;
        existingPatient.Address = patient.Address;
        existingPatient.PhoneNumber = patient.PhoneNumber;
        await _db.SaveChangesAsync();

        return Ok(existingPatient); // Лучше возвращать Ok(...) вместо неявного преобразования
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        Patient? patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null)
        {
            return NotFound("Пациент не найден.");
        }

        _db.Patients.Remove(patient);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}