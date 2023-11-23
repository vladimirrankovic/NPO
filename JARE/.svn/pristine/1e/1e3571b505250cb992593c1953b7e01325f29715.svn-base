using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// This class supports basic functionality of the JOptionPane class.
    /// </summary>
    public class OptionPaneSupport
    {
        /// <summary>
        /// This method finds the form which contains an specific control.
        /// </summary>
        /// <param name="control">The control which we need to find its form.</param>
        /// <returns>The form which contains the control</returns>
        public static System.Windows.Forms.Form GetFrameForComponent(System.Windows.Forms.Control control)
        {
            System.Windows.Forms.Form result = null;
            if (control == null) return null;
            if (control is System.Windows.Forms.Form)
                result = (System.Windows.Forms.Form)control;
            else if (control.Parent != null)
                result = GetFrameForComponent(control.Parent);
            return result;
        }

        /// <summary>
        /// This method finds the MDI container form which contains an specific control.
        /// </summary>
        /// <param name="control">The control which we need to find its MDI container form.</param>
        /// <returns>The MDI container form which contains the control.</returns>
        public static System.Windows.Forms.Form GetDesktopPaneForComponent(System.Windows.Forms.Control control)
        {
            System.Windows.Forms.Form result = null;
            if (control == null) return null;
            if (control.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form)))
                if (((System.Windows.Forms.Form)control).IsMdiContainer)
                    result = (System.Windows.Forms.Form)control;
                else if (((System.Windows.Forms.Form)control).IsMdiChild)
                    result = GetDesktopPaneForComponent(((System.Windows.Forms.Form)control).MdiParent);
                else if (control.Parent != null)
                    result = GetDesktopPaneForComponent(control.Parent);
            return result;
        }

        /// <summary>
        /// This method retrieves the message that is contained into the object that is sended by the user.
        /// </summary>
        /// <param name="control">The control which we need to find its form.</param>
        /// <returns>The form which contains the control</returns>
        public static System.String GetMessageForObject(System.Object message)
        {
            System.String result = "";
            if (message == null)
                return result;
            else
                result = message.ToString();
            return result;
        }


        /// <summary>
        /// This method displays a dialog with a Yes, No, Cancel buttons and a question icon.
        /// </summary>
        /// <param name="parent">The component which will be the owner of the dialog.</param>
        /// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
        /// <returns>The integer value which represents the button pressed.</returns>
        public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message)
        {
            return ShowConfirmDialog(parent, message, "Select an option...", (int)System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                (int)System.Windows.Forms.MessageBoxIcon.Question);
        }

        /// <summary>
        /// This method displays a dialog with specific buttons and a question icon.
        /// </summary>
        /// <param name="parent">The component which will be the owner of the dialog.</param>
        /// <param name="message">The message to be displayed; if it isn't an String it displays the result value of the ToString() method of the object.</param>
        /// <param name="title">The title for the message dialog.</param>
        /// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
        /// <returns>The integer value which represents the button pressed.</returns>
        public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
            System.String title, int optiontype)
        {
            return ShowConfirmDialog(parent, message, title, optiontype, (int)System.Windows.Forms.MessageBoxIcon.Question);
        }

        /// <summary>
        /// This method displays a dialog with specific buttons and specific icon.
        /// </summary>
        /// <param name="parent">The component which will be the owner of the dialog.</param>
        /// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
        /// <param name="title">The title for the message dialog.</param>
        /// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
        /// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
        /// <returns>The integer value which represents the button pressed.</returns>
        public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
            System.String title, int optiontype, int messagetype)
        {
            return (int)System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
                (System.Windows.Forms.MessageBoxButtons)optiontype, (System.Windows.Forms.MessageBoxIcon)messagetype);
        }

        /// <summary>
        /// This method displays a simple MessageBox.
        /// </summary>
        /// <param name="parent">The component which will be the owner of the dialog.</param>
        /// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
        public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message)
        {
            ShowMessageDialog(parent, message, "Message", (int)System.Windows.Forms.MessageBoxIcon.Information);
        }

        /// <summary>
        /// This method displays a simple MessageBox with a specific icon.
        /// </summary>
        /// <param name="parent">The component which will be the owner of the dialog.</param>
        /// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
        /// <param name="title">The title for the message dialog.</param>
        /// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
        public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message,
            System.String title, int messagetype)
        {
            System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
                System.Windows.Forms.MessageBoxButtons.OK, (System.Windows.Forms.MessageBoxIcon)messagetype);
        }
    }
}
