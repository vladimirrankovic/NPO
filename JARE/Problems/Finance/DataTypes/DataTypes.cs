using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.problems.Finance;

namespace JARE.problems.Finance.DataTypes
{

    public enum DecisionVariableType
    {
        capitalInvested = 0,    //wi
        constantCapitalInvested = 1,
        sharesInvestedRealValues = 2,
        sharesInvestedIntValues = 3      //ni - each variable represents number of shares invested in single asset
    }

    public abstract class EvaluationCriteria
    {
        protected executionType m_executionType;

        public executionType ExecutionType
        {
            get { return m_executionType; }
            set { m_executionType = value; }
        }

        public enum executionType
        {
            singleThread,
            viaBinder
        }

        public EvaluationCriteria()
        {
            m_executionType = executionType.singleThread;
        }

        public EvaluationCriteria(executionType executionType)
        {
            m_executionType = executionType;
        }
   }

    public class EvaluationCriteriaTimeSeriesBased:EvaluationCriteria
    {
        protected int m_timeSeriesSetID;

        public EvaluationCriteriaTimeSeriesBased():base()
        {
            m_timeSeriesSetID = 0;
        }

        public EvaluationCriteriaTimeSeriesBased(executionType executionType, int timeSeriesSetID = 0):base(executionType)
        {
            m_timeSeriesSetID = timeSeriesSetID;
        }

        public int TimeSeriesSetID
        {
            get { return m_timeSeriesSetID; }
            set { m_timeSeriesSetID = value; }
        }
    }

    public class EvaluationCriteriaParameterBased : EvaluationCriteria
    {
        public enum criteriaType
        {
            none = 0,
            YieldToMaturity = 1, //maximal
            SCR //minimal
        }

        private criteriaType m_evaluationCriteria;

        public criteriaType CriteriaType
        {
            get { return m_evaluationCriteria; }
            set { m_evaluationCriteria = value; }
        }

        public EvaluationCriteriaParameterBased()
            : base()
        {
            m_evaluationCriteria = criteriaType.YieldToMaturity;
        }

        public EvaluationCriteriaParameterBased(criteriaType criteriaType, executionType executionType)
            : base(executionType)
        {
            m_evaluationCriteria = criteriaType;
        }    
    }

    public class EvaluationCriteriaReturnBased : EvaluationCriteriaTimeSeriesBased
    {
        public enum criteriaType
        {
            none = 0,
            AverageReturn = 1, //maximal
            StandardDeviation = 2, //minimal
            SharpIndex = 3, //maximal
            Sortino = 4, //maximal
            VaR = 5, //minimal
            cVaR = 6, //minimal
            ExpWeightedVaR = 7, //minimal
            ExpWeightedcVaR = 8, //minimal
            Skewness = 9, //maximal
            Kurtosis = 10, //minimal
            VaRGarch = 11, //minimal
            SharpVaR = 12, //maximal
            SharpCVar = 13, //maximal
            ReplicationStDev=14, //minimal
            CR_VaR = 15, //minimal
            CR_GARCHVaR = 16, //minimal
            DrawDown //minimal
        }

        private criteriaType m_evaluationCriteria;

        public criteriaType CriteriaType
        {
            get { return m_evaluationCriteria; }
            set { m_evaluationCriteria = value; }
        }

        public EvaluationCriteriaReturnBased()
            : base()
        {
            m_evaluationCriteria = criteriaType.AverageReturn;
        }

        public EvaluationCriteriaReturnBased(criteriaType criteriaType, executionType executionType, int timeSeriesSetID = 0)
            : base()
        {
            m_evaluationCriteria = criteriaType;
        }
    }

    public class EvaluationCriteriaCreditExposureBased : EvaluationCriteriaTimeSeriesBased
    {
     
        public enum criteriaType
        {
            none = 0,
            minimalAverageCreditExposure = 1,
            minimalStandardDeviationCreditExposure = 2,
        }

        private criteriaType m_evaluationCriteria;

        public criteriaType CriteriaType
        {
            get { return m_evaluationCriteria; }
            set { m_evaluationCriteria = value; }
        }

        public EvaluationCriteriaCreditExposureBased()
            : base()
        {
            m_evaluationCriteria = criteriaType.minimalAverageCreditExposure;
        }

        public EvaluationCriteriaCreditExposureBased(criteriaType criteriaType, executionType executionType, int timeSeriesSetID = 0, int secondaryTimeSeriesSetID = 0)
            : base()
        {
            m_evaluationCriteria = criteriaType;
        }
    }
}
