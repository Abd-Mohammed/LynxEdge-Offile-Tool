using Microsoft.AspNetCore.Mvc;

namespace LynxEdge.Managers;

public static class FileManager
{
	public static void Save(IFormFile file, string environmentPath )
	{
		//var filePath = GetFilePath(environmentPath);

		//if (!Directory.Exists(filePath))
		//{
		//	Directory.CreateDirectory(environmentPath);
		//}

		//using var stream = new FileStream(filePath, FileMode.Create);

		//file.CopyTo(stream);
	}

	public static byte[] Download(string fileName, string environmentPath)
	{
		var filePath = Path.Combine(environmentPath, "Files", fileName);

		var fileBytes = File.ReadAllBytes(filePath);

		return fileBytes;
	}

	public static string GetFilePath(string fileName, string environmentPath)
	{
		return Path.Combine(environmentPath,"Files" , fileName);
	}
}