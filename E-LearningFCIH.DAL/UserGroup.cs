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
    
    public partial class UserGroup
    {
        public UserGroup()
        {
            this.Users = new HashSet<User>();
        }
    
        public int ID { get; set; }
        public string GroupName { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}