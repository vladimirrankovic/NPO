/// <summary> OkJPanel.java
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
namespace jmetal.gui.components
{
	
	
	[Serializable]
	public abstract class OkJPanel:System.Windows.Forms.Panel
	{
		virtual public System.Windows.Forms.Control FrameContainer
		{
			get
			{
				System.Windows.Forms.Control c = Parent;
				while (c.Parent != null)
					c = c.Parent;
				return c;
			}
			
		}
		virtual public int Columns
		{
			// Return the number of columns of the panel
			
			get
			{
				return columns_;
			}
			// getColumns;
			
		}
		virtual public int Rows
		{
			// Return the number of rows of the panel
			
			get
			{
				return rows_;
			}
			// getRows;
			
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private List < OkEventListener > list_ = new LinkedList < OkEventListener >();
		private int rows_;
		private int columns_;
		
		public OkJPanel()
		{
			rows_ = 0;
			columns_ = 0;
		}
		
		
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
		
		// Increase the number of columns of the panel
		public virtual void  increaseColumns()
		{
			columns_++;
		} // increaseColumns
		
		// Increase the number rows 
		public virtual void  increaseRows()
		{
			rows_++;
		} // increaseRows
		
		public abstract void  draw();
	} // OkJPanel
}