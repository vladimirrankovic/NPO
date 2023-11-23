/// <summary> OMOPSO.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using CrowdingArchive = JARE.util.archive.CrowdingArchive;
using JARE.Base.operators.mutation;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using JARE.util;
namespace JARE.metaheuristics.omopso
{
	
	/// <summary> This class representing an asychronous version of OMOPSO algorithm</summary>
	[Serializable]
	public class OMOPSO:Algorithm
	{
		/// <summary> Gets the leaders of the OMOPSO algorithm</summary>
		virtual public SolutionSet Leader
		{
			get
			{
				return m_leaders;
			}
			// getLeader 
			
		}
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Stores the number of m_particles used</summary>
		private int m_particlesSize;
		
		/// <summary> Stores the maximum size for the archive</summary>
		private int m_archiveSize;
		
		/// <summary> Stores the maximum number of m_iteration</summary>
		private int m_maxIterations;
		
		/// <summary> Stores the current number of m_iteration</summary>
		private int m_iteration;
		
		/// <summary> Stores the perturbation used by the non-uniform mutation</summary>
		private double m_perturbation;
		
		/// <summary> Stores the particles</summary>
		private SolutionSet m_particles;
		
		/// <summary> Stores the m_best solutions founds so far for each particles</summary>
		private Solution[] m_best;
		
		/// <summary> Stores the m_leaders</summary>
		private CrowdingArchive m_leaders;
		
		/// <summary> Stores the epsilon-archive</summary>
		private NonDominatedSolutionList m_eArchive;
		
		/// <summary> Stores the m_speed of each particle</summary>
		private double[][] m_speed;
		
		/// <summary> Stores a comparator for checking dominance</summary>
		private System.Collections.IComparer m_dominance;
		
		/// <summary> Stores a comparator for crowding checking</summary>
        private System.Collections.Generic.IComparer<JARE.Base.Solution> m_crowdingDistanceComparator;
		
		/// <summary> Stores a <code>Distance</code> object</summary>
		private Distance m_distance;
		
		/// <summary> Stores a operator for uniform mutations</summary>
		private Operator m_uniformMutation;
		
		/// <summary> Stores a operator for non uniform mutations</summary>
		private Operator m_nonUniformMutation;
		
		/// <summary> m_eta value</summary>
		private double m_eta = 0.0075;
		
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public OMOPSO(Problem problem)
		{
			m_problem = problem;
		} // OMOPSO
		
		/// <summary> Initialize all parameter of the algorithm</summary>
		public virtual void  initParams()
		{
			m_particlesSize = ((System.Int32) getInputParameter("swarmSize"));
			m_archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			m_maxIterations = ((System.Int32) getInputParameter("maxIterations"));
			m_perturbation = ((System.Double) getInputParameter("perturbationIndex"));
			
			m_particles = new SolutionSet(m_particlesSize);
			m_best = new Solution[m_particlesSize];
			m_leaders = new CrowdingArchive(m_archiveSize, m_problem.NumberOfObjectives);
			m_eArchive = new NonDominatedSolutionList(new EpsilonDominanceComparator(m_eta));
			
			
			// Create the dominator for equadless and dominance
			m_dominance = new DominanceComparator();
			m_crowdingDistanceComparator = new CrowdingDistanceComparator();
			m_distance = new Distance();
			
			// Create the m_speed vector
			m_speed = new double[m_particlesSize][];
			for (int i = 0; i < m_particlesSize; i++)
			{
				m_speed[i] = new double[m_problem.NumberOfVariables];
			}
			
			m_uniformMutation = new UniformMutation();
			m_uniformMutation.setParameter("perturbationIndex", m_perturbation);
			m_uniformMutation.setParameter("probability", 1.0 / m_problem.NumberOfVariables);
			m_nonUniformMutation = new NonUniformMutation();
			m_nonUniformMutation.setParameter("perturbationIndex", m_perturbation);
			m_nonUniformMutation.setParameter("maxIterations", m_maxIterations);
			m_nonUniformMutation.setParameter("probability", 1.0 / m_problem.NumberOfVariables);
		} // initParams
		
		
		/// <summary> Update the spped of each particle</summary>
		/// <throws>  SMException  </throws>
		private void  computeSpeed()
		{
			double r1, r2, W, C1, C2;
			Variable[] bestGlobal;
			
			for (int i = 0; i < m_particlesSize; i++)
			{
				Variable[] particle = m_particles.getSolution(i).DecisionVariables;
				Variable[] bestParticle = m_best[i].DecisionVariables;
				
				//Select a global m_best for calculate the speed of particle i, bestGlobal
				Solution one, two;
				int pos1 = PseudoRandom.randInt(0, m_leaders.size() - 1);
				int pos2 = PseudoRandom.randInt(0, m_leaders.size() - 1);
				one = m_leaders.getSolution(pos1);
				two = m_leaders.getSolution(pos2);
				
				if (m_crowdingDistanceComparator.Compare(one, two) < 1)
					bestGlobal = one.DecisionVariables;
				else
					bestGlobal = two.DecisionVariables;
				//
				
				//Params for velocity equation
				r1 = PseudoRandom.randDouble();
				r2 = PseudoRandom.randDouble();
				C1 = PseudoRandom.randDouble(1.5, 2.0);
				C2 = PseudoRandom.randDouble(1.5, 2.0);
				W = PseudoRandom.randDouble(0.1, 0.5);
				//
				
				for (int var = 0; var < particle.Length; var++)
				{
					//Computing the velocity of this particle
					m_speed[i][var] = W * m_speed[i][var] + C1 * r1 * (bestParticle[var].getValue() - particle[var].getValue()) + C2 * r2 * (bestGlobal[var].getValue() - particle[var].getValue());
				}
			}
		} // computeSpeed
		
		/// <summary> Update the position of each particle</summary>
		/// <throws>  SMException  </throws>
		private void  computeNewPositions()
		{
			for (int i = 0; i < m_particlesSize; i++)
			{
				Variable[] particle = m_particles.getSolution(i).DecisionVariables;
				//particle.move(m_speed[i]);
				for (int var = 0; var < particle.Length; var++)
				{
					particle[var].setValue(particle[var].getValue() + m_speed[i][var]);
					if (particle[var].getValue() < m_problem.getLowerLimit(var))
					{
						particle[var].setValue(m_problem.getLowerLimit(var));
						m_speed[i][var] = m_speed[i][var] * (- 1.0);
					}
					if (particle[var].getValue() > m_problem.getUpperLimit(var))
					{
						particle[var].setValue(m_problem.getUpperLimit(var));
						m_speed[i][var] = m_speed[i][var] * (- 1.0);
					}
				}
			}
		} // computeNewPositions
		
		
		/// <summary> Apply a mutation operator to all particles in the swarm</summary>
		/// <throws>  SMException  </throws>
		private void  mopsoMutation(int actualIteration, int totalIterations)
		{
			//There are three groups of m_particles, the ones that are mutated with
			//a non-uniform mutation operator, the ones that are mutated with a 
			//uniform mutation and the one that no are mutated
			m_nonUniformMutation.setParameter("currentIteration", actualIteration);
			//*/
			
			for (int i = 0; i < m_particles.size(); i++)
				if (i % 3 == 0)
				{
					//m_particles mutated with a non-uniform mutation
					m_nonUniformMutation.execute(m_particles.getSolution(i));
				}
				else if (i % 3 == 1)
				{
					//m_particles mutated with a uniform mutation operator
					m_uniformMutation.execute(m_particles.getSolution(i));
				}
				//m_particles without mutation
				else
				{
				}
		} // mopsoMutation
		
		
		/// <summary> Runs of the OMOPSO algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			initParams();
			
			//->Step 1 (and 3) Create the initial population and evaluate
			for (int i = 0; i < m_particlesSize; i++)
			{
				Solution particle = new Solution(m_problem);
				m_problem.evaluate(particle);
				m_problem.evaluateConstraints(particle);
				m_particles.add(particle);
			}
			
			//-> Step2. Initialize the m_speed of each particle to 0
			for (int i = 0; i < m_particlesSize; i++)
			{
				for (int j = 0; j < m_problem.NumberOfVariables; j++)
				{
					m_speed[i][j] = 0.0;
				}
			}
			
			
			// Step4 and 5   
			for (int i = 0; i < m_particles.size(); i++)
			{
				Solution particle = new Solution(m_particles.getSolution(i));
				if (m_leaders.add(particle))
				{
					m_eArchive.add(new Solution(particle));
				}
			}
			
			//-> Step 6. Initialice the memory of each particle
			for (int i = 0; i < m_particles.size(); i++)
			{
				Solution particle = new Solution(m_particles.getSolution(i));
				m_best[i] = particle;
			}
			
			//Crowding the m_leaders
			m_distance.crowdingDistanceAssignment(m_leaders, m_problem.NumberOfObjectives);
			
			//-> Step 7. Iterations ..        
			while (m_iteration < m_maxIterations)
			{
				//Compute the m_speed        
				computeSpeed();
				
				//Compute the new positions for the m_particles            
				computeNewPositions();
				
				//Mutate the m_particles          
				mopsoMutation(m_iteration, m_maxIterations);
				
				//Evaluate the new m_particles in new positions
				for (int i = 0; i < m_particles.size(); i++)
				{
					Solution particle = m_particles.getSolution(i);
					m_problem.evaluate(particle);
					m_problem.evaluateConstraints(particle);
				}
				
				//Actualize the archive          
				for (int i = 0; i < m_particles.size(); i++)
				{
					Solution particle = new Solution(m_particles.getSolution(i));
					if (m_leaders.add(particle))
					{
						m_eArchive.add(new Solution(particle));
					}
				}
				
				//Actualize the memory of this particle
				for (int i = 0; i < m_particles.size(); i++)
				{
					int flag = m_dominance.Compare(m_particles.getSolution(i), m_best[i]);
					if (flag != 1)
					{
						// the new particle is m_best than the older remeber        
						Solution particle = new Solution(m_particles.getSolution(i));
						//this.m_best.reemplace(i,particle);
						m_best[i] = particle;
					}
				}
				
				//Crowding the m_leaders
				m_distance.crowdingDistanceAssignment(m_leaders, m_problem.NumberOfObjectives);
				m_iteration++;
			}
			
			return this.m_leaders;
			//return m_eArchive;
		} // execute
	} // OMOPSO
}