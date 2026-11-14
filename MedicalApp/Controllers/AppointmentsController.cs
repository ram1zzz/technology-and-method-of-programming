using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalApi.Models;
using MedicalApi.Data;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public AppointmentsController(MedicalDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<Appointment>> GetAll()
    {
        return await _db.Appointments.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Appointment>> GetById(int id)
    {
        Appointment? appointment = await _db.Appointments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (appointment == null)
        {
            return NotFound("Запись на приём не найдена.");
        }
        return appointment;
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> Create(Appointment appointment)
    {
        bool patientExists = await _db.Patients.AnyAsync(p => p.Id == appointment.PatientId);
        if (!patientExists)
        {
            return BadRequest("Пациент с указанным ID не найден.");
        }

        if (string.IsNullOrWhiteSpace(appointment.DoctorName))
        {
            return BadRequest("Имя врача не может быть пустым.");
        }

        if (appointment.DoctorName.Length < 2)
        {
            return BadRequest("Имя врача должно содержать хотя бы 2 символа.");
        }

        if (appointment.DoctorName.Length > 100)
        {
            return BadRequest("Имя врача не может превышать 100 символов.");
        }

        if (string.IsNullOrWhiteSpace(appointment.Specialization))
        {
            return BadRequest("Специализация врача не может быть пустой.");
        }

        appointment.Id = 0; // Id генерирует база данных

        // Npgsql для timestamptz требует DateTime с Kind = Utc
        appointment.AppointmentDate = appointment.AppointmentDate == default
            ? DateTime.UtcNow
            : DateTime.SpecifyKind(appointment.AppointmentDate, DateTimeKind.Utc);

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Appointment>> Update(int id, Appointment updatedAppointment)
    {
        Appointment? existingAppointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        if (existingAppointment == null)
        {
            return NotFound("Запись на приём не найдена.");
        }

        bool patientExists = await _db.Patients.AnyAsync(p => p.Id == updatedAppointment.PatientId);
        if (!patientExists)
        {
            return BadRequest("Пациент с указанным ID не найден.");
        }

        if (string.IsNullOrWhiteSpace(updatedAppointment.DoctorName))
        {
            return BadRequest("Имя врача не может быть пустым.");
        }

        if (updatedAppointment.DoctorName.Length < 2)
        {
            return BadRequest("Имя врача должно содержать хотя бы 2 символа.");
        }

        if (updatedAppointment.DoctorName.Length > 100)
        {
            return BadRequest("Имя врача не может превышать 100 символов.");
        }

        if (string.IsNullOrWhiteSpace(updatedAppointment.Specialization))
        {
            return BadRequest("Специализация врача не может быть пустой.");
        }

        existingAppointment.PatientId = updatedAppointment.PatientId;
        existingAppointment.DoctorName = updatedAppointment.DoctorName;
        existingAppointment.Specialization = updatedAppointment.Specialization;
        if (updatedAppointment.AppointmentDate != default)
        {
            existingAppointment.AppointmentDate = DateTime.SpecifyKind(updatedAppointment.AppointmentDate, DateTimeKind.Utc);
        }
        if (!string.IsNullOrWhiteSpace(updatedAppointment.Status))
        {
            existingAppointment.Status = updatedAppointment.Status;
        }

        await _db.SaveChangesAsync();

        return Ok(existingAppointment);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        Appointment? appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        if (appointment == null)
        {
            return NotFound("Запись на приём не найдена.");
        }

        _db.Appointments.Remove(appointment);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}