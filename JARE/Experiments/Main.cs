/// <summary> Main.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using SharpMetal.Base;
using SharpMetal.problems;
using Configuration = SharpMetal.util.Configuration;
using SMException = SharpMetal.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
using Logger = SharpMetal.util.Logger;
using QualityIndicator = SharpMetal.qualityIndicator.QualityIndicator;
namespace SharpMetal.experiments
{
	
	public class Main
	{
		public static Logger m_logger; // Logger object
        //////public static FileHandler fileHandler_; // FileHandler object
		
		/// <param name="args">Command line arguments.
		/// </param>
		/// <throws>  SMException </throws>
		/// <throws>  IOException </throws>
		/// <throws>  SecurityException </throws>
		/// <summary> Usage: three options
		/// - SharpMetal.experiments.Main algorithmName
		/// - SharpMetal.experiments.Main algorithmName problemName
		/// - SharpMetal.experiments.Main algorithmName problemName paretoFrontFile
		/// </summary>
		/// <throws>  ClassNotFoundException  </throws>
		[STAThread]
		public static void main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			
			QualityIndicator indicators; // Object to get quality indicators
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection properties;
			Settings settings = null;
			
			System.String algorithmName = "";
			System.String problemName = "Kursawe"; // Default problem
			System.String paretoFrontFile = "";
			
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			properties = new System.Collections.Specialized.NameValueCollection();
			indicators = null;
			problem = null;
			
			if (args.Length == 0)
			{
				//
				System.Console.Error.WriteLine("Sintax error. Usage:");
				System.Console.Error.WriteLine("a) SharpMetal.experiments.Main algorithmName ");
				System.Console.Error.WriteLine("b) SharpMetal.experiments.Main algorithmName problemName");
				System.Console.Error.WriteLine("c) SharpMetal.experiments.Main algorithmName problemName paretoFrontFile");
				System.Environment.Exit(- 1);
			}
			// if
			else if (args.Length == 1)
			{
				// algorithmName
				algorithmName = args[0];
				System.Object[] problemParams = new System.Object[]{"Real"};
				problem = (new ProblemFactory()).getProblem(problemName, problemParams);
				System.Object[] settingsParams = new System.Object[]{problem};
				settings = (new SettingsFactory()).getSettingsObject(algorithmName, settingsParams);
			}
			// if
			else if (args.Length == 2)
			{
				// algorithmName problemName
				algorithmName = args[0];
				problemName = args[1];
				System.Object[] problemParams = new System.Object[]{"Real"};
				problem = (new ProblemFactory()).getProblem(problemName, problemParams);
				System.Object[] settingsParams = new System.Object[]{problem};
				settings = (new SettingsFactory()).getSettingsObject(algorithmName, settingsParams);
			}
			// if
			else if (args.Length == 3)
			{
				// algorithmName problemName paretoFrontFile
				algorithmName = args[0];
				problemName = args[1];
				paretoFrontFile = args[2];
				System.Object[] problemParams = new System.Object[]{"Real"};
				problem = (new ProblemFactory()).getProblem(problemName, problemParams);
				System.Object[] settingsParams = new System.Object[]{problem};
				settings = (new SettingsFactory()).getSettingsObject(algorithmName, settingsParams);
				
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				properties["m_paretoFrontFile"] = paretoFrontFile;
				indicators = new QualityIndicator(problem, paretoFrontFile);
			} // if
			
			// Logger object and file to store log messages
			m_logger = Configuration.m_logger;
            //////fileHandler_ = new FileHandler(algorithmName + ".log");
            //////logger_.addHandler(fileHandler_);
			
			algorithm = settings.configure(properties);
			
			// Execute the Algorithm
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			
			// Result messages
            ////////logger_.info("Total execution time: " + estimatedTime + "ms");
            ////////logger_.info("Objectives values have been writen to file FUN");
            ////////population.printObjectivesToFile("FUN");
            ////////logger_.info("Variables values have been writen to file VAR");
            ////////population.printVariablesToFile("VAR");

            m_logger.WriteLog("Total execution time: " + estimatedTime + "ms");
            m_logger.WriteLog("Objectives values have been writen to file FUN");
            population.printObjectivesToFile("FUN");
            m_logger.WriteLog("Variables values have been writen to file VAR");
            population.printVariablesToFile("VAR");

            if (indicators != null)
			{
                //////logger_.info("Quality indicators");
                //////logger_.info("Hypervolume: " + indicators.getHypervolume(population));
                //////logger_.info("GD         : " + indicators.getGD(population));
                //////logger_.info("IGD        : " + indicators.getIGD(population));
                //////logger_.info("Spread     : " + indicators.getSpread(population));
                //////logger_.info("Epsilon    : " + indicators.getEpsilon(population));

                m_logger.WriteLog("Quality indicators");
                m_logger.WriteLog("Hypervolume: " + indicators.getHypervolume(population));
                m_logger.WriteLog("GD         : " + indicators.getGD(population));
                m_logger.WriteLog("IGD        : " + indicators.getIGD(population));
                m_logger.WriteLog("Spread     : " + indicators.getSpread(population));
                m_logger.WriteLog("Epsilon    : " + indicators.getEpsilon(population));

                if (algorithm.getOutputParameter("evaluations") != null)
				{
					System.Int32 evals = (System.Int32) algorithm.getOutputParameter("evaluations");
					int evaluations = (System.Int32) evals;
                    ////////logger_.info("Speed      : " + evaluations + " evaluations");
                    m_logger.WriteLog("Speed      : " + evaluations + " evaluations");
                } // if
			} // if
		} //main
	} // main
}