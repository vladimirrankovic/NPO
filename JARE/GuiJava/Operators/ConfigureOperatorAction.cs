/*
* @author Juan J. Durillo
* @version 1.0
*
* This class provides a single GUI for configuring algorithms
*/
using System;
using ConfigurationsContainer = jmetal.gui.ConfigurationsContainer;
using PropUtils = jmetal.gui.utils.PropUtils;
using Configuration = jmetal.gui.utils.Configuration;
using OkJPanel = jmetal.gui.components.OkJPanel;
using AlgorithmsWareHouse = jmetal.gui.warehouses.AlgorithmsWareHouse;
namespace jmetal.gui.operators
{
	
	[Serializable]
	public class ConfigureOperatorAction:OkJPanel, ConfigurationsContainer
	{
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection properties_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection defaultProperties_;
		private System.String operatorName_;
		private ConfigurationsContainer algorithmConfigurations_;
		
		private System.Object[][] windowsContent_;
		private System.String algorithmName_;
		private System.Windows.Forms.ComboBox combo_;
		
		
		public ConfigureOperatorAction(System.String algorithmName, System.Windows.Forms.ComboBox combo)
		{
			combo_ = combo;
			algorithmName_ = algorithmName;
		}
		
		public override void  draw()
		{
			operatorName_ = ((System.String) combo_.SelectedItem);
			
			// load algorithm properties		
			properties_ = PropUtils.getPropertiesWithPrefix(Configuration.Settings, operatorName_ + ".PARAMETER.");
			defaultProperties_ = PropUtils.getPropertiesWithPrefix(Configuration.Settings, operatorName_ + ".DEFAULT.");
			
			
			System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(properties_).GetEnumerator();
			
			windowsContent_ = new System.Object[5][];
			for (int i = 0; i < 5; i++)
			{
				windowsContent_[i] = new System.Object[new SupportClass.HashSetSupport(properties_).Count + 1];
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
			temp_label3.Text = "Default";
			windowsContent_[3][0] = temp_label3;
			System.Windows.Forms.Label temp_label4;
			temp_label4 = new System.Windows.Forms.Label();
			temp_label4.Text = "Value";
			windowsContent_[4][0] = temp_label4;
			int i2 = 0;
			// Fill the windowsContent
			
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				windowsContent_[0][++i2] = iterator.Current;
				System.String type = properties_.Get((System.String) windowsContent_[0][i2]);
				System.Windows.Forms.Label temp_label5;
				temp_label5 = new System.Windows.Forms.Label();
				temp_label5.Text = (System.String) windowsContent_[0][i2];
				windowsContent_[1][i2] = temp_label5;
				System.Windows.Forms.Label temp_label6;
				temp_label6 = new System.Windows.Forms.Label();
				temp_label6.Text = type;
				windowsContent_[2][i2] = temp_label6;
				System.Windows.Forms.Label temp_label7;
				temp_label7 = new System.Windows.Forms.Label();
				temp_label7.Text = defaultProperties_.Get((System.String) windowsContent_[0][i2]);
				windowsContent_[3][i2] = temp_label7;
				windowsContent_[4][i2] = new System.Windows.Forms.TextBox();
				
				//UPGRADE_ISSUE: Method 'javax.swing.JTextField.setColumns' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTextFieldsetColumns_int'"
				((System.Windows.Forms.TextBox) windowsContent_[4][i2]).setColumns(4);
				//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
				((System.Windows.Forms.TextBox) windowsContent_[4][i2]).Text = defaultProperties_.Get((System.String) windowsContent_[0][i2]);
			}
			
			// Place the windowsContent in the panel
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints c = new GridBagConstraints();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			setLayout(new GridBagLayout());
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.insets' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.insets = new System.Int32[]{4, 4, 4, 4};
			int k = 1;
			for (int j = 0; j < windowsContent_[0].Length; j++)
			{
				k = 1;
				while (k < windowsContent_.Length)
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
					c.anchor = c.WEST;
					if (k == windowsContent_.Length - 1)
					{
						//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
						//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.EAST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
						c.anchor = c.EAST;
					}
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
					Controls.Add((System.Windows.Forms.Control) windowsContent_[k][j]);
					((System.Windows.Forms.Control) windowsContent_[k][j]).Dock = new System.Windows.Forms.DockStyle();
					((System.Windows.Forms.Control) windowsContent_[k][j]).BringToFront();
					k++;
				}
			}
			
			//      c.gridx = 2;
			//      c.gridy = windowsContent_[0].length;
			//      JButton okButton = new JButton("OK");
			//      okButton.addActionListener(new ActionListener() {
			//
			//        public void actionPerformed(ActionEvent e) {
			//         frame.setVisible(false);
			//
			//          Properties algorithmSettings_ = algorithmConfigurations_.getConfiguration(algorithmName_);
			//          if (windowsContent_.length > 0) {
			//            for (int index = 1; index < windowsContent_[0].length;index++) {
			//
			//                  // If the textField contains anything, set the default value
			//                  if (!(((JTextField)windowsContent_[4][index]).getText() == null) &&
			//                      !(((JTextField)windowsContent_[4][index]).getText().equals(""))) {
			//
			//                     algorithmSettings_.setProperty(
			//                                    operatorName_+"."+(String)windowsContent_[0][index],
			//                                    ((JTextField)windowsContent_[4][index]).getText());
			//                  }
			//            }
			//            //previousFrame_.setVisible(true);
			//          }
			//        }
			//      } );
			//      mainPanel.add(okButton,c);
			//      frame.add(mainPanel);
			//          pack();
			//      frame.setResizable(false);
			//
			//      Toolkit toolkit = Toolkit.getDefaultToolkit();
			//      Dimension screenSize = toolkit.getScreenSize();
			//      frame.setLocation(screenSize.width /2 - frame.getSize().width/2,
			//                        screenSize.height/2 - frame.getSize().width/2);
			//      frame.setVisible(true);
			
			
			//frame.
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
			System.Collections.Specialized.NameValueCollection algorithmSettings = AlgorithmsWareHouse.getSettings(algorithmName_);
			if (windowsContent_.Length > 0)
			{
				for (int index = 1; index < windowsContent_[0].Length; index++)
				{
					// If the textField contains anything, set the default value
					if (!(((System.Windows.Forms.TextBox) windowsContent_[4][index]).Text == null) && !(((System.Windows.Forms.TextBox) windowsContent_[4][index]).Text.Equals("")))
					{
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						algorithmSettings[operatorName_ + ".VALUE." + ((System.String) windowsContent_[0][index])] = ((System.Windows.Forms.TextBox) windowsContent_[4][index]).Text;
					}
				}
				//previousFrame_.setVisible(true);
			}
			int index2 = AlgorithmsWareHouse.getAlgorithmIndex(algorithmName_);
			AlgorithmsWareHouse.setSettings(index2, algorithmSettings);
			return algorithmSettings;
		}
	} // ConfigureAlgorithmAction
}