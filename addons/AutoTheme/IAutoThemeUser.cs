using Godot;
using System;

namespace AutoThemes
{
    public interface IAutoThemeUser
    {
        // public AutoTheme ATheme { get; set; }
        public AutoThemeSetter ThemeSource { get; set; }

        public abstract void _OnAutoThemeChanged();
    }
}