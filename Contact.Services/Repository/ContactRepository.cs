using Contact.Common;
using Contact.Common.Types;
using Contact.Entity.Data;
using Contact.Entity.Models;
using Contact.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Services.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactdbContext _context;
        public ContactRepository(ContactdbContext context)
        {
            _context = context;
        }
        public async Task<Result<int?>> CreateAsync(ContactCreateDto dto)
        {
            var contact = new ContactUser
            {
         FirstName = dto.FirstName,
         LastName = dto.LastName,
         PhoneNumber = dto.PhoneNumber,
            };
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return new Result<int?>(contact.Id);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return new Result<bool>(ResultType.NotFound)
                .AddError("Contact not found.", ResultType.NotFound);
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return new Result<bool>(true) { Message = "contact deleted successfully"};
        }

        public async Task<Result<ContactViewDto[]>> GetAllAsync()
        {
            var contact = await _context.Contacts
                .OrderBy(c => c.FirstName)
                .Select(c => new ContactViewDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                })
                .ToArrayAsync();
            return new Result<ContactViewDto[]>(contact);
        }

        public async Task<Result<ContactViewDto?>> GetByIdAsync(string name)
        {
            var contact = await _context.Contacts
        .Where(c => c.FirstName == name)
        .Select(c => new ContactViewDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName= c.LastName,
            PhoneNumber = c.PhoneNumber,
        })
         .FirstOrDefaultAsync();
            if (contact == null)
            {
                return new Result<ContactViewDto?>(ResultType.NotFound)
                    .AddError("Contact not found.", ResultType.NotFound);
            }
            return new Result<ContactViewDto?>(contact);
        }

        public async Task<Result<bool?>> UpdateAsync(int id, ContactUpdateDto dto)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return new Result<bool?>(ResultType.NotFound)
                    .AddError("Contact not found.", ResultType.NotFound);
            }
            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.PhoneNumber = dto.PhoneNumber;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return new Result<bool?>(true) { Message = "Contact updated successfully." };

        }
    }
}
