﻿using FinalSkillsLabProject.BL.BusinessLogicLayer;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalSkillsLabProject.Controllers
{
    // having access without logging in
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountBL _accountBL;
        private readonly IPendingUserBL _pendingUserBL;

        public AccountController(IAccountBL accountBL, IPendingUserBL pendingUserBL)
        {
            _accountBL = accountBL;
            _pendingUserBL = pendingUserBL;
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
                LoginModel user = _accountBL.GetByUsername(loginModel.Username);
                this.Session["CurrentUser"] = user;
                this.Session["CurrentUsername"] = user.Username;
                this.Session["CurrentUserId"] = user.UserId;
                this.Session["CurrentRole"] = user.Role.RoleName;
                // for authentication to work using FormsAuthentication class
                // Not creating persistent cookie --> setting it to false
                FormsAuthentication.SetAuthCookie(loginModel.Username, false);
            }
            return Json(new { result = isUserValid, url = Url.Action("GetAll", "Training") });
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Signup(PendingUserModel pendingUser)
        {
            string result = _pendingUserBL.Add(pendingUser);
            return Json(new { result = result, url = Url.Action("Login", "Account") });
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}