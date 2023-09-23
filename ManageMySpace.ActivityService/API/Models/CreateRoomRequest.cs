namespace ManageMySpace.ActivityService.API.Models
{
    public class CreateRoomRequest
    {
        public string RoomNumber { get; set; }
        public bool IsComputerClass { get; set; }
        public bool HasProjector { get; set; }
        public int Capacity { get; set; }
        public string Notes { get; set; }
    }
}
