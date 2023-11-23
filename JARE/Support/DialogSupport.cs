using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// Support class for creation of Forms behaving like Dialog windows.
    /// </summary>
    public class DialogSupport
    {
        /// <summary>
        /// Creates a dialog Form.
        /// </summary>
        /// <returns>The new dialog Form instance.</returns>
        public static System.Windows.Forms.Form CreateDialog()
        {
            System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
            tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            tempForm.ShowInTaskbar = false;
            return tempForm;
        }

        /// <summary>
        /// Sets dialog like properties to a Form.
        /// </summary>
        /// <param name="formInstance">Form instance to be modified.</param>
        public static void SetDialog(System.Windows.Forms.Form formInstance)
        {
            formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            formInstance.ShowInTaskbar = false;
        }

        /// <summary>
        /// Creates a dialog Form with an owner.
        /// </summary>
        /// <param name="owner">The form to be set as owner of the newly created one.</param>
        /// <returns>A new dialog Form.</returns>
        public static System.Windows.Forms.Form CreateDialog(System.Windows.Forms.Form owner)
        {
            System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
            tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            tempForm.ShowInTaskbar = false;
            tempForm.Owner = owner;
            return tempForm;
        }

        /// <summary>
        /// Sets dialog like properties and an owner to a Form.
        /// </summary>
        /// <param name="formInstance">Form instance to be modified.</param>
        /// <param name="owner">The form to be set as owner of the newly created one.</param>
        public static void SetDialog(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner)
        {
            formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            formInstance.ShowInTaskbar = false;
            formInstance.Owner = owner;
        }


        /// <summary>
        /// Creates a dialog Form with an owner and a text property.
        /// </summary>
        /// <param name="owner">The form to be set as owner of the newly created one.</param>
        /// <param name="text">The title text for the form.</param>
        /// <returns>The new dialog Form.</returns>
        public static System.Windows.Forms.Form CreateDialog(System.Windows.Forms.Form owner, System.String text)
        {
            System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
            tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            tempForm.ShowInTaskbar = false;
            tempForm.Owner = owner;
            tempForm.Text = text;
            return tempForm;
        }

        /// <summary>
        /// Sets dialog like properties, an owner and a text property to a Form.
        /// </summary>
        /// <param name="formInstance">Form instance to be modified.</param>
        /// <param name="owner">The form to be set as owner of the newly created one.</param>
        /// <param name="text">The title text for the form.</param>
        public static void SetDialog(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner, System.String text)
        {
            formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            formInstance.ShowInTaskbar = false;
            formInstance.Owner = owner;
            formInstance.Text = text;
        }


        /// <summary>
        /// This method sets or unsets a resizable border style to a Form.
        /// </summary>
        /// <param name="formInstance">The form to be modified.</param>
        /// <param name="sizable">Boolean value to be set.</param>
        public static void SetSizable(System.Windows.Forms.Form formInstance, bool sizable)
        {
            if (sizable)
            {
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            }
            else
            {
                formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            }
        }
    }
}
