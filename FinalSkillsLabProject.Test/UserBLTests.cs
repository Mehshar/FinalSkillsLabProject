using FinalSkillsLabProject.BL.BusinessLogicLayer;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using Moq;

namespace FinalSkillsLabProject.Test
{
    [TestFixture]
    public class UserBLTests
    {
        private Mock<IUserDAL> _stubUser;
        private Mock<IAccountDAL> _stubAccount;
        private IUserBL _userBL;

        private List<SignUpModel> _signUps;
        private List<UserModel> _users;
        private List<UserViewModel> _userViews;

        [SetUp]
        public void Setup()
        {
            _signUps = new List<SignUpModel>()
            {
                new SignUpModel()
                {
                    NIC = "H9I5BOCT94RILM",
                    FirstName = "Harper",
                    LastName = "Robinson",
                    Email = "harper.robinson40@gmail.com",
                    MobileNum = "59364506",
                    DepartmentId = 1,
                    ManagerId = 2,
                    RoleId = 3,
                    Username = "Harper",
                    Password = "Harper"
                }
            };

            _users = new List<UserModel>()
            {
                new UserModel()
                {
                    UserId = 3,
                    NIC = "H9I5BOCT94RILM",
                    FirstName = "Harper",
                    LastName = "Robinson",
                    Email = "harper.robinson40@gmail.com",
                    MobileNum = "59364506",
                    DepartmentId = 1,
                    ManagerId = 2,
                    RoleId = 3
                }
            };

            _userViews = new List<UserViewModel>()
            {
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

            _stubUser = new Mock<IUserDAL>();
            _stubAccount = new Mock<IAccountDAL>();

            _stubUser.Setup(userDAL => userDAL.AddAsync(It.IsAny<SignUpModel>()))
                .ReturnsAsync(true);

            _stubUser.Setup(userDAL => userDAL.GetAllAsync())
                .ReturnsAsync(_users);

            _stubAccount.Setup(accountDAL => accountDAL.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => _userViews.FirstOrDefault(x => x.Username == username));

            _userBL = new UserBL(_stubUser.Object, _stubAccount.Object);
        }

        [Test]
        [TestCase("marie.smith", "Invalid email!")]
        [TestCase("mariesmith.com", "Invalid email!")]
        [TestCase("marie.smith@gmail.com", "Account created successfully!")]
        public async Task AddAsync_ValidInvalidEmail(string email, string expectedResult)
        {
            // Arrange
            var signUp = new SignUpModel()
            {
                NIC = "M9I5BOCA94RILM",
                FirstName = "Marie",
                LastName = "Smith",
                Email = email,
                MobileNum = "58102561",
                DepartmentId = 1,
                ManagerId = 2,
                RoleId = 3,
                Username = "Marie",
                Password = "Marie"
            };

            // Act
            var result = await _userBL.AddAsync(signUp);

            // Assert
            Assert.That(result, Does.Contain(expectedResult));
        }

        [Test]
        [TestCase("H9I5BOCT94RILM", "marie.smith@gmail.com", "58102561", "Marie", "NIC already exists!")]
        [TestCase("X8I5BOCT94RILM", "harper.robinson40@gmail.com", "58102561", "Marie", "Email already exists!")]
        [TestCase("X8I5BOCT94RILM", "marie.smith@gmail.com", "59364506", "Marie", "Mobile number already exists!")]
        [TestCase("X8I5BOCT94RILM", "marie.smith@gmail.com", "58102561", "Harper", "Username already exists!")]
        public async Task AddAsync_Duplicates_ReturnsError(string nic, string email, string mobileNum, string username, string expectedResult)
        {
            // Arrange
            var signUp = new SignUpModel()
            {
                NIC = nic,
                FirstName = "Marie",
                LastName = "Smith",
                Email = email,
                MobileNum = mobileNum,
                DepartmentId = 1,
                ManagerId = 2,
                RoleId = 3,
                Username = username,
                Password = "Marie"
            };

            // Act
            var result = await _userBL.AddAsync(signUp);

            // Assert
            Assert.That(result, Does.Contain(expectedResult));
        }
    }
}
