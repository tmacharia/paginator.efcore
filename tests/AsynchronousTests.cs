using Paginator.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace tests
{
    [Order(1)]
    [TestFixture]
    [SingleThreaded]
    internal class AsynchronousTests : TestBaseContext
    {
        public AsynchronousTests()
        {
            InitContx();
        }
        [SetUp]
        public void Setup()
        {
            InitContx();
        }

        #region Asynchronous
        [Order(0)]
        [TestCase(Category = ASYNC_TESTS)]
        public async Task AsyncPg_Empty()
        {
            var paged = await Context.Cars.PaginateAsync(1, 10);

            Assert.IsNotNull(paged);
            Assert.Zero(paged.TotalItems);

            Log(paged);
        }
        [Order(1)]
        [TestCase(Category = ASYNC_TESTS)]
        public void AsyncPg_Invalid_PageParam_ThrowEx()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Context.Cars.PaginateAsync(0, 2));
            Assert.ThrowsAsync<ArgumentException>(() => Context.Cars.PaginateAsync(-1, 2));
        }
        [Order(1)]
        [TestCase(Category = ASYNC_TESTS)]
        public void AsyncPg_Invalid_PerPageParam_ThrowEx()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Context.Cars.PaginateAsync(1, -1));
        }
        [Order(1)]
        [TestCase(Category = ASYNC_TESTS)]
        public void AsyncPg_CancelledToken_Throw()
        {
            var src = new CancellationTokenSource();
            src.Cancel();
            Assert.ThrowsAsync<OperationCanceledException>(() => Context.Cars.PaginateAsync(1, 2, cancellationToken: src.Token));
        }
        [Order(1)]
        [TestCase(Category = ASYNC_TESTS)]
        public async Task AsyncPg_Skip_Count()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var paged = await Context.Cars.PaginateAsync(1, 2, true);

            Assert.IsNotNull(paged);
            Assert.AreEqual(2, paged.TotalPages);
            Assert.AreEqual(2, paged.TotalItems);

            paged = await Context.Cars.PaginateAsync(2, 2, true);

            Assert.AreEqual(3, paged.TotalPages);
            Assert.AreEqual(4, paged.TotalItems);

            paged = await Context.Cars.PaginateAsync(3, 2, true);

            Assert.AreEqual(1, paged.Items.Count);
            Assert.AreEqual(3, paged.TotalPages);
            Assert.AreEqual(5, paged.TotalItems);

            Log(paged);
        }
        #endregion
    }
}