using Microsoft.EntityFrameworkCore;

using ToDo_Server.Data.Models;

namespace ToDo_Server.Data.DbAccess
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {

        }

        public DbSet<ToDo> ToDos => Set<ToDo>();
    }
}
