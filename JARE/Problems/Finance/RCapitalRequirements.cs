/// <summary> RCapitalRequirements</summary>
/// <author>  Vladimir Ranković
/// </author>
/// <version>  1.0  
/// </version>
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;
using JARE.problems.Finance.Parameters;
using PortfolioOptimizationGA = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGA;
using RDotNet;


namespace JARE.problems
{
    public class RCapitalRequirements : RCalculation
    {
        private AssetSet timeSeriesSet;
        private AssetSet stressTimeSeriesSet;
        DateTime CapitalRequirementsDate;
        int evaluationPeriod;
        int backtestingPeriod;
        int ReturnPeriod;
        double VaRTreshold;
        DecisionVariableType variableType;
        bool stressPart = true;

        public RCapitalRequirements(AssetSet timeSeriesSet, DateTime CapitalRequirementsDate, int evaluationPeriod, int backtestingPeriod,
                                    int ReturnPeriod, DecisionVariableType variableType, double varTreshold, string RScriptPath, 
            ExecutionType executionType, List<EvaluationCriteria> evaluationCriteria, AssetSet stressTimeSeriesSet) : base(evaluationCriteria, RScriptPath, executionType)
        {
            this.timeSeriesSet = timeSeriesSet;
            this.evaluationPeriod = evaluationPeriod;
            this.backtestingPeriod = backtestingPeriod;
            this.ReturnPeriod = ReturnPeriod;
            this.CapitalRequirementsDate = CapitalRequirementsDate;
            this.variableType = variableType;
            if (stressTimeSeriesSet == null) stressPart = false;
            else this.stressTimeSeriesSet = stressTimeSeriesSet;

            VaRTreshold = varTreshold;

            CalculationPreparation(timeSeriesSet);
        }

        public void Init()
        {
            base.Init();
            CalculationPreparation(timeSeriesSet);
        }


        #region R
        //R preparation
        protected void CalculationPreparation(AssetSet timeSeriesSet)
        {
            //R Initialization
            base.Init();

            //int CapitalRequirementsDateIndex;
            //timeSeriesSet.GetItemIndex(CapitalRequirementsDate, out CapitalRequirementsDateIndex);
            int TimeSeriesLength = evaluationPeriod + backtestingPeriod + 1;//Uvecanje za 1 zbog racunanja prinosa
            string timeSeriesFileName = "\\timeSeries.csv";
            timeSeriesSet.DumpToCsv(RScriptPath + timeSeriesFileName, CapitalRequirementsDate, TimeSeriesLength, false, ',');
            if(stressPart)
            {
                timeSeriesFileName = "\\timeSeriesStress.csv";
                stressTimeSeriesSet.DumpToCsv(RScriptPath + timeSeriesFileName, CapitalRequirementsDate, TimeSeriesLength, false, ',');
            }

            if (usingRScriptFile)
            {
                //PRINT SCRIPT FILE
                string scriptName = "\\Script.R";
                string scriptPath = RScriptPath + scriptName;
                printRScript(scriptPath);

                if (executionType == ExecutionType.viaBinder)
                {
                    UploadFiles(RScriptPath, "\\timeSeries.csv");
                    UploadFiles(RScriptPath, "\\Script.R");
                    if (stressPart) UploadFiles(RScriptPath, "\\timeSeriesStress.csv");
                }
            }
            else
            {
                engine.EagerEvaluate("d=read.table(\"timeSeries.csv\", header=FALSE, sep=',')");
                engine.EagerEvaluate("m=data.matrix(d,rownames.force=NA)[,-1]");
            }
        }

        // VaR from GARCH
        public void CalculateCapitalRequirements(double[] weights, out double CapitalRequirements, out int ViolationsCount, out int CorrectionCount)
        {

            string strPonders = string.Empty;
            strPonders = arrayToString(weights);

            //string pondersFileName = "ponders.csv";
            string pondersFileName = "weights.dat";

            //PRINT PONDERS TO FILE
            string path = string.Empty;
            StreamWriter writer = new StreamWriter(path + pondersFileName, false);
            //writer.WriteLine(strPonders);
            //writer.Dispose();

            for (int i = 0; i < weights.Length; i++)
            {
                writer.WriteLine(weights[i]);
            }
            writer.Dispose();
            //

            try
            {
                engine.EagerEvaluate("source(\"Script.R\")");
            }
            catch(Exception ex)
            {
                throw new Exception("R", ex);
            }

            RDotNet.DynamicVector dv = engine.EagerEvaluate("capitalRequirements").AsVector();
            CapitalRequirements = double.Parse(dv[0].ToString());
            RDotNet.DynamicVector dvViolations = engine.EagerEvaluate("violationsCounter").AsVector();
            ViolationsCount = int.Parse(dvViolations[0].ToString());
            RDotNet.DynamicVector dvCorrections = engine.EagerEvaluate("correctionCounter").AsVector();
            CorrectionCount = int.Parse(dvCorrections[0].ToString());
        }

        private void printRScript(string scriptPath)
        {
            StreamWriter writer = new StreamWriter(scriptPath, false);
            string commandLine;

            /////////////////////////////////////////////////////////
            if (executionType == ExecutionType.singleThread)
            {
                writer.WriteLine("#Fajl sa weights");
                writer.WriteLine("weightsFileName=\"weights.dat\"");
            }
            else if (executionType == ExecutionType.viaBinder)
            {
                writer.WriteLine("#Naziv fajla sa weights kao argument skript fajla");
                writer.WriteLine("args=commandArgs(trailingOnly = TRUE)");
                writer.WriteLine("weightsFileName=args[1]");
            }
            writer.WriteLine("weights= read.table(weightsFileName, header=FALSE)");
            writer.WriteLine("w=data.matrix(weights,rownames.force=NA)");

            writer.WriteLine("#Parametri proracuna");
            writer.WriteLine("numberOfObjectives = " + evaluationCriteria.Count.ToString());
            writer.WriteLine("backtestingPeriod = " + backtestingPeriod.ToString());
            writer.WriteLine("evaluationPeriod = " + evaluationPeriod.ToString());
            double alpha = VaRTreshold;
            writer.WriteLine("alpha = " + alpha.ToString());
            writer.WriteLine("#################### Regulatory VaR #####################");

            writer.WriteLine("#Ucitavanje vremenskih serija");
            if (executionType == ExecutionType.singleThread)
            {
                writer.WriteLine("d=read.table(\"timeSeries.csv\", header=FALSE, sep=',')");
            }
            else if (executionType == ExecutionType.viaBinder)
            {
                writer.WriteLine("d=read.table(\"timeSeries.csv-uploaded\", header=FALSE, sep=',')");
            }
            writer.WriteLine("m=data.matrix(d,rownames.force=NA)[,-1]");
            writer.WriteLine("#Racunanje vrednosti portfolija");
            if (variableType == DecisionVariableType.capitalInvested || variableType == DecisionVariableType.constantCapitalInvested)
            {
                writer.WriteLine("#Vrednost holdingsa se racuna na osnovu capital invested weights i vrednosti aseta na poslednji dan evaluacije");
                writer.WriteLine("timeSeriesRowCount = nrow(m)");
                writer.WriteLine("assetCount = ncol(m)");
                writer.WriteLine("shares=matrix(0,nrow=assetCount,ncol=1)");
                writer.WriteLine("portfolioValue=1");
                writer.WriteLine("for(i in 1:assetCount) shares[i]=w[i]*portfolioValue/m[timeSeriesRowCount,i]");
                writer.WriteLine("p=m%*%shares");
            }
            else
            {
                writer.WriteLine("p=m%*%w");
            }
            //writer.WriteLine("p=m%*%t(w)");
            //writer.WriteLine("p=m%*%w");
            writer.WriteLine("VaRestimations = matrix(0,backtestingPeriod+1,1)");
            writer.WriteLine("secondPass = matrix(0,backtestingPeriod+1,1)");
            writer.WriteLine("portfolioLength = nrow(p)");
            writer.WriteLine("returnLength = portfolioLength-1");
            writer.WriteLine("return=matrix(0,nrow=returnLength,ncol=1)");
            writer.WriteLine("#Racunanje prinosa portfolija");
            writer.WriteLine("for( i in 1:returnLength) {return[i,1]=p[i+1]/p[i]-1}");
            writer.WriteLine("library(rugarch)");
            writer.WriteLine("library(simsalapar)");
            writer.WriteLine("#Inicijalni parametri");
            writer.WriteLine("ar1=0");
            writer.WriteLine("ma1=0");
            writer.WriteLine("omega=0");
            writer.WriteLine("alpha1=0");
            writer.WriteLine("beta1=0");
            writer.WriteLine("shape=0");
            writer.WriteLine("correctionCounter=0");
            writer.WriteLine("averageReturn=1000");
            writer.WriteLine("capitalRequirements = 1000");
            writer.WriteLine("violationsCounter = 1000");
            writer.WriteLine("DONE=TRUE");
            writer.WriteLine("MESSAGE=\"OK\"");

            writer.WriteLine("j=1");
            writer.WriteLine("while(j<=(backtestingPeriod+1))");
            writer.WriteLine("{");
            writer.WriteLine("r=matrix(return[(returnLength - evaluationPeriod - backtestingPeriod + j):(returnLength - backtestingPeriod + j - 1)],ncol=1)");
            writer.WriteLine("if(j==(backtestingPeriod+1))");
            writer.WriteLine("{");
            writer.WriteLine("averageReturn=-mean(r)");
            writer.WriteLine("}");
            writer.WriteLine("if(j==1 || secondPass[j]==1)");
            writer.WriteLine("{");
            writer.WriteLine("sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = FALSE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(), fixed.pars = list())");
            writer.WriteLine("}");
            writer.WriteLine("else");
            writer.WriteLine("{");
            writer.WriteLine("sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = FALSE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(ar1=ar1,ma1=ma1,omega=omega,alpha1=alpha1,beta1=beta1,shape=shape), fixed.pars = list())");
            writer.WriteLine("}");

            writer.WriteLine("#Fitovanje GARCH modela");
            writer.WriteLine("tryObject=tryCatch.W.E(ugarchfit(data = r, spec = sp))");
            writer.WriteLine("if(inherits(tryObject$value, \"error\") || is.null(coef(tryObject$value)['ar1'])){");
            writer.WriteLine("if(secondPass[j]==0){");
            writer.WriteLine("secondPass[j]=1");
            writer.WriteLine("}else{");
            //if (executionType == ExecutionType.viaBinder)
            if (true)
                {
                writer.WriteLine("DONE=FALSE");
                writer.WriteLine("if(!is.null(tryObject$warning)){");
                writer.WriteLine("MESSAGE=tryObject$warning$message");
                writer.WriteLine("}");
                writer.WriteLine("else if(!is.null(tryObject$error)){");
                writer.WriteLine("MESSAGE=tryObject$error$message");
                writer.WriteLine("}");
                writer.WriteLine("break");
            }
            else
            {
                writer.WriteLine("VaR = min(VaRestimations)");
                writer.WriteLine("correctionCounter = correctionCounter+1");
                writer.WriteLine("j<-j+1");
            }
            writer.WriteLine("}");
            writer.WriteLine("}else{");
            writer.WriteLine("fit = tryObject$value");
            writer.WriteLine("ar1=coef(fit)['ar1']");
            writer.WriteLine("ma1=coef(fit)['ma1']");
            writer.WriteLine("omega=coef(fit)['omega']");
            writer.WriteLine("alpha1=coef(fit)['alpha1']");
            writer.WriteLine("beta1=coef(fit)['beta1']");
            writer.WriteLine("shape=coef(fit)['shape']");
            writer.WriteLine("#Predikcija VaRa");
            writer.WriteLine("tryObject2=tryCatch.W.E(ugarchforecast(fit, n.ahead=1))");
            writer.WriteLine("if(inherits(tryObject2$value, \"error\")){");
            //if (executionType == ExecutionType.viaBinder)
            if (true)
            {
                writer.WriteLine("DONE=FALSE");
                writer.WriteLine("MESSAGE=tryObject2$value$message");
                writer.WriteLine("break");
            }
            else
            {
                writer.WriteLine("VaR = min(VaRestimations)");
                writer.WriteLine("correctionCounter = correctionCounter+1");
                writer.WriteLine("j<-j+1");
            }
            writer.WriteLine("}else{");
            writer.WriteLine("forc = tryObject2$value");
            writer.WriteLine("sigma=sigma(forc)");
            commandLine = "quantil= qdist('std', p= alpha, mu = 0, sigma = 1, shape=shape)";
            writer.WriteLine(commandLine);
            writer.WriteLine("#Odredjivanje VaR-a");
            writer.WriteLine("VaR = sigma*quantil");
            writer.WriteLine("VaRestimations[j,1]=VaR");
            writer.WriteLine("j<-j+1");
            writer.WriteLine("}");
            writer.WriteLine("}");
            writer.WriteLine("}");

            writer.WriteLine("regulatoryVaR = 0");
            writer.WriteLine("stressVaR = 0");

            //if (executionType == ExecutionType.viaBinder)
            if (true)
            {
                writer.WriteLine("if(DONE){");
            }
            writer.WriteLine("violationsCounter = 0");
            writer.WriteLine("averageRisk = 0.0");
            writer.WriteLine("averageRiskCalculationPeriod = 60");
            writer.WriteLine("for (i in 1:backtestingPeriod)");
            writer.WriteLine("{");
            writer.WriteLine("if(return[returnLength-backtestingPeriod+i] < VaRestimations[i]) violationsCounter = violationsCounter + 1");
            writer.WriteLine("}");
            writer.WriteLine("for (i in 1:averageRiskCalculationPeriod)");
            writer.WriteLine("{");
            writer.WriteLine("averageRisk = averageRisk + VaRestimations[backtestingPeriod - averageRiskCalculationPeriod + i]");
            writer.WriteLine("}");
            writer.WriteLine("averageRisk = averageRisk / averageRiskCalculationPeriod");
            writer.WriteLine("k = 0.0");
            writer.WriteLine("if(violationsCounter <= 4) {");
            writer.WriteLine("k = 0.0");
            writer.WriteLine("} else if (violationsCounter == 5) {");
            writer.WriteLine("k = 0.4");
            writer.WriteLine("} else if (violationsCounter == 6) {");
            writer.WriteLine("k = 0.5");
            writer.WriteLine("} else if (violationsCounter == 7) {");
            writer.WriteLine("k = 0.65");
            writer.WriteLine("} else if (violationsCounter == 8) {");
            writer.WriteLine("k = 0.75");
            writer.WriteLine("} else if (violationsCounter == 9) {");
            writer.WriteLine("k = 0.85");
            writer.WriteLine("} else k = 1");
            writer.WriteLine("Comp1 = abs(VaRestimations[backtestingPeriod+1,1])");
            writer.WriteLine("Comp2 = abs((3 + k) * averageRisk)");
            writer.WriteLine("regulatoryVaR = max(c(Comp1, Comp2)) * sqrt(10)");
            writer.WriteLine("}");
            writer.WriteLine("#################### End Regulatory VaR #####################");

            if (stressPart)
            {
                writer.WriteLine("#################### Stress VaR #####################");

                writer.WriteLine("if(DONE){");
                writer.WriteLine("stressbacktestingPeriod = 60");
                writer.WriteLine("#Ucitavanje stress vremenskih serija");
                if (executionType == ExecutionType.singleThread)
                {
                    writer.WriteLine("d=read.table(\"timeSeriesStress.csv\", header=FALSE, sep=',')");
                }
                else if (executionType == ExecutionType.viaBinder)
                {
                    writer.WriteLine("d=read.table(\"timeSeriesStress.csv-uploaded\", header=FALSE, sep=',')");
                }
                writer.WriteLine("stressValues=data.matrix(d,rownames.force=NA)[,-1]");
                writer.WriteLine("stressPortfolio=stressValues%*%shares");
                writer.WriteLine("stressVaRestimations = matrix(0,stressbacktestingPeriod+1,1)");
                writer.WriteLine("secondPassStress = matrix(0,stressbacktestingPeriod+1,1)");
                writer.WriteLine("stressPortfolioLength = nrow(stressPortfolio)");
                writer.WriteLine("stressReturnLength = stressPortfolioLength-1");
                writer.WriteLine("stressReturn=matrix(0,nrow=stressReturnLength,ncol=1)");
                writer.WriteLine("#Racunanje prinosa portfolija");
                writer.WriteLine("for( i in 1:stressReturnLength) {stressReturn[i,1]=stressPortfolio[i+1]/stressPortfolio[i]-1}");

                writer.WriteLine("#Inicijalni parametri");
                writer.WriteLine("ar1=0");
                writer.WriteLine("ma1=0");
                writer.WriteLine("omega=0");
                writer.WriteLine("alpha1=0");
                writer.WriteLine("beta1=0");
                writer.WriteLine("shape=0");

                writer.WriteLine("j=1");
                writer.WriteLine("while(j<=(stressbacktestingPeriod+1))");
                writer.WriteLine("{");
                writer.WriteLine("r=matrix(stressReturn[(stressReturnLength - evaluationPeriod - stressbacktestingPeriod + j):(stressReturnLength - stressbacktestingPeriod +j-1)],ncol=1)");
                writer.WriteLine("if(j==1 || secondPass[j]==1)");
                writer.WriteLine("{");
                writer.WriteLine("sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = FALSE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(), fixed.pars = list())");
                writer.WriteLine("}");
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.WriteLine("sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = FALSE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(ar1=ar1,ma1=ma1,omega=omega,alpha1=alpha1,beta1=beta1,shape=shape), fixed.pars = list())");
                writer.WriteLine("}");
                writer.WriteLine("#Fitovanje GARCH modela");
                writer.WriteLine("tryObject=tryCatch.W.E(ugarchfit(data = r, spec = sp))");
                writer.WriteLine("if(inherits(tryObject$value, \"error\") || is.null(coef(tryObject$value)['ar1'])){");
                writer.WriteLine("if(secondPassStress[j]==0){");
                writer.WriteLine("secondPassStress[j]=1");
                writer.WriteLine("}else{");
                writer.WriteLine("DONE=FALSE");
                writer.WriteLine("if(!is.null(tryObject$warning)){");
                writer.WriteLine("MESSAGE=tryObject$warning$message");
                writer.WriteLine("}");
                writer.WriteLine("else if(!is.null(tryObject$error)){");
                writer.WriteLine("MESSAGE=tryObject$error$message");
                writer.WriteLine("}");
                writer.WriteLine("break");
                writer.WriteLine("}");
                writer.WriteLine("}else{");
                writer.WriteLine("fit = tryObject$value");
                writer.WriteLine("ar1=coef(fit)['ar1']");
                writer.WriteLine("ma1=coef(fit)['ma1']");
                writer.WriteLine("omega=coef(fit)['omega']");
                writer.WriteLine("alpha1=coef(fit)['alpha1']");
                writer.WriteLine("beta1=coef(fit)['beta1']");
                writer.WriteLine("shape=coef(fit)['shape']");
                writer.WriteLine("#Predikcija Stress VaRa");
                writer.WriteLine("tryObject2=tryCatch.W.E(ugarchforecast(fit, n.ahead=1))");
                writer.WriteLine("if(inherits(tryObject2$value, \"error\")){");
                writer.WriteLine("DONE=FALSE");
                writer.WriteLine("MESSAGE=tryObject2$value$message");
                writer.WriteLine("break");
                writer.WriteLine("}else{");
                writer.WriteLine("forc = tryObject2$value");
                writer.WriteLine("sigma=sigma(forc)");
                writer.WriteLine("quantil= qdist('std', p= alpha, mu = 0, sigma = 1, shape=shape)");
                writer.WriteLine("#Odredjivanje Stress VaR-a");
                writer.WriteLine("VaR = sigma*quantil");
                writer.WriteLine("stressVaRestimations[j,1]=VaR");
                writer.WriteLine("j<-j+1");
                writer.WriteLine("}\n}\n}\n");
                writer.WriteLine("stressVaR = 0");
                writer.WriteLine("if(DONE){");
                writer.WriteLine("averageRisk = 0.0");
                writer.WriteLine("averageRiskCalculationPeriod = 60");
                writer.WriteLine("for(i in 1:averageRiskCalculationPeriod)");
                writer.WriteLine("{");
                writer.WriteLine("averageRisk = averageRisk + stressVaRestimations[stressbacktestingPeriod - averageRiskCalculationPeriod + i]");
                writer.WriteLine("}");
                writer.WriteLine("averageRisk = averageRisk / averageRiskCalculationPeriod");
                writer.WriteLine("Comp1 = abs(stressVaRestimations[stressbacktestingPeriod+1,1])");
                writer.WriteLine("Comp2 = abs((3 + k) * averageRisk)");
                writer.WriteLine("stressVaR = max(c(Comp1, Comp2)) * sqrt(10)");
                writer.WriteLine("}\n}\n");

                writer.WriteLine("############# End Stress VaR ###################");
            }

            writer.WriteLine("if(DONE){");
            writer.WriteLine("capitalRequirements = regulatoryVaR + stressVaR");
            writer.WriteLine("}");

            //writer.WriteLine("if(DONE){");
            //writer.WriteLine("cat(\"OK\n\")");
            //writer.WriteLine("cat(numberOfObjectives, \"\n\")");

            //foreach (EvaluationCriteria EC in evaluationCriteria)
            //{
            //    if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR)
            //    {
            //        writer.WriteLine("cat(capitalRequirements, \"\n\")");
            //    }
            //    else if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.AverageReturn)
            //    {
            //        writer.WriteLine("cat(averageReturn, \"\n\")");
            //    }
            //}
            //writer.WriteLine("}else{");
            //writer.WriteLine("cat(\"EXE_FAILED\n\")");
            //writer.WriteLine("}");

            writer.WriteLine("{");
            writer.WriteLine("cat(\"OK\n\")");
            writer.WriteLine("cat(numberOfObjectives, \"\n\")");
            foreach (EvaluationCriteria EC in evaluationCriteria)
            {
                if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR)
                {
                    writer.WriteLine("cat(capitalRequirements, \"\n\")");
                    //writer.WriteLine("cat(violationsCounter, \"\n\")");
                }
                else if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.AverageReturn)
                {
                    writer.WriteLine("cat(averageReturn, \"\n\")");
                }
            }
            writer.WriteLine("}");
            writer.Dispose();
            ////////////////////////////////////////////////////////
        }

        //private void printRScript(string scriptPath)
        //{
        //    StreamWriter writer = new StreamWriter(scriptPath, false);

        //    /////////////////////////////////////////////////////////
        //    if (executionType == ExecutionType.singleThread)
        //    {
        //        writer.WriteLine("#Fajl sa weights");
        //        writer.WriteLine("weightsFileName=\"weights.dat\"");
        //    }
        //    else if (executionType == ExecutionType.viaBinder)
        //    {
        //        writer.WriteLine("#Naziv fajla sa weights kao argument skript fajla");
        //        writer.WriteLine("args=commandArgs(trailingOnly = TRUE)");
        //        writer.WriteLine("weightsFileName=args[1]");
        //    }
        //    writer.WriteLine("weights= read.table(weightsFileName, header=FALSE)");
        //    writer.WriteLine("w=data.matrix(weights,rownames.force=NA)");

        //    writer.WriteLine("#Parametri proracuna");
        //    writer.WriteLine("numberOfObjectives = " + evaluationCriteria.Count.ToString());
        //    writer.WriteLine("backtestingPeriod = " + backtestingPeriod.ToString());
        //    writer.WriteLine("evaluationPeriod = " + evaluationPeriod.ToString());
        //    double alpha = VaRTreshold;
        //    writer.WriteLine("alpha = " + alpha.ToString());
        //    writer.WriteLine("#Ucitavanje vremenskih serija");
        //    if (executionType == ExecutionType.singleThread)
        //    {
        //        writer.WriteLine("d=read.table(\"timeSeries.csv\", header=FALSE, sep=',')");
        //    }
        //    else if (executionType == ExecutionType.viaBinder)
        //    {
        //        writer.WriteLine("d=read.table(\"timeSeries.csv-uploaded\", header=FALSE, sep=',')");
        //    }

        //    writer.WriteLine("DONE=TRUE");
        //    writer.WriteLine("capitalRequirements=0.1");
        //    writer.WriteLine("averageReturn=0.5");
        //    writer.WriteLine("numberOfObjectives=2");
        //    writer.WriteLine("if(DONE){");
        //    //writer.WriteLine("cat(\"OK\n2\n\")");
        //    writer.WriteLine("cat(\"OK\n\")");
        //    writer.WriteLine("cat(numberOfObjectives, \"\n\")");
        //    //writer.WriteLine("cat(\"OK\n\",numberOfObjectives,\"\n\")");

        //    foreach (EvaluationCriteria EC in evaluationCriteria)
        //    {
        //        if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR)
        //        {
        //            writer.WriteLine("cat(capitalRequirements, \"\n\")");
        //        }
        //        else if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.AverageReturn)
        //        {
        //            writer.WriteLine("cat(averageReturn, \"\n\")");
        //        }
        //    }
        //    writer.WriteLine("}else{");
        //    writer.WriteLine("cat(\"EXE_FAILED\n\")");
        //    writer.WriteLine("}");

        //    writer.Dispose();
        //    ////////////////////////////////////////////////////////
        //}

        #endregion 
        
    }
}

  