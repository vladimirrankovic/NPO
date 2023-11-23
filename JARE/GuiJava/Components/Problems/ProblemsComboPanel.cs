/// <summary> AlgorithmsConfigurationPanel.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
// This class is amied at providing a Panel containing all the algorithms
using System;
using ConfigurationsContainer = jmetal.gui.ConfigurationsContainer;
using OkJPanel = jmetal.gui.components.OkJPanel;
using jmetal.gui.utils;
using ProblemWareHouse = jmetal.gui.warehouses.ProblemWareHouse;
namespace jmetal.gui.components.problems
{
	
	[Serializable]
	public class ProblemsComboPanel:OkJPanel, ConfigurationsContainer
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(ProblemsComboPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ProblemsComboPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ProblemsComboPanel enclosingInstance;
			public ProblemsComboPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  itemStateChanged(System.Object event_sender, System.EventArgs arg0)
			{
				if (event_sender is System.Windows.Forms.MenuItem)
					((System.Windows.Forms.MenuItem) event_sender).Checked = !((System.Windows.Forms.MenuItem) event_sender).Checked;
				//UPGRADE_ISSUE: Class 'java.awt.CardLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtCardLayout'"
				//UPGRADE_ISSUE: Method 'java.awt.Container.getLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainergetLayout'"
				CardLayout cl = (CardLayout) (Enclosing_Instance.jpanel_.getLayout());
				//UPGRADE_ISSUE: Method 'java.awt.CardLayout.show' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtCardLayout'"
				//UPGRADE_TODO: Method 'java.awt.event.ItemEvent.getItem' was converted to 'event_sender' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				cl.show(Enclosing_Instance.jpanel_, (System.String) event_sender);
				//UPGRADE_TODO: Method 'java.awt.event.ItemEvent.getItem' was converted to 'event_sender' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				Enclosing_Instance.addConfiguration(null, (System.String) event_sender);
			}
		}
		
		private const long serialVersionUID = - 6109336517949272379L;
		
		
		
		public System.Windows.Forms.ComboBox problemsBox_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection jMetalProperties;
		private System.Windows.Forms.Panel[] paneles;
		private System.String[] names;
		
		
		private System.Windows.Forms.Panel jpanel_;
		
		// AlgorithmsConfigurationPanel
		public ProblemsComboPanel()
		{
			
			jMetalProperties = Configuration.Settings;
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection aux = PropUtils.getPropertiesWithPrefix(jMetalProperties, "PROBLEM.");
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			
			problemsBox_ = new System.Windows.Forms.ComboBox();
			jpanel_ = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.CardLayout.CardLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtCardLayout'"
			jpanel_.setLayout(new CardLayout());
			
			
			System.String name;
			while (iterator.hasNext())
			{
				name = ((System.String) iterator.next());
				ProblemWareHouse.addProblem(name, PropUtils.setDefaultParameters2(jMetalProperties, name));
			}
			
			paneles = new System.Windows.Forms.Panel[(new SupportClass.HashSetSupport(aux).Count)];
			names = new System.String[(new SupportClass.HashSetSupport(aux).Count)];
			
			for (int i = 0; i < ProblemWareHouse.size(); i++)
			{
				name = ProblemWareHouse.getProblemName(i);
				problemsBox_.Items.Add(name);
				paneles[i] = new ConfigureProblemAction(name);
				((ConfigureProblemAction) paneles[i]).draw();
				names[i] = name;
			}
			
			int ancho = - 1, alto = - 1;
			for (int j = 0; j < paneles.Length; j++)
			{
				if (paneles[j].Size.Height > alto)
					alto = paneles[j].Size.Height;
				
				if (paneles[j].Size.Width > ancho)
					ancho = paneles[j].Size.Width;
			}
			
			System.Drawing.Size d = new System.Drawing.Size(0, 0);
			d = new System.Drawing.Size(ancho, alto);
			
			for (int j = 0; j < paneles.Length; j++)
			{
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
				jpanel_.Controls.Add(paneles[j]);
				paneles[j].Dock = new System.Windows.Forms.DockStyle();
				paneles[j].BringToFront();
			}
			
			
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			setLayout(new GridBagLayout());
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints c = new GridBagConstraints();
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.insets' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.insets = new System.Int32[]{2, 2, 2, 2};
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weighty = 1.0;
			
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridheight = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridwidth = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridy = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.CENTER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.anchor = GridBagConstraints.CENTER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.fill = GridBagConstraints.HORIZONTAL;
			System.Windows.Forms.Label temp_label2;
			temp_label2 = new System.Windows.Forms.Label();
			temp_label2.Text = "Problem";
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = temp_label2;
			Controls.Add(temp_Control);
			temp_Control.Dock = new System.Windows.Forms.DockStyle();
			temp_Control.BringToFront();
			
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridheight = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridwidth = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridx = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridy = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.CENTER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.anchor = GridBagConstraints.CENTER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			Controls.Add(problemsBox_);
			problemsBox_.Dock = new System.Windows.Forms.DockStyle();
			problemsBox_.BringToFront();
			
			
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridheight = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridwidth = 2;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridx = 0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridy = 1;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.SOUTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.anchor = GridBagConstraints.SOUTH;
			//  c.fill = GridBagConstraints.VERTICAL;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			Controls.Add(jpanel_);
			jpanel_.Dock = new System.Windows.Forms.DockStyle();
			jpanel_.BringToFront();
			
			
			problemsBox_.SelectedValueChanged += new System.EventHandler(new AnonymousClassItemListener(this).itemStateChanged);
		}
		
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
				
				
				if (configuration != null)
				{
					//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
					while (iterator.hasNext())
					{
						System.String next = (System.String) iterator.next();
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						ProblemWareHouse.getSettings(i)[next] = configuration.Get(next);
					}
				}
			}
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual System.Collections.Specialized.NameValueCollection getConfiguration(System.String name)
		{
			
			
			int i = 0;
			bool found = false;
			
			while ((i < names.Length) && !found)
			{
				if (names[i].Equals(name))
					found = true;
				else
					i++;
			}
			
			if (!found)
				return null;
			else
				return ((ConfigureProblemAction) paneles[i]).getConfiguration(name);
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public override void  draw()
		{
			throw new System.NotSupportedException("Not supported yet.");
		}
	} // ProblemsConfigurationPanel
}