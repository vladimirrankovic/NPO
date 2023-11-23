/*
* Experiment.java
*
* @author Antonio J. Nebro
* @version 1.1
*
* This is the base class to define experiments to be carried out with JARE
*/
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using Algorithm = JARE.Base.Algorithm;
using Problem = JARE.Base.Problem;
using RBoxplot = JARE.experiments.util.RBoxplot;
using RWilcoxon = JARE.experiments.util.RWilcoxon;
using Statistics = JARE.experiments.util.Statistics;
using runExperiment = JARE.experiments.util.runExperiment;
using SMException = JARE.util.SMException;
using ThreadClass = JARE.support.ThreadClass;
using CollectionsSupport = JARE.support.CollectionsSupport;
using System.Collections.Generic;
namespace JARE.experiments
{
	
	/// <summary> </summary>
	/// <author>  antonio
	/// </author>
	public abstract class Experiment
	{
		
		public System.String m_experimentName;
		public System.String[] m_algorithmNameList; // List of the names of the algorithms to be executed
		public System.String[] m_problemList; // List of problems to be solved
		public System.String[] m_paretoFrontFile; // List of the files containing the pareto fronts
		// corresponding to the problems in m_problemList
		public System.String[] m_indicatorList; // List of the quality indicators to be applied
		public System.String m_experimentBaseDirectory; // Directory to store the results
		public System.String m_latexDirectory; // Directory to store the latex files  
		public System.String m_paretoFrontDirectory; // Directory containing the Pareto front files
		public System.String m_outputParetoFrontFile; // Name of the file containing the output
		// Pareto front
		public System.String m_outputParetoSetFile; // Name of the file containing the output
		// Pareto set
		public int m_independentRuns; // Number of independent runs per algorithm
		public Settings[] m_algorithmSettings; // Paremeter settings of each algorithm
		//Algorithm[] algorithm_; // JARE algorithms to be executed
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Dictionary < string, Object > m_map; // Map used to send experiment parameters to threads
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Dictionary < string, Boolean > m_indicatorMinimize;
		// To indicate whether an indicator
		// is to be minimized. Hard-coded
		// in the constructor
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public System.Collections.Specialized.NameValueCollection[] m_problemsSettings;
		
		/// <summary> Constructor
		/// 
		/// Contains default settings
		/// </summary>
		public Experiment()
		{
			m_experimentName = "noName";
			
			m_problemsSettings = null;
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			m_map = new Dictionary < string, Object >();
			
			m_algorithmNameList = null;
			m_problemList = null;
			m_paretoFrontFile = null;
			m_indicatorList = null;
			
			m_experimentBaseDirectory = "";
			m_paretoFrontDirectory = "";
			m_latexDirectory = "latex";
			
			m_outputParetoFrontFile = "FUN";
			m_outputParetoSetFile = "VAR";
			
			m_algorithmSettings = null;
			//algorithm_ = null;
			
			m_independentRuns = 0;
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			m_indicatorMinimize = new Dictionary < string, Boolean >();
			m_indicatorMinimize.Add("HV", false);
			m_indicatorMinimize.Add("EPSILON", true);
			m_indicatorMinimize.Add("SPREAD", true);
			m_indicatorMinimize.Add("GD", true);
			m_indicatorMinimize.Add("IGD", true);
		} // Constructor
		
		/// <summary> Runs the experiment</summary>
		public virtual void  runExperiment(int numberOfThreads)
		{
			// Step 1: check experiment base directory
			checkExperimentDirectory();
			
			m_map.Add("experimentDirectory", m_experimentBaseDirectory);
			m_map.Add("algorithmNameList", m_algorithmNameList);
			m_map.Add("problemList", m_problemList);
			m_map.Add("indicatorList", m_indicatorList);
			m_map.Add("paretoFrontDirectory", m_paretoFrontDirectory);
			m_map.Add("paretoFrontFile", m_paretoFrontFile);
			m_map.Add("independentRuns", m_independentRuns);
			m_map.Add("outputParetoFrontFile", m_outputParetoFrontFile);
			m_map.Add("outputParetoSetFile", m_outputParetoSetFile);
			m_map.Add("problemsSettings", m_problemsSettings);
			
			//SolutionSet[] resultFront = new SolutionSet[m_algorithmNameList.length];
			
			if (m_problemList.Length < numberOfThreads)
			{
				numberOfThreads = m_problemList.Length;
				System.Console.Out.WriteLine("Experiments: list of problems is shorter than the " + "of requested threads. Creating " + numberOfThreads);
			}
			// if
			else
			{
				System.Console.Out.WriteLine("Experiments: creating " + numberOfThreads + " threads");
			}
			
			ThreadClass[] p = new runExperiment[numberOfThreads];
			for (int i = 0; i < numberOfThreads; i++)
			{
				//p[i] = new Experiment(m_map, i, numberOfThreads, m_problemList.length);
				p[i] = new runExperiment(this, m_map, i, numberOfThreads, m_problemList.Length);
				p[i].Start();
			}
			
			try
			{
				for (int i = 0; i < numberOfThreads; i++)
				{
					p[i].Join();
				}
			}
			catch (System.Threading.ThreadInterruptedException ex)
			{
                ////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
			}
		}
		
		/// <summary> Runs the experiment</summary>
		public virtual void  runExperiment()
		{
			runExperiment(1);
		} // runExperiment
		
		public virtual void  checkExperimentDirectory()
		{
			System.IO.FileInfo experimentDirectory;
			
			experimentDirectory = new System.IO.FileInfo(m_experimentBaseDirectory);
			bool tmpBool;
			if (System.IO.File.Exists(experimentDirectory.FullName))
				tmpBool = true;
			else
				tmpBool = System.IO.Directory.Exists(experimentDirectory.FullName);
			if (tmpBool)
			{
				System.Console.Out.WriteLine("Experiment directory exists");
				if (System.IO.Directory.Exists(experimentDirectory.FullName))
				{
					System.Console.Out.WriteLine("Experiment directory is a directory");
				}
				else
				{
					System.Console.Out.WriteLine("Experiment directory is not a directory. Deleting file and creating directory");
				}
				bool tmpBool2;
				if (System.IO.File.Exists(experimentDirectory.FullName))
				{
					System.IO.File.Delete(experimentDirectory.FullName);
					tmpBool2 = true;
				}
				else if (System.IO.Directory.Exists(experimentDirectory.FullName))
				{
					System.IO.Directory.Delete(experimentDirectory.FullName);
					tmpBool2 = true;
				}
				else
					tmpBool2 = false;
				bool generatedAux4 = tmpBool2;
				//UPGRADE_TODO: Method 'java.io.File.mkdirs' was converted to 'System.IO.Directory.CreateDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilemkdirs'"
				System.IO.Directory.CreateDirectory(new System.IO.FileInfo(m_experimentBaseDirectory).FullName);
			}
			// if
			else
			{
				System.Console.Out.WriteLine("Experiment directory does NOT exist. Creating");
				//UPGRADE_TODO: Method 'java.io.File.mkdirs' was converted to 'System.IO.Directory.CreateDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilemkdirs'"
				System.IO.Directory.CreateDirectory(new System.IO.FileInfo(m_experimentBaseDirectory).FullName);
			} // else
		} // checkDirectories
		
		/// <summary> Especifies the settings of each algorith. This method is checked in each
		/// experiment run
		/// </summary>
		/// <param name="problem">Problem to solve
		/// </param>
		/// <param name="problemId">Index of the problem in m_problemList
		/// </param>
		/// <param name="algorithm">Array containing the algorithms to execute
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public abstract void  algorithmSettings(Problem problem, int problemId, Algorithm[] algorithm);
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
		}
		
		
		
		public virtual void  generateLatexTables()
		{
			m_latexDirectory = m_experimentBaseDirectory + "/" + m_latexDirectory;
			System.Console.Out.WriteLine("latex directory: " + m_latexDirectory);
			
			System.Collections.ArrayList[][][] data = new System.Collections.ArrayList[m_indicatorList.Length][][];
			for (int indicator = 0; indicator < m_indicatorList.Length; indicator++)
			{
				// A data vector per problem
				data[indicator] = new System.Collections.ArrayList[m_problemList.Length][];
				
				for (int problem = 0; problem < m_problemList.Length; problem++)
				{
					data[indicator][problem] = new System.Collections.ArrayList[m_algorithmNameList.Length];
					
					for (int algorithm = 0; algorithm < m_algorithmNameList.Length; algorithm++)
					{
						data[indicator][problem][algorithm] = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
						
						System.String directory = m_experimentBaseDirectory;
						directory += "/data/";
						directory += ("/" + m_algorithmNameList[algorithm]);
						directory += ("/" + m_problemList[problem]);
						directory += ("/" + m_indicatorList[indicator]);
						// Read values from data files
						//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
						System.IO.FileStream fis = new System.IO.FileStream(directory, System.IO.FileMode.Open, System.IO.FileAccess.Read);
						System.IO.StreamReader isr = new System.IO.StreamReader(fis, System.Text.Encoding.Default);
						//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						System.IO.StreamReader br = new System.IO.StreamReader(isr.BaseStream, isr.CurrentEncoding);
						System.Console.Out.WriteLine(directory);
						System.String aux = br.ReadLine();
						while (aux != null)
						{
							data[indicator][problem][algorithm].Add(System.Double.Parse(aux));
							System.Console.Out.WriteLine(System.Double.Parse(aux));
							aux = br.ReadLine();
						} // while
					} // for
				} // for
			} // for
			
			double[][][] mean;
			double[][][] median;
			double[][][] stdDeviation;
			double[][][] iqr;
			double[][][] max;
			double[][][] min;
			int[][][] numberOfValues;
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Dictionary < string, Double > statValues = new Dictionary < string, Double >();
			
			statValues.Add("mean", 0.0);
			statValues.Add("median", 0.0);
			statValues.Add("stdDeviation", 0.0);
			statValues.Add("iqr", 0.0);
			statValues.Add("max", 0.0);
			statValues.Add("min", 0.0);
			
			mean = new double[m_indicatorList.Length][][];
			median = new double[m_indicatorList.Length][][];
			stdDeviation = new double[m_indicatorList.Length][][];
			iqr = new double[m_indicatorList.Length][][];
			min = new double[m_indicatorList.Length][][];
			max = new double[m_indicatorList.Length][][];
			numberOfValues = new int[m_indicatorList.Length][][];
			
			for (int indicator = 0; indicator < m_indicatorList.Length; indicator++)
			{
				// A data vector per problem
				mean[indicator] = new double[m_problemList.Length][];
				median[indicator] = new double[m_problemList.Length][];
				stdDeviation[indicator] = new double[m_problemList.Length][];
				iqr[indicator] = new double[m_problemList.Length][];
				min[indicator] = new double[m_problemList.Length][];
				max[indicator] = new double[m_problemList.Length][];
				numberOfValues[indicator] = new int[m_problemList.Length][];
				
				for (int problem = 0; problem < m_problemList.Length; problem++)
				{
					mean[indicator][problem] = new double[m_algorithmNameList.Length];
					median[indicator][problem] = new double[m_algorithmNameList.Length];
					stdDeviation[indicator][problem] = new double[m_algorithmNameList.Length];
					iqr[indicator][problem] = new double[m_algorithmNameList.Length];
					min[indicator][problem] = new double[m_algorithmNameList.Length];
					max[indicator][problem] = new double[m_algorithmNameList.Length];
					numberOfValues[indicator][problem] = new int[m_algorithmNameList.Length];
					
					for (int algorithm = 0; algorithm < m_algorithmNameList.Length; algorithm++)
					{
                        // Visnja: primenjen sort metod ArrayList-e
						//CollectionsSupport.Sort(data[indicator][problem][algorithm], null);
                        data[indicator][problem][algorithm].Sort();
						
						System.String directory = m_experimentBaseDirectory;
						directory += ("/" + m_algorithmNameList[algorithm]);
						directory += ("/" + m_problemList[problem]);
						directory += ("/" + m_indicatorList[indicator]);
						
						//System.out.println("----" + directory + "-----");
						//calculateStatistics(data[indicator][problem][algorithm], meanV, medianV, minV, maxV, stdDeviationV, iqrV) ;
						calculateStatistics(data[indicator][problem][algorithm], statValues);
						/*
						System.out.println("Mean: " + statValues.get("mean"));
						System.out.println("Median : " + statValues.get("median"));
						System.out.println("Std : " + statValues.get("stdDeviation"));
						System.out.println("IQR : " + statValues.get("iqr"));
						System.out.println("Min : " + statValues.get("min"));
						System.out.println("Max : " + statValues.get("max"));
						System.out.println("N_values: " + data[indicator][problem][algorithm].size()) ;
						*/
						mean[indicator][problem][algorithm] = statValues["mean"];
						median[indicator][problem][algorithm] = statValues["median"];
						stdDeviation[indicator][problem][algorithm] = statValues["stdDeviation"];
						iqr[indicator][problem][algorithm] = statValues["iqr"];
						min[indicator][problem][algorithm] = statValues["min"];
						max[indicator][problem][algorithm] = statValues["max"];
						numberOfValues[indicator][problem][algorithm] = data[indicator][problem][algorithm].Count;
					}
				}
			}
			
			System.IO.FileInfo latexOutput;
			latexOutput = new System.IO.FileInfo(m_latexDirectory);
			bool tmpBool;
			if (System.IO.File.Exists(latexOutput.FullName))
				tmpBool = true;
			else
				tmpBool = System.IO.Directory.Exists(latexOutput.FullName);
			if (!tmpBool)
			{
				//UPGRADE_TODO: Method 'java.io.File.mkdirs' was converted to 'System.IO.Directory.CreateDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilemkdirs'"
				/*bool result =*/ System.IO.Directory.CreateDirectory(new System.IO.FileInfo(m_latexDirectory).FullName);
				System.Console.Out.WriteLine("Creating " + m_latexDirectory + " directory");
			}
			System.Console.Out.WriteLine("Experiment name: " + m_experimentName);
			System.String latexFile = m_latexDirectory + "/" + m_experimentName + ".tex";
			printHeaderLatexCommands(latexFile);
			for (int i = 0; i < m_indicatorList.Length; i++)
			{
				printMeanStdDev(latexFile, i, mean, stdDeviation);
				printMedianIQR(latexFile, i, median, iqr);
			} // for
			printEndLatexCommands(latexFile);
		} // generateLatexTables
		
		/// <summary> Calculates statistical values from a vector of Double objects</summary>
		/// <param name="vector">
		/// </param>
		/// <param name="values">
		/// </param>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        void calculateStatistics(System.Collections.ArrayList vector, System.Collections.Generic.Dictionary<string, Double> values)
        {
            if (vector.Count > 0)
            {
                double sum, minimum, maximum, sqsum, min, max, median, mean, iqr, stdDeviation;

                sqsum = 0.0;
                sum = 0.0;
                min = 1e300;
                max = -1e300;
                median = 0;

                for (int i = 0; i < vector.Count; i++)
                {
                    double val = (System.Double)vector[i];

                    sqsum += val * val;
                    sum += val;
                    if (val < min)
                    {
                        min = val;
                    }
                    if (val > max)
                    {
                        max = val;
                    } // if
                } // for

                // Mean
                mean = sum / vector.Count;

                // Standard deviation
                if (sqsum / vector.Count - mean * mean < 0.0)
                {
                    stdDeviation = 0.0;
                }
                else
                {
                    stdDeviation = System.Math.Sqrt(sqsum / vector.Count - mean * mean);
                } // if

                // Median
                if (vector.Count % 2 != 0)
                {
                    median = (System.Double)vector[vector.Count / 2];
                }
                else
                {
                    median = ((System.Double)vector[vector.Count / 2 - 1] + (System.Double)vector[vector.Count / 2]) / 2.0;
                } // if

                values.Add("mean", (System.Double)mean);
                values.Add("median", Statistics.calculateMedian(vector, 0, vector.Count - 1));
                values.Add("iqr", Statistics.calculateIQR(vector));
                values.Add("stdDeviation", (System.Double)stdDeviation);
                values.Add("min", (System.Double)min);
                values.Add("max", (System.Double)max);
            }
            // if
            else
            {
                values.Add("mean", System.Double.NaN);
                values.Add("median", System.Double.NaN);
                values.Add("iqr", System.Double.NaN);
                values.Add("stdDeviation", System.Double.NaN);
                values.Add("min", System.Double.NaN);
                values.Add("max", System.Double.NaN);
            } // else // calculateStatistics
        }
		
		
		internal virtual void  printHeaderLatexCommands(System.String fileName)
		{
			//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
			//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
			System.IO.StreamWriter os = new System.IO.StreamWriter(fileName, false, System.Text.Encoding.Default);
			os.Write("\\documentclass{article}" + "\n");
			os.Write("\\title{" + m_experimentName + "}" + "\n");
			os.Write("\\usepackage{colortbl}" + "\n");
			os.Write("\\usepackage[table*]{xcolor}" + "\n");
			os.Write("\\xdefinecolor{gray95}{gray}{0.65}" + "\n");
			os.Write("\\xdefinecolor{gray25}{gray}{0.8}" + "\n");
			os.Write("\\author{}" + "\n");
			os.Write("\\begin{document}" + "\n");
			os.Write("\\maketitle" + "\n");
			os.Write("\\section{Tables}" + "\n");
			
			os.Close();
		}
		
		internal virtual void  printEndLatexCommands(System.String fileName)
		{
			//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
			//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
			System.IO.StreamWriter os = new System.IO.StreamWriter(fileName, true, System.Text.Encoding.Default);
			os.Write("\\end{document}" + "\n");
			os.Close();
		} // printEndLatexCommands
		
		internal virtual void  printMeanStdDev(System.String fileName, int indicator, double[][][] mean, double[][][] stdDev)
		{
			//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
			//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
			System.IO.StreamWriter os = new System.IO.StreamWriter(fileName, true, System.Text.Encoding.Default);
			os.Write("\\" + "\n");
			os.Write("\\begin{table}" + "\n");
			os.Write("\\caption{" + m_indicatorList[indicator] + ". Mean and standard deviation}" + "\n");
			os.Write("\\label{table:mean." + m_indicatorList[indicator] + "}" + "\n");
			os.Write("\\centering" + "\n");
			os.Write("\\begin{scriptsize}" + "\n");
			os.Write("\\begin{tabular}{l");
			
			// calculate the number of columns
			for (int i = 0; i < m_algorithmNameList.Length; i++)
			{
				os.Write("l");
			}
			os.Write("}\n");
			
			os.Write("\\hline");
			// write table head
			for (int i = - 1; i < m_algorithmNameList.Length; i++)
			{
				if (i == - 1)
				{
					os.Write(" & ");
				}
				else if (i == (m_algorithmNameList.Length - 1))
				{
					os.Write(" " + m_algorithmNameList[i] + "\\\\" + "\n");
				}
				else
				{
					os.Write("" + m_algorithmNameList[i] + " & ");
				}
			}
			os.Write("\\hline" + "\n");
			
			System.String m, s;
			// write lines
			for (int i = 0; i < m_problemList.Length; i++)
			{
				// find the best value and second best value
				double bestValue;
				double bestValueIQR;
				double secondBestValue;
				double secondBestValueIQR;
				int bestIndex = - 1;
				int secondBestIndex = - 1;
				if (/*(System.Object)*/ m_indicatorMinimize[m_indicatorList[indicator]] == true)
				{
					// minimize by default
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValue = System.Double.MaxValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValueIQR = System.Double.MaxValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValue = System.Double.MaxValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValueIQR = System.Double.MaxValue;
					for (int j = 0; j < (m_algorithmNameList.Length); j++)
					{
						if ((mean[indicator][i][j] < bestValue) || ((mean[indicator][i][j] == bestValue) && (stdDev[indicator][i][j] < bestValueIQR)))
						{
							secondBestIndex = bestIndex;
							secondBestValue = bestValue;
							secondBestValueIQR = bestValueIQR;
							bestValue = mean[indicator][i][j];
							bestValueIQR = stdDev[indicator][i][j];
							bestIndex = j;
						}
						else if ((mean[indicator][i][j] < secondBestValue) || ((mean[indicator][i][j] == secondBestValue) && (stdDev[indicator][i][j] < secondBestValueIQR)))
						{
							secondBestIndex = j;
							secondBestValue = mean[indicator][i][j];
							secondBestValueIQR = stdDev[indicator][i][j];
						} // else if
					}
				}
				// if
				else
				{
					// indicator to maximize e.g., the HV
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValue = System.Double.MinValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValueIQR = System.Double.MinValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValue = System.Double.MinValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValueIQR = System.Double.MinValue;
					for (int j = 0; j < (m_algorithmNameList.Length); j++)
					{
						if ((mean[indicator][i][j] > bestValue) || ((mean[indicator][i][j] == bestValue) && (stdDev[indicator][i][j] < bestValueIQR)))
						{
							secondBestIndex = bestIndex;
							secondBestValue = bestValue;
							secondBestValueIQR = bestValueIQR;
							bestValue = mean[indicator][i][j];
							bestValueIQR = stdDev[indicator][i][j];
							bestIndex = j;
						}
						else if ((mean[indicator][i][j] > secondBestValue) || ((mean[indicator][i][j] == secondBestValue) && (stdDev[indicator][i][j] < secondBestValueIQR)))
						{
							secondBestIndex = j;
							secondBestValue = mean[indicator][i][j];
							secondBestValueIQR = stdDev[indicator][i][j];
						} // else if
					} // for
				} // else
				
				os.Write(m_problemList[i] + " & ");
				for (int j = 0; j < (m_algorithmNameList.Length - 1); j++)
				{
					if (j == bestIndex)
					{
						os.Write("\\cellcolor{gray95}");
					}
					if (j == secondBestIndex)
					{
						os.Write("\\cellcolor{gray25}");
					}
					
					m = String.Format(new System.Globalization.CultureInfo("en"), "%10.2e", mean[indicator][i][j]);
					s = String.Format(new System.Globalization.CultureInfo("en"), "%8.1e", stdDev[indicator][i][j]);
					os.Write("$" + m + "_{" + s + "}$ & ");
				}
				if (bestIndex == (m_algorithmNameList.Length - 1))
				{
					os.Write("\\cellcolor{gray95}");
				}
				m = String.Format(new System.Globalization.CultureInfo("en"), "%10.2e", mean[indicator][i][m_algorithmNameList.Length - 1]);
				s = String.Format(new System.Globalization.CultureInfo("en"), "%8.1e", stdDev[indicator][i][m_algorithmNameList.Length - 1]);
				os.Write("$" + m + "_{" + s + "}$ \\\\" + "\n");
			} // for
			//os.write("" + mean[0][m_problemList.length-1][m_algorithmNameList.length-1] + "\\\\"+ "\n" ) ;
			
			os.Write("\\hline" + "\n");
			os.Write("\\end{tabular}" + "\n");
			os.Write("\\end{scriptsize}" + "\n");
			os.Write("\\end{table}" + "\n");
			os.Close();
		} // printMeanStdDev
		
		internal virtual void  printMedianIQR(System.String fileName, int indicator, double[][][] median, double[][][] IQR)
		{
			//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
			//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
			System.IO.StreamWriter os = new System.IO.StreamWriter(fileName, true, System.Text.Encoding.Default);
			os.Write("\\" + "\n");
			os.Write("\\begin{table}" + "\n");
			os.Write("\\caption{" + m_indicatorList[indicator] + ". Median and IQR}" + "\n");
			os.Write("\\label{table:median." + m_indicatorList[indicator] + "}" + "\n");
			os.Write("\\begin{scriptsize}" + "\n");
			os.Write("\\centering" + "\n");
			os.Write("\\begin{tabular}{l");
			
			// calculate the number of columns
			for (int i = 0; i < m_algorithmNameList.Length; i++)
			{
				os.Write("l");
			}
			os.Write("}\n");
			
			os.Write("\\hline");
			// write table head
			for (int i = - 1; i < m_algorithmNameList.Length; i++)
			{
				if (i == - 1)
				{
					os.Write(" & ");
				}
				else if (i == (m_algorithmNameList.Length - 1))
				{
					os.Write(" " + m_algorithmNameList[i] + "\\\\" + "\n");
				}
				else
				{
					os.Write("" + m_algorithmNameList[i] + " & ");
				}
			}
			os.Write("\\hline" + "\n");
			
			System.String m, s;
			// write lines
			for (int i = 0; i < m_problemList.Length; i++)
			{
				// find the best value and second best value
				double bestValue;
				double bestValueIQR;
				double secondBestValue;
				double secondBestValueIQR;
				int bestIndex = - 1;
				int secondBestIndex = - 1;
				if (/*(System.Object)*/ m_indicatorMinimize[m_indicatorList[indicator]] == true)
				{
					// minimize by default
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValue = System.Double.MaxValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValueIQR = System.Double.MaxValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValue = System.Double.MaxValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValueIQR = System.Double.MaxValue;
					for (int j = 0; j < (m_algorithmNameList.Length); j++)
					{
						if ((median[indicator][i][j] < bestValue) || ((median[indicator][i][j] == bestValue) && (IQR[indicator][i][j] < bestValueIQR)))
						{
							secondBestIndex = bestIndex;
							secondBestValue = bestValue;
							secondBestValueIQR = bestValueIQR;
							bestValue = median[indicator][i][j];
							bestValueIQR = IQR[indicator][i][j];
							bestIndex = j;
						}
						else if ((median[indicator][i][j] < secondBestValue) || ((median[indicator][i][j] == secondBestValue) && (IQR[indicator][i][j] < secondBestValueIQR)))
						{
							secondBestIndex = j;
							secondBestValue = median[indicator][i][j];
							secondBestValueIQR = IQR[indicator][i][j];
						} // else if
					} // for
				}
				// if
				else
				{
					// indicator to maximize e.g., the HV
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValue = System.Double.MinValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					bestValueIQR = System.Double.MinValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValue = System.Double.MinValue;
					//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					secondBestValueIQR = System.Double.MinValue;
					for (int j = 0; j < (m_algorithmNameList.Length); j++)
					{
						if ((median[indicator][i][j] > bestValue) || ((median[indicator][i][j] == bestValue) && (IQR[indicator][i][j] < bestValueIQR)))
						{
							secondBestIndex = bestIndex;
							secondBestValue = bestValue;
							secondBestValueIQR = bestValueIQR;
							bestValue = median[indicator][i][j];
							bestValueIQR = IQR[indicator][i][j];
							bestIndex = j;
						}
						else if ((median[indicator][i][j] > secondBestValue) || ((median[indicator][i][j] == secondBestValue) && (IQR[indicator][i][j] < secondBestValueIQR)))
						{
							secondBestIndex = j;
							secondBestValue = median[indicator][i][j];
							secondBestValueIQR = IQR[indicator][i][j];
						} // else if
					} // for
				} // else
				
				
				os.Write(m_problemList[i] + " & ");
				for (int j = 0; j < (m_algorithmNameList.Length - 1); j++)
				{
					if (j == bestIndex)
					{
						os.Write("\\cellcolor{gray95}");
					}
					if (j == secondBestIndex)
					{
						os.Write("\\cellcolor{gray25}");
					}
					m = String.Format(new System.Globalization.CultureInfo("en"), "%10.2e", median[indicator][i][j]);
					s = String.Format(new System.Globalization.CultureInfo("en"), "%8.1e", IQR[indicator][i][j]);
					os.Write("$" + m + "_{" + s + "}$ & ");
				}
				if (bestIndex == (m_algorithmNameList.Length - 1))
				{
					os.Write("\\cellcolor{gray95}");
				}
				m = String.Format(new System.Globalization.CultureInfo("en"), "%10.2e", median[indicator][i][m_algorithmNameList.Length - 1]);
				s = String.Format(new System.Globalization.CultureInfo("en"), "%8.1e", IQR[indicator][i][m_algorithmNameList.Length - 1]);
				os.Write("$" + m + "_{" + s + "}$ \\\\" + "\n");
			} // for
			//os.write("" + mean[0][m_problemList.length-1][m_algorithmNameList.length-1] + "\\\\"+ "\n" ) ;
			
			os.Write("\\hline" + "\n");
			os.Write("\\end{tabular}" + "\n");
			os.Write("\\end{scriptsize}" + "\n");
			os.Write("\\end{table}" + "\n");
			os.Close();
		} // printMedianIQR
		
		/// <summary> Invoking the generateScripts method on the RBoxplot class</summary>
		/// <param name="rows">
		/// </param>
		/// <param name="cols">
		/// </param>
		/// <param name="problems">
		/// </param>
		/// <param name="prefix">
		/// </param>
		/// <param name="notch">
		/// </param>
		/// <param name="experiment">
		/// </param>
		/// <throws>  IOException  </throws>
		/// <throws>  FileNotFoundException  </throws>
		internal virtual void  generateRBoxplotScripts(int rows, int cols, System.String[] problems, System.String prefix, bool notch, Experiment experiment)
		{
			RBoxplot.generateScripts(rows, cols, problems, prefix, notch, this);
		} // generateRBoxplotScripts
		
		/// <summary> Invoking the generateScripts method on the RWilcoxon class</summary>
		/// <param name="problems">
		/// </param>
		/// <param name="prefix">
		/// </param>
		/// <param name="experiment">
		/// </param>
		/// <throws>  FileNotFoundException </throws>
		/// <throws>  IOException </throws>
		internal virtual void  generateRWilcoxonScripts(System.String[] problems, System.String prefix, Experiment experiment)
		{
			RWilcoxon.generateScripts(problems, prefix, this);
		} // generateRWilcoxonScripts
	} // Experiment
}