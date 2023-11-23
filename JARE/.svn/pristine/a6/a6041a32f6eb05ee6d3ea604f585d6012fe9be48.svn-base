/// <summary> ExecutionGUI.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This class is aimed at executing and showing the Pareto Front computed by
/// a multi-objective algorithm
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Logger = java.util.logging.Logger;
using Plot2Da = jmetal.gui.plot.Plot2Da;
using Algorithm = jmetal.base_Renamed.Algorithm;
using Problem = jmetal.base_Renamed.Problem;
using BinaryTournament2 = jmetal.base_Renamed.operator_Renamed.selection.BinaryTournament2;
using AddAlgorithm = jmetal.gui.components.algorithms.AddAlgorithm;
using AddOperator = jmetal.gui.operators.AddOperator;
using AddProblem = jmetal.gui.components.problems.AddProblem;
using ProblemFactory = jmetal.problems.ProblemFactory;
using JMException = jmetal.util.JMException;
namespace jmetal.gui
{
	
	[Serializable]
	public class SimpleExecutionSupportGUI:System.Windows.Forms.Form
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(SimpleExecutionSupportGUI enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(SimpleExecutionSupportGUI enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SimpleExecutionSupportGUI enclosingInstance;
			public SimpleExecutionSupportGUI Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			SuppressWarnings(empty-statement)
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Problem problem = null; // The problem to solve
				
				
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Collections.Specialized.NameValueCollection problemSettings = Enclosing_Instance.sep.pcp.getConfiguration((System.String) Enclosing_Instance.sep.pcp.problemsBox_.SelectedItem);
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Collections.Specialized.NameValueCollection algorithmSettings = Enclosing_Instance.sep.acp.getConfiguration((System.String) Enclosing_Instance.sep.acp.algorithmsBox_.SelectedItem);
				System.String problemName = (System.String) Enclosing_Instance.sep.pcp.problemsBox_.SelectedItem;
				System.String algorithmName = (System.String) Enclosing_Instance.sep.acp.algorithmsBox_.SelectedItem;
				
				try
				{
					
					if ((problemSettings == null) || (problemSettings.Count == 0))
					{
						System.Object[] params_Renamed = new System.Object[]{}; // Parameters of the problem
						problem = (new ProblemFactory()).getProblem(problemName, params_Renamed);
					}
					else
					{
						problem = (new jmetal.gui.utils.PrintProblemsInfo()).getProblem(problemName, problemSettings);
					}
					
					if (problem.NumberOfObjectives > 2)
					{
						SupportClass.OptionPaneSupport.ShowMessageDialog(null, "Sorry, current version of Single Execution Support GUI works only with bi-objective problems");
					}
					else
					{
						
						
						try
						{
							System.Object[] settingsParams = new System.Object[]{problem};
							Algorithm algorithm_ = (new jmetal.gui.utils.PrintAlgorithmsInfo()).getAlgorithm(algorithmName, algorithmSettings, problem);
							
							long initTime;
							long estimatedTime;
							initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
							
							algorithm_.execute().printObjectivesToFile("Success!");
							estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
						}
						//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
						catch (System.Exception e1)
						{
							// TODO Auto-generated catch block
							//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
							e1.printStackTrace();
						}
						
						System.Drawing.Color tempAux = Enclosing_Instance.jcc.Color;
						//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
						Enclosing_Instance.plot2D.plot_.refresh("Success!", ref tempAux, Enclosing_Instance.plot2D.check_.Checked);
						//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
						Enclosing_Instance.plot2D.plot_.Refresh();
						System.IO.FileInfo f = new System.IO.FileInfo("Success!");
						bool tmpBool;
						if (System.IO.File.Exists(f.FullName))
						{
							System.IO.File.Delete(f.FullName);
							tmpBool = true;
						}
						else if (System.IO.Directory.Exists(f.FullName))
						{
							System.IO.Directory.Delete(f.FullName);
							tmpBool = true;
						}
						else
							tmpBool = false;
						bool generatedAux8 = tmpBool;
					}
				}
				catch (System.ArgumentException ex)
				{
					Logger.getLogger(typeof(SimpleExecutionSupportGUI).FullName).log(Level.SEVERE, null, ex);
				}
				catch (JMException ex)
				{
					Logger.getLogger(typeof(SimpleExecutionSupportGUI).FullName).log(Level.SEVERE, null, ex);
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener1
		{
			public AnonymousClassActionListener1(SimpleExecutionSupportGUI enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(SimpleExecutionSupportGUI enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SimpleExecutionSupportGUI enclosingInstance;
			public SimpleExecutionSupportGUI Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			SuppressWarnings(empty-statement)
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Environment.Exit(0);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener2' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener2
		{
			public AnonymousClassActionListener2(SimpleExecutionSupportGUI enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(SimpleExecutionSupportGUI enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SimpleExecutionSupportGUI enclosingInstance;
			public SimpleExecutionSupportGUI Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			SuppressWarnings(empty-statement)
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.Form auxFrame = SupportClass.FormSupport.CreateForm("Select a new color");
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
				auxFrame.Controls.Add(Enclosing_Instance.jcc);
				//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
				auxFrame.Size = Enclosing_Instance.jcc.Size;
				//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
				auxFrame.pack();
				//UPGRADE_ISSUE: Class 'java.awt.Toolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				Toolkit toolkit = Toolkit.getDefaultToolkit();
				System.Drawing.Size screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
				//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
				auxFrame.Location = new System.Drawing.Point(screenSize.Width / 2 - auxFrame.Width / 2, screenSize.Height / 2 - auxFrame.Height / 2);
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				auxFrame.Visible = true;
			}
		}
		internal SingleExecutionPanel sep = new SingleExecutionPanel();
		internal Plot2Da plot2D = new Plot2Da();
		internal System.Windows.Forms.ColorDialog jcc = new System.Windows.Forms.ColorDialog();
		private System.Windows.Forms.MainMenu menuBar_;
		
		public SimpleExecutionSupportGUI()
		{
			
			// Giving a title to this window
			Text = "jMetal Simple Execution GUI";
			
			// Obtaining the container into the jFrame
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			System.Windows.Forms.Control container = ((System.Windows.Forms.ContainerControl) this);
			
			// Creating a TabbedPane to store the different configurations
			System.Windows.Forms.TabControl tabbed = new System.Windows.Forms.TabControl();
			
			//UPGRADE_TODO: Method 'javax.swing.JTabbedPane.addTab' was converted to 'SupportClass.TabControlSupport.AddTab' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTabbedPaneaddTab_javalangString_javaawtComponent'"
			SupportClass.TabControlSupport.AddTab(tabbed, "Configuration 1", sep);
			
			// Creating a JPanel for the tabbed and the plot areas
			System.Windows.Forms.Panel tabAndPlot = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			tabAndPlot.setLayout(new BoxLayout(tabAndPlot, BoxLayout.X_AXIS));
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			tabAndPlot.Controls.Add(tabbed);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			tabAndPlot.Controls.Add(plot2D);
			
			jcc.setColor(Color.BLACK);
			//UPGRADE_ISSUE: Class 'javax.swing.colorchooser.AbstractColorChooserPanel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingcolorchooserAbstractColorChooserPanel'"
			//UPGRADE_ISSUE: Method 'javax.swing.JColorChooser.getChooserPanels' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJColorChoosergetChooserPanels'"
			AbstractColorChooserPanel[] panels = jcc.getChooserPanels();
			for (int i = 1; i < panels.Length; i++)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.JColorChooser.removeChooserPanel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJColorChooserremoveChooserPanel_javaxswingcolorchooserAbstractColorChooserPanel'"
				jcc.removeChooserPanel(panels[i]);
			}
			//UPGRADE_ISSUE: Method 'javax.swing.JColorChooser.setPreviewPanel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJColorChoosersetPreviewPanel_javaxswingJComponent'"
			jcc.setPreviewPanel(new System.Windows.Forms.Panel());
			
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.Y_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			container.setLayout(new BoxLayout(container, BoxLayout.Y_AXIS));
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			container.Controls.Add(tabAndPlot);
			
			System.Windows.Forms.Button execute = SupportClass.ButtonSupport.CreateStandardButton("Execute");
			execute.Click += new System.EventHandler(new AnonymousClassActionListener(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(execute);
			
			System.Windows.Forms.Panel aux = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			aux.setLayout(new BoxLayout(aux, BoxLayout.X_AXIS));
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = Box.createHorizontalGlue();
			aux.Controls.Add(temp_Control);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			aux.Controls.Add(execute);
			
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalStrut' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control2;
			temp_Control2 = Box.createHorizontalStrut(30);
			container.Controls.Add(temp_Control2);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			container.Controls.Add(aux);
			
			// Adding a MenuBar
			menuBar_ = new System.Windows.Forms.MainMenu();
			
			// File menu
			{
				System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("File");
				System.Int32 tempInt;
				tempInt = menu.Text.ToLower().IndexOf(char.ToLower('F'));
				menu.Text = tempInt >= 0?menu.Text.Insert(tempInt, "&"):menu.Text;
				
				System.Windows.Forms.MenuItem menuItemExit = new System.Windows.Forms.MenuItem("Exit");
				menuItemExit.Click += new System.EventHandler(new AnonymousClassActionListener1(this).actionPerformed);
				SupportClass.CommandManager.CheckCommand(menuItemExit);
				menu.MenuItems.Add(menuItemExit);
				menuBar_.MenuItems.Add(menu);
			}
			
			// Add menu
			{
				System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("Add ...");
				System.Int32 tempInt;
				tempInt = menu.Text.ToLower().IndexOf(char.ToLower('A'));
				menu.Text = tempInt >= 0?menu.Text.Insert(tempInt, "&"):menu.Text;
				
				System.Windows.Forms.MenuItem menuItemAlgorithm = new System.Windows.Forms.MenuItem("Algorithm");
				menuItemAlgorithm.Click += new System.EventHandler(new AddAlgorithm(sep.acp).actionPerformed);
				SupportClass.CommandManager.CheckCommand(menuItemAlgorithm);
				
				System.Windows.Forms.MenuItem menuItemOperator = new System.Windows.Forms.MenuItem("Operator");
				menuItemOperator.Click += new System.EventHandler(new AddOperator().actionPerformed);
				SupportClass.CommandManager.CheckCommand(menuItemOperator);
				System.Windows.Forms.MenuItem menuItemIndicator = new System.Windows.Forms.MenuItem("Quality Indicator");
				
				System.Windows.Forms.MenuItem menuItemProblem = new System.Windows.Forms.MenuItem("Problem");
				menuItemProblem.Click += new System.EventHandler(new AddProblem().actionPerformed);
				SupportClass.CommandManager.CheckCommand(menuItemProblem);
				
				menu.MenuItems.Add(menuItemAlgorithm);
				menu.MenuItems.Add(menuItemOperator);
				menu.MenuItems.Add(menuItemProblem);
				menu.MenuItems.Add(menuItemIndicator);
				menuBar_.MenuItems.Add(menu);
			}
			
			
			// File menu
			{
				System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("Configure");
				System.Int32 tempInt2;
				tempInt2 = menu.Text.ToLower().IndexOf(char.ToLower('C'));
				menu.Text = tempInt2 >= 0?menu.Text.Insert(tempInt2, "&"):menu.Text;
				
				System.Windows.Forms.MenuItem menuItemConfigure = new System.Windows.Forms.MenuItem("Change Color");
				
				menuItemConfigure.Click += new System.EventHandler(new AnonymousClassActionListener2(this).actionPerformed);
				SupportClass.CommandManager.CheckCommand(menuItemConfigure);
				menu.MenuItems.Add(menuItemConfigure);
				menuBar_.MenuItems.Add(menu);
			}
			
			Menu = menuBar_;
			
			// Setting the size of the Window
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			
			// Establishing the Default Location of the windows
			
			//UPGRADE_ISSUE: Class 'java.awt.Toolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			Toolkit toolkit = Toolkit.getDefaultToolkit();
			System.Drawing.Size screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
			//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
			Location = new System.Drawing.Point(screenSize.Width / 2 - Size.Width / 2, screenSize.Height / 2 - Size.Height / 2);
			
			// Making the frame visible
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			Visible = true;
		}
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			new SimpleExecutionSupportGUI();
		}
	}
}