using CleanUps.BusinessLogic.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    internal interface IUserRepository : IRepository<User>;
}
