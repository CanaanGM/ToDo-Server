namespace ToDo_Server.Data.Mapper
{
    public class ToDoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime MarkedDone { get; set; }
        public DateTime AddedToDatabase { get; } = DateTime.UtcNow;
        public bool status { get; set; } = false;
    }
}
