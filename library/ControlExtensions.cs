using Godot;
using System;

namespace Godot.Utilities.Extensions
{
    public static class Extensions
    {
        // Control Extensions
        public static Godot.Theme? GetInheritedTheme(this Control self)
        {
            var parent = self.GetParentControl();
            // Crawl up the parent tree until parent is null or we find a theme
            while (parent is not null)
            {
                if (parent.Theme is not null)
                {
                    return parent.Theme;
                }
                parent = parent.GetParentControl();
            }
            return null;
        }

        public static Control? GetTopLevelcontrol(this Control self)
        {
            var current = self.GetParentControl();
            if (current is null)
            {
                return null; // This control is top level
            }
            Control next = current.GetParentControl();
            while (next is not null)
            {
                current = next;
                next = current.GetParentControl();
            }
            return current;
        }
    }
}