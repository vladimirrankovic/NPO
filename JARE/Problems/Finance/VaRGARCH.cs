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
    public class VaRGARCH : RCalculation
    {
        private AssetSet timeSeriesSet;
        DateTime evaluationDate;
        int evaluationPeriod;
        int ReturnPeriod;
        DecisionVariableType variableType;
        double VaRTreshold;
        

        //Additional parameters
        public ParameterCollection GARCHparameters;


        public VaRGARCH(AssetSet timeSeriesSet, List<EvaluationCriteria> evaluationCriteria, DateTime EvaluationDate, int evaluationPeriod,
                                     int ReturnPeriod, DecisionVariableType variableType, bool UsingRScriptFile,
                                     double varTreshold, Dictionary<string, Object> parameters,
                                     string RScriptPath, ExecutionType executionType = ExecutionType.singleThread) : base(evaluationCriteria, RScriptPath, executionType)
        {
            this.timeSeriesSet = timeSeriesSet;
            this.evaluationPeriod = evaluationPeriod;
            this.ReturnPeriod = ReturnPeriod;
            this.variableType = variableType;
            evaluationDate = EvaluationDate;
            VaRTreshold = varTreshold;
            GARCHparameters = new ParameterCollection(parameters);

            CalculationPreparation(timeSeriesSet);
        }

        public void Init()
        {
            //R Initialization
            base.Init();
            CalculationPreparation(timeSeriesSet);
        }


        #region VaRGARCH
        //R preparation
        protected void CalculationPreparation(AssetSet timeSeriesSet)
        {
            int increasement = 0;
            //timeSeriesSet.GetItemIndex(evaluationDate, out evaluationDateIndex);
            if (variableType != DecisionVariableType.constantCapitalInvested) increasement = ReturnPeriod;
            string timeSeriesFileName = "\\timeSeries.csv";
            timeSeriesSet.DumpToCsv(RScriptPath + timeSeriesFileName, evaluationDate, evaluationPeriod + increasement, false, ',');

            //PRINT SCRIPT FILE
            string scriptName = "\\Script.R";
            string scriptPath = RScriptPath + scriptName;
            printRScript(scriptPath);

            if (executionType == ExecutionType.viaBinder)
            {
                UploadFiles2(RScriptPath, "\\timeSeries.csv");
                UploadFiles2(RScriptPath, "\\Script.R");
            }
        }

        // VaR from GARCH
        public double CalculateVaRGarch(Solution ponder)
        {
            double varGarch = 1000;
            string weightsFileName = "\\weights.dat";

            //PRINT PONDERS TO FILE
            StreamWriter writer = new StreamWriter(RScriptPath + weightsFileName, false);

            for (int i = 0; i < ponder.numberOfVariables(); i++)
            {
                writer.WriteLine(ponder.DecisionVariables[i]);
            }
            writer.Dispose();

            if (!viaRDotNet)
            {
                System.Diagnostics.ProcessStartInfo processInfo;

                string batPath = RScriptPath + "\\R.bat";
                processInfo = new System.Diagnostics.ProcessStartInfo(batPath);
                
                processInfo.WorkingDirectory = RScriptPath;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardOutput = true;
                processInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                processInfo.CreateNoWindow = true; //not diplay a windows

                try
                {
                    System.Diagnostics.Process pProcess = System.Diagnostics.Process.Start(processInfo);
                    //pProcess.Start();
                    string output = pProcess.StandardOutput.ReadToEnd(); //The output result
                    pProcess.WaitForExit();
                    varGarch = readRStandardOutput(output);
                }
                catch (Exception ex)
                {
                    throw new Exception("R", ex);
                }
            }
            else
            {
                try
                {
                    engine.EagerEvaluate("source(\"Script.R\")");

                    //Get GARCH model parameters
                    GARCHparameters.Clear();
                    engine.EagerEvaluate("ar1=coef(fit)['ar1']");
                    RDotNet.DynamicVector ar1v = engine.EagerEvaluate("ar1").AsVector();
                    double ar1 = double.Parse(ar1v[0].ToString());
                    GARCHparameters.setParameter("ar1", ar1);
                    engine.EagerEvaluate("ma1=coef(fit)['ma1']");
                    RDotNet.DynamicVector ma1v = engine.EagerEvaluate("ma1").AsVector();
                    double ma1 = double.Parse(ma1v[0].ToString());
                    GARCHparameters.setParameter("ma1", ma1);
                    engine.EagerEvaluate("omega=coef(fit)['omega']");
                    RDotNet.DynamicVector omegav = engine.EagerEvaluate("omega").AsVector();
                    double omega = double.Parse(omegav[0].ToString());
                    GARCHparameters.setParameter("omega", omega);
                    engine.EagerEvaluate("alpha1=coef(fit)['alpha1']");
                    RDotNet.DynamicVector alpha1v = engine.EagerEvaluate("alpha1").AsVector();
                    double alpha1 = double.Parse(alpha1v[0].ToString());
                    GARCHparameters.setParameter("alpha1", alpha1);
                    engine.EagerEvaluate("beta1=coef(fit)['beta1']");
                    RDotNet.DynamicVector beta1v = engine.EagerEvaluate("beta1").AsVector();
                    double beta1 = double.Parse(beta1v[0].ToString());
                    GARCHparameters.setParameter("beta1", beta1);
                    engine.EagerEvaluate("shape=coef(fit)['shape']");
                    RDotNet.DynamicVector shapev = engine.EagerEvaluate("shape").AsVector();
                    double shape = double.Parse(shapev[0].ToString());
                    GARCHparameters.setParameter("shape", shape);
                    RDotNet.DynamicVector sigmav = engine.EagerEvaluate("sigma").AsVector();
                    double sigma = double.Parse(sigmav[0].ToString());
                    //

                    RDotNet.DynamicVector dv = engine.EagerEvaluate("VaR").AsVector();
                    varGarch = double.Parse(dv[0].ToString());

                }
                catch (Exception ex)
                {
                    throw new Exception("R", ex);
                }
            }

            return varGarch;
            //return sigma;
        }

        private void printRScript(string scriptPath)
        {
            StreamWriter writer = new StreamWriter(scriptPath, false);
            string commandLine;
            bool IncludeMean = false;

            /////////////////////////////////////////////////////////
            writer.WriteLine("numberOfObjectives = " + evaluationCriteria.Count.ToString());
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
            if (variableType == DecisionVariableType.capitalInvested)
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

            writer.WriteLine("#Racunanje prinosa portfolija");
            if (variableType == DecisionVariableType.constantCapitalInvested)
            {
                writer.WriteLine("r=matrix(p,ncol=1)");
            }
            else
            {
                writer.WriteLine("yield=matrix(,ncol=1)");
                commandLine = "for( i in 1:" + evaluationPeriod.ToString() + ") {yield[i]=p[i+" + ReturnPeriod.ToString() + "]/p[i]-1}";
                writer.WriteLine(commandLine);
                writer.WriteLine("r=matrix(yield,ncol=1)");
            }
            writer.WriteLine("averageReturn=-mean(r)");//MO minimizes objective function
            writer.WriteLine("#Lodovanje biblioteke rugacrch");
            writer.WriteLine("library(rugarch)");
            writer.WriteLine("library(simsalapar)");
            writer.WriteLine("#Definisanje GARCH modela");
            string startPars = string.Empty;
            startPars = GetStartParametersForGARCHFitting();
            if(IncludeMean == false) writer.WriteLine("sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = FALSE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(" + startPars + "), fixed.pars = list())");
            else writer.WriteLine("sp<-ugarchspec (variance.model = list(model = 'sGARCH', garchOrder = c(1, 1), submodel = NULL, external.regressors = NULL, variance.targeting = FALSE), mean.model = list(armaOrder = c(1, 1), include.mean = TRUE, archm = FALSE, archpow = 1, arfima = FALSE, external.regressors = NULL, archex = FALSE), distribution.model = 'std', start.pars = list(" + startPars + "), fixed.pars = list())");
            writer.WriteLine("#Fitovanje GARCH modela");
            writer.WriteLine("DONE=TRUE");
            writer.WriteLine("MESSAGE=\"OK\"");
            writer.WriteLine("tryObject=tryCatch.W.E(ugarchfit(data = r, spec = sp))");
            writer.WriteLine("if(inherits(tryObject$value, \"error\") || is.null(coef(tryObject$value)['ar1'])){");
            writer.WriteLine("DONE=FALSE");
            writer.WriteLine("if(!is.null(tryObject$warning)){");
            writer.WriteLine("MESSAGE=tryObject$warning$message");
            writer.WriteLine("}");
            writer.WriteLine("else if(!is.null(tryObject$error)){");
            writer.WriteLine("MESSAGE=tryObject$error$message");
            writer.WriteLine("}");
            writer.WriteLine("}else{");
            writer.WriteLine("fit = tryObject$value");
            writer.WriteLine("df=coef(fit)['shape']");
            writer.WriteLine("#Predikcija VaRa");
            writer.WriteLine("tryObject2=tryCatch.W.E(ugarchforecast(fit, n.ahead=1))");
            writer.WriteLine("if(inherits(tryObject2$value, \"error\")){");
            writer.WriteLine("DONE=FALSE");
            writer.WriteLine("MESSAGE=tryObject2$value$message");
            writer.WriteLine("}else{");
            writer.WriteLine("forc = tryObject2$value");
            writer.WriteLine("sigma=sigma(forc)");
            writer.WriteLine("mean=fitted(forc)");
            commandLine = "quantil= qdist('std', p= " + VaRTreshold.ToString() + ", mu = 0, sigma = 1, shape=df)";
            writer.WriteLine(commandLine);
            writer.WriteLine("#Odredjivanje VaR-a");
            if(IncludeMean == false) writer.WriteLine("VaR = -sigma*quantil");
            else writer.WriteLine("VaR = -(mean + sigma*quantil)");
            writer.WriteLine("}");
            writer.WriteLine("}");

            //writer.WriteLine("if(DONE)");
            //writer.WriteLine("{");
            //writer.WriteLine("cat(\"OK\n\")");
            //writer.WriteLine("cat(numberOfObjectives, \"\n\")");
            //foreach (EvaluationCriteria EC in evaluationCriteria)
            //{
            //    if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch)
            //    {
            //        writer.WriteLine("cat(VaR, \"\n\")");
            //    }
            //    else if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.AverageReturn)
            //    {
            //        writer.WriteLine("cat(averageReturn, \"\n\")");
            //    }
            //}

            //writer.WriteLine("}else{");
            //writer.WriteLine("cat(\"EXE_FAILED\n\")");
            //writer.WriteLine("}");
           
            writer.WriteLine("if(!DONE)");
            writer.WriteLine("{");
            writer.WriteLine("VaR = 1000");
            writer.WriteLine("averageReturn = 1000");
            writer.WriteLine("}");

            writer.WriteLine("{");
            writer.WriteLine("cat(\"OK\n\")");
            writer.WriteLine("cat(numberOfObjectives, \"\n\")");
            foreach (EvaluationCriteria EC in evaluationCriteria)
            {
                if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch)
                {
                    writer.WriteLine("cat(\"VaR=\", VaR, \"\n\")");
                }
                else if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.AverageReturn)
                {
                    writer.WriteLine("cat(\"averageReturn=\", averageReturn, \"\n\")");
                }
            }
            writer.WriteLine("}");

            writer.Dispose();
            ////////////////////////////////////////////////////////

        }

        #endregion 
        
        private string GetStartParametersForGARCHFitting()
        {
            string startPars = string.Empty;

            if (GARCHparameters.ContainsParameter("ar1")) startPars = "ar1=" + ((double)GARCHparameters.getParameter("ar1")).ToString();
            if (GARCHparameters.ContainsParameter("ar1")) startPars += ",ma1=" + ((double)GARCHparameters.getParameter("ma1")).ToString();
            if (GARCHparameters.ContainsParameter("omega")) startPars += ",omega=" + ((double)GARCHparameters.getParameter("omega")).ToString();
            if (GARCHparameters.ContainsParameter("alpha1")) startPars += ",alpha1=" + ((double)GARCHparameters.getParameter("alpha1")).ToString();
            if (GARCHparameters.ContainsParameter("beta1")) startPars += ",beta1=" + ((double)GARCHparameters.getParameter("beta1")).ToString();
            if (GARCHparameters.ContainsParameter("shape")) startPars += ",shape=" + ((double)GARCHparameters.getParameter("shape")).ToString();

            return startPars;
        }

        private double readRStandardOutput(string output)
        {
            double VaR = 1000;
            string[] lines = output.Replace("\r", "").Split('\n');

            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Contains("VaR"))
                    {
                        line.Trim();
                        string[] values = line.Split('=');
                        VaR = double.Parse(values[1]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("R", ex);
            }

            return VaR;

        }
    }
}

  