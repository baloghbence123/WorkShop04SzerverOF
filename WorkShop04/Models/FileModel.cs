namespace WorkShop04.Models
{
    public class FileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }


        public FileModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
