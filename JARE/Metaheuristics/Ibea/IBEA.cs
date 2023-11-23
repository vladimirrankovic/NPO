/// <summary> IBEA.java
/// 
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using DominanceComparator = JARE.Base.operators.comparator.DominanceComparator;
using FitnessComparator = JARE.Base.operators.comparator.FitnessComparator;
using Epsilon = JARE.qualityIndicator.Epsilon;
using Hypervolume = JARE.qualityIndicator.Hypervolume;
using JARE.util;
namespace JARE.metaheuristics.ibea
{
	
	/// <summary> This class representing the SPEA2 algorithm</summary>
	[Serializable]
	public class IBEA:Algorithm
	{
		
		/// <summary> Defines the number of tournaments for creating the mating pool</summary>
		public const int TOURNAMENTS_ROUNDS = 1;
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Stores the value of the indicator between each pair of solutions into
		/// the solution set
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        private System.Collections.Generic.List<System.Collections.Generic.List<double>> m_indicatorValues;
		
		/// <summary> </summary>
		private double m_maxIndicatorValue;
		/// <summary> Constructor.
		/// Create a new IBEA instance
		/// </summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public IBEA(Problem problem)
		{
			this.m_problem = problem;
		} // Spea2
		
		/// <summary> calculates the hypervolume of that portion of the objective space that
		/// is dominated by individual a but not by individual b
		/// </summary>
		internal virtual double calcHypervolumeIndicator(Solution p_ind_a, Solution p_ind_b, int d, double[] maximumValues, double[] minimumValues)
		{
			double a, b, r, max;
			double volume = 0;
			double rho = 2.0;
			
			r = rho * (maximumValues[d - 1] - minimumValues[d - 1]);
			max = minimumValues[d - 1] + r;
			
			
			a = p_ind_a.getObjective(d - 1);
			if (p_ind_b == null)
				b = max;
			else
				b = p_ind_b.getObjective(d - 1);
			
			if (d == 1)
			{
				if (a < b)
					volume = (b - a) / r;
				else
					volume = 0;
			}
			else
			{
				if (a < b)
				{
					volume = calcHypervolumeIndicator(p_ind_a, null, d - 1, maximumValues, minimumValues) * (b - a) / r;
					volume += calcHypervolumeIndicator(p_ind_a, p_ind_b, d - 1, maximumValues, minimumValues) * (max - b) / r;
				}
				else
				{
					volume = calcHypervolumeIndicator(p_ind_a, p_ind_b, d - 1, maximumValues, minimumValues) * (max - b) / r;
				}
			}
			
			return (volume);
		}
		
		
		
		/// <summary> This structure store the indicator values of each pair of elements</summary>
		public virtual void  computeIndicatorValuesHD(SolutionSet solutionSet, double[] maximumValues, double[] minimumValues)
		{
			SolutionSet A, B;
			// Initialize the structures
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			m_indicatorValues = new System.Collections.Generic.List<System.Collections.Generic.List<double>>();
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			m_maxIndicatorValue = - System.Double.MaxValue;
			
			for (int j = 0; j < solutionSet.size(); j++)
			{
				A = new SolutionSet(1);
				A.add(solutionSet.getSolution(j));
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				System.Collections.Generic.List<double> aux = new System.Collections.Generic.List<double>();
				for (int i = 0; i < solutionSet.size(); i++)
				{
					B = new SolutionSet(1);
					B.add(solutionSet.getSolution(i));
					
					int flag = (new DominanceComparator()).Compare(A.getSolution(0), B.getSolution(0));
					
					double value = 0.0;
					if (flag == - 1)
					{
						value = - calcHypervolumeIndicator(A.getSolution(0), B.getSolution(0), m_problem.NumberOfObjectives, maximumValues, minimumValues);
					}
					else
					{
						value = calcHypervolumeIndicator(B.getSolution(0), A.getSolution(0), m_problem.NumberOfObjectives, maximumValues, minimumValues);
					}
					//double value = epsilon.epsilon(matrixA,matrixB,m_problem.getNumberOfObjectives());
					
					
					//Update the max value of the indicator
					if (System.Math.Abs(value) > m_maxIndicatorValue)
						m_maxIndicatorValue = System.Math.Abs(value);
					aux.Add(value);
				}
				m_indicatorValues.Add(aux);
			}
		} // computeIndicatorValues
		
		
		
		/// <summary> Calculate the fitness for the individual at position pos</summary>
		public virtual void  fitness(SolutionSet solutionSet, int pos)
		{
			double fitness = 0.0;
			double kappa = 0.05;
			
			for (int i = 0; i < solutionSet.size(); i++)
			{
				if (i != pos)
				{
					fitness += System.Math.Exp(((- 1) * m_indicatorValues[i][pos] / m_maxIndicatorValue) / kappa);
				}
			}
			solutionSet.getSolution(pos).Fitness = fitness;
		}
		
		
		/// <summary> Calculate the fitness for the entire population.
		/// 
		/// </summary>
		public virtual void  calculateFitness(SolutionSet solutionSet)
		{
			// Obtains the lower and upper bounds of the population
			double[] maximumValues = new double[m_problem.NumberOfObjectives];
			double[] minimumValues = new double[m_problem.NumberOfObjectives];
			
			for (int i = 0; i < m_problem.NumberOfObjectives; i++)
			{
				//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				maximumValues[i] = - System.Double.MaxValue; // i.e., the minus maxium value
				//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				minimumValues[i] = System.Double.MaxValue; // i.e., the maximum value
			}
			
			for (int pos = 0; pos < solutionSet.size(); pos++)
			{
				for (int obj = 0; obj < m_problem.NumberOfObjectives; obj++)
				{
					double value = solutionSet.getSolution(pos).getObjective(obj);
					if (value > maximumValues[obj])
						maximumValues[obj] = value;
					if (value < minimumValues[obj])
						minimumValues[obj] = value;
				}
			}
			
			computeIndicatorValuesHD(solutionSet, maximumValues, minimumValues);
			for (int pos = 0; pos < solutionSet.size(); pos++)
			{
				fitness(solutionSet, pos);
			}
		}
		
		
		
		/// <summary> Update the fitness before removing an individual</summary>
		public virtual void  removeWorst(SolutionSet solutionSet)
		{
			
			// Find the worst;
			double worst = solutionSet.getSolution(0).Fitness;
			int worstIndex = 0;
			double kappa = 0.05;
			
			for (int i = 1; i < solutionSet.size(); i++)
			{
				if (solutionSet.getSolution(i).Fitness > worst)
				{
					worst = solutionSet.getSolution(i).Fitness;
					worstIndex = i;
				}
			}
			
			//if (worstIndex == -1) {
			//    System.out.println("Yes " + worst);
			//}
			//System.out.println("Solution Size "+solutionSet.size());
			//System.out.println(worstIndex);
			
			// Update the population
			for (int i = 0; i < solutionSet.size(); i++)
			{
				if (i != worstIndex)
				{
					double fitness = solutionSet.getSolution(i).Fitness;
					fitness -= Math.Exp(((- m_indicatorValues[worstIndex][i]) / m_maxIndicatorValue) / kappa);
					solutionSet.getSolution(i).Fitness = fitness;
				}
			}
			
			// remove worst from the indicatorValues list
			m_indicatorValues.RemoveAt(worstIndex); // Remove its own list
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
            //Iterator<List<Double>> it = m_indicatorValues.iterator();
            //while (it.hasNext()) it.next().remove(worstIndex);
            //VLADA - CONVERT NAPOMENA: Rucno preveden kod ispod nije jasan!!!
            System.Collections.Generic.List<System.Collections.Generic.List<double>>.Enumerator it = m_indicatorValues.GetEnumerator();
            while (it.MoveNext()) it.Current.RemoveAt(worstIndex);

			
			// remove the worst individual from the population
			solutionSet.remove(worstIndex);
		} // removeWorst
		
		
		/// <summary> Runs of the IBEA algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution
		/// </returns>
		/// <throws>  SMException </throws>
		public override SolutionSet execute()
		{
			int populationSize, archiveSize, maxEvaluations, evaluations;
			Operator crossoverOperator, mutationOperator, selectionOperator;
			SolutionSet solutionSet, archive, offSpringSolutionSet;
			
			//Read the params
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			//Read the operators
			crossoverOperator = m_operators["crossover"];
			mutationOperator = m_operators["mutation"];
			selectionOperator = m_operators["selection"];
			
			//Initialize the variables
			solutionSet = new SolutionSet(populationSize);
			archive = new SolutionSet(archiveSize);
			evaluations = 0;
			
			//-> Create the initial solutionSet
			Solution newSolution;
			for (int i = 0; i < populationSize; i++)
			{
				newSolution = new Solution(m_problem);
				m_problem.evaluate(newSolution);
				m_problem.evaluateConstraints(newSolution);
				evaluations++;
				solutionSet.add(newSolution);
			}
			
			while (evaluations < maxEvaluations)
			{
				SolutionSet union = ((SolutionSet) solutionSet).union(archive);
				calculateFitness(union);
				archive = union;
				
				while (archive.size() > populationSize)
				{
					removeWorst(archive);
				}
				// Create a new offspringPopulation
				offSpringSolutionSet = new SolutionSet(populationSize);
				Solution[] parents = new Solution[2];
				while (offSpringSolutionSet.size() < populationSize)
				{
					int j = 0;
					do 
					{
						j++;
						parents[0] = (Solution) selectionOperator.execute(archive);
					}
					while (j < IBEA.TOURNAMENTS_ROUNDS); // do-while
					int k = 0;
					do 
					{
						k++;
						parents[1] = (Solution) selectionOperator.execute(archive);
					}
					while (k < IBEA.TOURNAMENTS_ROUNDS); // do-while
					
					//make the crossover
					Solution[] offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					offSpringSolutionSet.add(offSpring[0]);
					evaluations++;
				} // while
				// End Create a offSpring solutionSet
				solutionSet = offSpringSolutionSet;
			} // while
			
			Ranking ranking = new Ranking(archive);
			return ranking.getSubfront(0);
		} // execute
	} // Spea2
}