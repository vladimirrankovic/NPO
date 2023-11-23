/// <summary> FastPGA.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using FPGAFitnessComparator = JARE.Base.operators.comparator.FPGAFitnessComparator;
using JARE.util;
namespace JARE.metaheuristics.fastPGA
{
	
	/*
	* This class implements the FPGA (Fast Pareto Genetic Algorithm).*/
	[Serializable]
	public class FastPGA:Algorithm
	{
		
		internal Problem m_problem;
		
		/// <summary> Constructor
		/// Creates a new instance of FastPGA
		/// </summary>
		public FastPGA(Problem problem)
		{
			m_problem = problem;
		} // FastPGA
		
		/// <summary> Runs of the FastPGA algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int maxPopSize, populationSize, offSpringSize, evaluations, maxEvaluations, initialPopulationSize;
			SolutionSet solutionSet, offSpringSolutionSet, candidateSolutionSet = null;
			double a, b, c, d;
			Operator crossover, mutation, selection;
			int termination;
			Distance distance = new Distance();
            System.Collections.Generic.IComparer<JARE.Base.Solution> fpgaFitnessComparator = new FPGAFitnessComparator();
			
			//Read the parameters
			maxPopSize = ((System.Int32) getInputParameter("maxPopSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			initialPopulationSize = ((System.Int32) getInputParameter("initialPopulationSize"));
			termination = ((System.Int32) getInputParameter("termination"));
			
			//Read the operators
			crossover = (Operator) m_operators["crossover"];
			mutation = (Operator) m_operators["mutation"];
			selection = (Operator) m_operators["selection"];
			
			//Read the params
			a = ((System.Double) getInputParameter("a"));
			b = ((System.Double) getInputParameter("b"));
			c = ((System.Double) getInputParameter("c"));
			d = ((System.Double) getInputParameter("d"));
			
			//Initialize populationSize and offSpringSize
			evaluations = 0;
			populationSize = initialPopulationSize;
			offSpringSize = maxPopSize;
			
			//Build a solution set randomly
			solutionSet = new SolutionSet(populationSize);
			for (int i = 0; i < populationSize; i++)
			{
				Solution solution = new Solution(m_problem);
				m_problem.evaluate(solution);
				m_problem.evaluateConstraints(solution);
				evaluations++;
				solutionSet.add(solution);
			}
			
			//Begin the iterations
			Solution[] parents = new Solution[2];
			Solution[] offSprings;
			bool stop = false;
			int reachesMaxNonDominated = 0;
			while (!stop)
			{
				
				// Create the candidate solutionSet
				offSpringSolutionSet = new SolutionSet(offSpringSize);
				for (int i = 0; i < offSpringSize / 2; i++)
				{
					parents[0] = (Solution) selection.execute(solutionSet);
					parents[1] = (Solution) selection.execute(solutionSet);
					offSprings = (Solution[]) crossover.execute(parents);
					mutation.execute(offSprings[0]);
					mutation.execute(offSprings[1]);
					m_problem.evaluate(offSprings[0]);
					m_problem.evaluateConstraints(offSprings[0]);
					evaluations++;
					m_problem.evaluate(offSprings[1]);
					m_problem.evaluateConstraints(offSprings[1]);
					evaluations++;
					offSpringSolutionSet.add(offSprings[0]);
					offSpringSolutionSet.add(offSprings[1]);
				}
				
				// Merge the populations
				candidateSolutionSet = solutionSet.union(offSpringSolutionSet);
				
				// Rank
				Ranking ranking = new Ranking(candidateSolutionSet);
				distance.crowdingDistanceAssignment(ranking.getSubfront(0), m_problem.NumberOfObjectives);
				FPGAFitness fitness = new FPGAFitness(candidateSolutionSet, m_problem);
				fitness.fitnessAssign();
				
				// Count the non-dominated solutions in candidateSolutionSet      
				int count = ranking.getSubfront(0).size();
				
				//Regulate
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				populationSize = (int) System.Math.Min(a + System.Math.Floor(b * count), maxPopSize);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				offSpringSize = (int) System.Math.Min(c + System.Math.Floor(d * count), maxPopSize);
				
				candidateSolutionSet.sort(fpgaFitnessComparator);
				solutionSet = new SolutionSet(populationSize);
				
				for (int i = 0; i < populationSize; i++)
				{
					solutionSet.add(candidateSolutionSet.getSolution(i));
				}
				
				//Termination test
				if (termination == 0)
				{
					ranking = new Ranking(solutionSet);
					count = ranking.getSubfront(0).size();
					if (count == maxPopSize)
					{
						if (reachesMaxNonDominated == 0)
						{
							reachesMaxNonDominated = evaluations;
						}
						if (evaluations - reachesMaxNonDominated >= maxEvaluations)
						{
							stop = true;
						}
					}
					else
					{
						reachesMaxNonDominated = 0;
					}
				}
				else
				{
					if (evaluations >= maxEvaluations)
					{
						stop = true;
					}
				}
			}
			
			setOutputParameter("evaluations", evaluations);
			
			Ranking ranking2 = new Ranking(solutionSet);
			return ranking2.getSubfront(0);
		} // execute
	} // FastPGA
}