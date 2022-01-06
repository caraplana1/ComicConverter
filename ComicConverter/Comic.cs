using System;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.SevenZip;
using System.Linq;
using System.Collections.Generic;

namespace ComicConverter
{
	public class Comic
	{
		#region Field Declaration

		private readonly string comicPath;
		private ComicFormat comicFormat;

		#endregion

		public Comic(string comicPath)
		{
			this.comicPath = comicPath;
			this.comicFormat = GetComicFormat();
		}

		public void Convert(string outputPath, ComicFormat format)
		{
			this.comicPath = comicPath;
			this.comicFormat = comicFormat;
		}

		private ComicFormat GetComicFormat()
		{
			string[] comicpathComponents = this.comicPath.Split('.');

			if (RarArchive.IsRarFile(comicPath))
				return ComicFormat.CBR;
			if (ZipArchive.IsZipFile(comicPath))
				return ComicFormat.CBZ;
			if (TarArchive.IsTarFile(comicPath))
				return ComicFormat.CBT;
			if (SevenZipArchive.IsSevenZipFile(comicPath))
				return ComicFormat.CB7;

			string invalidFormat = null;

			if (comicpathComponents.Length > 1)
				invalidFormat = comicpathComponents.Last();

			if (invalidFormat is not null)
				throw new FormatException($"The comic cannot be used beacuse is a {invalidFormat.ToUpper()}");
			else
				throw new FormatException("The comic cannot be used");
		}

		}

		private Action<string, string> GetExtractorImageMethod(ComicFormat format)
		{
			throw new NotImplementedException();
		}
	}
}