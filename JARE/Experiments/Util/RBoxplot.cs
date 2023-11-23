/// <summary> RBoxplot.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// 
/// </author>
/// <version>  1.1
/// </version>
using System;
using Experiment = JARE.experiments.Experiment;
namespace JARE.experiments.util
{
	
	public class RBoxplot
	{
		/// <summary> This script produces R scripts for generating eps files containing boxplots
		/// of the results previosly obtained. The boxplots will be arranged in a grid
		/// of rows x cols. As the number of problems in the experiment can be too high,
		/// </summary>
		/// <param name="problems">includes a list of the problems to be plotted.
		/// </param>
		/// <param name="rows">
		/// </param>
		/// <param name="cols">
		/// </param>
		/// <param name="problems">List of problem to plot
		/// </param>
		/// <param name="prefix">Prefix to be added to the names of the R scripts
		/// </param>
		/// <throws>  java.io.FileNotFoundException </throws>
		/// <throws>  java.io.IOException </throws>
		public static void  generateScripts(int rows, int cols, System.String[] problems, System.String prefix, bool notch, Experiment experiment)
		{
			// STEP 1. Creating R output directory
			
			System.String rDirectory = "R";
			rDirectory = experiment.m_experimentBaseDirectory + "/" + rDirectory;
			System.Console.Out.WriteLine("R    : " + rDirectory);
			System.IO.FileInfo rOutput;
			rOutput = new System.IO.FileInfo(rDirectory);
			bool tmpBool;
			if (System.IO.File.Exists(rOutput.FullName))
				tmpBool = true;
			else
				tmpBool = System.IO.Directory.Exists(rOutput.FullName);
			if (!tmpBool)
			{
				//UPGRADE_TODO: Method 'java.io.File.mkdirs' was converted to 'System.IO.Directory.CreateDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilemkdirs'"
				System.IO.Directory.CreateDirectory(new System.IO.FileInfo(rDirectory).FullName);
				System.Console.Out.WriteLine("Creating " + rDirectory + " directory");
			}
			
			for (int indicator = 0; indicator < experiment.m_indicatorList.Length; indicator++)
			{
				System.Console.Out.WriteLine("Indicator: " + experiment.m_indicatorList[indicator]);
				System.String rFile = rDirectory + "/" + prefix + "." + experiment.m_indicatorList[indicator] + ".Boxplot.R";
				
				//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
				//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
				System.IO.StreamWriter os = new System.IO.StreamWriter(rFile, false, System.Text.Encoding.Default);
				os.Write("postscript(\"" + prefix + "." + experiment.m_indicatorList[indicator] + ".Boxplot.eps\", horizontal=FALSE, onefile=FALSE, height=8, width=12, pointsize=10)" + "\n");
				//os.write("resultDirectory<-\"../data/" + m_experimentName +"\"" + "\n");
				os.Write("resultDirectory<-\"../data/" + "\"" + "\n");
				os.Write("qIndicator <- function(indicator, problem)" + "\n");
				os.Write("{" + "\n");
				
				for (int i = 0; i < experiment.m_algorithmNameList.Length; i++)
				{
					os.Write("file" + experiment.m_algorithmNameList[i] + "<-paste(resultDirectory, \"" + experiment.m_algorithmNameList[i] + "\", sep=\"/\")" + "\n");
					os.Write("file" + experiment.m_algorithmNameList[i] + "<-paste(file" + experiment.m_algorithmNameList[i] + ", " + "problem, sep=\"/\")" + "\n");
					os.Write("file" + experiment.m_algorithmNameList[i] + "<-paste(file" + experiment.m_algorithmNameList[i] + ", " + "indicator, sep=\"/\")" + "\n");
					os.Write(experiment.m_algorithmNameList[i] + "<-scan(" + "file" + experiment.m_algorithmNameList[i] + ")" + "\n");
					os.Write("\n");
				} // for
				
				os.Write("algs<-c(");
				for (int i = 0; i < experiment.m_algorithmNameList.Length - 1; i++)
				{
					os.Write("\"" + experiment.m_algorithmNameList[i] + "\",");
				} // for
				os.Write("\"" + experiment.m_algorithmNameList[experiment.m_algorithmNameList.Length - 1] + "\")" + "\n");
				
				os.Write("boxplot(");
				for (int i = 0; i < experiment.m_algorithmNameList.Length; i++)
				{
					os.Write(experiment.m_algorithmNameList[i] + ",");
				} // for
				if (notch)
				{
					os.Write("names=algs, notch = TRUE)" + "\n");
				}
				else
				{
					os.Write("names=algs, notch = FALSE)" + "\n");
				}
				os.Write("titulo <-paste(indicator, problem, sep=\":\")" + "\n");
				os.Write("title(main=titulo)" + "\n");
				
				os.Write("}" + "\n");
				
				os.Write("par(mfrow=c(" + rows + "," + cols + "))" + "\n");
				
				os.Write("indicator<-\"" + experiment.m_indicatorList[indicator] + "\"" + "\n");
				
				for (int i = 0; i < problems.Length; i++)
				{
					os.Write("qIndicator(indicator, \"" + problems[i] + "\")" + "\n");
				}
				
				os.Close();
			} // for
		} // generateRBoxplotScripts
	}
}