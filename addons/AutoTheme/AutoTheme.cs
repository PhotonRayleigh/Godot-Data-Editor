// Author: PhotonRayleigh
// Year: 2021
// GitHub: https://github.com/PhotonRayleigh

using Godot;
using System;
using SparkLib.GodotLib.Theme;
using SparkLib;
using System.Threading.Tasks;

namespace AutoThemes
{
    [Tool]
    public class AutoTheme : Theme
    {
        /*
            The colors and styleboxes define the model of my theme.
            How those colors and styleboxes are applied depends on the
            controls themselves.
        */

        //Properties
        protected bool refresh = false;
        [Export]
        public bool AutoRefresh
        {
            get => refresh;
            set
            {
                refresh = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        public enum FontIconSetting { Dark, Light }
        protected FontIconSetting iconAndFontColor = FontIconSetting.Light;
        [Export]
        public FontIconSetting IconAndFontColor
        {
            get => iconAndFontColor;
            set
            {
                iconAndFontColor = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }
        public Color DarkFontColor = new("383838");
        public Color LightFontColor = new("cdcfd2");
        public Color FontColor;

        protected Color baseColor = new("3D3D3D");
        [Export]
        public Color BaseColor
        {
            get => baseColor;
            set
            {
                baseColor = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected Color accentColor = new("b8e3ff");
        [Export]
        public Color AccentColor
        {
            get => accentColor;
            set
            {
                accentColor = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected float contrast = 0.2f;
        [Export(PropertyHint.Range, "-1, 1, 0.01")]
        public float Contrast
        {
            get => contrast;
            set
            {
                contrast = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected float iconSaturation = 1.0f;
        [Export(PropertyHint.Range, "0, 2, 0.01")]
        public float IconSaturation
        {
            get => iconSaturation;
            set
            {
                iconSaturation = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected float relationshipLineOpacity = 0.1f;
        [Export(PropertyHint.Range, "0, 1, 0.01")]
        public float RelationshipLineOpacity
        {
            get => relationshipLineOpacity;
            set
            {
                relationshipLineOpacity = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected int borderSize = 0;
        [Export(PropertyHint.Range, "0, 2, 1")]
        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected int cornerRadius = 3;
        [Export(PropertyHint.Range, "0, 6, 1")]
        public int CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected float additionalSpacing = 0.0f;
        [Export(PropertyHint.Range, "0, 5, 0.1")]
        public float AdditionalSpacing
        {
            get => additionalSpacing;
            set
            {
                additionalSpacing = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        protected int focusBorderSize = 1;
        [Export(PropertyHint.Range, "0, 10, 1")]
        public int FocusBorderSize
        {
            get => focusBorderSize;
            set
            {
                focusBorderSize = value;
                if (refresh == true)
                {
                    // var t = RefreshAsync();
                    Refresh();
                }
            }
        }

        public AutoThemeIcons iconSetLight = new();
        [Export]
        public AutoThemeIcons IconSetLight
        {
            get => iconSetLight;
            set
            {
                iconSetLight = value;
                //EmitChanged();
            }
        }
        public AutoThemeIcons iconSetDark = new();
        [Export]
        public AutoThemeIcons IconSetDark
        {
            get => iconSetDark;
            set
            {
                iconSetDark = value;
                //EmitChanged();
            }
        }

        // [Export]
        public AutoThemeIcons? IconSet
        {
            get => iconSet;
            // set => iconSet = iconSet;
        }
        protected AutoThemeIcons? iconSet = null;

        // Constants
        public const int TextMarginSides = 5;
        public const int TextMarginTopBottom = 4;
        public const int TextEditUnderlineWidth = 1;
        public const int ActiveTabAccentBorderWidth = 2;

        // Derived colors
        // Descends in lightness if contrast is positive
        // Ascends if contrast is negative
        public Color ForegroundColor;
        public Color MidgroundColor;
        public Color BackgroundColor;
        public Color BackdropColor;
        public Color BackgroundContrastColor;
        public Color HoverColor;
        public Color ForegroundContrastColor;
        public Color HighlightColor; // Used for highlighting selections, requires transparency
        public Color MonoColor; // Black if Dark Font, white if Light Font
        public Color TooltipPanelColor = new("e6ffffff");
        public Color BoxSelectionFillColor;
        public Color BoxSelectionStrokeColor;
        public Color DisabledBgColor;
        public Color SubAccentColor;

        public Color GuideColor;
        public Color RelationshipLineColor;
        public Color ChildrenHlLineColor;
        public Color ParentHlLineColor;

        public Color HighlightHoverColor;

        // Font Colors
        public Color DisabledFontColor = new("#4dffffff");
        public Color HighlightedFontColor = new("e2e2e2");
        public Color PressedFontColor = new("b8e3ff");
        public Color TooltipFontColor = new("303030");
        public Color TooltipFontShadowColor = new("1a000000");

        // Icon Colors
        public Color HoverIconColor = new(0, 0, 0, 1); // Default is 293,293,293,255 - overbright
        public Color PressedIconColor = new(0, 0, 0, 1);// Default is 211, 261, 293, 255
        public Color FolderIconMod = new("cdebff");

        // Theme styleboxes
        StyleBoxFlat? PanelBg;// Default color is #313131
        StyleBoxFlat? PanelMg; // Default color is #373737
        StyleBoxFlat? PanelFg;// Default color is #3D3D3D
        StyleBoxFlat? PanelBd; // Default color is #252525
        StyleBoxFlat? PanelHover;
        StyleBoxFlat? PanelDisabled;

        StyleBoxFlat? PanelBgBorderless;
        StyleBoxFlat? PanelMgBorderless;
        StyleBoxFlat? PanelFgBorderless;
        StyleBoxFlat? PanelBdBorderless;
        StyleBoxFlat? PanelHoverBorderless;
        StyleBoxFlat? PanelDisabledBorderless;

        StyleBoxFlat? PanelFocusBg;
        StyleBoxFlat? PanelFocusMg;
        StyleBoxFlat? PanelFocusFg;
        StyleBoxFlat? PanelFocusHover;
        StyleBoxFlat? PanelFocusDisabled;

        StyleBoxFlat? PanelTextFgBorderless;
        StyleBoxFlat? PanelTextBgBorderless;
        StyleBoxFlat? PanelTextMgBorderless;
        StyleBoxFlat? PanelTextHoverBorderless;
        StyleBoxFlat? PanelTextDisabledBorderless;

        StyleBoxFlat? PanelTextFg;
        StyleBoxFlat? PanelTextBg;
        StyleBoxFlat? PanelTextMg;
        StyleBoxFlat? PanelTextHover;
        StyleBoxFlat? PanelTextDisabled;

        StyleBoxFlat? PanelTextFocusBg;
        StyleBoxFlat? PanelTextFocusFg;
        StyleBoxFlat? PanelTextFocusMg;
        StyleBoxFlat? PanelTextFocusHover;
        StyleBoxFlat? PanelTextFocusDisabled;

        StyleBoxFlat? HighlightStyle;// Default color is #6e6e6e
        StyleBoxFlat? HighlightHoverStyle;

        StyleBoxLine? SeparatorStyle;// Default color is #1affffff (first byte is alpha)

        StyleBoxFlat? ToolTipPanel;// Default color is #e6ffffff

        StyleBoxFlat? ButtonDisabledStyle;
        StyleBoxFlat? ButtonFocusStyle;
        StyleBoxFlat? ButtonHoverStyle;
        StyleBoxFlat? ButtonNormalStyle;
        StyleBoxFlat? ButtonPressedStyle;

        StyleBoxFlat? TabStylePanel;
        StyleBoxFlat? TabStyleBg;
        StyleBoxFlat? TabStyleDis;
        StyleBoxFlat? TabStyleFg;

        StyleBoxFlat? WindowDialogPanelStyle;

        StyleBoxFlat? TextEditBackground;
        StyleBoxFlat? TextEditReadonly;
        StyleBoxFlat? TextEditFocus;

        StyleBoxTexture? ScrollBarBaseStyle;

        public AutoTheme()
        {
            // Not sure when this gets called
            // On every file load? 
            // Are there virtual resource functions I can use
            // like OnSave and OnLoad? 
            // AutoRefresh = false;
            // var signals = GetSignalConnectionList("changed");
            // foreach (var signal in signals)
            // {
            //     GD.Print($"{signal.ToString()}");
            // }
        }

        public void Refresh()
        {
            ApplyChanges();
            EmitChanged();
        }

        protected Task refreshHandle;
        /// <summary>
        /// Use this with care. Running update code on a different
        /// thread in the editor can cause issues.
        /// </summary>
        /// <returns>async Task object</returns>
        public async Task RefreshAsync()
        {
            if (refreshHandle is null)
            {
                refreshHandle = Task.Run(ApplyChanges);
                await refreshHandle;
                EmitChanged();
            }
            else
            {
                await refreshHandle.ContinueWith((Task prev) =>
                {
                    ApplyChanges();
                });
                EmitChanged();
            }
        }

        protected void GenerateColors()
        {
            switch (iconAndFontColor)
            {
                case FontIconSetting.Light:
                    FontColor = LightFontColor;
                    iconSet = IconSetLight;
                    break;
                case FontIconSetting.Dark:
                    FontColor = DarkFontColor;
                    iconSet = IconSetDark;
                    break;
                default:
                    FontColor = LightFontColor;
                    iconSet = IconSetLight;
                    break;
            }

            // Create derived colors based on contrast settings
            SubAccentColor = AccentColor.Darkened(0.2f);
            if (Contrast == 0)
            {
                ForegroundColor = BaseColor;
                MidgroundColor = BaseColor;
                BackgroundColor = BaseColor;
                BackdropColor = BackgroundColor.Darkened(0.1f);
                DisabledBgColor = BaseColor;

                HoverColor = BaseColor.Lightened(0.1f);
                ForegroundContrastColor = BaseColor.Lightened(0.2f);

                HighlightColor = AccentColor.Lightened(0.1f);
            }
            else if (Contrast > 0)
            {
                ForegroundColor = BaseColor;
                MidgroundColor = BaseColor.Darkened(Contrast * 0.2f);
                BackgroundColor = BaseColor.Darkened(Contrast * 0.80f);
                BackdropColor = BackgroundColor.Darkened(Contrast);
                DisabledBgColor = BaseColor.Darkened(Contrast * 0.6f);

                HoverColor = BaseColor.Lightened(Contrast * 0.1f);
                ForegroundContrastColor = BaseColor.Lightened(Contrast * 0.2f);

                HighlightColor = AccentColor.Lightened(0.1f);
            }
            else if (Contrast < 0)
            {
                float absContrast = Godot.Mathf.Abs(Contrast);

                ForegroundColor = BaseColor;
                MidgroundColor = BaseColor.Lightened(absContrast * 0.2f);
                BackgroundColor = BaseColor.Lightened(absContrast * 0.80f);
                BackdropColor = BaseColor.Lightened(absContrast);
                DisabledBgColor = BaseColor.Lightened(absContrast * 0.6f);

                HoverColor = BaseColor.Darkened(absContrast * 0.1f);
                ForegroundContrastColor = ForegroundColor.Darkened(absContrast * 0.2f);

                HighlightColor = AccentColor.Darkened(0.1f);
            }
            BackgroundContrastColor = BackdropColor.Contrasted();
            HighlightColor.a = 0.2f;
            //DisabledBgColor.s -= 0.05f;
            HighlightHoverColor = new Color(HoverColor);
            HighlightHoverColor.a = 0.08f;

            BoxSelectionStrokeColor = new(AccentColor);
            BoxSelectionStrokeColor.a = 0.8f;
            BoxSelectionFillColor = new(AccentColor);
            BoxSelectionFillColor.a = 0.3f;

            // Make sure derived font colors follow global setting
            if (IconAndFontColor == FontIconSetting.Light)
            {
                HighlightedFontColor = new Color(FontColor);
                HighlightedFontColor.Lightened(0.1f);
                MonoColor = new Color(1, 1, 1);
                // Tooltip should oppose whatever the main color level is
                TooltipFontColor = DarkFontColor;
                TooltipPanelColor = BackgroundContrastColor;
            }
            else if (IconAndFontColor == FontIconSetting.Dark)
            {
                HighlightedFontColor = new Color(FontColor);
                HighlightedFontColor.Darkened(0.1f);
                MonoColor = new Color(0, 0, 0);
                // Tooltip should oppose whatever the main color level is
                TooltipFontColor = LightFontColor;
                TooltipPanelColor = BackgroundContrastColor;
            }
            DisabledFontColor = new(MonoColor);
            DisabledFontColor.a = 0.3f;

            HoverIconColor = new Color(1.15f, 1.15f, 1.15f);
            PressedIconColor = AccentColor.Lightened(0.2f);

            // When clicking on buttons, will make the font
            // a color brighter or darker than the accent color
            // depending on the accent color's brightness.
            PressedFontColor = new Color(AccentColor);
            if ((PressedFontColor.r + PressedFontColor.g + PressedFontColor.b) >= 1.5f)
            {
                PressedFontColor.Darkened(0.2f);
            }
            else
            {
                PressedFontColor.Lightened(0.2f);
            }

            GuideColor = new Color(MonoColor);
            GuideColor.a = 0.05f;
            RelationshipLineColor = new Color(MonoColor);
            RelationshipLineColor.a = RelationshipLineOpacity;
            ChildrenHlLineColor = new Color(MonoColor);
            ChildrenHlLineColor.a = 0.35f;
            ParentHlLineColor = new Color(MonoColor);
            ParentHlLineColor.a = 0.55f;

        }

        public StyleBoxFlat StyleBoxFlatBuilder(Color bgColor, bool borders = true, bool focus = false, bool textMargin = false, bool tab = false)
        {
            StyleBoxFlat newStyle = new();
            newStyle.BgColor = bgColor;
            newStyle.DrawCenter = true;
            newStyle.CornerDetail = 8;
            if (focus)
            {
                newStyle.SetBorderWidthAll(FocusBorderSize);
                newStyle.BorderColor = AccentColor;
            }
            else if (borders)
            {
                newStyle.SetBorderWidthAll(BorderSize);
                newStyle.BorderColor = BackgroundContrastColor;
            }
            else
            {
                newStyle.SetBorderWidthAll(0);
                newStyle.BorderColor = BackgroundContrastColor;
            }
            newStyle.BorderBlend = false;
            newStyle.SetCornerRadiusAll(CornerRadius);
            newStyle.SetExpandMarginAll(0);

            newStyle.ShadowColor = new Color(0, 0, 0, 0);
            newStyle.ShadowSize = 0;
            newStyle.ShadowOffset = new Vector2(0, 0);
            newStyle.AntiAliasing = true;
            newStyle.AntiAliasingSize = 1;
            newStyle.SetContentMarginAll(AdditionalSpacing);

            if (textMargin)
            {
                newStyle.ContentMarginTop += TextMarginTopBottom;
                newStyle.ContentMarginLeft += TextMarginSides;
                newStyle.ContentMarginRight += TextMarginSides;
                newStyle.ContentMarginBottom += TextMarginTopBottom;
            }

            if (tab) Tabify(ref newStyle);

            return newStyle;

            void Tabify(ref StyleBoxFlat style)
            {
                style.CornerRadiusBottomLeft = 0;
                style.CornerRadiusBottomRight = 0;
                style.BorderWidthBottom = 0;
                style.ContentMarginLeft = 10 + AdditionalSpacing;
                style.ContentMarginRight = 10 + AdditionalSpacing;
                style.ContentMarginTop = 5 + AdditionalSpacing;
                style.ContentMarginBottom = 5 + AdditionalSpacing;
                style.ExpandMarginBottom = 1;
            }
        }

        protected void GenerateStyles()
        {
            // Fg = Foreground, Mg = Midground, Bg = Background, Bd = Backdrop
            // Base panel styles
            // Borders are on by default
            PanelFg = StyleBoxFlatBuilder(ForegroundColor);
            PanelMg = StyleBoxFlatBuilder(MidgroundColor);
            PanelBg = StyleBoxFlatBuilder(BackgroundColor);
            PanelBd = StyleBoxFlatBuilder(BackdropColor);
            PanelHover = StyleBoxFlatBuilder(HoverColor);
            PanelDisabled = StyleBoxFlatBuilder(DisabledBgColor);

            // Borderless versions of panels
            PanelBgBorderless = StyleBoxFlatBuilder(BackgroundColor, false);
            PanelMgBorderless = StyleBoxFlatBuilder(MidgroundColor, false);
            PanelFgBorderless = StyleBoxFlatBuilder(ForegroundColor, false);
            PanelBdBorderless = StyleBoxFlatBuilder(BackdropColor, false);
            PanelHoverBorderless = StyleBoxFlatBuilder(HoverColor, false);
            PanelDisabledBorderless = StyleBoxFlatBuilder(DisabledBgColor, false);

            // Focused versions of panels
            PanelFocusBg = StyleBoxFlatBuilder(BackgroundColor, false, true);
            PanelFocusMg = StyleBoxFlatBuilder(MidgroundColor, false, true);
            PanelFocusFg = StyleBoxFlatBuilder(ForegroundColor, false, true);
            PanelFocusHover = StyleBoxFlatBuilder(HoverColor, false, true);
            PanelFocusDisabled = StyleBoxFlatBuilder(DisabledBgColor, false, true);

            // Special content margin for text containing controls
            PanelTextFgBorderless = StyleBoxFlatBuilder(ForegroundColor, false, false, true);
            PanelTextBgBorderless = StyleBoxFlatBuilder(BackgroundColor, false, false, true);
            PanelTextMgBorderless = StyleBoxFlatBuilder(MidgroundColor, false, false, true);
            PanelTextHoverBorderless = StyleBoxFlatBuilder(HoverColor, false, false, true);
            PanelTextDisabledBorderless = StyleBoxFlatBuilder(DisabledBgColor, false, false, true);

            PanelTextFg = StyleBoxFlatBuilder(ForegroundColor, true, false, true);
            PanelTextBg = StyleBoxFlatBuilder(BackgroundColor, true, false, true);
            PanelTextMg = StyleBoxFlatBuilder(MidgroundColor, true, false, true);
            PanelTextHover = StyleBoxFlatBuilder(HoverColor, true, false, true);
            PanelTextDisabled = StyleBoxFlatBuilder(DisabledBgColor, true, false, true);

            PanelTextFocusBg = StyleBoxFlatBuilder(BackgroundColor, false, true, true);
            PanelTextFocusFg = StyleBoxFlatBuilder(ForegroundColor, false, true, true);
            PanelTextFocusMg = StyleBoxFlatBuilder(MidgroundColor, false, true, true);
            PanelTextFocusHover = StyleBoxFlatBuilder(HoverColor, false, true, true);
            PanelTextFocusDisabled = StyleBoxFlatBuilder(DisabledBgColor, false, true, true);

            // Highlight boxes for selecting items in lists
            HighlightStyle = StyleBoxFlatBuilder(HighlightColor, false);
            HighlightHoverStyle = StyleBoxFlatBuilder(HighlightHoverColor, false);

            // Line-based styles
            SeparatorStyle = new StyleBoxLine();
            SeparatorStyle.Color = DisabledFontColor;

            // Control specific styles. Some are redudant for clarity.
            ToolTipPanel = StyleBoxFlatBuilder(TooltipPanelColor);

            ButtonDisabledStyle = PanelTextDisabled;
            ButtonFocusStyle = PanelTextFocusMg;
            ButtonHoverStyle = PanelTextHover;
            ButtonNormalStyle = PanelTextMg;
            ButtonPressedStyle = PanelTextFocusBg;

            TabStylePanel = PanelTextFg;
            TabStyleBg = StyleBoxFlatBuilder(BackgroundColor, true, false, false, true);
            TabStyleDis = StyleBoxFlatBuilder(DisabledBgColor, true, false, false, true);
            TabStyleFg = StyleBoxFlatBuilder(ForegroundColor, true, false, false, true);
            if (BorderSize == 0)
            {
                TabStyleFg.BorderWidthTop = ActiveTabAccentBorderWidth;
                TabStyleFg.BorderColor = SubAccentColor;
            }

            WindowDialogPanelStyle = StyleBoxFlatBuilder(ForegroundColor);
            WindowDialogPanelStyle.BorderWidthTop = 24;
            WindowDialogPanelStyle.ExpandMarginTop = 24;
            WindowDialogPanelStyle.ShadowSize = 4;
            WindowDialogPanelStyle.SetContentMarginAll(8);

            TextEditFocus = PanelTextFocusBg;
            TextEditBackground = PanelTextBg.Copy();
            TextEditReadonly = PanelTextDisabled.Copy();

            if (BorderSize == 0)
            {
                TextEditBackground.BorderWidthBottom = TextEditUnderlineWidth;
                TextEditReadonly.BorderWidthBottom = TextEditUnderlineWidth;
            }
        }

        public void ApplyChanges()
        {
            /*
                There are a multitude of styles available in themes.
                Themes can also hold arbitrary keys/values in them,
                so you could make a whole custom UI system and make a custom
                theme resource they use.

                The main categories used by each control are:
                - Colors (usually for fonts, sometimes for icons)
                - Constants (these are usually parameters for the control)
                - Icons (I don't think I will be setting these procedurally)
                - Styles (Style boxes)

                For the most part, I just need a set of standard
                styleboxes and colors
            */

            GenerateColors();
            GenerateStyles();
            //IconSet.FixAssignments();

            try
            {
                SetBoxContainer();
                SetButton();
                SetCheckBox();
                SetCheckButton();
                SetColorPicker();
                SetColorPickerButton();

                SetEditor();
                SetEditorAbout();
                SetEditorFonts();
                SetEditorHelp();
                SetEditorIcons();
                SetEditorSettingsDialog();
                SetEditorStyles();

                SetFileDialog();
                SetGraphEdit();
                SetGraphNode();

                SetGridContainer();
                SetHBoxContainer();
                SetHScrollBar();
                SetHSeparator();
                SetHSlider();
                SetHSplitContainer();

                SetItemList();

                SetLabel();
                SetLineEdit();
                SetLinkButton();
                SetMarginContainer();
                SetMenuButton();
                SetOptionButton();

                SetPanel();
                SetPanelContainer();

                SetPopupDialog();
                SetPopupMenu();
                SetPopupPanel();

                SetProgressBar();
                SetProjectSettingsEditor();

                SetRichTextLabel();
                SetSpinBox();

                SetTabContainer();
                SetTabs();

                SetTextEdit();
                SetToolButton();
                SetTooltipLabel();
                SetTooltipPanel();

                SetTree();

                SetVBoxContainer();
                SetVSCrollBar();
                SetVSeparator();
                SetVSlider();
                SetVSplitContainer();

                SetWindowDialog();
            }
            catch (Exception e)
            {
                GD.PrintErr($"Exception caught: {e.ToString()}");
            }

            // Place custom after the fact...
        }

        protected void SetBoxContainer()
        {
            //string ControlName = "BoxContainer";
        }

        protected void SetButton()
        {
            string ControlName = nameof(Button);
            // Button
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_pressed", ControlName, PressedFontColor);
            SetColor("icon_color_hover", ControlName, HoverIconColor);
            SetColor("icon_color_pressed", ControlName, PressedIconColor);

            SetStylebox("disabled", ControlName, ButtonDisabledStyle);
            SetStylebox("focus", ControlName, ButtonFocusStyle);
            SetStylebox("hover", ControlName, ButtonHoverStyle);
            SetStylebox("normal", ControlName, ButtonNormalStyle);
            SetStylebox("pressed", ControlName, ButtonPressedStyle);
        }

        protected void SetCheckBox()
        {
            string ControlName = nameof(CheckBox);
            // Check Box
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_pressed", ControlName, PressedFontColor);
            // SetColor("icon_color_hover", ControlName, HoverIconColor);

            // SetConstant("check_vadjust", ControlName, 0);
            // SetConstant("hseparation", ControlName, 4);

            SetIcon("checked", ControlName, IconSet.GuiChecked);
            SetIcon("radio_checked", ControlName, IconSet.GuiRadioChecked);
            SetIcon("radio_unchecked", ControlName, IconSet.GuiRadioUnchecked);
            SetIcon("unchecked", ControlName, IconSet.GuiUnchecked);

            SetStylebox("disabled", ControlName, PanelBgBorderless);
            SetStylebox("hover", ControlName, PanelBgBorderless);
            SetStylebox("normal", ControlName, PanelBgBorderless);
            SetStylebox("pressed", ControlName, PanelBgBorderless);
            SetStylebox("focus", ControlName, PanelFocusBg);
            SetStylebox("hover_pressed", ControlName, PanelFocusBg);
        }

        protected void SetCheckButton()
        {
            string ControlName = nameof(CheckButton);
            // Check Button
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_pressed", ControlName, PressedFontColor);
            // SetColor("icon_color_hover", ControlName, HoverIconColor);

            // SetConstant("check_vadjust", ControlName, 0);
            // SetConstant("hseparation", ControlName, 4);

            SetIcon("off", ControlName, IconSet.GuiToggleOff);
            SetIcon("off_disabled", ControlName, new ImageTexture());
            SetIcon("on", ControlName, IconSet.GuiToggleOn);
            SetIcon("on_disabled", ControlName, new ImageTexture());

            SetStylebox("disabled", ControlName, PanelBgBorderless);
            SetStylebox("hover", ControlName, PanelBgBorderless);
            SetStylebox("normal", ControlName, PanelBgBorderless);
            SetStylebox("pressed", ControlName, PanelBgBorderless);
            SetStylebox("focus", ControlName, PanelFocusBg);
        }

        protected void SetColorPicker()
        {
            string ControlName = nameof(ColorPicker);
            SetIcon("add_preset", ControlName, IconSet.Add);
            SetIcon("overbright_indicator", ControlName, IconSet.OverbrightIndicator);
            SetIcon("preset_bg", ControlName, IconSet.GuiMiniCheckerboard);
            SetIcon("screen_picker", ControlName, IconSet.ColorPick);
        }

        protected void SetColorPickerButton()
        {
            string ControlName = nameof(ColorPickerButton);
            SetIcon("bg", ControlName, IconSet.GuiMiniCheckerboard);
        }

        protected void SetEditor()
        {
            string ControlName = "Editor";
        }

        protected void SetEditorAbout()
        {
            string ControlName = "EditorAbout";
        }

        protected void SetEditorFonts()
        {
            string ControlName = "EditorFonts";
        }

        protected void SetEditorHelp()
        {
            string ControlName = "EditorHelp";
        }

        protected void SetEditorIcons()
        {
            string ControlName = "EditorIcons";
            // Includes ALL engine icons.
            // Setting this might be smart, ultimately, but will be a time sink to setup
        }

        protected void SetEditorSettingsDialog()
        {
            string ControlName = "EditorSettingsDialog";
        }

        protected void SetEditorStyles()
        {
            string ControlName = "EditorStyles";
        }

        protected void SetFileDialog()
        {
            string ControlName = nameof(FileDialog);
            // File Dialog
            SetColor("files_disabled", ControlName, DisabledFontColor);
            SetColor("folder_icon_mod", ControlName, FolderIconMod);

            SetIcon("folder", ControlName, IconSet.Folder);
            SetIcon("parent_folder", ControlName, IconSet.ArrowUp);
            SetIcon("reload", ControlName, IconSet.Reload);
            SetIcon("toggle_hidden", ControlName, IconSet.GuiVisibilityVisible);
        }

        protected void SetGraphEdit()
        {
            string ControlName = nameof(GraphEdit);
            // Graph Edit
            // Leaving the colors for now
            SetStylebox("bg", ControlName, PanelBgBorderless);

            // This one has a few icons, will revisist later
        }

        protected void SetGraphNode()
        {
            // string ControlName = nameof(GraphNode);
            // Graph Node
            // This one is complicated, manually edit as needed for now

            // Has 3 icons, revisit later
        }
        protected void SetGridContainer()
        {
            // string ControlName = nameof(GridContainer);
        }
        protected void SetHBoxContainer()
        {
            // string ControlName = nameof(HBoxContainer);
        }
        protected void SetHScrollBar()
        {
            string ControlName = nameof(HScrollBar);
            StyleBoxTexture grabber = new();
            grabber.Texture = IconSet.GuiScrollGrabber;
            grabber.DrawCenter = true;
            grabber.MarginBottom = 6;
            grabber.MarginLeft = 6;
            grabber.MarginRight = 6;
            grabber.MarginTop = 6;
            grabber.SetExpandMarginAll(0);
            grabber.ContentMarginBottom = 2;
            grabber.ContentMarginLeft = 2;
            grabber.ContentMarginRight = 2;
            grabber.ContentMarginTop = 2;
            grabber.AxisStretchHorizontal = StyleBoxTexture.AxisStretchMode.Stretch;
            grabber.AxisStretchVertical = StyleBoxTexture.AxisStretchMode.Stretch;

            StyleBoxTexture grabberHl = grabber.Copy();
            grabberHl.Texture = IconSet.GuiScrollGrabberHl;
            grabberHl.MarginBottom = 5;
            grabberHl.MarginLeft = 5;
            grabberHl.MarginRight = 5;
            grabberHl.MarginTop = 5;

            StyleBoxTexture grabberPressed = grabber.Copy();
            grabberPressed.Texture = IconSet.GuiScrollGrabberPressed;

            StyleBoxTexture scroll = grabberPressed.Copy();
            ImageTexture scrollTexture = new();
            Image scrollImage = new Image();
            scrollImage.Create(12, 12, false, Image.Format.Rgba8);
            scrollImage.Fill(new Color(0, 0, 0, 0));

            scrollTexture.CreateFromImage(scrollImage);
            scroll.Texture = scrollTexture;
            scroll.ContentMarginBottom = 0;
            scroll.ContentMarginLeft = 0;
            scroll.ContentMarginRight = 0;
            scroll.ContentMarginTop = 0;

            StyleBoxTexture scrollFocus = scroll.Copy();

            SetStylebox("grabber", ControlName, grabber);
            SetStylebox("grabber_highlight", ControlName, grabberHl);
            SetStylebox("grabber_pressed", ControlName, grabberPressed);
            SetStylebox("scroll", ControlName, scroll);
            SetStylebox("scroll_focus", ControlName, scrollFocus);
        }
        protected void SetHSeparator()
        {
            string ControlName = nameof(HSeparator);
            // H Separator
            SetStylebox("separator", ControlName, SeparatorStyle);
        }
        protected void SetHSlider()
        {
            string ControlName = nameof(HSlider);
            // H Slider
            // Also has icons
            SetStylebox("grabber_area", ControlName, HighlightStyle);
            SetStylebox("grabber_area_highlight", ControlName, HighlightStyle);
            SetStylebox("slider", ControlName, PanelBdBorderless);

            SetIcon("grabber", ControlName, IconSet.GuiSliderGrabber);
            SetIcon("grabber_highlight", ControlName, IconSet.GuiSliderGrabberHl);
        }
        protected void SetHSplitContainer()
        {
            string ControlName = nameof(HSplitContainer);

            SetIcon("grabber", ControlName, IconSet.GuiHsplitter);
            SetStylebox("bg", ControlName, PanelBdBorderless); // This is normally a stylebox texture
                                                               // I don't know why it is a texture.
        }
        protected void SetItemList()
        {
            string ControlName = nameof(ItemList);
            // Item List
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_selected", ControlName, MonoColor);
            SetColor("guide_color", ControlName, GuideColor);

            SetStylebox("bg", ControlName, PanelTextBgBorderless);
            SetStylebox("bg_focus", ControlName, PanelTextFocusBg);
            SetStylebox("cursor", ControlName, HighlightStyle);
            SetStylebox("cursor_unfocused", ControlName, HighlightStyle);
            SetStylebox("selected", ControlName, HighlightStyle);
            SetStylebox("selected_focus", ControlName, HighlightStyle);
        }
        protected void SetLabel()
        {
            // ALL PARAMETERS SET!
            string ControlName = nameof(Label);
            // Label
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_shadow", ControlName, new Color(0, 0, 0, 0));

            StyleBoxEmpty LabelBg = new StyleBoxEmpty();
            LabelBg.ContentMarginBottom = TextMarginTopBottom;
            LabelBg.ContentMarginLeft = TextMarginSides;
            LabelBg.ContentMarginRight = TextMarginSides;
            LabelBg.ContentMarginTop = TextMarginTopBottom;
            SetStylebox("normal", ControlName, LabelBg);

            SetConstant("line_spacing", ControlName, 3);
            SetConstant("shadow_as_outline", ControlName, 0);
            SetConstant("shadow_offset_x", ControlName, 1);
            SetConstant("shadow_offset_y", ControlName, 1);
        }
        protected void SetLineEdit()
        {
            string ControlName = nameof(LineEdit);
            // Line Edit
            SetColor("clear_button_color", ControlName, FontColor);
            SetColor("clear_button_color_pressed", ControlName, PressedFontColor);
            SetColor("cursor_color", ControlName, FontColor);
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_select", ControlName, MonoColor);
            SetColor("read_only", ControlName, DisabledFontColor);
            SetColor("selection_color", ControlName, HighlightColor);
            SetColor("font_color_uneditable", ControlName, DisabledFontColor);

            SetIcon("clear", ControlName, IconSet.GuiClose);

            SetStylebox("focus", ControlName, TextEditFocus);
            SetStylebox("normal", ControlName, TextEditBackground);
            SetStylebox("read_only", ControlName, TextEditReadonly);
        }
        protected void SetLinkButton()
        {
            string ControlName = nameof(LinkButton);
            // Link Button
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_pressed", ControlName, PressedFontColor);
        }
        protected void SetMarginContainer()
        {
            // string ControlName = nameof(MarginContainer);
        }
        protected void SetMenuButton()
        {
            string ControlName = nameof(MenuButton);
            // Menu Button
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);

            SetStylebox("disabled", ControlName, PanelTextDisabledBorderless);
            SetStylebox("focus", ControlName, PanelTextBgBorderless);
            SetStylebox("hover", ControlName, PanelTextHoverBorderless);
            SetStylebox("normal", ControlName, PanelTextBgBorderless);
            SetStylebox("pressed", ControlName, PanelTextBgBorderless);
        }
        protected void SetOptionButton()
        {
            string ControlName = nameof(OptionButton);
            // Option Button
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_pressed", ControlName, PressedFontColor);

            SetIcon("arrow", ControlName, IconSet.GuiOptionArrow);

            SetStylebox("disabled", ControlName, ButtonDisabledStyle);
            SetStylebox("focus", ControlName, ButtonFocusStyle);
            SetStylebox("hover", ControlName, ButtonHoverStyle);
            SetStylebox("normal", ControlName, ButtonNormalStyle);
            SetStylebox("pressed", ControlName, ButtonPressedStyle);
        }
        protected void SetPanel()
        {
            string ControlName = nameof(Panel);
            // Panel
            SetStylebox("panel", ControlName, PanelBgBorderless);
        }
        protected void SetPanelContainer()
        {
            string ControlName = nameof(PanelContainer);
            // Panel Container
            SetStylebox("panel", ControlName, PanelBgBorderless);
        }
        protected void SetPopupDialog()
        {
            string ControlName = nameof(PopupDialog);
            // Popup Dialog
            SetStylebox("panel", ControlName, PanelFg);
        }
        protected void SetPopupMenu()
        {
            string ControlName = nameof(PopupMenu);
            // Popup Menu
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_accel", ControlName, DisabledFontColor);

            SetIcon("checked", ControlName, IconSet.GuiChecked);
            SetIcon("radio_checked", ControlName, IconSet.GuiRadioChecked);
            SetIcon("radio_unchecked", ControlName, IconSet.GuiRadioUnchecked);
            SetIcon("submenu", ControlName, IconSet.ArrowRight);
            SetIcon("unchecked", ControlName, IconSet.GuiUnchecked);
            SetIcon("visibility_hidden", ControlName, IconSet.GuiVisibilityHidden);
            SetIcon("visibility_visible", ControlName, IconSet.GuiVisibilityVisible);
            SetIcon("visibility_xray", ControlName, IconSet.GuiVisibilityXray);

            SetStylebox("disabled", ControlName, PanelBgBorderless);
            SetStylebox("focus", ControlName, PanelBgBorderless);
            SetStylebox("hover", ControlName, PanelHoverBorderless);
            SetStylebox("labeled_separator_left", ControlName, SeparatorStyle);
            SetStylebox("labeled_separator_right", ControlName, SeparatorStyle);
            SetStylebox("normal", ControlName, PanelBgBorderless);
            SetStylebox("panel", ControlName, PanelTextFg);
            SetStylebox("pressed", ControlName, PanelFocusBg); // Double check this
            SetStylebox("separator", ControlName, SeparatorStyle);
        }
        protected void SetPopupPanel()
        {
            string ControlName = nameof(PopupPanel);
            // Popup Panel
            SetStylebox("panel", ControlName, PanelFg);
        }
        protected void SetProgressBar()
        {
            string ControlName = nameof(ProgressBar);
            // Progress Bar
            SetColor("font_color", ControlName, FontColor);

            StyleBoxTexture Bg = new();

            Bg.Texture = IconSet.GuiProgressBar;
            Bg.DrawCenter = true;
            Bg.MarginBottom = 4;
            Bg.MarginLeft = 4;
            Bg.MarginRight = 4;
            Bg.MarginTop = 4;
            Bg.SetExpandMarginAll(0);
            Bg.ContentMarginBottom = 0;
            Bg.ContentMarginLeft = 0;
            Bg.ContentMarginRight = 0;
            Bg.ContentMarginTop = 0;
            Bg.AxisStretchHorizontal = StyleBoxTexture.AxisStretchMode.Stretch;
            Bg.AxisStretchVertical = StyleBoxTexture.AxisStretchMode.Stretch;

            StyleBoxTexture Fg = Bg.Copy();
            Fg.MarginBottom = 6;
            Fg.MarginLeft = 6;
            Fg.MarginRight = 6;
            Fg.MarginTop = 6;
            Fg.ContentMarginBottom = 1;
            Fg.ContentMarginLeft = 2;
            Fg.ContentMarginRight = 2;
            Fg.ContentMarginTop = 1;

            SetStylebox("bg", ControlName, Bg);
            SetStylebox("fg", ControlName, Fg);
        }
        protected void SetProjectSettingsEditor()
        {
            // string ControlName = "ProjectSettingsEditor";
            // Project Settings Editor
            // Passing for now
        }
        protected void SetRichTextLabel()
        {
            string ControlName = nameof(RichTextLabel);
            // Rich Text Label
            SetColor("default_color", ControlName, FontColor);
            SetColor("selection_color", ControlName, HighlightColor);
            SetColor("font_color_selected", ControlName, MonoColor);
            SetColor("font_color_shadow", ControlName, new Color(0, 0, 0, 0));

            SetStylebox("normal", ControlName, PanelTextBgBorderless);
            SetStylebox("focus", ControlName, PanelTextFocusBg);
        }
        protected void SetSpinBox()
        {
            string ControlName = nameof(SpinBox);

            SetIcon("updown", ControlName, IconSet.GuiSpinboxUpdown);
        }
        protected void SetTabContainer()
        {
            string ControlName = nameof(TabContainer);
            // Tab Container
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_bg", ControlName, DisabledFontColor);
            SetColor("font_color_fg", ControlName, FontColor);

            SetIcon("decrement", ControlName, IconSet.GuiScrollArrowLeft);
            SetIcon("decrement_highlight", ControlName, IconSet.GuiScrollArrowLeftHl);
            SetIcon("increment", ControlName, IconSet.GuiScrollArrowRight);
            SetIcon("increment_highlight", ControlName, IconSet.GuiScrollArrowRightHl);
            SetIcon("menu", ControlName, IconSet.GuiTabMenu);
            SetIcon("menu_highlight", ControlName, IconSet.GuiTabMenuHl);

            SetStylebox("panel", ControlName, TabStylePanel);
            // All the bottom borders need to be zero
            SetStylebox("tab_bg", ControlName, TabStyleBg);
            SetStylebox("tab_disabled", ControlName, TabStyleDis);
            SetStylebox("tab_fg", ControlName, TabStyleFg);

        }
        protected void SetTabs()
        {
            string ControlName = nameof(Tabs);
            // Tabs
            SetColor("font_color_disabled", ControlName, DisabledFontColor);
            SetColor("font_color_bg", ControlName, DisabledFontColor);
            SetColor("font_color_fg", ControlName, FontColor);

            SetIcon("close", ControlName, IconSet.GuiClose);
            SetIcon("decrement", ControlName, IconSet.GuiScrollArrowLeft);
            SetIcon("decrement_highlight", ControlName, IconSet.GuiScrollArrowLeftHl);
            SetIcon("increment", ControlName, IconSet.GuiScrollArrowRight);
            SetIcon("increment_highlight", ControlName, IconSet.GuiScrollArrowRightHl);

            SetStylebox("button", ControlName, ButtonNormalStyle);
            SetStylebox("button_pressed", ControlName, ButtonPressedStyle);
            SetStylebox("tab_bg", ControlName, TabStyleBg);
            SetStylebox("tab_disabled", ControlName, TabStyleDis);
            SetStylebox("tab_fg", ControlName, TabStyleFg);
        }
        protected void SetTextEdit()
        {
            string ControlName = nameof(TextEdit);
            // Text Edit
            SetColor("caret_color", ControlName, FontColor);
            SetColor("font_color", ControlName, FontColor);
            SetColor("selection_color", ControlName, HighlightColor);

            SetIcon("fold", ControlName, IconSet.GuiTreeArrowDown);
            SetIcon("folded", ControlName, IconSet.GuiTreeArrowRight);
            SetIcon("space", ControlName, IconSet.GuiSpace);
            SetIcon("tab", ControlName, IconSet.GuiTab);

            SetStylebox("focus", ControlName, TextEditFocus);
            SetStylebox("normal", ControlName, TextEditBackground);
            SetStylebox("read_only", ControlName, TextEditReadonly);
        }

        protected void SetToolButton()
        {
            string ControlName = nameof(ToolButton);
            // Tool Button
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_hover", ControlName, HighlightedFontColor);
            SetColor("font_color_pressed", ControlName, PressedFontColor);

            SetStylebox("disabled", ControlName, PanelTextBgBorderless);
            SetStylebox("focus", ControlName, ButtonFocusStyle);
            SetStylebox("hover", ControlName, PanelTextBgBorderless);
            SetStylebox("normal", ControlName, PanelTextBgBorderless);
            SetStylebox("pressed", ControlName, PanelTextBgBorderless);
        }
        protected void SetTooltipLabel()
        {
            string ControlName = "TooltipLabel";
            // Tooltip Label
            SetColor("font_color", ControlName, TooltipFontColor);
            SetColor("font_color_shadow", ControlName, TooltipFontShadowColor);
        }
        protected void SetTooltipPanel()
        {
            string ControlName = "TooltipPanel";
            // Tooltip Panel
            SetStylebox("panel", ControlName, ToolTipPanel);
        }
        protected void SetTree()
        {
            string ControlName = nameof(Tree);
            // Tree
            SetColor("custom_button_font_highlight", ControlName, HighlightedFontColor);
            SetColor("drop_position_color", ControlName, PressedFontColor);
            SetColor("font_color", ControlName, FontColor);
            SetColor("font_color_selected", ControlName, MonoColor);
            SetColor("title_button_color", ControlName, FontColor);

            SetColor("guide_color", ControlName, GuideColor);
            SetColor("relationship_line_color", ControlName, RelationshipLineColor);
            SetColor("children_hl_line_color", ControlName, ChildrenHlLineColor);
            SetColor("parent_hl_line_color", ControlName, ParentHlLineColor);

            SetIcon("arrow", ControlName, IconSet.GuiTreeArrowDown);
            SetIcon("arrow_collapsed", ControlName, IconSet.GuiTreeArrowRight);
            SetIcon("checked", ControlName, IconSet.GuiChecked);
            SetIcon("select_arrow", ControlName, IconSet.GuiTreeArrowDown);
            SetIcon("unchecked", ControlName, IconSet.GuiUnchecked);
            SetIcon("updown", ControlName, IconSet.GuiTreeArrowUpdown);

            SetStylebox("bg", ControlName, PanelBgBorderless);
            SetStylebox("bg_focus", ControlName, PanelFocusBg);
            SetStylebox("selected", ControlName, HighlightStyle);
            SetStylebox("selected_focus", ControlName, HighlightStyle);
            SetStylebox("button_pressed", ControlName, ButtonPressedStyle);
            SetStylebox("cursor", ControlName, HighlightStyle);
            SetStylebox("cursor_unfocused", ControlName, HighlightStyle);
            SetStylebox("custom_button", ControlName, new StyleBoxEmpty());
            SetStylebox("custom_button_hover", ControlName, ButtonHoverStyle);
            SetStylebox("custom_button_pressed", ControlName, new StyleBoxEmpty());
            SetStylebox("hover", ControlName, HighlightHoverStyle);
            SetStylebox("title_button_hover", ControlName, PanelBdBorderless);
            SetStylebox("title_button_normal", ControlName, PanelBdBorderless);
            SetStylebox("title_button_pressed", ControlName, PanelBdBorderless);
        }
        protected void SetVBoxContainer()
        {
            string ControlName = nameof(VBoxContainer);
        }
        protected void SetVSCrollBar()
        {
            string ControlName = nameof(VScrollBar);

            StyleBoxTexture grabber = new();
            grabber.Texture = IconSet.GuiScrollGrabber;
            grabber.DrawCenter = true;
            grabber.MarginBottom = 6;
            grabber.MarginLeft = 6;
            grabber.MarginRight = 6;
            grabber.MarginTop = 6;
            grabber.SetExpandMarginAll(0);
            grabber.ContentMarginBottom = 2;
            grabber.ContentMarginLeft = 2;
            grabber.ContentMarginRight = 2;
            grabber.ContentMarginTop = 2;
            grabber.AxisStretchHorizontal = StyleBoxTexture.AxisStretchMode.Stretch;
            grabber.AxisStretchVertical = StyleBoxTexture.AxisStretchMode.Stretch;

            StyleBoxTexture grabberHl = grabber.Copy();
            grabberHl.Texture = IconSet.GuiScrollGrabberHl;
            grabberHl.MarginBottom = 5;
            grabberHl.MarginLeft = 5;
            grabberHl.MarginRight = 5;
            grabberHl.MarginTop = 5;

            StyleBoxTexture grabberPressed = grabber.Copy();
            grabberPressed.Texture = IconSet.GuiScrollGrabberPressed;

            StyleBoxTexture scroll = grabberPressed.Copy();
            ImageTexture scrollTexture = new();
            Image scrollImage = new Image();
            scrollImage.Create(12, 12, false, Image.Format.Rgba8);
            scrollImage.Fill(new Color(0, 0, 0, 0));

            scrollTexture.CreateFromImage(scrollImage);
            scroll.Texture = scrollTexture;
            scroll.ContentMarginBottom = 0;
            scroll.ContentMarginLeft = 0;
            scroll.ContentMarginRight = 0;
            scroll.ContentMarginTop = 0;

            StyleBoxTexture scrollFocus = scroll.Copy();

            SetStylebox("grabber", ControlName, grabber);
            SetStylebox("grabber_highlight", ControlName, grabberHl);
            SetStylebox("grabber_pressed", ControlName, grabberPressed);
            SetStylebox("scroll", ControlName, scroll);
            SetStylebox("scroll_focus", ControlName, scrollFocus);
        }
        protected void SetVSeparator()
        {
            string ControlName = nameof(VSeparator);
            // V Separator
            SetStylebox("separator", ControlName, SeparatorStyle);
        }
        protected void SetVSlider()
        {
            string ControlName = nameof(VSlider);
            // V Slider
            SetIcon("grabber", ControlName, IconSet.GuiSliderGrabber);
            SetIcon("grabber_highlight", ControlName, IconSet.GuiSliderGrabberHl);

            SetStylebox("grabber_area", ControlName, HighlightStyle);
            SetStylebox("grabber_area_highlight", ControlName, HighlightStyle);
            SetStylebox("slider", ControlName, PanelBdBorderless);
        }
        protected void SetVSplitContainer()
        {
            string ControlName = nameof(VSplitContainer);

            SetIcon("grabber", ControlName, IconSet.GuiVsplitter);

            SetStylebox("bg", ControlName, PanelBdBorderless);
        }
        protected void SetWindowDialog()
        {
            string ControlName = nameof(WindowDialog);
            // Window Dialog
            SetColor("title_color", ControlName, FontColor);

            SetIcon("close", ControlName, IconSet.GuiClose);
            SetIcon("close_highlight", ControlName, IconSet.GuiCloseCustomizable);

            SetStylebox("panel", ControlName, WindowDialogPanelStyle);
        }
    }
}


