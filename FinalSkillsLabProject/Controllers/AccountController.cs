using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalSkillsLabProject.Controllers
{
    // having access without logging in
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountBL _accountBL;
        private readonly IUserBL _userBL;
        private readonly IDepartmentBL _departmentBL;

        public AccountController(IAccountBL accountBL, IUserBL userBL, IDepartmentBL departmentBL)
        {
            _accountBL = accountBL;
            _userBL = userBL;
            _departmentBL = departmentBL;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Authenticate(LoginModel loginModel)
        {
            bool isUserValid = _accountBL.AuthenticateUser(loginModel);
            if (isUserValid)
            {
                UserViewModel user = _accountBL.GetByUsername(loginModel.Username);
                SetSessionVariables(user);

                // for authentication to work using FormsAuthentication class
                // Not creating persistent cookie --> setting it to false
                FormsAuthentication.SetAuthCookie(loginModel.Username, false);
            }
            return Json(new { result = isUserValid, url = Url.Action("Index", "Home") });
        }

        [HttpGet]
        public ActionResult Signup()
        {
            ViewBag.Departments = _departmentBL.GetAll().ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Signup(SignUpModel signUp)
        {
            string result = _userBL.Add(signUp);
            return Json(new { result = result, url = Url.Action("Login", "Account") });
        }

        public JsonResult GetDepartmentManagers(int departmentId)
        {
            List<UserModel> managersList = _departmentBL.GetManagerByDepartment(departmentId).ToList();
            return Json(new { managers = managersList });
        }

        public JsonResult GetCurrentRole()
        {
            string currentRole = Session["CurrentRole"]?.ToString() ?? "";
            return Json(new { currentRole }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private void SetSessionVariables(UserViewModel user)
        {
            this.Session["CurrentUser"] = user;
            this.Session["CurrentUsername"] = user.Username;
            this.Session["CurrentUserId"] = user.UserId;
            this.Session["CurrentRole"] = user.Role.RoleName.ToString();
        }
    }
}