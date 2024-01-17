using FinalSkillsLabProject.BL;
using FinalSkillsLabProject.BL.BusinessLogicLayer;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using FluentAssertions;
using Moq;

namespace FinalSkillsLabProject.Test
{
    [TestFixture]
    public class AccountBLTests
    {
        private Mock<IAccountDAL> _stubAccount;
        private IAccountBL _accountBL;

        private List<LoginModel> _logins;
        private List<UserViewModel> _users;
        private List<AccountModel> _accounts;

        [SetUp]
        public void Setup()
        {
            _logins = new List<LoginModel>()
            {
                new LoginModel()
                {
                    UserId = 1,
                    Username = "Mehshar",
                    Password = "Mehshar",
                    Role = new RoleModel()
                    {
                        RoleId = 1,
                        RoleName = RoleEnum.Admin
                    }
                },

                new LoginModel()
                {
                    UserId = 2,
                    Username = "Sophia",
                    Password = "Sophia",
                    Role = new RoleModel()
                    {
                        RoleId = 2,
                        RoleName = RoleEnum.Manager
                    }
                },

                new LoginModel()
                {
                    UserId = 3,
                    Username = "Harper",
                    Password = "Harper",
                    Role = new RoleModel()
                    {
                        RoleId = 3,
                        RoleName = RoleEnum.Employee
                    }
                }
            };

            _users = new List<UserViewModel>()
            {
                new UserViewModel()
                {
                    UserId = 1,
                    Username = "Mehshar",
                    Role = new RoleModel()
                    {
                        RoleId = 1,
                        RoleName = RoleEnum.Admin
                    },
                    FirstName = "Mehshar",
                    LastName = "Mauraknah",
                    Email = "mehshar.mauraknah@gmail.com",
                    MobileNum = "54770024",
                    Department = "Support & Services"
                },

                new UserViewModel()
                {
                    UserId = 3,
                    Username = "Harper",
                    Role = new RoleModel()
                    {
                        RoleId = 3,
                        RoleName = RoleEnum.Employee
                    },
                    FirstName = "Harper",
                    LastName = "Robinson",
                    Email = "harper.robinson40@gmail.com",
                    MobileNum = "59364506",
                    Department = "Product & Technology",
                    ManagerFirstName = "Sophia",
                    ManagerLastName = "Middleton",
                    ManagerEmail = "sophia.middleton40@gmail.com"
                }
            };

            _accounts = new List<AccountModel>()
            {
                new AccountModel()
                {
                    UserId = 1,
                    Username = "Mehshar",
                    Password = "Mehshar"
                },

                new AccountModel()
                {
                    UserId = 2,
                    Username = "Sophia",
                    Password = "Sophia"
                }
            };

            _stubAccount = new Mock<IAccountDAL>();

            

            _stubAccount.Setup(accountDAL => accountDAL.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => _users.FirstOrDefault(x => x.Username == username));

            _stubAccount.Setup(accountDAL => accountDAL.GetByUsernameAndUserIdAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string username, int userId) => _accounts.FirstOrDefault(x => x.Username == username && x.UserId != userId));

            _stubAccount.Setup(accountDAL => accountDAL.UpdateAsync(It.IsAny<AccountModel>()))
                .ReturnsAsync(true);

            _accountBL = new AccountBL(_stubAccount.Object);
        }

        [Test]
        public async Task AuthenticateUserAsync_SuccessfulLogin_ReturnsTrue()
        {
            // Arrange
            var login = new LoginModel()
            {
                Username = "Mehshar",
                Password = "Mehshar"
            };

            var existingLogin = _logins.First(x => x.Username == login.Username);

            byte[] salt = HashingBL.GenerateSalt();
            (byte[] hashedPassword, _) = HashingBL.HashPassword(existingLogin.Password, salt);

            _stubAccount.Setup(accountDAL => accountDAL.AuthenticateUserAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync((hashedPassword, salt));

            // Act
            var result = await _accountBL.AuthenticateUserAsync(login);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AuthenticateUserAsync_IncorrectUsername_Failure()
        {
            // Arrange
            var login = new LoginModel()
            {
                Username = "Mehsharzs",
                Password = "Mehshar"
            };

            _stubAccount.Setup(accountDAL => accountDAL.AuthenticateUserAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync((null, null));

            // Act
            var result = await _accountBL.AuthenticateUserAsync(login);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AuthenticateUserAsync_IncorrectPassword_Failure()
        {
            // Arrange
            var login = new LoginModel()
            {
                Username = "Mehshar",
                Password = "Mehsharsz"
            };

            var existingLogin = _logins.First(x => x.Username == login.Username);

            byte[] salt = HashingBL.GenerateSalt();
            (byte[] hashedPassword, _) = HashingBL.HashPassword(existingLogin.Password, salt);

            _stubAccount.Setup(accountDAL => accountDAL.AuthenticateUserAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync((hashedPassword, salt));

            // Act
            var result = await _accountBL.AuthenticateUserAsync(login);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetByUsernameAsync_Successful_ReturnsUser()
        {
            // Arrange
            string username = "Harper";

            var expectedResult = new UserViewModel()
            {
                UserId = 3,
                Username = "Harper",
                Role = new RoleModel()
                {
                    RoleId = 3,
                    RoleName = RoleEnum.Employee
                },
                FirstName = "Harper",
                LastName = "Robinson",
                Email = "harper.robinson40@gmail.com",
                MobileNum = "59364506",
                Department = "Product & Technology",
                ManagerFirstName = "Sophia",
                ManagerLastName = "Middleton",
                ManagerEmail = "sophia.middleton40@gmail.com"
            };

            // Act
            var result = await _accountBL.GetByUsernameAsync(username);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        [TestCase(9, "Marie", "Marie", "Account successfully updated!")]
        [TestCase(10, "Mehshar", "Mehshar40", "Username already exists!")]
        public async Task UpdateAsync_SuccessFailure_ReturnsMessage(int userId, string username, string password, string expectedResult)
        {
            // Arrange
            var accountToUpdate = new AccountModel()
            {
                UserId = userId,
                Username = username,
                Password = password
            };

            // Act
            var result = await _accountBL.UpdateAsync(accountToUpdate);

            // Assert
            Assert.That(result, Does.Contain(expectedResult));
        }
    }
}
