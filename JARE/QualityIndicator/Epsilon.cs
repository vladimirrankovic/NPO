/// <summary> Epsilon.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.qualityIndicator
{
	
	/*===========================================================================*
	* This class implements the unary epsilon additive indicator as proposed in
	* E. Zitzler, E. Thiele, L. Laummanns, M., Fonseca, C., and Grunert da Fonseca.
	* V (2003): Performance Assesment of Multiobjective Optimizers: An Analysis and
	* Review. The code is the a Java version of the orginal metric implementation 
	* by Eckart Zitzler.
	* It can be used also as a command line program just by typing
	* $java Epsilon <solutionFrontFile> <trueFrontFile> <numberOfOjbectives>
	* Reference: 
	*           E. Zitzler, L. Thiele, M. Laummanns, C.M. Fonseca, and Grunert da Fonseca.
	*           Performance Assesment of Multiobjective Optimizars: An Analysis and
	*           Review 
	*           IEEE Transactions on Evolutionary Computation, vol. 3, no. 4, 
	*           pp. 257-271, 2003.
	*/
	
	public class Epsilon
	{
		
		/* stores the number of objectives */
		internal int m_dim;
		/* obj_[i]=0 means objective i is to be minimized. This code always suposse
		* the minimization of all the objectives
		*/
		internal int[] m_obj; /* obj_[i] = 0 means objective i is to be minimized */
		/* method_ = 0 means apply additive epsilon and method_ = 1 means multiplicative
		* epsilon. This code always apply additive epsilon
		*/
		internal int m_method;
		/* stores a reference to  qualityIndicatorUtils */
        internal JARE.qualityIndicator.util.MetricsUtil m_utils = new JARE.qualityIndicator.util.MetricsUtil();
		
		
		/// <summary> Returns the epsilon indicator.</summary>
		/// <param name="b.">True Pareto front
		/// </param>
		/// <param name="a.">Solution front
		/// </param>
		/// <returns> the value of the epsilon indicator
		/// </returns>
		internal virtual double epsilon(double[][] b, double[][] a, int dim)
		{
			int i, j, k;
			double eps, eps_j = 0.0, eps_k = 0.0, eps_temp;
			
			m_dim = dim;
			set_params();
			
			if (m_method == 0)
			{
				eps = Double.MinValue;
			}
			else
				eps = 0;
			
			for (i = 0; i < a.Length; i++)
			{
				for (j = 0; j < b.Length; j++)
				{
					for (k = 0; k < m_dim; k++)
					{
						switch (m_method)
						{
							
							case 0: 
								if (m_obj[k] == 0)
									eps_temp = b[j][k] - a[i][k];
								//eps_temp = b[j * dim_ + k] - a[i * dim_ + k];
								else
									eps_temp = a[i][k] - b[j][k];
								//eps_temp = a[i * dim_ + k] - b[j * dim_ + k];
								break;
							
							default: 
								if ((a[i][k] < 0 && b[j][k] > 0) || (a[i][k] > 0 && b[j][k] < 0) || (a[i][k] == 0 || b[j][k] == 0))
								{
									//if ( (a[i * dim_ + k] < 0 && b[j * dim_ + k] > 0) ||
									//     (a[i * dim_ + k] > 0 && b[j * dim_ + k] < 0) ||
									//     (a[i * dim_ + k] == 0 || b[j * dim_ + k] == 0)) {
									System.Console.Error.WriteLine("error in data file");
									System.Environment.Exit(0);
								}
								if (m_obj[k] == 0)
									eps_temp = b[j][k] / a[i][k];
								//eps_temp = b[j * dim_ + k] / a[i * dim_ + k];
								else
									eps_temp = a[i][k] / b[j][k];
								//eps_temp = a[i * dim_ + k] / b[j * dim_ + k];
								break;
							
						}
						if (k == 0)
							eps_k = eps_temp;
						else if (eps_k < eps_temp)
							eps_k = eps_temp;
					}
					if (j == 0)
						eps_j = eps_k;
					else if (eps_j > eps_k)
						eps_j = eps_k;
				}
				if (i == 0)
					eps = eps_j;
				else if (eps < eps_j)
					eps = eps_j;
			}
			return eps;
		} // epsilon
		
		/// <summary> Established the params by default</summary>
		internal virtual void  set_params()
		{
			int i;
			m_obj = new int[m_dim];
			for (i = 0; i < m_dim; i++)
			{
				m_obj[i] = 0;
			}
			m_method = 0;
		} // set_params
		
		
		/// <summary> Returns the additive-epsilon value of the paretoFront. This method call to the
		/// calculate epsilon-indicator one
		/// </summary>
		/// <param name="paretoFront">The pareto front
		/// </param>
		/// <param name="paretoTrueFront">The true pareto front
		/// </param>
		/// <param name="numberOfObjectives">Number of objectives of the pareto front
		/// </param>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			double ind_value;
			
			if (args.Length < 2)
			{
				System.Console.Error.WriteLine("Error using delta. Type: \n java AdditiveEpsilon " + "<FrontFile>" + "<TrueFrontFile> + <numberOfObjectives>");
				System.Environment.Exit(1);
			}
			
			Epsilon qualityIndicator = new Epsilon();
			double[][] solutionFront = qualityIndicator.m_utils.readFront(args[0]);
			double[][] trueFront = qualityIndicator.m_utils.readFront(args[1]);
			//qualityIndicator.dim_ = trueParetoFront[0].length;
			//qualityIndicator.set_params();
			
			ind_value = qualityIndicator.epsilon(trueFront, solutionFront, System.Int32.Parse(args[2]));
			
			System.Console.Out.WriteLine(ind_value);
		} // main
	} // Epsilon
}