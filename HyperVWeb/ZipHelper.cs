using HyperVWeb.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;

namespace HyperVWeb
{
	internal static class ZipHelper
	{
		public static Dictionary<string, byte[]> Zipped;

		static ZipHelper()
		{
			ZipArchive zipArchive = new ZipArchive(new MemoryStream(Resources.WebContent));
			HyperVWeb.ZipHelper.Zipped = new Dictionary<string, byte[]>(zipArchive.Entries.Count, StringComparer.InvariantCultureIgnoreCase);
			foreach (ZipArchiveEntry entry in zipArchive.Entries)
			{
				using (Stream stream = entry.Open())
				{
					byte[] numArray = HyperVWeb.ZipHelper.ReadFully(stream);
					HyperVWeb.ZipHelper.Zipped.Add(entry.FullName, numArray);
				}
			}
		}

		public static byte[] ReadFully(Stream input)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				input.CopyTo(memoryStream);
				array = memoryStream.ToArray();
			}
			return array;
		}
	}
}