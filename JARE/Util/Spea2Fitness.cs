/// <summary> Spea2Fitness.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
using System.Collections;
using System.Collections.Generic;
using JARE.Base;
using JARE.Base.operators.comparator;
namespace JARE.util
{
	
	/// <summary> This class implements some facilities for calculating the Spea2Fitness</summary>
	public class Spea2Fitness
	{
		
		/// <summary> Stores the distance between solutions</summary>
		private double[][] m_distance = null;
		
		/// <summary> Stores the solutionSet to assign the fitness</summary>
		private SolutionSet m_solutionSet = null;
		
		/// <summary> stores a <code>Distance</code> object</summary>
		private static readonly Distance m_distanceObject = new Distance();
		
		/// <summary> stores a <code>Comparator</code> for distance between nodes checking</summary>
		private static readonly System.Collections.Generic.IComparer<DistanceNode> m_distanceNodeComparator = new DistanceNodeComparator();
		
		/// <summary> stores a <code>Comparator</code> for dominance checking</summary>
		private static readonly System.Collections.IComparer m_dominance = new DominanceComparator();
		
		/// <summary> Constructor.
		/// Creates a new instance of Spea2Fitness for a given <code>SolutionSet</code>.
		/// </summary>
		/// <param name="solutionSet">The <code>SolutionSet</code>
		/// </param>
		public Spea2Fitness(SolutionSet solutionSet)
		{
			m_distance = m_distanceObject.distanceMatrix(solutionSet);
			m_solutionSet = solutionSet;
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				m_solutionSet.getSolution(i).Location = i;
			} 
		} 
		
		
		/// <summary> Assigns fitness for all the solutions.</summary>
		public virtual void  fitnessAssign()
		{
			double[] strength = new double[m_solutionSet.size()];
			double[] rawFitness = new double[m_solutionSet.size()];
			double kDistance;
			
			
			//Calculate the strength value
			// strength(i) = |{j | j <- SolutionSet and i dominate j}|
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				for (int j = 0; j < m_solutionSet.size(); j++)
				{
					if (m_dominance.Compare(m_solutionSet.getSolution(i), m_solutionSet.getSolution(j)) == - 1)
					{
						strength[i] += 1.0;
					}        
				} 
			} 
			
			
			//Calculate the raw fitness
			// rawFitness(i) = |{sum strenght(j) | j <- SolutionSet and j dominate i}|
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				for (int j = 0; j < m_solutionSet.size(); j++)
				{
					if (m_dominance.Compare(m_solutionSet.getSolution(i), m_solutionSet.getSolution(j)) == 1)
					{
						rawFitness[i] += strength[j];
					}
				}
			}
			
			
			// Add the distance to the k-th individual. In the reference paper of SPEA2, 
			// k = sqrt(population.size()), but a value of k = 1 recommended. See
			// http://www.tik.ee.ethz.ch/pisa/selectors/spea2/spea2_documentation.txt
			int k = 1;
			for (int i = 0; i < m_distance.Length; i++)
			{
				System.Array.Sort(m_distance[i]);
				kDistance = 1.0 / (m_distance[i][k] + 2.0); // Calcule de D(i) distance
				//population.get(i).setFitness(rawFitness[i]);
				m_solutionSet.getSolution(i).Fitness = rawFitness[i] + kDistance;
			}                   
		} 
		
		
		/// <summary>  Gets 'size' elements from a population of more than 'size' elements
		/// using for this de enviromentalSelection truncation
		/// </summary>
		/// <param name="size">The number of elements to get.
		/// </param>
		public virtual SolutionSet environmentalSelection(int size)
		{
			
			if (m_solutionSet.size() < size)
			{
				size = m_solutionSet.size();
			}
			
			// Create a new auxiliar population for no alter the original population
			SolutionSet aux = new SolutionSet(m_solutionSet.size());
			
			int i = 0;
			while (i < m_solutionSet.size())
			{
				if (m_solutionSet.getSolution(i).Fitness < 1.0)
				{
					aux.add(m_solutionSet.getSolution(i));
					m_solutionSet.remove(i);
				}
				else
				{
					i++;
				}
			} 
			
			if (aux.size() < size)
			{
				FitnessComparator comparator = new FitnessComparator();
				m_solutionSet.sort(comparator);
				int remain = size - aux.size();
				for (i = 0; i < remain; i++)
				{
					aux.add(m_solutionSet.getSolution(i));
				}
				return aux;
			}
			else if (aux.size() == size)
			{
				return aux;
			}
			
			double[][] distance = m_distanceObject.distanceMatrix(aux);
            List<List<DistanceNode>> distanceList = new List<List<DistanceNode>>(); 
            
			for (int pos = 0; pos < aux.size(); pos++)
			{
				aux.getSolution(pos).Location = pos;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				List < DistanceNode > distanceNodeList = new List < DistanceNode >();
				for (int r = 0; r < aux.size(); r++)
				{
					if (pos != r)
					{
						distanceNodeList.Add(new DistanceNode(distance[pos][r], r));
					} 
				} 
				distanceList.Add(distanceNodeList);
			}                         
			
			for (int q = 0; q < distanceList.Count; q++)
			{
             
                distanceList[q].Sort(m_distanceNodeComparator);
				//Collections.sort(distanceList.get(q), m_distanceNodeComparator);
			} 
			
			while (aux.size() > size)
			{
				//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				double minDistance = Double.MaxValue;
				int toRemove = 0;
				i = 0;
                //Iterator<List<DistanceNode>> iterator = distanceList.iterator();
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				//while (iterator.hasNext())
                foreach (List<DistanceNode> dn in distanceList) 
				{
					//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
                    
                    if (dn[0].Distance < minDistance)
					{
						toRemove = i;
						minDistance = dn[0].Distance;
						//i y toRemove have the same distance to the first solution
					}
					else if (dn[0].Distance == minDistance)
					{
						int k = 0;
                      
						while ((dn[k].Distance == distanceList[toRemove][k].Distance) && k < (distanceList[i].Count - 1))
						{
							k++;
						}
						
						if (dn[k].Distance < distanceList[toRemove][k].Distance)
						{
							toRemove = i;
						} 
					} 
					i++;
				} 
				
				int tmp = aux.getSolution(toRemove).Location;
				aux.remove(toRemove);
				distanceList.RemoveAt(toRemove);
				
                //Iterator<List<DistanceNode>> externIterator = distanceList.iterator();
                foreach (List<DistanceNode> extItem in distanceList)
                {
                    //Iterator<DistanceNode> interIterator = externIterator.next().iterator();
					//while (interIterator.hasNext())
                    foreach (DistanceNode intItem in extItem)
					{
                        if (intItem.Reference == tmp)
						{
                            extItem.Remove(intItem);
						}
					}
				}    
			} 
			return aux;
		} 
	} 
}