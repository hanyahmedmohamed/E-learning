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
    public class LecturesController : Controller
    {
        // GET: Lectures
        public ActionResult Index()
        {
            return View();
        }


        //public JsonResult isNowHasLecture()
        //{ 
        //db.Lectures.Where(x=>x.StartsOn)
        //}



        DAL.Entities db = new Entities();
        [HttpPost]
        public JsonResult GetLectures(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Lectures;
            return Json(new { Records = result, Result = "OK", TotalRecordCount = result.Count() });

        }


        [HttpPost]
        public JsonResult GetSubjects()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Subjects.Select(x => new { DisplayText = x.SubjectNames, Value = x.ID });
            return Json(new { Options = result, Result = "OK" });

        }

        [HttpPost]
        public JsonResult GetDrs()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Users.Where(x => x.UserGroupID == 4).Select(x => new { DisplayText = x.Username, Value = x.ID });
            return Json(new { Options = result, Result = "OK" });

        }
        [HttpPost]
        public JsonResult Create(Lecture Lecture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                else
                {
                    var addedStudent = db.Lectures.Add(Lecture);
                    db.SaveChanges();
                    int sID = (int)addedStudent.SubjectID;
                    var userIDs = db.User_Subject.Where(x => x.SubjectID == sID).Select(y => y.UserID);
                    var mails = db.Users.Where(x => userIDs.Contains(x.ID)).Select(y => y.Username).ToList();
                    var phones = db.Users.Where(x => userIDs.Contains(x.ID)).Select(y => y.Phone).ToList();
                    if (addedStudent.StartsOn.HasValue)
                    {
                        for (int i = 0; i < mails.Count; i++)
                        {
                            var MailHelper = new MailHelper
                            {

                                Sender = "Sherif.farouk@isourceglobal.com",
                                Recipient = mails[i],
                                RecipientCC = null,
                                Subject = "FCIH E-Learning New Lecture Added",
                                Body = "New Lecture Of You Has Been Created " + addedStudent.Title + " Starts On " + addedStudent.StartsOn.Value.ToString()
                            };
                            MailHelper.Send();
                        }
                        for (int i = 0; i < phones.Count; i++)
                        {
                            new BaseClass().SendSMS(phones[i].Value.ToString(), "New Lecture Of You Has Been Created " + addedStudent.Title + " Starts On " + addedStudent.StartsOn.Value.ToString());
                        }

                    }

                    return Json(new { Result = "OK", Record = addedStudent });
                }
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
                var Lecture = db.Lectures.Where(x => x.ID == Id).FirstOrDefault();
                db.Entry(Lecture).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Update(Lecture Lecture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                db.Entry(Lecture).State = EntityState.Modified;
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