using Microsoft.EntityFrameworkCore;

namespace tests
{
    internal class TestBaseContext : TestBase
    {
        private TestDbContext _context;
        private DbContextOptions<TestDbContext> DbContextOptions;

        public TestBaseContext()
        { }

        protected TestDbContext Context
        {
            get
            {
                if (_context == null)
                    InitContx();
                return _context;
            }
        }
        protected void InitContx()
        {
            DbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                        .UseInMemoryDatabase(databaseName: "test_db")
                        .EnableServiceProviderCaching(true)
                        .EnableSensitiveDataLogging(true)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                        .Options;
            _context = new TestDbContext(DbContextOptions);
        }


        internal T Add<T>(T obj, bool save = true)
            where T : class
        {
            obj = Context.Add(obj).Entity;
            if (save)
                Save();
            return obj;
        }
        protected void Save() => Context.SaveChanges();
    }
}