using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPadCore
{
    public class UserPreferences
    {
        private int columnCount = 6;

        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = (value >= 1 && value <= 10) ? value : columnCount; }
        }
    }
}
