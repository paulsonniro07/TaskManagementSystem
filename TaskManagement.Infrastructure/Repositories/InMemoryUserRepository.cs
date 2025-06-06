using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Budi" },
            new User { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Andi" },
            new User { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Rudi" }

        };

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }
        public Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }
    }
}
