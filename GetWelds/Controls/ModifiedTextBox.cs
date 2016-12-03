using GetWelds.Robots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GetWelds.Controls
{
   public class ModifiedTextBox:TextBox
    {
       public ModifiedTextBox()
       {
       }


        public AbstractRobot Robot
        {
            get { return (AbstractRobot)GetValue(RobotProperty); }
            set { SetValue(RobotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Robot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RobotProperty =
            DependencyProperty.Register("Robot", typeof(AbstractRobot), typeof(ModifiedTextBox), new PropertyMetadata(0));

        

        public Zone Zone
        {
            get { return (Zone)GetValue(ZoneProperty); }
            set { SetValue(ZoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Zone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneProperty =
            DependencyProperty.Register("Zone", typeof(Zone), typeof(ModifiedTextBox), new PropertyMetadata(0));



    }
}
