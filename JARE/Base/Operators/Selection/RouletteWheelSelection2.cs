/// <summary> RouletteWheelSelection2.css</summary>
/// <author>  Vladimir Rankovic
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
    public class RouletteWheelSelection2 : Selection
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
            //double fitnessSum = 0;
            double max = double.MinValue;
            for (int i = 0; i < population.size(); i++)
            {
                //Find the worst fitness (the worst fitness is max fitness if fitness function is Min(f))
                if (max < population.getSolution(i).getObjective(0)) max = population.getSolution(i).getObjective(0);
                //if (max < population.getSolution(i).Fitness) max = population.getSolution(i).Fitness;
                //fitnessSum += population.getSolution(i).Fitness;
            }

            // create wheel ranges
            double[] fitnessRange = new double[currentSize];
            double s = 0;
            int k = 0;
            //double minimumAbs = Math.Abs(minimum);
            for (int i = 0; i < population.size(); i++)
            {
                //s += (population.getSolution(i).Fitness / fitnessSum);
                s += System.Math.Abs(population.getSolution(i).getObjective(0) - max);
                fitnessRange[k++] = s;
            }

            // select chromosomes from old population to the new population
            for (int j = 0; j < size; j++)
            {
                // get wheel value
                //double wheelValue = PseudoRandom.randDouble(0.0, 1.0);
                double wheelValue = PseudoRandom.randDouble(0.0, fitnessRange[currentSize - 1]);
                // find the chromosome for the wheel value
                for (int i = 0; i < currentSize; i++)
                {
                    if (wheelValue <= fitnessRange[i])
                    {
                        // add the chromosome to the new population
                        newPopulation.add(population.getSolution(i));
                        break;
                    }
                }
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