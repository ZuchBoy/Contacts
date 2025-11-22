using Contacts.Interfaces;
using Contacts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contactService.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var contact = await _contactService.Get(id);
            if (contact == null) {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            var newContact = _contactService.Create(contact);
            return CreatedAtAction(nameof(Get), new { id = newContact.Id }, newContact);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] Contact contact)
        {
            return await _contactService.Update(contact) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _contactService.Delete(id) ? NoContent() : NotFound();
        }


    }
}
