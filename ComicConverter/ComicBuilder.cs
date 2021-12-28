using System;
using System.Text;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using System.Collections.Generic;

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
			DirectoryInfo dir = CreateHiddenDir(imagesPaths);

			using (var archive = ZipArchive.Create())
			{
				archive.AddAllFromDirectory(dir.Name);
				archive.SaveTo($"{fileName}.cbz", CompressionType.Deflate);
			}

			dir.Delete(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="imagesPaths"></param>
		/// <param name="fileName"></param>
		public static void CreateCBT(string[] imagesPaths, string fileName) => throw new NotImplementedException();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="imagesPaths"></param>
		/// <param name="fileName"></param>
		public static void CreateCB7(string[] imagesPaths, string fileName) => throw new NotImplementedException();

		/// <summary>
		/// Simple funtion to create a hidden directory and copy all files specificataed.
		/// </summary>
		/// <param name="filesPaths">Files wanted to copy on the directory</param>
		/// <returns>Directory info</returns>
		private static DirectoryInfo CreateHiddenDir(string[] filesPaths)
		{
			DirectoryInfo dir = Directory.CreateDirectory(".hidden");
			dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

			if (filesPaths.All(f => !File.Exists(f))) 
				throw new IOException("There is no file to add");

			foreach (var image in filesPaths)
			{
				var finalFilePath = image.Replace(@"\", "/").Split('/');
				File.Copy(image, $"{dir.Name}/{finalFilePath.Last()}", true);
			}

			return dir;
		}
	}
}