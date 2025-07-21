using Contact.Common;
using Contact.Common.Types;
using Contact.Entity.Data;
using Contact.Entity.Models;
using Contact.Services.Dtos;
using FluentValidation;
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
        private readonly IValidator<ContactCreateDto> _createValidator;
        private readonly IValidator<ContactUpdateDto> _updateValidator;

        public ContactRepository(
            ContactdbContext context,
            IValidator<ContactCreateDto> createValidator,
            IValidator<ContactUpdateDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<string?>> CreateAsync(string userId, ContactCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var result = new Result<string?>(ResultType.InvalidData);
                foreach (var error in validationResult.Errors)
                    result.AddError(error.ErrorMessage, ResultType.InvalidData);
                return result;
            }

            var contact = new ContactUser
            {
                UserId = userId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return new Result<string?>();
        }

        public async Task<Result<bool?>> UpdateAsync(string userId, int id, ContactUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var result = new Result<bool?>(ResultType.InvalidData);
                foreach (var error in validationResult.Errors)
                    result.AddError(error.ErrorMessage, ResultType.InvalidData);
                return result;
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
            {
                return new Result<bool?>(ResultType.NotFound)
                    .AddError("Contact not found.", ResultType.NotFound);
            }

            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.Email = dto.Email;
            contact.PhoneNumber = dto.PhoneNumber;
            contact.Address = dto.Address;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return new Result<bool?>(true) { Message = "Contact updated successfully." };
        }

        public async Task<Result<bool>> DeleteAsync(string userId, int id)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
            {
                return new Result<bool>(ResultType.NotFound)
                    .AddError("Contact not found.", ResultType.NotFound);
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return new Result<bool>(true) { Message = "Contact deleted successfully." };
        }

        public async Task<Result<ContactViewDto?>> GetByIdAsync(string userId, int id)
        {
            var contact = await _context.Contacts
                .Where(c => c.Id == id && c.UserId == userId)
                .Select(c => new ContactViewDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                })
                .FirstOrDefaultAsync();

            if (contact == null)
            {
                return new Result<ContactViewDto?>(ResultType.NotFound)
                    .AddError("Contact not found.", ResultType.NotFound);
            }

            return new Result<ContactViewDto?>(contact);
        }

        public async Task<Result<ContactViewDto[]>> GetAllAsync(string userId)
        {
            var contacts = await _context.Contacts
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.FirstName)
                .Select(c => new ContactViewDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                })
                .ToArrayAsync();

            return new Result<ContactViewDto[]>(contacts);
        }

    }
}
