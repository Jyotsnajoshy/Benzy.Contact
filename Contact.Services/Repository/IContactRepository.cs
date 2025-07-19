using Contact.Common.Types;
using Contact.Services.Dtos;

namespace Contact.Services.Repository
{
    public interface IContactRepository
    {
        Task<Result<ContactViewDto[]>> GetAllAsync();
        Task<Result<ContactViewDto?>> GetByIdAsync(string name);
        Task<Result<int?>> CreateAsync(ContactCreateDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool?>> UpdateAsync(int id, ContactUpdateDto dto);
    }
}
