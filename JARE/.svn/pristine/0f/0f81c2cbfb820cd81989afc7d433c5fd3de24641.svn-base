using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// Give functions to obtain information of graphic elements
    /// </summary>
    public class GraphicsManager
    {
        //Instance of GDI+ drawing surfaces graphics hashtable
        static public GraphicsHashTable manager = new GraphicsHashTable();

        /// <summary>
        /// Creates a new Graphics object from the device context handle associated with the Graphics
        /// parameter
        /// </summary>
        /// <param name="oldGraphics">Graphics instance to obtain the parameter from</param>
        /// <returns>A new GDI+ drawing surface</returns>
        public static System.Drawing.Graphics CreateGraphics(System.Drawing.Graphics oldGraphics)
        {
            System.Drawing.Graphics createdGraphics;
            System.IntPtr hdc = oldGraphics.GetHdc();
            createdGraphics = System.Drawing.Graphics.FromHdc(hdc);
            oldGraphics.ReleaseHdc(hdc);
            return createdGraphics;
        }

        /// <summary>
        /// This method draws a Bezier curve.
        /// </summary>
        /// <param name="graphics">It receives the Graphics instance</param>
        /// <param name="array">An array of (x,y) pairs of coordinates used to draw the curve.</param>
        public static void Bezier(System.Drawing.Graphics graphics, int[] array)
        {
            System.Drawing.Pen pen;
            pen = GraphicsManager.manager.GetPen(graphics);
            try
            {
                graphics.DrawBezier(pen, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
            }
            catch (System.IndexOutOfRangeException e)
            {
                throw new System.IndexOutOfRangeException(e.ToString());
            }
        }

        /// <summary>
        /// Gets the text size width and height from a given GDI+ drawing surface and a given font
        /// </summary>
        /// <param name="graphics">Drawing surface to use</param>
        /// <param name="graphicsFont">Font type to measure</param>
        /// <param name="text">String of text to measure</param>
        /// <returns>A point structure with both size dimentions; x for width and y for height</returns>
        public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text)
        {
            System.Drawing.Point textSize;
            System.Drawing.SizeF tempSizeF;
            tempSizeF = graphics.MeasureString(text, graphicsFont);
            textSize = new System.Drawing.Point();
            textSize.X = (int)tempSizeF.Width;
            textSize.Y = (int)tempSizeF.Height;
            return textSize;
        }

        /// <summary>
        /// Gets the text size width and height from a given GDI+ drawing surface and a given font
        /// </summary>
        /// <param name="graphics">Drawing surface to use</param>
        /// <param name="graphicsFont">Font type to measure</param>
        /// <param name="text">String of text to measure</param>
        /// <param name="width">Maximum width of the string</param>
        /// <param name="format">StringFormat object that represents formatting information, such as line spacing, for the string</param>
        /// <returns>A point structure with both size dimentions; x for width and y for height</returns>
        public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text, System.Int32 width, System.Drawing.StringFormat format)
        {
            System.Drawing.Point textSize;
            System.Drawing.SizeF tempSizeF;
            tempSizeF = graphics.MeasureString(text, graphicsFont, width, format);
            textSize = new System.Drawing.Point();
            textSize.X = (int)tempSizeF.Width;
            textSize.Y = (int)tempSizeF.Height;
            return textSize;
        }
    }
}