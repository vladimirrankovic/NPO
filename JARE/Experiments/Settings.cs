/// <summary> Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// Abstract Settings class
/// </version>
using System;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using SMException = JARE.util.SMException;
namespace JARE.experiments
{
	
	public abstract class Settings
	{
		/// <summary> Change the problem to solve</summary>
		/// <param name="problem">
		/// </param>
		virtual internal Problem Problem
		{
			set
			{
				m_problem = value;
			}
			// setProblem
			
		}
		protected internal Problem m_problem;
		public System.String m_paretoFrontFile;
		
		/// <summary> Constructor</summary>
		public Settings()
		{
		} // Constructor
		
		/// <summary> Constructor</summary>
		public Settings(Problem problem)
		{
			m_problem = problem;
		} // Constructor
		
		/// <summary> Default configure method</summary>
		/// <returns> A problem with the default configuration
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		abstract public Algorithm configure();
		
		/// <summary> Configure method. Change the default configuration</summary>
		/// <param name="settings">
		/// </param>
		/// <returns> A problem with the settings indicated as argument
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		/// <throws>  ClassNotFoundException  </throws>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //public Algorithm configure(System.Collections.Specialized.NameValueCollection settings)
        //{

        //    if (settings != null)
        //    {

        //        System.Reflection.FieldInfo[] fields = this.GetType().GetFields();

        //        for (int i = 0; i < fields.Length; i++)
        //        {
        //            if (fields[i].Name.EndsWith("_"))
        //            {
        //                // it is a configuration field             	
        //                // The configuration field is an integer
        //                if (fields[i].FieldType.Equals(typeof(int)) || fields[i].FieldType.Equals(typeof(System.Int32)))
        //                {

        //                    int value = System.Int32.Parse(settings[fields[i].Name] == null ? "" + (int)fields[i].GetValue(this) : settings[fields[i].Name]);

        //                    fields[i].SetValue(this, value);
        //                }
        //                else if (fields[i].FieldType.Equals(typeof(double)) || fields[i].FieldType.Equals(typeof(System.Double)))
        //                {
        //                    // The configuration field is a double
        //                    double value = System.Double.Parse(settings[fields[i].Name] == null ? "" + (double)fields[i].GetValue(this) : settings[fields[i].Name]);

        //                    if (fields[i].Name.Equals("m_mutationProbability") && value == 0)
        //                    {
        //                        //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //                        if ((m_problem.SolutionType.GetType() == System.Type.GetType("JARE.Base.solutionType.RealSolutionType")) || (m_problem.SolutionType.GetType() == System.Type.GetType("JARE.Base.solutionType.ArrayRealSolutionType")))
        //                        {
        //                            value = 1.0 / m_problem.NumberOfVariables;
        //                        }
        //                        else
        //                        {
        //                            //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //                            if (m_problem.SolutionType.GetType() == System.Type.GetType("JARE.Base.solutionType.BinarySolutionType") || m_problem.SolutionType.GetType() == System.Type.GetType("JARE.Base.solutionType.BinaryRealSolutionType"))
        //                            {
        //                                int length = m_problem.NumberOfBits;

        //                                value = 1.0 / length;
        //                                System.Console.Out.WriteLine("La probabilidad es : " + value);
        //                            }
        //                            else
        //                            {
        //                                int length = 0;
        //                                for (int j = 0; j < m_problem.NumberOfVariables; j++)
        //                                {
        //                                    length += m_problem.getLength(j);
        //                                }
        //                                value = 1.0 / length;
        //                            }
        //                        }
        //                        fields[i].SetValue(this, value);
        //                    }
        //                    // if
        //                    else
        //                    {
        //                        fields[i].SetValue(this, value);
        //                    }
        //                }
        //                else
        //                {
        //                    System.Object value = settings[fields[i].Name] == null ? null : settings[fields[i].Name];

        //                    if (value != null)
        //                    {
        //                        if (fields[i].FieldType.Equals(typeof(JARE.Base.operators.crossover.Crossover)))
        //                        {
        //                            System.Object value2 = CrossoverFactory.getCrossoverOperator((System.String)value, settings);
        //                            value = value2;
        //                        }

        //                        if (fields[i].FieldType.Equals(typeof(JARE.Base.operators.mutation.Mutation)))
        //                        {
        //                            System.Object value2 = MutationFactory.getMutationOperator((System.String)value, settings);
        //                            value = value2;
        //                        }

        //                        fields[i].SetValue(this, value);
        //                    }
        //                }
        //            }
        //        } // for

        //        // At this point all the fields have been read from the properties
        //        // parameter. Those fields representing crossover and mutations should also
        //        // be initialized. However, there is still mandatory to configure them
        //        for (int i = 0; i < fields.Length; i++)
        //        {
        //            if (fields[i].FieldType.Equals(typeof(JARE.Base.operators.crossover.Crossover)) || fields[i].FieldType.Equals(typeof(JARE.Base.operators.mutation.Mutation)))
        //            {
        //                Operator op = (Operator)fields[i].GetValue(this);
        //                // This field stores a crossover operator
        //                System.String tmp = fields[i].Name;
        //                System.String aux = fields[i].Name.Substring(0, (tmp.Length - 1) - (0));

        //                for (int j = 0; j < fields.Length; j++)
        //                {
        //                    if (i != j)
        //                    {
        //                        if (fields[j].Name.StartsWith(aux))
        //                        {
        //                            // The field is a configuration parameter of the crossover
        //                            tmp = fields[j].Name.Substring(aux.Length, (fields[j].Name.Length - 1) - (aux.Length));

        //                            if ((fields[j].GetValue(this) != null))
        //                            {
        //                                System.Console.Out.WriteLine(fields[j].Name);
        //                                if (fields[j].FieldType.Equals(typeof(int)) || fields[j].FieldType.Equals(typeof(System.Int32)))
        //                                {
        //                                    op.setParameter(tmp, (int)fields[j].GetValue(this));
        //                                }
        //                                else if (fields[j].FieldType.Equals(typeof(double)) || fields[j].FieldType.Equals(typeof(System.Double)))
        //                                {
        //                                    op.setParameter(tmp, (double)fields[j].GetValue(this));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        //At this point, we should compare if the pareto front have been added
        //        m_paretoFrontFile = settings["m_paretoFrontFile"] == null ? "" : settings["m_paretoFrontFile"];
        //    }

        //    return configure();
        //} // configure
	} // Settings
}