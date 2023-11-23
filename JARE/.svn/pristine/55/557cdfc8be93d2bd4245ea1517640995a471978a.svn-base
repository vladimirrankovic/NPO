/// <summary> SolutionSet.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using System.Collections;
using System.Collections.Generic;
using Configuration = JARE.util.Configuration;
namespace JARE.Base
{
	
	/// <summary> Class representing a SolutionSet (a set of solutions)</summary>
	[Serializable]
	public class SolutionSet
	{
        //private void  InitBlock()
        //{
        //    return solutionsList.iterator(); // iterator   
        //}
		/// <summary> Returns the maximum capacity of the solution set</summary>
		/// <returns> The maximum capacity of the solution set
		/// </returns>
		virtual public int MaxSize
		{
			get
			{
				return m_capacity;
			}
		}
		
		/// <summary> Stores a list of <code>solution</code> objects.</summary>
        protected internal List <Solution> m_solutionsList;
        
		
		/// <summary> Maximum size of the solution set </summary>
		private int m_capacity = 0;
		
		/// <summary> Constructor.
		/// Creates an unbounded solution set.
		/// </summary>
		public SolutionSet()
		{
			m_solutionsList = new List<Solution>();
		} 
		
		/// <summary> Creates a empty solutionSet with a maximum capacity.</summary>
		/// <param name="maximumSize">Maximum size.
		/// </param>
		public SolutionSet(int maximumSize)
		{
			m_solutionsList = new List<Solution>();
			m_capacity = maximumSize;
		} 
		
        /** 
   * Returns the number of solutions in the SolutionSet.
   * @return The size of the SolutionSet.
   */  
  public int size(){
    return m_solutionsList.Count;
  } 

		/// <summary> Inserts a new solution into the SolutionSet. </summary>
		/// <param name="solution">The <code>Solution</code> to store
		/// </param>
		/// <returns> True If the <code>Solution</code> has been inserted, false 
		/// otherwise. 
		/// </returns>
		public virtual bool add(Solution solution)
		{
            if (m_solutionsList.Count == m_capacity)
            {
                Console.WriteLine("The population is full");
                Console.WriteLine("Capacity is : " + m_capacity.ToString());
                Console.WriteLine("\t Size is: " + m_solutionsList.Count.ToString());
                return false;
            } 
			
			m_solutionsList.Add(solution);
			return true;
		} // add
		
		/// <summary> Returns the ith solution in the set.</summary>
		/// <param name="i">Position of the solution to obtain.
		/// </param>
		/// <returns> The <code>Solution</code> at the position i.
		/// </returns>
		/// <throws>  IndexOutOfBoundsException. </throws>
		public virtual Solution getSolution(int i)
		{
            // TODO zar nece on sam da digne exception kad je indeks out of bound

			if (i >= m_solutionsList.Count)
			{
				throw new System.IndexOutOfRangeException("Index out of Bound " + i);
			}
			return m_solutionsList[i];
		} 
		
		/// <summary> Sorts a SolutionSet using a <code>Comparator</code>.</summary>
		/// <param name="comparator"><code>Comparator</code> used to sort.
		/// </param>
        public virtual void sort(System.Collections.Generic.IComparer<JARE.Base.Solution> comparator)
		{
            // Visnja: bice koriscen sort metod genericke liste 
            //JARE.support.CollectionsSupport.Sort(m_solutionsList, comparator);
            m_solutionsList.Sort(comparator);
		}
		
		
		/// <summary> Writes the objective funcion values of the <code>Solution</code> 
		/// objects into the set in a file.
		/// </summary>
		/// <param name="path">The output file name
		/// </param>
		public virtual void printObjectivesToFile(string path)
		{
			try
			{
				/* Open the file */
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
				System.IO.FileStream fos = new System.IO.FileStream(path, System.IO.FileMode.Create);
				System.IO.StreamWriter osw = new System.IO.StreamWriter(fos, System.Text.Encoding.Default);
				System.IO.StreamWriter bw = new System.IO.StreamWriter(osw.BaseStream, osw.Encoding);

				
                //foreach (Solution s in m_solutionsList)
                //{
                //    bw.WriteLine(s.ToString()+",");
                //}

                // visnja: formatirano za .csv fajl
                for (int i = 0; i < m_solutionsList.Count; i++)
                {
                    //if (this.vector[i].getFitness()<1.0) {
                    for (int j = 0; j < m_solutionsList[i].NumberOfObjectives(); j++)
                    {
                        bw.Write(m_solutionsList[i].getObjective(j).ToString() + ",");
                    }
                    bw.WriteLine();
                    //UPGRADE_TODO: Method 'java.io.BufferedWriter.newLine' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                    
                    //}
                }
				
				/* Close the file */
				bw.Close();
			}
			catch (System.IO.IOException e)
			{
				Console.WriteLine("Error acceding to the file");
				Console.WriteLine(e.Message);
			}
		} 
		
		/// <summary> Writes the decision variable values of the <code>Solution</code>
		/// solutions objects into the set in a file.
		/// </summary>
		/// <param name="path">The output file name
		/// </param>
		public virtual void printVariablesToFile(string path)
		{
			try
			{
				/* Open the file */
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
				System.IO.FileStream fos = new System.IO.FileStream(path, System.IO.FileMode.Create);
				System.IO.StreamWriter osw = new System.IO.StreamWriter(fos, System.Text.Encoding.Default);
				System.IO.StreamWriter bw = new System.IO.StreamWriter(osw.BaseStream, osw.Encoding);
				
               
				int numberOfVariables = m_solutionsList[0].DecisionVariables.Length; 
				//for (int i = 0; i < solutionsList.size(); i++)
                foreach (Solution s in m_solutionsList)
				{
					for (int j = 0; j < numberOfVariables; j++)
                    {
                        bw.Write(s.DecisionVariables[j].ToString() + ","); 
                    }
						
					bw.WriteLine();
				}
				
				/* Close the file */
				bw.Close();
			}
			catch (System.IO.IOException e)
			{
				Console.WriteLine("Error acceding to the file");
				Console.WriteLine(e.Message);
			}
		} 
		
		/// <summary> Empties the SolutionSet</summary>
		public virtual void clear()
		{
			m_solutionsList.Clear();
		} // clear
		
		/// <summary> Deletes the <code>Solution</code> at position i in the set.</summary>
		/// <param name="i">The position of the solution to remove.
		/// </param>
		public virtual void remove (int i)
		{
			if (i > m_solutionsList.Count - 1)
			{
				Console.WriteLine("Size is: " + this.size());
			} 
			m_solutionsList.RemoveAt(i);
		} 
		
		
		/// <summary> Returns an <code>Iterator</code> to access to the solution set list.</summary>
		/// <returns> the <code>Iterator</code>.
		/// </returns>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        
        //public Iterator < Solution > iterator() 
		
		/// <summary> Returns a new <code>SolutionSet</code> which is the result of the union
		/// between the current solution set and the one passed as a parameter.
		/// </summary>
		/// <param name="solutionSet">SolutionSet to join with the current solutionSet.
		/// </param>
		/// <returns> The result of the union operation.
		/// </returns>
		public virtual SolutionSet union(SolutionSet solutionSet)
		{
			//Check the correct size. In development ////////////////////////////////////////////////////////////////////////////////////////////
			int newSize = this.size() + solutionSet.size();
			if (newSize < m_capacity)
				newSize = m_capacity;
			
			//Create a new population 
			SolutionSet union = new SolutionSet(newSize);
			for (int i = 0; i < this.size(); i++)
			{
				union.add(this.m_solutionsList[i]);
			} // for
			
			for (int i = this.size(); i < (this.size() + solutionSet.size()); i++)
			{
				union.add(solutionSet.m_solutionsList[i - this.size()]);
			} // for
			
			return union;
		} // union                   
		
		/// <summary> Replaces a solution by a new one</summary>
		/// <param name="position">The position of the solution to replace
		/// </param>
		/// <param name="solution">The new solution
		/// </param>
		public virtual void  replace(int position, Solution solution)
		{
			if (position > this.size())
			{
				m_solutionsList.Add(solution);
			} // if 

			m_solutionsList.RemoveAt(position);
            m_solutionsList.Insert(position, solution);
			
		} // replace
		
		/// <summary> Copies the objectives of the solution set to a matrix</summary>
		/// <returns> A matrix containing the objectives
		/// </returns>
		public virtual double[][] writeObjectivesToMatrix()
		{
			if (this.size() == 0)
			{
				return null;
			}
			double[][] objectives;
			objectives = new double[this.size()][];
			for (int i = 0; i < this.size(); i++)
			{
				objectives[i] = new double[ getSolution(0).NumberOfObjectives()] ;
			}
			for (int i = 0; i < this.size(); i++)
			{
				for (int j = 0; j < getSolution(0).NumberOfObjectives(); j++)
				{
					objectives[i][j] = getSolution(i).getObjective(j);
				}
			}
			return objectives;
		} 
	} 
}