/// <summary> Distance.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using ObjectiveComparator = JARE.Base.operators.comparator.ObjectiveComparator;
using JARE.Base;
namespace JARE.util
{
	
	/// <summary> This class implements some facilities for distances</summary>
	public class Distance
	{
		
		/// <summary> Constructor.</summary>
		public Distance()
		{
			//do nothing.
		} 
		
		
		/// <summary> Returns a matrix with distances between solutions in a 
		/// <code>SolutionSet</code>.
		/// </summary>
		/// <param name="solutionSet">The <code>SolutionSet</code>.
		/// </param>
		/// <returns> a matrix with distances.
		/// </returns>
		public virtual double[][] distanceMatrix(SolutionSet solutionSet)
		{
			Solution solutionI, solutionJ;
			
			//The matrix of distances
			double[][] distance = new double[solutionSet.size()][];
			for (int i = 0; i < solutionSet.size(); i++)
			{
				distance[i] = new double[solutionSet.size()];
			}
			//-> Calculate the distances
			for (int i = 0; i < solutionSet.size(); i++)
			{
				distance[i][i] = 0.0;
				solutionI = solutionSet.getSolution(i);
				for (int j = i + 1; j < solutionSet.size(); j++)
				{
					solutionJ = solutionSet.getSolution(j);
					distance[i][j] = this.distanceBetweenObjectives(solutionI, solutionJ);
					distance[j][i] = distance[i][j];
				} 
			}        
			
			//->Return the matrix of distances
			return distance;
		} // distanceMatrix
		
		/// <summary>Returns the minimum distance from a <code>Solution</code> to a 
		/// <code>SolutionSet according to the objective values</code>.
		/// </summary>
		/// <param name="solution">The <code>Solution</code>.
		/// </param>
		/// <param name="solutionSet">The <code>SolutionSet</code>.
		/// </param>
		/// <returns> The minimum distance between solution and the set.
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual double distanceToSolutionSetInObjectiveSpace(Solution solution, SolutionSet solutionSet)
		{
			//At start point the distance is the max
			double distance = Double.MaxValue;
			
			// found the min distance respect to population
			for (int i = 0; i < solutionSet.size(); i++)
			{
				double aux = this.distanceBetweenObjectives(solution, solutionSet.getSolution(i));
				if (aux < distance)
					distance = aux;
			} 
			
			//->Return the best distance
			return distance;
		}
		
		/// <summary>Returns the minimum distance from a <code>Solution</code> to a 
		/// <code>SolutionSet according to the variable values</code>.
		/// </summary>
		/// <param name="solution">The <code>Solution</code>.
		/// </param>
		/// <param name="solutionSet">The <code>SolutionSet</code>.
		/// </param>
		/// <returns> The minimum distance between solution and the set.
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual double distanceToSolutionSetInSolutionSpace(Solution solution, SolutionSet solutionSet)
		{
			//At start point the distance is the max
			double distance = Double.MaxValue;
			
			// found the min distance respect to population
			for (int i = 0; i < solutionSet.size(); i++)
			{
				double aux = this.distanceBetweenSolutions(solution, solutionSet.getSolution(i));
				if (aux < distance)
					distance = aux;
			} 
			
			//->Return the best distance
			return distance;
		} 
		
		/// <summary>Returns the distance between two solutions in the search space.</summary>
		/// <param name="solutionI">The first <code>Solution</code>. 
		/// </param>
		/// <param name="solutionJ">The second <code>Solution</code>.
		/// </param>
		/// <returns> the distance between solutions.
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual double distanceBetweenSolutions(Solution solutionI, Solution solutionJ)
		{
			//->Obtain his decision variables
			Variable[] decisionVariableI = solutionI.DecisionVariables;
			Variable[] decisionVariableJ = solutionJ.DecisionVariables;
			
			double diff; //Auxiliar var
			double distance = 0.0;
			//-> Calculate the Euclidean distance
			for (int i = 0; i < decisionVariableI.Length; i++)
			{
				diff = decisionVariableI[i].getValue() - decisionVariableJ[i].getValue();
				distance += System.Math.Pow(diff, 2.0);
			} 
			
			//-> Return the euclidean distance
			return System.Math.Sqrt(distance);
		} 
		
		/// <summary>Returns the distance between two solutions in objective space.</summary>
		/// <param name="solutionI">The first <code>Solution</code>.
		/// </param>
		/// <param name="solutionJ">The second <code>Solution</code>.
		/// </param>
		/// <returns> the distance between solutions in objective space.
		/// </returns>
		public virtual double distanceBetweenObjectives(Solution solutionI, Solution solutionJ)
		{
			double diff; //Auxiliar var
			double distance = 0.0;
			//-> Calculate the euclidean distance
			for (int nObj = 0; nObj < solutionI.NumberOfObjectives(); nObj++)
			{
				diff = solutionI.getObjective(nObj) - solutionJ.getObjective(nObj);
				distance += System.Math.Pow(diff, 2.0);
			}    
			
			//Return the euclidean distance
			return System.Math.Sqrt(distance);
		} 
		
		/// <summary>Assigns crowding distances to all solutions in a <code>SolutionSet</code>.</summary>
		/// <param name="solutionSet">The <code>SolutionSet</code>.
		/// </param>
		/// <param name="nObjs">Number of objectives.
		/// </param>
		public virtual void  crowdingDistanceAssignment(SolutionSet solutionSet, int nObjs)
		{
			int size = solutionSet.size();
			
			if (size == 0)
				return ;
			
			if (size == 1)
			{
				solutionSet.getSolution(0).CrowdingDistance = System.Double.PositiveInfinity;
				return ;
			} 
			
			if (size == 2)
			{
				solutionSet.getSolution(0).CrowdingDistance = System.Double.PositiveInfinity;
				solutionSet.getSolution(1).CrowdingDistance = System.Double.PositiveInfinity;
				return ;
			}    
			
			//Use a new SolutionSet to evite alter original solutionSet
			SolutionSet front = new SolutionSet(size);
			for (int i = 0; i < size; i++)
			{
				front.add(solutionSet.getSolution(i));
			}
			
			for (int i = 0; i < size; i++)
				front.getSolution(i).CrowdingDistance = 0.0;
			
			double objetiveMaxn;
			double objetiveMinn;
			double distance;
			
			for (int i = 0; i < nObjs; i++)
			{
				// Sort the population by Obj n     
       
				front.sort(new ObjectiveComparator(i));
				objetiveMinn = front.getSolution(0).getObjective(i);
				objetiveMaxn = front.getSolution(front.size() - 1).getObjective(i);
				
				//Set de crowding distance            
				front.getSolution(0).CrowdingDistance = System.Double.PositiveInfinity;
				front.getSolution(size - 1).CrowdingDistance = System.Double.PositiveInfinity;
				
				for (int j = 1; j < size - 1; j++)
				{
					distance = front.getSolution(j + 1).getObjective(i) - front.getSolution(j - 1).getObjective(i);
					distance = distance / (objetiveMaxn - objetiveMinn);
					distance += front.getSolution(j).CrowdingDistance;
					front.getSolution(j).CrowdingDistance = distance;
				} 
			}       
		}           
	} 
}