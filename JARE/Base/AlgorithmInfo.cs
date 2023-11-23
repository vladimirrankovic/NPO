using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JARE.Base
{
	public class AlgorithmInfo
	{
        private SolutionSet m_bestFront;
        private int m_numberOfVariables;
        private String m_info;

        public SolutionSet BestFront
        {
            get { return m_bestFront; }
            set { m_bestFront = value; }
        }

        public int NumberOfVariables
        {
            get { return m_numberOfVariables; }
            set { m_numberOfVariables = value; }
        }
        
        public String Info
        {
            get { return m_info; }
            set { m_info = value; }
        }

        public AlgorithmInfo()
        {
        }

        public AlgorithmInfo(SolutionSet bestFront, String info, int numberOfVariables)
        {
            m_bestFront = bestFront;
            m_info = info;
            m_numberOfVariables = numberOfVariables;
        }

	}
}
