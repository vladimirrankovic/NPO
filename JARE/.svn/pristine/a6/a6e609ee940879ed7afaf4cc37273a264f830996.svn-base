/*
* @author Juan J. Durillo
* @version 1.0
*
* This class provides a single GUI for configuring algorithms
*/
using System;
using ConfigurationsContainer = jmetal.gui.ConfigurationsContainer;
using OkEvent = jmetal.gui.events.OkEvent;
using ConfigureOperatorAction = jmetal.gui.operators.ConfigureOperatorAction;
using OkJFrame = jmetal.gui.components.OkJFrame;
using OkJPanel = jmetal.gui.components.OkJPanel;
using Configuration = jmetal.gui.utils.Configuration;
using PropUtils = jmetal.gui.utils.PropUtils;
using AlgorithmsWareHouse = jmetal.gui.warehouses.AlgorithmsWareHouse;
using OkEventListener = jmetal.gui.listeners.OkEventListener;
namespace jmetal.gui.components.algorithms
{
	
	[Serializable]
	public class ConfigureAlgorithmAction:OkJPanel, ConfigurationsContainer, OkEventListener
	{
		
		private const long serialVersionUID = 8559151680852062304L;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection algorithmParameters_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection crossoverProperties_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection mutationProperties_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection defaultProperties_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection jMetalProperties_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection algorithmOperators_;
		private System.String algorithmName_;
		
		
		
		
		private System.Object[][] windowsContent_;
		private System.Windows.Forms.Panel[] windowsPanel_;
		
		public ConfigureAlgorithmAction(System.String name)
		{
			// save the algorithmName      
			algorithmName_ = name;
			jMetalProperties_ = Configuration.Settings;
			crossoverProperties_ = PropUtils.getPropertiesWithPrefix(jMetalProperties_, "Crossover.");
			mutationProperties_ = PropUtils.getPropertiesWithPrefix(jMetalProperties_, "Mutation.");
			algorithmParameters_ = PropUtils.getPropertiesWithPrefix(jMetalProperties_, algorithmName_ + ".PARAMETER.");
			algorithmOperators_ = PropUtils.getPropertiesWithPrefix(jMetalProperties_, name + ".OPERATOR.");
			defaultProperties_ = AlgorithmsWareHouse.getSettings(name);
		}
		
		public override void  draw()
		{;
			int algorithmComponents = new SupportClass.HashSetSupport(algorithmParameters_).Count + new SupportClass.HashSetSupport(algorithmOperators_).Count;
			windowsContent_ = new System.Object[5][];
			for (int i = 0; i < 5; i++)
			{
				windowsContent_[i] = new System.Object[algorithmComponents + 1];
			}
			windowsContent_[0][0] = null;
			System.Windows.Forms.Label temp_label;
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = "Parameter";
			windowsContent_[1][0] = temp_label;
			System.Windows.Forms.Label temp_label2;
			temp_label2 = new System.Windows.Forms.Label();
			temp_label2.Text = "Type";
			windowsContent_[2][0] = temp_label2;
			System.Windows.Forms.Label temp_label3;
			temp_label3 = new System.Windows.Forms.Label();
			temp_label3.Text = "Value";
			windowsContent_[3][0] = temp_label3;
			windowsContent_[4][0] = null;
			windowsPanel_ = new System.Windows.Forms.Panel[new SupportClass.HashSetSupport(algorithmParameters_).Count + new SupportClass.HashSetSupport(algorithmOperators_).Count];
			int i2 = 0;
			
			iterator = new SupportClass.HashSetSupport(algorithmParameters_).GetEnumerator();
			// Fill the windowsContent with basic parameters
			while (iterator.hasNext())
			{
				windowsContent_[0][++i2] = iterator.next();
				System.String type = algorithmParameters_.Get((System.String) windowsContent_[0][i2]);
				
				
				System.Windows.Forms.Label temp_label4;
				temp_label4 = new System.Windows.Forms.Label();
				temp_label4.Text = (System.String) windowsContent_[0][i2];
				windowsContent_[1][i2] = temp_label4;
				System.Windows.Forms.Label temp_label5;
				temp_label5 = new System.Windows.Forms.Label();
				temp_label5.Text = type;
				windowsContent_[2][i2] = temp_label5;
				windowsContent_[3][i2] = new System.Windows.Forms.TextBox();
				//UPGRADE_ISSUE: Method 'javax.swing.JTextField.setColumns' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTextFieldsetColumns_int'"
				((System.Windows.Forms.TextBox) windowsContent_[3][i2]).setColumns(4);
				
				
				System.String auxValue = defaultProperties_.Get(".DEFAULT." + ((System.String) windowsContent_[0][i2]));
				//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				if (System.Double.Parse(auxValue).equals(System.Double.NaN))
				{
					auxValue = "1/(Number of Variables)";
					//UPGRADE_ISSUE: Method 'javax.swing.JTextField.setColumns' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTextFieldsetColumns_int'"
					((System.Windows.Forms.TextBox) windowsContent_[3][i2]).setColumns(auxValue.Length);
				}
				//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
				((System.Windows.Forms.TextBox) windowsContent_[3][i2]).Text = auxValue;
				windowsContent_[4][i2] = windowsContent_[3][i2];
				
				windowsPanel_[i2 - 1] = new System.Windows.Forms.Panel();
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
				((System.Windows.Forms.Panel) windowsPanel_[i2 - 1]).Controls.Add((System.Windows.Forms.TextBox) windowsContent_[3][i2]);
				//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
				//UPGRADE_ISSUE: Method 'javax.swing.BorderFactory.createTitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBorderFactorycreateTitledBorder_javaxswingborderBorder_javalangString'"
				System.Windows.Forms.ControlPaint.DrawBorder3D(windowsPanel_[i2 - 1].CreateGraphics(), 0, 0, windowsPanel_[i2 - 1].Width, windowsPanel_[i2 - 1].Height, BorderFactory.createTitledBorder(System.Windows.Forms.Border3DStyle.Sunken, (System.String) windowsContent_[0][i2]));
			}
			
			iterator = new SupportClass.HashSetSupport(algorithmOperators_).GetEnumerator();
			// Fill the windowsContent with operators
			while (iterator.hasNext())
			{
				windowsContent_[0][++i2] = iterator.next();
				System.String type = algorithmOperators_.Get((System.String) windowsContent_[0][i2]);
				
				if (type.Equals("Crossover"))
				{
					
					System.Windows.Forms.Label temp_label6;
					temp_label6 = new System.Windows.Forms.Label();
					temp_label6.Text = (System.String) windowsContent_[0][i2];
					windowsContent_[1][i2] = temp_label6;
					System.Windows.Forms.Label temp_label7;
					temp_label7 = new System.Windows.Forms.Label();
					temp_label7.Text = type;
					windowsContent_[2][i2] = temp_label7;
					System.Windows.Forms.ComboBox auxCombo = new System.Windows.Forms.ComboBox();
					//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
					while (iterator2.hasNext())
					{
						System.String crossoverOperator = (System.String) iterator2.next();
						//               if (defaultProperties_.getProperty((String)windowsContent_[0][i]).contains(crossoverOperator)) {
						//                 auxCombo.addItem(crossoverOperator);
						//                 auxCombo.setSelectedItem(crossoverOperator);
						//               } else {
						auxCombo.Items.Add(crossoverOperator);
						//               }
					}
					System.Windows.Forms.Button configureButton = SupportClass.ButtonSupport.CreateStandardButton("...");
					OkJFrame ok = new OkJFrame(algorithmName_);
					ConfigureOperatorAction coa = new ConfigureOperatorAction(algorithmName_, auxCombo);
					ok.OkJPanel = coa;
					
					
					coa.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(ok.catchOkEvent);
					ok.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(this.catchOkEvent);
					configureButton.Click += new System.EventHandler(ok.actionPerformed);
					SupportClass.CommandManager.CheckCommand(configureButton);
					
					//configureButton.addActionListener(new ConfigureOperatorAction(algorithmName_,auxCombo,getFrameContainer()));
					System.Windows.Forms.Panel auxPanel = new System.Windows.Forms.Panel();
					//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
					//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
					//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
					auxPanel.setLayout(new BoxLayout(auxPanel, BoxLayout.X_AXIS));
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					auxPanel.Controls.Add(auxCombo);
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					auxPanel.Controls.Add(configureButton);
					windowsContent_[3][i2] = auxPanel;
					windowsContent_[4][i2] = auxCombo;
					
					windowsPanel_[i2 - 1] = (System.Windows.Forms.Panel) windowsContent_[3][i2];
					//->windowsPanel_[i-1].setBorder(BorderFactory.createTitledBorder(BorderFactory.createLoweredBevelBorder(),(String)windowsContent_[0][i]));
				}
				else if (type.Equals("Mutation"))
				{
					
					System.Windows.Forms.Label temp_label8;
					temp_label8 = new System.Windows.Forms.Label();
					temp_label8.Text = (System.String) windowsContent_[0][i2];
					windowsContent_[1][i2] = temp_label8;
					System.Windows.Forms.Label temp_label9;
					temp_label9 = new System.Windows.Forms.Label();
					temp_label9.Text = type;
					windowsContent_[2][i2] = temp_label9;
					System.Windows.Forms.ComboBox auxCombo = new System.Windows.Forms.ComboBox();
					
					//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
					while (iterator2.hasNext())
					{
						System.String mutationOperator = (System.String) iterator2.next();
						//               if (defaultProperties_.getProperty((String)windowsContent_[0][i]).contains(mutationOperator)) {
						//                 auxCombo.addItem(mutationOperator);
						//                 auxCombo.setSelectedItem(mutationOperator);
						//               } else {
						auxCombo.Items.Add(mutationOperator);
						//               }
					}
					
					
					System.Windows.Forms.Button configureButton = SupportClass.ButtonSupport.CreateStandardButton("...");
					OkJFrame ok = new OkJFrame(algorithmName_);
					ConfigureOperatorAction coa = new ConfigureOperatorAction(algorithmName_, auxCombo);
					ok.OkJPanel = coa;
					
					
					coa.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(ok.catchOkEvent);
					ok.OkEventListenerDelegateVar += new jmetal.gui.listeners.OkEventListenerDelegate(this.catchOkEvent);
					configureButton.Click += new System.EventHandler(ok.actionPerformed);
					SupportClass.CommandManager.CheckCommand(configureButton);
					
					System.Windows.Forms.Panel auxPanel = new System.Windows.Forms.Panel();
					//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
					//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
					//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
					auxPanel.setLayout(new BoxLayout(auxPanel, BoxLayout.X_AXIS));
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					auxPanel.Controls.Add(auxCombo);
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					auxPanel.Controls.Add(configureButton);
					windowsContent_[3][i2] = auxPanel;
					windowsContent_[4][i2] = auxCombo;
					
					windowsPanel_[i2 - 1] = (System.Windows.Forms.Panel) windowsContent_[3][i2];
					//->windowsPanel_[i-1].setBorder(BorderFactory.createTitledBorder(BorderFactory.createLoweredBevelBorder(),(String)windowsContent_[0][i]));
				}
			}
			
			
			//      // Place the windowsContent in the panel
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints c = new GridBagConstraints();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			setLayout(new GridBagLayout());
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.insets' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.insets = new System.Int32[]{2, 2, 2, 2};
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weighty = 1.0;
			int k = 1;
			for (int j = 0; j < windowsContent_[0].Length; j++)
			{
				k = 1;
				while (k < windowsContent_.Length - 1)
				{
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridx = k - 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridy = j;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridheight = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridwidth = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.WEST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.anchor = GridBagConstraints.WEST;
					if (k == windowsContent_.Length - 1)
					{
						//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
						//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
						c.anchor = GridBagConstraints.EAST;
					}
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
					Controls.Add((System.Windows.Forms.Control) windowsContent_[k][j]);
					((System.Windows.Forms.Control) windowsContent_[k][j]).Dock = new System.Windows.Forms.DockStyle();
					((System.Windows.Forms.Control) windowsContent_[k][j]).BringToFront();
					k++;
				}
			}
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual void  addConfiguration(System.Collections.Specialized.NameValueCollection configuration, System.String name)
		{
			throw new System.NotSupportedException("Not supported yet.");
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual System.Collections.Specialized.NameValueCollection getConfiguration(System.String name)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection algorithmSettings_ = AlgorithmsWareHouse.getSettings(algorithmName_);
			
			
			
			if (windowsContent_.Length > 0)
			{
				for (int index = 1; index < windowsContent_[0].Length; index++)
				{
					if (windowsContent_[4][index].GetType().Equals(typeof(System.Windows.Forms.ComboBox)))
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						System.String operatorName = ((System.Windows.Forms.ComboBox) windowsContent_[4][index]).SelectedItem.ToString();
						
						
						// Add all the properties of the Operator
						//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						System.Collections.Specialized.NameValueCollection operatorSetting = PropUtils.getPropertiesWithPrefix(jMetalProperties_, ((System.Windows.Forms.ComboBox) windowsContent_[4][index]).SelectedItem.ToString());
						System.Collections.IEnumerator it = new SupportClass.HashSetSupport(operatorSetting).GetEnumerator();
						//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
						while (it.MoveNext())
						{
							//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
							System.String next = (System.String) it.Current;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							algorithmSettings_[operatorName + next] = operatorSetting.Get(next);
						}
						
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						algorithmSettings_[".VALUE." + ((System.String) windowsContent_[0][index])] = operatorName;
						
						// Add the parameters of the new opeator
						//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
						System.Collections.Specialized.NameValueCollection aux;
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						aux = PropUtils.getPropertiesWithPrefix(jMetalProperties_, ((System.Windows.Forms.ComboBox) windowsContent_[4][index]).SelectedItem.ToString() + ".PARAMETER.");
						
						System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(aux).GetEnumerator();
						//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
						while (iterator.MoveNext())
						{
							//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
							System.String parameter = (System.String) iterator.Current;
							if (!SupportClass.ContainsKeySupport(algorithmSettings_, operatorName + ".VALUE." + parameter))
							{
								System.String value_Renamed;
								value_Renamed = jMetalProperties_.Get(operatorName + ".DEFAULT." + parameter);
								//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
								algorithmSettings_[operatorName + ".VALUE." + parameter] = value_Renamed;
							}
						}
					}
					else
					{
						System.String auxString = ((System.Windows.Forms.TextBox) windowsContent_[3][index]).Text;
						if (!auxString.Equals("1/(Number of Variables)"))
						{
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							algorithmSettings_[".VALUE." + ((System.String) windowsContent_[0][index])] = ((System.Windows.Forms.TextBox) windowsContent_[3][index]).Text;
						}
						else
						{
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							algorithmSettings_[".VALUE." + ((System.String) windowsContent_[0][index])] = System.Double.NaN + "";
						}
					}
				}
			}
			
			
			
			return algorithmSettings_;
		}
		
		public virtual void  catchOkEvent(System.Object event_sender, OkEvent evt)
		{
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			FrameContainer.Visible = true;
		}
	} // ConfigureAlgorithmAction
}