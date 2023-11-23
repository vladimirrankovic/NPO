/// <summary> AbYSS.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using CrowdingArchive = JARE.util.archive.CrowdingArchive;
using Algorithm = JARE.Base.Algorithm;
using CrowdingDistanceComparator = JARE.Base.operators.comparator.CrowdingDistanceComparator;
using JARE.util;
using LocalSearch = JARE.Base.operators.localSearch.LocalSearch;
namespace JARE.metaheuristics.abyss
{
	
	/// <summary> This class implements the AbYSS algorithm. This algorithm is an adaptation
	/// of the single-objective scatter search template defined by F. Glover in:
	/// F. Glover. "A template for scatter search and path relinking", Lecture Notes 
	/// in Computer Science, Springer Verlag, 1997.
	/// </summary>
	[Serializable]
	public class AbYSS:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Stores the number of subranges in which each variable is divided. Used in
		/// the diversification method. By default it takes the value 4 (see the method
		/// <code>initParams</code>).
		/// </summary>
		internal int m_numberOfSubranges;
		
		/// <summary> These variables are used in the diversification method.</summary>
		internal int[] m_sumOfFrequencyValues;
		internal int[] m_sumOfReverseFrequencyValues;
		internal int[][] m_frequency;
		internal int[][] m_reverseFrequency;
		
		/// <summary> Stores the initial solution set</summary>
		private SolutionSet m_solutionSet;
		
		/// <summary> Stores the external solution archive</summary>
		private CrowdingArchive m_archive;
		
		/// <summary> Stores the reference set one</summary>
		private SolutionSet m_refSet1;
		
		/// <summary> Stores the reference set two</summary>
		private SolutionSet m_refSet2;
		
		/// <summary> Stores the solutions provided by the subset generation method of the
		/// scatter search template
		/// </summary>
		private SolutionSet m_subSet;
		
		/// <summary> Maximum number of solution allowed for the initial solution set</summary>
		private int m_solutionSetSize;
		
		/// <summary> Maximum size of the external archive</summary>
		private int m_archiveSize;
		
		/// <summary> Maximum size of the reference set one</summary>
		private int m_refSet1Size;
		
		/// <summary> Maximum size of the reference set two</summary>
		private int m_refSet2Size;
		
		/// <summary> Maximum number of getEvaluations to carry out</summary>
		private int maxEvaluations;
		
		/// <summary> Stores the current number of performed getEvaluations</summary>
		private int m_evaluations;
		
		/// <summary> Stores the comparators for dominance and equality, respectively</summary>
		private System.Collections.IComparer m_dominance;
		private System.Collections.IComparer m_equal;
        System.Collections.Generic.IComparer<JARE.Base.Solution> m_fitness;
        System.Collections.Generic.IComparer<JARE.Base.Solution> m_crowdingDistance;
		
		/// <summary> Stores the crossover operator</summary>
		private Operator m_crossoverOperator;
		
		/// <summary> Stores the improvement operator</summary>
		private LocalSearch m_improvementOperator;
		
		/// <summary> Stores a <code>Distance</code> object</summary>
		private Distance m_distance;
		
		/// <summary> Constructor.</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public AbYSS(Problem problem)
		{
			//Initialize the fields 
			m_problem = problem;
			
			m_solutionSet = null;
			m_archive = null;
			m_refSet1 = null;
			m_refSet2 = null;
			m_subSet = null;
		} // AbYSS
		
		/// <summary> Reads the parameter from the parameter list using the
		/// <code>getInputParameter</code> method.
		/// </summary>
		public virtual void  initParam()
		{
			//Read the parameters
			m_solutionSetSize = ((System.Int32) getInputParameter("populationSize"));
			m_refSet1Size = ((System.Int32) getInputParameter("refSet1Size"));
			m_refSet2Size = ((System.Int32) getInputParameter("refSet2Size"));
			m_archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			//Initialize the variables
			m_solutionSet = new SolutionSet(m_solutionSetSize);
			m_archive = new CrowdingArchive(m_archiveSize, m_problem.NumberOfObjectives);
			m_refSet1 = new SolutionSet(m_refSet1Size);
			m_refSet2 = new SolutionSet(m_refSet2Size);
			m_subSet = new SolutionSet(m_solutionSetSize * 1000);
			m_evaluations = 0;
			
			m_numberOfSubranges = 4;
			
			m_dominance = new JARE.Base.operators.comparator.DominanceComparator();
            m_equal = new JARE.Base.operators.comparator.EqualSolutions();
            m_fitness = new JARE.Base.operators.comparator.FitnessComparator();
            m_crowdingDistance = new JARE.Base.operators.comparator.CrowdingDistanceComparator();
			m_distance = new Distance();
			m_sumOfFrequencyValues = new int[m_problem.NumberOfVariables];
			m_sumOfReverseFrequencyValues = new int[m_problem.NumberOfVariables];
			m_frequency = new int[m_numberOfSubranges][];
			for (int i = 0; i < m_numberOfSubranges; i++)
			{
				m_frequency[i] = new int[m_problem.NumberOfVariables];
			}
			m_reverseFrequency = new int[m_numberOfSubranges][];
			for (int i2 = 0; i2 < m_numberOfSubranges; i2++)
			{
				m_reverseFrequency[i2] = new int[m_problem.NumberOfVariables];
			}
			
			//Read the operators of crossover and improvement
			m_crossoverOperator = m_operators["crossover"];
			m_improvementOperator = (LocalSearch) m_operators["improvement"];
			m_improvementOperator.setParameter("archive", m_archive);
		} // initParam
		
		/// <summary> Returns a <code>Solution</code> using the diversification generation method
		/// described in the scatter search template.
		/// </summary>
		/// <throws>  SMException  </throws>
		/// <throws>  ClassNotFoundException  </throws>
		public virtual Solution diversificationGeneration()
		{
			Solution solution;
			solution = new Solution(m_problem);
			
			double value;
			int range;
			
			for (int i = 0; i < m_problem.NumberOfVariables; i++)
			{
				m_sumOfReverseFrequencyValues[i] = 0;
				for (int j = 0; j < m_numberOfSubranges; j++)
				{
					m_reverseFrequency[j][i] = m_sumOfFrequencyValues[i] - m_frequency[j][i];
					m_sumOfReverseFrequencyValues[i] += m_reverseFrequency[j][i];
				} // for
				
				if (m_sumOfReverseFrequencyValues[i] == 0)
				{
					range = PseudoRandom.randInt(0, m_numberOfSubranges - 1);
				}
				else
				{
					value = PseudoRandom.randInt(0, m_sumOfReverseFrequencyValues[i] - 1);
					range = 0;
					while (value > m_reverseFrequency[range][i])
					{
						value -= m_reverseFrequency[range][i];
						range++;
					} // while
				} // else            
				
				m_frequency[range][i]++;
				m_sumOfFrequencyValues[i]++;
				
				double low = m_problem.getLowerLimit(i) + range * (m_problem.getUpperLimit(i) - m_problem.getLowerLimit(i)) / m_numberOfSubranges;
				double high = low + (m_problem.getUpperLimit(i) - m_problem.getLowerLimit(i)) / m_numberOfSubranges;
				value = PseudoRandom.randDouble(low, high);
				solution.DecisionVariables[i].setValue(value);
			} // for       
			return solution;
		} // diversificationGeneration
		
		
		/// <summary> Implements the referenceSetUpdate method.</summary>
		/// <param name="build">if true, indicates that the reference has to be build for the
		/// first time; if false, indicates that the reference set has to be
		/// updated with new solutions
		/// </param>
		/// <throws>  SMException  </throws>
		public virtual void  referenceSetUpdate(bool build)
		{
			if (build)
			{
				// Build a new reference set
				// STEP 1. Select the p best individuals of P, where p is m_refSet1Size. 
				//         Selection Criterium: Spea2Fitness
				Solution individual;
				(new Spea2Fitness(m_solutionSet)).fitnessAssign();
				m_solutionSet.sort(m_fitness);
				
				// STEP 2. Build the RefSet1 with these p individuals            
				for (int i = 0; i < m_refSet1Size; i++)
				{
					individual = m_solutionSet.getSolution(0);
					m_solutionSet.remove(0);
					individual.unMarked();
					m_refSet1.add(individual);
				}
				
				// STEP 3. Compute Euclidean distances in SolutionSet to obtain q 
				//         individuals, where q is m_refSet2Size
				for (int i = 0; i < m_solutionSet.size(); i++)
				{
					individual = m_solutionSet.getSolution(i);
					individual.DistanceToSolutionSet = m_distance.distanceToSolutionSetInSolutionSpace(individual, m_refSet1);
				}
				
				int size = m_refSet2Size;
				if (m_solutionSet.size() < m_refSet2Size)
				{
					size = m_solutionSet.size();
				}
				
				// STEP 4. Build the RefSet2 with these q individuals
				for (int i = 0; i < size; i++)
				{
					// Find the maximumMinimunDistanceToPopulation
					double maxMinimum = 0.0;
					int index = 0;
					for (int j = 0; j < m_solutionSet.size(); j++)
					{
						if (m_solutionSet.getSolution(j).DistanceToSolutionSet > maxMinimum)
						{
							maxMinimum = m_solutionSet.getSolution(j).DistanceToSolutionSet;
							index = j;
						}
					}
					individual = m_solutionSet.getSolution(index);
					m_solutionSet.remove(index);
					
					// Update distances to REFSET in population
					for (int j = 0; j < m_solutionSet.size(); j++)
					{
						double aux = m_distance.distanceBetweenSolutions(m_solutionSet.getSolution(j), individual);
						if (aux < individual.DistanceToSolutionSet)
						{
							m_solutionSet.getSolution(j).DistanceToSolutionSet = aux;
						}
					}
					
					// Insert the individual into REFSET2
					m_refSet2.add(individual);
					
					// Update distances in REFSET2
					for (int j = 0; j < m_refSet2.size(); j++)
					{
						for (int k = 0; k < m_refSet2.size(); k++)
						{
							if (i != j)
							{
								double aux = m_distance.distanceBetweenSolutions(m_refSet2.getSolution(j), m_refSet2.getSolution(k));
								if (aux < m_refSet2.getSolution(j).DistanceToSolutionSet)
								{
									m_refSet2.getSolution(j).DistanceToSolutionSet = aux;
								} // if
							} // if
						} // for
					} // for   
				} // for                       
			}
			else
			{
				// Update the reference set from the subset generation result
				Solution individual;
				for (int i = 0; i < m_subSet.size(); i++)
				{
					individual = (Solution) m_improvementOperator.execute(m_subSet.getSolution(i));
					m_evaluations += m_improvementOperator.Evaluations;
					
					if (refSet1Test(individual))
					{
						//Update distance of RefSet2
						for (int indSet2 = 0; indSet2 < m_refSet2.size(); indSet2++)
						{
							double aux = m_distance.distanceBetweenSolutions(individual, m_refSet2.getSolution(indSet2));
							if (aux < m_refSet2.getSolution(indSet2).DistanceToSolutionSet)
							{
								m_refSet2.getSolution(indSet2).DistanceToSolutionSet = aux;
							} // if
						} // for                    
					}
					else
					{
						refSet2Test(individual);
					} // if 
				}
				m_subSet.clear();
			}
		} // referenceSetUpdate
		
		/// <summary> Tries to update the reference set 2 with a <code>Solution</code></summary>
		/// <param name="solution">The <code>Solution</code>
		/// </param>
		/// <returns> true if the <code>Solution</code> has been inserted, false 
		/// otherwise.
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual bool refSet2Test(Solution solution)
		{
			
			if (m_refSet2.size() < m_refSet2Size)
			{
				solution.DistanceToSolutionSet = m_distance.distanceToSolutionSetInSolutionSpace(solution, m_refSet1);
				double aux = m_distance.distanceToSolutionSetInSolutionSpace(solution, m_refSet2);
				if (aux < solution.DistanceToSolutionSet)
				{
					solution.DistanceToSolutionSet = aux;
				}
				m_refSet2.add(solution);
				return true;
			}
			
			solution.DistanceToSolutionSet = m_distance.distanceToSolutionSetInSolutionSpace(solution, m_refSet1);
			double aux2 = m_distance.distanceToSolutionSetInSolutionSpace(solution, m_refSet2);
			if (aux2 < solution.DistanceToSolutionSet)
			{
				solution.DistanceToSolutionSet = aux2;
			}
			
			double peor = 0.0;
			int index = 0;
			for (int i = 0; i < m_refSet2.size(); i++)
			{
				aux2 = m_refSet2.getSolution(i).DistanceToSolutionSet;
				if (aux2 > peor)
				{
					peor = aux2;
					index = i;
				}
			}
			
			if (solution.DistanceToSolutionSet < peor)
			{
				m_refSet2.remove(index);
				//Update distances in REFSET2
				for (int j = 0; j < m_refSet2.size(); j++)
				{
					aux2 = m_distance.distanceBetweenSolutions(m_refSet2.getSolution(j), solution);
					if (aux2 < m_refSet2.getSolution(j).DistanceToSolutionSet)
					{
						m_refSet2.getSolution(j).DistanceToSolutionSet = aux2;
					}
				}
				solution.unMarked();
				m_refSet2.add(solution);
				return true;
			}
			return false;
		} // refSet2Test
		
		/// <summary> Tries to update the reference set one with a <code>Solution</code>.</summary>
		/// <param name="solution">The <code>Solution</code>
		/// </param>
		/// <returns> true if the <code>Solution</code> has been inserted, false
		/// otherwise.
		/// </returns>
		public virtual bool refSet1Test(Solution solution)
		{
			bool dominated = false;
			int flag;
			int i = 0;
			while (i < m_refSet1.size())
			{
				flag = m_dominance.Compare(solution, m_refSet1.getSolution(i));
				if (flag == - 1)
				{
					//This is: solution dominates 
					m_refSet1.remove(i);
				}
				else if (flag == 1)
				{
					dominated = true;
					i++;
				}
				else
				{
					flag = m_equal.Compare(solution, m_refSet1.getSolution(i));
					if (flag == 0)
					{
						return true;
					} // if
					i++;
				} // if 
			} // while
			
			if (!dominated)
			{
				solution.unMarked();
				if (m_refSet1.size() < m_refSet1Size)
				{
					//refSet1 isn't full
					m_refSet1.add(solution);
				}
				else
				{
					m_archive.add(solution);
				} // if
			}
			else
			{
				return false;
			} // if
			return true;
		} // refSet1Test
		
		/// <summary> Implements the subset generation method described in the scatter search
		/// template
		/// </summary>
		/// <returns>  Number of solutions created by the method
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual int subSetGeneration()
		{
			Solution[] parents = new Solution[2];
			Solution[] offSpring;
			
			m_subSet.clear();
			
			//All pairs from refSet1
			for (int i = 0; i < m_refSet1.size(); i++)
			{
				parents[0] = m_refSet1.getSolution(i);
				for (int j = i + 1; j < m_refSet1.size(); j++)
				{
					parents[1] = m_refSet1.getSolution(j);
					if (!parents[0].Marked || !parents[1].Marked)
					{
						//offSpring = parent1.crossover(1.0,parent2);
						offSpring = (Solution[]) m_crossoverOperator.execute(parents);
						m_problem.evaluate(offSpring[0]);
						m_problem.evaluate(offSpring[1]);
						m_problem.evaluateConstraints(offSpring[0]);
						m_problem.evaluateConstraints(offSpring[1]);
						m_evaluations += 2;
						if (m_evaluations < maxEvaluations)
						{
							m_subSet.add(offSpring[0]);
							m_subSet.add(offSpring[1]);
						}
						parents[0].Mark();
						parents[1].Mark();
					}
				}
			}
			
			// All pairs from refSet2
			for (int i = 0; i < m_refSet2.size(); i++)
			{
				parents[0] = m_refSet2.getSolution(i);
				for (int j = i + 1; j < m_refSet2.size(); j++)
				{
					parents[1] = m_refSet2.getSolution(j);
					if (!parents[0].Marked || !parents[1].Marked)
					{
						//offSpring = parents[0].crossover(1.0,parent2);                    
						offSpring = (Solution[]) m_crossoverOperator.execute(parents);
						m_problem.evaluateConstraints(offSpring[0]);
						m_problem.evaluateConstraints(offSpring[1]);
						m_problem.evaluate(offSpring[0]);
						m_problem.evaluate(offSpring[1]);
						m_evaluations += 2;
						if (m_evaluations < maxEvaluations)
						{
							m_subSet.add(offSpring[0]);
							m_subSet.add(offSpring[1]);
						}
						parents[0].Mark();
						parents[1].Mark();
					}
				}
			}
			
			return m_subSet.size();
		} // subSetGeneration
		
		/// <summary> Runs of the AbYSS algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			// STEP 1. Initialize parameters
			initParam();
			
			// STEP 2. Build the initial solutionSet
			Solution solution;
			for (int i = 0; i < m_solutionSetSize; i++)
			{
				solution = diversificationGeneration();
				m_problem.evaluateConstraints(solution);
				m_problem.evaluate(solution);
				m_evaluations++;
				solution = (Solution) m_improvementOperator.execute(solution);
				m_evaluations += m_improvementOperator.Evaluations;
				m_solutionSet.add(solution);
			} // fpr
			
			// STEP 3. Main loop
			int newSolutions = 0;
			while (m_evaluations < maxEvaluations)
			{
				referenceSetUpdate(true);
				newSolutions = subSetGeneration();
				while (newSolutions > 0)
				{
					// New solutions are created           
					referenceSetUpdate(false);
					if (m_evaluations >= maxEvaluations)
						return m_archive;
					newSolutions = subSetGeneration();
				} // while
				
				// RE-START
				if (m_evaluations < maxEvaluations)
				{
					m_solutionSet.clear();
					// Add refSet1 to SolutionSet
					for (int i = 0; i < m_refSet1.size(); i++)
					{
						solution = m_refSet1.getSolution(i);
						solution.unMarked();
						solution = (Solution) m_improvementOperator.execute(solution);
						m_evaluations += m_improvementOperator.Evaluations;
						m_solutionSet.add(solution);
					}
					// Remove refSet1 and refSet2
					m_refSet1.clear();
					m_refSet2.clear();
					
					// Sort the archive and insert the best solutions
					m_distance.crowdingDistanceAssignment(m_archive, m_problem.NumberOfObjectives);
					m_archive.sort(m_crowdingDistance);
					
					int insert = m_solutionSetSize / 2;
					if (insert > m_archive.size())
						insert = m_archive.size();
					
					if (insert > (m_solutionSetSize - m_solutionSet.size()))
						insert = m_solutionSetSize - m_solutionSet.size();
					
					// Insert solutions 
					for (int i = 0; i < insert; i++)
					{
						solution = new Solution(m_archive.getSolution(i));
						//solution = improvement(solution);
						solution.unMarked();
						m_solutionSet.add(solution);
					}
					
					// Create the rest of solutions randomly
					while (m_solutionSet.size() < m_solutionSetSize)
					{
						solution = diversificationGeneration();
						m_problem.evaluateConstraints(solution);
						m_problem.evaluate(solution);
						m_evaluations++;
						solution = (Solution) m_improvementOperator.execute(solution);
						m_evaluations += m_improvementOperator.Evaluations;
						solution.unMarked();
						m_solutionSet.add(solution);
					} // while
				} // if   
			} // while       
			
			// STEP 4. Return the archive
			return m_archive;
		} // execute
	} // AbYSS
}