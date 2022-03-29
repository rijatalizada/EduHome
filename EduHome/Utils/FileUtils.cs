using NuGet.Common;

namespace EduHome.Utils;

public class FileUtils
{
    public static string CreateFile(string folderPath,string folderName, IFormFile file )
    {
        var fileName = Guid.NewGuid() + file.FileName;
        var imagePath = Path.Combine(folderPath, folderName, fileName);
        
        FileStream stream = new FileStream(imagePath, FileMode.Create);
        file.CopyTo(stream);
        stream.Close();

        return fileName;

    }
    
    public static void DeleteFile(string fullPath)
    {
   
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}