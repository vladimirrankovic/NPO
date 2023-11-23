/// <summary> NSGAIIStudy.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Level = java.util.logging.Level;
using Algorithm = SharpMetal.Base.Algorithm;
using Problem = SharpMetal.Base.Problem;
using NSGAII_Settings = SharpMetal.experiments.settings.NSGAII_Settings;
using RBoxplot = SharpMetal.experiments.util.RBoxplot;
using RWilcoxon = SharpMetal.experiments.util.RWilcoxon;
using SMException = SharpMetal.util.SMException;
namespace SharpMetal.experiments
{
	
	/// <author>  Antonio J. Nebro
	/// </author>
	public class NSGAIIStudy:Experiment
	{
		
		/// <summary> Configures the algorithms in each independent run</summary>
		/// <param name="problem">The problem to solve
		/// </param>
		/// <param name="problemIndex">
		/// </param>
		/// <param name="algorithm">Array containing the algorithms to run
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'algorithmSettings'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public override void  algorithmSettings(Problem problem, int problemIndex, Algorithm[] algorithm)
		{
			lock (this)
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
					
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					parameters[0]["m_crossoverProbability"] = "1.0";
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					parameters[1]["m_crossoverProbability"] = "0.9";
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					parameters[2]["m_crossoverProbability"] = "0.8";
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					parameters[3]["m_crossoverProbability"] = "0.7";
					
					if ((!m_paretoFrontFile[problemIndex].Equals("")) || (m_paretoFrontFile[problemIndex] == null))
					{
						for (int i = 0; i < numberOfAlgorithms; i++)
						{
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							parameters[i]["m_paretoFrontFile"] = m_paretoFrontFile[problemIndex];
						}
					} // if
					
					for (int i = 0; i < numberOfAlgorithms; i++)
						algorithm[i] = new NSGAII_Settings(problem).configure(parameters[i]);
				}
				catch (System.ArgumentException ex)
				{
                    ////////Logger.getLogger(typeof(NSGAIIStudy).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
                    ////////Logger.getLogger(typeof(NSGAIIStudy).FullName).log(Level.SEVERE, null, ex);
				}
				catch (SMException ex)
				{
                    ////////Logger.getLogger(typeof(NSGAIIStudy).FullName).log(Level.SEVERE, null, ex);
				}
			}
		} // algorithmSettings
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			NSGAIIStudy exp = new NSGAIIStudy(); // exp = experiment
			
			exp.m_experimentName = "NSGAIIStudy";
			exp.m_algorithmNameList = new System.String[]{"NSGAIIa", "NSGAIIb", "NSGAIIc", "NSGAIId"};
			exp.m_problemList = new System.String[]{"ZDT1", "ZDT2", "ZDT3", "ZDT4", "DTLZ1", "WFG2"};
			exp.m_paretoFrontFile = new System.String[]{"ZDT1.pf", "ZDT2.pf", "ZDT3.pf", "ZDT4.pf", "DTLZ1.2D.pf", "WFG2.2D.pf"};
			exp.m_indicatorList = new System.String[]{"HV", "SPREAD", "IGD", "EPSILON"};
			
			int numberOfAlgorithms = exp.m_algorithmNameList.Length;
			
			exp.m_experimentBaseDirectory = "/Users/antonio/Softw/pruebas/SharpMetal/" + exp.m_experimentName;
			exp.m_paretoFrontDirectory = "/Users/antonio/Softw/pruebas/data/paretoFronts";
			
			exp.m_algorithmSettings = new Settings[numberOfAlgorithms];
			
			exp.m_independentRuns = 30;
			
			// Run the experiments
			int numberOfThreads;
			exp.runExperiment(numberOfThreads = 2);
			
			// Generate latex tables (comment this sentence is not desired)
			exp.generateLatexTables();
			
			// Configure the R scripts to be generated
			int rows;
			int columns;
			System.String prefix;
			System.String[] problems;
			
			rows = 2;
			columns = 3;
			prefix = new System.Text.StringBuilder("Problems").ToString();
			problems = new System.String[]{"ZDT1", "ZDT2", "ZDT3", "ZDT4", "DTLZ1", "WFG2"};
			
			bool notch;
			exp.generateRBoxplotScripts(rows, columns, problems, prefix, notch = true, exp);
			exp.generateRWilcoxonScripts(problems, prefix, exp);
		} // main
	} // NSGAIIStudy
}