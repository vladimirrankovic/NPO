using System;
namespace jmetal.gui.components
{
	
	[Serializable]
	public class JMetalFileChooser:System.Windows.Forms.Form
	{
		
		/// <summary> </summary>
		private const long serialVersionUID = - 2015168568267285572L;
		private System.Windows.Forms.FileDialog fileChooser_;
		/// <summary> Returns the file chosen by the user.
		/// 
		/// </summary>
		/// <returns> The file chosen by the user.
		/// </returns>
		public virtual System.IO.FileInfo chooseFile()
		{
			if (fileChooser_ == null)
			{
				//UPGRADE_TODO: Constructor may need to be changed depending on function performed by the 'System.Windows.Forms.FileDialog' object. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1270'"
				fileChooser_ = new System.Windows.Forms.OpenFileDialog();
				//UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileSelectionMode_int'"
				fileChooser_.setFileSelectionMode(1);
			}
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.JFileChooser.APPROVE_OPTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showOpenDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			if ((int) System.Windows.Forms.DialogResult.OK == (int) fileChooser_.ShowDialog(this))
			{
				return new System.IO.FileInfo(fileChooser_.FileName);
			}
			else
			{
				return null;
			}
		}
	} // JMetalFileChooser
}