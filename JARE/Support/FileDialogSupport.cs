using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// Support Methods for FileDialog class. Note that several methods receive a DirectoryInfo object, but it won't be used in all cases.
    /// </summary>
    public class FileDialogSupport
    {
        /// <summary>
        /// Creates an OpenFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the OpenFileDialog.</param>
        /// <returns>A new instance of OpenFileDialog.</returns>
        public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path)
        {
            System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
            temp_fileDialog.InitialDirectory = path.Directory.FullName;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an OpenFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the OpenFileDialog.</param>
        /// <param name="directory">Directory to get the path from.</param>
        /// <returns>A new instance of OpenFileDialog.</returns>
        public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
        {
            System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
            temp_fileDialog.InitialDirectory = path.Directory.FullName;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates a OpenFileDialog open in a given path.
        /// </summary>		
        /// <returns>A new instance of OpenFileDialog.</returns>
        public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog()
        {
            System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
            temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an OpenFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the OpenFileDialog</param>
        /// <returns>A new instance of OpenFileDialog.</returns>
        public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.String path)
        {
            System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
            temp_fileDialog.InitialDirectory = path;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an OpenFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the OpenFileDialog.</param>
        /// <param name="directory">Directory to get the path from.</param>
        /// <returns>A new instance of OpenFileDialog.</returns>
        public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.String path, System.IO.DirectoryInfo directory)
        {
            System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
            temp_fileDialog.InitialDirectory = path;
            return temp_fileDialog;
        }

        /// <summary>
        /// Modifies an instance of OpenFileDialog, to open a given directory.
        /// </summary>
        /// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
        /// <param name="path">Path to be opened by the OpenFileDialog.</param>
        /// <param name="directory">Directory to get the path from.</param>
        public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path, System.IO.DirectoryInfo directory)
        {
            fileDialog.InitialDirectory = path;
        }

        /// <summary>
        /// Modifies an instance of OpenFileDialog, to open a given directory.
        /// </summary>
        /// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
        /// <param name="path">Path to be opened by the OpenFileDialog</param>
        public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.IO.FileInfo path)
        {
            fileDialog.InitialDirectory = path.Directory.FullName;
        }

        /// <summary>
        /// Modifies an instance of OpenFileDialog, to open a given directory.
        /// </summary>
        /// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
        /// <param name="path">Path to be opened by the OpenFileDialog.</param>
        public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path)
        {
            fileDialog.InitialDirectory = path;
        }

        ///
        ///  Use the following static methods to create instances of SaveFileDialog.
        ///  By default, JFileChooser is converted as an OpenFileDialog, the following methods
        ///  are provided to create file dialogs to save files.
        ///	


        /// <summary>
        /// Creates a SaveFileDialog.
        /// </summary>		
        /// <returns>A new instance of SaveFileDialog.</returns>
        public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog()
        {
            System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
            temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an SaveFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the SaveFileDialog.</param>
        /// <returns>A new instance of SaveFileDialog.</returns>
        public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path)
        {
            System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
            temp_fileDialog.InitialDirectory = path.Directory.FullName;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an SaveFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the SaveFileDialog.</param>
        /// <param name="directory">Directory to get the path from.</param>
        /// <returns>A new instance of SaveFileDialog.</returns>
        public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
        {
            System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
            temp_fileDialog.InitialDirectory = path.Directory.FullName;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates a SaveFileDialog open in a given path.
        /// </summary>
        /// <param name="directory">Directory to get the path from.</param>
        /// <returns>A new instance of SaveFileDialog.</returns>
        public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.DirectoryInfo directory)
        {
            System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
            temp_fileDialog.InitialDirectory = directory.FullName;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an SaveFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the SaveFileDialog</param>
        /// <returns>A new instance of SaveFileDialog.</returns>
        public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.String path)
        {
            System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
            temp_fileDialog.InitialDirectory = path;
            return temp_fileDialog;
        }

        /// <summary>
        /// Creates an SaveFileDialog open in a given path.
        /// </summary>
        /// <param name="path">Path to be opened by the SaveFileDialog.</param>
        /// <param name="directory">Directory to get the path from.</param>
        /// <returns>A new instance of SaveFileDialog.</returns>
        public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.String path, System.IO.DirectoryInfo directory)
        {
            System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
            temp_fileDialog.InitialDirectory = path;
            return temp_fileDialog;
        }
    }
}
