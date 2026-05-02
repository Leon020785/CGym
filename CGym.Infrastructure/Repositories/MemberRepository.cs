using CGym.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using CGym.Application.Interfaces;
using CGym.Domain.Entities;
using CGym.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace CGym.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext gymDbContext;

        public MemberRepository(GymDbContext context) // NÅr jeg opretter MemberRepos.. så giver jeg dig en GymDBContext.
        {
            gymDbContext = context;

        }

        public async Task<Member> AddAsync(Member member)
        {
            await gymDbContext.Members.AddAsync(member); // tilføje en medlem 
            await gymDbContext.SaveChangesAsync(); // gem 

            return member;
        }

        public async Task DeleteAsync(int id)
        {
            var member = await gymDbContext.Members.FindAsync(id); // vi gemmer member i en variable 
            
            if(member != null) // vi tjeker den som vi har fundet 
            {
                gymDbContext.Members.Remove(member); // vi fjerne den 
                await gymDbContext.SaveChangesAsync(); // vi gemmer !

            }
        }

        public async Task<List<Member>> GetAllAsync()
        {
            var adminEmails = await gymDbContext.Users
                .Where(u => u.IsAdmin)
                .Select(u => u.Email)
                .ToHashSetAsync();

            return await gymDbContext.Members
                .Where(m => !adminEmails.Contains(m.Email))
                .ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await gymDbContext.Members.FindAsync(id);
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await gymDbContext.Members
                .FirstOrDefaultAsync(member => member.Email == email);
        }

        public async Task<Member?> UpdateAsync(int id, string firstName, string lastName, string phoneNumber)
        {
            var member = await gymDbContext.Members.FindAsync(id);

            if (member == null)
            {
                return null;
            }

            member.Update(firstName, lastName, phoneNumber);
            await gymDbContext.SaveChangesAsync();

            return member;
        }

        public async Task<Member?> UpdateEmailAsync(int memberId, string newEmail)
        {
            var member = await gymDbContext.Members.FindAsync(memberId);
            if (member == null) return null;

            var user = await gymDbContext.Users.FirstOrDefaultAsync(u => u.Email == member.Email);

            member.UpdateEmail(newEmail);
            if (user != null)
                user.Email = newEmail;

            await gymDbContext.SaveChangesAsync();
            return member;
        }
    }
}
