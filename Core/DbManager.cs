using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class DbManager
    {
        public static void SaveTheme(Theme theme)
        {
            using (var db = new AppDbContext())
            {
                var existingTheme = db.Theme.FirstOrDefault();
                if (existingTheme != null)
                {
                    db.Entry(existingTheme).CurrentValues.SetValues(theme);
                }
                else
                {
                    theme.Id = 1; // Ensure a single theme exists
                    db.Theme.Add(theme);
                }
                db.SaveChanges();
            }
        }

        public static Theme LoadTheme()
        {
            using (var db = new AppDbContext())
            {
                return db.Theme.FirstOrDefault() ?? new Theme();
            }
        }
    }
}
