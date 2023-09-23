using System;

namespace ManageMySpace.Common.EF.Models
{
    public class UserActivityOrganizator
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}
