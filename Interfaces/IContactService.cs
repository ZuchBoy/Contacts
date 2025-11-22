using Contacts.Models;

namespace Contacts.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAll();
        Task<Contact> Get(Guid id);
        Task<bool> Create(Contact contact);
        Task<bool> Update(Contact contact);
        Task<bool> Delete(Guid id);
    }
}
