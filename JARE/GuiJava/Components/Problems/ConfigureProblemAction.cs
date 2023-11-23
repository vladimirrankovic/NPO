/*
* @author Juan J. Durillo
* @version 1.0
*
* This class provides a single GUI for configuring algorithms
*/
using System;
using ConfigurationsContainer = jmetal.gui.ConfigurationsContainer;
using OkJPanel = jmetal.gui.components.OkJPanel;
using Configuration = jmetal.gui.utils.Configuration;
using PropUtils = jmetal.gui.utils.PropUtils;
using ProblemWareHouse = jmetal.gui.warehouses.ProblemWareHouse;
namespace jmetal.gui.components.problems
{
	
	
	
	[Serializable]
	public class ConfigureProblemAction:OkJPanel, ConfigurationsContainer
	{
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection problemParameters_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection jMetalProperties_;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection defaultProperties_;
		private System.String problemName_;
		//protected ConfigurationsContainer problemConfigurations_;
		
		private System.Object[][] windowsContent_;
		
		// String name, AlgorithmConfigurations algorithmConfigurations
		public ConfigureProblemAction(System.String name)
		{
			// save the algorithmName
			problemName_ = name;
			jMetalProperties_ = Configuration.Settings;
			problemParameters_ = PropUtils.getPropertiesWithPrefix(jMetalProperties_, problemName_ + ".PARAMETER.");
			defaultProperties_ = PropUtils.getPropertiesWithPrefix(ProblemWareHouse.getSettings(problemName_), ".DEFAULT.");
		}
		
		public override void  draw()
		{
			
			if (new SupportClass.HashSetSupport(problemParameters_).Count > 0)
			{
				System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(problemParameters_).GetEnumerator();
				
				
				System.Object[][] tmpArray = new System.Object[4][];
				for (int i = 0; i < 4; i++)
				{
					tmpArray[i] = new System.Object[new SupportClass.HashSetSupport(problemParameters_).Count + 1];
				}
				windowsContent_ = tmpArray;
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
				int i2 = 0;
				// Fill the windowsContent
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iterator.MoveNext())
				{
					
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					windowsContent_[0][++i2] = iterator.Current;
					System.String type = problemParameters_.Get((System.String) windowsContent_[0][i2]);
					
					if (type.Equals("SolutionType"))
					{
						
						System.Windows.Forms.Label temp_label4;
						temp_label4 = new System.Windows.Forms.Label();
						temp_label4.Text = (System.String) windowsContent_[0][i2];
						windowsContent_[1][i2] = temp_label4;
						System.Windows.Forms.Label temp_label5;
						temp_label5 = new System.Windows.Forms.Label();
						temp_label5.Text = type;
						windowsContent_[2][i2] = temp_label5;
						windowsContent_[3][i2] = new System.Windows.Forms.ComboBox();
						
						
						//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
						System.Collections.Specialized.NameValueCollection solutionTypes = PropUtils.getPropertiesWithPrefix(jMetalProperties_, "SOLUTION_TYPE.");
						
						
						System.String defaultType = defaultProperties_.Get((System.String) windowsContent_[0][i2]);
						
						System.Collections.IEnumerator iterator2 = new SupportClass.HashSetSupport(solutionTypes).GetEnumerator();
						((System.Windows.Forms.ComboBox) windowsContent_[3][i2]).Items.Add("");
						//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
						while (iterator2.MoveNext())
						{
							//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
							System.String aux = (System.String) iterator2.Current;
							((System.Windows.Forms.ComboBox) windowsContent_[3][i2]).Items.Add(aux);
							if (aux.ToUpper().Equals(defaultType.ToUpper()))
								((System.Windows.Forms.ComboBox) windowsContent_[3][i2]).SelectedItem = aux;
						}
					}
					else
					{
						
						
						System.Windows.Forms.Label temp_label6;
						temp_label6 = new System.Windows.Forms.Label();
						temp_label6.Text = (System.String) windowsContent_[0][i2];
						windowsContent_[1][i2] = temp_label6;
						System.Windows.Forms.Label temp_label7;
						temp_label7 = new System.Windows.Forms.Label();
						temp_label7.Text = type;
						windowsContent_[2][i2] = temp_label7;
						windowsContent_[3][i2] = new System.Windows.Forms.TextBox();
						//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
						((System.Windows.Forms.TextBox) windowsContent_[3][i2]).Text = defaultProperties_.Get((System.String) windowsContent_[0][i2]);
						//UPGRADE_ISSUE: Method 'javax.swing.JTextField.setColumns' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTextFieldsetColumns_int'"
						((System.Windows.Forms.TextBox) windowsContent_[3][i2]).setColumns(4);
					}
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
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.weightx = 1.0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.weighty = 1.0;
				int k = 1;
				for (int j = 0; j < windowsContent_[0].Length; j++)
				{
					k = 1;
					while (k < windowsContent_.Length)
					{
						if (!windowsContent_[k][j].GetType().Equals(typeof(System.Windows.Forms.ComboBox)))
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
						else
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
							k = windowsContent_.Length;
						}
					}
				}
				
				
				
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridx = 2;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridy = windowsContent_[0].Length;
				//      JButton okButton = new JButton("OK");
				//      okButton.addActionListener(new ActionListener() {
				//
				//        public void actionPerformed(ActionEvent e) {
				//
				//          //problemPanel_.problemSettings[index_] = new Properties();
				//          Properties aux = new Properties();
				//          if (windowsContent_.length > 0) {
				//            for (int index = 1; index < windowsContent_[0].length;index++) {
				//
				//              if (windowsContent_[3][index].getClass().equals(javax.swing.JComboBox.class)) {
				//
				//                if (!((JComboBox)windowsContent_[3][index]).getSelectedItem().equals("")){
				//                  String typeName = ((JComboBox)windowsContent_[3][index]).getSelectedItem().toString();
				//                  aux.setProperty((String)windowsContent_[0][index],typeName);
				//                }
				//
				//              } else {
				//                  // If the textField contains anything, set the default value
				//                  if (!(((JTextField)windowsContent_[3][index]).getText() == null) &&
				//                      !(((JTextField)windowsContent_[3][index]).getText().equals(""))) {
				//                     aux.setProperty((String)windowsContent_[0][index],
				//                                    ((JTextField)windowsContent_[3][index]).getText());
				//                  }
				//              }
				//            }
				//          }
				//
				//          //problemConfigurations_.addConfiguration(aux, problemName_);
				//          ProblemWareHouse.addProblem(problemName_, aux);
				//          newOkEvent(new OkEvent(this));
				//        }
				//
				//     } );
				//      add(okButton,c);
				
				//UPGRADE_ISSUE: Class 'java.awt.Toolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				Toolkit toolkit = Toolkit.getDefaultToolkit();
				System.Drawing.Size screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
				// problemPanel_.mainContainer_.setVisible(false);
				//frame.
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
			//problemPanel_.problemSettings[index_] = new Properties();
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection aux = ProblemWareHouse.getSettings(problemName_);
			
			// If the users wants to use the 2D gui
			if (windowsContent_.Length > 0)
			{
				for (int index = 1; index < windowsContent_[0].Length; index++)
				{
					if (windowsContent_[3][index].GetType().Equals(typeof(System.Windows.Forms.ComboBox)))
					{
						
						if (!((System.Windows.Forms.ComboBox) windowsContent_[3][index]).SelectedItem.Equals(""))
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.String typeName = ((System.Windows.Forms.ComboBox) windowsContent_[3][index]).SelectedItem.ToString();
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							aux[".VALUE." + ((System.String) windowsContent_[0][index])] = typeName;
						}
					}
					else
					{
						// If the textField contains anything, set the default value
						if (!(((System.Windows.Forms.TextBox) windowsContent_[3][index]).Text == null) && !(((System.Windows.Forms.TextBox) windowsContent_[3][index]).Text.Equals("")))
						{
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							aux[".VALUE." + ((System.String) windowsContent_[0][index])] = ((System.Windows.Forms.TextBox) windowsContent_[3][index]).Text;
						}
					}
				}
			}
			
			
			//problemConfigurations_.addConfiguration(aux, problemName_);
			int problemIndex = ProblemWareHouse.getProblemIndex(problemName_);
			if (problemIndex == - 1)
				ProblemWareHouse.addProblem(problemName_, aux);
			else
				ProblemWareHouse.setSettings(problemIndex, aux);
			return aux;
		}
	} // ConfigureAlgorithmAction
}