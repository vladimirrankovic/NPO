using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// Class used to store and retrieve an object command specified as a String.
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Private Hashtable used to store objects and their commands.
        /// </summary>
        private static System.Collections.Hashtable Commands = new System.Collections.Hashtable();

        /// <summary>
        /// Sets a command to the specified object.
        /// </summary>
        /// <param name="obj">The object that has the command.</param>
        /// <param name="cmd">The command for the object.</param>
        public static void SetCommand(System.Object obj, System.String cmd)
        {
            if (obj != null)
            {
                if (Commands.Contains(obj))
                    Commands[obj] = cmd;
                else
                    Commands.Add(obj, cmd);
            }
        }

        /// <summary>
        /// Gets a command associated with an object.
        /// </summary>
        /// <param name="obj">The object whose command is going to be retrieved.</param>
        /// <returns>The command of the specified object.</returns>
        public static System.String GetCommand(System.Object obj)
        {
            System.String result = "";
            if (obj != null)
                result = System.Convert.ToString(Commands[obj]);
            return result;
        }



        /// <summary>
        /// Checks if the Control contains a command, if it does not it sets the default
        /// </summary>
        /// <param name="button">The control whose command will be checked</param>
        public static void CheckCommand(System.Windows.Forms.ButtonBase button)
        {
            if (button != null)
            {
                if (GetCommand(button).Equals(""))
                    SetCommand(button, button.Text);
            }
        }

        /// <summary>
        /// Checks if the Control contains a command, if it does not it sets the default
        /// </summary>
        /// <param name="button">The control whose command will be checked</param>
        public static void CheckCommand(System.Windows.Forms.MenuItem menuItem)
        {
            if (menuItem != null)
            {
                if (GetCommand(menuItem).Equals(""))
                    SetCommand(menuItem, menuItem.Text);
            }
        }

        /// <summary>
        /// Checks if the Control contains a command, if it does not it sets the default
        /// </summary>
        /// <param name="button">The control whose command will be checked</param>
        public static void CheckCommand(System.Windows.Forms.ComboBox comboBox)
        {
            if (comboBox != null)
            {
                if (GetCommand(comboBox).Equals(""))
                    SetCommand(comboBox, "comboBoxChanged");
            }
        }

    }
}
