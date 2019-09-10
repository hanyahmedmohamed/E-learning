using E_LearningFCIH.CustomClasses;
using E_LearningFCIH.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_LearningFCIH.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Students
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewMaterials()
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

        public ActionResult Attendance()
        {
            return View();

        }
        public ActionResult StudentsRequests()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 2)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }

        }

        public ActionResult Quizzes()
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
        public ActionResult Result()
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            if (usr != null)
            {
                if (usr.UserGroupID == 1 || usr.UserGroupID==3)
                    return View();


                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }
        public ActionResult ViewDoctors()
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
        [HttpPost]
        public void changeProfile(string ID)
        {
            int id = int.Parse(ID);
            var user = db.Users.Where(x => x.ID == id).FirstOrDefault();
            Session["_ProfileID"] = user;
            Profile();
        }
        public ActionResult Profile()
        {

            return View();
        }

        public bool changeRequestStatus(string doctorID, string status)
        {
            var usr = (User)Session["_CurrentFCIHUser"];
            try
            {
                int dr = int.Parse(doctorID);
                if (status == "Request")
                {
                    User_Doctor request = new User_Doctor();
                    request.Status = status;
                    request.UserID = usr.ID;
                    request.DoctorID = dr;
                    db.User_Doctor.Add(request);

                }

                else if (status == "Confirm")
                {


                    var request = db.User_Doctor.Where(x => x.DoctorID == dr && x.UserID == usr.ID).FirstOrDefault();
                    request.Status = status;
                    db.Entry(request).State = EntityState.Modified;

                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool changeRequestStatusForDr(string userrID, string status)
        {
            var usr = (User)Session["_CurrentFCIHUser"];
            try
            {
                int userID = int.Parse(userrID);

                var request = db.User_Doctor.Where(x => x.DoctorID == usr.ID && x.UserID == userID).FirstOrDefault();
                request.Status = status;
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        [HttpPost]
        public void Attend(string userID, string quizId)
        {
            int tempuserID = Convert.ToInt32(userID);
            int tempSubjectID = Convert.ToInt32(quizId);
            Attendance newAttendance = new DAL.Attendance();
            newAttendance.StudentID = tempuserID;
            newAttendance.QuizID = tempSubjectID;
            db.Attendances.Add(newAttendance);
            db.SaveChanges();
        }
        [HttpPost]
        public void SolveQuiz(string answer, string quizId)
        {
            var usr = (User)Session["_CurrentFCIHUser"];
            User_Quiz solve = new User_Quiz();
            solve.QuizID = int.Parse(quizId);
            solve.UserID = usr.ID;
            solve.AnswerValue = int.Parse(answer);
            db.User_Quiz.Add(solve);
            db.SaveChanges();
            Attend(usr.ID.ToString(), quizId);

        }


        DAL.Entities db = new Entities();

        [HttpPost]
        public JsonResult Evaluate()
        {
            var usr = (User)Session["_CurrentFCIHUser"];
            try
            {
                List<Result> result = new List<Result>();
                List<int> userSubjects = db.User_Subject.Where(x => x.UserID == usr.ID).Select(x => x.SubjectID).ToList();
                var source = db.Quizs.Where(y => userSubjects.Contains(y.ID) && y.Deadline >= DateTime.Today).Select(x => new { ID = x.QuizID, Name = x.Name }).ToList();

                for (int i = 0; i < source.Count(); i++)
                {
                    var quizAnswer = db.Answers.Where(x => x.QuizID == source[i].ID && x.AnswerValue == 1).FirstOrDefault();
                    if (quizAnswer != null)
                    {
                        Result newResult = new Result();
                        newResult.Quiz = source.Where(x => x.ID == source[i].ID).FirstOrDefault().Name;

                        newResult.TotalScore = 1;

                        var userAnswers = db.User_Quiz.Where(x => x.UserID == usr.ID && x.QuizID == source[i].ID).FirstOrDefault();
                        if (userAnswers != null)
                        {
                            int temp = (int)userAnswers.AnswerValue;
                            if (quizAnswer.AnswerID == temp)
                                newResult.StudentScore = 1;
                        }
                        else
                        {
                            newResult.StudentScore = 0;
                        }
                        result.Add(newResult);
                    }
                }


                return Json(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
    }
}