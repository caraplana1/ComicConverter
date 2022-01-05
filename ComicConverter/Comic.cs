using System;
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
		}

		public Comic(string comicPath, ComicFormat comicFormat)
		{
			this.comicPath = comicPath;
			this.comicFormat = comicFormat;
		}

		public void Convert(string outputPath, ComicFormat outputFormat)
		{
			throw new NotImplementedException();
		}
	}
}