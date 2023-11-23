using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    public class FormSupport
    {
        /// <summary>
        /// Creates a Form instance and sets the property Text to the specified parameter.
        /// </summary>
        /// <param name="Text">Value for the Form property Text</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.String text)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Text = text;
            return tempForm;
        }

        /// <summary>
        /// Creates a Form instance and sets the property Text to the specified parameter.
        /// Adds the received control to the Form's Controls collection in the position 0.
        /// </summary>
        /// <param name="text">Value for the Form Text property.</param>
        /// <param name="control">Control to be added to the Controls collection.</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.String text, System.Windows.Forms.Control control)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Text = text;
            tempForm.Controls.Add(control);
            tempForm.Controls.SetChildIndex(control, 0);
            return tempForm;
        }


        /// <summary>
        /// Creates a Form instance and sets the property Owner to the specified parameter.
        /// This also sets the FormBorderStyle and ShowInTaskbar properties.
        /// </summary>
        /// <param name="Owner">Value for the Form property Owner</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.Windows.Forms.Form owner)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Owner = owner;
            tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            tempForm.ShowInTaskbar = false;
            return tempForm;
        }


        /// <summary>
        /// Creates a Form instance and sets the property Owner to the specified parameter.
        /// Sets the FormBorderStyle and ShowInTaskbar properties.
        /// Also add the received Control to the Form's Controls collection in the position 0.
        /// </summary>
        /// <param name="owner">Value for the Form property Owner</param>
        /// <param name="header">Control to be added to the Form's Controls collection</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.Windows.Forms.Form owner, System.Windows.Forms.Control header)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Owner = owner;
            tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            tempForm.ShowInTaskbar = false;
            tempForm.Controls.Add(header);
            tempForm.Controls.SetChildIndex(header, 0);
            return tempForm;
        }

        /// <summary>
        /// Creates a Form instance and sets the FormBorderStyle property.
        /// </summary>
        /// <param name="title">The title of the Form</param>
        /// <param name="resizable">Boolean value indicating if the Form is or not resizable</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.String title, bool resizable)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Text = title;
            if (resizable)
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            else
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            tempForm.TopLevel = false;
            tempForm.MaximizeBox = false;
            tempForm.MinimizeBox = false;
            return tempForm;
        }

        /// <summary>
        /// Creates a Form instance and sets the FormBorderStyle property.
        /// </summary>
        /// <param name="title">The title of the Form</param>
        /// <param name="resizable">Boolean value indicating if the Form is or not resizable</param>
        /// <param name="maximizable">Boolean value indicating if the Form displays the maximaze box</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.String title, bool resizable, bool maximizable)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Text = title;
            if (resizable)
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            else
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            tempForm.TopLevel = false;
            tempForm.MaximizeBox = maximizable;
            tempForm.MinimizeBox = false;
            return tempForm;
        }

        /// <summary>
        /// Creates a Form instance and sets the FormBorderStyle property.
        /// </summary>
        /// <param name="title">The title of the Form</param>
        /// <param name="resizable">Boolean value indicating if the Form is or not resizable</param>
        /// <param name="maximizable">Boolean value indicating if the Form displays the maximaze box</param>
        /// <param name="minimizable">Boolean value indicating if the Form displays the minimaze box</param>
        /// <returns>A new Form instance</returns>
        public static System.Windows.Forms.Form CreateForm(System.String title, bool resizable, bool maximizable, bool minimizable)
        {
            System.Windows.Forms.Form tempForm;
            tempForm = new System.Windows.Forms.Form();
            tempForm.Text = title;
            if (resizable)
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            else
                tempForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            tempForm.TopLevel = false;
            tempForm.MaximizeBox = maximizable;
            tempForm.MinimizeBox = minimizable;
            return tempForm;
        }

        /// <summary>
        /// Receives a Form instance and sets the property Owner to the specified parameter.
        /// This also sets the FormBorderStyle and ShowInTaskbar properties.
        /// </summary>
        /// <param name="formInstance">Form instance to be set</param>
        /// <param name="Owner">Value for the Form property Owner</param>
        public static void SetForm(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner)
        {
            formInstance.Owner = owner;
            formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            formInstance.ShowInTaskbar = false;
        }

        /// <summary>
        /// Receives a Form instance, sets the Text property and adds a Control.
        /// The received Control is added in the position 0 of the Form's Controls collection.
        /// </summary>
        /// <param name="formInstance">Form instance to be set</param>
        /// <param name="text">Value to be set to the Text property.</param>
        /// <param name="control">Control to add to the Controls collection.</param>
        public static void SetForm(System.Windows.Forms.Form formInstance, System.String text, System.Windows.Forms.Control control)
        {
            formInstance.Text = text;
            formInstance.Controls.Add(control);
            formInstance.Controls.SetChildIndex(control, 0);
        }

        /// <summary>
        /// Receives a Form instance and sets the property Owner to the specified parameter.
        /// Sets the FormBorderStyle and ShowInTaskbar properties.
        /// Also adds the received Control to the Form's Controls collection in position 0.
        /// </summary>
        /// <param name="formInstance">Form instance to be set</param>
        /// <param name="owner">Value for the Form property Owner</param>
        /// <param name="header">The Control to be added to the Controls collection.</param>
        public static void SetForm(System.Windows.Forms.Form formInstance, System.Windows.Forms.Form owner, System.Windows.Forms.Control header)
        {
            formInstance.Owner = owner;
            formInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            formInstance.ShowInTaskbar = false;
            formInstance.Controls.Add(header);
            formInstance.Controls.SetChildIndex(header, 0);
        }
    }
}
