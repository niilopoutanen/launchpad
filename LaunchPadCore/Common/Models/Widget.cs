using LaunchPadCore.Utility;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace LaunchPadCore.Models
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
        public static List<Widget> LoadWidgets()
        {
            List<Widget> widgets = new();

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "LaunchPadCore.widgets.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new(stream))
                {
                    string jsonContent = reader.ReadToEnd();
                    // Do something with the JSON content
                    widgets = JsonSerializer.Deserialize<List<Widget>>(jsonContent);
                }
            }
            Dictionary<string, bool> activeDict = UserPreferences.Load().ActiveWidgets;
            foreach (Widget widget in widgets)
            {
                foreach (string key in activeDict.Keys)
                {
                    if (widget.ID == key)
                    {
                        widget.Active = activeDict[key];
                        break;
                    }
                }
            }
            return widgets;
        }
        public static void SaveWidgets(List<Widget> widgets)
        {
            UserPreferences preferences = UserPreferences.Load();
            Dictionary<string, bool> activeWidgets = new();
            if (widgets == null)
            {
                widgets = LoadWidgets();
            }
            foreach (Widget widget in widgets)
            {
                activeWidgets.Add(widget.ID, widget.Active);
            }
            preferences.ActiveWidgets = activeWidgets;
            preferences.Save();
        }
        public void Save()
        {
            SaveSystem.VerifyPathIntegrity();
            List<Widget> widgets = LoadWidgets();

            Widget? existingWidget = widgets.FirstOrDefault(a => a.WidgetName == WidgetName);

            if (existingWidget != null)
            {
                widgets.Remove(existingWidget);
                existingWidget.Active = Active;
                widgets.Add(existingWidget);
            }
            else
            {
                widgets.Add(this);
            }

            SaveWidgets(widgets);
        }
    }
}
