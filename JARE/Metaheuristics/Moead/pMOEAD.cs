/// <summary> pMOEAD.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.concurrent.BrokenBarrierException' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using BrokenBarrierException = java.util.concurrent.BrokenBarrierException;
//UPGRADE_TODO: The type 'java.util.concurrent.CyclicBarrier' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using CyclicBarrier = java.util.concurrent.CyclicBarrier;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using JARE.Base;
using JARE.util;
using JARE.support;
using Algorithm = JARE.Base.Algorithm;
using Problem = JARE.Base.Problem;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using PseudoRandom = JARE.util.PseudoRandom;
using ThreadClass = JARE.support.ThreadClass;
using System.Threading;
namespace JARE.metaheuristics.moead
{
	
	[Serializable]
	public class pMOEAD:Algorithm, IThreadRunnable
	{
		/// <summary> Problem to solve</summary>
		private Problem m_problem;
		/// <summary> Population size</summary>
		private int m_populationSize;
		/// <summary> Stores the population</summary>
		private SolutionSet m_population;
		/// <summary> Number of threads</summary>
		private int m_numberOfThreads;
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
		internal int m_H;
		internal Solution[] m_indArray;
		internal System.String m_functionType;
		internal int m_evaluations;
		internal int m_maxEvaluations;
		/// <summary> Operators</summary>
		internal Operator m_crossover;
		internal Operator m_mutation;
		internal int m_id;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        //public HashMap < String, Object > m_map;
		public System.Collections.Generic.Dictionary<String,Object> m_map;
		internal pMOEAD m_parentThread;
		internal ThreadClass[] m_thread;
		
		internal System.String m_dataDirectory;

        //VLADA: OVO SAM NAPISAO PREVEO SKORO NAPAMET PA MU NE TREBA MNOGO VEROVATI!!!
        ////////internal CyclicBarrier m_barrier;
        internal System.Threading.AutoResetEvent m_barrier;
		
		internal long m_initTime;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public pMOEAD(Problem problem)
		{
			m_parentThread = null;
			m_problem = problem;
			
			m_functionType = "_TCHE1";
			
			m_id = 0;
		} // DMOEA
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		
		public pMOEAD(pMOEAD parentThread, Problem problem, int id, int numberOfThreads)
		{
			m_parentThread = parentThread;
			m_problem = problem;
			
			m_numberOfThreads = numberOfThreads;
			m_thread = new ThreadClass[m_numberOfThreads];
			
			m_functionType = "_TCHE1";
			
			m_id = id;
		} // DMOEA
		
		public virtual void  Run()
		{
			
			m_neighborhood = m_parentThread.m_neighborhood;
			m_problem = m_parentThread.m_problem;
			m_lambda = m_parentThread.m_lambda;
			m_population = m_parentThread.m_population;
			m_z = m_parentThread.m_z;
			m_indArray = m_parentThread.m_indArray;
			m_barrier = m_parentThread.m_barrier;
			
			
			int partitions = m_parentThread.m_populationSize / m_parentThread.m_numberOfThreads;
			
			m_evaluations = 0;
			m_maxEvaluations = m_parentThread.m_maxEvaluations / m_parentThread.m_numberOfThreads;
			
			
			try
			{
				//System.out.println("en espera: " + m_barrier.getNumberWaiting()) ;
                
                //VLADA: OVO SAM NAPISAO PREVEO SKORO NAPAMET PA MU NE TREBA MNOGO VEROVATI!!!
                //////m_barrier.await();
                m_barrier.WaitOne();
                //System.out.println("Running: " + m_id ) ;
			}
			catch (System.Threading.ThreadInterruptedException e)
			{
				// TODO Auto-generated catch block
				SupportClass.WriteStackTrace(e, Console.Error);
			}
            //VLADA - STAVLJENO POD KOMENTAR DOK NE ODLUCIMO STA CEMO DA RADIMO SA MULTITHREAD KONCEPTOM
            ////////catch (BrokenBarrierException e)
            ////////{
            ////////    // TODO Auto-generated catch block
            ////////    e.printStackTrace();
            ////////}
			
			
			int first;
			int last;
			
			first = partitions * m_id;
			if (m_id == (m_parentThread.m_numberOfThreads - 1))
			{
				last = m_parentThread.m_populationSize - 1;
			}
			else
			{
				last = first + partitions - 1;
			}
			
			System.Console.Out.WriteLine("Id: " + m_id + "  Partitions: " + partitions + " First: " + first + " Last: " + last);
			
			do 
			{
				// int[] permutation = new int[m_populationSize];
				//Utils.randomPermutation(permutation, m_populationSize);
				
				for (int i = first; i <= last; i++)
				{
					//int n = permutation[i]; // or int n = i;
					int n = i; // or int n = i;
					int type;
					double rnd = PseudoRandom.randDouble();
					
					// STEP 2.1. Mating selection based on probability
					if (rnd < m_parentThread.m_delta)
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
					this.matingSelection(p, n, 2, type);
					
					// STEP 2.2. Reproduction
					Solution child = null;
					Solution[] parents = new Solution[3];
					
					try
					{
						lock (m_parentThread)
						{
                            parents[0] = m_parentThread.m_population.getSolution(p[0]);
                            parents[1] = m_parentThread.m_population.getSolution(p[1]);
							parents[2] = m_parentThread.m_population.getSolution(n);
							// Apply DE crossover
							child = (Solution) m_parentThread.m_crossover.execute(new System.Object[]{m_parentThread.m_population.getSolution(n), parents});
						}
						// Apply mutation
						m_parentThread.m_mutation.execute(child);
						
						// Evaluation
						m_parentThread.m_problem.evaluate(child);
					}
					catch (SMException ex)
					{
                        ////////Logger.getLogger(typeof(pMOEAD).FullName).log(Level.SEVERE, null, ex);
					}
					
					m_evaluations++;
					
					// STEP 2.3. Repair. Not necessary
					
					// STEP 2.4. Update m_z
					updateReference(child);
					
					// STEP 2.5. Update of solutions
					updateOfSolutions(child, n, type);
				} // for
			}
			while (m_evaluations < m_maxEvaluations);
			
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - m_parentThread.m_initTime;
			System.Console.Out.WriteLine("Time thread " + m_id + ": " + estimatedTime);
		}
		
		public override SolutionSet execute()
		{
			m_parentThread = this;
			
			m_evaluations = 0;
			m_maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			m_populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			m_dataDirectory = this.getInputParameter("dataDirectory").ToString();
			m_numberOfThreads = ((System.Int32) this.getInputParameter("numberOfThreads"));
			
			m_thread = new ThreadClass[m_numberOfThreads];

            //m_barrier = new CyclicBarrier(m_numberOfThreads);
            //m_barrier = new AutoResetEvent(m_numberOfThreads);
			
			m_population = new SolutionSet(m_populationSize);
			m_indArray = new Solution[m_problem.NumberOfObjectives];
			
			m_T = 20;
			m_delta = 0.9;
			m_nr = 2;
			m_H = 33;
			
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
			
			m_initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			
			for (int i = 0; i < m_numberOfThreads; i++)
			{
				m_thread[i] = new ThreadClass(new System.Threading.ThreadStart(new pMOEAD(this, m_problem, i, m_numberOfThreads).Run), "pepe");
				m_thread[i].Start();
			}
			
			for (int i = 0; i < m_numberOfThreads; i++)
			{
				try
				{
					m_thread[i].Join();
					//long estimatedTime = System.currentTimeMillis() - initTime;
					//System.out.println("Time thread " + i +": " + estimatedTime) ;
				}
				catch (System.Threading.ThreadInterruptedException ex)
				{
                    ////////Logger.getLogger(typeof(pMOEAD).FullName).log(Level.SEVERE, null, ex);
				}
			}
			
			return m_population;
		}
		
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
				}
			} // for
		} // initNeighborhood
		
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
				
				System.Console.Out.WriteLine(m_dataDirectory);
				System.Console.Out.WriteLine(m_dataDirectory + "/" + dataFileName);
				
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
					System.Console.Out.WriteLine("initUniformWeight: fail when reading for file: " + m_dataDirectory + "/" + dataFileName);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			} // else
		} // initUniformWeight
		
		/// <summary> </summary>
		public virtual void  initPopulation()
		{
			for (int i = 0; i < m_populationSize; i++)
			{
				Solution newSolution = new Solution(m_problem);
				
				m_problem.evaluate(newSolution);
				//m_problem.evaluateConstraints(newSolution);
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

            ss = m_parentThread.m_neighborhood[cid].Length;
            while (list.Count < size)
            {
                if (type == 1)
                {
                    r = PseudoRandom.randInt(0, ss - 1);
                    p = m_parentThread.m_neighborhood[cid][r];
                    //p = population[cid].table[r];
                }
                else
                {
                    p = PseudoRandom.randInt(0, m_parentThread.m_populationSize - 1);
                    //p = int(population.size()*rnd_uni(&rnd_uni_init));
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
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'updateReference'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  updateReference(Solution individual)
		{
			lock (this)
			{
				for (int n = 0; n < m_parentThread.m_problem.NumberOfObjectives; n++)
				{
					if (individual.getObjective(n) < m_z[n])
					{
						m_parentThread.m_z[n] = individual.getObjective(n);
						
						m_parentThread.m_indArray[n] = individual;
					}
				}
			}
		} // updateReference
		
		/// <param name="individual">
		/// </param>
		/// <param name="id">
		/// </param>
		/// <param name="type">
		/// </param>
		internal virtual void  updateOfSolutions(Solution indiv, int id, int type)
		{
			// indiv: child solution
			// id:   the id of current subproblem
			// type: update solutions in - neighborhood (1) or whole population (otherwise)
			int size;
			int time;
			
			time = 0;
			
			if (type == 1)
			{
				size = m_parentThread.m_neighborhood[id].Length;
			}
			else
			{
				size = m_parentThread.m_population.size();
			}
			int[] perm = new int[size];
			
			Utils.randomPermutation(perm, size);
			
			for (int i = 0; i < size; i++)
			{
				int k;
				if (type == 1)
				{
					k = m_parentThread.m_neighborhood[id][perm[i]];
				}
				else
				{
					k = perm[i]; // calculate the values of objective function regarding the current subproblem
				}
				double f1, f2;
				
				f2 = fitnessFunction(indiv, m_parentThread.m_lambda[k]);
				lock (m_parentThread)
				{
					f1 = fitnessFunction(m_parentThread.m_population.getSolution(k), m_parentThread.m_lambda[k]);
					
					if (f2 < f1)
					{
						m_parentThread.m_population.replace(k, new Solution(indiv));
						//population[k].indiv = indiv;
						time++;
					}
				}
				// the maximal number of solutions updated is not allowed to exceed 'limit'
				if (time >= m_parentThread.m_nr)
				{
					return ;
				}
			}
		} // updateProblem
		
		internal virtual double fitnessFunction(Solution individual, double[] lambda)
		{
			double fitness;
			fitness = 0.0;
			
			if (m_parentThread.m_functionType.Equals("_TCHE1"))
			{
				double maxFun = - 1.0e+30;
				
				for (int n = 0; n < m_parentThread.m_problem.NumberOfObjectives; n++)
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
	} // pMOEAD
}