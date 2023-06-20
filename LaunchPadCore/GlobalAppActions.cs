using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaunchPadCore
{
    public class GlobalAppActions
    {

        public const float SIZE_FOCUS = 1.05f;
        public const float SIZE_STATIC = 1f;
        public const float SIZE_PRESSED = 0.9f;


        public static Tuple<ScaleTransform, DoubleAnimation> GetReleaseAnim(bool isFocused)
        {
            float finalValue = SIZE_STATIC;
            if (isFocused)
            {
                finalValue = SIZE_FOCUS;
            }
            ScaleTransform scaleTransform = new(SIZE_PRESSED, SIZE_PRESSED);

            DoubleAnimation scaleAnimation = new()
            {
                To = finalValue,
                Duration = TimeSpan.FromSeconds(0.1)
            };


            return Tuple.Create(scaleTransform, scaleAnimation);
        }

        public static Tuple<ScaleTransform, DoubleAnimation> GetPressAnim()
        {
            ScaleTransform scaleTransform = new ScaleTransform(SIZE_STATIC, SIZE_STATIC);

            DoubleAnimation scaleAnimation = new()
            {
                To = SIZE_PRESSED,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            return Tuple.Create(scaleTransform, scaleAnimation);
        }


        public static Tuple<ScaleTransform, DoubleAnimation> GetFocusEnterAnim()
        {
            ScaleTransform scaleTransform = new(SIZE_STATIC, SIZE_STATIC);

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = SIZE_FOCUS,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            return Tuple.Create(scaleTransform, scaleAnimation);
        }

        public static Tuple<ScaleTransform, DoubleAnimation> GetFocusLeaveAnim()
        {
            ScaleTransform scaleTransform = new ScaleTransform(GlobalAppActions.SIZE_FOCUS, GlobalAppActions.SIZE_FOCUS);

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                To = GlobalAppActions.SIZE_STATIC,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            return Tuple.Create(scaleTransform, scaleAnimation);
        }
    }
}
