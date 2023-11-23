/// <summary> RouletteWheelSelection.css</summary>
/// <author>  Visnja Simic
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.selection
{

    /// <summary> This class implements a roulette-wheel selection operator used for selecting two
	/// random parents
	/// </summary>
	[Serializable]
    public class RouletteWheelSelection : Selection
	{
		/// <summary> Performs the operation</summary>
		/// <param name="object">Object representing a SolutionSet.
		/// </param>
		/// <returns> an object representing an array with the selected parents
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			SolutionSet population = (SolutionSet) obj;
            
            // size of current population
            int currentSize = population.size();
            int size = (int) getParameter("size");
            if (size == 0) size = currentSize;
            // new population, initially empty
            JARE.Base.SolutionSet newPopulation = new JARE.Base.SolutionSet(size);
            
            // calculate summary fitness of current population
            double fitnessSum = 0;
            double minimum = double.MaxValue;
            for (int i = 0; i < population.size(); i++)
            {
                if (minimum > population.getSolution(i).Fitness) minimum = population.getSolution(i).Fitness;
                //fitnessSum += population.getSolution(i).Fitness;
            }

            // create wheel ranges
            double[] rangeMax = new double[currentSize];
            double s = 0;
            int k = 0;
            //double minimumAbs = Math.Abs(minimum);
            for (int i = 0; i < population.size(); i++)
            {
                //s += (population.getSolution(i).Fitness / fitnessSum);
                s += population.getSolution(i).Fitness - minimum;
                rangeMax[k++] = s;
            }

            // select chromosomes from old population to the new population
            for (int j = 0; j < size; j++)
            {
                // get wheel value
                //double wheelValue = PseudoRandom.randDouble(0.0, 1.0);
                double wheelValue = PseudoRandom.randDouble(0.0, rangeMax[population.size()-1]);
                // find the chromosome for the wheel value
                for (int i = 0; i < currentSize; i++)
                {
                    if (wheelValue <= rangeMax[i])
                    {
                        // add the chromosome to the new population
                        newPopulation.add(population.getSolution(i));
                        break;
                    }
                }
                if (newPopulation.size() > size) Console.WriteLine();
            }

            // empty current population
            population.clear();
            for (int i = 0; i < newPopulation.size(); i++)
            {
                population.add(newPopulation.getSolution(i));
            }

            return population;
        } // Execute     
	} // RandomSelection
}