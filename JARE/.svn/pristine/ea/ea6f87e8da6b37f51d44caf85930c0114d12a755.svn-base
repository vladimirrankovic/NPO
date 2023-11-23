/// <summary> OkEventGenerator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This class should be implemented for all the objects which generates
/// OkJPanel
/// </version>
using System;
using OkEvent = jmetal.gui.events.OkEvent;
using OkEventListener = jmetal.gui.listeners.OkEventListener;
namespace jmetal.gui.generators
{
	
	
	public class OkEventGenerator
	{
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private List < OkEventListener > list_ = new LinkedList < OkEventListener >();
		
		// Add a new OkEventListener Listener
		public event jmetal.gui.listeners.OkEventListenerDelegate OkEventListenerDelegateVar;
		protected virtual void  OnOkEven(jmetal.gui.events.OkEvent eventParam)
		{
			if (OkEventListenerDelegateVar != null)
				OkEventListenerDelegateVar(this, eventParam);
		}
		public virtual void  addOkEvenListener(OkEventListener listener)
		{
			list_.add(listener);
		} // OkEventListener
		
		// Lauch a new OkEvent
		public virtual void  newOkEvent(OkEvent event_Renamed)
		{
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			while (iterator.hasNext())
				iterator.next().catchOkEvent(event_Renamed);
		} // newOkEvent
	} // OkEventGeneratorJPanel
}