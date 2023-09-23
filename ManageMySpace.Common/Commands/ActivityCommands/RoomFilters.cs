using System;

namespace ManageMySpace.Common.Commands.ActivityCommands
{
    public class RoomFilters
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public bool IsComputerClass { get; set; }
        public bool HasProjector { get; set; }
        public int Capacity { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
