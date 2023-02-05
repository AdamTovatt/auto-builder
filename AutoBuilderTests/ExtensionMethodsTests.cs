using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBuilder.Helpers;

namespace AutoBuilderTests
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        public void KebabToPascalIsWorking()
        {
            string kebab1 = "this-is-kebab-case";
            string kebab2 = "another-test";

            Assert.IsTrue(kebab1.ToPascalCasing() == "ThisIsKebabCase");
            Assert.IsTrue(kebab2.ToPascalCasing() == "AnotherTest");
            Assert.IsTrue(kebab1.ToPascalCasing().ToKebabCasing() == kebab1);
            Assert.IsTrue(kebab1.ToKebabCasing() == kebab1);
        }

        [TestMethod]
        public void PascalToKebabCaseIsWorking()
        {
            string pascal1 = "ThisIsAPascalCase";
            string pascal2 = "RoomReservation";

            Assert.IsTrue(pascal1.ToKebabCasing() == "this-is-a-pascal-case");
            Assert.IsTrue(pascal2.ToKebabCasing() == "room-reservation");
            Assert.IsTrue(pascal1.ToKebabCasing().ToPascalCasing() == pascal1);
            Assert.IsTrue(pascal1.ToPascalCasing() == pascal1);
        }
    }
}
