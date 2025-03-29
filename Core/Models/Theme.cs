using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Theme
    {
        public int Id { get; set; } = 1;
        public bool IconNameVisible { get; set; } = false;
        public bool Rounded { get; set; } = false;
        public enum Styles
        {
            Dark,
            DarkTransparent,
            Light,
            LightTransparent
        }
        public Styles Style { get; set; } = Theme.Styles.DarkTransparent;
    }
}
