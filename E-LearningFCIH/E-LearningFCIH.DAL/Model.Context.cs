﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User_Subject> User_Subject { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User_Quiz> User_Quiz { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_Doctor> User_Doctor { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Quiz> Quizs { get; set; }
    }
}
