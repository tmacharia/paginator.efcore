﻿using Paginator.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace tests
{
    [TestFixture]
    [SingleThreaded]
    internal class AsynchronousTests : TestBaseContext
    {
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
        [Order(2)]
        [TestCase(Category = ASYNC_TESTS)]
        public async Task AsyncPg_Skip_Count()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var paged = await Context.Cars.PaginateAsync(1, 2, true);

            Assert.IsNotNull(paged);
            Assert.AreEqual(2, paged.TotalItems);
            Assert.AreEqual(1, paged.TotalPages);

            Log(paged);
        }
        [Order(2)]
        [TestCase(Category = ASYNC_TESTS)]
        public async Task AsyncPg_With_Items()
        {
            int k = 5174;
            for (int i = 0; i < k; i++)
                Context.Add(new Car());

            Save();

            var paged = await Context.Cars.PaginateAsync(35, 8);

            Assert.IsNotNull(paged);
            Assert.AreEqual(k, paged.TotalItems);
            Assert.AreEqual(647, paged.TotalPages);

            Log(paged);
        }
        #endregion
    }
}