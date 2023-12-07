using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HVPPCafeDesktop.Resources.Utilities
{
    class NavigateButton : RadioButton
    {
        static NavigateButton()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(
                    typeof(NavigateButton),
                    new FrameworkPropertyMetadata(typeof(NavigateButton))
                );
        }
    }
}
