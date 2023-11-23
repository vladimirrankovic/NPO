/// <summary> MOCHC.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.util.archive;
using CrowdingComparator = JARE.Base.operators.comparator.CrowdingComparator;
using Binary = JARE.Base.variable.Binary;
using SMException = JARE.util.SMException;
namespace JARE.metaheuristics.mochc
{
	
	/// <summary> 
	/// Class implementing the CHC algorithm.
	/// </summary>
	[Serializable]
	public class MOCHC:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor
		/// Creates a new instance of MOCHC 
		/// </summary>
		public MOCHC(Problem problem)
		{
			m_problem = problem;
		}
		
		/// <summary> Compares two solutionSets to determine if both are equals</summary>
		/// <param name="solutionSet">A <code>SolutionSet</code>
		/// </param>
		/// <param name="newSolutionSet">A <code>SolutionSet</code>
		/// </param>
		/// <returns> true if both are cotains the same solutions, false in other case
		/// </returns>
		public virtual bool equals(SolutionSet solutionSet, SolutionSet newSolutionSet)
		{
			bool found;
			for (int i = 0; i < solutionSet.size(); i++)
			{
				
				int j = 0;
				found = false;
				while (j < newSolutionSet.size())
				{
					
					if (solutionSet.getSolution(i).Equals(newSolutionSet.getSolution(j)))
					{
						found = true;
					}
					j++;
				}
				if (!found)
				{
					return false;
				}
			}
			return true;
		} // equals
		
		/// <summary> Calculate the hamming distance between two solutions</summary>
		/// <param name="solutionOne">A <code>Solution</code>
		/// </param>
		/// <param name="solutionTwo">A <code>Solution</code>
		/// </param>
		/// <returns> the hamming distance between solutions
		/// </returns>
		public virtual int hammingDistance(Solution solutionOne, Solution solutionTwo)
		{
			int distance = 0;
			for (int i = 0; i < m_problem.NumberOfVariables; i++)
			{
				distance += ((Binary) solutionOne.DecisionVariables[i]).hammingDistance((Binary) solutionTwo.DecisionVariables[i]);
			}
			
			return distance;
		} // hammingDistance 
		
		/// <summary> Runs of the MOCHC algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		public override SolutionSet execute()
		{
			int iterations;
			int populationSize;
			int convergenceValue;
			int maxEvaluations;
			int minimumDistance;
			int evaluations;

            System.Collections.Generic.IComparer<JARE.Base.Solution> crowdingComparator = new CrowdingComparator();
			
			Operator crossover;
			Operator parentSelection;
			Operator newGenerationSelection;
			Operator cataclysmicMutation;
			
			double preservedPopulation;
			double initialConvergenceCount;
			bool condition = false;
			SolutionSet solutionSet, offspringPopulation, newPopulation;
			
			// Read parameters
			initialConvergenceCount = ((System.Double) getInputParameter("initialConvergenceCount"));
			preservedPopulation = ((System.Double) getInputParameter("preservedPopulation"));
			convergenceValue = ((System.Int32) getInputParameter("convergenceValue"));
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			
			// Read operators
			crossover = (Operator) getOperator("crossover");
			cataclysmicMutation = (Operator) getOperator("cataclysmicMutation");
			parentSelection = (Operator) getOperator("parentSelection");
			newGenerationSelection = (Operator) getOperator("newGenerationSelection");
			
			iterations = 0;
			evaluations = 0;
			
			//Calculate the maximum problem sizes
			Solution aux = new Solution(m_problem);
			int size = 0;
			for (int var = 0; var < m_problem.NumberOfVariables; var++)
			{
				size += ((Binary) aux.DecisionVariables[var]).NumberOfBits;
			}
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			minimumDistance = (int) System.Math.Floor(initialConvergenceCount * size);
			
			solutionSet = new SolutionSet(populationSize);
			for (int i = 0; i < populationSize; i++)
			{
				Solution solution = new Solution(m_problem);
				m_problem.evaluate(solution);
				m_problem.evaluateConstraints(solution);
				evaluations++;
				solutionSet.add(solution);
			}
			
			while (!condition)
			{
				offspringPopulation = new SolutionSet(populationSize);
				for (int i = 0; i < solutionSet.size() / 2; i++)
				{
					Solution[] parents = (Solution[]) parentSelection.execute(solutionSet);
					
					//Equality condition between solutions
					if (hammingDistance(parents[0], parents[1]) >= (minimumDistance))
					{
						Solution[] offspring = (Solution[]) crossover.execute(parents);
						m_problem.evaluate(offspring[0]);
						m_problem.evaluateConstraints(offspring[0]);
						m_problem.evaluate(offspring[1]);
						m_problem.evaluateConstraints(offspring[1]);
						evaluations += 2;
						offspringPopulation.add(offspring[0]);
						offspringPopulation.add(offspring[1]);
					}
				}
				SolutionSet union = solutionSet.union(offspringPopulation);
				newGenerationSelection.setParameter("populationSize", populationSize);
				newPopulation = (SolutionSet) newGenerationSelection.execute(union);
				
				if (equals(solutionSet, newPopulation))
				{
					minimumDistance--;
				}
				if (minimumDistance <= - convergenceValue)
				{
					
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					minimumDistance = (int) (1.0 / size * (1 - 1.0 / size) * size);
					//minimumDistance = (int) (0.35 * (1 - 0.35) * size);
					
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					int preserve = (int) System.Math.Floor(preservedPopulation * populationSize);
					newPopulation = new SolutionSet(populationSize);
					solutionSet.sort(crowdingComparator);
					for (int i = 0; i < preserve; i++)
					{
						newPopulation.add(new Solution(solutionSet.getSolution(i)));
					}
					for (int i = preserve; i < populationSize; i++)
					{
						Solution solution = new Solution(solutionSet.getSolution(i));
						cataclysmicMutation.execute(solution);
						m_problem.evaluate(solution);
						m_problem.evaluateConstraints(solution);
						newPopulation.add(solution);
					}
				}
				iterations++;
				
				solutionSet = newPopulation;
				if (evaluations >= maxEvaluations)
				{
					condition = true;
				}
			}
			
			
			CrowdingArchive archive;
			archive = new CrowdingArchive(populationSize, m_problem.NumberOfObjectives);
			for (int i = 0; i < solutionSet.size(); i++)
			{
				archive.add(solutionSet.getSolution(i));
			}
			
			return archive;
		} // execute
	} // MOCHC
}