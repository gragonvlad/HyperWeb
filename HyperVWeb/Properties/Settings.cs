using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HyperVWeb.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance;

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>computername\\username1</string>\r\n  <string>computername\\username2</string>\r\n  <string>computername\\username3</string>\r\n</ArrayOfString>")]
		public StringCollection AllowedWindowsUsers
		{
			get
			{
				return (StringCollection)this["AllowedWindowsUsers"];
			}
		}

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("Off")]
		[SettingsDescription("Possible values are: Off, Basic, Windows")]
		public string Auth
		{
			get
			{
				return (string)this["Auth"];
			}
		}

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>testuser1:testpassword1</string>\r\n  <string>testuser2:testpassword2</string>\r\n  <string>testuser3:testpassword3</string>\r\n</ArrayOfString>")]
		[SettingsDescription("username:password One user per line")]
		public StringCollection BasicAuthUserCredentials
		{
			get
			{
				return (StringCollection)this["BasicAuthUserCredentials"];
			}
		}

		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool EnableAuthLogging
		{
			get
			{
				return (bool)this["EnableAuthLogging"];
			}
		}

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("http://0.0.0.0:80/")]
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		public string ListeningUrl
		{
			get
			{
				return (string)this["ListeningUrl"];
			}
		}

		static Settings()
		{
			Settings.defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		}

		public Settings()
		{
		}
	}
}