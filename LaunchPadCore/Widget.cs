namespace LaunchPadCore
{
    public class Widget
    {
        public string WidgetName { get; set; }
        public string Description { get; set; }
        public string IconFile { get; set; }
        public string ID { get; set; }
        public bool Active { get; set; }
        public int VariationCount { get; set; }


        public static Widget LoadWidget(Type requester)
        {
            Widget widget = new();
            List<Widget> widgets = SaveSystem.LoadWidgets();
            switch(requester.ToString()) 
            {
                case "Launchpad.Apps.PowerWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "pwr_01");
                    break;
                case "Launchpad.Apps.BatteryWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "btr_02"); 
                    break;
                case "Launchpad.Apps.DateWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "dt_03");
                    break;
                case "Launchpad.Apps.ClockWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "clk_04");
                    break;
                case "Launchpad.Apps.PlaybackWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "plbk_05");
                    break;
            }


            return widget;
        }
    }
}
