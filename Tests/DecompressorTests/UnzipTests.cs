using Xunit;
using System.IO;
using System.Linq;
using ComicConverter;
using System.Collections.Generic;

namespace ImageExtractorsTests
{
	public class UnzipTests
	{
		#region Files Paths Region

		private readonly string cbzPath = "../../../Samples/Crossed 3D.cbz";
		private readonly string cbrPath = "../../../Samples/CROSSED Wish you were here.cbr";
		private readonly string testFilePath = "../../../Samples/File.test";

		#endregion

		/// <summary>
		/// Makes sure that the files are extarcted to a folder
		/// </summary>
		[Fact]
		public void Unzip()
		{
			string path = "ZipTestDir";

			ImageExtractors.UnZip(cbzPath, path); // Descompress File.

			Assert.True(Directory.Exists(path)); // Makes sure the directory is created.

			string[] filesExtracted = Directory.GetFiles(path);
			Directory.Delete(path, true);

			Assert.True(filesExtracted.Length > 0); // Makes sure that the directory isnot empty.
		}

		/// <summary>
		/// Throw an exception if the file is not a zip
		/// </summary>
		[Fact]
		public void FileIsNotZip()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.UnZip(testFilePath));
		}

		/// <summary>
		///	Verifies that only descompress zip files 
		/// </summary>
		[Fact]
		public void FileIsRar()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.UnZip(cbrPath));
		}

		/// <summary>
		/// Throw an error is the file is not found.
		/// </summary>
		[Fact]
		public void FileNotFound()
		{
			Assert.Throws<FileNotFoundException>(() => ImageExtractors.UnZip("FakeFile.txt"));
		}

		/// <summary>
		/// Verifies if the same files are extracted if the second argument is not given.
		/// </summary>
		[Fact]
		public void FolderEmptyArgument()
		{
			string folderToCompare = "TestFolderToCompare";

			// Extract the same file in the current directory and in the output directoty
			ImageExtractors.UnZip(cbzPath);
			ImageExtractors.UnZip(cbzPath, folderToCompare);

			// Get a list of all files in current directory
			List<string> filesExtracted = Directory.GetFiles(".").ToList<string>(); 
			
			foreach (var file in Directory.GetFiles(folderToCompare))
			{
				// Verifies if the files in output directory are in the current and delete them.
				Assert.Contains(file.Replace(folderToCompare, "."), filesExtracted);
				File.Delete(file.Replace(folderToCompare, "."));
			}

			Directory.Delete(folderToCompare, true);
		}

		[Fact]
		public void FolderIsAnEmptyString()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.UnZip(cbzPath, ""));
		}
	}
}