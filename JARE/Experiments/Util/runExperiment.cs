/*
* To change this template, choose Tools | Templates
* and open the template in the editor.
*/
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using Algorithm = JARE.Base.Algorithm;
using Problem = JARE.Base.Problem;
using SolutionSet = JARE.Base.SolutionSet;
using Experiment = JARE.experiments.Experiment;
using Settings = JARE.experiments.Settings;
using ProblemFactory = JARE.problems.ProblemFactory;
using SMException = JARE.util.SMException;
using JARE.qualityIndicator;
using JARE.support;
namespace JARE.experiments.util
{
	/// <summary> </summary>
	/// <author>  antonio
	/// </author>
	public class runExperiment:ThreadClass
	{
		public Experiment m_experiment;
		public int m_id;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public System.Collections.Generic.Dictionary < string, Object > m_map;
		public int m_numberOfThreads;
		public int m_numberOfProblems;
		
		internal int m_first;
		internal int m_last;
		
		internal System.String m_experimentName;
		internal System.String[] m_algorithmNameList; // List of the names of the algorithms to be executed
		internal System.String[] m_problemList; // List of problems to be solved
		internal System.String[] m_paretoFrontFile; // List of the files containing the pareto fronts
		// corresponding to the problems in m_problemList
		internal System.String[] m_indicatorList; // List of the quality indicators to be applied
		internal System.String m_experimentBaseDirectory; // Directory to store the results
		internal System.String m_latexDirectory; // Directory to store the latex files
		internal System.String m_rDirectory; // Directory to store the generated R scripts
		internal System.String m_paretoFrontDirectory; // Directory containing the Pareto front files
		internal System.String m_outputParetoFrontFile; // Name of the file containing the output
		// Pareto front
		internal System.String m_outputParetoSetFile; // Name of the file containing the output
		// Pareto set
		internal int m_independentRuns; // Number of independent runs per algorithm
		internal Settings[] m_algorithmSettings; // Paremeter settings of each algorithm
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        public runExperiment(Experiment experiment, System.Collections.Generic.Dictionary<string, Object> map, int id, int numberOfThreads, int numberOfProblems)
        {
            m_experiment = experiment;
            m_id = id;
            m_map = map;
            m_numberOfThreads = numberOfThreads;
            m_numberOfProblems = numberOfProblems;

            int partitions = numberOfProblems / numberOfThreads;

            m_first = partitions * id;
            if (id == (numberOfThreads - 1))
            {
                m_last = numberOfProblems - 1;
            }
            else
            {
                m_last = m_first + partitions - 1;
            }

            System.Console.Out.WriteLine("Id: " + id + "  Partitions: " + partitions + " First: " + m_first + " Last: " + m_last);
        }
		
		override public void  Run()
		{
			Algorithm[] algorithm; // JARE algorithms to be executed

            //System.String experimentName = (System.String)m_map.get("name");
            System.String experimentName;
            experimentName = (System.String) m_map["name"];
            m_experimentBaseDirectory = ((System.String)m_map["experimentDirectory"]);
			m_algorithmNameList = (System.String[]) m_map["algorithmNameList"];
			m_problemList = (System.String[]) m_map["problemList"];
			m_indicatorList = (System.String[]) m_map["indicatorList"];
			m_paretoFrontDirectory = ((System.String) m_map["paretoFrontDirectory"]);
			m_paretoFrontFile = (System.String[]) m_map["paretoFrontFile"];
			m_independentRuns = (System.Int32) m_map["independentRuns"];
			m_outputParetoFrontFile = ((System.String) m_map["outputParetoFrontFile"]);
			m_outputParetoSetFile = ((System.String) m_map["outputParetoSetFile"]);
			
			int numberOfAlgorithms = m_algorithmNameList.Length;
			System.Console.Out.WriteLine("Experiment: Number of algorithms: " + numberOfAlgorithms);
			System.Console.Out.WriteLine("Experiment: runs: " + m_independentRuns);
			algorithm = new Algorithm[numberOfAlgorithms];
			
			System.Console.Out.WriteLine("Nombre: " + experimentName);
			System.Console.Out.WriteLine("experimentDirectory: " + m_experimentBaseDirectory);
			System.Console.Out.WriteLine("m_numberOfThreads: " + m_numberOfThreads);
			System.Console.Out.WriteLine("m_numberOfProblems: " + m_numberOfProblems);
			System.Console.Out.WriteLine("first: " + m_first);
			System.Console.Out.WriteLine("last: " + m_last);
			
			SolutionSet resultFront = null;
			
			
			for (int problemId = m_first; problemId <= m_last; problemId++)
			{
				Problem problem; // The problem to solve
				
				problem = null;
				// STEP 2: get the problem from the list
				System.Object[] parameters = new System.Object[]{"Real"}; // Parameters of the problem
				try
				{
					// Parameters of the problem
					problem = (new ProblemFactory()).getProblem(m_problemList[problemId], parameters);
				}
				catch (SMException ex)
				{
                    //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
				}
				
				// STEP 3: check the file containing the Pareto front of the problem
				lock (m_experiment)
				{
					if (m_indicatorList.Length > 0)
					{
						System.IO.FileInfo pfFile = new System.IO.FileInfo(m_paretoFrontDirectory + "/" + m_paretoFrontFile[problemId]);
						
						bool tmpBool;
						if (System.IO.File.Exists(pfFile.FullName))
							tmpBool = true;
						else
							tmpBool = System.IO.Directory.Exists(pfFile.FullName);
						if (tmpBool)
						{
							m_paretoFrontFile[problemId] = m_paretoFrontDirectory + "/" + m_paretoFrontFile[problemId];
						}
						else
						{
							m_paretoFrontFile[problemId] = "";
						}
					} // if
				}
				try
				{
					m_experiment.algorithmSettings(problem, problemId, algorithm);
				}
				//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception e1)
				{
					// TODO Auto-generated catch block
					//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                    //e1.StackTrace;
                    SupportClass.WriteStackTrace(e1, Console.Error);
				}
				for (int runs = 0; runs < m_independentRuns; runs++)
				{
					System.Console.Out.WriteLine("Iruns: " + runs);
					// STEP 4: configure the algorithms
					
					// STEP 5: run the algorithms
					for (int i = 0; i < numberOfAlgorithms; i++)
					{
						//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
						System.Console.Out.WriteLine(algorithm[i].GetType());
						// STEP 6: create output directories
						System.IO.FileInfo experimentDirectory;
						System.String directory;
						
						directory = m_experimentBaseDirectory + "/data/" + m_algorithmNameList[i] + "/" + m_problemList[problemId];
						
						experimentDirectory = new System.IO.FileInfo(directory);
						bool tmpBool2;
						if (System.IO.File.Exists(experimentDirectory.FullName))
							tmpBool2 = true;
						else
							tmpBool2 = System.IO.Directory.Exists(experimentDirectory.FullName);
						if (!tmpBool2)
						{
							//UPGRADE_TODO: Method 'java.io.File.mkdirs' was converted to 'System.IO.Directory.CreateDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilemkdirs'"
							/*bool result = */System.IO.Directory.CreateDirectory(new System.IO.FileInfo(directory).FullName);
							System.Console.Out.WriteLine("Creating " + directory);
						}
						
						// STEP 7: run the algorithm
						System.Console.Out.WriteLine("Running algorithm: " + m_algorithmNameList[i] + ", problem: " + m_problemList[problemId] + ", run: " + runs);
						try
						{
							try
							{
								resultFront = algorithm[i].execute();
							}
							//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
							catch (System.Exception e)
							{
								// TODO Auto-generated catch block
								//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                                //e.StackTrace;
                                SupportClass.WriteStackTrace(e, Console.Error);
							}
						}
						catch (SMException ex)
						{
                            //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
						}
						
						// STEP 8: put the results in the output directory
						resultFront.printObjectivesToFile(directory + "/" + m_outputParetoFrontFile + "." + runs);
						resultFront.printVariablesToFile(directory + "/" + m_outputParetoSetFile + "." + runs);
						
						// STEP 9: calculate quality indicators
						if (m_indicatorList.Length > 0)
						{
							QualityIndicator indicators;
							//System.out.println("PF file: " + m_paretoFrontFile[problemId]);
							indicators = new QualityIndicator(problem, m_paretoFrontFile[problemId]);
							
							for (int j = 0; j < m_indicatorList.Length; j++)
							{
								if (m_indicatorList[j].Equals("HV"))
								{
									double value = indicators.getHypervolume(resultFront);
									//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
									System.IO.StreamWriter os;
									try
									{
										//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
										//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
										os = new System.IO.StreamWriter(experimentDirectory + "/HV", true, System.Text.Encoding.Default);
										os.Write("" + value + "\n");
										os.Close();
									}
									catch (System.IO.IOException ex)
									{
                                        //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
									}
								}
								if (m_indicatorList[j].Equals("SPREAD"))
								{
									//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
									System.IO.StreamWriter os = null;
									try
									{
										double value = indicators.getSpread(resultFront);
										//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
										//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
										os = new System.IO.StreamWriter(experimentDirectory + "/SPREAD", true, System.Text.Encoding.Default);
										os.Write("" + value + "\n");
										os.Close();
									}
									catch (System.IO.IOException ex)
									{
                                        //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
									}
									finally
									{
										try
										{
											os.Close();
										}
										catch (System.IO.IOException ex)
										{
                                            //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
										}
									}
								}
								if (m_indicatorList[j].Equals("IGD"))
								{
									//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
									System.IO.StreamWriter os = null;
									try
									{
										double value = indicators.getIGD(resultFront);
										//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
										//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
										os = new System.IO.StreamWriter(experimentDirectory + "/IGD", true, System.Text.Encoding.Default);
										os.Write("" + value + "\n");
										os.Close();
									}
									catch (System.IO.IOException ex)
									{
                                        //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
									}
									finally
									{
										try
										{
											os.Close();
										}
										catch (System.IO.IOException ex)
										{
                                            ////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
										}
									}
								}
								if (m_indicatorList[j].Equals("EPSILON"))
								{
									//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
									System.IO.StreamWriter os = null;
									try
									{
										double value = indicators.getEpsilon(resultFront);
										//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
										//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
										os = new System.IO.StreamWriter(experimentDirectory + "/EPSILON", true, System.Text.Encoding.Default);
										os.Write("" + value + "\n");
										os.Close();
									}
									catch (System.IO.IOException ex)
									{
                                        ////////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
									}
									finally
									{
										try
										{
											os.Close();
										}
										catch (System.IO.IOException ex)
										{
                                            //////////Logger.getLogger(typeof(Experiment).FullName).log(Level.SEVERE, null, ex);
										}
									}
								}
							} // for
						} // if
					} // for
				} // for
			} //for
		}
	}
}