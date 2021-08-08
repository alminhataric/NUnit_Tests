using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    class CustomerControllerTests
    {
        [Test]
        public void GetCustomer_IdIsZero_ReturnNotFound()
        {
            var controller = new Fundamentals.CustomerController();

            var result = controller.GetCustomer(0);

            // NotFound
            Assert.That(result, Is.TypeOf<NotFound>());

            //NotFound or one of its derivitives
            //Assert.That(result, Is.InstanceOf<NotFound>());

        }

        [Test]
        public void GetCustomer_IdIsNotZero_ReturnOk()
        {
            var controller = new Fundamentals.CustomerController();

            var result = controller.GetCustomer(1);

            // NotFound
            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}
