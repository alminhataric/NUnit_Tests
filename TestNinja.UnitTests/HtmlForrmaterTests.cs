using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestNinja.UnitTests
{
    [TestFixture]
    class HtmlForrmaterTests
    {
        [Test]
        public void FormatAsBold_WhenCalled_ShouldEncloseTheStringWithStrongElement()
        {
            var formatter = new Fundamentals.HtmlFormatter();

            var result = formatter.FormatAsBold("abc");

            // Specific
            Assert.That(result, Is.EqualTo("<strong>abc</strong>").IgnoreCase);

            // More general
            Assert.That(result, Does.StartWith("<strong>").IgnoreCase);
            Assert.That(result, Does.EndWith("</strong>"));
            Assert.That(result, Does.Contain("abc"));

        }
    }
}
