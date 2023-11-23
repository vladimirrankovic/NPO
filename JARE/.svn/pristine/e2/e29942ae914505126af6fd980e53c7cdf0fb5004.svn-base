/// <summary> StandardStudy.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Logger = java.util.logging.Logger;
using Algorithm = SharpMetal.Base.Algorithm;
using Problem = SharpMetal.Base.Problem;
using GDE3_Settings = SharpMetal.experiments.settings.GDE3_Settings;
using MOCell_Settings = SharpMetal.experiments.settings.MOCell_Settings;
using NSGAII_Settings = SharpMetal.experiments.settings.NSGAII_Settings;
using SPEA2_Settings = SharpMetal.experiments.settings.SPEA2_Settings;
using SMPSO_Settings = SharpMetal.experiments.settings.SMPSO_Settings;
using RBoxplot = SharpMetal.experiments.util.RBoxplot;
using RWilcoxon = SharpMetal.experiments.util.RWilcoxon;
using SMException = SharpMetal.util.SMException;
namespace SharpMetal.experiments
{
	
	/// <author>  Antonio J. Nebro
	/// </author>
	public class StandardStudy:Experiment
	{
		
		/// <summary> Configures the algorithms in each independent run</summary>
		/// <param name="problem">The problem to solve
		/// </param>
		/// <param name="problemIndex">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public override void  algorithmSettings(Problem problem, int problemIndex, Algorithm[] algorithm)
		{
			try
			{
				int numberOfAlgorithms = m_algorithmNameList.Length;
				
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Collections.Specialized.NameValueCollection[] parameters = new System.Collections.Specialized.NameValueCollection[numberOfAlgorithms];
				
				for (int i = 0; i < numberOfAlgorithms; i++)
				{
					//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
					//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
					parameters[i] = new System.Collections.Specialized.NameValueCollection();
				} // for
				
				if (!m_paretoFrontFile[problemIndex].Equals(""))
				{
					for (int i = 0; i < numberOfAlgorithms; i++)
					{
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						parameters[i]["m_paretoFrontFile"] = m_paretoFrontFile[problemIndex];
					}
				} // if
				
				algorithm[0] = new NSGAII_Settings(problem).configure(parameters[0]);
				algorithm[1] = new SPEA2_Settings(problem).configure(parameters[1]);
				algorithm[2] = new MOCell_Settings(problem).configure(parameters[2]);
				algorithm[3] = new SMPSO_Settings(problem).configure(parameters[3]);
				algorithm[4] = new GDE3_Settings(problem).configure(parameters[4]);
			}
			catch (System.ArgumentException ex)
			{
                ////////Logger.getLogger(typeof(StandardStudy).FullName).log(Level.SEVERE, null, ex);
			}
			catch (System.UnauthorizedAccessException ex)
			{
                ////////Logger.getLogger(typeof(StandardStudy).FullName).log(Level.SEVERE, null, ex);
			}
			catch (SMException ex)
			{
                ////////Logger.getLogger(typeof(StandardStudy).FullName).log(Level.SEVERE, null, ex);
			}
		} // algorithmSettings
		
		/// <summary> Main method</summary>
		/// <param name="args">
		/// </param>
		/// <throws>  SMException </throws>
		/// <throws>  IOException </throws>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			StandardStudy exp = new StandardStudy();
			
			exp.m_experimentName = "StandardStudy";
			exp.m_algorithmNameList = new System.String[]{"NSGAII", "SPEA2", "MOCell", "SMPSO", "GDE3"};
			exp.m_problemList = new System.String[]{"ZDT1", "ZDT2", "ZDT3", "ZDT4", "ZDT6", "WFG1", "WFG2", "WFG3", "WFG4", "WFG5", "WFG6", "WFG7", "WFG8", "WFG9", "DTLZ1", "DTLZ2", "DTLZ3", "DTLZ4", "DTLZ5", "DTLZ6", "DTLZ7"};
			exp.m_paretoFrontFile = new System.String[]{"ZDT1.pf", "ZDT2.pf", "ZDT3.pf", "ZDT4.pf", "ZDT6.pf", "WFG1.2D.pf", "WFG2.2D.pf", "WFG3.2D.pf", "WFG4.2D.pf", "WFG5.2D.pf", "WFG6.2D.pf", "WFG7.2D.pf", "WFG8.2D.pf", "WFG9.2D.pf", "DTLZ1.2D.pf", "DTLZ2.2D.pf", "DTLZ3.2D.pf", "DTLZ4.2D.pf", "DTLZ5.2D.pf", "DTLZ6.2D.pf", "DTLZ7.2D.pf"};
			
			exp.m_indicatorList = new System.String[]{"HV", "SPREAD", "EPSILON"};
			
			int numberOfAlgorithms = exp.m_algorithmNameList.Length;
			
			exp.m_experimentBaseDirectory = "/Users/antonio/Softw/pruebas/SharpMetal/" + exp.m_experimentName;
			exp.m_paretoFrontDirectory = "/Users/antonio/Softw/pruebas/data/paretoFronts";
			
			exp.m_algorithmSettings = new Settings[numberOfAlgorithms];
			
			exp.m_independentRuns = 100;
			
			// Run the experiments
			int numberOfThreads;
			exp.runExperiment(numberOfThreads = 4);
			
			// Generate latex tables
			exp.generateLatexTables();
			
			// Configure the R scripts to be generated
			int rows;
			int columns;
			System.String prefix;
			System.String[] problems;
			bool notch;
			
			// Configuring scripts for ZDT
			rows = 3;
			columns = 2;
			prefix = new System.Text.StringBuilder("ZDT").ToString();
			problems = new System.String[]{"ZDT1", "ZDT2", "ZDT3", "ZDT4", "ZDT6"};
			
			exp.generateRBoxplotScripts(rows, columns, problems, prefix, notch = false, exp);
			exp.generateRWilcoxonScripts(problems, prefix, exp);
			
			// Configure scripts for DTLZ
			rows = 3;
			columns = 3;
			prefix = new System.Text.StringBuilder("DTLZ").ToString();
			problems = new System.String[]{"DTLZ1", "DTLZ2", "DTLZ3", "DTLZ4", "DTLZ5", "DTLZ6", "DTLZ7"};
			
			exp.generateRBoxplotScripts(rows, columns, problems, prefix, notch = false, exp);
			exp.generateRWilcoxonScripts(problems, prefix, exp);
			
			// Configure scripts for WFG
			rows = 3;
			columns = 3;
			prefix = new System.Text.StringBuilder("WFG").ToString();
			problems = new System.String[]{"WFG1", "WFG2", "WFG3", "WFG4", "WFG5", "WFG6", "WFG7", "WFG8", "WFG9"};
			
			exp.generateRBoxplotScripts(rows, columns, problems, prefix, notch = false, exp);
			exp.generateRWilcoxonScripts(problems, prefix, exp);
		} // main
	} // StandardStudy
}