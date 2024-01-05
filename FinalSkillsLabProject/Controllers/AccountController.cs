using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<JsonResult> Authenticate(LoginModel loginModel)
        {
            bool isUserValid = await _accountBL.AuthenticateUserAsync(loginModel);
            if (isUserValid)
            {
                UserViewModel user = await _accountBL.GetByUsernameAsync(loginModel.Username);
                SetSessionVariables(user);

                // for authentication to work using FormsAuthentication class
                // Not creating persistent cookie --> setting it to false
                FormsAuthentication.SetAuthCookie(loginModel.Username, false);
            }
            return Json(new { result = isUserValid, url = Url.Action("Index", "Home") });
        }

        [HttpGet]
        public async Task<ActionResult> Signup()
        {
            ViewBag.Departments = (await _departmentBL.GetAllAsync()).ToList();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Signup(SignUpModel signUp)
        {
            string result = await _userBL.AddAsync(signUp);
            return Json(new { result = result, url = Url.Action("Login", "Account") });
        }

        public async Task<JsonResult> GetDepartmentManagers(int departmentId)
        {
            List<UserModel> managersList = (await _departmentBL.GetManagerByDepartmentAsync(departmentId)).ToList();
            return Json(new { managers = managersList });
        }

        public JsonResult GetCurrentRole()
        {
            string currentRole = null;
            if (Session["CurrentUser"] != null)
            {
                currentRole = ((UserViewModel)Session["CurrentUser"]).Role.RoleName.ToString();
            }
            //string currentRole = Session["CurrentRole"]?.ToString() ?? "";
            return Json(new { currentRole }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            RemoveSessionVariables();
            return RedirectToAction("Login");
        }

        private void SetSessionVariables(UserViewModel user)
        {
            this.Session["CurrentUser"] = user;
        }

        private void RemoveSessionVariables()
        {
            Session.Clear();
            //Session.Remove("CurrentUser");
            //Session.Remove("CurrentUsername");
            //Session.Remove("CurrentUserId");
            //Session.Remove("CurrentRole");
        }
    }
}