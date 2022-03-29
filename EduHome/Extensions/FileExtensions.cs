namespace EduHome.Extensions;

public static class FileExtensions
{
    public static bool IsSupportedFile(this IFormFile file, string contentType)
    {
        return file.ContentType.Contains(contentType);
    }
    
    public static bool IsGreaterThanGivenMb(this IFormFile file, int mb)
    {
        return file.Length > mb * 1024 * 1024;
    }
}