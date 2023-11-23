/// <summary> OkEvent.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This class implements a new java event
/// 
/// </version>
using System;
namespace jmetal.gui.events
{
	
	
	[Serializable]
	public class OkEvent:System.EventArgs
	{
		
		public OkEvent(System.Object obj):base()
		{
			// Se guarda el identificador del objeto
		}
	}
}