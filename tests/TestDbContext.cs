using Microsoft.EntityFrameworkCore;

namespace tests
{
    internal class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            :base(options)
        {

        }
    }
}