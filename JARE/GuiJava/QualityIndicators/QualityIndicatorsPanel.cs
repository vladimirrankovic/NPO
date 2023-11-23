/// <summary> QualityIndicatorsPanel.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
// This class is amied at providing a Panel containing all the qualityIndicators
using System;
namespace jmetal.gui.qualityIndicators
{
	
	public class QualityIndicatorsPanel
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(QualityIndicatorsPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(QualityIndicatorsPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private QualityIndicatorsPanel enclosingInstance;
			public QualityIndicatorsPanel Enclosing_Instance
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
			// Return a Panel containing all the qualityIndicators
			
			get
			{
				// The base container is a ScrollPane() container
				System.Windows.Forms.Label temp_label;
				temp_label = new System.Windows.Forms.Label();
				temp_label.Text = "Quality Indicators";
				System.Windows.Forms.Label label = temp_label;
				
				// Panel containing the information
				System.Windows.Forms.Panel panelContainer = new System.Windows.Forms.Panel();
				
				
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
				
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridx = 0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridy = 0;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridheight = 1;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.gridwidth = 1;
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.NORTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
				c.anchor = GridBagConstraints.NORTH;
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
				panelContainer.Controls.Add(label);
				label.Dock = new System.Windows.Forms.DockStyle();
				label.BringToFront();
				
				qualityIndicatorsBox_ = new System.Windows.Forms.CheckBox[qualityIndicators_.Length];
				
				for (int i = 0; i < qualityIndicators_.Length; i++)
				{
					qualityIndicatorsBox_[i] = SupportClass.CheckBoxSupport.CreateCheckBox(qualityIndicators_[i]);
					qualityIndicatorsBox_[i].CheckedChanged += new System.EventHandler(new AnonymousClassItemListener(this).itemStateChanged);
					
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridx = 0;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridy' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridy = i + 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridheight = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.gridwidth = 1;
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.anchor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.NORTHWEST' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
					c.anchor = GridBagConstraints.NORTHWEST;
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
					panelContainer.Controls.Add(qualityIndicatorsBox_[i]);
					qualityIndicatorsBox_[i].Dock = new System.Windows.Forms.DockStyle();
					qualityIndicatorsBox_[i].BringToFront();
				}
				
				
				//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
				System.Windows.Forms.ScrollableControl temp_scrollablecontrol;
				temp_scrollablecontrol = new System.Windows.Forms.ScrollableControl();
				temp_scrollablecontrol.AutoScroll = true;
				temp_scrollablecontrol.Controls.Add(panelContainer);
				System.Windows.Forms.ScrollableControl basePanel = temp_scrollablecontrol;
				System.Drawing.Size d = new System.Drawing.Size(WIDTH_, HEIGHT_);
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
				basePanel.setMinimumSize(d);
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMaximumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMaximumSize_javaawtDimension'"
				basePanel.setMaximumSize(d);
				//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
				basePanel.Size = d;
				
				
				
				return basePanel;
				//return panelContainer;
			}
			// getPanel
			
		}
		virtual public System.String[] SelectedQualityIndicators
		{
			// getSelectedQualityIndicators
			
			get
			{
				System.String[] result = new System.String[selectedCount_];
				int index = 0;
				for (int i = 0; i < qualityIndicators_.Length; i++)
				{
					if (qualityIndicatorsBox_[i].Checked)
					{
						result[index] = qualityIndicators_[i];
						index++;
					}
				}
				return result;
			}
			
		}
		
		private const int WIDTH_ = 150;
		private const int HEIGHT_ = 400;
		
		private System.String[] qualityIndicators_ = new System.String[]{"HV", "SPREAD", "EPSILON", "IGD"};
		
		private System.Windows.Forms.CheckBox[] qualityIndicatorsBox_;
		protected internal int selectedCount_;
		
		// QualityIndicatorsPanel
		public QualityIndicatorsPanel()
		{
			selectedCount_ = 0;
		} // QualityIndicatorsPanel
	} // QualityIndicatorsPanel
}