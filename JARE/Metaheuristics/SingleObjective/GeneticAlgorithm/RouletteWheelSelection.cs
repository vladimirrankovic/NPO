using System;
using System.Collections.Generic;
using System.Text;


namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
    public class RouletteWheelSelection
    {
        // random number generator
        private static Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="RouletteWheelSelection"/> class.
        /// </summary>
        public RouletteWheelSelection() { }

        /// <summary>
        /// Apply selection to the specified population.
        /// </summary>
        /// 
        /// <param name="chromosomes">Population, which should be filtered.</param>
        /// <param name="size">The amount of chromosomes to keep.</param>
        /// 
        /// <remarks>Filters specified population keeping only those chromosomes, which
        /// won "roulette" game.</remarks>
        /// 
        public void ApplySelection(JARE.Base.SolutionSet population, int size)
        {
            // new population, initially empty
            // size of current population
            int currentSize = population.size();
            JARE.Base.SolutionSet newPopulation = new JARE.Base.SolutionSet(500);
            // calculate summary fitness of current population
            double fitnessSum = 0;
            for (int i = 0; i < population.size(); i++)
            {
                fitnessSum += population.getSolution(i).Fitness;
            }
            
            //foreach ( IChromosome c in chromosomes )
            //{
            //    fitnessSum += c.Fitness;
            //}

            // create wheel ranges
            double[] rangeMax = new double[currentSize];
            double s = 0;
            int k = 0;

            //foreach ( IChromosome c in chromosomes )
            //{
            //    // cumulative normalized fitness
            //    s += ( c.Fitness / fitnessSum );
            //    rangeMax[k++] = s;
            //}

            for (int i = 0; i < population.size(); i++)
            {
                s += (population.getSolution(i).Fitness / fitnessSum);
                rangeMax[k++] = s;
            }
           

            // select chromosomes from old population to the new population
            for (int j = 0; j < size; j++)
            {
                // get wheel value
                double wheelValue = rand.NextDouble();
                // find the chromosome for the wheel value
                for (int i = 0; i < currentSize; i++)
                {
                    if (wheelValue <= rangeMax[i])
                    {
                        
                        // add the chromosome to the new population
                        newPopulation.add(population.getSolution(i));
                        //newPopulation.Add( ((IChromosome) chromosomes[i]).Clone( ) );
                        break;
                    }
                }
                if (newPopulation.size() > 100) Console.WriteLine();
            }

            // empty current population
            population.clear();
            for (int i = 0; i < newPopulation.size(); i++)
            {
                population.add(newPopulation.getSolution(i)); 
            }
        }
    }
}
