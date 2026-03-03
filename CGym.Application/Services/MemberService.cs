using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CGym.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly List<Member> _members = new();

        public Task<Member> CreateMemberAsync(string firstName, string lastName, string email)
        {
            var member = new Member(firstName, lastName, email);

            _members.Add(member);

            return Task.FromResult(member);
        }

        public Task<List<Member>> GetAllMembersAsync()
        {
            return Task.FromResult(_members);
        }
    }
}