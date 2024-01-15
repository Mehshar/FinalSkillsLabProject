//using FinalSkillsLabProject.BL;
//using FinalSkillsLabProject.Common.Models;
//using FinalSkillsLabProject.DAL.Interfaces;
//using Moq;

//namespace FinalSkillsLabProject.Test
//{
//    public class Tests
//    {
//        private Mock<IAccountDAL> _stubAccount;
//        private AccountBL _accountBL;
//        private List<AccountModel> _accounts;

//        [SetUp]
//        public void Setup()
//        {
//            _accounts = new List<AccountModel>()
//            {
//                new AccountModel()
//                {
//                    Username = "Test",
//                    UserId = 1,
//                    Password = "0x9D7C47244D1EEA1B0D1847658B634C5C95BFAFEA389FB20283F79AFE4BA80EA658C19785D589E387A6605EC155599BFE3823FF3797AC24AB183823C729178ABE"
//                },
//            };

//            _stubAccount = new Mock<IAccountDAL>();

//            _stubAccount.Setup(accountDAL => accountDAL.AuthenticateUserAsync(It.IsAny<LoginModel>()))
//                .ReturnsAsync((LoginModel model) => _accounts.Any(a => a.Username == model.Username && a.Password == model.Password));

//            _accountBL = new AccountBL(_stubAccount.Object);
//        }

//        [Test]
//        [TestCase("Test", "Mehshar", ExpectedResult = true)]
//        public async Task<bool> Test_login(string username, string password)
//        {
//            // Arrange
//            var model = new LoginModel()
//            {
//                Username = username,
//                Password = password
//            };
//            // Act
//            var result = await _accountBL.AuthenticateUserAsync(model);

//            // Assert
//            return result;
//        }
//    }
//}