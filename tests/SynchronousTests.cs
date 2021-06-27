using Paginator.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace tests
{
    [TestFixture]
    //[SingleThreaded]
    internal class SynchronousTests : TestBaseContext
    {
        [SetUp]
        public void Setup()
        {
            InitContx();
        }

        #region Synchronous
        [Order(0)]
        [TestCase(Category = SYNC_TESTS)]
        public void Pg_Empty()
        {
            var paged = Context.Cars.Paginate(1, 10);

            Assert.IsNotNull(paged);
            Assert.Zero(paged.TotalItems);

            Log(paged);
        }
        [Order(1)]
        [TestCase(Category = SYNC_TESTS)]
        public void Pg_Invalid_PageParam_ThrowEx()
        {
            Assert.Throws<ArgumentException>(() => Context.Cars.Paginate(0, 2));
            Assert.Throws<ArgumentException>(() => Context.Cars.Paginate(-1, 2));
        }
        [Order(1)]
        [TestCase(Category = SYNC_TESTS)]
        public void Pg_Invalid_PerPageParam_ThrowEx()
        {
            Assert.Throws<ArgumentException>(() => Context.Cars.Paginate(1, -1));
        }
        [Order(2)]
        [TestCase(Category = SYNC_TESTS)]
        public void Pg_Skip_Count()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var paged = Context.Cars.Paginate(1, 2, true);

            Assert.IsNotNull(paged);
            Assert.AreEqual(2, paged.TotalItems);
            Assert.AreEqual(1, paged.TotalPages);

            Log(paged);
        }
        [Order(2)]
        [RequiresThread]
        [TestCase(Category = SYNC_TESTS)]
        public void Pg_With_Items()
        {
            int k = 5174;
            for (int i = 0; i < k; i++)
                Context.Add(new Car());

            Save();

            var paged = Context.Cars.Paginate(35, 8);

            Assert.IsNotNull(paged);
            Assert.AreEqual(k, paged.TotalItems);
            Assert.AreEqual(647, paged.TotalPages);

            Log(paged);
        }
        #endregion
    }
}