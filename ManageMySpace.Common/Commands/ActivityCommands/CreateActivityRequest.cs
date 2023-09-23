using System;

namespace ManageMySpace.Common.Commands.ActivityCommands
{
    public class CreateActivityRequest : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PeriodType { get; set; }
        public int Duration { get; set; }
        public string RoomNumber { get; set; }
        public string OrganizatorId { get; set; }
    }
}
