using CleanUps.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    internal interface IAuthRepository
    {
        public DbSet<User> Users { get; set; }
    }
}
