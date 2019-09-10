using E_LearningFCIH.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_LearningFCIH.Controllers
{
    public class SubjectsController : Controller
    {
        // GET: Subjects
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

        public ActionResult AttendanceReport()
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
        [HttpPost]
        public JsonResult GetSubjectsList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Subjects.Select(x => new { DisplayText = x.SubjectNames, Value = x.ID });
            return Json(new { Options = result, Result = "OK" });

        }
        [HttpPost]
        public JsonResult CreateMaterials(Material material)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                var addedmaterial = db.Materials.Add(material);
                db.SaveChanges();
                return Json(new { Result = "OK", Record = addedmaterial });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public JsonResult DeleteMaterials(int Id)
        {

            try
            {
                var material = db.Materials.Where(x => x.ID == Id).FirstOrDefault();
                db.Entry(material).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateMaterials(Material material)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult Materials()
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
        DAL.Entities db = new Entities();

        [HttpPost]
        public JsonResult quizList(int ID, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var result = db.Quizs.Where(x => x.ID == ID).ToList();
                return Json(new { Result = "OK", Records = result, TotalRecordCount = result.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult GetMaterials(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var result = db.Materials.ToList();
                return Json(new { Result = "OK", Records = result, TotalRecordCount = result.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult quizAnswers(int ID, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var result = db.Answers.Where(x => x.QuizID == ID).ToList();
                return Json(new { Result = "OK", Records = result, TotalRecordCount = result.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CreateAnswer(Answer quiz)
        {
            try
            {
                var items = db.Answers.Where(x => x.QuizID == quiz.QuizID && x.AnswerValue == 1).ToList();
                if (items != null)
                {
                    if (items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.AnswerValue = 2;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
                var result = db.Answers.Add(quiz);
                db.SaveChanges();
                return Json(new { Result = "OK", Record = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult CreateQuiz(Quiz quiz)
        {
            try
            {
                var result = db.Quizs.Add(quiz);
                
                db.SaveChanges();
                return Json(new { Result = "OK", Record = quiz });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult DeleteQuiz(int QuizID, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var result = db.Quizs.Where(x => x.QuizID == QuizID).FirstOrDefault();
                var answers = db.Answers.Where(x => x.QuizID == QuizID).ToList();
                for (int i = 0; i < answers.Count(); i++)
                {
                    db.Answers.Remove(answers[i]);
                }
                db.SaveChanges();

                var colection = db.User_Quiz.Where(x => x.QuizID == QuizID).ToList();
                for (int i = 0; i < colection.Count(); i++)
                {
                    db.User_Quiz.Remove(colection[i]);
                }
                db.SaveChanges();

                db.Quizs.Remove(result);
                db.SaveChanges();

                return Json(new { Result = "OK", Record = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Getsubjects(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Subjects;
            return Json(new { Records = result, Result = "OK", TotalRecordCount = result.Count() });

        }


        //[HttpPost]
        //public JsonResult GetSubjects()
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var result = db.Subjects.Select(x => new { DisplayText = x.SubjectNames, Value = x.ID });
        //    return Json(new { Options = result, Result = "OK" });

        //}

        //[HttpPost]
        //public JsonResult GetDrs()
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var result = db.Doctors.Select(x => new { DisplayText = x.Name, Value = x.ID });
        //    return Json(new { Options = result, Result = "OK" });

        //}
        [HttpPost]
        public JsonResult Create(Subject subject)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                var addedStudent = db.Subjects.Add(subject);
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
                var subject = db.Subjects.Where(x => x.ID == Id).FirstOrDefault();
                db.Entry(subject).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Update(Subject subject)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                db.Entry(subject).State = EntityState.Modified;
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