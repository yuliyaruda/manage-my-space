using System;
using System.Collections.Generic;

namespace ManageMySpace.Common.EF.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }
        public bool IsComputerClass { get; set; }
        public bool HasProjector { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
