using Contact.Common.Types;
using Contact.Services.Dtos;

namespace Contact.Services.Repository
{
    public interface IContactRepository
    {
        Task<Result<ContactViewDto[]>> GetAllAsync(string userId);
        Task<Result<ContactViewDto?>> GetByIdAsync(string userId, int Id);
        Task<Result<string?>> CreateAsync(string userId, ContactCreateDto dto);
        Task<Result<bool?>> UpdateAsync(string userId, int Id, ContactUpdateDto dto);
        Task<Result<bool>> DeleteAsync(string userId, int Id);
    }
}
