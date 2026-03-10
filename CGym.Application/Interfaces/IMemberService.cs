using System;
using System.Collections.Generic;
using System.Text;
using CGym.Domain.Entities;


namespace CGym.Application.Interfaces
{
    public interface IMemberService
    {
        Task<Member> CreateMemberAsync(string firstName, string lastName, string email);
        Task<List<Member>> GetAllMembersAsync();
        Task<Member?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
