/// <summary> Solution.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0 
/// </version>
/// 


using System;
using Binary = JARE.Base.variable.Binary;
using JARE.Base;
using System.Collections.Generic;
namespace JARE.Base
{
	
	/// <summary> Class representing a solution for a problem.</summary>
	[Serializable]
	public class Solution
	{
		/// <summary> Gets the distance from the solution to a <code>SolutionSet</code>. 
		/// <b> REQUIRE </b>: this method has to be invoked after calling 
		/// <code>setDistanceToPopulation</code>.
		/// </summary>
		/// <returns> the distance to a specific solutionSet.
		/// </returns>
		/// <summary> Sets the distance between this solution and a <code>SolutionSet</code>.
		/// The value is stored in <code>distanceTom_solutionSet</code>.
		/// </summary>
		/// <param name="distance">The distance to a solutionSet.
		/// </param>
		virtual public double DistanceToSolutionSet
		{
			get
			{
				return m_distanceToSolutionSet;
			}
			
			
			set
			{
				m_distanceToSolutionSet = value;
			}
		
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the distance from the solution to his k-nearest nighbor in a 
		/// <code>SolutionSet</code>. Returns the value stored in
		/// <code>kDistance_</code>. <b> REQUIRE </b>: this method has to be invoked 
		/// after calling <code>setKDistance</code>.
		/// </summary>
		/// <returns> the distance to k-nearest neighbor.
		/// </returns>
		/// <summary> Sets the distance between the solution and its k-nearest neighbor in 
		/// a <code>SolutionSet</code>. The value is stored in <code>kDistance_</code>.
		/// </summary>
		/// <param name="distance">The distance to the k-nearest neighbor.
		/// </param>
		virtual public double KDistance
		{
			get
			{
				return m_kDistance;
			}
		
			
			set
			{
				m_kDistance = value;
			}
			
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the crowding distance of the solution into a <code>SolutionSet</code>.
		/// Returns the value stored in <code>m_crowdingDistance</code>.
		/// <b> REQUIRE </b>: this method has to be invoked after calling 
		/// <code>setCrowdingDistance</code>.
		/// </summary>
		/// <returns> the distance crowding distance of the solution.
		/// </returns>
		/// <summary> Sets the crowding distance of a solution in a <code>SolutionSet</code>.
		/// The value is stored in <code>m_crowdingDistance</code>.
		/// </summary>
		/// <param name="distance">The crowding distance of the solution.
		/// </param>
		virtual public double CrowdingDistance
		{
			get
			{
				return m_crowdingDistance;
			}
			
			set
			{
				m_crowdingDistance = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the fitness of the solution.
		/// Returns the value of stored in the variable <code>m_fitness</code>.
		/// <b> REQUIRE </b>: This method has to be invoked after calling 
		/// <code>setFitness()</code>.
		/// </summary>
		/// <returns> the fitness.
		/// </returns>
		/// <summary> Sets the fitness of a solution.
		/// The value is stored in <code>m_fitness</code>.
		/// </summary>
		/// <param name="fitness">The fitness of the solution.
		/// </param>
		virtual public double Fitness
		{
			get
			{
				return m_fitness;
			}
		
			set
			{
				m_fitness = value;
			}
		
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the decision variables of the solution.</summary>
		/// <returns> the <code>DecisionVariables</code> object representing the decision
		/// variables of the solution.
		/// </returns>
		/// <summary> Sets the decision variables for the solution.</summary>
		/// <param name="decisionVariables">The <code>DecisionVariables</code> object 
		/// representing the decision variables of the solution.
		/// </param>
		virtual public Variable[] DecisionVariables
		{
			get
			{
				return m_variable;
			}
			set
			{
				m_variable = value;
			}
		}
		/// <summary> Indicates if the solution is marked.</summary>
		/// <returns> true if the method <code>marked</code> has been called and, after 
		/// that, the method <code>unmarked</code> hasn't been called. False in other
		/// case.
		/// </returns>
		virtual public bool Marked
		{
			get
			{
				return this.m_marked;
			}
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the rank of the solution.
		/// <b> REQUIRE </b>: This method has to be invoked after calling 
		/// <code>setRank()</code>.
		/// </summary>
		/// <returns> the rank of the solution.
		/// </returns>
		/// <summary> Sets the rank of a solution. </summary>
		/// <param name="value">The rank of the solution.
		/// </param>
		virtual public int Rank
		{
			get
			{
				return this.m_rank;
			}
			set
			{
				this.m_rank = value;
			}
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the overall constraint violated by the solution.
		/// <b> REQUIRE </b>: This method has to be invoked after calling 
		/// <code>overallConstraintViolation</code>.
		/// </summary>
		/// <returns> the overall constraint violation by the solution.
		/// </returns>
		/// <summary> Sets the overall constraints violated by the solution.</summary>
		/// <param name="value">The overall constraints violated by the solution.
		/// </param>
		virtual public double OverallConstraintViolation
		{
			get
			{
				return this.m_overallConstraintViolation;
			}
			set
			{
				this.m_overallConstraintViolation = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the number of constraint violated by the solution.
		/// <b> REQUIRE </b>: This method has to be invoked after calling
		/// <code>setNumberOfViolatedConstraint</code>.
		/// </summary>
		/// <returns> the number of constraints violated by the solution.
		/// </returns>
		/// <summary> Sets the number of constraints violated by the solution.</summary>
		/// <param name="value">The number of constraints violated by the solution.
		/// </param>
		virtual public int NumberOfViolatedConstraint
		{
			get
			{
				return this.m_numberOfViolatedConstraints;
			}
			set
			{
				this.m_numberOfViolatedConstraints = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the location of this solution in a <code>SolutionSet</code>.
		/// <b> REQUIRE </b>: This method has to be invoked after calling
		/// <code>setLocation</code>.
		/// </summary>
		/// <returns> the location of the solution into a solutionSet
		/// </returns>
		/// <summary> Sets the location of the solution into a solutionSet. </summary>
		/// <param name="location">The location of the solution.
		/// </param>
		virtual public int Location
		{
			get
			{
				return this.m_location;
			}
			set
			{
				this.m_location = value;
			}
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the type of the variable</summary>
		/// <returns> the type of the variable
		/// </returns>
		/// <summary> Sets the type of the variable. </summary>
		/// <param name="type">The type of the variable.
		/// </param>
		virtual public SolutionType Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}
		/// <summary> Returns the aggregative value of the solution</summary>
		/// <returns> The aggregative value.
		/// </returns>
		virtual public double AggregativeValue
		{
			get
			{
				double aggValue = 0.0;
				for (int i = 0; i < NumberOfObjectives(); i++)
				{
					aggValue += getObjective(i);
				}
				return aggValue;
			}
		}
		/// <summary> Returns the number of bits of the chromosome in case of using a binary
		/// representation
		/// </summary>
		/// <returns> The number of bits if the case of binary variables, 0 otherwise
		/// </returns>
		virtual public int NumberOfBits
		{
			get
			{
				int bits = 0;
				
				for (int i = 0; i < m_variable.Length; i++)
					try
					{
                        if ((m_variable[i].VariableType == System.Type.GetType("JARE.Base.variable.Binary")) || (m_variable[i].VariableType == System.Type.GetType("JARE.Base.variable.BinaryReal")))
                            bits += ((Binary)(m_variable[i])).NumberOfBits;
                        
					}
					catch (TypeLoadException exc)
					{
                        Console.WriteLine(exc.Message);
					}
				
				return bits;
			}
			
		}
		/// <summary> Stores the problem </summary>
		internal Problem m_problem;
		
		/// <summary> Stores the type of the variable</summary>
		private SolutionType m_type;
		
		/// <summary> Stores the decision variables of the solution.</summary>
		private Variable[] m_variable;
		
		/// <summary> Stores the objectives values of the solution.</summary>
		private double[] m_objective;
		
		/// <summary> Stores the number of objective values of the solution</summary>
		private int m_numberOfObjectives;
		
		/// <summary> Stores the so called fitness value. Used in some metaheuristics</summary>
		private double m_fitness;
		
		/// <summary> Used in algorithm AbYSS, this field is intended to be used to know
		/// when a <code>Solution</code> is marked.
		/// </summary>
		private bool m_marked;
		
		/// <summary> Stores the so called rank of the solution. Used in NSGA-II</summary>
		private int m_rank;
		
		/// <summary> Stores the overall constraint violation of the solution.</summary>
		private double m_overallConstraintViolation;
		
		/// <summary> Stores the number of constraints violated by the solution.</summary>
		private int m_numberOfViolatedConstraints;
		
		/// <summary> This field is intended to be used to know the location of
		/// a solution into a <code>SolutionSet</code>. Used in MOCell
		/// </summary>
		private int m_location;
		
		/// <summary> Stores the distance to his k-nearest neighbor into a 
		/// <code>SolutionSet</code>. Used in SPEA2.
		/// </summary>
		private double m_kDistance;
		
		/// <summary> Stores the crowding distance of the the solution in a 
		/// <code>SolutionSet</code>. Used in NSGA-II.
		/// </summary>
		private double m_crowdingDistance;
		
		/// <summary> Stores the distance between this solution and a <code>SolutionSet</code>.
		/// Used in AbySS.
		/// </summary>
		private double m_distanceToSolutionSet;

#if ADDITIONAL_INFORMATION 
        public Dictionary<string, object> Info = new Dictionary<string, object>();
#endif
	
        /// <summary> Constructor.</summary>
		public Solution()
		{
			m_problem = null;
			m_marked = false;
			m_overallConstraintViolation = 0.0;
			m_numberOfViolatedConstraints = 0;
			m_type = null;
			m_variable = null;
			m_objective = null;
		} 
		
		/// <summary> Constructor</summary>
		/// <param name="numberOfObjectives">Number of objectives of the solution
		/// 
		/// This constructor is used mainly to read objective values from a file to
		/// variables of a SolutionSet to apply quality indicators
		/// </param>
		public Solution(int numberOfObjectives)
		{
			m_numberOfObjectives = numberOfObjectives;
			m_objective = new double[numberOfObjectives];
		}
		
		/// <summary> Constructor.</summary>
		/// <param name="problem">The problem to solve
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public Solution(Problem problem)
		{
			m_problem = problem;
			m_type = problem.SolutionType;
			m_numberOfObjectives = problem.NumberOfObjectives;
			m_objective = new double[m_numberOfObjectives];
			
			// Setting initial values
			m_fitness = 0.0;
			m_kDistance = 0.0;
			m_crowdingDistance = 0.0;
			m_distanceToSolutionSet = System.Double.PositiveInfinity;
			//<-
			
			m_variable = problem.createVariables();
        } 
		
		static public Solution getNewSolution(Problem problem)
		{
			return new Solution(problem);
		}
		
		/// <summary> Constructor</summary>
		/// <param name="problem">The problem to solve
		/// </param>
		public Solution(Problem problem, Variable[] variables)
		{
			m_problem = problem;
			m_type = problem.SolutionType;
			m_numberOfObjectives = problem.NumberOfObjectives;
			m_objective = new double[m_numberOfObjectives];
			
			// Setting initial values
			m_fitness = 0.0;
			m_kDistance = 0.0;
			m_crowdingDistance = 0.0;
			m_distanceToSolutionSet = System.Double.PositiveInfinity;
			//<-
			
			m_variable = variables;
		} // Constructor
		
		/// <summary> Copy constructor.</summary>
		/// <param name="solution">Solution to copy.
		/// </param>
		public Solution(Solution solution)
		{
			m_problem = solution.m_problem;
			m_type = solution.m_type;
			
			m_numberOfObjectives = solution.NumberOfObjectives();
			m_objective = new double[m_numberOfObjectives];
			for (int i = 0; i < m_objective.Length; i++)
			{
				m_objective[i] = solution.getObjective(i);
			} // for
			//<-
			
			m_variable = m_type.copyVariables(solution.m_variable);
			m_overallConstraintViolation = solution.OverallConstraintViolation;
			m_numberOfViolatedConstraints = solution.NumberOfViolatedConstraint;
			m_distanceToSolutionSet = solution.DistanceToSolutionSet;
			m_crowdingDistance = solution.CrowdingDistance;
			m_kDistance = solution.KDistance;
			m_fitness = solution.Fitness;
			m_marked = solution.Marked;
			m_rank = solution.Rank;
			m_location = solution.Location;
		} // Solution
		
		/// <summary> Sets the value of the i-th objective.</summary>
		/// <param name="i">The number identifying the objective.
		/// </param>
		/// <param name="value">The value to be stored.
		/// </param>
		public virtual void  setObjective(int i, double val)
		{
			m_objective[i] = val;
		} // setObjective
		
		/// <summary> Returns the value of the i-th objective.</summary>
		/// <param name="i">The value of the objective.
		/// </param>
		public virtual double getObjective(int i)
		{
			return m_objective[i];
		} // getObjective
		
		/// <summary> Returns the number of objectives.</summary>
		/// <returns> The number of objectives.
		/// </returns>
		public virtual int NumberOfObjectives()
		{
			if (m_objective == null)
				return 0;
			else
				return m_numberOfObjectives;
		} // numberOfObjectives
		
		/// <summary> Returns the number of decision variables of the solution.</summary>
		/// <returns> The number of decision variables.
		/// </returns>
		public virtual int numberOfVariables()
		{
			return m_problem.NumberOfVariables;
		} // numberOfVariables
		
		/// <summary> Returns a string representing the solution.</summary>
		/// <returns> The string.
		/// </returns>
		public override string ToString()
		{
			string aux = "";
			for (int i = 0; i < this.m_numberOfObjectives; i++)
				aux = aux + this.getObjective(i) + " ";
			
			return aux;
		} // toString
		
		/// <summary> Establishes the solution as marked.</summary>
		public virtual void Mark()
		{
			this.m_marked = true;
		} // marked
		
		/// <summary> Established the solution as unmarked.</summary>
		public virtual void  unMarked()
		{
			this.m_marked = false;
		} // unMarked
		
		/// <summary> Sets the type of the variable. </summary>
		/// <param name="type">The type of the variable.
		/// </param>
		//public void setType(String type) {
		// type_ = Class.forName("") ;
		//} // setType
	} // Solution
}