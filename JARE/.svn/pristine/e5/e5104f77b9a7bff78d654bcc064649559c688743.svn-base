using System;
using MetricsUtil = jmetal.qualityIndicator.util.MetricsUtil;
namespace jmetal.gui.plot
{
	/// <summary> Displays a JFrame and draws a line on it using the Java 2D Graphics API
	/// 
	/// </summary>
	/// <author>  www.javadb.com
	/// </author>
	[Serializable]
	public class Plot2D:System.Windows.Forms.Panel
	{
		private void  InitBlock()
		{
			for (int i = 0; i < 2; i++)
			{
				objsValues[i] = new double[2];
			}
		}
		
		private int width_ = 400;
		private int height_ = 400;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public java.util.List < double [] [] > points_;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public java.util.List < Color > colors_;
		private MetricsUtil utils_ = new MetricsUtil();
		internal double[][] objsValues = new double[2][];
		internal System.Drawing.PointF start
		{
			get
			{
				return start_Renamed;
			}
			
			set
			{
				start_Renamed = value;
			}
			
		}
		internal System.Drawing.PointF end
		{
			get
			{
				return end_Renamed;
			}
			
			set
			{
				end_Renamed = value;
			}
			
		}
		internal System.Drawing.PointF start_Renamed, end_Renamed;
		
		public Plot2D()
		{
			InitBlock();
			System.Drawing.Size d = new System.Drawing.Size(width_, height_);
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
			Size = d;
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMaximumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMaximumSize_javaawtDimension'"
			setMaximumSize(d);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
			setMinimumSize(d);
			
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			points_ = new LinkedList < double [] [] >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			colors_ = new LinkedList < Color >();
			setBackground(Color.WHITE);
			
			objsValues[0][0] = 0.0;
			objsValues[0][1] = 1.0;
			objsValues[1][0] = 0.0;
			objsValues[1][1] = 1.0;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public virtual void  refresh(System.String path, ref System.Drawing.Color color, bool holdOn)
		{
			if (!holdOn)
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				points_ = new LinkedList < double [] [] >();
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				colors_ = new LinkedList < Color >();
			}
			
			double[][] aux = utils_.readFront(path);
			points_.add(aux);
			colors_.add(color);
		}
		
		
		public virtual void  drawAxis(System.Drawing.Graphics g2)
		{
			g2.setBackground(Color.WHITE);
			g2.FillRegion(new System.Drawing.SolidBrush(SupportClass.GraphicsManager.manager.GetBackColor(g2)), new System.Drawing.Region(new System.Drawing.Rectangle(1, 1, width_ + 100, height_ + 100)));
			
			start = new System.Drawing.PointF((float) 30.0, (float) 30.0);
			end = new System.Drawing.PointF((float) ((double) start.X + width_), (float) ((double) start.Y + height_));
			
			System.Drawing.PointF[] verticalPoints = new System.Drawing.PointF[5];
			for (int i = 0; i < verticalPoints.Length; i++)
			{
				verticalPoints[i] = new System.Drawing.PointF((float) start.X, (float) ((double) start.Y + i * (height_ / 5.0)));
				System.Drawing.Drawing2D.GraphicsPath line = SupportClass.Line2DSupport.CreateLine2DPath((float) ((double) verticalPoints[i].X - 2.5), (float) verticalPoints[i].Y, (float) ((double) verticalPoints[i].X + 2.5), (float) verticalPoints[i].Y);
				//UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
				g2.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g2), line);
			}
			
			System.Drawing.PointF[] horizontalPoints = new System.Drawing.PointF[5];
			for (int i = 0; i < horizontalPoints.Length; i++)
			{
				horizontalPoints[i] = new System.Drawing.PointF((float) ((double) start.X + (i + 1) * (width_ / 5.0)), (float) end.Y);
				System.Drawing.Drawing2D.GraphicsPath line = SupportClass.Line2DSupport.CreateLine2DPath((float) horizontalPoints[i].X, (float) ((double) horizontalPoints[i].Y - 2.5), (float) horizontalPoints[i].X, (float) ((double) horizontalPoints[i].Y + 2.5));
				//UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
				g2.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g2), line);
			}
			
			
			
			
			double[] verticalAxis = new double[5];
			for (int i = 0; i < verticalAxis.Length; i++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				verticalAxis[i] = (float) objsValues[1][1] - (float) i * (float) ((float) objsValues[1][1] - (float) objsValues[1][0]) / 5.0;
				
				//UPGRADE_TODO: Class 'java.math.BigDecimal' was converted to 'System.Decimal' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javamathBigDecimal'"
				//UPGRADE_TODO: Method 'java.math.BigDecimal.setScale' was converted to 'System.Decimal.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javamathBigDecimalsetScale_int_int'"
				System.Decimal round = System.Decimal.Round(new System.Decimal(verticalAxis[i]), 2);
				System.Double doubleValue = (double) System.Decimal.ToDouble(round);
				verticalAxis[i] = doubleValue;
				
				//UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				g2.DrawString(((double) verticalAxis[i]) + "", SupportClass.GraphicsManager.manager.GetFont(g2), SupportClass.GraphicsManager.manager.GetPaint(g2), (float) 0.0, (float) ((double) verticalPoints[i].Y + 5.0) - SupportClass.GraphicsManager.manager.GetFont(g2).GetHeight());
			}
			
			double[] horizontalAxis = new double[5];
			for (int i = 0; i < horizontalAxis.Length; i++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				horizontalAxis[i] = (float) objsValues[0][0] + (float) (i + 1) * (float) ((float) objsValues[0][1] - (float) objsValues[0][0]) / 5.0;
				
				//UPGRADE_TODO: Class 'java.math.BigDecimal' was converted to 'System.Decimal' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javamathBigDecimal'"
				//UPGRADE_TODO: Method 'java.math.BigDecimal.setScale' was converted to 'System.Decimal.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javamathBigDecimalsetScale_int_int'"
				System.Decimal round = System.Decimal.Round(new System.Decimal(horizontalAxis[i]), 2);
				System.Double doubleValue = (double) System.Decimal.ToDouble(round);
				horizontalAxis[i] = doubleValue;
				
				//UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				g2.DrawString(((double) horizontalAxis[i]) + "", SupportClass.GraphicsManager.manager.GetFont(g2), SupportClass.GraphicsManager.manager.GetPaint(g2), (float) ((double) horizontalPoints[i].X - 10.0), (float) ((double) end.Y + 20.0) - SupportClass.GraphicsManager.manager.GetFont(g2).GetHeight());
			}
			
			
			System.Drawing.Drawing2D.GraphicsPath verticalLine = SupportClass.Line2DSupport.CreateLine2DPath((float) start.X, (float) start.X, (float) start.Y, (float) end.Y);
			System.Drawing.Drawing2D.GraphicsPath horizontalLine = SupportClass.Line2DSupport.CreateLine2DPath((float) start.X, (float) end.X, (float) end.Y, (float) end.Y);
			
			//UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
			g2.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g2), verticalLine);
			//UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
			g2.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g2), horizontalLine);
		}
		
		public virtual void  drawChart(System.Drawing.Graphics g2)
		{
			double[] maxValues = null;
			double[] minValues = null;
			if (points_.size() > 0)
			{
				double[][] aux = points_.get_Renamed(0);
				maxValues = utils_.getMaximumValues(aux, 2);
				minValues = utils_.getMinimumValues(aux, 2);
				for (int i = 1; i < points_.size(); i++)
				{
					double[][] otherPoints = points_.get_Renamed(i);
					double[] candidateMaxValues = utils_.getMaximumValues(otherPoints, 2);
					double[] candidateMinValues = utils_.getMinimumValues(otherPoints, 2);
					for (int j = 0; j < maxValues.Length; j++)
					{
						if (candidateMaxValues[j] > maxValues[j])
							maxValues[j] = candidateMaxValues[j];
						if (candidateMinValues[j] < minValues[j])
							minValues[j] = candidateMinValues[j];
					}
				}
				
				objsValues[0][0] = minValues[0];
				objsValues[0][1] = maxValues[0];
				objsValues[1][0] = minValues[1];
				objsValues[1][1] = maxValues[1];
			}
			drawAxis(g2);
			
			if (points_.size() > 0)
			{
				
				System.Drawing.RectangleF auxRectangle;
				
				for (int p = 0; p < points_.size(); p++)
				{
					double[][] currentPoints = points_.get_Renamed(p);
					System.Drawing.Color currentColor = colors_.get_Renamed(p);
					SupportClass.GraphicsManager.manager.SetColor(g2, currentColor);
					
					float[] tmpPoint;
					for (int i = 0; i < currentPoints.Length; i++)
					{
						tmpPoint = new float[2];
						
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						tmpPoint[0] = (float) ((float) start.X + ((width_ * (currentPoints[i][0] - objsValues[0][0])) / (objsValues[0][1] - objsValues[0][0])));
						
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						tmpPoint[1] = (float) ((float) start.Y + height_ - ((height_ * (currentPoints[i][1] - objsValues[1][0])) / (objsValues[1][1] - objsValues[1][0])));
						
						
						auxRectangle = new System.Drawing.RectangleF((float) tmpPoint[0], (float) tmpPoint[1], (float) 1.0, (float) 1.0);
						
						
						
						//UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
						g2.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g2), auxRectangle);
					}
				}
			}
		}
		
		
		//    public void drawChart(Graphics2D g2) {
		//        g2.clearRect(0, 0, width_, height_);
		//
		//        g2.setBackground(Color.WHITE);
		//        corner_ = getLocation();
		//        Line2D verticalLine = new Line2D.Double(5.0, 5.0, 5.0, height_+5.0);
		//        Line2D [] semiVerticalLines = new Line2D[5];
		//        for (int i = 0; i < semiVerticalLines.length; i++) {
		//          semiVerticalLines[i] = new Line2D.Double(2.5, 5.0 + i * (height_/5.0) , 7.5, 5.0 + i * (height_/5.0));
		//        }
		//
		//
		//        Line2D horizontalLine = new Line2D.Double(5.0,height_ + 5.0,width_+ 5.0,height_ + 5.0);
		//        Line2D [] semiHorizontalLines = new Line2D[5];
		//        for (int i = 0; i < semiHorizontalLines.length; i++) {
		//          semiHorizontalLines[i] = new Line2D.Double(5.0 + (i+1) * (width_/5.0),height_ + 2.5 ,5.0 + (i+1) * (width_/5.0),height_ + 7.5);
		//        }
		//
		//
		//        g2.setColor(Color.BLACK);
		//        g2.draw(verticalLine);
		//
		//
		//        for (int i = 0; i < semiVerticalLines.length; i++) {
		//          g2.draw(semiVerticalLines[i]);
		//          g2.draw(semiHorizontalLines[i]);
		//        }
		//
		//        g2.draw(horizontalLine);
		//
		//        if (points_.size() > 0) {
		//          double [][] aux = points_.get(0);
		//          double [] maxValues = utils_.getMaximumValues(aux, 2);
		//          double [] minValues = utils_.getMinimumValues(aux, 2);
		//          for (int i = 1; i < points_.size(); i++) {
		//            double [][] otherPoints = points_.get(i);
		//            double [] candidateMaxValues = utils_.getMaximumValues(otherPoints, 2);
		//            double [] candidateMinValues = utils_.getMinimumValues(otherPoints, 2);
		//            for (int j = 0; j < maxValues.length; j++) {
		//              if (candidateMaxValues[j] > maxValues[j])
		//                maxValues[j] = candidateMaxValues[j];
		//              if (candidateMinValues[j] < minValues[j])
		//                minValues[j] = candidateMinValues[j];
		//            }
		//          }
		//
		//          float [] ratio     = new float[2];
		//          ratio[0] = (float) ((width_ - 10) / (maxValues[0] - minValues[0]));
		//          ratio[1] = (float) ((height_- 10) / (maxValues[1] - minValues[1]));
		//
		//
		//          Rectangle2D auxRectangle;
		//
		//          for (int p = 0; p < points_.size(); p++) {
		//            double [][] currentPoints = points_.get(p);
		//            Color currentColor        = colors_.get(p);
		//            g2.setColor(currentColor);
		//
		//            float [] tmpPoint;
		//            for (int i = 0; i < currentPoints.length; i++) {
		//              tmpPoint = new float[2];
		//
		//              tmpPoint[0] = (float)Math.floor(((currentPoints[i][0] - minValues[0])/(maxValues[0]-minValues[0]))*ratio[0]) ;
		//              tmpPoint[1] = (float)Math.ceil(((currentPoints[i][1] - minValues[1])/(maxValues[1]-minValues[1]))*ratio[1]) ;
		//              auxRectangle = new Rectangle2D.Double(Math.floor(tmpPoint[0]+5.0),
		//                                            Math.ceil((height_ -5.0) - tmpPoint[1]),
		//                                            3.0,
		//                                            3.0);
		//
		//
		//
		//              g2.draw(auxRectangle);
		//
		//            }
		//          }
		//        }
		//    }
		
		
		/// <summary> This is the method where the line is drawn.
		/// 
		/// </summary>
		/// <param name="g">The graphics object
		/// </param>
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			
			// Cast the Graphic object to Graphic2D, in order to work with Java2D
			System.Drawing.Graphics g2 = (System.Drawing.Graphics) g;
			
			drawChart(g2);
		}
	}
}