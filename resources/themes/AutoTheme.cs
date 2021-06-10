using Godot;
using System;
using Godot.Utilities.Theme;

[Tool]
public class AutoTheme : Theme
{
    // Properties
    bool refresh = false;
    [Export]
    bool Refresh
    {
        get => refresh;
        set
        {
            refresh = value;
            if (refresh == true) ApplyChanges();
        }
    }

    enum FontIconSetting { Dark, Light }
    FontIconSetting iconAndFontColor = FontIconSetting.Light;
    [Export]
    FontIconSetting IconAndFontColor
    {
        get => iconAndFontColor;
        set
        {
            iconAndFontColor = value;
            switch (iconAndFontColor)
            {
                case FontIconSetting.Light:
                    SelectedFontColor = LightFontColor;
                    break;
                case FontIconSetting.Dark:
                    SelectedFontColor = DarkFontColor;
                    break;
            }
        }
    }
    Color DarkFontColor = new("383838");
    Color LightFontColor = new("cdcfd2");
    Color SelectedFontColor;

    [Export]
    Color BaseColor = new("3D3D3D");
    [Export]
    Color AccentColor = new("b8e3ff");
    [Export(PropertyHint.Range, "-1, 1, 0.01")]
    float Contrast = 0.2f;
    [Export(PropertyHint.Range, "0, 2, 0.01")]
    float IconSaturation = 1.0f;
    [Export(PropertyHint.Range, "0, 1, 0.01")]
    float RelationshipLineOpacity = 0.1f;
    [Export(PropertyHint.Range, "0, 2, 1")]
    int BorderSize = 0;
    [Export(PropertyHint.Range, "0, 6, 1")]
    int CornerRadius = 3;
    [Export(PropertyHint.Range, "0, 5, 0.1")]
    float AdditionalSpacing = 0.0f;
    [Export]
    int TabContentMargin = 3;

    // Derived colors
    Color SecondaryColor;
    Color TertiaryColor;
    Color HoverColor;
    Color BorderColor;
    Color HighlightColor;
    Color HighlightColorContrast;
    Color TooltipPanelColor = new("e6ffffff");

    // Font Colors
    Color DisabledFontColor = new("#4dffffff"); // Default is #4dffffff
    Color HoverFontColor = new("e2e2e2");// Default is #e2e2e2
    Color PressedFontColor = new("b8e3ff");// Default is #b8e3ff
    Color SelectFontColor = new("ffffff");
    Color TooltipFontColor = new("303030");
    Color TooltipFontShadowColor = new("1a000000");

    // Icon Colors
    Color HoverIconColor = new(0, 0, 0, 1); // Default is 293,293,293,255 (16 bit color?)
    Color PressedIconColor = new(0, 0, 0, 1);// Default is 211, 261, 293, 255
    Color FolderIconMod = new("cdebff");

    // Theme styleboxes
    StyleBoxFlat BackgroundStyle;// Default color is #313131
    StyleBoxFlat MidgroundStyle; // Default color is #373737
    StyleBoxFlat ForegroundStyle;// Default color is #3D3D3D
    StyleBoxFlat BackgroundStyleBorderless;
    StyleBoxFlat MidgroundStyleBorderless;
    StyleBoxFlat ForegroundStyleBorderless;
    StyleBoxFlat FocusBackground;
    StyleBoxFlat FocusMidground;
    StyleBoxFlat FocusForeground;
    StyleBoxLine SeparatorStyle;// Default color is #1affffff (first byte is alpha)
    StyleBoxFlat HighlightStyle;// Default color is #6e6e6e
    StyleBoxFlat HighlightContrastStyle;// Default color is #252525
    StyleBoxFlat ItemSelectedStyle;// Default color is #33ffffff
    StyleBoxFlat ToolTipStyle;// Default color is #e6ffffff

    StyleBoxFlat ButtonDisabledStyle;// Default Midground
    // Rest of buttons are background
    StyleBoxFlat ButtonFocusStyle;
    StyleBoxFlat ButtonHoverStyle;
    StyleBoxFlat ButtonNormalStyle;
    StyleBoxFlat ButtonPressedStyle;

    StyleBoxFlat TabStylePanel;
    StyleBoxFlat TabStyleBg;
    StyleBoxFlat TabStyleDis;
    StyleBoxFlat TabStyleFg;
    StyleBoxFlat WindowDialogPanelStyle;

    public AutoTheme()
    {
        // Not sure when this gets called
        // On every file load? 
        // Are there virtual resource functions I can use
        // like OnSave and OnLoad? 
        IconAndFontColor = IconAndFontColor;
    }

    // There is the Changed() signal, which I can call and use
    // To update on changes

    protected void GenerateColors()
    {
        // Create derived colors based on contrast settings
        if (Contrast == 0)
        {
            SecondaryColor = BaseColor;
            TertiaryColor = BaseColor;
            HoverColor = BaseColor.Lightened(0.1f);
            BorderColor = TertiaryColor.Darkened(0.1f);
            HighlightColor = BaseColor.Lightened(0.2f);
            HighlightColorContrast = BaseColor.Darkened(0.2f);
        }
        else if (Contrast > 0)
        {
            SecondaryColor = BaseColor.Darkened(Contrast / 2);
            TertiaryColor = BaseColor.Darkened(Contrast);
            HoverColor = BaseColor.Lightened(0.1f);
            BorderColor = TertiaryColor.Darkened(0.1f);
            HighlightColor = BaseColor.Lightened(0.2f);
            HighlightColorContrast = BaseColor.Darkened(0.2f);
        }
        else if (Contrast < 0)
        {
            float absContrast = Godot.Mathf.Abs(Contrast);
            SecondaryColor = BaseColor.Lightened(absContrast / 2);
            TertiaryColor = BaseColor.Lightened(absContrast);
            BorderColor = BaseColor.Darkened(0.1f);
            HighlightColor = BaseColor.Darkened(0.2f);
            HighlightColorContrast = BaseColor.Lightened(0.2f);
        }
        HoverColor.s -= 0.1f;
        HighlightColor.a = 0.2f;
        HighlightColor.s -= 0.3f;
        HighlightColorContrast.s -= 0.1f;

        // Make sure derived font colors follow global setting
        if (IconAndFontColor == FontIconSetting.Light)
        {
            DisabledFontColor = new Color(1, 1, 1, 0.3f);
            HoverFontColor = new Color(SelectedFontColor);
            HoverFontColor.Lightened(0.1f);
            SelectFontColor = new Color(1, 1, 1);
            // Tooltip should oppose whatever the main color level is
            TooltipFontColor = DarkFontColor;
            TooltipPanelColor = HighlightColorContrast.Contrasted();
        }
        else if (IconAndFontColor == FontIconSetting.Dark)
        {
            DisabledFontColor = new Color(0, 0, 0, 0.3f);
            HoverFontColor = new Color(SelectedFontColor);
            HoverFontColor.Darkened(0.1f);
            SelectFontColor = new Color(0, 0, 0);
            // Tooltip should oppose whatever the main color level is
            TooltipFontColor = LightFontColor;
            TooltipPanelColor = HighlightColorContrast.Contrasted();
        }

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

    }

    public StyleBoxFlat StyleBoxFlatBuilder(Color bgColor)
    {
        StyleBoxFlat newStyle = new();
        newStyle.BgColor = bgColor;
        newStyle.DrawCenter = true;
        newStyle.CornerDetail = 8;
        newStyle.SetBorderWidthAll(BorderSize);
        newStyle.BorderColor = BorderColor;
        newStyle.BorderBlend = false;
        newStyle.SetCornerRadiusAll(CornerRadius);
        newStyle.SetExpandMarginAll(AdditionalSpacing);
        newStyle.ShadowColor = new Color(0, 0, 0, 0);
        newStyle.ShadowSize = 0;
        newStyle.ShadowOffset = new Vector2(0, 0);
        newStyle.AntiAliasing = true;
        newStyle.AntiAliasingSize = 1;
        newStyle.SetContentMarginAll(AdditionalSpacing);

        return newStyle;
    }

    public StyleBoxFlat StyleBoxFlatBuilder(Color bgColor, Color BorderColor, int BorderSize)
    {
        StyleBoxFlat newStyle = new();
        newStyle.BgColor = bgColor;
        newStyle.DrawCenter = true;
        newStyle.CornerDetail = 8;
        newStyle.SetBorderWidthAll(BorderSize);
        newStyle.BorderColor = BorderColor;
        newStyle.BorderBlend = false;
        newStyle.SetCornerRadiusAll(CornerRadius);
        newStyle.SetExpandMarginAll(AdditionalSpacing);
        newStyle.ShadowColor = new Color(0, 0, 0, 0);
        newStyle.ShadowSize = 0;
        newStyle.ShadowOffset = new Vector2(0, 0);
        newStyle.AntiAliasing = true;
        newStyle.AntiAliasingSize = 1;
        newStyle.SetContentMarginAll(AdditionalSpacing);

        return newStyle;
    }

    protected void GenerateStyles()
    {
        if (Contrast >= 0)
        {
            ForegroundStyle = StyleBoxFlatBuilder(BaseColor);
            MidgroundStyle = StyleBoxFlatBuilder(SecondaryColor);
            BackgroundStyle = StyleBoxFlatBuilder(TertiaryColor);
        }
        else
        {
            ForegroundStyle = StyleBoxFlatBuilder(TertiaryColor);
            MidgroundStyle = StyleBoxFlatBuilder(SecondaryColor);
            BackgroundStyle = StyleBoxFlatBuilder(BaseColor);
        }

        BackgroundStyleBorderless = (BackgroundStyle.Duplicate() as StyleBoxFlat)!;
        BackgroundStyleBorderless.SetBorderWidthAll(0);
        MidgroundStyleBorderless = (MidgroundStyle.Duplicate() as StyleBoxFlat)!;
        MidgroundStyleBorderless.SetBorderWidthAll(0);
        ForegroundStyleBorderless = (ForegroundStyle.Duplicate() as StyleBoxFlat)!;
        ForegroundStyleBorderless.SetBorderWidthAll(0);

        FocusBackground = (BackgroundStyle.Duplicate() as StyleBoxFlat)!;
        FocusBackground.SetBorderWidthAll(2);
        FocusBackground.BorderColor = AccentColor;
        FocusMidground = (MidgroundStyle.Duplicate() as StyleBoxFlat)!;
        FocusMidground.SetBorderWidthAll(2);
        FocusMidground.BorderColor = AccentColor;
        FocusForeground = (ForegroundStyle.Duplicate() as StyleBoxFlat)!;
        FocusForeground.SetBorderWidthAll(2);
        FocusForeground.BorderColor = AccentColor;

        SeparatorStyle = new StyleBoxLine();
        SeparatorStyle.Color = DisabledFontColor;

        HighlightStyle = (ForegroundStyle.Duplicate() as StyleBoxFlat)!;
        HighlightStyle.BgColor = HighlightColor;
        HighlightContrastStyle = (BackgroundStyle.Duplicate() as StyleBoxFlat)!;
        HighlightContrastStyle.BgColor = HighlightColorContrast;

        ItemSelectedStyle = (BackgroundStyleBorderless.Duplicate() as StyleBoxFlat)!;
        Color ItemSelectedColor = new Color(SelectedFontColor);
        ItemSelectedStyle.BgColor = ItemSelectedColor;
        ToolTipStyle = StyleBoxFlatBuilder(TooltipPanelColor);

        ButtonDisabledStyle = ForegroundStyle;
        ButtonFocusStyle = FocusMidground;
        ButtonHoverStyle = StyleBoxFlatBuilder(HoverColor);
        ButtonNormalStyle = MidgroundStyle;
        ButtonPressedStyle = FocusBackground;

        TabStylePanel = (ForegroundStyle.Duplicate() as StyleBoxFlat)!;
        TabStylePanel.SetContentMarginAll(TabContentMargin);

        TabStyleBg = (BackgroundStyle.Duplicate() as StyleBoxFlat)!;
        TabStyleDis = (MidgroundStyle.Duplicate() as StyleBoxFlat)!;
        TabStyleFg = (ForegroundStyle.Duplicate() as StyleBoxFlat)!;
        TabStyleBg.Tabify(AdditionalSpacing);
        TabStyleDis.Tabify(AdditionalSpacing);
        TabStyleFg.Tabify(AdditionalSpacing);

        WindowDialogPanelStyle = (ForegroundStyle.Duplicate() as StyleBoxFlat) ?? new StyleBoxFlat();
        WindowDialogPanelStyle.BorderWidthTop = 24;
        WindowDialogPanelStyle.ExpandMarginTop = 24;
        WindowDialogPanelStyle.ShadowSize = 4;
        WindowDialogPanelStyle.SetContentMarginAll(8);
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

        // Need to move the BorderlessBackgroundStyle
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

        // Place custom after the fact...
    }

    // protected void Set()
    // {
    //     string ControlName = "";
    // }

    protected void SetBoxContainer()
    {
        string ControlName = "BoxContainer";
    }

    protected void SetButton()
    {
        string ControlName = "Button";
        // Button
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_pressed", ControlName, PressedFontColor);
        // SetColor("icon_color_hover", ControlName, HoverIconColor);
        // SetColor("icon_color_pressed", ControlName, PressedIconColor);
        SetStylebox("disabled", ControlName, ButtonDisabledStyle);
        SetStylebox("focus", ControlName, ButtonFocusStyle);
        SetStylebox("hover", ControlName, ButtonHoverStyle);
        SetStylebox("normal", ControlName, ButtonNormalStyle);
        SetStylebox("pressed", ControlName, ButtonPressedStyle);
    }

    protected void SetCheckBox()
    {
        string ControlName = "CheckBox";
        // Check Box
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_pressed", ControlName, PressedFontColor);
        // SetColor("icon_color_hover", ControlName, HoverIconColor);
        // Icons need to be manual? 
        SetStylebox("disabled", ControlName, BackgroundStyle);
        SetStylebox("hover", ControlName, BackgroundStyle);
        SetStylebox("normal", ControlName, BackgroundStyle);
        SetStylebox("pressed", ControlName, BackgroundStyle);
    }

    protected void SetCheckButton()
    {
        string ControlName = "CheckButton";
        // Check Button
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_pressed", ControlName, PressedFontColor);
        // SetColor("icon_color_hover", ControlName, HoverIconColor);
        // Icons need to be manual? 
        SetStylebox("disabled", ControlName, BackgroundStyle);
        SetStylebox("hover", ControlName, BackgroundStyle);
        SetStylebox("normal", ControlName, BackgroundStyle);
        SetStylebox("pressed", ControlName, BackgroundStyle);
    }

    protected void SetColorPicker()
    {
        string ControlName = "ColorPicker";
    }

    protected void SetColorPickerButton()
    {
        string ControlName = "ColorPickerButton";
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
        string ControlName = "FileDialog";
        // File Dialog
        SetColor("files_disabled", ControlName, DisabledFontColor);
        SetColor("folder_icon_mod", ControlName, FolderIconMod);
    }

    protected void SetGraphEdit()
    {
        string ControlName = "GraphEdit";
        // Graph Edit
        // Leaving the colors for now
        SetStylebox("bg", ControlName, BackgroundStyle);
    }

    protected void SetGraphNode()
    {
        string ControlName = "GraphNode";
        // Graph Node
        // This one is complicated, manually edit as needed for now
    }
    protected void SetGridContainer()
    {
        string ControlName = "GridContainer";
    }
    protected void SetHBoxContainer()
    {
        string ControlName = "HBoxContainer";
    }
    protected void SetHScrollBar()
    {
        string ControlName = "HScrollBar";
        // H Scroll Bar
        // Uses textues for icons, revisist later
    }
    protected void SetHSeparator()
    {
        string ControlName = "HSeparator";
        // H Separator
        SetStylebox("separator", ControlName, SeparatorStyle);
    }
    protected void SetHSlider()
    {
        string ControlName = "HSlider";
        // H Slider
        // Also has icons
        SetStylebox("grabber_area", ControlName, HighlightStyle);
        SetStylebox("grabber_area_highlight", ControlName, HighlightStyle);
        SetStylebox("slider", ControlName, HighlightContrastStyle);
    }
    protected void SetHSplitContainer()
    {
        string ControlName = "HSplitContainer";
        // H Split Container
        // Uses textures also
        // Can probably use color modulation for controls like this
    }
    protected void SetItemList()
    {
        string ControlName = "ItemList";
        // Item List
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_select", ControlName, SelectFontColor);
        //SetColor("guide_color", ControlName, --);
        SetStylebox("bg", ControlName, BackgroundStyle);
        SetStylebox("bg_focus", ControlName, ForegroundStyle);
        SetStylebox("cursor", ControlName, ForegroundStyle);
        SetStylebox("cursor_unfocused", ControlName, ForegroundStyle);
        SetStylebox("selected", ControlName, ItemSelectedStyle);
        SetStylebox("selected_focus", ControlName, ItemSelectedStyle);
    }
    protected void SetLabel()
    {
        string ControlName = "Label";
        // Label
        SetColor("font_color", ControlName, SelectedFontColor);
        //SetColor("font_color_shadow", "Label", PrimaryFontColor);
        //SetStyleBox("normal", "Label", <emptystylebox>);
    }
    protected void SetLineEdit()
    {
        string ControlName = "LineEdit";
        // Line Edit
        SetColor("clear_button_color", ControlName, SelectedFontColor);
        SetColor("clear_button_color_pressed", ControlName, PressedFontColor);
        SetColor("cursor_color", ControlName, SelectedFontColor);
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_select", ControlName, SelectFontColor);
        SetColor("read_only", ControlName, DisabledFontColor);
        SetColor("selection_color", ControlName, new Color("66b8e3ff"));
        SetColor("font_color_uneditable", ControlName, DisabledFontColor);
        SetStylebox("focus", ControlName, BackgroundStyle);
        SetStylebox("normal", ControlName, BackgroundStyle);
        SetStylebox("read_only", ControlName, MidgroundStyle);
    }
    protected void SetLinkButton()
    {
        string ControlName = "LinkButton";
        // Link Button
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_pressed", ControlName, PressedFontColor);
    }
    protected void SetMarginContainer()
    {
        string ControlName = "MarginContainer";
    }
    protected void SetMenuButton()
    {
        string ControlName = "MenuButton";
        // Menu Button
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetStylebox("disabled", ControlName, BackgroundStyle);
        SetStylebox("focus", ControlName, BackgroundStyle);
        SetStylebox("hover", ControlName, BackgroundStyle);
        SetStylebox("normal", ControlName, BackgroundStyle);
        SetStylebox("pressed", ControlName, BackgroundStyle);
    }
    protected void SetOptionButton()
    {
        string ControlName = "OptionButton";
        // Option Button
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_pressed", ControlName, PressedFontColor);
        SetStylebox("disabled", ControlName, MidgroundStyle);
        SetStylebox("focus", ControlName, BackgroundStyle);
        SetStylebox("hover", ControlName, BackgroundStyle);
        SetStylebox("normal", ControlName, BackgroundStyle);
        SetStylebox("pressed", ControlName, BackgroundStyle);
    }
    protected void SetPanel()
    {
        string ControlName = "Panel";
        // Panel
        SetStylebox("panel", ControlName, BackgroundStyleBorderless);
    }
    protected void SetPanelContainer()
    {
        string ControlName = "PanelContainer";
        // Panel Container
        SetStylebox("panel", ControlName, BackgroundStyleBorderless);
    }
    protected void SetPopupDialog()
    {
        string ControlName = "PopupDialog";
        // Popup Dialog
        SetStylebox("panel", ControlName, ForegroundStyle);
    }
    protected void SetPopupMenu()
    {
        string ControlName = "PopupMenu";
        // Popup Menu
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_accel", ControlName, DisabledFontColor);
        SetStylebox("disabled", ControlName, BackgroundStyleBorderless);
        SetStylebox("focus", ControlName, BackgroundStyleBorderless);
        SetStylebox("hover", ControlName, BackgroundStyleBorderless);
        SetStylebox("labeled_separator_left", ControlName, SeparatorStyle);
        SetStylebox("labeled_separator_right", ControlName, SeparatorStyle);
        SetStylebox("normal", ControlName, BackgroundStyleBorderless);
        SetStylebox("panel", ControlName, ForegroundStyle);
        SetStylebox("pressed", ControlName, BackgroundStyleBorderless);
        SetStylebox("separator", ControlName, SeparatorStyle);
    }
    protected void SetPopupPanel()
    {
        string ControlName = "PopupPanel";
        // Popup Panel
        SetStylebox("panel", ControlName, ForegroundStyle);
    }
    protected void SetProgressBar()
    {
        string ControlName = "ProgressBar";
        // Progress Bar
        SetColor("font_color", ControlName, SelectedFontColor);
    }
    protected void SetProjectSettingsEditor()
    {
        string ControlName = "ProjectSettingsEditor";
        // Project Settings Editor
        // Passing for now
    }
    protected void SetRichTextLabel()
    {
        string ControlName = "RichTextLabel";
        // Rich Text Label
        SetColor("default_color", ControlName, SelectedFontColor);
        SetStylebox("normal", ControlName, BackgroundStyle);
    }
    protected void SetSpinBox()
    {
        string ControlName = "SpinBox";
        // Spin Box
        // Only has icon
    }
    protected void SetTabContainer()
    {
        string ControlName = "TabContainer";
        // Tab Container
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_bg", ControlName, DisabledFontColor);
        SetColor("font_color_fg", ControlName, SelectedFontColor);
        SetStylebox("panel", ControlName, TabStylePanel);
        // All the bottom borders need to be zero
        SetStylebox("tab_bg", ControlName, TabStyleBg);
        SetStylebox("tab_disabled", ControlName, TabStyleDis);
        SetStylebox("tab_fg", ControlName, TabStyleFg);

    }
    protected void SetTabs()
    {
        string ControlName = "Tabs";
        // Tabs
        SetColor("font_color_disabled", ControlName, DisabledFontColor);
        SetColor("font_color_bg", ControlName, DisabledFontColor);
        SetColor("font_color_fg", ControlName, SelectedFontColor);
        SetStylebox("button", ControlName, BackgroundStyle);
        SetStylebox("button_pressed", ControlName, BackgroundStyle);
        SetStylebox("tab_bg", ControlName, TabStyleBg);
        SetStylebox("tab_disabled", ControlName, TabStyleDis);
        SetStylebox("tab_fg", ControlName, TabStyleFg);
    }
    protected void SetTextEdit()
    {
        string ControlName = "TextEdit";
        // Text Edit
        SetColor("caret_color", ControlName, SelectedFontColor);
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("selection_color", ControlName, SecondaryColor.Contrasted());
        SetStylebox("focus", ControlName, BackgroundStyle);
        SetStylebox("normal", ControlName, BackgroundStyle);
        SetStylebox("read_only", ControlName, MidgroundStyle);
    }
    protected void SetToolButton()
    {
        string ControlName = "ToolButton";
        // Tool Button
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_hover", ControlName, HoverFontColor);
        SetColor("font_color_pressed", ControlName, PressedFontColor);
        SetStylebox("disabled", ControlName, BackgroundStyleBorderless);
        SetStylebox("focus", ControlName, BackgroundStyleBorderless);
        SetStylebox("hover", ControlName, BackgroundStyleBorderless);
        SetStylebox("normal", ControlName, BackgroundStyleBorderless);
        SetStylebox("pressed", ControlName, BackgroundStyleBorderless);
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
        SetStylebox("panel", ControlName, ToolTipStyle);
    }
    protected void SetTree()
    {
        string ControlName = "Tree";
        // Tree
        SetColor("custom_button_font_highlight", ControlName, HoverFontColor);
        SetColor("drop_position_color", ControlName, PressedFontColor);
        SetColor("font_color", ControlName, SelectedFontColor);
        SetColor("font_color_selected", ControlName, SelectedFontColor);
        Color lineColor = new Color(SelectFontColor);
        lineColor.a = 0.05f;
        SetColor("guide_color", ControlName, lineColor);
        lineColor = new Color(SelectFontColor);
        lineColor.a = 0.1f;
        SetColor("relationship_line_color", ControlName, lineColor);
        SetColor("title_button_color", ControlName, SelectedFontColor);
        SetStylebox("bg", ControlName, BackgroundStyle);
        SetStylebox("bg_focus", ControlName, FocusBackground);
        SetStylebox("selected", ControlName, HighlightStyle);
        SetStylebox("selected_focus", ControlName, HighlightStyle);
        SetStylebox("button_pressed", ControlName, ButtonPressedStyle);
        SetStylebox("cursor", ControlName, HighlightStyle);
        SetStylebox("cursor_unfocused", ControlName, HighlightStyle);
        SetStylebox("custom_button", ControlName, new StyleBoxEmpty());
        SetStylebox("custom_button_hover", ControlName, ButtonHoverStyle);
        SetStylebox("custom_button_pressed", ControlName, new StyleBoxEmpty());
        //SetStylebox("hover", ControlName, ); // Doesn't have a standard stylebox
        //SetStylebox("selected", ControlName, ); // Also non-standard stylebox
        // SetStylebox("selected_focus", ControlName, ); // non-standard stylebox
        SetStylebox("title_button_hover", ControlName, HighlightContrastStyle);
        SetStylebox("title_button_normal", ControlName, HighlightContrastStyle);
        SetStylebox("title_button_pressed", ControlName, HighlightContrastStyle);
    }
    protected void SetVBoxContainer()
    {
        string ControlName = "VBoxContainer";
    }
    protected void SetVSCrollBar()
    {
        string ControlName = "VSCrollBar";
    }
    protected void SetVSeparator()
    {
        string ControlName = "VSeparator";
        // V Separator
        SetStylebox("separator", ControlName, SeparatorStyle);
    }
    protected void SetVSlider()
    {
        string ControlName = "VSlider";
        // V Slider
        SetStylebox("grabber_area", ControlName, HighlightStyle);
        SetStylebox("grabber_area_highlight", ControlName, HighlightStyle);
        SetStylebox("slider", ControlName, HighlightContrastStyle);
    }
    protected void SetVSplitContainer()
    {
        string ControlName = "VSplitContainer";
    }
    protected void SetWindowDialog()
    {
        string ControlName = "WindowDialog";
        // Window Dialog
        SetColor("title_color", ControlName, SelectedFontColor);

        SetStylebox("panel", ControlName, WindowDialogPanelStyle);
    }
}

namespace Godot.Utilities.Theme
{
    public static class StyleExtensions
    {
        public static void SetContentMarginAll(this StyleBoxFlat style, float margin)
        {
            style.ContentMarginBottom = margin;
            style.ContentMarginLeft = margin;
            style.ContentMarginRight = margin;
            style.ContentMarginTop = margin;
        }

        public static void Tabify(this StyleBoxFlat style, float additionalSpacing = 0)
        {
            style.CornerRadiusBottomLeft = 0;
            style.CornerRadiusBottomRight = 0;
            style.BorderWidthBottom = 0;
            style.ContentMarginLeft = 10;
            style.ContentMarginRight = 10;
            style.ContentMarginTop = 5;
            style.ContentMarginBottom = 5;
            style.ExpandMarginBottom = 1 + additionalSpacing;
        }
    }
}
