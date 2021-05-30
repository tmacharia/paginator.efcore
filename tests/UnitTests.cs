using System.Linq;
using Paginator.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace tests
{
    internal class UnitTests : TestBaseContext
    {
        [SetUp]
        public void Setup()
        {
        }

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
    }
}