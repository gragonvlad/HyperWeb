using System;

namespace HyperVWeb.Models
{
	public class CreateVM
	{
		public int DiskGb = 40;

		public int MemoryMb = 512;

		public string Name;

		public string NetworkName;

		public string Path;

		public CreateVM()
		{
		}
	}
}