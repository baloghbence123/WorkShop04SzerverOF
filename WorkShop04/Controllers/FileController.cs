using Microsoft.AspNetCore.Mvc;
using WorkShop04.Models;

namespace WorkShop04.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        static List<FileModel> fileModels = new List<FileModel>();
        [HttpGet]
        public IEnumerable<FileModel> GetFiles()
        {
            return fileModels;
        }
        [HttpGet("{id}")]
        public FileModel? GetFiles(string id)
        {
            return fileModels.FirstOrDefault(t => t.Id == id);

        }
        [HttpPost]
        public void AddFile([FromBody] FileModel model)
        {
            fileModels.Add(model);
        }
        [HttpPut]
        public void EditFile([FromBody] FileModel model)
        {
            var old = fileModels.FirstOrDefault(t => t.Id == model.Id);
            old.Path = model.Path;
            old.Name = model.Name;

        }
        [HttpDelete("{id}")]
        public void DeleteFile(string id)
        {
            var delete = fileModels.FirstOrDefault(t => t.Id == id);
            if (delete != null)
            {
                fileModels.Remove(delete);
            }
        }


    }
}
