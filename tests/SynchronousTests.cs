using Paginator.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace tests
{
    [Order(0)]
    [TestFixture]
    internal class SynchronousTests : TestBaseContext
    {
        public SynchronousTests()
        {
            InitContx();
        }
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
        [Order(1)]
        [TestCase(Category = SYNC_TESTS)]
        public void Pg_Skip_Count()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var paged = Context.Cars.Paginate(1, 2, true);

            Assert.IsNotNull(paged);
            Assert.AreEqual(2, paged.TotalPages);
            Assert.AreEqual(2, paged.TotalItems);

            paged = Context.Cars.Paginate(2, 2, true);

            Assert.AreEqual(3, paged.TotalPages);
            Assert.AreEqual(4, paged.TotalItems);

            paged = Context.Cars.Paginate(3, 2, true);

            Assert.AreEqual(1, paged.Items.Count);
            Assert.AreEqual(3, paged.TotalPages);
            Assert.AreEqual(5, paged.TotalItems);

            Log(paged);
        }
        
        #endregion
    }
}