using System.Linq;
using Paginator.EntityFrameworkCore;
using NUnit.Framework;

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
        public void Normal_Paginate_Empty()
        {
            var list = Context.Cars.Paginate(1, 10);
            Assert.IsNotNull(list);
            Assert.Zero(list.TotalItems);
        }

        [Test]
        [Order(1)]
        public void Paginate_With_Items()
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
        public void Paginate_Skip_Count()
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
    }
}