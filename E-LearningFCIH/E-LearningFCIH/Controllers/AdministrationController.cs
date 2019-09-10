using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using E_LearningFCIH.DAL;
using System.Data.Entity;
using E_LearningFCIH.CustomClasses;

namespace E_LearningFCIH.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Index()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 4)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }



        public ActionResult Users()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 4)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult Doctors()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 4)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }
        public ActionResult Subjects()
        {
            return View();
        }

        public ActionResult Notification()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 4)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        //tifications
        public void Notifications(string SubjectID, string Message)
        {
            int sID = int.Parse(SubjectID);

            var userIDs = db.User_Subject.Where(x => x.SubjectID == sID).Select(y => y.UserID);
            var mails = db.Users.Where(x => userIDs.Contains(x.ID)).Select(y => y.Username).ToList();
            var phones = db.Users.Where(x => userIDs.Contains(x.ID)).Select(y => y.Phone).ToList();

            for (int i = 0; i < mails.Count; i++)
            {
                var MailHelper = new MailHelper
                {

                    Sender = "Sherif.farouk@isourceglobal.com",
                    Recipient = mails[i],
                    RecipientCC = null,
                    Subject = "FCIH E-Learning New Lecture Added",
                    Body = Message
                };
                MailHelper.Send();
            }
            for (int i = 0; i < phones.Count; i++)
            {
                new BaseClass().SendSMS(phones[i].Value.ToString(), Message);
            }
        }

        DAL.Entities db = new Entities();
        [HttpPost]
        public JsonResult GetUsers(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Users;
            return Json(new { Records = result, Result = "OK", TotalRecordCount = result.Count() });

        }

        [HttpPost]
        public JsonResult GetDoctors(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<customUser> customResult = new List<customUser>(); 
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Users.Where(x => x.UserGroupID == 4).ToList();
            foreach (var item in result)
            {
                customUser usr = new customUser();
                usr.ID = item.ID;
                usr.Username = item.Username;
               var subjects= db.User_Subject.Where(x=>x.UserID==usr.ID).FirstOrDefault();

               usr.SubjectID = subjects != null ? subjects.SubjectID : 0;
               customResult.Add(usr);
            }
            return Json(new { Records = customResult.ToList(), Result = "OK", TotalRecordCount = result.Count() });

        }
        

              [HttpPost]
        public JsonResult UpdateDoctor(customUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                User_Subject updatedUser = new User_Subject();
                var userSub = db.User_Subject.Where(x => x.UserID == user.ID).FirstOrDefault();
                if (userSub != null)
                {
                    userSub.SubjectID = user.SubjectID;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    updatedUser.UserID=user.ID;
                    updatedUser.SubjectID=user.SubjectID;
                    updatedUser.SubjectStatus = "Open";
                    db.User_Subject.Add(updatedUser);
                    db.SaveChanges();
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetSubjectsList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Subjects.Where(y=>y.Status==true).Select(x => new { DisplayText = x.SubjectNames, Value = x.ID });
            return Json(new { Options = result, Result = "OK" });

        }
        [HttpPost]
        public JsonResult GetGroups()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.UserGroups.Select(x => new { DisplayText = x.GroupName, Value = x.ID });
            return Json(new { Options = result, Result = "OK" });

        }
        [HttpPost]
        public JsonResult Create(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                var addedStudent = db.Users.Add(user);
                db.SaveChanges();
                return Json(new { Result = "OK", Record = addedStudent });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                var user = db.Users.Where(x => x.ID == Id).FirstOrDefault();
                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Update(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}