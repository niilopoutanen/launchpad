using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore.Models
{
    public class AppTemplate
    {
        public string Name { get; set; }
        public string FamilyId { get; set; }
        public string AppId { get; set; }
        public int ID { get; set; }

        public AppTemplate() { }

        public AppTemplate(string name, string familyId, string appId, int id)
        {
            Name = name;
            FamilyId = familyId;
            AppId = appId;
            ID = id;
        }
    }
}
