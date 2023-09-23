using System;
using System.Collections.Generic;

namespace ManageMySpace.Common.EF.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PeriodType { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<UserActivityOrganizator> Organizators { get; set; }
        public ICollection<UserActivityVisitor> Visitors { get; set; }
    }
}
