using Contact.Services.Dtos;
using Contact.Services.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Benzy.Contact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        // GET: api/<ContactController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactRepository.GetAllAsync();
            return StatusCode((int)result.ResultType, result);
        }

        // GET api/<ContactController>/5
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _contactRepository.GetByIdAsync(name);
            return StatusCode((int)result.ResultType, result);
        }

        // POST api/<ContactController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contactRepository.CreateAsync(dto);
            return StatusCode((int)result.ResultType, result);
        }

        // PUT api/<ContactController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContactUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contactRepository.UpdateAsync(id, dto);
            return StatusCode((int)result.ResultType, result);
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactRepository.DeleteAsync(id);
            return StatusCode((int)result.ResultType, result);
        }
    }
}
