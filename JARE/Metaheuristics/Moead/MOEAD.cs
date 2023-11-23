/// <summary> MOEAD.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.util;
using Algorithm = JARE.Base.Algorithm;
using Problem = JARE.Base.Problem;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using PseudoRandom = JARE.util.PseudoRandom;
using SupportClass = JARE.support.SupportClass;
using Tokenizer = JARE.support.Tokenizer;
namespace JARE.metaheuristics.moead
{
	
	[Serializable]
	public class MOEAD:Algorithm
	{
		private Problem m_problem;
		/// <summary> Population size</summary>
		private int m_populationSize;
		/// <summary> Stores the population</summary>
		private SolutionSet m_population;
		/// <summary> Z vector (ideal point)</summary>
		internal double[] m_z;
		/// <summary> Lambda vectors</summary>
		//Vector<Vector<Double>> m_lambda ; 
		internal double[][] m_lambda;
		/// <summary> T: neighbour size</summary>
		internal int m_T;
		/// <summary> Neighborhood</summary>
		internal int[][] m_neighborhood;
		/// <summary> delta: probability that parent solutions are selected from neighbourhood</summary>
		internal double m_delta;
		/// <summary> nr: maximal number of solutions replaced by each child solution</summary>
		internal int m_nr;
		internal Solution[] m_indArray;
        internal System.String m_functionType;
		internal int m_evaluations;
		/// <summary> Operators</summary>
		internal Operator m_crossover;
		internal Operator m_mutation;
		
		internal System.String m_dataDirectory;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public MOEAD(Problem problem)
		{
			m_problem = problem;
			
			m_functionType = "_TCHE1";
		} // DMOEA
		
		public override SolutionSet execute()
		{
			int maxEvaluations;
			
			m_evaluations = 0;
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			m_populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			m_dataDirectory = this.getInputParameter("dataDirectory").ToString();
			
			m_population = new SolutionSet(m_populationSize);
			m_indArray = new Solution[m_problem.NumberOfObjectives];
			
			m_T = 20;
			m_delta = 0.9;
			m_nr = 2;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_T = (int) (0.1 * m_populationSize);
			m_delta = 0.9;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_nr = (int) (0.01 * m_populationSize);
			
			m_neighborhood = new int[m_populationSize][];
			for (int i = 0; i < m_populationSize; i++)
			{
				m_neighborhood[i] = new int[m_T];
			}
			
			m_z = new double[m_problem.NumberOfObjectives];
			//m_lambda = new Vector(m_problem.getNumberOfObjectives()) ;
			m_lambda = new double[m_populationSize][];
			for (int i2 = 0; i2 < m_populationSize; i2++)
			{
				m_lambda[i2] = new double[m_problem.NumberOfObjectives];
			}
			
			m_crossover = m_operators["crossover"]; // default: DE crossover
			m_mutation = m_operators["mutation"]; // default: polynomial mutation
			
			// STEP 1. Initialization
			// STEP 1.1. Compute euclidean distances between weight vectors and find T
			initUniformWeight();
			
			initNeighborhood();
			
			// STEP 1.2. Initialize population
			initPopulation();
			
			// STEP 1.3. Initialize m_z
			initIdealPoint();
			
			// STEP 2. Update
			do 
			{
				int[] permutation = new int[m_populationSize];
				Utils.randomPermutation(permutation, m_populationSize);
				
				for (int i = 0; i < m_populationSize; i++)
				{
					//int n = permutation[i]; // or int n = i;
					int n = i; // or int n = i;
					int type;
					double rnd = PseudoRandom.randDouble();
					
					// STEP 2.1. Mating selection based on probability
					if (rnd < m_delta)
					// if (rnd < realb)    
					{
						type = 1; // neighborhood
					}
					else
					{
						type = 2; // whole population
					}
					//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
                    //Vector < Integer > p = new Vector < Integer >();
					System.Collections.Generic.List<int> p = new System.Collections.Generic.List<int>();
					matingSelection(p, n, 2, type);
					
					// STEP 2.2. Reproduction
					Solution child;
					Solution[] parents = new Solution[3];
					
					parents[0] = m_population.getSolution(p[0]);
                    parents[1] = m_population.getSolution(p[1]);
					parents[2] = m_population.getSolution(n);
					
					// Apply DE crossover 
					child = (Solution) m_crossover.execute(new System.Object[]{m_population.getSolution(n), parents});
					
					// Apply mutation
					m_mutation.execute(child);
					
					// Evaluation
					m_problem.evaluate(child);
					
					m_evaluations++;
					
					// STEP 2.3. Repair. Not necessary
					
					// STEP 2.4. Update m_z
					updateReference(child);
					
					// STEP 2.5. Update of solutions
					updateProblem(child, n, type);
				} // for 
			}
			while (m_evaluations < maxEvaluations);
			
			return m_population;
		}
		
		
		/// <summary> initUniformWeight</summary>
		public virtual void  initUniformWeight()
		{
			if ((m_problem.NumberOfObjectives == 2) && (m_populationSize < 300))
			{
				for (int n = 0; n < m_populationSize; n++)
				{
					double a = 1.0 * n / (m_populationSize - 1);
					m_lambda[n][0] = a;
					m_lambda[n][1] = 1 - a;
				} // for
			}
			// if
			else
			{
				System.String dataFileName;
				dataFileName = "W" + m_problem.NumberOfObjectives + "m_D" + m_populationSize + ".dat";
				
				
				try
				{
					// Open the file
                    //VLADA-CONVERT NAPOMENA: POTENCIJALNI PROBLEM
					//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
					System.IO.FileStream fis = new System.IO.FileStream(m_dataDirectory + "/" + dataFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					System.IO.StreamReader isr = new System.IO.StreamReader(fis, System.Text.Encoding.Default);
					//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					System.IO.StreamReader br = new System.IO.StreamReader(isr.BaseStream, isr.CurrentEncoding);
					
					int numberOfObjectives = 0;
					int i = 0;
					int j = 0;
					System.String aux = br.ReadLine();
					while (aux != null)
					{
						Tokenizer st = new Tokenizer(aux);
						j = 0;
						numberOfObjectives = st.Count;
						while (st.HasMoreTokens())
						{
							//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
							double value = (System.Double.Parse(st.NextToken()));
							m_lambda[i][j] = value;
							//System.out.println("lambda["+i+","+j+"] = " + value) ;
							j++;
						}
						aux = br.ReadLine();
						i++;
					}
					br.Close();
				}
				catch (System.Exception e)
				{
					System.Console.Out.WriteLine("initUniformWeight: failed when reading for file: " + m_dataDirectory + "/" + dataFileName);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			} // else
			
			//System.exit(0) ;
		} // initUniformWeight
		/// <summary> </summary>
		public virtual void  initNeighborhood()
		{
			double[] x = new double[m_populationSize];
			int[] idx = new int[m_populationSize];
			
			for (int i = 0; i < m_populationSize; i++)
			{
				// calculate the distances based on weight vectors
				for (int j = 0; j < m_populationSize; j++)
				{
					x[j] = Utils.distVector(m_lambda[i], m_lambda[j]);
					//x[j] = dist_vector(population[i].namda,population[j].namda);
					idx[j] = j;
					//System.out.println("x["+j+"]: "+x[j]+ ". idx["+j+"]: "+idx[j]) ;
				} // for
				
				// find 'niche' nearest neighboring subproblems
				Utils.minFastSort(x, idx, m_populationSize, m_T);
				//minfastsort(x,idx,population.size(),niche);
				
				for (int k = 0; k < m_T; k++)
				{
					m_neighborhood[i][k] = idx[k];
					//System.out.println("neg["+i+","+k+"]: "+ m_neighborhood[i][k]) ;
				}
			} // for
		} // initNeighborhood
		
		/// <summary> </summary>
		public virtual void  initPopulation()
		{
			for (int i = 0; i < m_populationSize; i++)
			{
				Solution newSolution = new Solution(m_problem);
				
				m_problem.evaluate(newSolution);
				m_evaluations++;
				m_population.add(newSolution);
			} // for
		} // initPopulation
		
		/// <summary> </summary>
		internal virtual void  initIdealPoint()
		{
			for (int i = 0; i < m_problem.NumberOfObjectives; i++)
			{
				m_z[i] = 1.0e+30;
				m_indArray[i] = new Solution(m_problem);
				m_problem.evaluate(m_indArray[i]);
				m_evaluations++;
			} // for
			
			for (int i = 0; i < m_populationSize; i++)
			{
				updateReference(m_population.getSolution(i));
			} // for
		} // initIdealPoint
		
		/// <summary> </summary>
        //PROVERENO VLADA-CONVERT: TELO FUNKCIJE UBACENO IZ FAJLA U JAVI!!! 
        ////UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        //public
        ////UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        //void matingSelection(Vector < Integer > list, int cid, int size, int type)
        public void matingSelection(System.Collections.Generic.List<int> list, int cid, int size, int type)
        {
            // list : the set of the indexes of selected mating parents
            // cid  : the id of current subproblem
            // size : the number of selected mating parents
            // type : 1 - neighborhood; otherwise - whole population
            int ss;
            int r;
            int p;

            ss = m_neighborhood[cid].Length;
            while (list.Count < size)
            {
                if (type == 1)
                {
                    r = PseudoRandom.randInt(0, ss - 1);
                    p = m_neighborhood[cid][r];
                    //p = population[cid].table[r];
                }
                else
                {
                    p = PseudoRandom.randInt(0, m_populationSize - 1);
                }
                Boolean flag = true;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == p) // p is in the list
                    {
                        flag = false;
                        break;
                    }
                }
                
                //if (flag) list.push_back(p);
                if (flag)
                {
                    list.Add(p);
                }
            }
        } // matingSelection 
		/// <summary> </summary>
		/// <param name="individual">
		/// </param>
		internal virtual void  updateReference(Solution individual)
		{
			for (int n = 0; n < m_problem.NumberOfObjectives; n++)
			{
				if (individual.getObjective(n) < m_z[n])
				{
					m_z[n] = individual.getObjective(n);
					
					m_indArray[n] = individual;
				}
			}
		} // updateReference
		
		/// <param name="individual">
		/// </param>
		/// <param name="id">
		/// </param>
		/// <param name="type">
		/// </param>
		internal virtual void  updateProblem(Solution indiv, int id, int type)
		{
			// indiv: child solution
			// id:   the id of current subproblem
			// type: update solutions in - neighborhood (1) or whole population (otherwise)
			int size;
			int time;
			
			time = 0;
			
			if (type == 1)
			{
				size = m_neighborhood[id].Length;
			}
			else
			{
				size = m_population.size();
			}
			int[] perm = new int[size];
			
			Utils.randomPermutation(perm, size);
			
			for (int i = 0; i < size; i++)
			{
				int k;
				if (type == 1)
				{
					k = m_neighborhood[id][perm[i]];
				}
				else
				{
					k = perm[i]; // calculate the values of objective function regarding the current subproblem
				}
				double f1, f2;
				
				f1 = fitnessFunction(m_population.getSolution(k), m_lambda[k]);
				f2 = fitnessFunction(indiv, m_lambda[k]);
				
				if (f2 < f1)
				{
					m_population.replace(k, new Solution(indiv));
					//population[k].indiv = indiv;
					time++;
				}
				// the maximal number of solutions updated is not allowed to exceed 'limit'
				if (time >= m_nr)
				{
					return ;
				}
			}
		} // updateProblem
		
		internal virtual double fitnessFunction(Solution individual, double[] lambda)
		{
			double fitness;
			fitness = 0.0;
			
			if (m_functionType.Equals("_TCHE1"))
			{
				double maxFun = - 1.0e+30;
				
				for (int n = 0; n < m_problem.NumberOfObjectives; n++)
				{
					double diff = System.Math.Abs(individual.getObjective(n) - m_z[n]);
					
					double feval;
					if (lambda[n] == 0)
					{
						feval = 0.0001 * diff;
					}
					else
					{
						feval = diff * lambda[n];
					}
					if (feval > maxFun)
					{
						maxFun = feval;
					}
				} // for
				
				fitness = maxFun;
			}
			// if
			else
			{
				System.Console.Out.WriteLine("MOEAD.fitnessFunction: unknown type " + m_functionType);
				System.Environment.Exit(- 1);
			}
			return fitness;
		} // fitnessEvaluation
	} // MOEAD
}