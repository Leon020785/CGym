using System;
using System.Collections.Generic;
using System.Text;
using CGym.Domain.Entities;

namespace CGym.Application.Interfaces
{
    public interface IMemberRepository
    {
        // vi bruger Async fordi database kald kan godt tag tid som vi lært til undervisning.
        Task<Member> AddAsync(Member member);  // Opret medlem
        Task<List<Member>> GetAllAsync(); // Hent Alle Medlemmer
        Task<Member?> GetByIdAsync(int id); // Hent via id

        Task DeleteAsync(int id); // slet medlem


    }
}
