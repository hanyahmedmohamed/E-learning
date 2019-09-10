using E_LearningFCIH.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_LearningFCIH.Controllers
{
    public class ChatController : Controller
    {

        DAL.Entities db = new Entities();
        // GET: Chat
        public ActionResult Chat()
        {
            return View();
        }
        [HttpPost]
        public void SendMessage(string msg, string subjectID)
        {
            var usr = (E_LearningFCIH.DAL.User)Session["_CurrentFCIHUser"];
            Message newmsg = new Message();
            newmsg.CreatedOn = DateTime.Now;
            newmsg.MessageBody = msg;
            newmsg.SubjectID = int.Parse(subjectID);
            newmsg.UserID = usr.ID;
            db.Messages.Add(newmsg);
            db.SaveChanges();
        }
        [HttpPost]
        public JsonResult getMessages(string subjectID)
        {
            List<CustomClasses.CustomMessage> customMessages = new List<CustomClasses.CustomMessage>();
            int temp = int.Parse(subjectID);

            var tempList = db.Messages.Where(x => x.SubjectID == temp).OrderBy(y => y.CreatedOn).ToList();
            var users = db.Users;
            foreach (var item in tempList)
            {
                CustomClasses.CustomMessage newMessages = new CustomClasses.CustomMessage();
                newMessages.MessageBody = users.Where(x => x.ID == item.UserID).FirstOrDefault().Username.Trim() + " : " + item.MessageBody + " (" + item.CreatedOn + ")";
                customMessages.Add(newMessages);
            }
            var msgs = customMessages.ToArray();
            return Json(new { Result = msgs });
        }

    }
}