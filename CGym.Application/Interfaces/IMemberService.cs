using System;
using System.Collections.Generic;
using System.Text;
using CGym.Domain.Entities;


namespace CGym.Application.Interfaces
{
    internal interface IMemberService
    {
        Task<Member> CreateMemberAsync(string firstName, string lastName, string email);
        Task<List<Member>> GetAllMembersAsync();
    }
}
