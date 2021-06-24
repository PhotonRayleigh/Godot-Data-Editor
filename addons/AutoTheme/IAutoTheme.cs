using Godot;
using System;

namespace AutoThemes
{
    public interface IAutoTheme
    {
        [Export]
        public FontIconSetting IconAndFontColor
        {
            get;
            set;
        }

        [Export]
        public Color BaseColor
        {
            get;
            set;
        }
        [Export]
        public Color AccentColor
        {
            get;
            set;
        }

        [Export(PropertyHint.Range, "-1, 1, 0.01")]
        public float Contrast
        {
            get;
            set;
        }

        [Export(PropertyHint.Range, "0, 2, 0.01")]
        public float IconSaturation
        {
            get;
            set;
        }

        [Export(PropertyHint.Range, "0, 1, 0.01")]
        public float RelationshipLineOpacity
        {
            get;
            set;
        }


        [Export(PropertyHint.Range, "0, 2, 1")]
        public int BorderSize
        {
            get;
            set;
        }

        [Export(PropertyHint.Range, "0, 6, 1")]
        public int CornerRadius
        {
            get;
            set;
        }

        [Export(PropertyHint.Range, "0, 5, 0.1")]
        public float AdditionalSpacing
        {
            get;
            set;
        }

        [Export(PropertyHint.Range, "0, 10, 1")]
        public int FocusBorderSize
        {
            get;
            set;
        }

        [Export]
        public AutoThemeIcons IconSetLight
        {
            get;
            set;
        }

        [Export]
        public AutoThemeIcons IconSetDark
        {
            get;
            set;
        }

        [Export]
        Theme ThemeResource
        {
            get;
            set;
        }
    }

}
