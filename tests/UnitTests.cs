using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Common;
using NUnit.Framework;
using Paginator.EntityFrameworkCore;

namespace tests
{
    [TestFixture]
    internal class UnitTests : TestBaseContext
    {
        [SetUp]
        public void Setup()
        {
            InitContx();
        }

        [TestCase(Category = SERIAL_TESTS)]
        public void ToJson()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var paged = Context.Cars.Paginate(1, 2, true);

            string json = paged.ToJson();

            Assert.IsNotEmpty(json);
            Assert.That(json.IsValidJson());

            System.Console.WriteLine(json);
        }

        [TestCase(Category = SERIAL_TESTS)]
        public void ToXml()
        {
            Add(new Car());
            Add(new Car());
            Add(new Car());
            Add(new Car());

            var paged = Context.Cars.Paginate(1, 2, true);

            XmlSerializer xsSubmit = new XmlSerializer(paged.GetType());
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, paged);
                    xml = sww.ToString(); // Your XML
                }
            }

            Assert.IsNotEmpty(xml);

            System.Console.WriteLine(xml);
        }
    }
}