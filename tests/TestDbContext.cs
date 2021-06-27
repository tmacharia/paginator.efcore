using Microsoft.EntityFrameworkCore;

namespace tests
{
    internal class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            :base(options)
        { }

        public DbSet<Car> Cars { get; set; }
    }
    public class Car
    {
        public int Id { get; set; }
    }
}