using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// This class has static methods for manage CheckBox and RadioButton controls.
    /// </summary>
    public class CheckBoxSupport
    {

        /// <summary>
        /// Receives a CheckBox instance and sets the specific text and style.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
        /// <param name="text">The text to set Text property.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, int style)
        {
            checkBoxInstance.Text = text;
            if (style == 2097152)
                checkBoxInstance.ThreeState = true;
        }

        /// <summary>
        /// Returns a new CheckBox instance with the specified text
        /// </summary>
        /// <param name="text">The text to create the CheckBox with</param>
        /// <returns>A new CheckBox instance</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text)
        {
            System.Windows.Forms.CheckBox tempCheck = new System.Windows.Forms.CheckBox();
            tempCheck.Text = text;
            return tempCheck;
        }

        /// <summary>
        /// Creates a CheckBox with a specified image.
        /// </summary>
        /// <param name="icon">The image to be used with the CheckBox.</param>
        /// <returns>A new CheckBox instance with an image.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.Drawing.Image icon)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Image = icon;
            return tempCheckBox;
        }

        /// <summary>
        /// Creates a CheckBox with a specified label and image.
        /// </summary>
        /// <param name="text">The text to be used as label.</param>
        /// <param name="icon">The image to be used with the CheckBox.</param>
        /// <returns>A new CheckBox instance with a label and an image.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, System.Drawing.Image icon)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Text = text;
            tempCheckBox.Image = icon;
            return tempCheckBox;
        }

        /// <summary>
        /// Returns a new CheckBox instance with the specified text and state
        /// </summary>
        /// <param name="text">The text to create the CheckBox with</param>
        /// <param name="checkedStatus">Indicates if the Checkbox is checked or not</param>
        /// <returns>A new CheckBox instance</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, bool checkedStatus)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Text = text;
            tempCheckBox.Checked = checkedStatus;
            return tempCheckBox;
        }

        /// <summary>
        /// Creates a CheckBox with a specified image and checked if checkedStatus is true.
        /// </summary>
        /// <param name="icon">The image to be used with the CheckBox.</param>
        /// <param name="checkedStatus">Value to be set to Checked property.</param>
        /// <returns>A new CheckBox instance.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.Drawing.Image icon, bool checkedStatus)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Image = icon;
            tempCheckBox.Checked = checkedStatus;
            return tempCheckBox;
        }

        /// <summary>
        /// Creates a CheckBox with label, image and checked if checkedStatus is true.
        /// </summary>
        /// <param name="text">The text to be used as label.</param>
        /// <param name="icon">The image to be used with the CheckBox.</param>
        /// <param name="checkedStatus">Value to be set to Checked property.</param>
        /// <returns>A new CheckBox instance.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, System.Drawing.Image icon, bool checkedStatus)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Text = text;
            tempCheckBox.Image = icon;
            tempCheckBox.Checked = checkedStatus;
            return tempCheckBox;
        }

        /// <summary>
        /// Creates a CheckBox with a specific control.
        /// </summary>
        /// <param name="control">The control to be added to the CheckBox.</param>
        /// <returns>The new CheckBox with the specific control.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.Windows.Forms.Control control)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Controls.Add(control);
            control.Location = new System.Drawing.Point(0, 18);
            return tempCheckBox;
        }

        /// <summary>
        /// Creates a CheckBox with the specific control and style.
        /// </summary>
        /// <param name="control">The control to be added to the CheckBox.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        /// <returns>The new CheckBox with the specific control and style.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.Windows.Forms.Control control, int style)
        {
            System.Windows.Forms.CheckBox tempCheckBox = new System.Windows.Forms.CheckBox();
            tempCheckBox.Controls.Add(control);
            control.Location = new System.Drawing.Point(0, 18);
            if (style == 2097152)
                tempCheckBox.ThreeState = true;
            return tempCheckBox;
        }

        /// <summary>
        /// Returns a new RadioButton instance with the specified text in the specified panel.
        /// </summary>
        /// <param name="text">The text to create the RadioButton with.</param>
        /// <param name="checkedStatus">Indicates if the RadioButton is checked or not.</param>
        /// <param name="panel">The panel where the RadioButton will be placed.</param>
        /// <returns>A new RadioButton instance.</returns>
        /// <remarks>If the panel is null the third param is ignored</remarks>
        public static System.Windows.Forms.RadioButton CreateRadioButton(System.String text, bool checkedStatus, System.Windows.Forms.Panel panel)
        {
            System.Windows.Forms.RadioButton tempCheckBox = new System.Windows.Forms.RadioButton();
            tempCheckBox.Text = text;
            tempCheckBox.Checked = checkedStatus;
            if (panel != null)
                panel.Controls.Add(tempCheckBox);
            return tempCheckBox;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the Text and Image properties.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
        /// <param name="text">Value to be set to Text property.</param>
        /// <param name="icon">Value to be set to Image property.</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, System.Drawing.Image icon)
        {
            checkBoxInstance.Text = text;
            checkBoxInstance.Image = icon;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the Text and Checked properties.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set</param>
        /// <param name="text">Value to be set to Text</param>
        /// <param name="checkedStatus">Value to be set to Checked property.</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, bool checkedStatus)
        {
            checkBoxInstance.Text = text;
            checkBoxInstance.Checked = checkedStatus;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the Image and Checked properties.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
        /// <param name="icon">Value to be set to Image property.</param>
        /// <param name="checkedStatus">Value to be set to Checked property.</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.Drawing.Image icon, bool checkedStatus)
        {
            checkBoxInstance.Image = icon;
            checkBoxInstance.Checked = checkedStatus;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the Text, Image and Checked properties.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
        /// <param name="text">Value to be set to Text property.</param>
        /// <param name="icon">Value to be set to Image property.</param>
        /// <param name="checkedStatus">Value to be set to Checked property.</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.String text, System.Drawing.Image icon, bool checkedStatus)
        {
            checkBoxInstance.Text = text;
            checkBoxInstance.Image = icon;
            checkBoxInstance.Checked = checkedStatus;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the control specified.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
        /// <param name="control">The control assiciated with the CheckBox</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.Windows.Forms.Control control)
        {
            checkBoxInstance.Controls.Add(control);
            control.Location = new System.Drawing.Point(0, 18);
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the specific control and style.
        /// </summary>
        /// <param name="checkBoxInstance">CheckBox instance to be set.</param>
        /// <param name="control">The control assiciated with the CheckBox.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        public static void SetCheckBox(System.Windows.Forms.CheckBox checkBoxInstance, System.Windows.Forms.Control control, int style)
        {
            checkBoxInstance.Controls.Add(control);
            control.Location = new System.Drawing.Point(0, 18);
            if (style == 2097152)
                checkBoxInstance.ThreeState = true;
        }

        /// <summary>
        /// Receives an instance of a RadioButton and sets its Text and Checked properties.
        /// </summary>
        /// <param name="RadioButtonInstance">The instance of RadioButton to be set.</param>
        /// <param name="text">The text to set Text property.</param>
        /// <param name="checkedStatus">Indicates if the RadioButton is checked or not.</param>
        public static void SetCheckbox(System.Windows.Forms.RadioButton radioButtonInstance, System.String text, bool checkedStatus)
        {
            radioButtonInstance.Text = text;
            radioButtonInstance.Checked = checkedStatus;
        }

        /// <summary>
        /// Receives an instance of a RadioButton and sets its Text and Checked properties and its containing panel
        /// </summary>
        /// <param name="CheckBoxInstance">The instance of RadioButton to be set</param>
        /// <param name="text">The text to set Text property</param>
        /// <param name="checkedStatus">Indicates if the RadioButton is checked or not</param>
        /// <param name="panel">The panel where the RadioButton will be placed</param>
        /// <remarks>If the panel is null the third param is ignored</remarks>
        public static void SetRadioButton(System.Windows.Forms.RadioButton radioButtonInstance, System.String text, bool checkedStatus, System.Windows.Forms.Panel panel)
        {
            radioButtonInstance.Text = text;
            radioButtonInstance.Checked = checkedStatus;
            if (panel != null)
                panel.Controls.Add(radioButtonInstance);
        }

        /// <summary>
        /// Creates a CheckBox with a specified text label and style.
        /// </summary>
        /// <param name="text">The text to be used as label.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        /// <returns>A new CheckBox instance.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, int style)
        {
            System.Windows.Forms.CheckBox checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Text = text;
            if (style == 2097152)
                checkBox.ThreeState = true;
            return checkBox;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the Text and ThreeState properties.
        /// </summary>
        /// <param name="checkBox">CheckBox instance to be set.</param>
        /// <param name="text">Value to be set to Text property.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        public static void setCheckBox(System.Windows.Forms.CheckBox checkBox, System.String text, int style)
        {
            checkBox.Text = text;
            if (style == 2097152)
                checkBox.ThreeState = true;
        }

        /// <summary>
        /// Creates a CheckBox with a specified text label, image and style.
        /// </summary>
        /// <param name="text">The text to be used as label.</param>
        /// <param name="icon">The image to be used with the CheckBox.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        /// <returns>A new CheckBox instance.</returns>
        public static System.Windows.Forms.CheckBox CreateCheckBox(System.String text, System.Drawing.Image icon, int style)
        {
            System.Windows.Forms.CheckBox checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Text = text;
            checkBox.Image = icon;
            if (style == 2097152)
                checkBox.ThreeState = true;
            return checkBox;
        }

        /// <summary>
        /// Receives a CheckBox instance and sets the Text, Image and ThreeState properties.
        /// </summary>
        /// <param name="checkBox">CheckBox instance to be set.</param>
        /// <param name="text">Value to be set to Text property.</param>
        /// <param name="icon">Value to be set to Image property.</param>
        /// <param name="style">The style to be used to set the threeState property.</param>
        public static void setCheckBox(System.Windows.Forms.CheckBox checkBox, System.String text, System.Drawing.Image icon, int style)
        {
            checkBox.Text = text;
            checkBox.Image = icon;
            if (style == 2097152)
                checkBox.ThreeState = true;
        }

        /// <summary>
        /// The SetIndeterminate method is used to sets or clear the CheckState of the CheckBox component.
        /// </summary>
        /// <param name="indeterminate">The value to the Indeterminate state. If true, the state is set; otherwise, it is cleared.</param>
        /// <param name="checkBox">The CheckBox component to be modified.</param>
        /// <returns></returns>
        public static System.Windows.Forms.CheckBox SetIndeterminate(bool indeterminate, System.Windows.Forms.CheckBox checkBox)
        {
            if (indeterminate)
                checkBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            else if (checkBox.Checked)
                checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            else
                checkBox.CheckState = System.Windows.Forms.CheckState.Unchecked;
            return checkBox;
        }
    }
}
