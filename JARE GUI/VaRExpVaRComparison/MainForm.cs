// Approximation (Symbolic Regression) using Genetic Programming and Gene Expression Programming
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Andrew Kirillov, 2006-2009
// andrew.kirillov@aforgenet.com
//

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using JARE;

using TimeSerie = JARE.problems.TimeSerie;
using Problem = JARE.Base.Problem;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using PortfolioOptimizationGA = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGA;
using PortfolioOptimization = JARE.problems.PortfolioOptimization;
using VaRExpVaRComparison = JARE.problems.VaRExpVaRComparison;

using AForge;
using AForge.Genetic;
using AForge.Controls;

namespace Approximation
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView dataList;
        private System.Windows.Forms.ColumnHeader xColumnHeader;
        private System.Windows.Forms.ColumnHeader yColumnHeader;
        private System.Windows.Forms.Button loadDataButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        StreamWriter sw;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox groupBox2;
        private AForge.Controls.Chart chart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox populationSizeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox selectionBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox currentErrorBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox functionsSetBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox geneticMethodBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox solutionBox;

        private double[,] data = null;


        private int populationSize = 50;
        private int iterations = 80;
        private int selectionMethod = 0;
        private int functionsSet = 0;
        private int geneticMethod = 0;
        private PortfolioOptimization.EvaluationCriteria[] evaluationCriteria = new PortfolioOptimization.EvaluationCriteria[2] { PortfolioOptimization.EvaluationCriteria.maximalAverageReturn, PortfolioOptimization.EvaluationCriteria.maximalVaR};

        //private int evaluationPeriod = 756;
        private int evaluationPeriod = 252;
        private int ReturnPeriod = 1;
        //private DateTime evaluationEndDate = new DateTime(2010, 12, 23);//za rad expsys, Comp Economics
        private DateTime evaluationEndDate = new DateTime(2010, 6, 2);
        //private DateTime evaluationEndDate = new DateTime(2011, 2, 18);//S&P100
        private double interestRate = 0.0;
        private double varThreshold = 5.0;
        private int weightParamDiscret = 10;
        private System.Collections.Generic.List<double> weightParameterList = new System.Collections.Generic.List<double>();


        private Thread workerThread = null;
        private volatile bool needToStop = false;
        private ComboBox fitnessCriteria1Box;
        private Label label9;
        private GroupBox groupBox6;
        private TextBox ReturnPeriodBox;
        private TextBox evaluationPeriodBox;
        private Label label10;
        private Label label11;
        private DateTimePicker dateTimePicker1;
        private Label label12;
        private TextBox interestRateBox;
        private Label label13;
        private TextBox varThresholdBox;
        private Label label14;
        private TextBox parametersBox;
        private Label label15;
        private TextBox weightParamDiscretbox;
        private ComboBox fitnessCriteria2Box;
        private Label label16;
        private Button buttonLoadWeightParam;
        private System.Collections.Generic.List<TimeSerie> Assets = new System.Collections.Generic.List<TimeSerie>();

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);

            selectionBox.SelectedIndex = selectionMethod;
            functionsSetBox.SelectedIndex = functionsSet;
            geneticMethodBox.SelectedIndex = geneticMethod;
            fitnessCriteria1Box.SelectedIndex = (int)evaluationCriteria[0];
            fitnessCriteria2Box.SelectedIndex = (int)evaluationCriteria[1];

            UpdateSettings();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataList = new System.Windows.Forms.ListView();
            this.xColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.yColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.loadDataButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chart = new AForge.Controls.Chart();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.geneticMethodBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.functionsSetBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.selectionBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.populationSizeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fitnessCriteria1Box = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.currentErrorBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.parametersBox = new System.Windows.Forms.TextBox();
            this.solutionBox = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonLoadWeightParam = new System.Windows.Forms.Button();
            this.fitnessCriteria2Box = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.weightParamDiscretbox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.varThresholdBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.interestRateBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ReturnPeriodBox = new System.Windows.Forms.TextBox();
            this.evaluationPeriodBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataList);
            this.groupBox1.Controls.Add(this.loadDataButton);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 310);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // dataList
            // 
            this.dataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xColumnHeader,
            this.yColumnHeader});
            this.dataList.FullRowSelect = true;
            this.dataList.GridLines = true;
            this.dataList.Location = new System.Drawing.Point(10, 20);
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(160, 255);
            this.dataList.TabIndex = 0;
            this.dataList.UseCompatibleStateImageBehavior = false;
            this.dataList.View = System.Windows.Forms.View.Details;
            // 
            // xColumnHeader
            // 
            this.xColumnHeader.Text = "X";
            // 
            // yColumnHeader
            // 
            this.yColumnHeader.Text = "Y";
            // 
            // loadDataButton
            // 
            this.loadDataButton.Location = new System.Drawing.Point(10, 280);
            this.loadDataButton.Name = "loadDataButton";
            this.loadDataButton.Size = new System.Drawing.Size(75, 23);
            this.loadDataButton.TabIndex = 1;
            this.loadDataButton.Text = "&Load";
            this.loadDataButton.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chart);
            this.groupBox2.Location = new System.Drawing.Point(200, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 126);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Function";
            // 
            // chart
            // 
            this.chart.Location = new System.Drawing.Point(10, 20);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(280, 96);
            this.chart.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.geneticMethodBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.functionsSetBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.iterationsBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.selectionBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.populationSizeBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(510, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(193, 160);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // geneticMethodBox
            // 
            this.geneticMethodBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.geneticMethodBox.Items.AddRange(new object[] {
            "GP",
            "GEP"});
            this.geneticMethodBox.Location = new System.Drawing.Point(110, 95);
            this.geneticMethodBox.Name = "geneticMethodBox";
            this.geneticMethodBox.Size = new System.Drawing.Size(77, 21);
            this.geneticMethodBox.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 6;
            this.label8.Text = "Genetic method:";
            // 
            // functionsSetBox
            // 
            this.functionsSetBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionsSetBox.Items.AddRange(new object[] {
            "Simple",
            "Extended"});
            this.functionsSetBox.Location = new System.Drawing.Point(110, 70);
            this.functionsSetBox.Name = "functionsSetBox";
            this.functionsSetBox.Size = new System.Drawing.Size(77, 21);
            this.functionsSetBox.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Functions set:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(128, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "( 0 - inifinity )";
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(125, 122);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(62, 20);
            this.iterationsBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Iterations:";
            // 
            // selectionBox
            // 
            this.selectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectionBox.Items.AddRange(new object[] {
            "Elite",
            "Rank",
            "Roulette"});
            this.selectionBox.Location = new System.Drawing.Point(110, 45);
            this.selectionBox.Name = "selectionBox";
            this.selectionBox.Size = new System.Drawing.Size(77, 21);
            this.selectionBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selection method:";
            // 
            // populationSizeBox
            // 
            this.populationSizeBox.Location = new System.Drawing.Point(125, 20);
            this.populationSizeBox.Name = "populationSizeBox";
            this.populationSizeBox.Size = new System.Drawing.Size(62, 20);
            this.populationSizeBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Population size:";
            // 
            // fitnessCriteria1Box
            // 
            this.fitnessCriteria1Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitnessCriteria1Box.Items.AddRange(new object[] {
            "None",
            "Max Return",
            "Min STDEVP",
            "Max Sharp Index",
            "Max Sortino",
            "Max VaR",
            "Max cVaR",
            "Max ExpWeightedVaR",
            "Max ExpWeightedcVaR",
            "Max Skew",
            "Max Kurtosis"});
            this.fitnessCriteria1Box.Location = new System.Drawing.Point(96, 17);
            this.fitnessCriteria1Box.Name = "fitnessCriteria1Box";
            this.fitnessCriteria1Box.Size = new System.Drawing.Size(144, 21);
            this.fitnessCriteria1Box.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(7, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 16);
            this.label9.TabIndex = 11;
            this.label9.Text = "Fitness criteria 1:";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(275, 336);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(365, 336);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.currentErrorBox);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.currentIterationBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(510, 289);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(193, 75);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current iteration";
            // 
            // currentErrorBox
            // 
            this.currentErrorBox.Location = new System.Drawing.Point(86, 45);
            this.currentErrorBox.Name = "currentErrorBox";
            this.currentErrorBox.ReadOnly = true;
            this.currentErrorBox.Size = new System.Drawing.Size(101, 20);
            this.currentErrorBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Ev. Param:";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(125, 20);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(62, 20);
            this.currentIterationBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Iteration:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.parametersBox);
            this.groupBox5.Controls.Add(this.solutionBox);
            this.groupBox5.Location = new System.Drawing.Point(10, 365);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(693, 80);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Solution:";
            // 
            // parametersBox
            // 
            this.parametersBox.Location = new System.Drawing.Point(10, 48);
            this.parametersBox.Name = "parametersBox";
            this.parametersBox.ReadOnly = true;
            this.parametersBox.Size = new System.Drawing.Size(677, 20);
            this.parametersBox.TabIndex = 1;
            // 
            // solutionBox
            // 
            this.solutionBox.Location = new System.Drawing.Point(10, 20);
            this.solutionBox.Name = "solutionBox";
            this.solutionBox.ReadOnly = true;
            this.solutionBox.Size = new System.Drawing.Size(677, 20);
            this.solutionBox.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buttonLoadWeightParam);
            this.groupBox6.Controls.Add(this.fitnessCriteria2Box);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.weightParamDiscretbox);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.varThresholdBox);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.interestRateBox);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.dateTimePicker1);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.ReturnPeriodBox);
            this.groupBox6.Controls.Add(this.evaluationPeriodBox);
            this.groupBox6.Controls.Add(this.fitnessCriteria1Box);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Location = new System.Drawing.Point(200, 170);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(549, 115);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "EvaluationSettingsBox";
            // 
            // buttonLoadWeightParam
            // 
            this.buttonLoadWeightParam.Location = new System.Drawing.Point(119, 89);
            this.buttonLoadWeightParam.Name = "buttonLoadWeightParam";
            this.buttonLoadWeightParam.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadWeightParam.TabIndex = 2;
            this.buttonLoadWeightParam.Text = "&Load";
            this.buttonLoadWeightParam.Click += new System.EventHandler(this.buttonLoadWeightParam_Click);
            // 
            // fitnessCriteria2Box
            // 
            this.fitnessCriteria2Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitnessCriteria2Box.Items.AddRange(new object[] {
            "None",
            "Max Return",
            "Min STDEVP",
            "Max Sharp Index",
            "Max Sortino",
            "Max VaR",
            "Max cVaR",
            "Max ExpWeightedVaR",
            "Max ExpWeightedcVaR",
            "Max Skew",
            "Max Kurtosis"});
            this.fitnessCriteria2Box.Location = new System.Drawing.Point(96, 45);
            this.fitnessCriteria2Box.Name = "fitnessCriteria2Box";
            this.fitnessCriteria2Box.Size = new System.Drawing.Size(144, 21);
            this.fitnessCriteria2Box.TabIndex = 28;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(7, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(90, 16);
            this.label16.TabIndex = 27;
            this.label16.Text = "Fitness criteria 2:";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(7, 96);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 16);
            this.label15.TabIndex = 26;
            this.label15.Text = "Weight param discret:";
            // 
            // weightParamDiscretbox
            // 
            this.weightParamDiscretbox.Location = new System.Drawing.Point(201, 92);
            this.weightParamDiscretbox.Name = "weightParamDiscretbox";
            this.weightParamDiscretbox.Size = new System.Drawing.Size(49, 20);
            this.weightParamDiscretbox.TabIndex = 25;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(248, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 16);
            this.label14.TabIndex = 21;
            this.label14.Text = "VaR threshold:";
            // 
            // varThresholdBox
            // 
            this.varThresholdBox.Location = new System.Drawing.Point(331, 70);
            this.varThresholdBox.Name = "varThresholdBox";
            this.varThresholdBox.Size = new System.Drawing.Size(54, 20);
            this.varThresholdBox.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(248, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 16);
            this.label13.TabIndex = 19;
            this.label13.Text = "Interest rate:";
            // 
            // interestRateBox
            // 
            this.interestRateBox.Location = new System.Drawing.Point(331, 44);
            this.interestRateBox.Name = "interestRateBox";
            this.interestRateBox.Size = new System.Drawing.Size(54, 20);
            this.interestRateBox.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(254, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 16);
            this.label12.TabIndex = 17;
            this.label12.Text = "Eval. End Date:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(341, 17);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(389, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Return period:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(389, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Evaluation period:";
            // 
            // ReturnPeriodBox
            // 
            this.ReturnPeriodBox.Location = new System.Drawing.Point(486, 73);
            this.ReturnPeriodBox.Name = "ReturnPeriodBox";
            this.ReturnPeriodBox.Size = new System.Drawing.Size(54, 20);
            this.ReturnPeriodBox.TabIndex = 14;
            // 
            // evaluationPeriodBox
            // 
            this.evaluationPeriodBox.Location = new System.Drawing.Point(486, 46);
            this.evaluationPeriodBox.Name = "evaluationPeriodBox";
            this.evaluationPeriodBox.Size = new System.Drawing.Size(54, 20);
            this.evaluationPeriodBox.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(761, 455);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Approximation (Symbolic Regression) using Genetic Programming and Gene Expression" +
                " Programming";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void SetTextCallback(System.Windows.Forms.Control control, string text);

        // Thread safe updating of control's text property
        private void SetText(System.Windows.Forms.Control control, string text)
        {
            if (control.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }

        // On main form closing
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if worker thread is running
            if ((workerThread != null) && (workerThread.IsAlive))
            {
                needToStop = true;
                while (!workerThread.Join(100))
                    Application.DoEvents();
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            populationSizeBox.Text = populationSize.ToString();
            iterationsBox.Text = iterations.ToString();
            evaluationPeriodBox.Text = evaluationPeriod.ToString();
            ReturnPeriodBox.Text = ReturnPeriod.ToString();
            this.dateTimePicker1.Value = evaluationEndDate;
            interestRateBox.Text = interestRate.ToString();
            varThresholdBox.Text = varThreshold.ToString();
            weightParamDiscretbox.Text = weightParamDiscret.ToString();
        }

        // Load data
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // read maximum 50 points
                double[,] tempData = new double[50, 2];

                int fundsCount = 5;
                Assets.Clear();
                double minX = double.MaxValue;
                double maxX = double.MinValue;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;
                    //int		i = 0;

                    if ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');
                        // parse X
                        fundsCount = strs.Length - 1;
                        //Funds.Capacity = fundsCount;
                        for (int i = 0; i < fundsCount; i++)
                        {
                            TimeSerie current = new TimeSerie();
                            current.name = strs[i + 1];
                            //Funds[i] = current;
                            Assets.Add(current);
                        }

                    }
                    // read the data
                    //while ((i < 50) && ((str = reader.ReadLine()) != null))
                    while ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');
                        // parse X
                        DateTime date = DateTime.Parse(strs[0]);

                        for (int i = 0; i < fundsCount; i++)
                        {
                            TimeSerie.DayValue dayValue = new TimeSerie.DayValue();
                            dayValue.Day = date;
                            dayValue.Value = double.Parse(strs[i + 1]);
                            Assets[i].DataSerie.Add(dayValue);

                            // search for min value
                            if (dayValue.Value < minX)
                                minX = dayValue.Value;
                            // search for max value
                            if (dayValue.Value > maxX)
                                maxX = dayValue.Value;
                        }

                        //i++;
                    }
                    populationSize = Assets.Count * 5;//preporuka
                    populationSizeBox.Text = populationSize.ToString();
                    // allocate and set data
                    //data = new double[i, 2];
                    //Array.Copy( tempData, 0, data, 0, i * 2 );
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

                // update list and chart
                //////////////UpdateDataListView( );
                //////////////chart.RangeX = new DoubleRange( minX, maxX );
                //////////////chart.UpdateDataSeries( "data", data );
                //////////////chart.UpdateDataSeries( "solution", null );
                // enable "Start" button
                startButton.Enabled = true;
            }
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            dataList.Items.Clear();
            // add new records
            for (int i = 0, n = data.GetLength(0); i < n; i++)
            {
                dataList.Items.Add(data[i, 0].ToString());
                dataList.Items[i].SubItems.Add(data[i, 1].ToString());
            }
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void EnableCallback(bool enable);

        // Enable/disale controls (safe for threading)
        private void EnableControls(bool enable)
        {
            if (InvokeRequired)
            {
                EnableCallback d = new EnableCallback(EnableControls);
                Invoke(d, new object[] { enable });
            }
            else
            {
                loadDataButton.Enabled = enable;
                populationSizeBox.Enabled = enable;
                iterationsBox.Enabled = enable;
                selectionBox.Enabled = enable;
                functionsSetBox.Enabled = enable;
                geneticMethodBox.Enabled = enable;
                fitnessCriteria1Box.Enabled = enable;
                fitnessCriteria2Box.Enabled = enable;

                startButton.Enabled = enable;
                stopButton.Enabled = !enable;
            }
        }

        void IterationCounterChanged(int iterationCount)
        {
            SetText(currentIterationBox, iterationCount.ToString());
        }


        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            solutionBox.Text = string.Empty;
            parametersBox.Text = string.Empty;

            // get population size
            try
            {
                populationSize = Math.Max(10, Math.Min(1000, int.Parse(populationSizeBox.Text)));
            }
            catch
            {
                populationSize = 40;
            }
            // iterations
            try
            {
                iterations = Math.Max(0, int.Parse(iterationsBox.Text));
            }
            catch
            {
                iterations = 100;
            }
            // evaluation period
            try
            {
                evaluationPeriod = Math.Max(0, int.Parse(evaluationPeriodBox.Text));
            }
            catch
            {
                evaluationPeriod = 12;
            }
            // Return period
            try
            {
                ReturnPeriod = Math.Max(0, int.Parse(ReturnPeriodBox.Text));
            }
            catch
            {
                ReturnPeriod = 12;
            }
            // evaluation start date
            try
            {
                evaluationEndDate = this.dateTimePicker1.Value;
            }
            catch
            {
                //ReturnPeriod = 12;
            }
            interestRate = double.Parse(interestRateBox.Text);
            varThreshold = double.Parse(varThresholdBox.Text);
            weightParamDiscret = int.Parse(weightParamDiscretbox.Text);

            // update settings controls
            //UpdateSettings( );

            selectionMethod = selectionBox.SelectedIndex;
            functionsSet = functionsSetBox.SelectedIndex;
            geneticMethod = geneticMethodBox.SelectedIndex;
            evaluationCriteria[0] = (PortfolioOptimization.EvaluationCriteria)fitnessCriteria1Box.SelectedIndex;
            evaluationCriteria[1] = (PortfolioOptimization.EvaluationCriteria)fitnessCriteria2Box.SelectedIndex;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            saveFileDialog.Title = "Save data file";
            //saveFileDialog.ShowDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            {
                sw = new StreamWriter(saveFileDialog.FileName, false);
            }
            else return;


            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));

            workerThread.Start();
        }

        // On button "Stop"
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // stop worker thread
            needToStop = true;
            while (!workerThread.Join(100))
                Application.DoEvents();
            workerThread = null;
        }

        // Worker thread
        void SearchSolution()
        {
            int testingPeriod = 126;
            VaRExpVaRComparison VEVC = new VaRExpVaRComparison(Assets, evaluationEndDate, evaluationPeriod, ReturnPeriod, varThreshold, PortfolioOptimization.DecisionVariableType.capitalInvestedProportionType,testingPeriod);
            VEVC.IterationCounterChanged += new VaRExpVaRComparison.IterationCounterChangedHandler(IterationCounterChanged);
            VEVC.Execute(sw);
                
            // enable settings controls
            EnableControls(true);
        }

        
        //void ExecuteAlgorithm(ref JARE.Base.SolutionSet s, double[] weightParameter, double[] weightCriteriaOptimalValues)
        //{
        //    InitOptimization(weightParameter, weightCriteriaOptimalValues);
        //    ((PortfolioOptimizationGA)algorithm).Init();

        //    // iterations
        //    int iteration = 1;
        //    double previousFitnessValue = 1.0e+10;
        //    //// calculate fitness
        //    double fitnessValue = 0.0;
        //    double iterToler = 1.0e-5;
        //    // loop
        //    do
        //    {
        //        s = algorithm.execute();

        //        try
        //        {
        //            // get best solution
        //            //JARE.Base.Solution solution = s.getSolution(0);
        //            JARE.Base.Solution solution = s.getSolution(s.size() - 1);

        //            // set current iteration's info
        //            SetText(currentIterationBox, iteration.ToString());
        //            //error = problem.evaluate(solution);
        //            fitnessValue = solution.Fitness;
        //            SetText(currentErrorBox, fitnessValue.ToString("E05"));
        //        }
        //        catch
        //        {
        //            // remove any solutions from chart in case of any errors
        //            chart.UpdateDataSeries("solution", null);
        //        }

        //        // increase current iteration
        //        if (Math.Abs(fitnessValue-previousFitnessValue) < iterToler) iteration++;
        //        else
        //        {
        //            previousFitnessValue = fitnessValue;
        //            iteration = 1;
        //        }
        //        //
        //        if ((iterations != 0) && (iteration > iterations))
        //            break;
        //    } while (!needToStop);
        //}


        private void buttonLoadWeightParam_Click(object sender, EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;

                    weightParameterList.Clear();
                    weightParamDiscret = 0;
                    // read the data
                    while ((str = reader.ReadLine()) != null)
                    {
                        weightParameterList.Add(double.Parse(str));
                    }
                    weightParamDiscret = weightParameterList.Count;
                    weightParamDiscretbox.Text = weightParamDiscret.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }
            }
        }
    }
}