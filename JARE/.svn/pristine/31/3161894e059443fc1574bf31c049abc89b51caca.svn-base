/// <summary> RWilcoxon.java
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
	
	public class RWilcoxon
	{
		/// <summary> Generate R scripts that generate latex tables including the Wilcoxon test</summary>
		/// <param name="problems">
		/// </param>
		/// <param name="prefix">
		/// </param>
		/// <throws>  java.io.FileNotFoundException </throws>
		/// <throws>  java.io.IOException </throws>
		public static void  generateScripts(System.String[] problems, System.String prefix, Experiment experiment)
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
				System.String rFile = rDirectory + "/" + prefix + "." + experiment.m_indicatorList[indicator] + ".Wilcox.R";
				System.String texFile = rDirectory + "/" + prefix + "." + experiment.m_indicatorList[indicator] + ".Wilcox.tex";
				
				//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
				//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
				System.IO.StreamWriter os = new System.IO.StreamWriter(rFile, false, System.Text.Encoding.Default);
				System.String output = "write(\"\", \"" + texFile + "\",append=FALSE)";
				os.Write(output + "\n");
				
				// Generate function latexHeader()
				System.String dataDirectory = experiment.m_experimentBaseDirectory + "/data";
				os.Write("resultDirectory<-\"" + dataDirectory + "\"" + "\n");
				output = "latexHeader <- function() {" + "\n" + "  write(\"\\\\documentclass{article}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\title{StandardStudy}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\usepackage{amssymb}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\author{A.J.Nebro}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\begin{document}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\maketitle\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\section{Tables}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\\", \"" + texFile + "\", append=TRUE)" + "\n" + "}" + "\n";
				os.Write(output + "\n");
				
				// Write function latexTableHeader
				System.String latexTableLabel = "";
				System.String latexTabularAlignment = "";
				System.String latexTableFirstLine = "";
				System.String latexTableCaption = "";
				
				latexTableCaption = "  write(\"\\\\caption{\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(problem, \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"." + experiment.m_indicatorList[indicator] + ".}\", \"" + texFile + "\", append=TRUE)" + "\n";
				latexTableLabel = "  write(\"\\\\label{Table:\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(problem, \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"." + experiment.m_indicatorList[indicator] + ".}\", \"" + texFile + "\", append=TRUE)" + "\n";
				latexTabularAlignment = "l";
				latexTableFirstLine = "\\\\hline ";
				
				for (int i = 1; i < experiment.m_algorithmNameList.Length; i++)
				{
					latexTabularAlignment += "c";
					latexTableFirstLine += (" & " + experiment.m_algorithmNameList[i]);
				} // for
				//latexTableFirstLine += "\\\\\\\\\",\"" + texFile + "\", append=TRUE)" + "\n";
				latexTableFirstLine += "\\\\\\\\ \"";
				
				// Generate function latexTableHeader()
				output = "latexTableHeader <- function(problem, tabularString, latexTableFirstLine) {" + "\n" + "  write(\"\\\\begin{table}\", \"" + texFile + "\", append=TRUE)" + "\n" + latexTableCaption + "\n" + latexTableLabel + "\n" + "  write(\"\\\\centering\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\begin{scriptsize}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\begin{tabular}{\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(tabularString, \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(latexTableFirstLine, \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\hline \", \"" + texFile + "\", append=TRUE)" + "\n" + "}" + "\n";
				os.Write(output + "\n");
				
				// Generate function latexTableTail()
				output = "latexTableTail <- function() { " + "\n" + "  write(\"\\\\hline\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\end{tabular}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\end{scriptsize}\", \"" + texFile + "\", append=TRUE)" + "\n" + "  write(\"\\\\end{table}\", \"" + texFile + "\", append=TRUE)" + "\n" + "}" + "\n";
				os.Write(output + "\n");
				
				// Generate function latexTail()
				output = "latexTail <- function() { " + "\n" + "  write(\"\\\\end{document}\", \"" + texFile + "\", append=TRUE)" + "\n" + "}" + "\n";
				os.Write(output + "\n");
				
				if (/*(System.Object)*/ experiment.m_indicatorMinimize[experiment.m_indicatorList[indicator]] == true)
				{
					// minimize by default
					// Generate function printTableLine()
					output = "printTableLine <- function(indicator, algorithm1, algorithm2, i, j, problem) { " + "\n" + "  file1<-paste(resultDirectory, algorithm1, sep=\"/\")" + "\n" + "  file1<-paste(file1, problem, sep=\"/\")" + "\n" + "  file1<-paste(file1, indicator, sep=\"/\")" + "\n" + "  data1<-scan(file1)" + "\n" + "  file2<-paste(resultDirectory, algorithm2, sep=\"/\")" + "\n" + "  file2<-paste(file2, problem, sep=\"/\")" + "\n" + "  file2<-paste(file2, indicator, sep=\"/\")" + "\n" + "  data2<-scan(file2)" + "\n" + "  if (i == j) {" + "\n" + "    write(\"-- \", \"" + texFile + "\", append=TRUE)" + "\n" + "  }" + "\n" + "  else if (i < j) {" + "\n" + "    if (wilcox.test(data1, data2)$p.value <= 0.05) {" + "\n" + "      if (median(data1) <= median(data2)) {" + "\n" + "        write(\"$\\\\blacktriangle$\", \"" + texFile + "\", append=TRUE)" + "\n" + "      }" + "\n" + "      else {" + "\n" + "        write(\"$\\\\triangledown$\", \"" + texFile + "\", append=TRUE) " + "\n" + "      }" + "\n" + "    }" + "\n" + "    else {" + "\n" + "      write(\"--\", \"" + texFile + "\", append=TRUE) " + "\n" + "    }" + "\n" + "  }" + "\n" + "  else {" + "\n" + "    write(\" \", \"" + texFile + "\", append=TRUE)" + "\n" + "  }" + "\n" + "}" + "\n";
				}
				// if
				else
				{
					// Generate function printTableLine()
					output = "printTableLine <- function(indicator, algorithm1, algorithm2, i, j, problem) { " + "\n" + "  file1<-paste(resultDirectory, algorithm1, sep=\"/\")" + "\n" + "  file1<-paste(file1, problem, sep=\"/\")" + "\n" + "  file1<-paste(file1, indicator, sep=\"/\")" + "\n" + "  data1<-scan(file1)" + "\n" + "  file2<-paste(resultDirectory, algorithm2, sep=\"/\")" + "\n" + "  file2<-paste(file2, problem, sep=\"/\")" + "\n" + "  file2<-paste(file2, indicator, sep=\"/\")" + "\n" + "  data2<-scan(file2)" + "\n" + "  if (i == j) {" + "\n" + "    write(\"--\", \"" + texFile + "\", append=TRUE)" + "\n" + "  }" + "\n" + "  else if (i < j) {" + "\n" + "    if (wilcox.test(data1, data2)$p.value <= 0.05) {" + "\n" + "      if (median(data1) >= median(data2)) {" + "\n" + "        write(\"$\\\\blacktriangle$\", \"" + texFile + "\", append=TRUE)" + "\n" + "      }" + "\n" + "      else {" + "\n" + "        write(\"$\\\\triangledown$\", \"" + texFile + "\", append=TRUE) " + "\n" + "      }" + "\n" + "    }" + "\n" + "    else {" + "\n" + "      write(\"--\", \"" + texFile + "\", append=TRUE) " + "\n" + "    }" + "\n" + "  }" + "\n" + "  else {" + "\n" + "    write(\" \", \"" + texFile + "\", append=TRUE)" + "\n" + "  }" + "\n" + "}" + "\n";
				}
				os.Write(output + "\n");
				
				// Start of the R script
				output = "### START OF SCRIPT ";
				os.Write(output + "\n");
				
				System.String problemList = "problemList <-c(";
				System.String algorithmList = "algorithmList <-c(";
				
				for (int i = 0; i < (problems.Length - 1); i++)
				{
					problemList += ("\"" + problems[i] + "\", ");
				}
				problemList += ("\"" + problems[problems.Length - 1] + "\") ");
				
				for (int i = 0; i < (experiment.m_algorithmNameList.Length - 1); i++)
				{
					algorithmList += ("\"" + experiment.m_algorithmNameList[i] + "\", ");
				}
				algorithmList += ("\"" + experiment.m_algorithmNameList[experiment.m_algorithmNameList.Length - 1] + "\") ");
				
				latexTabularAlignment = "l";
				for (int i = 1; i < experiment.m_algorithmNameList.Length; i++)
				{
					latexTabularAlignment += "c";
				} // for
				System.String tabularString = "tabularString <-c(" + "\"" + latexTabularAlignment + "\"" + ") ";
				System.String tableFirstLine = "latexTableFirstLine <-c(" + "\"" + latexTableFirstLine + ") ";
				
				output = "# Constants" + "\n" + problemList + "\n" + algorithmList + "\n" + tabularString + "\n" + tableFirstLine + "\n" + "indicator<-\"" + experiment.m_indicatorList[indicator] + "\"";
				os.Write(output + "\n");
				
				
				output = "\n # Step 1.  Writes the latex header" + "\n" + "latexHeader()";
				os.Write(output + "\n");
				
				// Generate tables per problem
				output = "# Step 2. Problem loop " + "\n" + "for (problem in problemList) {" + "\n" + "  latexTableHeader(problem,  tabularString, latexTableFirstLine)" + "\n\n" + "  indx = 0" + "\n" + "  for (i in algorithmList) {" + "\n" + "    if (i != \"" + experiment.m_algorithmNameList[experiment.m_algorithmNameList.Length - 1] + "\") {" + "\n" + "      write(i , \"" + texFile + "\", append=TRUE)" + "\n" + "      write(\" & \", \"" + texFile + "\", append=TRUE)" + "\n" + "      jndx = 0 " + "\n" + "      for (j in algorithmList) {" + "\n" + "        if (jndx != 0) {" + "\n" + "          if (indx != jndx) {" + "\n" + "            printTableLine(indicator, i, j, indx, jndx, problem)" + "\n" + "          }" + "\n" + "          else {" + "\n" + "            write(\"  \", \"" + texFile + "\", append=TRUE)" + "\n" + "          }" + "\n" + "          if (j != \"" + experiment.m_algorithmNameList[experiment.m_algorithmNameList.Length - 1] + "\") {" + "\n" + "            write(\" & \", \"" + texFile + "\", append=TRUE)" + "\n" + "          }" + "\n" + "          else {" + "\n" + "            write(\" \\\\\\\\ \", \"" + texFile + "\", append=TRUE)" + "\n" + "          }" + "\n" + "        }" + "\n" + "        jndx = jndx + 1" + "\n" + "      }" + "\n" + "      indx = indx + 1" + "\n" + "    }" + "\n" + "  }" + "\n" + "\n" + "  latexTableTail()" + "\n" + "} # for problem" + "\n";
				os.Write(output + "\n");
				
				// Generate full table
				problemList = "";
				for (int i = 0; i < problems.Length; i++)
				{
					problemList += (problems[i] + " ");
				}
				// The tabular environment and the latexTableFirstLine variable must be redefined
				latexTabularAlignment = "| l | ";
				latexTableFirstLine = "\\\\hline \\\\multicolumn{1}{|c|}{}";
				for (int i = 1; i < experiment.m_algorithmNameList.Length; i++)
				{
					for (int j = 0; j < problems.Length; j++)
					{
						latexTabularAlignment += "p{0.15cm}  ";
						//latexTabularAlignment += "c ";
					} // for
					latexTableFirstLine += (" & \\\\multicolumn{" + problems.Length + "}{c|}{" + experiment.m_algorithmNameList[i] + "}");
					latexTabularAlignment += " | ";
				} // for
				latexTableFirstLine += " \\\\\\\\";
				
				tabularString = "tabularString <-c(" + "\"" + latexTabularAlignment + "\"" + ") ";
				latexTableFirstLine = "latexTableFirstLine <-c(" + "\"" + latexTableFirstLine + "\"" + ") ";
				
				output = tabularString;
				os.Write(output + "\n" + "\n");
				output = latexTableFirstLine;
				os.Write(output + "\n" + "\n");
				
				output = "# Step 3. Problem loop " + "\n" + "latexTableHeader(\"" + problemList + "\", tabularString, latexTableFirstLine)" + "\n\n" + "indx = 0" + "\n" + "for (i in algorithmList) {" + "\n" + "  if (i != \"" + experiment.m_algorithmNameList[experiment.m_algorithmNameList.Length - 1] + "\") {" + "\n" + "    write(i , \"" + texFile + "\", append=TRUE)" + "\n" + "    write(\" & \", \"" + texFile + "\", append=TRUE)" + "\n" + "\n" + "    jndx = 0" + "\n" + "    for (j in algorithmList) {" + "\n" + "      for (problem in problemList) {" + "\n" + "        if (jndx != 0) {" + "\n" + "          if (i != j) {" + "\n" + "            printTableLine(indicator, i, j, indx, jndx, problem)" + "\n" + "          }" + "\n" + "          else {" + "\n" + "            write(\"  \", \"" + texFile + "\", append=TRUE)" + "\n" + "          } " + "\n" + "          if (problem == \"" + problems[problems.Length - 1] + "\") {" + "\n" + "            if (j == \"" + experiment.m_algorithmNameList[experiment.m_algorithmNameList.Length - 1] + "\") {" + "\n" + "              write(\" \\\\\\\\ \", \"" + texFile + "\", append=TRUE)" + "\n" + "            } " + "\n" + "            else {" + "\n" + "              write(\" & \", \"" + texFile + "\", append=TRUE)" + "\n" + "            }" + "\n" + "          }" + "\n" + "     else {" + "\n" + "    write(\"&\", \"" + texFile + "\", append=TRUE)" + "\n" + "     }" + "\n" + "        }" + "\n" + "      }" + "\n" + "      jndx = jndx + 1" + "\n" + "    }" + "\n" + "    indx = indx + 1" + "\n" + "  }" + "\n" + "} # for algorithm" + "\n" + "\n" + "  latexTableTail()" + "\n";
				
				os.Write(output + "\n");
				
				// Generate end of file
				output = "#Step 3. Writes the end of latex file " + "\n" + "latexTail()" + "\n";
				os.Write(output + "\n");
				
				
				os.Close();
			} // for
		} // generateRBoxplotScripts
	}
}