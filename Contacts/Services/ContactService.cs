using Contacts.Interfaces;
using Contacts.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Contacts.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactContext _dbContext;

        public ContactService(ContactContext contactContext)
        {
            _dbContext = contactContext;
        }

        //public async Task<bool> Create(RegisterModel contact)
        //{
        //    Contact newContact = new Contact
        //    {
        //        Id = Guid.NewGuid(),
        //        FirstName = contact.FirstName,
        //        Surname = contact.Surname,
        //        Email = contact.Email,
        //        Category = new Category { Name = contact.Category },
        //        };

        //    User newUser = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        ContactId = newContact.Id,
        //        Username = contact.Email,
        //        PwdHash = contact.Password 
        //    };

        //    _dbContext.Contacts.Add(newContact);
        //    _dbContext.Users.Add(newUser);
        //    if (await _dbContext.SaveChangesAsync() > 0)
        //        return true;
        //    else
        //        return false;
        //}

        public async Task<bool> Delete(Guid id)
        {
            var c = await _dbContext.Contacts.FirstAsync(c => c.Id == id);
            if (c != null) {
				await using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try {
                    transaction.CreateSavepoint("BeforeDeleteContact");
                    _dbContext.Users.Where(u => u.ContactId == c.Id)
                        .ExecuteDeleteAsync().Wait();
                    await _dbContext.SaveChangesAsync();

					_dbContext.Contacts.Remove(c);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

					return true;
                } catch {
                    await transaction.RollbackToSavepointAsync("BeforeDeleteContact");
					return false;
                }
            } else {
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

        public async Task<bool> Update(Guid contactId, ContactDTO contact)
        {
            var c = await _dbContext.Contacts.FirstAsync(c => c.Id == contactId);
            if (c != null) {
                c.FirstName = contact.FirstName;
                c.Surname = contact.Surname;
                c.Email = contact.Email;
                c.Phone = contact.Phone;
                if (DateOnly.TryParse(contact.DateOfBirth, out var dob)) {
                    c.BirthDate = dob;
                } else {
                    c.BirthDate = null;
				}
                c.CategoryId = contact.CategoryId;
                c.SubcategoryId = contact.SubcategoryId;
                _dbContext.Entry(c).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            } else {
                return false;
            }
        }
    }
}
