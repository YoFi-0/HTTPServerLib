using Microsoft.EntityFrameworkCore;
using Server_Side.Repositorys.Classes;

namespace Server_Side.DataBase
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
           
        }
    }
}
