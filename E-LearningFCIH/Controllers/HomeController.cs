using E_LearningFCIH.CustomClasses;
using E_LearningFCIH.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_LearningFCIH.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["_CurrentFCIHUser"] = null;
            return View("Index");
        }
       
        public ActionResult PreHome()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {               
                    return View();
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult RegisterCourse()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 1 || usr.UserGroupID == 3)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }
        DAL.Entities entity = new DAL.Entities();
          [HttpPost]
        [AllowAnonymous]      
        public  void RegisterCourseProcess()
        {
            var user =(User)Session["_CurrentFCIHUser"];
              List<string> subjects = new List<string>();
            if (Convert.ToInt32(Request["firstSubject"]) != -1 && !string.IsNullOrEmpty(Request["firstSubject"]))
                subjects.Add(Request["firstSubject"]);
            if (Convert.ToInt32(Request["secondSubject"]) != -1 && !string.IsNullOrEmpty(Request["secondSubject"]))
                subjects.Add(Request["secondSubject"]);
            if (Convert.ToInt32(Request["thirdSubject"]) != -1 && !string.IsNullOrEmpty(Request["thirdSubject"]))
                subjects.Add(Request["thirdSubject"]);
            if (Convert.ToInt32(Request["fourthSubject"]) != -1 && !string.IsNullOrEmpty(Request["fourthSubject"]))
                subjects.Add(Request["fourthSubject"]);

            subjects = subjects.Distinct().ToList();

            if (subjects.Count > 0)
            {
                for (int i = 0; i < subjects.Count; i++)
                {
                    DAL.User_Subject userSubject = new DAL.User_Subject();
                    userSubject.UserID = user.ID;
                    userSubject.SubjectID = Convert.ToInt32(subjects[i]);
                    userSubject.SubjectStatus = Status.Request.ToString();
                    entity.User_Subject.Add(userSubject);
                }
                entity.SaveChanges();
            }
          
        }
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}