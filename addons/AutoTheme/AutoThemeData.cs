using Godot;
using System;
using SparkLib.GodotLib.Theme;

namespace AutoThemes
{
    public enum FontIconSetting { Dark, Light }
    public class AutoThemeData : Godot.Resource
    {
        /*
        The colors and styleboxes define the model of my theme.
        How those colors and styleboxes are applied depends on the
        controls themselves.
        */

        // Exported parameters
        // internal FontIconSetting iconAndFontColor = FontIconSetting.Light;
        // internal Color baseColor = new("3D3D3D");
        // internal Color accentColor = new("b8e3ff");
        // internal float contrast = 0.2f;
        // internal float iconSaturation = 1.0f;
        // internal float relationshipLineOpacity = 0.1f;
        // internal int borderSize = 0;
        // internal int cornerRadius = 3;
        // internal float additionalSpacing = 0.0f;
        // internal int focusBorderSize = 1;
        // public AutoThemeIcons owner.IconSetLight = new();
        // public AutoThemeIcons iconSetDark = new();

        // iconSet selection
        internal AutoThemeIcons? iconSet = null;
        public AutoThemeIcons? IconSet
        {
            get => iconSet;
        }

        // Base font colors
        public Color DarkFontColor = new("383838");
        public Color LightFontColor = new("cdcfd2");
        public Color FontColor;

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

        protected IAutoTheme owner;

        public AutoThemeData(IAutoTheme owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// This is ONLY for Godot's binding purposes.
        /// </summary>
        public AutoThemeData()
        {

        }

        protected void GenerateColors()
        {
            switch (owner.IconAndFontColor)
            {
                case FontIconSetting.Light:
                    FontColor = LightFontColor;
                    iconSet = owner.IconSetLight;
                    break;
                case FontIconSetting.Dark:
                    FontColor = DarkFontColor;
                    iconSet = owner.IconSetDark;
                    break;
                default:
                    FontColor = LightFontColor;
                    iconSet = owner.IconSetLight;
                    break;
            }

            // Create derived colors based on contrast settings
            SubAccentColor = owner.AccentColor.Darkened(0.2f);
            if (owner.Contrast == 0)
            {
                ForegroundColor = owner.BaseColor;
                MidgroundColor = owner.BaseColor;
                BackgroundColor = owner.BaseColor;
                BackdropColor = BackgroundColor.Darkened(0.1f);
                DisabledBgColor = owner.BaseColor;

                HoverColor = owner.BaseColor.Lightened(0.1f);
                ForegroundContrastColor = owner.BaseColor.Lightened(0.2f);

                HighlightColor = owner.AccentColor.Lightened(0.1f);
            }
            else if (owner.Contrast > 0)
            {
                ForegroundColor = owner.BaseColor;
                MidgroundColor = owner.BaseColor.Darkened(owner.Contrast * 0.2f);
                BackgroundColor = owner.BaseColor.Darkened(owner.Contrast * 0.80f);
                BackdropColor = BackgroundColor.Darkened(owner.Contrast);
                DisabledBgColor = owner.BaseColor.Darkened(owner.Contrast * 0.6f);

                HoverColor = owner.BaseColor.Lightened(owner.Contrast * 0.1f);
                ForegroundContrastColor = owner.BaseColor.Lightened(owner.Contrast * 0.2f);

                HighlightColor = owner.AccentColor.Lightened(0.1f);
            }
            else if (owner.Contrast < 0)
            {
                float absContrast = Godot.Mathf.Abs(owner.Contrast);

                ForegroundColor = owner.BaseColor;
                MidgroundColor = owner.BaseColor.Lightened(absContrast * 0.2f);
                BackgroundColor = owner.BaseColor.Lightened(absContrast * 0.80f);
                BackdropColor = owner.BaseColor.Lightened(absContrast);
                DisabledBgColor = owner.BaseColor.Lightened(absContrast * 0.6f);

                HoverColor = owner.BaseColor.Darkened(absContrast * 0.1f);
                ForegroundContrastColor = ForegroundColor.Darkened(absContrast * 0.2f);

                HighlightColor = owner.AccentColor.Darkened(0.1f);
            }
            BackgroundContrastColor = BackdropColor.Contrasted();
            HighlightColor.a = 0.2f;
            //DisabledBgColor.s -= 0.05f;
            HighlightHoverColor = new Color(HoverColor);
            HighlightHoverColor.a = 0.08f;

            BoxSelectionStrokeColor = new(owner.AccentColor);
            BoxSelectionStrokeColor.a = 0.8f;
            BoxSelectionFillColor = new(owner.AccentColor);
            BoxSelectionFillColor.a = 0.3f;

            // Make sure derived font colors follow global setting
            if (owner.IconAndFontColor == FontIconSetting.Light)
            {
                HighlightedFontColor = new Color(FontColor);
                HighlightedFontColor.Lightened(0.1f);
                MonoColor = new Color(1, 1, 1);
                // Tooltip should oppose whatever the main color level is
                TooltipFontColor = DarkFontColor;
                TooltipPanelColor = BackgroundContrastColor;
            }
            else if (owner.IconAndFontColor == FontIconSetting.Dark)
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
            PressedIconColor = owner.AccentColor.Lightened(0.2f);

            // When clicking on buttons, will make the font
            // a color brighter or darker than the accent color
            // depending on the accent color's brightness.
            PressedFontColor = new Color(owner.AccentColor);
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
            RelationshipLineColor.a = owner.RelationshipLineOpacity;
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
                newStyle.SetBorderWidthAll(owner.FocusBorderSize);
                newStyle.BorderColor = owner.AccentColor;
            }
            else if (borders)
            {
                newStyle.SetBorderWidthAll(owner.BorderSize);
                newStyle.BorderColor = BackgroundContrastColor;
            }
            else
            {
                newStyle.SetBorderWidthAll(0);
                newStyle.BorderColor = BackgroundContrastColor;
            }
            newStyle.BorderBlend = false;
            newStyle.SetCornerRadiusAll(owner.CornerRadius);
            newStyle.SetExpandMarginAll(0);

            newStyle.ShadowColor = new Color(0, 0, 0, 0);
            newStyle.ShadowSize = 0;
            newStyle.ShadowOffset = new Vector2(0, 0);
            newStyle.AntiAliasing = true;
            newStyle.AntiAliasingSize = 1;
            newStyle.SetContentMarginAll(owner.AdditionalSpacing);

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
                style.ContentMarginLeft = 10 + owner.AdditionalSpacing;
                style.ContentMarginRight = 10 + owner.AdditionalSpacing;
                style.ContentMarginTop = 5 + owner.AdditionalSpacing;
                style.ContentMarginBottom = 5 + owner.AdditionalSpacing;
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
            if (owner.BorderSize == 0)
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

            if (owner.BorderSize == 0)
            {
                TextEditBackground.BorderWidthBottom = TextEditUnderlineWidth;
                TextEditReadonly.BorderWidthBottom = TextEditUnderlineWidth;
            }
        }

        public void ApplyToTheme()
        {
            GenerateColors();
            GenerateStyles();

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
        }

        protected void SetBoxContainer()
        {
            //string ControlName = "BoxContainer";
        }

        protected void SetButton()
        {
            string ControlName = nameof(Button);
            // Button
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_pressed", ControlName, PressedFontColor);
            owner.ThemeResource.SetColor("icon_color_hover", ControlName, HoverIconColor);
            owner.ThemeResource.SetColor("icon_color_pressed", ControlName, PressedIconColor);

            owner.ThemeResource.SetStylebox("disabled", ControlName, ButtonDisabledStyle);
            owner.ThemeResource.SetStylebox("focus", ControlName, ButtonFocusStyle);
            owner.ThemeResource.SetStylebox("hover", ControlName, ButtonHoverStyle);
            owner.ThemeResource.SetStylebox("normal", ControlName, ButtonNormalStyle);
            owner.ThemeResource.SetStylebox("pressed", ControlName, ButtonPressedStyle);
        }

        protected void SetCheckBox()
        {
            string ControlName = nameof(CheckBox);
            // Check Box
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_pressed", ControlName, PressedFontColor);
            // owner.ThemeResource.SetColor("icon_color_hover", ControlName, HoverIconColor);

            // owner.ThemeResource.SetConstant("check_vadjust", ControlName, 0);
            // owner.ThemeResource.SetConstant("hseparation", ControlName, 4);

            owner.ThemeResource.SetIcon("checked", ControlName, iconSet.GuiChecked);
            owner.ThemeResource.SetIcon("radio_checked", ControlName, iconSet.GuiRadioChecked);
            owner.ThemeResource.SetIcon("radio_unchecked", ControlName, iconSet.GuiRadioUnchecked);
            owner.ThemeResource.SetIcon("unchecked", ControlName, iconSet.GuiUnchecked);

            owner.ThemeResource.SetStylebox("disabled", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("hover", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("normal", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("pressed", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("focus", ControlName, PanelFocusBg);
            owner.ThemeResource.SetStylebox("hover_pressed", ControlName, PanelFocusBg);
        }

        protected void SetCheckButton()
        {
            string ControlName = nameof(CheckButton);
            // Check Button
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_pressed", ControlName, PressedFontColor);
            // owner.ThemeResource.SetColor("icon_color_hover", ControlName, HoverIconColor);

            // owner.ThemeResource.SetConstant("check_vadjust", ControlName, 0);
            // owner.ThemeResource.SetConstant("hseparation", ControlName, 4);

            owner.ThemeResource.SetIcon("off", ControlName, iconSet.GuiToggleOff);
            owner.ThemeResource.SetIcon("off_disabled", ControlName, new ImageTexture());
            owner.ThemeResource.SetIcon("on", ControlName, iconSet.GuiToggleOn);
            owner.ThemeResource.SetIcon("on_disabled", ControlName, new ImageTexture());

            owner.ThemeResource.SetStylebox("disabled", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("hover", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("normal", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("pressed", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("focus", ControlName, PanelFocusBg);
        }

        protected void SetColorPicker()
        {
            string ControlName = nameof(ColorPicker);
            owner.ThemeResource.SetIcon("add_preset", ControlName, iconSet.Add);
            owner.ThemeResource.SetIcon("overbright_indicator", ControlName, iconSet.OverbrightIndicator);
            owner.ThemeResource.SetIcon("preset_bg", ControlName, iconSet.GuiMiniCheckerboard);
            owner.ThemeResource.SetIcon("screen_picker", ControlName, iconSet.ColorPick);
        }

        protected void SetColorPickerButton()
        {
            string ControlName = nameof(ColorPickerButton);
            owner.ThemeResource.SetIcon("bg", ControlName, iconSet.GuiMiniCheckerboard);
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
            owner.ThemeResource.SetColor("files_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("folder_icon_mod", ControlName, FolderIconMod);

            owner.ThemeResource.SetIcon("folder", ControlName, iconSet.Folder);
            owner.ThemeResource.SetIcon("parent_folder", ControlName, iconSet.ArrowUp);
            owner.ThemeResource.SetIcon("reload", ControlName, iconSet.Reload);
            owner.ThemeResource.SetIcon("toggle_hidden", ControlName, iconSet.GuiVisibilityVisible);
        }

        protected void SetGraphEdit()
        {
            string ControlName = nameof(GraphEdit);
            // Graph Edit
            // Leaving the colors for now
            owner.ThemeResource.SetStylebox("bg", ControlName, PanelBgBorderless);

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
            grabber.Texture = iconSet.GuiScrollGrabber;
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
            grabberHl.Texture = iconSet.GuiScrollGrabberHl;
            grabberHl.MarginBottom = 5;
            grabberHl.MarginLeft = 5;
            grabberHl.MarginRight = 5;
            grabberHl.MarginTop = 5;

            StyleBoxTexture grabberPressed = grabber.Copy();
            grabberPressed.Texture = iconSet.GuiScrollGrabberPressed;

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

            owner.ThemeResource.SetStylebox("grabber", ControlName, grabber);
            owner.ThemeResource.SetStylebox("grabber_highlight", ControlName, grabberHl);
            owner.ThemeResource.SetStylebox("grabber_pressed", ControlName, grabberPressed);
            owner.ThemeResource.SetStylebox("scroll", ControlName, scroll);
            owner.ThemeResource.SetStylebox("scroll_focus", ControlName, scrollFocus);
        }
        protected void SetHSeparator()
        {
            string ControlName = nameof(HSeparator);
            // H Separator
            owner.ThemeResource.SetStylebox("separator", ControlName, SeparatorStyle);
        }
        protected void SetHSlider()
        {
            string ControlName = nameof(HSlider);
            // H Slider
            // Also has icons
            owner.ThemeResource.SetStylebox("grabber_area", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("grabber_area_highlight", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("slider", ControlName, PanelBdBorderless);

            owner.ThemeResource.SetIcon("grabber", ControlName, iconSet.GuiSliderGrabber);
            owner.ThemeResource.SetIcon("grabber_highlight", ControlName, iconSet.GuiSliderGrabberHl);
        }
        protected void SetHSplitContainer()
        {
            string ControlName = nameof(HSplitContainer);

            owner.ThemeResource.SetIcon("grabber", ControlName, iconSet.GuiHsplitter);
            owner.ThemeResource.SetStylebox("bg", ControlName, PanelBdBorderless); // This is normally a stylebox texture
                                                                                   // I don't know why it is a texture.
        }
        protected void SetItemList()
        {
            string ControlName = nameof(ItemList);
            // Item List
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_selected", ControlName, MonoColor);
            owner.ThemeResource.SetColor("guide_color", ControlName, GuideColor);

            owner.ThemeResource.SetStylebox("bg", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("bg_focus", ControlName, PanelTextFocusBg);
            owner.ThemeResource.SetStylebox("cursor", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("cursor_unfocused", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("selected", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("selected_focus", ControlName, HighlightStyle);
        }
        protected void SetLabel()
        {
            // ALL PARAMETERS SET!
            string ControlName = nameof(Label);
            // Label
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_shadow", ControlName, new Color(0, 0, 0, 0));

            StyleBoxEmpty LabelBg = new StyleBoxEmpty();
            LabelBg.ContentMarginBottom = TextMarginTopBottom;
            LabelBg.ContentMarginLeft = TextMarginSides;
            LabelBg.ContentMarginRight = TextMarginSides;
            LabelBg.ContentMarginTop = TextMarginTopBottom;
            owner.ThemeResource.SetStylebox("normal", ControlName, LabelBg);

            owner.ThemeResource.SetConstant("line_spacing", ControlName, 3);
            owner.ThemeResource.SetConstant("shadow_as_outline", ControlName, 0);
            owner.ThemeResource.SetConstant("shadow_offset_x", ControlName, 1);
            owner.ThemeResource.SetConstant("shadow_offset_y", ControlName, 1);
        }
        protected void SetLineEdit()
        {
            string ControlName = nameof(LineEdit);
            // Line Edit
            owner.ThemeResource.SetColor("clear_button_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("clear_button_color_pressed", ControlName, PressedFontColor);
            owner.ThemeResource.SetColor("cursor_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_select", ControlName, MonoColor);
            owner.ThemeResource.SetColor("read_only", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("selection_color", ControlName, HighlightColor);
            owner.ThemeResource.SetColor("font_color_uneditable", ControlName, DisabledFontColor);

            owner.ThemeResource.SetIcon("clear", ControlName, iconSet.GuiClose);

            owner.ThemeResource.SetStylebox("focus", ControlName, TextEditFocus);
            owner.ThemeResource.SetStylebox("normal", ControlName, TextEditBackground);
            owner.ThemeResource.SetStylebox("read_only", ControlName, TextEditReadonly);
        }
        protected void SetLinkButton()
        {
            string ControlName = nameof(LinkButton);
            // Link Button
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_pressed", ControlName, PressedFontColor);
        }
        protected void SetMarginContainer()
        {
            // string ControlName = nameof(MarginContainer);
        }
        protected void SetMenuButton()
        {
            string ControlName = nameof(MenuButton);
            // Menu Button
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);

            owner.ThemeResource.SetStylebox("disabled", ControlName, PanelTextDisabledBorderless);
            owner.ThemeResource.SetStylebox("focus", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("hover", ControlName, PanelTextHoverBorderless);
            owner.ThemeResource.SetStylebox("normal", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("pressed", ControlName, PanelTextBgBorderless);
        }
        protected void SetOptionButton()
        {
            string ControlName = nameof(OptionButton);
            // Option Button
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_pressed", ControlName, PressedFontColor);

            owner.ThemeResource.SetIcon("arrow", ControlName, iconSet.GuiOptionArrow);

            owner.ThemeResource.SetStylebox("disabled", ControlName, ButtonDisabledStyle);
            owner.ThemeResource.SetStylebox("focus", ControlName, ButtonFocusStyle);
            owner.ThemeResource.SetStylebox("hover", ControlName, ButtonHoverStyle);
            owner.ThemeResource.SetStylebox("normal", ControlName, ButtonNormalStyle);
            owner.ThemeResource.SetStylebox("pressed", ControlName, ButtonPressedStyle);
        }
        protected void SetPanel()
        {
            string ControlName = nameof(Panel);
            // Panel
            owner.ThemeResource.SetStylebox("panel", ControlName, PanelBgBorderless);
        }
        protected void SetPanelContainer()
        {
            string ControlName = nameof(PanelContainer);
            // Panel Container
            owner.ThemeResource.SetStylebox("panel", ControlName, PanelBgBorderless);
        }
        protected void SetPopupDialog()
        {
            string ControlName = nameof(PopupDialog);
            // Popup Dialog
            owner.ThemeResource.SetStylebox("panel", ControlName, PanelFg);
        }
        protected void SetPopupMenu()
        {
            string ControlName = nameof(PopupMenu);
            // Popup Menu
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_accel", ControlName, DisabledFontColor);

            owner.ThemeResource.SetIcon("checked", ControlName, iconSet.GuiChecked);
            owner.ThemeResource.SetIcon("radio_checked", ControlName, iconSet.GuiRadioChecked);
            owner.ThemeResource.SetIcon("radio_unchecked", ControlName, iconSet.GuiRadioUnchecked);
            owner.ThemeResource.SetIcon("submenu", ControlName, iconSet.ArrowRight);
            owner.ThemeResource.SetIcon("unchecked", ControlName, iconSet.GuiUnchecked);
            owner.ThemeResource.SetIcon("visibility_hidden", ControlName, iconSet.GuiVisibilityHidden);
            owner.ThemeResource.SetIcon("visibility_visible", ControlName, iconSet.GuiVisibilityVisible);
            owner.ThemeResource.SetIcon("visibility_xray", ControlName, iconSet.GuiVisibilityXray);

            owner.ThemeResource.SetStylebox("disabled", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("focus", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("hover", ControlName, PanelHoverBorderless);
            owner.ThemeResource.SetStylebox("labeled_separator_left", ControlName, SeparatorStyle);
            owner.ThemeResource.SetStylebox("labeled_separator_right", ControlName, SeparatorStyle);
            owner.ThemeResource.SetStylebox("normal", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("panel", ControlName, PanelTextFg);
            owner.ThemeResource.SetStylebox("pressed", ControlName, PanelFocusBg); // Double check this
            owner.ThemeResource.SetStylebox("separator", ControlName, SeparatorStyle);
        }
        protected void SetPopupPanel()
        {
            string ControlName = nameof(PopupPanel);
            // Popup Panel
            owner.ThemeResource.SetStylebox("panel", ControlName, PanelFg);
        }
        protected void SetProgressBar()
        {
            string ControlName = nameof(ProgressBar);
            // Progress Bar
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);

            StyleBoxTexture Bg = new();

            Bg.Texture = iconSet.GuiProgressBar;
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

            owner.ThemeResource.SetStylebox("bg", ControlName, Bg);
            owner.ThemeResource.SetStylebox("fg", ControlName, Fg);
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
            owner.ThemeResource.SetColor("default_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("selection_color", ControlName, HighlightColor);
            owner.ThemeResource.SetColor("font_color_selected", ControlName, MonoColor);
            owner.ThemeResource.SetColor("font_color_shadow", ControlName, new Color(0, 0, 0, 0));

            owner.ThemeResource.SetStylebox("normal", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("focus", ControlName, PanelTextFocusBg);
        }
        protected void SetSpinBox()
        {
            string ControlName = nameof(SpinBox);

            owner.ThemeResource.SetIcon("updown", ControlName, iconSet.GuiSpinboxUpdown);
        }
        protected void SetTabContainer()
        {
            string ControlName = nameof(TabContainer);
            // Tab Container
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_bg", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_fg", ControlName, FontColor);

            owner.ThemeResource.SetIcon("decrement", ControlName, iconSet.GuiScrollArrowLeft);
            owner.ThemeResource.SetIcon("decrement_highlight", ControlName, iconSet.GuiScrollArrowLeftHl);
            owner.ThemeResource.SetIcon("increment", ControlName, iconSet.GuiScrollArrowRight);
            owner.ThemeResource.SetIcon("increment_highlight", ControlName, iconSet.GuiScrollArrowRightHl);
            owner.ThemeResource.SetIcon("menu", ControlName, iconSet.GuiTabMenu);
            owner.ThemeResource.SetIcon("menu_highlight", ControlName, iconSet.GuiTabMenuHl);

            owner.ThemeResource.SetStylebox("panel", ControlName, TabStylePanel);
            // All the bottom borders need to be zero
            owner.ThemeResource.SetStylebox("tab_bg", ControlName, TabStyleBg);
            owner.ThemeResource.SetStylebox("tab_disabled", ControlName, TabStyleDis);
            owner.ThemeResource.SetStylebox("tab_fg", ControlName, TabStyleFg);

        }
        protected void SetTabs()
        {
            string ControlName = nameof(Tabs);
            // Tabs
            owner.ThemeResource.SetColor("font_color_disabled", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_bg", ControlName, DisabledFontColor);
            owner.ThemeResource.SetColor("font_color_fg", ControlName, FontColor);

            owner.ThemeResource.SetIcon("close", ControlName, iconSet.GuiClose);
            owner.ThemeResource.SetIcon("decrement", ControlName, iconSet.GuiScrollArrowLeft);
            owner.ThemeResource.SetIcon("decrement_highlight", ControlName, iconSet.GuiScrollArrowLeftHl);
            owner.ThemeResource.SetIcon("increment", ControlName, iconSet.GuiScrollArrowRight);
            owner.ThemeResource.SetIcon("increment_highlight", ControlName, iconSet.GuiScrollArrowRightHl);

            owner.ThemeResource.SetStylebox("button", ControlName, ButtonNormalStyle);
            owner.ThemeResource.SetStylebox("button_pressed", ControlName, ButtonPressedStyle);
            owner.ThemeResource.SetStylebox("tab_bg", ControlName, TabStyleBg);
            owner.ThemeResource.SetStylebox("tab_disabled", ControlName, TabStyleDis);
            owner.ThemeResource.SetStylebox("tab_fg", ControlName, TabStyleFg);
        }
        protected void SetTextEdit()
        {
            string ControlName = nameof(TextEdit);
            // Text Edit
            owner.ThemeResource.SetColor("caret_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("selection_color", ControlName, HighlightColor);

            owner.ThemeResource.SetIcon("fold", ControlName, iconSet.GuiTreeArrowDown);
            owner.ThemeResource.SetIcon("folded", ControlName, iconSet.GuiTreeArrowRight);
            owner.ThemeResource.SetIcon("space", ControlName, iconSet.GuiSpace);
            owner.ThemeResource.SetIcon("tab", ControlName, iconSet.GuiTab);

            owner.ThemeResource.SetStylebox("focus", ControlName, TextEditFocus);
            owner.ThemeResource.SetStylebox("normal", ControlName, TextEditBackground);
            owner.ThemeResource.SetStylebox("read_only", ControlName, TextEditReadonly);
        }

        protected void SetToolButton()
        {
            string ControlName = nameof(ToolButton);
            // Tool Button
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_hover", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("font_color_pressed", ControlName, PressedFontColor);

            owner.ThemeResource.SetStylebox("disabled", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("focus", ControlName, ButtonFocusStyle);
            owner.ThemeResource.SetStylebox("hover", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("normal", ControlName, PanelTextBgBorderless);
            owner.ThemeResource.SetStylebox("pressed", ControlName, PanelTextBgBorderless);
        }
        protected void SetTooltipLabel()
        {
            string ControlName = "TooltipLabel";
            // Tooltip Label
            owner.ThemeResource.SetColor("font_color", ControlName, TooltipFontColor);
            owner.ThemeResource.SetColor("font_color_shadow", ControlName, TooltipFontShadowColor);
        }
        protected void SetTooltipPanel()
        {
            string ControlName = "TooltipPanel";
            // Tooltip Panel
            owner.ThemeResource.SetStylebox("panel", ControlName, ToolTipPanel);
        }
        protected void SetTree()
        {
            string ControlName = nameof(Tree);
            // Tree
            owner.ThemeResource.SetColor("custom_button_font_highlight", ControlName, HighlightedFontColor);
            owner.ThemeResource.SetColor("drop_position_color", ControlName, PressedFontColor);
            owner.ThemeResource.SetColor("font_color", ControlName, FontColor);
            owner.ThemeResource.SetColor("font_color_selected", ControlName, MonoColor);
            owner.ThemeResource.SetColor("title_button_color", ControlName, FontColor);

            owner.ThemeResource.SetColor("guide_color", ControlName, GuideColor);
            owner.ThemeResource.SetColor("relationship_line_color", ControlName, RelationshipLineColor);
            owner.ThemeResource.SetColor("children_hl_line_color", ControlName, ChildrenHlLineColor);
            owner.ThemeResource.SetColor("parent_hl_line_color", ControlName, ParentHlLineColor);

            owner.ThemeResource.SetIcon("arrow", ControlName, iconSet.GuiTreeArrowDown);
            owner.ThemeResource.SetIcon("arrow_collapsed", ControlName, iconSet.GuiTreeArrowRight);
            owner.ThemeResource.SetIcon("checked", ControlName, iconSet.GuiChecked);
            owner.ThemeResource.SetIcon("select_arrow", ControlName, iconSet.GuiTreeArrowDown);
            owner.ThemeResource.SetIcon("unchecked", ControlName, iconSet.GuiUnchecked);
            owner.ThemeResource.SetIcon("updown", ControlName, iconSet.GuiTreeArrowUpdown);

            owner.ThemeResource.SetStylebox("bg", ControlName, PanelBgBorderless);
            owner.ThemeResource.SetStylebox("bg_focus", ControlName, PanelFocusBg);
            owner.ThemeResource.SetStylebox("selected", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("selected_focus", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("button_pressed", ControlName, ButtonPressedStyle);
            owner.ThemeResource.SetStylebox("cursor", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("cursor_unfocused", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("custom_button", ControlName, new StyleBoxEmpty());
            owner.ThemeResource.SetStylebox("custom_button_hover", ControlName, ButtonHoverStyle);
            owner.ThemeResource.SetStylebox("custom_button_pressed", ControlName, new StyleBoxEmpty());
            owner.ThemeResource.SetStylebox("hover", ControlName, HighlightHoverStyle);
            owner.ThemeResource.SetStylebox("title_button_hover", ControlName, PanelBdBorderless);
            owner.ThemeResource.SetStylebox("title_button_normal", ControlName, PanelBdBorderless);
            owner.ThemeResource.SetStylebox("title_button_pressed", ControlName, PanelBdBorderless);
        }
        protected void SetVBoxContainer()
        {
            string ControlName = nameof(VBoxContainer);
        }
        protected void SetVSCrollBar()
        {
            string ControlName = nameof(VScrollBar);

            StyleBoxTexture grabber = new();
            grabber.Texture = iconSet.GuiScrollGrabber;
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
            grabberHl.Texture = iconSet.GuiScrollGrabberHl;
            grabberHl.MarginBottom = 5;
            grabberHl.MarginLeft = 5;
            grabberHl.MarginRight = 5;
            grabberHl.MarginTop = 5;

            StyleBoxTexture grabberPressed = grabber.Copy();
            grabberPressed.Texture = iconSet.GuiScrollGrabberPressed;

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

            owner.ThemeResource.SetStylebox("grabber", ControlName, grabber);
            owner.ThemeResource.SetStylebox("grabber_highlight", ControlName, grabberHl);
            owner.ThemeResource.SetStylebox("grabber_pressed", ControlName, grabberPressed);
            owner.ThemeResource.SetStylebox("scroll", ControlName, scroll);
            owner.ThemeResource.SetStylebox("scroll_focus", ControlName, scrollFocus);
        }
        protected void SetVSeparator()
        {
            string ControlName = nameof(VSeparator);
            // V Separator
            owner.ThemeResource.SetStylebox("separator", ControlName, SeparatorStyle);
        }
        protected void SetVSlider()
        {
            string ControlName = nameof(VSlider);
            // V Slider
            owner.ThemeResource.SetIcon("grabber", ControlName, iconSet.GuiSliderGrabber);
            owner.ThemeResource.SetIcon("grabber_highlight", ControlName, iconSet.GuiSliderGrabberHl);

            owner.ThemeResource.SetStylebox("grabber_area", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("grabber_area_highlight", ControlName, HighlightStyle);
            owner.ThemeResource.SetStylebox("slider", ControlName, PanelBdBorderless);
        }
        protected void SetVSplitContainer()
        {
            string ControlName = nameof(VSplitContainer);

            owner.ThemeResource.SetIcon("grabber", ControlName, iconSet.GuiVsplitter);

            owner.ThemeResource.SetStylebox("bg", ControlName, PanelBdBorderless);
        }
        protected void SetWindowDialog()
        {
            string ControlName = nameof(WindowDialog);
            // Window Dialog
            owner.ThemeResource.SetColor("title_color", ControlName, FontColor);

            owner.ThemeResource.SetIcon("close", ControlName, iconSet.GuiClose);
            owner.ThemeResource.SetIcon("close_highlight", ControlName, iconSet.GuiCloseCustomizable);

            owner.ThemeResource.SetStylebox("panel", ControlName, WindowDialogPanelStyle);
        }
    }
}