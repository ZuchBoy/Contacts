using Contacts.Interfaces;
using Contacts.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactContext _dbContext;

        public ContactService(ContactContext contactContext)
        {
            _dbContext = contactContext;
        }

        public async Task<bool> Create(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            if (await _dbContext.SaveChangesAsync() > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var c = await _dbContext.Contacts.FirstAsync(c => c.Id == id);
            if (c != null)
            {
                _dbContext.Contacts.Remove(c);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Contact> Get(Guid id)
        {
            return await _dbContext.Contacts.FirstAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await _dbContext.Contacts.ToListAsync();
        }

        public async Task<bool> Update(Contact contact)
        {
            var c = await _dbContext.Contacts.FirstAsync(c => c.Id == contact.Id);
            if (c != null) {
                c = contact;
                await _dbContext.SaveChangesAsync();
                return true;
            } else
            {
                return false;
            }
        }
    }
}
