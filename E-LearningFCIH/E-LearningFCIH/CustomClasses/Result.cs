using E_LearningFCIH.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningFCIH.CustomClasses
{
    public class Result
    {
        public string SubjectName { set; get; }
        public int SubjectTotalScore { set; get; }
        public int StudentSubjectScore { set; get; }
        public string Quiz { set; get; }
        public int TotalScore { set; get; }
        public int StudentScore { set; get; }
        

        public List<Result> Evaluate(int id)
        {
            DAL.Entities db = new Entities();
            
                List<Result> result = new List<Result>();
                List<int> userSubjects = db.User_Subject.Where(x => x.UserID == id).Select(x => x.SubjectID).ToList();
                var source = db.Quizs.Where(y => userSubjects.Contains(y.ID) && y.Deadline <= DateTime.Today).Select(x => new { ID = x.QuizID, Name = x.Name }).ToList();

                for (int i = 0; i < source.Count(); i++)
                {
                    int tempID=source[i].ID;
                    var quizAnswer = db.Answers.Where(x => x.QuizID == tempID && x.AnswerValue == 1).FirstOrDefault();
                    if (quizAnswer != null)
                    {
                        Result newResult = new Result();
                        newResult.Quiz = source.Where(x => x.ID == source[i].ID).FirstOrDefault().Name;

                        newResult.TotalScore = 1;

                        var userAnswers = db.User_Quiz.Where(x => x.UserID == 2 && x.QuizID == tempID).FirstOrDefault();
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


                return result ;
          
          

        }

        public List<Result> EvaluateSubjects(int id)
        {
            DAL.Entities db = new Entities();

            List<Result> result = new List<Result>();
            List<int> userSubjects = db.User_Subject.Where(x => x.UserID == id).Select(x => x.SubjectID).ToList();
            var source = db.Quizs.Where(y => userSubjects.Contains(y.ID) && y.Deadline <= DateTime.Today).Select(x => new { ID = x.QuizID, Name = x.Name,SubjectID=x.ID }).ToList();

            for (int i = 0; i < source.Count(); i++)
            {
                int tempID = source[i].ID;
                var quizAnswer = db.Answers.Where(x => x.QuizID == tempID && x.AnswerValue == 1).FirstOrDefault();
                if (quizAnswer != null)
                {
                    Result newResult = new Result();
                    newResult.Quiz = source.Where(x => x.ID == source[i].ID).FirstOrDefault().Name;
                    int sbjctid = source.Where(x => x.ID == source[i].ID).FirstOrDefault().SubjectID;
                    newResult.SubjectName = db.Subjects.Where(x => x.ID==sbjctid).FirstOrDefault().SubjectNames;
                    newResult.SubjectTotalScore = 1;
                    newResult.TotalScore = 1;

                    var userAnswers = db.User_Quiz.Where(x => x.UserID == 2 && x.QuizID == tempID).FirstOrDefault();
                    if (userAnswers != null)
                    {
                        int temp = (int)userAnswers.AnswerValue;
                        if (quizAnswer.AnswerID == temp)
                            newResult.StudentScore = 1;
                        newResult.StudentSubjectScore = 1;
                    }
                    else
                    {
                        newResult.StudentScore = 0;
                        newResult.StudentSubjectScore = 0;
                    }
                    result.Add(newResult);
                }
            }


            return result;



        }
    }
}