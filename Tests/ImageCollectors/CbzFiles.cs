using Xunit;
using System.IO;
using System.Linq;
using ComicConverter;
using System.Collections.Generic;

namespace Test.ImageCollector
{
	public class CbzFiles
	{
		[Fact]
		public void UnZip()
		{
			string path = "ZipTestDir";

			ImageExtractors.UnZip(Samples.CBZPATH, path); // Descompress File.

			Assert.True(Directory.Exists(path)); // Makes sure the directory is created.

			string[] filesExtracted = Directory.GetFiles(path);
			Directory.Delete(path, true);

			Assert.True(filesExtracted.Length > 0); // Makes sure that the directory isnot empty.
		}

		[Fact]
		public void FileIsNotZip()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.UnZip(Samples.TESTPATH));
		}

		[Fact]
		public void FileNotFound()
		{
			Assert.Throws<FileNotFoundException>(() => ImageExtractors.UnZip("FakeFile.txt"));
		}

		[Fact]
		public void EmptyAttributeDirectory()
		{
			string folderToCompare = "TestFolderToCompare";

			// Extract the same file in the current directory and in the output directoty
			ImageExtractors.UnZip(Samples.CBZPATH);
			ImageExtractors.UnZip(Samples.CBZPATH, folderToCompare);

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
		public void EmptyStringDirectory()
		{
			Assert.Throws<System.FormatException>(() => ImageExtractors.UnZip(Samples.CBZPATH, ""));
		}
	}
}