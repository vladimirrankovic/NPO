//
// In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
// creates "support classes" that duplicate the original functionality.  
//
// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;
namespace JARE.support{
	/// <summary>
	/// This interface should be implemented by any class whose instances are intended 
	/// to be executed by a thread.
	/// </summary>

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
    public class SupportClass
    {
        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="throwable">Exception to obtain information from</param>
        /// <param name="stream">Output sream used to write to</param>
        public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }


        /*******************************/
        /// <summary>
        /// This method works as a handler for the Control.Layout event, it arranges the controls into a container
        /// control in a rectangular grid (rows and columns).
        /// The size and location of each inner control will be calculated according the number of them in the 
        /// container.
        /// The number of columns, rows, horizontal and vertical spacing between the inner controls will are
        /// specified as array of object values in the Tag property of the container.
        /// If the number of rows and columns specified is not enought to allocate all the controls, the number of 
        /// columns will be increased in order to maintain the number of rows specified.
        /// </summary>
        /// <param name="event_sender">The container control in which the controls will be relocated.</param>
        /// <param name="eventArgs">Data of the event.</param>
        public static void GridLayoutResize(System.Object event_sender, System.Windows.Forms.LayoutEventArgs eventArgs)
        {
            System.Windows.Forms.Control container = (System.Windows.Forms.Control)event_sender;
            if ((container.Tag is System.Drawing.Rectangle) && (container.Controls.Count > 0))
            {
                System.Drawing.Rectangle tempRectangle = (System.Drawing.Rectangle)container.Tag;

                if ((tempRectangle.X <= 0) && (tempRectangle.Y <= 0))
                    throw new System.Exception("Illegal column and row layout count specified");

                int horizontal = tempRectangle.Width;
                int vertical = tempRectangle.Height;
                int count = container.Controls.Count;

                int rows = (tempRectangle.X == 0) ? (int)System.Math.Ceiling((double)(count / tempRectangle.Y)) : tempRectangle.X;
                int columns = (tempRectangle.Y == 0) ? (int)System.Math.Ceiling((double)(count / tempRectangle.X)) : tempRectangle.Y;

                rows = (rows == 0) ? 1 : rows;
                columns = (columns == 0) ? 1 : columns;

                while (count > rows * columns) columns++;

                int width = (container.DisplayRectangle.Width - horizontal * (columns - 1)) / columns;
                int height = (container.DisplayRectangle.Height - vertical * (rows - 1)) / rows;

                int indexColumn = 0;
                int indexRow = 0;

                foreach (System.Windows.Forms.Control tempControl in container.Controls)
                {
                    int xCoordinate = indexColumn++ * (width + horizontal);
                    int yCoordinate = indexRow * (height + vertical);
                    tempControl.Location = new System.Drawing.Point(xCoordinate, yCoordinate);
                    tempControl.Width = width;
                    tempControl.Height = height;
                    if (indexColumn == columns)
                    {
                        indexColumn = 0;
                        indexRow++;
                    }
                }
            }
        }

        // visnja: stavljeno pod komentar jer ga niko ne poziva

        /*******************************/
        /// <summary>
        /// Verifies if a value exist in a NameValueCollection.
        /// </summary>
        /// <param name="collection">The NameValueCollection to look in.</param>
        /// <param name="key">The key to look for.</param>
        /// <returns>If key exist in the NameValueCollection returns true, otherwise false.</returns>
        //public static bool ContainsKeySupport(System.Collections.IDictionary collection, string key) ///////////////////////////////////////////
        //{
        //    bool exists = false;
        //    if (collection != null)
        //    {
        //        exists = collection.Contains(key);

        //        //string[] keys = collection.AllKeys;
        //        //exists = !(System.Array.IndexOf(keys, key) == -1);
        //    }
        //    return exists;
        //}

        /*******************************/
        /// <summary>
        /// Converts an array of sbytes to an array of chars
        /// </summary>
        /// <param name="sByteArray">The array of sbytes to convert</param>
        /// <returns>The new array of chars</returns>
        public static char[] ToCharArray(sbyte[] sByteArray)
        {
            return System.Text.UTF8Encoding.UTF8.GetChars(ToByteArray(sByteArray));
        }

        /// <summary>
        /// Converts an array of bytes to an array of chars
        /// </summary>
        /// <param name="byteArray">The array of bytes to convert</param>
        /// <returns>The new array of chars</returns>
        public static char[] ToCharArray(byte[] byteArray)
        {
            return System.Text.UTF8Encoding.UTF8.GetChars(byteArray);
        }

        /*******************************/
        /// <summary>
        /// Converts an array of sbytes to an array of bytes
        /// </summary>
        /// <param name="sbyteArray">The array of sbytes to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = null;

            if (sbyteArray != null)
            {
                byteArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; index++)
                    byteArray[index] = (byte)sbyteArray[index];
            }
            return byteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(System.String sourceString)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(sourceString);
        }

        /// <summary>
        /// Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(System.Object[] tempObjectArray)
        {
            byte[] byteArray = null;
            if (tempObjectArray != null)
            {
                byteArray = new byte[tempObjectArray.Length];
                for (int index = 0; index < tempObjectArray.Length; index++)
                    byteArray[index] = (byte)tempObjectArray[index];
            }
            return byteArray;
        }






        /*******************************/
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an output file stream to write to the file with the specified name.
        /// </summary>
        /// <param name="FileName">Name of the file to write.</param>
        /// <param name="Append">True in order to write to the end of the file, false otherwise.</param>
        /// <returns>New instance of FileStream with the proper file mode.</returns>
        public static System.IO.FileStream GetFileStream(System.String FileName, bool Append)
        {
            if (Append)
                return new System.IO.FileStream(FileName, System.IO.FileMode.Append);
            else
                return new System.IO.FileStream(FileName, System.IO.FileMode.Create);
        }




        /*******************************/
        /// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array
        /// </summary>
        /// <param name="byteArray">Byte array to process</param>
        /// <returns>The transformed array</returns>
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] sbyteArray = null;
            if (byteArray != null)
            {
                sbyteArray = new sbyte[byteArray.Length];
                for (int index = 0; index < byteArray.Length; index++)
                    sbyteArray[index] = (sbyte)byteArray[index];
            }
            return sbyteArray;
        }
    }
}
