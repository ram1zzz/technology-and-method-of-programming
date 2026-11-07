using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    public static List<Patient> Patients = new List<Patient>();

    [HttpGet]
    public List<Patient> GetAll()
    {
        return Patients;
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id) 
    {
        // ИСПРАВЛЕНО: Ищем пациента в статическом списке Patients
        var item = Patients.FirstOrDefault(p => p.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public ActionResult<Patient> Create(Patient patient)
    {
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

        patient.Id = Patients.Count > 0 ? Patients.Max(p => p.Id) + 1 : 1;
        Patients.Add(patient);

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Patient> Update(int id, Patient patient)
    {
        Patient? existingPatient = Patients.FirstOrDefault(p => p.Id == id);
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

        return Ok(existingPatient); // Лучше возвращать Ok(...) вместо неявного преобразования
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        Patient? patient = Patients.FirstOrDefault(p => p.Id == id);
        if (patient == null)
        {
            return NotFound("Пациент не найден.");
        }

        Patients.Remove(patient);
        return NoContent();
    }
}