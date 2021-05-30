using System.Linq;
using Paginator.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace tests
{
    internal class UnitTests : TestBaseContext
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Synchronous
        [Test]
        [Order(0)]
        public void Pg_Empty()
        {
            var list = Context.Cars.Paginate(1, 10);
            Assert.IsNotNull(list);
            Assert.Zero(list.TotalItems);
        }
        [Test]
        [Order(1)]
        public void Pg_With_Items()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var list = Context.Cars.Paginate(1, 2);
            Assert.IsNotNull(list);
            Assert.AreEqual(4, list.TotalItems);
            Assert.AreEqual(2, list.TotalPages);
        }
        [Test]
        [Order(2)]
        public void Pg_Skip_Count()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var list = Context.Cars.Paginate(1, 2, true);
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.TotalItems);
            Assert.AreEqual(1, list.TotalPages);
        }
        [Test]
        [Order(2)]
        public void Pg_Invalid_PageParam_ThrowEx()
        {
            Assert.Throws<ArgumentException>(() => Context.Cars.Paginate(0, 2));
            Assert.Throws<ArgumentException>(() => Context.Cars.Paginate(-1, 2));
        }
        [Test]
        [Order(2)]
        public void Pg_Invalid_PerPageParam_ThrowEx()
        {
            Assert.Throws<ArgumentException>(() => Context.Cars.Paginate(1, -1));
        }
        #endregion

        #region Asynchronous
        [Test]
        [Order(0)]
        public async Task AsyncPg_Empty()
        {
            var list = await Context.Cars.PaginateAsync(1, 10);
            Assert.IsNotNull(list);
            Assert.Zero(list.TotalItems);
        }
        [Test]
        [Order(1)]
        public async Task AsyncPg_With_Items()
        {
            var list = await Context.Cars.PaginateAsync(1, 2);
            Assert.IsNotNull(list);
            Assert.AreEqual(4, list.TotalItems);
            Assert.AreEqual(2, list.TotalPages);
        }
        [Test]
        [Order(2)]
        public async Task AsyncPg_Skip_Count()
        {
            var list = await Context.Cars.PaginateAsync(1, 2, true);
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.TotalItems);
            Assert.AreEqual(1, list.TotalPages);
        }
        [Test]
        [Order(2)]
        public void AsyncPg_Invalid_PageParam_ThrowEx()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Context.Cars.PaginateAsync(0, 2));
            Assert.ThrowsAsync<ArgumentException>(() => Context.Cars.PaginateAsync(-1, 2));
        }
        [Test]
        [Order(2)]
        public void AsyncPg_Invalid_PerPageParam_ThrowEx()
        {
            Assert.ThrowsAsync<ArgumentException>(() => Context.Cars.PaginateAsync(1, -1));
        }
        [Test]
        [Order(3)]
        public void AsyncPg_CancelledToken_Throw()
        {
            var src = new CancellationTokenSource();
            src.Cancel();
            Assert.ThrowsAsync<OperationCanceledException>(() => Context.Cars.PaginateAsync(1, 2, token: src.Token));
        }
        #endregion
    }
}