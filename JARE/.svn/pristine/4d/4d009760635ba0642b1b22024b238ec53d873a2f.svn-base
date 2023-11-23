/// <summary> MetricsUtil.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using System.Collections.Generic;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using NonDominatedSolutionList = JARE.util.NonDominatedSolutionList;
namespace JARE.qualityIndicator.util
{
	
	/// <summary> This class provides some facilities for metrics. 
	/// 
	/// </summary>
	public class MetricsUtil
	{
		
		/// <summary> This method reads a Pareto Front for a file.</summary>
		/// <param name="path">The path to the file that contains the pareto front
		/// </param>
		/// <returns> double [][] whit the pareto front
		/// 
		/// </returns>
		public virtual double[][] readFront(System.String path)
		{
			try
			{
				// Open the file
				//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
				System.IO.FileStream fis = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				System.IO.StreamReader isr = new System.IO.StreamReader(fis, System.Text.Encoding.Default);
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.IO.StreamReader br = new System.IO.StreamReader(isr.BaseStream, isr.CurrentEncoding);
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				
                List < double [] > list = new List < double [] >();
				int numberOfObjectives = 0;
				System.String aux = br.ReadLine();
				while (aux != null)
				{
                    JARE.support.Tokenizer st = new JARE.support.Tokenizer(aux);
					int i = 0;
					numberOfObjectives = st.Count;
					double[] vector = new double[st.Count];
					while (st.HasMoreTokens())
					{
						//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						double val = (System.Double.Parse(st.NextToken()));
						vector[i] = val;
						i++;
					}
					list.Add(vector);
					aux = br.ReadLine();
				}
				
				br.Close();
				
				double[][] front = new double[list.Count][];
				for (int i = 0; i < list.Count; i++)
				{
					front[i] = new double[numberOfObjectives];
				}
				for (int i = 0; i < list.Count; i++)
				{
					front[i] = list[i];
				}
				return front;
			}
			catch (System.Exception e)
			{
				System.Console.Out.WriteLine("InputFacilities crashed reading for file: " + path);
				JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			}
			return null;
		} // readFront
		
		/// <summary>Gets the maximun values for each objectives in a given pareto
		/// front
		/// </summary>
		/// <param name="front">The pareto front
		/// </param>
		/// <param name="noObjectives">Number of objectives in the pareto front
		/// </param>
		/// <returns> double [] An array of noOjectives values whit the maximun values
		/// for each objective
		/// 
		/// </returns>
		public virtual double[] getMaximumValues(double[][] front, int noObjectives)
		{
			double[] maximumValue = new double[noObjectives];
			for (int i = 0; i < noObjectives; i++)
				maximumValue[i] = System.Double.NegativeInfinity;
			
			
			for (int i = 0; i < front.Length; i++)
			{
				for (int j = 0; j < front[i].Length; j++)
				{
					if (front[i][j] > maximumValue[j])
						maximumValue[j] = front[i][j];
				}
			}
			
			return maximumValue;
		} // getMaximumValues
		
		
		/// <summary>Gets the minimun values for each objectives in a given pareto
		/// front
		/// </summary>
		/// <param name="front">The pareto front
		/// </param>
		/// <param name="noObjectives">Number of objectives in the pareto front
		/// </param>
		/// <returns> double [] An array of noOjectives values whit the minimum values
		/// for each objective
		/// 
		/// </returns>
		public virtual double[] getMinimumValues(double[][] front, int noObjectives)
		{
			double[] minimumValue = new double[noObjectives];
			for (int i = 0; i < noObjectives; i++)
			{
				//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				minimumValue[i] = System.Double.MaxValue;
			}
			
			for (int i = 0; i < front.Length; i++)
			{
				for (int j = 0; j < front[i].Length; j++)
				{
					if (front[i][j] < minimumValue[j])
						minimumValue[j] = front[i][j];
				}
			}
			return minimumValue;
		} // getMinimumValues
		
		
		/// <summary>  This method returns the distance (taken the euclidean distance) between
		/// two points given as <code>double []</code>
		/// </summary>
		/// <param name="a">A point
		/// </param>
		/// <param name="b">A point
		/// </param>
		/// <returns> The euclidean distance between the points
		/// 
		/// </returns>
		public virtual double distance(double[] a, double[] b)
		{
			double distance = 0.0;
			
			for (int i = 0; i < a.Length; i++)
			{
				distance += System.Math.Pow(a[i] - b[i], 2.0);
			}
			return System.Math.Sqrt(distance);
		} // distance
		
		
		/// <summary> Gets the distance between a point and the nearest one in
		/// a given front (the front is given as <code>double [][]</code>)
		/// </summary>
		/// <param name="point">The point
		/// </param>
		/// <param name="front">The front that contains the other points to calculate the 
		/// distances
		/// </param>
		/// <returns> The minimun distance between the point and the front
		/// 
		/// </returns>
		public virtual double distanceToClosedPoint(double[] point, double[][] front)
		{
			double minDistance = distance(point, front[0]);
			
			
			for (int i = 1; i < front.Length; i++)
			{
				double aux = distance(point, front[i]);
				if (aux < minDistance)
				{
					minDistance = aux;
				}
			}
			
			return minDistance;
		} // distanceToClosedPoint
		
		
		/// <summary> Gets the distance between a point and the nearest one in
		/// a given front, and this distance is greater than 0.0
		/// </summary>
		/// <param name="point">The point
		/// </param>
		/// <param name="front">The front that contains the other points to calculate the
		/// distances
		/// </param>
		/// <returns> The minimun distances greater than zero between the point and
		/// the front
		/// </returns>
		public virtual double distanceToNearestPoint(double[] point, double[][] front)
		{
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			double minDistance = System.Double.MaxValue;
			
			for (int i = 0; i < front.Length; i++)
			{
				double aux = distance(point, front[i]);
				if ((aux < minDistance) && (aux > 0.0))
				{
					minDistance = aux;
				}
			}
			
			return minDistance;
		} // distanceToNearestPoint
		
		/// <summary> This method receives a pareto front and two points, one whit maximun values
		/// and the other with minimun values allowed, and returns a the normalized
		/// pareto front.
		/// </summary>
		/// <param name="front">A pareto front.
		/// </param>
		/// <param name="maximumValue">The maximun values allowed
		/// </param>
		/// <param name="minimumValue">The mininum values allowed
		/// </param>
		/// <returns> the normalized pareto front
		/// 
		/// </returns>
		public virtual double[][] getNormalizedFront(double[][] front, double[] maximumValue, double[] minimumValue)
		{
			
			double[][] normalizedFront = new double[front.Length][];
			
			for (int i = 0; i < front.Length; i++)
			{
				normalizedFront[i] = new double[front[i].Length];
				for (int j = 0; j < front[i].Length; j++)
				{
					normalizedFront[i][j] = (front[i][j] - minimumValue[j]) / (maximumValue[j] - minimumValue[j]);
				}
			}
			return normalizedFront;
		} // getNormalizedFront
		
		
		/// <summary> This method receives a normalized pareto front and return the inverted one.
		/// This operation needed for minimization problems
		/// </summary>
		/// <param name="front">The pareto front to inverse
		/// </param>
		/// <returns> The inverted pareto front
		/// 
		/// </returns>
		public virtual double[][] invertedFront(double[][] front)
		{
			double[][] invertedFront = new double[front.Length][];
			
			for (int i = 0; i < front.Length; i++)
			{
				invertedFront[i] = new double[front[i].Length];
				for (int j = 0; j < front[i].Length; j++)
				{
					if (front[i][j] <= 1.0 && front[i][j] >= 0.0)
					{
						invertedFront[i][j] = 1.0 - front[i][j];
					}
					else if (front[i][j] > 1.0)
					{
						invertedFront[i][j] = 0.0;
					}
					else if (front[i][j] < 0.0)
					{
						invertedFront[i][j] = 1.0;
					}
				}
			}
			return invertedFront;
		} // invertedFront
		
		/// <summary> Reads a set of non dominated solutions from a file</summary>
		/// <param name="path">The path of the file containing the data
		/// </param>
		/// <returns> A solution set
		/// </returns>
		public virtual SolutionSet readNonDominatedSolutionSet(System.String path)
		{
			try
			{
				/* Open the file */
				//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
				System.IO.FileStream fis = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				System.IO.StreamReader isr = new System.IO.StreamReader(fis, System.Text.Encoding.Default);
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.IO.StreamReader br = new System.IO.StreamReader(isr.BaseStream, isr.CurrentEncoding);
				
				SolutionSet solutionSet = new NonDominatedSolutionList();
				
				System.String aux = br.ReadLine();
				while (aux != null)
				{
                    JARE.support.Tokenizer st = new JARE.support.Tokenizer(aux);
					int i = 0;
					Solution solution = new Solution(st.Count);
					while (st.HasMoreTokens())
					{
						//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						double val = (System.Double.Parse(st.NextToken()));
						solution.setObjective(i, val);
						i++;
					}
					solutionSet.add(solution);
					aux = br.ReadLine();
				}
				br.Close();
				return solutionSet;
			}
			catch (System.Exception e)
			{
                System.Console.Out.WriteLine("JARE.qualityIndicator.util.readNonDominatedSolutionSet: " + path);
				JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			}
			return null;
		} // readNonDominatedSolutionSet
	} // MetricsUtil
}