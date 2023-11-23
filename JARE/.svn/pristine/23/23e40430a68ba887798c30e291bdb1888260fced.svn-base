using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// Contains methods to construct customized Buttons
    /// </summary>
    public class ButtonSupport
    {
        /// <summary>
        /// Creates a popup style Button with an specific text.	
        /// </summary>
        /// <param name="label">The text associated with the Button</param>
        /// <returns>The new Button</returns>
        public static System.Windows.Forms.Button CreateButton(System.String label)
        {
            System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
            tempButton.Text = label;
            tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            return tempButton;
        }

        /// <summary>
        /// Sets the an specific text for the Button
        /// </summary>
        /// <param name="Button">The button to be set</param>
        /// <param name="label">The text associated with the Button</param>
        public static void SetButton(System.Windows.Forms.ButtonBase Button, System.String label)
        {
            Button.Text = label;
            Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        }

        /// <summary>
        /// Creates a Button with an specific text and style.
        /// </summary>
        /// <param name="label">The text associated with the Button</param>
        /// <param name="style">The style of the Button</param>
        /// <returns>The new Button</returns>
        public static System.Windows.Forms.Button CreateButton(System.String label, int style)
        {
            System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
            tempButton.Text = label;
            setStyle(tempButton, style);
            return tempButton;
        }

        /// <summary>
        /// Sets the specific Text and Style for the Button
        /// </summary>
        /// <param name="Button">The button to be set</param>
        /// <param name="label">The text associated with the Button</param>
        /// <param name="style">The style of the Button</param>
        public static void SetButton(System.Windows.Forms.ButtonBase Button, System.String label, int style)
        {
            Button.Text = label;
            setStyle(Button, style);
        }

        /// <summary>
        /// Creates a standard style Button that contains an specific text and/or image
        /// </summary>
        /// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
        /// <returns>The new Button</returns>
        public static System.Windows.Forms.Button CreateButton(System.Windows.Forms.Control control)
        {
            System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
            if (control.GetType().FullName == "System.Windows.Forms.Label")
            {
                tempButton.Image = ((System.Windows.Forms.Label)control).Image;
                tempButton.Text = ((System.Windows.Forms.Label)control).Text;
                tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                tempButton.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
            }
            else
            {
                if (control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
                {
                    tempButton.Image = ((System.Windows.Forms.PictureBox)control).Image;
                    tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                }
                else
                    tempButton.Text = control.Text;
            }
            tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            return tempButton;
        }

        /// <summary>
        /// Sets an specific text and/or image to the Button
        /// </summary>
        /// <param name="Button">The button to be set</param>
        /// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
        public static void SetButton(System.Windows.Forms.ButtonBase Button, System.Windows.Forms.Control control)
        {
            if (control.GetType().FullName == "System.Windows.Forms.Label")
            {
                Button.Image = ((System.Windows.Forms.Label)control).Image;
                Button.Text = ((System.Windows.Forms.Label)control).Text;
                Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                Button.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
            }
            else
            {
                if (control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
                {
                    Button.Image = ((System.Windows.Forms.PictureBox)control).Image;
                    Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
                }
                else
                    Button.Text = control.Text;
            }
            Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
        }

        /// <summary>
        /// Creates a Button with an specific control and style
        /// </summary>
        /// <param name="control">The control to be contained by the button</param>
        /// <param name="style">The style of the button</param>
        /// <returns>The new Button</returns>
        public static System.Windows.Forms.Button CreateButton(System.Windows.Forms.Control control, int style)
        {
            System.Windows.Forms.Button tempButton = CreateButton(control);
            setStyle(tempButton, style);
            return tempButton;
        }

        /// <summary>
        /// Sets an specific text and/or image to the Button
        /// </summary>
        /// <param name="Button">The button to be set</param>
        /// <param name="control">The control to be contained by the button</param>
        /// <param name="style">The style of the button</param>
        public static void SetButton(System.Windows.Forms.ButtonBase Button, System.Windows.Forms.Control control, int style)
        {
            SetButton(Button, control);
            setStyle(Button, style);
        }

        /// <summary>
        /// Sets the style of the Button
        /// </summary>
        /// <param name="Button">The Button that will change its style</param>
        /// <param name="style">The new style of the Button</param>
        /// <remarks> 
        /// If style is 0 then sets a popup style to the Button, otherwise sets a standard style to the Button.
        /// </remarks>
        public static void setStyle(System.Windows.Forms.ButtonBase Button, int style)
        {
            if ((style == 0) || (style == 67108864) || (style == 33554432))
                Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            else if ((style == 2097152) || (style == 1048576) || (style == 16777216))
                Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            else
                throw new System.ArgumentException("illegal style: " + style);
        }

        /// <summary>
        /// Selects the Button
        /// </summary>
        /// <param name="Button">The Button that will change its style</param>
        /// <param name="select">It determines if the button woll be selected</param>
        /// <remarks> 
        /// If select is true thebutton will be selected, otherwise not.
        /// </remarks>
        public static void setSelected(System.Windows.Forms.ButtonBase Button, bool select)
        {
            if (select)
                Button.Select();
        }

        /// <summary>
        /// Receives a Button instance and sets the Text and Image properties.
        /// </summary>
        /// <param name="buttonInstance">Button instance to be set.</param>
        /// <param name="buttonText">Value to be set to Text.</param>
        /// <param name="icon">Value to be set to Image.</param>
        public static void SetStandardButton(System.Windows.Forms.ButtonBase buttonInstance, System.String buttonText, System.Drawing.Image icon)
        {
            buttonInstance.Text = buttonText;
            buttonInstance.Image = icon;
        }

        /// <summary>
        /// Creates a Button with a given text.
        /// </summary>
        /// <param name="buttonText">The text to be displayed in the button.</param>
        /// <returns>The new created button with text</returns>
        public static System.Windows.Forms.Button CreateStandardButton(System.String buttonText)
        {
            System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
            newButton.Text = buttonText;
            return newButton;
        }

        /// <summary>
        /// Creates a Button with a given image.
        /// </summary>
        /// <param name="buttonImage">The image to be displayed in the button.</param>
        /// <returns>The new created button with an image</returns>
        public static System.Windows.Forms.Button CreateStandardButton(System.Drawing.Image buttonImage)
        {
            System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
            newButton.Image = buttonImage;
            return newButton;
        }

        /// <summary>
        /// Creates a Button with a given image and a text.
        /// </summary>
        /// <param name="buttonText">The text to be displayed in the button.</param>
        /// <param name="buttonImage">The image to be displayed in the button.</param>
        /// <returns>The new created button with text and image</returns>
        public static System.Windows.Forms.Button CreateStandardButton(System.String buttonText, System.Drawing.Image buttonImage)
        {
            System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
            newButton.Text = buttonText;
            newButton.Image = buttonImage;
            return newButton;
        }
    }
}
