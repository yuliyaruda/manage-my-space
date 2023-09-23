using System;

namespace ManageMySpace.ActivityService.API.Models
{
    public class ActivityResponseModel
    {
        public bool IsVisitor { get; set; }
        public bool IsOrganizator { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OrganizatorName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PeriodType { get; set; }
        public int MaxCapacity { get; set; }
        public int VisitorsNumber { get; set; }
        public int Duration { get; set; }
        public string RoomNumber { get; set; }
    }
}
