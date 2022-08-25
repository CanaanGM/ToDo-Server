namespace ToDo_Server.Data.Mapper
{
    public class ToDoReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime MarkedDone { get; set; }
        public bool status { get; set; } = false;
        public DateTime AddedToDatabase { get; } = DateTime.UtcNow;
    }


    public class ToDoUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime MarkedDone { get; set; }
        public bool status { get; set; } = false;
    }
}
