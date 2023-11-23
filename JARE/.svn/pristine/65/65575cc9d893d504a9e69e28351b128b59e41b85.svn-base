/// <summary> ConstrainedProblemStudy.java
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
using AbYSS_Settings = SharpMetal.experiments.settings.AbYSS_Settings;
using MOCell_Settings = SharpMetal.experiments.settings.MOCell_Settings;
using NSGAII_Settings = SharpMetal.experiments.settings.NSGAII_Settings;
using SPEA2_Settings = SharpMetal.experiments.settings.SPEA2_Settings;
using RBoxplot = SharpMetal.experiments.util.RBoxplot;
using RWilcoxon = SharpMetal.experiments.util.RWilcoxon;
using SMException = SharpMetal.util.SMException;
namespace SharpMetal.experiments
{
	
	/// <author>  Antonio J. Nebro
	/// </author>
	public class ConstrainedProblemsStudy:Experiment
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
				}
				
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
				algorithm[3] = new AbYSS_Settings(problem).configure(parameters[3]);
			}
			catch (System.ArgumentException ex)
			{
                ////////Logger.getLogger(typeof(ConstrainedProblemsStudy).FullName).log(Level.SEVERE, null, ex);
			}
			catch (System.UnauthorizedAccessException ex)
			{
                ////////Logger.getLogger(typeof(ConstrainedProblemsStudy).FullName).log(Level.SEVERE, null, ex);
			}
			catch (SMException ex)
			{
                ////////Logger.getLogger(typeof(ConstrainedProblemsStudy).FullName).log(Level.SEVERE, null, ex);
			}
		}
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			ConstrainedProblemsStudy exp = new ConstrainedProblemsStudy();
			
			exp.m_experimentName = "ConstrainedProblemsStudy";
			exp.m_algorithmNameList = new System.String[]{"NSGAII", "SPEA2", "MOCell", "AbYSS"};
			exp.m_problemList = new System.String[]{"Golinski", "Srinivas", "Tanaka", "Osyczka2"};
			exp.m_paretoFrontFile = new System.String[]{"Golinski.pf", "Srinivas.pf", "Tanaka.pf", "Osyczka2.pf"};
			
			exp.m_indicatorList = new System.String[]{"EPSILON", "SPREAD", "HV"};
			
			int numberOfAlgorithms = exp.m_algorithmNameList.Length;
			
			exp.m_experimentBaseDirectory = "/Users/antonio/Softw/pruebas/" + exp.m_experimentName;
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
			rows = 2;
			columns = 2;
			prefix = new System.Text.StringBuilder("Constrained").ToString();
			problems = new System.String[]{"Golinski", "Srinivas", "Tanaka", "Osyczka2"};
			exp.generateRBoxplotScripts(rows, columns, problems, prefix, notch = false, exp);
			exp.generateRWilcoxonScripts(problems, prefix, exp);
		}
	} // ConstrainedProblemsStudy
}