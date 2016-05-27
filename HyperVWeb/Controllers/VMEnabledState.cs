using System;

namespace HyperVWeb.Controllers
{
	public enum VMEnabledState : ushort
	{
		Unknown = 0,
		Enabled = 2,
		Disabled = 3,
		Saved = 6,
		Paused = 9,
		Suspended = 32769,
		Starting = 32770,
		Snapshotting = 32771,
		Saving = 32773,
		Stopping = 32774,
		Pausing = 32776,
		Resuming = 32777
	}
}