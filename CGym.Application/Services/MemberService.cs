using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CGym.Application.Services
{
    // Servicelaget — binder API og database sammen
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        // Modtager repository via Dependency Injection
        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // Opretter og gemmer et nyt medlem i databasen
        public async Task<Member> CreateMemberAsync(string firstName, string lastName, string email)
        {
            var member = new Member(firstName, lastName, email);
            return await _memberRepository.AddAsync(member);
        }

        // Henter alle medlemmer fra databasen
        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        // Henter ét medlem via ID — returnerer null hvis ikke fundet
        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        // Sletter et medlem fra databasen via ID
        public async Task DeleteAsync(int id)
        {
            await _memberRepository.DeleteAsync(id);
        }
    }
}