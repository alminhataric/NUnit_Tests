using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private HouseKeeperService _service;
        private Mock<IStatmentGenerator> _statmentGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private DateTime _statmentDate = new DateTime(2017, 1, 1);
        private Housekeeper _housekeeper;
        private string _statmentFileName;
        [SetUp]
        public void SetUp()
        {
            _housekeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
               _housekeeper
            }.AsQueryable());

            _statmentFileName = "fileName"; 
            _statmentGenerator = new Mock<IStatmentGenerator>();
            _statmentGenerator
               .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statmentDate)))
               .Returns(() => _statmentFileName);

            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();

            _service = new HouseKeeperService(
                unitOfWork.Object,
                _statmentGenerator.Object,
                _emailSender.Object,
                _messageBox.Object);
        }

        [Test]
        public void SendStatmentEmails_WhenCalled_GenerateStatments()
        {
            _service.SendStatementEmails(_statmentDate);

            _statmentGenerator.Verify(sg =>
            sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statmentDate)));
        }

        [Test]
        public void SendStatmentEmails_HouseKeeperEmailIsNull_ShouldNotGenerateStatments()
        {
            _housekeeper.Email = null;

            _service.SendStatementEmails(_statmentDate);

            _statmentGenerator.Verify(sg =>
            sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statmentDate)),
            Times.Never);
        }

        [Test]
        public void SendStatmentEmails_HouseKeeperEmailIsWhitespace_ShouldNotGenerateStatments()
        {
            _housekeeper.Email = " ";

            _service.SendStatementEmails(_statmentDate);

            _statmentGenerator.Verify(sg =>
            sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statmentDate)),
            Times.Never);
        }

        [Test]
        public void SendStatmentEmails_HouseKeeperEmailIsEmpty_ShouldNotGenerateStatments()
        {
            _housekeeper.Email = "";

            _service.SendStatementEmails(_statmentDate);

            _statmentGenerator.Verify(sg =>
            sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statmentDate)),
            Times.Never);
        }

        [Test]
        public void SendStatmentEmails_WhenCalled_EmailTheStatment()
        {

            _service.SendStatementEmails(_statmentDate);

            VerfyEmailSent();

        }
       


        [Test]
        public void SendStatmentEmails_StatmentFileIsNull_ShouldNotEmailStatment()
        {
            _statmentFileName = null;

            _service.SendStatementEmails(_statmentDate);

            VerifyEmailNotSent();

           
        }
       

        [Test]
        public void SendStatmentEmails_StatmentFileIsEmptyString_ShouldNotEmailStatment()
        {
            _statmentFileName = "";

            _service.SendStatementEmails(_statmentDate);

            VerifyEmailNotSent();

        }

        [Test]
        public void SendStatmentEmails_StatmentFileIsWhitespace_ShouldNotEmailStatment()
        {
            _statmentFileName = " ";

            _service.SendStatementEmails(_statmentDate);

            VerifyEmailNotSent();

        }

        [Test]
        public void SendStatmentEmails_EmailSendingFails_ShouldDisplayAMessageBox()
        {
            _emailSender.Setup(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
                )).Throws<Exception>();

            _service.SendStatementEmails(_statmentDate);

            _messageBox.Verify(mb => mb.Show(
                It.IsAny<string>(),
                It.IsAny<string>(),
                MessageBoxButtons.OK));

        }
       

        private void VerifyEmailNotSent()
        {
            _emailSender.Verify(es => es.EmailFile(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Never);
        }

        private void VerfyEmailSent()
        {
            _emailSender.Verify(es => es.EmailFile(
               _housekeeper.Email,
               _housekeeper.StatementEmailBody,
               _statmentFileName,
               It.IsAny<string>()));
        }
    }
}
