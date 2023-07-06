using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore
{
    public abstract class LaunchPadWidgetControl : LaunchPadItemControl
    {
        public override bool WaitForAnim => false;
        public abstract Widget Widget { get; set; }
        public abstract int Variation { get; set; }

        public override void InitializeControl()
        {
            base.InitializeControl();
            Variation = Widget.LoadSelectedVariation();
            SetVariation(Variation);
        }
        public override Task OnSecondaryClick()
        {
            Variation++;
            Variation = Widget.SwapWidgetVariation(Variation);
            SetVariation(Variation);
            return base.OnSecondaryClick();
        }
        public abstract void SetVariation(int variation);
    }
}
