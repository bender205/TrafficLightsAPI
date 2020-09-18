using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrafficLights.Data.DataAccess;
using TrafficLights.Model;
using TrafficLights.Model.Entities;

namespace TrafficLights.Data
{
    public class AuthRepository
    {
        private readonly TraficLightsContext _databaseContext;

        public AuthRepository(TraficLightsContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
         
        public async Task<User> GetByCredentialsAsync(string userName, string passWord,
            CancellationToken cancellationToken)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(
                (x => x.Username == userName && x.Password == passWord), cancellationToken);
        }
        public async Task<User> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _databaseContext.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public void Update(User user)
        {
            _databaseContext.Users.Update(user);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.Run(() => _databaseContext.Users);
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _databaseContext.Users.FindAsync(id);
        }
    }
}
