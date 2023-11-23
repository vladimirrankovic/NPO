using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// This class contains static methods to manage tab controls.
    /// </summary>
    public class TabControlSupport
    {
        /// <summary>
        /// Create a new instance of TabControl and set the alignment property.
        /// </summary>
        /// <param name="alignment">The alignment property value.</param>
        /// <returns>New TabControl instance.</returns>
        public static System.Windows.Forms.TabControl CreateTabControl(System.Windows.Forms.TabAlignment alignment)
        {
            System.Windows.Forms.TabControl tabcontrol = new System.Windows.Forms.TabControl();
            tabcontrol.Alignment = alignment;
            return tabcontrol;
        }

        /// <summary>
        /// Set the alignment property to an instance of TabControl .
        /// </summary>
        /// <param name="tabcontrol">An instance of TabControl.</param>
        /// <param name="alignment">The alignment property value.</param>
        public static void SetTabControl(System.Windows.Forms.TabControl tabcontrol, System.Windows.Forms.TabAlignment alignment)
        {
            tabcontrol.Alignment = alignment;
        }

        /// <summary>
        /// Method to add TabPages into the TabControl object.
        /// </summary>
        /// <param name="tabControl">The TabControl to be modified.</param>
        /// <param name="component">A component to be added into the new TabControl.</param>
        public static System.Windows.Forms.Control AddTab(System.Windows.Forms.TabControl tabControl, System.Windows.Forms.Control component)
        {
            System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
            tabPage.Controls.Add(component);
            tabControl.TabPages.Add(tabPage);
            return component;
        }

        /// <summary>
        /// Method to add TabPages into the TabControl object.
        /// </summary>
        /// <param name="tabControl">The TabControl to be modified.</param>
        /// <param name="TabLabel">The label for the new TabPage.</param>
        /// <param name="component">A component to be added into the new TabControl.</param>
        public static System.Windows.Forms.Control AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Windows.Forms.Control component)
        {
            System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
            tabPage.Controls.Add(component);
            tabControl.TabPages.Add(tabPage);
            return component;
        }

        /// <summary>
        /// Method to add TabPages into the TabControl object.
        /// </summary>
        /// <param name="tabControl">The TabControl to be modified.</param>
        /// <param name="component">A component to be added into the new TabControl.</param>
        /// <param name="constraints">The object that should be displayed in the tab but won't because of limitations</param>		
        public static void AddTab(System.Windows.Forms.TabControl tabControl, System.Windows.Forms.Control component, System.Object constraints)
        {
            System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
            if (constraints is System.String)
            {
                tabPage.Text = (String)constraints;
            }
            tabPage.Controls.Add(component);
            tabControl.TabPages.Add(tabPage);
        }

        /// <summary>
        /// Method to add TabPages into the TabControl object.
        /// </summary>
        /// <param name="tabControl">The TabControl to be modified.</param>
        /// <param name="TabLabel">The label for the new TabPage.</param>
        /// <param name="constraints">The object that should be displayed in the tab but won't because of limitations</param>
        /// <param name="component">A component to be added into the new TabControl.</param>
        public static void AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Object constraints, System.Windows.Forms.Control component)
        {
            System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
            tabPage.Controls.Add(component);
            tabControl.TabPages.Add(tabPage);
        }

        /// <summary>
        /// Method to add TabPages into the TabControl object.
        /// </summary>
        /// <param name="tabControl">The TabControl to be modified.</param>
        /// <param name="tabLabel">The label for the new TabPage.</param>
        /// <param name="image">Background image for the TabPage.</param>
        /// <param name="component">A component to be added into the new TabControl.</param>
        public static void AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Drawing.Image image, System.Windows.Forms.Control component)
        {
            System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
            tabPage.BackgroundImage = image;
            tabPage.Controls.Add(component);
            tabControl.TabPages.Add(tabPage);
        }
    }
}
