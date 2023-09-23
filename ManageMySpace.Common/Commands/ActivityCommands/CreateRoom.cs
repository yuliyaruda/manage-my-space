using System;

namespace ManageMySpace.Common.Commands.ActivityCommands
{
    public class CreateRoom
    {
        public string RoomNumber { get; set; }
        public bool IsComputerClass { get; set; }
        public bool HasProjector { get; set; }
        public int Capacity { get; set; }
        public  string Notes { get; set; }
    }
}
