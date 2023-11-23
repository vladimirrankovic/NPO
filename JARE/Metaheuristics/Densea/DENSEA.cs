/// <summary> DENSEA.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using JARE.util;
namespace JARE.metaheuristics.densea
{
	
	
	[Serializable]
	public class DENSEA:Algorithm
	{
		
		private Problem m_problem;
		
		/* Create a new instance of DENSEA algorithm */
		public DENSEA(Problem problem)
		{
			m_problem = problem;
		}
		
		//Implements the Densea delete duplicate elements
		public virtual void  deleteDuplicates(SolutionSet population)
		{
			System.Collections.IComparer equalIndividuals = new EqualSolutions();
			for (int i = 0; i < population.size() / 2; i++)
			{
				for (int j = i + 1; j < population.size() / 2; j++)
				{
					int flag = equalIndividuals.Compare(population.getSolution(i), population.getSolution(j));
					if (flag == 0)
					{
						Solution aux = population.getSolution(j);
						population.replace(j, population.getSolution((population.size() / 2) + j));
						population.replace((population.size() / 2) + j, aux);
					}
				}
			}
		}
		
		/* Execute the algorithm */
		public override SolutionSet execute()
		{
			int populationSize, maxEvaluations, evaluations;
			SolutionSet population, offspringPopulation, union;
			Operator mutationOperator, crossoverOperator, selectionOperator;
			Distance distance = new Distance();
			
			//Read the params
			populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			//Init the variables
			population = new SolutionSet(populationSize);
			evaluations = 0;
			
			//Read the operators
			mutationOperator = this.m_operators["mutation"];
			crossoverOperator = this.m_operators["crossover"];
			selectionOperator = this.m_operators["selection"];
			
			//-> Create the initial population
			Solution newIndividual;
			for (int i = 0; i < populationSize; i++)
			{
				newIndividual = new Solution(m_problem);
				m_problem.evaluate(newIndividual);
				m_problem.evaluateConstraints(newIndividual);
				evaluations++;
				population.add(newIndividual);
			} //for       
			//<-
			
			Ranking r;
			
			while (evaluations < maxEvaluations)
			{
				SolutionSet P3 = new SolutionSet(populationSize);
				for (int i = 0; i < populationSize / 2; i++)
				{
					Solution[] parents = new Solution[2];
					Solution[] offSpring;
					parents[0] = (Solution) selectionOperator.execute(population);
					parents[1] = (Solution) selectionOperator.execute(population);
					offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					evaluations++;
					mutationOperator.execute(offSpring[1]);
					m_problem.evaluate(offSpring[1]);
					m_problem.evaluateConstraints(offSpring[1]);
					evaluations++;
					P3.add(offSpring[0]);
					P3.add(offSpring[1]);
				}
				
				r = new Ranking(P3);
				for (int i = 0; i < r.getNumberOfSubfronts(); i++)
				{
					distance.crowdingDistanceAssignment(r.getSubfront(i), m_problem.NumberOfObjectives);
				}
				P3.sort(new CrowdingComparator());
				
				
				population.sort(new CrowdingComparator());
				//deleteDuplicates(population);
				//deleteDuplicates(P3);
				SolutionSet auxiliar = new SolutionSet(populationSize);
				for (int i = 0; i < (populationSize / 2); i++)
				{
					auxiliar.add(population.getSolution(i));
				}
				
				for (int j = 0; j < (populationSize / 2); j++)
				{
					auxiliar.add(population.getSolution(j));
				}
				
				population = auxiliar;
				
				r = new Ranking(population);
				for (int i = 0; i < r.getNumberOfSubfronts(); i++)
				{
					distance.crowdingDistanceAssignment(r.getSubfront(i), m_problem.NumberOfObjectives);
				}
			}
			r = new Ranking(population);
			return r.getSubfront(0);
		}
	}
}