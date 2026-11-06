using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    public static List<Appointment> Appointments = new List<Appointment>();

    [HttpGet]
    public List<Appointment> GetAll()
    {
        return Appointments;
    }

    [HttpGet("{id}")]
    public ActionResult<Appointment> GetById(int id)
    {
        Appointment? appointment = Appointments.FirstOrDefault(a => a.Id == id);
        if (appointment == null)
        {
            return NotFound("Запись на приём не найдена.");
        }
        return appointment;
    }

    [HttpPost]
    public ActionResult<Appointment> Create(Appointment appointment)
    {
        Patient? patient = PatientsController.Patients.FirstOrDefault(p => p.Id == appointment.PatientId);
        if (patient == null)
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

        if (appointment.AppointmentDate == default)
        {
            appointment.AppointmentDate = DateTime.Now;
        }

        appointment.Id = Appointments.Count > 0 ? Appointments.Max(a => a.Id) + 1 : 1;
        Appointments.Add(appointment);

        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public ActionResult<Appointment> Update(int id, Appointment updatedAppointment)
    {
        Appointment? existingAppointment = Appointments.FirstOrDefault(a => a.Id == id);
        if (existingAppointment == null)
        {
            return NotFound("Запись на приём не найдена.");
        }

        Patient? patient = PatientsController.Patients.FirstOrDefault(p => p.Id == updatedAppointment.PatientId);
        if (patient == null)
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
            existingAppointment.AppointmentDate = updatedAppointment.AppointmentDate;
        }
        if (!string.IsNullOrWhiteSpace(updatedAppointment.Status))
        {
            existingAppointment.Status = updatedAppointment.Status;
        }

        return existingAppointment;
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Appointment? appointment = Appointments.FirstOrDefault(a => a.Id == id);
        if (appointment == null)
        {
            return NotFound("Запись на приём не найдена.");
        }

        Appointments.Remove(appointment);
        return NoContent();
    }
}
