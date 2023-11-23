/// <summary> SMPSO.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using JARE.Base;
using CrowdingArchive = JARE.util.archive.CrowdingArchive;
using JARE.Base.operators.mutation;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using Hypervolume = JARE.qualityIndicator.Hypervolume;
using JARE.util;
using XReal = JARE.util.wrapper.XReal;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
namespace JARE.metaheuristics.smpso
{
	
	[Serializable]
	public class SMPSO:Algorithm
	{
		
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
		/// <summary> Stores the particles</summary>
		private SolutionSet m_particles;
		/// <summary> Stores the m_best solutions founds so far for each particles</summary>
		private Solution[] m_best;
		/// <summary> Stores the m_leaders</summary>
		private CrowdingArchive m_leaders;
		/// <summary> Stores the m_speed of each particle</summary>
		private double[][] m_speed;
		/// <summary> Stores a comparator for checking dominance</summary>
		private System.Collections.IComparer m_dominance;
		/// <summary> Stores a comparator for crowding checking</summary>
        private System.Collections.Generic.IComparer<JARE.Base.Solution> m_crowdingDistanceComparator;
		/// <summary> Stores a <code>Distance</code> object</summary>
		private Distance m_distance;
		/// <summary> Stores a operator for non uniform mutations</summary>
		private Operator m_polynomialMutation;
		
		internal QualityIndicator m_indicators; // QualityIndicator object
		
		internal double m_r1Max;
		internal double m_r1Min;
		internal double m_r2Max;
		internal double m_r2Min;
		internal double m_C1Max;
		internal double m_C1Min;
		internal double m_C2Max;
		internal double m_C2Min;
		internal double m_WMax;
		internal double m_WMin;
		internal double m_ChVel1;
		internal double m_ChVel2;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public SMPSO(Problem problem)
		{
			m_problem = problem;
			
			m_r1Max = 1.0;
			m_r1Min = 0.0;
			m_r2Max = 1.0;
			m_r2Min = 0.0;
			m_C1Max = 2.5;
			m_C1Min = 1.5;
			m_C2Max = 2.5;
			m_C2Min = 1.5;
			m_WMax = 0.1;
			m_WMin = 0.1;
			m_ChVel1 = - 1;
			m_ChVel2 = - 1;
		} // Constructor

        //PROVERENO VLADA-CONVERT: TELO FUNKCIJE UBACENO IZ FAJLA U JAVI!!!
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        //public SMPSO(Problem problem, 
        //Vector < Double > variables, 
        //String trueParetoFront) throws FileNotFoundException
		public SMPSO(Problem problem, System.Collections.Generic.List<double> variables, String trueParetoFront)// throws FileNotFoundException
        {
            m_problem = problem;
            
            m_r1Max = variables[0];
            m_r1Min = variables[1];
            m_r2Max = variables[2];
            m_r2Min = variables[3];
            m_C1Max = variables[4];
            m_C1Min = variables[5];
            m_C2Max = variables[6];
            m_C2Min = variables[7];
            m_WMax = variables[8];
            m_WMin = variables[9];
            m_ChVel1 = variables[10];
            m_ChVel2 = variables[11];

            m_hy = new Hypervolume();
            JARE.qualityIndicator.util.MetricsUtil mu = new JARE.qualityIndicator.util.MetricsUtil();
            m_trueFront = mu.readNonDominatedSolutionSet(trueParetoFront);
            m_trueHypervolume = m_hy.hypervolume(m_trueFront.writeObjectivesToMatrix(),
            m_trueFront.writeObjectivesToMatrix(),
            m_problem.NumberOfObjectives);

        } // SMPSO
		private double m_trueHypervolume;
		private Hypervolume m_hy;
		private SolutionSet m_trueFront;
		private double[] m_deltaMax;
		private double[] m_deltaMin;
		internal bool m_success;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public SMPSO(Problem problem, System.String trueParetoFront)
		{
			m_problem = problem;
			m_hy = new Hypervolume();
			JARE.qualityIndicator.util.MetricsUtil mu = new JARE.qualityIndicator.util.MetricsUtil();
			m_trueFront = mu.readNonDominatedSolutionSet(trueParetoFront);
			m_trueHypervolume = m_hy.hypervolume(m_trueFront.writeObjectivesToMatrix(), m_trueFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
			
			// Default configuration
			m_r1Max = 1.0;
			m_r1Min = 0.0;
			m_r2Max = 1.0;
			m_r2Min = 0.0;
			m_C1Max = 2.5;
			m_C1Min = 1.5;
			m_C2Max = 2.5;
			m_C2Min = 1.5;
			m_WMax = 0.1;
			m_WMin = 0.1;
			m_ChVel1 = - 1;
			m_ChVel2 = - 1;
		} // Constructor
		
		/// <summary> Initialize all parameter of the algorithm</summary>
		public virtual void  initParams()
		{
			m_particlesSize = ((System.Int32) getInputParameter("swarmSize"));
			m_archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			m_maxIterations = ((System.Int32) getInputParameter("maxIterations"));
			
			m_indicators = (QualityIndicator) getInputParameter("indicators");
			
			m_polynomialMutation = m_operators["mutation"];
			
			m_iteration = 0;
			
			m_success = false;
			
			m_particles = new SolutionSet(m_particlesSize);
			m_best = new Solution[m_particlesSize];
			m_leaders = new CrowdingArchive(m_archiveSize, m_problem.NumberOfObjectives);
			
			// Create comparators for dominance and crowding distance
			m_dominance = new DominanceComparator();
			m_crowdingDistanceComparator = new CrowdingDistanceComparator();
			m_distance = new Distance();
			
			// Create the m_speed vector
			m_speed = new double[m_particlesSize][];
			for (int i = 0; i < m_particlesSize; i++)
			{
				m_speed[i] = new double[m_problem.NumberOfVariables];
			}
			
			
			m_deltaMax = new double[m_problem.NumberOfVariables];
			m_deltaMin = new double[m_problem.NumberOfVariables];
			for (int i = 0; i < m_problem.NumberOfVariables; i++)
			{
				m_deltaMax[i] = (m_problem.getUpperLimit(i) - m_problem.getLowerLimit(i)) / 2.0;
				m_deltaMin[i] = - m_deltaMax[i];
			} // for
		} // initParams 
		
		// Adaptive inertia 
		private double inertiaWeight(int iter, int miter, double wma, double wmin)
		{
			return wma; // - (((wma-wmin)*(double)iter)/(double)miter);
		} // inertiaWeight
		
		// constriction coefficient (M. Clerc)
		private double constrictionCoefficient(double c1, double c2)
		{
			double rho = c1 + c2;
			//rho = 1.0 ;
			if (rho <= 4)
			{
				return 1.0;
			}
			else
			{
				return 2 / (2 - rho - System.Math.Sqrt(System.Math.Pow(rho, 2.0) - 4.0 * rho));
			}
		} // constrictionCoefficient
		
		
		// velocity bounds
		private double velocityConstriction(double v, double[] deltaMax, double[] deltaMin, int variableIndex, int particleIndex)
		{
			
			
			double result;
			
			double dmax = deltaMax[variableIndex];
			double dmin = deltaMin[variableIndex];
			
			result = v;
			
			if (v > dmax)
			{
				result = dmax;
			}
			
			if (v < dmin)
			{
				result = dmin;
			}
			
			return result;
		} // velocityConstriction
		
		/// <summary> Update the speed of each particle</summary>
		/// <throws>  SMException  </throws>
		private void  computeSpeed(int iter, int miter)
		{
			double r1, r2, W, C1, C2;
			double wmax, wmin, deltaMax, deltaMin;
			XReal bestGlobal;
			
			for (int i = 0; i < m_particlesSize; i++)
			{
				XReal particle = new XReal(m_particles.getSolution(i));
				XReal bestParticle = new XReal(m_best[i]);
				
				//Select a global m_best for calculate the speed of particle i, bestGlobal
				Solution one, two;
				int pos1 = PseudoRandom.randInt(0, m_leaders.size() - 1);
				int pos2 = PseudoRandom.randInt(0, m_leaders.size() - 1);
				one = m_leaders.getSolution(pos1);
				two = m_leaders.getSolution(pos2);
				
				if (m_crowdingDistanceComparator.Compare(one, two) < 1)
				{
					bestGlobal = new XReal(one);
				}
				else
				{
					bestGlobal = new XReal(two);
					//Params for velocity equation
				}
				r1 = PseudoRandom.randDouble(m_r1Min, m_r1Max);
				r2 = PseudoRandom.randDouble(m_r2Min, m_r2Max);
				C1 = PseudoRandom.randDouble(m_C1Min, m_C1Max);
				C2 = PseudoRandom.randDouble(m_C2Min, m_C2Max);
				W = PseudoRandom.randDouble(m_WMin, m_WMax);
				//
				wmax = m_WMax;
				wmin = m_WMin;
				
				for (int var = 0; var < particle.NumberOfDecisionVariables; var++)
				{
					//Computing the velocity of this particle 
					m_speed[i][var] = velocityConstriction(constrictionCoefficient(C1, C2) * (inertiaWeight(iter, miter, wmax, wmin) * m_speed[i][var] + C1 * r1 * (bestParticle.getValue(var) - particle.getValue(var)) + C2 * r2 * (bestGlobal.getValue(var) - particle.getValue(var))), m_deltaMax, m_deltaMin, var, i);
				}
			}
		} // computeSpeed
		
		/// <summary> Update the position of each particle</summary>
		/// <throws>  SMException  </throws>
		private void  computeNewPositions()
		{
			for (int i = 0; i < m_particlesSize; i++)
			{
				//Variable[] particle = m_particles.get(i).getDecisionVariables();
				XReal particle = new XReal(m_particles.getSolution(i));
				//particle.move(m_speed[i]);
				for (int var = 0; var < particle.NumberOfDecisionVariables; var++)
				{
					particle.setValue(var, particle.getValue(var) + m_speed[i][var]);
					
					if (particle.getValue(var) < m_problem.getLowerLimit(var))
					{
						particle.setValue(var, m_problem.getLowerLimit(var));
						m_speed[i][var] = m_speed[i][var] * m_ChVel1; //    
					}
					if (particle.getValue(var) > m_problem.getUpperLimit(var))
					{
						particle.setValue(var, m_problem.getUpperLimit(var));
						m_speed[i][var] = m_speed[i][var] * m_ChVel2; //   
					}
				}
			}
		} // computeNewPositions
		
		/// <summary> Apply a mutation operator to some particles in the swarm</summary>
		/// <throws>  SMException  </throws>
		private void  mopsoMutation(int actualIteration, int totalIterations)
		{
			for (int i = 0; i < m_particles.size(); i++)
			{
				if ((i % 6) == 0)
					m_polynomialMutation.execute(m_particles.getSolution(i));
				//if (i % 3 == 0) { //m_particles mutated with a non-uniform mutation %3
				//  m_nonUniformMutation.execute(m_particles.get(i));
				//} else if (i % 3 == 1) { //m_particles mutated with a uniform mutation operator
				//  m_uniformMutation.execute(m_particles.get(i));
				//} else //m_particles without mutation
				//;
			}
		} // mopsoMutation
		
		/// <summary> Runs of the SMPSO algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			initParams();
			
			m_success = false;
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
				m_leaders.add(particle);
			}
			
			//-> Step 6. Initialize the memory of each particle
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
				try
				{
					//Compute the m_speed
					computeSpeed(m_iteration, m_maxIterations);
				}
				catch (System.IO.IOException ex)
				{
                    ////////Logger.getLogger(typeof(SMPSO).FullName).log(Level.SEVERE, null, ex);
				}
				
				//Compute the new positions for the m_particles            
				computeNewPositions();
				
				//Mutate the m_particles          
				mopsoMutation(m_iteration, m_maxIterations);
				
				//Evaluate the new m_particles in new positions
				for (int i = 0; i < m_particles.size(); i++)
				{
					Solution particle = m_particles.getSolution(i);
					m_problem.evaluate(particle);
				}
				
				//Actualize the archive          
				for (int i = 0; i < m_particles.size(); i++)
				{
					Solution particle = new Solution(m_particles.getSolution(i));
					m_leaders.add(particle);
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
		} // execute
		/// <summary> Gets the leaders of the SMPSO algorithm</summary>
		virtual public SolutionSet Leader
		{
			get
			{
				return m_leaders;
			}
			// getLeader   
			
		}

	} // SMPSO
}