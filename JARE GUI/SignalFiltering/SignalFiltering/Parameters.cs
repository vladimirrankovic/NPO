
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;


namespace SignalFilteringGUI.Util
{
      public class OptimizationParameters
    {
        private int populationSize = 50;
        private int generationsNumber = 5;
        private double mutationProbability;// = 0.2;
        private double mutationPerturbation;// = 0.1;
        private double crossoverProbability;// = 1.0;

        private int maxPlateauGenerations; //Maximal number of successive generations without quality improvement
        private double plateauTolerance; //Minimal improvement of quality indicator

        public int PopulationSize { get { return populationSize; } set { populationSize = value; } }
        public int Generations { get { return generationsNumber; } set { generationsNumber = value; } }
        public double MutationProbability { get { return mutationProbability; } set { mutationProbability = value; } }
        public double MutationPerturbation { get { return mutationPerturbation; } set { mutationPerturbation = value; } }
        public double CrossoverProbability { get { return crossoverProbability; } set { crossoverProbability = value; } }
         public int MaxPlateauGenerations { get { return maxPlateauGenerations; } set { maxPlateauGenerations = value; } }
        public double PlateauTolerance { get { return plateauTolerance; } set { plateauTolerance = value; } }
 

         public OptimizationParameters(int populationSize, int iterations, double mutationProbability,
            double mutationPerturbation, double crossoverProbability, int maxPlateauGenerations, double plateauTolerance)
        {
            PopulationSize = populationSize;
            Generations = iterations;
            MutationProbability = mutationProbability;
            MutationPerturbation = mutationProbability;
            CrossoverProbability = crossoverProbability;
            MaxPlateauGenerations = maxPlateauGenerations;
            PlateauTolerance = plateauTolerance;
        }
        public OptimizationParameters()
        {
            Init();
        }
        void Init()
        {            
            PopulationSize = 50;
            Generations = 10;
            MutationProbability = 0.05;
            MutationPerturbation = 0.1;
            CrossoverProbability = 0.9;
            MaxPlateauGenerations = 10;
            PlateauTolerance = 0.0005;
         }
    }
}
