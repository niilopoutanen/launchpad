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
                case "LaunchPad.Apps.PowerWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "pwr_01");
                    break;
                case "LaunchPad.Apps.BatteryWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "btr_02"); 
                    break;
                case "LaunchPad.Apps.DateWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "dt_03");
                    break;
                case "LaunchPad.Apps.ClockWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "clk_04");
                    break;
                case "LaunchPad.Apps.PlaybackWidget":
                    widget = widgets.FirstOrDefault(w => w.ID == "plbk_05");
                    break;
            }


            return widget;
        }
        public int SwapWidgetVariation(int variation)
        {
            UserPreferences preferences = SaveSystem.LoadPreferences();
            if(VariationCount < variation)
            {
                variation = 1;
            }
            //null check
            preferences.WidgetVariations ??= new Dictionary<string, int>();

            if (preferences.WidgetVariations.ContainsKey(ID))
            {
                preferences.WidgetVariations[ID] = variation;
            }
            else
            {
                preferences.WidgetVariations.Add(ID, variation);
            }
            SaveSystem.SavePreferences(preferences);
            return variation;
        }
        public int LoadSelectedVariation()
        {
            int variation = 1;
            UserPreferences preferences = SaveSystem.LoadPreferences();

            //null check
            preferences.WidgetVariations ??= new Dictionary<string, int>();

            foreach (string key in  preferences.WidgetVariations.Keys)
            {
                if(key == ID)
                {
                    variation = preferences.WidgetVariations[key];
                    break;
                }
            }

            return variation;
        }
    }
}
