using System;

namespace ManageMySpace.Common.EF.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }
        public DateTime StartDateTime { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
