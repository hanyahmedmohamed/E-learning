//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_LearningFCIH.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subject
    {
        public Subject()
        {
            this.User_Subject = new HashSet<User_Subject>();
            this.Lectures = new HashSet<Lecture>();
            this.Quizs = new HashSet<Quiz>();
        }
    
        public int ID { get; set; }
        public string SubjectNames { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual ICollection<User_Subject> User_Subject { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Quiz> Quizs { get; set; }
    }
}
