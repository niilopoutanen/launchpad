using System.Windows;

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

        public abstract void SetTheme(ResourceDictionary activeDictionary);
    }
}
