using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestNinja.UnitTests
{
    [TestFixture]
    class ErrorLoggerTests
    {
        [Test]
        public void Log_WhenCalled_SetTheLastErrorProperty()
        {
            var logger = new Fundamentals.ErrorLogger();

            logger.Log("a");

            Assert.That(logger.LastError, Is.EqualTo("a"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]

        public void Log_InvalidError_ThrowArgumentNullException(string error)
        {
            var logger = new Fundamentals.ErrorLogger();

            //logger.Log(error);

            Assert.That(()=> logger.Log(error), Throws.ArgumentNullException);

        }

       [Test]
       public void Log_ValidError_RaiseErrorLoggedEvent()
        {
            var logger = new Fundamentals.ErrorLogger();

            var id = Guid.Empty;
            logger.ErrorLogged += (sender, args) => { id = args; };

            logger.Log("a");

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }

        
    }
}
