using System;

namespace CustomRoutes.Models
{
    public class ProfileModel
    {
        public DAL.UserProfile UserProfile;

        public DateTime LastLogin { get; set; }
    }
}