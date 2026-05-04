using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> AddAsync(Admin admin);
    }
}
