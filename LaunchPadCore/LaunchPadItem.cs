using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore
{
    public interface ILaunchPadItem
    {
        public AppShortcut App { get; set; }
        public bool Pressed { get; set; }
        public bool Focused { get; set; }
        public abstract void OnFocusEnter();
        public abstract void OnFocusLeave();

        public abstract void OnPress();
        public abstract void OnRelease();

        public abstract Task OnClick(Action closeHandler);
    }
}
