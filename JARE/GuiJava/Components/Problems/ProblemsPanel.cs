/// <summary> ProblemsPanel.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
// This class is amied at providing a Panel containing all the problems
using System;
using ConfigurationsContainer = jmetal.gui.ConfigurationsContainer;
using OkJFrame = jmetal.gui.components.OkJFrame;
using OkJPanel = jmetal.gui.components.OkJPanel;
using OkEvent = jmetal.gui.events.OkEvent;
using OkEventListener = jmetal.gui.listeners.OkEventListener;
using Configuration = jmetal.gui.utils.Configuration;
using PropUtils = jmetal.gui.utils.PropUtils;
using ProblemWareHouse = jmetal.gui.warehouses.ProblemWareHouse;
namespace jmetal.gui.components.problems
{
	
	[Serializable]
	public class ProblemsPanel:OkJPanel, ConfigurationsContainer, OkEventListener
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(ProblemsPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ProblemsPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ProblemsPanel enclosingInstance;
			public ProblemsPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  itemStateChanged(System.Object event_sender, System.EventArgs ev)
			{
				if (event_sender is System.Windows.Forms.MenuItem)
					((System.Windows.Forms.MenuItem) event_sender).Checked = !((System.Windows.Forms.MenuItem) event_sender).Checked;
				System.Windows.Forms.CheckBox jcb = (System.Windows.Forms.CheckBox) event_sender;
				//               if (jcb.isSelected())
				//                  selectedCount_++;
				//               else if (selectedCount_  > 0)
				//                   selectedCount_--;
			}
		}
		virtual public System.Windows.Forms.Control Panel
		{
			// Return a Panel containing all the problems
			
			get
			{
				// The base container is a ScrollPane() container
				//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane'"
				System.Windows.Forms.ScrollableControl temp_scrollablecontrol;
				temp_scrollablecontrol = new System.Windows.Forms.ScrollableControl();
				temp_scrollablecontrol.AutoScroll = true;
				System.Windows.Forms.ScrollableControl basePanel = temp_scrollablecontrol;
				
				System.Drawing.Size d = new System.Drawing.Size(WIDTH_, HEIGHT_);
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
				basePanel.setMinimumSize(d);
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMaximumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMaximumSize_javaawtDimension'"
				basePanel.setMaximumSize(d);
				//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
				basePanel.Size = d;
				
				
				// Panel containing the information
				System.Windows.Forms.Panel panelContainer = new System.Windows.Forms.Panel();
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMaximumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMaximumSize_javaawtDimension'"
				panelContainer.setMaximumSize(d);
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
				panelContainer.setMinimumSize(d);
				//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
				panelContainer.Size = d;
				
				
				
				//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
				//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
				panelContainer.setLayout(new GridBagLayout());
				//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				GridBagConstraints c = new GridBagConstraints();
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.insets' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.insets = new System.Int32[]{2, 2, 2, 2};
				
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.weightx = 1.0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.weighty = 1.0;
				
				System.Windows.Forms.Label temp_label;
				temp_label = new System.Windows.Forms.Label();
				temp_label.Text = "Problems";
				System.Windows.Forms.Label label = temp_label;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridx = 0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridy = 0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridheight = 1;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridwidth = 2;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.CENTER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.anchor = GridBagConstraints.CENTER;
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
				panelContainer.Controls.Add(label);
				label.Dock = new System.Windows.Forms.DockStyle();
				label.BringToFront();
				
				
				
				for (int i = 0; i < ProblemWareHouse.size(); i++)
				{
					System.Windows.Forms.CheckBox tmpCheck = SupportClass.CheckBoxSupport.CreateCheckBox(ProblemWareHouse.getProblemName(i));
					tmpCheck.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener(this).itemStateChanged);
					problemsBox_.add(tmpCheck);
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridx = 0;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridy = i + 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridwidth = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridheight = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.anchor = GridBagConstraints.WEST;
					panelContainer.add(problemsBox_.get_Renamed(i), c);
					
					System.Windows.Forms.Button jbutton = SupportClass.ButtonSupport.CreateStandardButton("...");
					OkJFrame sa = new OkJFrame(ProblemWareHouse.getProblemName(i));
					ConfigureProblemAction cpa = new ConfigureProblemAction(ProblemWareHouse.getProblemName(i));
					sa.OkJPanel = cpa;
					
					cpa.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(sa.catchOkEvent);
					sa.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(this.catchOkEvent);
					jbutton.Click += new System.EventHandler(sa.actionPerformed);
					SupportClass.CommandManager.CheckCommand(jbutton);
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridx = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridy = i + 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridwidth = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridheight = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.anchor = GridBagConstraints.EAST;
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
					panelContainer.Controls.Add(jbutton);
					jbutton.Dock = new System.Windows.Forms.DockStyle();
					jbutton.BringToFront();
				}
				basePanel.Controls.Add(panelContainer);
				return basePanel;
			}
			// getPanel
			
		}
		virtual public System.String[] SelectedProblems
		{
			// getSelectedProblems
			
			get
			{
				
				// Calculates the number of selected problems
				int selectedCount = 0;
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				while (iterator.hasNext())
					if (iterator.next().isSelected())
						selectedCount++;
				
				System.String[] result = new System.String[selectedCount];
				int index = 0;
				for (int i = 0; i < ProblemWareHouse.size(); i++)
				{
					if (problemsBox_.get_Renamed(i).isSelected())
					{
						result[index] = ProblemWareHouse.getProblemName(i);
						index++;
					}
				}
				return result;
			}
			
		}
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		virtual public System.Collections.Specialized.NameValueCollection[] Parameters
		{
			// getSelectedAlgorithms
			
			get
			{
				
				// Calculates the number of selected problems
				int selectedCount = 0;
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				while (iterator.hasNext())
					if (iterator.next().isSelected())
						selectedCount++;
				
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Collections.Specialized.NameValueCollection[] result = new System.Collections.Specialized.NameValueCollection[selectedCount];
				int index = 0;
				for (int i = 0; i < ProblemWareHouse.size(); i++)
				{
					if (problemsBox_.get_Renamed(i).isSelected())
					{
						result[index] = ProblemWareHouse.getSettings(i);
						index++;
					}
				}
				return result;
			}
			
		}
		virtual public System.String[] SelectedProblemsFronts
		{
			// getSelectedProblems
			
			get
			{
				// Calculates the number of selected problems
				int selectedCount = 0;
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				while (iterator.hasNext())
					if (iterator.next().isSelected())
						selectedCount++;
				
				
				System.String[] result = new System.String[selectedCount];
				int index = 0;
				for (int i = 0; i < ProblemWareHouse.size(); i++)
				{
					if (problemsBox_.get_Renamed(i).isSelected())
					{
						result[index] = ProblemWareHouse.getProblemName(i) + ".pf";
						index++;
					}
				}
				return result;
			}
			
		}
		
		
		private const int WIDTH_ = 200;
		private const int HEIGHT_ = 400;
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection jMetalProperties_;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private List < JCheckBox > problemsBox_;
		
		// ProblemsPanel
		public ProblemsPanel()
		{
			jMetalProperties_ = Configuration.Settings;
			
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection aux = PropUtils.getPropertiesWithPrefix(jMetalProperties_, "PROBLEM.");
			System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(aux).GetEnumerator();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			problemsBox_ = new ArrayList < JCheckBox >(aux.keySet().size());
			
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.String problemName = (System.String) iterator.Current;
				ProblemWareHouse.addProblem(problemName, PropUtils.setDefaultParameters2(jMetalProperties_, problemName));
				// MOD-1      problemsBox_.add(new JCheckBox(problemName));
			}
		} // ProblemsPanel
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual void  addConfiguration(System.Collections.Specialized.NameValueCollection configuration, System.String name)
		{
			int i = 0;
			while ((i < ProblemWareHouse.size()) && (!name.Equals(ProblemWareHouse.getProblemName(i))))
			{
				i++;
			}
			if (i < ProblemWareHouse.size())
			{
				if (ProblemWareHouse.getSettings(i) == null)
				{
					//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
					//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
					ProblemWareHouse.setSettings(i, new System.Collections.Specialized.NameValueCollection());
				}
				
				System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(configuration).GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iterator.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					System.String next = (System.String) iterator.Current;
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					ProblemWareHouse.getSettings(i)[next] = configuration.Get(next);
				}
			}
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual System.Collections.Specialized.NameValueCollection getConfiguration(System.String name)
		{
			int i = 0;
			while ((i < ProblemWareHouse.size()) && (!name.Equals(ProblemWareHouse.getProblemName(i))))
			{
				i++;
			}
			if (i < ProblemWareHouse.size())
				return ProblemWareHouse.getSettings(i);
			else
				return null;
		}
		
		public virtual void  catchOkEvent(System.Object event_sender, OkEvent evt)
		{
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			FrameContainer.Visible = true;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public override void  draw()
		{
			throw new System.NotSupportedException("Not supported yet.");
		}
	} // ProblemsPanel
}