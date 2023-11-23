/*
* AddAlgorithm.java
* @author Juan J. Durillo
* @version 1.0
*
* Thiss class provide a GUI for adding a new Algorithm
*/
using System;
using AddAlgorithm = jmetal.gui.components.algorithms.AddAlgorithm;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Logger = java.util.logging.Logger;
using PrintAlgorithmsInfo = jmetal.gui.utils.PrintAlgorithmsInfo;
using PrintProblemsInfo = jmetal.gui.utils.PrintProblemsInfo;
namespace jmetal.gui.components.problems
{
	
	public class AddProblem
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(AddProblem enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AddProblem enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AddProblem enclosingInstance;
			public AddProblem Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				Enclosing_Instance.mainFrame_.Visible = false;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener1
		{
			public AnonymousClassActionListener1(AddProblem enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AddProblem enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AddProblem enclosingInstance;
			public AddProblem Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				try
				{
					(new PrintProblemsInfo()).printProblemInfo(Enclosing_Instance.algorithmName_.Text, Enclosing_Instance.packageName_.Text);
				}
				catch (System.FieldAccessException ex)
				{
					Logger.getLogger(typeof(AddProblem).FullName).log(Level.SEVERE, null, ex);
				}
				//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception ex)
				{
					Logger.getLogger(typeof(AddAlgorithm).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.IO.FileNotFoundException ex)
				{
					Logger.getLogger(typeof(AddAlgorithm).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.IO.IOException ex)
				{
					Logger.getLogger(typeof(AddAlgorithm).FullName).log(Level.SEVERE, null, ex);
				}
				//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception ex)
				{
					Logger.getLogger(typeof(AddAlgorithm).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(AddAlgorithm).FullName).log(Level.SEVERE, null, ex);
				}
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				Enclosing_Instance.mainFrame_.Visible = false;
			}
		}
		
		private System.Windows.Forms.Form mainFrame_;
		private System.Windows.Forms.TextBox algorithmName_;
		private System.Windows.Forms.TextBox packageName_;
		
		
		
		
		// Action Listener Behavior
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			mainFrame_ = new System.Windows.Forms.Form();
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			System.Windows.Forms.Control panel = ((System.Windows.Forms.ContainerControl) mainFrame_);
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			panel.Tag = new System.Drawing.Rectangle(3, 2, 0, 0);
			panel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.GridLayoutResize);
			
			System.Windows.Forms.Label temp_label2;
			temp_label2 = new System.Windows.Forms.Label();
			temp_label2.Text = "Problem Name";
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = temp_label2;
			panel.Controls.Add(temp_Control);
			algorithmName_ = new System.Windows.Forms.TextBox();
			//UPGRADE_ISSUE: Method 'javax.swing.JTextField.setColumns' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTextFieldsetColumns_int'"
			algorithmName_.setColumns(20);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			panel.Controls.Add(algorithmName_);
			
			
			System.Windows.Forms.Label temp_label4;
			temp_label4 = new System.Windows.Forms.Label();
			temp_label4.Text = "Package Name";
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control2;
			temp_Control2 = temp_label4;
			panel.Controls.Add(temp_Control2);
			packageName_ = new System.Windows.Forms.TextBox();
			//UPGRADE_ISSUE: Method 'javax.swing.JTextField.setColumns' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTextFieldsetColumns_int'"
			packageName_.setColumns(20);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			panel.Controls.Add(packageName_);
			
			System.Windows.Forms.Button cancelButton = SupportClass.ButtonSupport.CreateStandardButton("Cancel");
			cancelButton.Click += new System.EventHandler(new AnonymousClassActionListener(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(cancelButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			panel.Controls.Add(cancelButton);
			
			
			System.Windows.Forms.Button addButton = SupportClass.ButtonSupport.CreateStandardButton("Add");
			addButton.Click += new System.EventHandler(new AnonymousClassActionListener1(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(addButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			panel.Controls.Add(addButton);
			
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			mainFrame_.Visible = true;
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			mainFrame_.pack();
		}
	} // AddAlgorithm
}