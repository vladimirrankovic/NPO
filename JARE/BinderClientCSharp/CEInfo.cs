using System;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace BinderClientCSharp
{
	[Serializable]
	public class CEInfo 
	{
		private static readonly long serialVersionUID = -8730805415008937744L;
		private String name;
		private String shortName;
		private int numReadyJobs;
		private String appList;

		public CEInfo() 
		{
		}

		public CEInfo(String name, String shortName, int numReadyJobs, String appList) 
		{
			this.name = name;
			this.shortName = shortName;
			this.numReadyJobs = numReadyJobs;
			this.appList = appList;
		}

		public void customWriteObject(BinaryWriter output /*DataOutputStream out*/) /*throws IOException*/ 
		{
			BinderUtil.writeString(output, name);
			BinderUtil.writeString(output, shortName);
			BinderUtil.writeInt (output,numReadyJobs);
			BinderUtil.writeString(output, appList);
		}

		public void customReadObject(BinaryReader input /*DataInputStream in*/) /*throws EOFException, IOException*/ 
		{
			name = BinderUtil.readString(input);
			shortName = BinderUtil.readString(input);
			numReadyJobs = BinderUtil.readInt (input);
			appList = BinderUtil.readString(input);
		}

		public String getName() 
		{
			return name;
		}

		public String getShortName() 
		{
			return shortName;
		}

		public int getNumReadyJobs() 
		{
			return numReadyJobs;
		}

		public String getAppList() 
		{
			return appList;
		}

		public override String ToString() 
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("CE: ");
			sb.Append(shortName);
			sb.Append(", full name/path: ");
			sb.Append(name);
			sb.Append(", ready jobs: ");
			sb.Append(numReadyJobs);
			sb.Append(", ");
			sb.Append(appList);
			return sb.ToString();
		}
	}
}

