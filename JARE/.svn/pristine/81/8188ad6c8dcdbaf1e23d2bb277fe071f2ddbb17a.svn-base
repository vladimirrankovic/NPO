/// <summary> ssNSGAII.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0  
/// </version>
using System;
using JARE.Base;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using JARE.util;
namespace JARE.metaheuristics.nsgaII
{
	
	/// <summary> This class implements a steady-state version of NSGA-II.</summary>
	[Serializable]
	public class ssNSGAII:Algorithm
	{
		
		/// <summary> stores the problem  to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public ssNSGAII(Problem problem)
		{
			this.m_problem = problem;
		} // NSGAII
		
		/// <summary> Runs the ssNSGA-II algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
			int maxEvaluations;
			int evaluations;
			
			QualityIndicator indicators; // QualityIndicator object
			int requiredEvaluations; // Use in the example of use of the
			// indicators object (see below)
			
			SolutionSet population;
			SolutionSet offspringPopulation;
			SolutionSet union;
			
			Operator mutationOperator;
			Operator crossoverOperator;
			Operator selectionOperator;
			
			Distance distance = new Distance();
			
			//Read the parameters
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			indicators = (QualityIndicator) getInputParameter("indicators");
			
			//Initialize the variables
			population = new SolutionSet(populationSize);
			evaluations = 0;
			
			requiredEvaluations = 0;
			
			//Read the operators
			mutationOperator = m_operators["mutation"];
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
			// Create the initial solutionSet
			Solution newSolution;
			for (int i = 0; i < populationSize; i++)
			{
				newSolution = new Solution(m_problem);
				m_problem.evaluate(newSolution);
				m_problem.evaluateConstraints(newSolution);
				evaluations++;
				population.add(newSolution);
			} //for       
			
			// Generations ...
			while (evaluations < maxEvaluations)
			{
				
				// Create the offSpring solutionSet      
				offspringPopulation = new SolutionSet(populationSize);
				Solution[] parents = new Solution[2];
				
				//obtain parents
				parents[0] = (Solution) selectionOperator.execute(population);
				parents[1] = (Solution) selectionOperator.execute(population);
				
				// crossover
				Solution[] offSpring = (Solution[]) crossoverOperator.execute(parents);
				
				// mutation
				mutationOperator.execute(offSpring[0]);
				
				// evaluation
				m_problem.evaluate(offSpring[0]);
				m_problem.evaluateConstraints(offSpring[0]);
				
				// insert child into the offspring population
				offspringPopulation.add(offSpring[0]);
				
				evaluations++;
				
				// Create the solutionSet union of solutionSet and offSpring
				union = ((SolutionSet) population).union(offspringPopulation);
				
				// Ranking the union
				Ranking ranking = new Ranking(union);
				
				int remain = populationSize;
				int index = 0;
				SolutionSet front = null;
				population.clear();
				
				// Obtain the next front
				front = ranking.getSubfront(index);
				
				while ((remain > 0) && (remain >= front.size()))
				{
					//Assign crowding distance to individuals
					distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
					//Add the individuals of this front
					for (int k = 0; k < front.size(); k++)
					{
						population.add(front.getSolution(k));
					} // for
					
					//Decrement remain
					remain = remain - front.size();
					
					//Obtain the next front
					index++;
					if (remain > 0)
					{
						front = ranking.getSubfront(index);
					} // if        
				} // while
				
				// Remain is less than front(index).size, insert only the best one
				if (remain > 0)
				{
					// front contains individuals to insert                        
					distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
					front.sort(new JARE.Base.operators.comparator.CrowdingComparator());
					for (int k = 0; k < remain; k++)
					{
						population.add(front.getSolution(k));
					} // for
					
					remain = 0;
				} // if                               
				
				// This piece of code shows how to use the indicator object into the code
				// of NSGA-II. In particular, it finds the number of evaluations required
				// by the algorithm to obtain a Pareto front with a hypervolume higher
				// than the hypervolume of the true Pareto front.
				if ((indicators != null) && (requiredEvaluations == 0))
				{
					double HV = indicators.getHypervolume(population);
					if (HV >= (0.98 * indicators.TrueParetoFrontHypervolume))
					{
						requiredEvaluations = evaluations;
					} // if
				} // if
			} // while
			
			// Return as output parameter the required evaluations
			setOutputParameter("evaluations", requiredEvaluations);
			
			// Return the first non-dominated front
			Ranking ranking2 = new Ranking(population);
			return ranking2.getSubfront(0);
		} // execute
	} // NSGA-II
}