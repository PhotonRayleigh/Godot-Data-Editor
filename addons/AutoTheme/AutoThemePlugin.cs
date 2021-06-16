#if TOOLS
using Godot;
using System;

namespace AutoThemes
{
    [Tool]
    public class AutoThemePlugin : EditorPlugin
    {
        public override void _EnterTree()
        {
            base._EnterTree();
            Script AutoThemeScript = GD.Load<Script>("addons/AutoTheme/AutoTheme.cs");
            Texture AutoThemeIcon = GD.Load<Texture>("addons/AutoTheme/resources/icons/Theme.svg");
            AddCustomType("AutoTheme", "Theme", AutoThemeScript, AutoThemeIcon);

            Script AutoThemeIconsScript = GD.Load<Script>("addons/AutoTheme/AutoThemeIcons.cs");
            Texture AutoThemeIconsIcon = GD.Load<Texture>("addons/AutoTheme/resources/icons/Image.svg");
            AddCustomType("AutoThemeIcons", "Resource", AutoThemeIconsScript, AutoThemeIconsIcon);
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            RemoveCustomType("AutoTheme");
            RemoveCustomType("AutoThemeIcons");
        }
    }
}
#endif