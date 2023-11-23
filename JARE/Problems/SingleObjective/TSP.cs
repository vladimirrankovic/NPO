/// <summary> TSP.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using PermutationSolutionType = JARE.Base.solutionType.PermutationSolutionType;
using Binary = JARE.Base.variable.Binary;
using Permutation = JARE.Base.variable.Permutation;
using SupportClass = JARE.support.SupportClass;
using StreamTokenizerSupport = JARE.support.StreamTokenizerSupport;
namespace JARE.problems.singleObjective
{
	
	/// <summary> Class representing a TSP (Traveling Salesman Problem) problem.</summary>
	[Serializable]
	public class TSP:Problem
	{
		
		public int m_numberOfCities;
		public double[][] m_distanceMatrix;
		
		
		/// <summary> Creates a new TSP problem instance. It accepts data files from TSPLIB</summary>
		/// <param name="filename">The file containing the definition of the problem
		/// </param>
		public TSP(System.String filename)
		{
			m_numberOfVariables = 1;
			m_numberOfObjectives = 1;
			m_numberOfConstraints = 0;
			m_problemName = "TSP";
			
			m_solutionType = new PermutationSolutionType(this);
			
			m_variableType = new System.Type[m_numberOfVariables];
			m_length = new int[m_numberOfVariables];
			
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			m_variableType[0] = System.Type.GetType("JARE.Base.variable.Permutation");
			
			readProblem(filename);
			System.Console.Out.WriteLine(m_numberOfCities);
			m_length[0] = m_numberOfCities;
		} // TSP
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		public override void  evaluate(Solution solution)
		{
			double fitness;
			
			fitness = 0.0;
			
			for (int i = 0; i < (m_numberOfCities - 1); i++)
			{
				int x;
				int y;
				
				x = ((Permutation) solution.DecisionVariables[0]).m_vector[i];
				y = ((Permutation) solution.DecisionVariables[0]).m_vector[i + 1];
				//  cout << "I : " << i << ", x = " << x << ", y = " << y << endl ;    
				fitness += m_distanceMatrix[x][y];
			} // for
			int firstCity;
			int lastCity;
			
			firstCity = ((Permutation) solution.DecisionVariables[0]).m_vector[0];
			lastCity = ((Permutation) solution.DecisionVariables[0]).m_vector[m_numberOfCities - 1];
			fitness += m_distanceMatrix[firstCity][lastCity];
			
			solution.setObjective(0, fitness);
		} // evaluate
		
		
		public virtual void  readProblem(System.String fileName)
		{
            //VLADA CONVERT - OSTAVLJENO ZA KOMPAJLIRANJE!!!
            //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
			System.IO.StreamReader inputFile = new System.IO.StreamReader(new System.IO.StreamReader(new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read), System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read), System.Text.Encoding.Default).CurrentEncoding);
			
			StreamTokenizerSupport token = new StreamTokenizerSupport(inputFile);
			try
			{
				bool found;
				found = false;
				
				token.NextToken();
				while (!found)
				{
					if ((token.sval != null) && ((String.CompareOrdinal(token.sval, "DIMENSION") == 0)))
						found = true;
					else
						token.NextToken();
				} // while
				
				token.NextToken();
				token.NextToken();
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m_numberOfCities = (int) token.nval;
				
				m_distanceMatrix = new double[m_numberOfCities][];
				for (int i = 0; i < m_numberOfCities; i++)
				{
					m_distanceMatrix[i] = new double[m_numberOfCities];
				}
				
				// Find the string SECTION  
				found = false;
				token.NextToken();
				while (!found)
				{
					if ((token.sval != null) && ((String.CompareOrdinal(token.sval, "SECTION") == 0)))
						found = true;
					else
						token.NextToken();
				} // while
				
				// Read the data
				
				double[] c = new double[2 * m_numberOfCities];
				
				for (int i = 0; i < m_numberOfCities; i++)
				{
					token.NextToken();
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					int j = (int) token.nval;
					
					token.NextToken();
					c[2 * (j - 1)] = token.nval;
					token.NextToken();
					c[2 * (j - 1) + 1] = token.nval;
				} // for
				
				double dist;
				for (int k = 0; k < m_numberOfCities; k++)
				{
					m_distanceMatrix[k][k] = 0;
					for (int j = k + 1; j < m_numberOfCities; j++)
					{
						dist = System.Math.Sqrt(System.Math.Pow((c[k * 2] - c[j * 2]), 2.0) + System.Math.Pow((c[k * 2 + 1] - c[j * 2 + 1]), 2));
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						dist = (int) (dist + .5);
						m_distanceMatrix[k][j] = dist;
						m_distanceMatrix[j][k] = dist;
					} // for
				} // for
			}
			// try
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine("TSP.readProblem(): error when reading data file " + e);
				System.Environment.Exit(1);
			} // catch
		} // readProblem
	} // TSP
}