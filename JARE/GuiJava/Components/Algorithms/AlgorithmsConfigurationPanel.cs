/// <summary> AlgorithmsConfigurationPanel.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
// This class is amied at providing a Panel containing all the algorithms
using System;
using OkJFrame = jmetal.gui.components.OkJFrame;
using jmetal.gui;
using OkJPanel = jmetal.gui.components.OkJPanel;
using OkEvent = jmetal.gui.events.OkEvent;
using OkEventListener = jmetal.gui.listeners.OkEventListener;
using Configuration = jmetal.gui.utils.Configuration;
using PropUtils = jmetal.gui.utils.PropUtils;
using AlgorithmsWareHouse = jmetal.gui.warehouses.AlgorithmsWareHouse;
namespace jmetal.gui.components.algorithms
{
	
	[Serializable]
	public class AlgorithmsConfigurationPanel:OkJPanel, ConfigurationsContainer, OkEventListener
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(AlgorithmsConfigurationPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AlgorithmsConfigurationPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AlgorithmsConfigurationPanel enclosingInstance;
			public AlgorithmsConfigurationPanel Enclosing_Instance
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
				if (jcb.Checked)
					Enclosing_Instance.selectedCount_++;
				else if (Enclosing_Instance.selectedCount_ > 0)
					Enclosing_Instance.selectedCount_--;
			}
		}
		virtual public System.Windows.Forms.Control Panel
		{
			get
			{
				return basePanel_;
			}
			
		}
		virtual public System.String[] SelectedAlgorithms
		{
			// getSelectedAlgorithms
			
			get
			{
				System.String[] result = new System.String[selectedCount_];
				
				int index = 0;
				for (int i = 0; i < AlgorithmsWareHouse.size(); i++)
				{
					if (algorithmsBox_.get_Renamed(i).isSelected())
					{
						
						result[index] = AlgorithmsWareHouse.getAlgorithmName(i);
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
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Collections.Specialized.NameValueCollection[] result = new System.Collections.Specialized.NameValueCollection[selectedCount_];
				
				int index = 0;
				for (int i = 0; i < AlgorithmsWareHouse.size(); i++)
				{
					if (algorithmsBox_.get_Renamed(i).isSelected())
					{
						algoritmsCaa_.get_Renamed(i).draw();
						result[index] = algoritmsCaa_.get_Renamed(i).getConfiguration(AlgorithmsWareHouse.getAlgorithmName(i)); ;
						index++;
					}
				}
				return result;
			}
			
		}
		
		/// <summary> </summary>
		private const long serialVersionUID = 1L;
		private const int WIDTH_ = 150;
		private const int HEIGHT_ = 400;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private List < JCheckBox > algorithmsBox_;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private List < ConfigureAlgorithmAction > algoritmsCaa_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection jMetalProperties_;
		private int selectedCount_;
		
		
		//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		private System.Windows.Forms.ScrollableControl basePanel_ = null;
		
		// AlgorithmsConfigurationPanel
		public AlgorithmsConfigurationPanel()
		{
			selectedCount_ = 0;
			jMetalProperties_ = Configuration.Settings;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection aux = PropUtils.getPropertiesWithPrefix(jMetalProperties_, "Algorithm.");
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			algorithmsBox_ = new ArrayList < JCheckBox >(aux.keySet().size());
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			algoritmsCaa_ = new ArrayList < ConfigureAlgorithmAction >(aux.keySet().size());
			while (iterator.hasNext())
			{
				System.String algorithmName = (System.String) iterator.next();
				AlgorithmsWareHouse.addAlgorithm(algorithmName, PropUtils.setDefaultParameters2(jMetalProperties_, algorithmName));
			}
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol;
			temp_scrollablecontrol = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol.AutoScroll = true;
			basePanel_ = temp_scrollablecontrol;
			drawPanel();
		} // AlgorithmsConfigurationPanel
		
		
		public virtual void  addAlgorithm()
		{
			Configuration.reload();
			jMetalProperties_ = Configuration.Settings;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection aux = PropUtils.getPropertiesWithPrefix(jMetalProperties_, "Algorithm.");
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			
			while (iterator.hasNext())
			{
				System.String algorithmName = (System.String) iterator.next();
				int algorithmIndex = AlgorithmsWareHouse.getAlgorithmIndex(algorithmName);
				if (algorithmIndex == - 1)
				{
					AlgorithmsWareHouse.addAlgorithm(algorithmName, PropUtils.setDefaultParameters2(jMetalProperties_, algorithmName));
					algorithmsBox_.add(SupportClass.CheckBoxSupport.CreateCheckBox(algorithmName));
				}
			}
			selectedCount_ = 0;
			drawPanel();
		}
		
		// Return a Panel containing all the algorithms
		public virtual void  drawPanel()
		{
			// The base container is a ScrollPane() container
			System.Drawing.Size d = new System.Drawing.Size(WIDTH_, HEIGHT_);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
			basePanel_.setMinimumSize(d);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMaximumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMaximumSize_javaawtDimension'"
			basePanel_.setMaximumSize(d);
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
			basePanel_.Size = d;
			
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
			
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridy = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weighty = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.CENTER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.anchor = GridBagConstraints.CENTER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridwidth = 2;
			System.Windows.Forms.Label temp_label;
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = "Algorithms";
			System.Windows.Forms.Label label = temp_label;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			panelContainer.Controls.Add(label);
			label.Dock = new System.Windows.Forms.DockStyle();
			label.BringToFront();
			//  algorithmsBox_ = new JCheckBox[algorithms_.length];
			
			for (int i = 0; i < AlgorithmsWareHouse.size(); i++)
			{
				System.Windows.Forms.CheckBox tmpCheck = SupportClass.CheckBoxSupport.CreateCheckBox(AlgorithmsWareHouse.getAlgorithmName(i));
				tmpCheck.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener(this).itemStateChanged);
				algorithmsBox_.add(tmpCheck);
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridx = 0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridy = i + 1;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridwidth = 1;
				//c.anchor = GridBagConstraints.WEST;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.anchor = GridBagConstraints.WEST;
				//c.fill  = GridBagConstraints.FIRST_LINE_START;
				panelContainer.add(algorithmsBox_.get_Renamed(i), c);
				
				System.Windows.Forms.Button configureButton = SupportClass.ButtonSupport.CreateStandardButton("...");
				OkJFrame sa = new OkJFrame(AlgorithmsWareHouse.getAlgorithmName(i));
				ConfigureAlgorithmAction caa = new ConfigureAlgorithmAction(AlgorithmsWareHouse.getAlgorithmName(i));
				algoritmsCaa_.add(caa);
				sa.OkJPanel = caa;
				
				caa.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(sa.catchOkEvent);
				sa.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(this.catchOkEvent);
				configureButton.Click += new System.EventHandler(sa.actionPerformed);
				SupportClass.CommandManager.CheckCommand(configureButton);
				
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridx = 1;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridy = i + 1;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridwidth = 1;
				//c.anchor = GridBagConstraints.EAST;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.anchor = GridBagConstraints.EAST;
				//c.fill  = GridBagConstraints.FIRST_LINE_START;
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
				panelContainer.Controls.Add(configureButton);
				configureButton.Dock = new System.Windows.Forms.DockStyle();
				configureButton.BringToFront();
			}
			
			basePanel_.Controls.Add(panelContainer);
			
			//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
			basePanel_.Refresh();
		} // getPanel
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual void  addConfiguration(System.Collections.Specialized.NameValueCollection configuration, System.String name)
		{
			int i = 0;
			while ((i < AlgorithmsWareHouse.size()) && (!name.Equals(AlgorithmsWareHouse.getAlgorithmName(i))))
			{
				i++;
			}
			if (i < AlgorithmsWareHouse.size())
			{
				if (AlgorithmsWareHouse.getSettings(i) == null)
				{
					//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
					//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
					AlgorithmsWareHouse.setSettings(i, new System.Collections.Specialized.NameValueCollection());
				}
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				while (iterator.hasNext())
				{
					System.String next = (System.String) iterator.next();
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					AlgorithmsWareHouse.getSettings(i)[next] = configuration.Get(next);
				}
			}
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual System.Collections.Specialized.NameValueCollection getConfiguration(System.String name)
		{
			int i = 0;
			while ((i < AlgorithmsWareHouse.size()) && (!name.Equals(AlgorithmsWareHouse.getAlgorithmName(i))))
			{
				i++;
			}
			if (i < AlgorithmsWareHouse.size())
				return AlgorithmsWareHouse.getSettings(i);
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
	} // AlgorithmsConfigurationPanel
}