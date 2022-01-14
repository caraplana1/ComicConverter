using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;

namespace ComicConverter
{
	public static class ComicBuilder
	{
		/// <summary>
		/// Creates zip file but with cbz extension
		/// </summary>
		/// <param name="imagesPath">Arrays of paths to  the images to include in the file</param>
		/// <param name="fileName">Name of the final document. Dont need to add the extension name</param>
		public static void CreateCBZ(string[] imagesPaths, string fileName)
		{
			DirectoryInfo dir = CreateHiddenDir(imagesPaths, $"{fileName.Split('/').Last()}CBZ");

			using (var archive = ZipArchive.Create())
			{
				archive.AddAllFromDirectory(dir.Name);
				archive.SaveTo($"{fileName}.cbz", CompressionType.Deflate);
			}

			dir.Delete(true);
		}

		/// <summary>
		/// Creates tar file but with cbt extension
		/// </summary>
		/// <param name="imagesPaths">Arrays of paths to  the images to include in the file</param>
		/// <param name="fileName">Name of the final document. Dont need to add the extension name</param>
		public static void CreateCBT(string[] imagesPaths, string fileName)
		{
			DirectoryInfo dir = CreateHiddenDir(imagesPaths, $"{fileName.Split('/').Last()}CBT");

			using (var archive = TarArchive.Create())
			{
				archive.AddAllFromDirectory(dir.Name);
				archive.SaveTo($"{fileName}.cbt", CompressionType.None);
			}

			dir.Delete(true);
		}

		/// <summary>
		/// Simple funtion to create a hidden directory and copy all files specificataed.
		/// </summary>
		/// <param name="filesPaths">Files wanted to copy on the directory</param>
		/// <returns>Directory info</returns>
		private static DirectoryInfo CreateHiddenDir(string[] filesPaths, string dirName)
		{
			DirectoryInfo dir = Directory.CreateDirectory(dirName);
			dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

			if (filesPaths.All(f => !File.Exists(f))) 
				throw new IOException("There is no file to add");
			
			filesPaths = filesPaths.Where(f => File.Exists(f)).ToArray();
			FileInfo fileInfo;

			foreach (var image in filesPaths)
			{
				fileInfo = new(image);
				File.Copy(fileInfo.FullName, $"{dirName}/{fileInfo.Name}", true);
			}

			return dir;
		}
	}
}