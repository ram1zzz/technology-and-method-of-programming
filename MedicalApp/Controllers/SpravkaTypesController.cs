using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;


    [ApiController]
    [Route("api/[controller]")]
    public class SpravkaTypesController : ControllerBase
    {
        public static List<SpravkaType> Types = new List<SpravkaType>
        {
            new SpravkaType { Id = 1, Name = "Справка 083", Price = 100, ValidityDays = 30, NeedsDoctorExam = true },
            new SpravkaType { Id = 2, Name = "Санитарная книжка", Price = 750, ValidityDays = 365, NeedsDoctorExam = true },
            new SpravkaType { Id = 3, Name = "Справка в бассейн", Price = 300, ValidityDays = 180, NeedsDoctorExam = false },
            new SpravkaType { Id = 4, Name = "Справка 086", Price = 500, ValidityDays = 90, NeedsDoctorExam = true }
        };

        // GET: api/SpravkaTypes
        [HttpGet]
        public List<SpravkaType> GetAll()
        {
            return Types;
        }

        // GET: api/SpravkaTypes/1
        [HttpGet("{id:int}")]
        public ActionResult<SpravkaType> GetById(int id)
        {
            SpravkaType? type = Types.FirstOrDefault(s => s.Id == id);
            if (type == null)
            {
                return NotFound("Вид справки не найден.");
            }
            return type;
        }

        // POST: api/SpravkaTypes
        [HttpPost]
        public ActionResult<SpravkaType> Create(SpravkaType Type)
        {
            // Валидация: цена должна быть больше 0
            if (Type.Price <= 0)
            {
                return BadRequest("Цена вида справки должна быть больше 0.");
            }

            if (string.IsNullOrWhiteSpace(Type.Name))
            {
                return BadRequest("Название справки не может быть пустым.");
            }

            if (Type.Name.Length < 2)
            {
                return BadRequest("Название справки должно содержать хотя бы 2 символа.");
            }

            Type.Id = Types.Count > 0 ? Types.Max(t => t.Id) + 1 : 1;
            Types.Add(Type);

              return CreatedAtAction(nameof(GetById), new { id = Type.Id }, Type);
        }

        // PUT: api/SpravkaTypes/1
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, SpravkaType updatedType)
        {
            var existingType = Types.FirstOrDefault(t => t.Id == id);
            if (existingType == null)
            {
                return NotFound("Вид справки не найден.");
            }

            if (updatedType.Price <= 0)
            {
                return BadRequest("Цена вида справки должна быть больше 0.");
            }

            existingType.Name = updatedType.Name;
            existingType.Price = updatedType.Price;
            existingType.ValidityDays = updatedType.ValidityDays;
            existingType.NeedsDoctorExam = updatedType.NeedsDoctorExam;

            return NoContent();
        }

        // DELETE: api/SpravkaTypes/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var type = Types.FirstOrDefault(t => t.Id == id);
            if (type == null)
            {
                return NotFound("Вид справки не найден.");
            }

            Types.Remove(type);
            return NoContent();
        }
    }
