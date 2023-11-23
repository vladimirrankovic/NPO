using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// This class contains support methods to work with GraphicsPath and Lines.
    /// </summary>
    public class Line2DSupport
    {
        /// <summary>
        /// Creates a GraphicsPath object and adds a line to it.
        /// </summary>
        /// <param name="x1">The x-coordinate of the starting point of the line.</param>
        /// <param name="y1">The y-coordinate of the starting point of the line.</param>
        /// <param name="x2">The x-coordinate of the endpoint of the line.</param>
        /// <param name="y2">The y-coordinate of the endpoint of the line.</param>
        /// <returns>Returns a GraphicsPath object containing the line.</returns>
        public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(float x1, float y1, float x2, float y2)
        {
            System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
            linePath.AddLine(x1, y1, x2, y2);
            return linePath;
        }

        /// <summary>
        /// Creates a GraphicsPath object and adds a line to it.
        /// </summary>
        /// <param name="p1">The starting point of the line.</param>
        /// <param name="p2">The endpoint of the line.</param>
        /// <returns>Returns a GraphicsPath object containing the line</returns>
        public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(System.Drawing.PointF p1, System.Drawing.PointF p2)
        {
            System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
            linePath.AddLine(p1, p2);
            return linePath;
        }

        /// <summary>
        /// Resets the specified GraphicsPath object an adds a line to it with the specified values.
        /// </summary>
        /// <param name="linePath">The GraphicsPath object to reset.</param>
        /// <param name="x1">The x-coordinate of the starting point of the line.</param>
        /// <param name="y1">The y-coordinate of the starting point of the line.</param>
        /// <param name="x2">The x-coordinate of the endpoint of the line.</param>
        /// <param name="y2">The y-coordinate of the endpoint of the line.</param>
        public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, float x1, float y1, float x2, float y2)
        {
            linePath.Reset();
            linePath.AddLine(x1, y1, x2, y2);
        }

        /// <summary>
        /// Resets the specified GraphicsPath object an adds a line to it with the specified values.
        /// </summary>
        /// <param name="linePath">The GraphicsPath object to reset.</param>
        /// <param name="p1">The starting point of the line.</param>
        /// <param name="p2">The endpoint of the line.</param>
        public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.PointF p1, System.Drawing.PointF p2)
        {
            linePath.Reset();
            linePath.AddLine(p1, p2);
        }

        /// <summary>
        /// Resets the specified GraphicsPath object an adds a line to it.
        /// </summary>
        /// <param name="linePath">The GraphicsPath object to reset.</param>
        /// <param name="newLinePath">The line to add.</param>
        public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.Drawing2D.GraphicsPath newLinePath)
        {
            linePath.Reset();
            linePath.AddPath(newLinePath, false);
        }
    }

}
