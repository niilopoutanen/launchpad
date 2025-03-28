﻿namespace LaunchPadCore.Models
{
    public class Widget
    {
        public string WidgetName { get; set; }
        public string Description { get; set; }
        public string IconFile { get; set; }
        public string ID { get; set; }
        public bool Active { get; set; }
        public int VariationCount { get; set; }

        public int SwapWidgetVariation(int variation)
        {
            UserPreferences preferences = UserPreferences.Load();
            if (VariationCount < variation)
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

            preferences.Save();
            return variation;
        }
        public int LoadSelectedVariation()
        {
            int variation = 1;
            UserPreferences preferences = UserPreferences.Load();

            if (preferences.WidgetVariations == null || !preferences.RememberWidgetVariation)
            {
                return variation;
            }

            foreach (string key in preferences.WidgetVariations.Keys)
            {
                if (key == ID)
                {
                    variation = preferences.WidgetVariations[key];
                    break;
                }
            }

            return variation;
        }
    }
}
