using Godot;
using System;

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

    [Export] // Default is #cfcfcf
    Color PrimaryFontColor = new(0, 0, 0, 1);
    [Export] // Default is #4dffffff
    Color DisabledFontColor = new(0, 0, 0, 1);
    [Export] // Default is #e2e2e2
    Color HoverFontColor = new(0, 0, 0, 1);
    [Export] // Default is #b8e3ff
    Color PressedFontColor = new(0, 0, 0, 1);
    [Export] // Default is 293,293,293,255 (16 bit color?)
    Color HoverIconColor = new(0, 0, 0, 1);
    [Export] // Default is 211, 261, 293, 255
    Color PressedIconColor = new(0, 0, 0, 1);
    [Export] // Default is 303030
    Color TooltipFontColor = new(0, 0, 0, 1);
    [Export] // Default is 1a000000
    Color TooltipFontShadowColor = new(0, 0, 0, 1);
    [Export] // Default is cdebff
    Color FolderIconMod = new(0, 0, 0, 1);
    [Export] // Default is ffffff
    Color SelectFontColor = new(0, 0, 0, 1);

    [Export] // Default color is #313131
    StyleBox BackgroundStyle = new StyleBoxEmpty();
    [Export] // Default color is #373737
    StyleBox MidgroundStyle = new StyleBoxEmpty();
    [Export] // Default color is #3D3D3D
    StyleBox ForegroundStyle = new StyleBoxEmpty();
    [Export] // Default color is #1affffff (first byte is alpha)
    StyleBox SeparatorStyle = new StyleBoxLine();
    [Export] // Default color is #6e6e6e
    StyleBox HighlightStyle = new StyleBoxEmpty();
    [Export] // Default color is #252525
    StyleBox HighlightContrastStyle = new StyleBoxEmpty();
    [Export] // Default color is #33ffffff
    StyleBox ItemSelectedStyle = new StyleBoxEmpty();
    [Export] // Default color is #e6ffffff
    StyleBox ToolTipStyle = new StyleBoxEmpty();



    [Export] // Default Midground
    StyleBox ButtonDisabledStyle = new StyleBoxEmpty();
    [Export] // Rest of buttons are background
    StyleBox ButtonFocusStyle = new StyleBoxEmpty();
    [Export]
    StyleBox ButtonHoverStyle = new StyleBoxEmpty();
    [Export]
    StyleBox ButtonNormalStyle = new StyleBoxEmpty();
    [Export]
    StyleBox ButtonPressedStyle = new StyleBoxEmpty();


    public AutoTheme()
    {
        // Not sure when this gets called
        // On every file load? 
        // Are there virtual resource functions I can use
        // like OnSave and OnLoad? 
    }

    // There is the Changed() signal, which I can call and use
    // To update on changes

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

        // Box Container
        // --
        // Button
        SetColor("font_color", "Button", PrimaryFontColor);
        SetColor("font_color_disabled", "Button", DisabledFontColor);
        SetColor("font_color_hover", "Button", HoverFontColor);
        SetColor("font_color_pressed", "Button", PressedFontColor);
        // SetColor("icon_color_hover", "Button", HoverIconColor);
        // SetColor("icon_color_pressed", "Button", PressedIconColor);
        SetStylebox("disabled", "Button", MidgroundStyle);
        SetStylebox("focus", "Button", BackgroundStyle);
        SetStylebox("hover", "Button", BackgroundStyle);
        SetStylebox("normal", "Button", BackgroundStyle);
        SetStylebox("pressed", "Button", BackgroundStyle);

        // Check Box
        SetColor("font_color", "CheckBox", PrimaryFontColor);
        SetColor("font_color_disabled", "CheckBox", DisabledFontColor);
        SetColor("font_color_hover", "CheckBox", HoverFontColor);
        SetColor("font_color_pressed", "CheckBox", PressedFontColor);
        // SetColor("icon_color_hover", "CheckBox", HoverIconColor);
        // Icons need to be manual? 
        SetStylebox("disabled", "CheckBox", BackgroundStyle);
        SetStylebox("hover", "CheckBox", BackgroundStyle);
        SetStylebox("normal", "CheckBox", BackgroundStyle);
        SetStylebox("pressed", "CheckBox", BackgroundStyle);

        // Check Button
        SetColor("font_color", "CheckButton", PrimaryFontColor);
        SetColor("font_color_disabled", "CheckButton", DisabledFontColor);
        SetColor("font_color_hover", "CheckButton", HoverFontColor);
        SetColor("font_color_pressed", "CheckButton", PressedFontColor);
        // SetColor("icon_color_hover", "CheckButton", HoverIconColor);
        // Icons need to be manual? 
        SetStylebox("disabled", "CheckButton", BackgroundStyle);
        SetStylebox("hover", "CheckButton", BackgroundStyle);
        SetStylebox("normal", "CheckButton", BackgroundStyle);
        SetStylebox("pressed", "CheckButton", BackgroundStyle);

        // Color Picker
        // --
        // Color Picker Button
        // --
        // Editor - The editor specific stuff is probably not needed.
        // --
        // Editor About
        // --
        // Editor Fonts
        // --
        // Editor Help
        // --
        // Editor Icons
        // --
        // Editor Settings Dialog
        // --
        // Editor Styles
        // --
        // File Dialog
        SetColor("files_disabled", "FileDialog", DisabledFontColor);
        SetColor("folder_icon_mod", "FileDialog", FolderIconMod);

        // Graph Edit
        // Leaving the colors for now
        SetStylebox("bg", "GraphEdit", BackgroundStyle);

        // Graph Node
        // This one is complicated, manually edit as needed for now

        // Grid Container
        // --
        // H Box Container
        // --
        // H Scroll Bar
        // Uses textues for icons, revisist later

        // H Separator
        SetStylebox("separator", "HSeparator", SeparatorStyle);

        // H Slider
        // Also has icons
        SetStylebox("grabber_area", "HSlider", HighlightStyle);
        SetStylebox("grabber_area_highlight", "HSlider", HighlightStyle);
        SetStylebox("slider", "HSlider", HighlightContrastStyle);

        // H Split Container
        // Uses textures also
        // Can probably use color modulation for controls like this

        // Item List
        SetColor("font_color", "ItemList", PrimaryFontColor);
        SetColor("font_color_select", "ItemList", SelectFontColor);
        //SetColor("guide_color", "ItemList", --);
        SetStylebox("bg", "ItemList", BackgroundStyle);
        SetStylebox("bg_focus", "ItemList", ForegroundStyle);
        SetStylebox("cursor", "ItemList", ForegroundStyle);
        SetStylebox("cursor_unfocused", "ItemList", ForegroundStyle);
        SetStylebox("selected", "ItemList", ItemSelectedStyle);
        SetStylebox("selected_focus", "ItemList", ItemSelectedStyle);

        // Label
        SetColor("font_color", "Label", PrimaryFontColor);
        //SetColor("font_color_shadow", "Label", PrimaryFontColor);
        //SetStyleBox("normal", "Label", <emptystylebox>);

        // Line Edit
        SetColor("clear_button_color", "LineEdit", PrimaryFontColor);
        SetColor("clear_button_color_pressed", "LineEdit", PressedFontColor);
        SetColor("cursor_color", "LineEdit", PrimaryFontColor);
        SetColor("font_color", "LineEdit", PrimaryFontColor);
        SetColor("font_color_select", "LineEdit", SelectFontColor);
        SetColor("read_only", "LineEdit", DisabledFontColor);
        SetColor("selection_color", "LineEdit", new Color("66b8e3ff"));
        SetColor("font_color_uneditable", "LineEdit", DisabledFontColor);
        SetStylebox("focus", "LineEdit", BackgroundStyle);
        SetStylebox("normal", "LineEdit", BackgroundStyle);
        SetStylebox("read_only", "LineEdit", MidgroundStyle);

        // Link Button
        SetColor("font_color", "LinkButton", PrimaryFontColor);
        SetColor("font_color_disabled", "LinkButton", DisabledFontColor);
        SetColor("font_color_hover", "LinkButton", HoverFontColor);
        SetColor("font_color_pressed", "LinkButton", PressedFontColor);

        // Margin Container
        // --

        // Menu Button
        SetColor("font_color", "MenuButton", PrimaryFontColor);
        SetColor("font_color_hover", "MenuButton", HoverFontColor);
        SetStylebox("disabled", "MenuButton", BackgroundStyle);
        SetStylebox("focus", "MenuButton", BackgroundStyle);
        SetStylebox("hover", "MenuButton", BackgroundStyle);
        SetStylebox("normal", "MenuButton", BackgroundStyle);
        SetStylebox("pressed", "MenuButton", BackgroundStyle);

        // Option Button
        SetColor("font_color", "OptionButton", PrimaryFontColor);
        SetColor("font_color_disabled", "OptionButton", DisabledFontColor);
        SetColor("font_color_hover", "OptionButton", HoverFontColor);
        SetColor("font_color_pressed", "OptionButton", PressedFontColor);
        SetStylebox("disabled", "MenuButton", MidgroundStyle);
        SetStylebox("focus", "MenuButton", BackgroundStyle);
        SetStylebox("hover", "MenuButton", BackgroundStyle);
        SetStylebox("normal", "MenuButton", BackgroundStyle);
        SetStylebox("pressed", "MenuButton", BackgroundStyle);

        // Panel
        StyleBoxFlat BorderlessBackgroundStyle = BackgroundStyle.Duplicate() as StyleBoxFlat ?? new StyleBoxFlat();
        BorderlessBackgroundStyle.Set("border_width_left", 0);
        BorderlessBackgroundStyle.Set("border_width_top", 0);
        BorderlessBackgroundStyle.Set("border_width_right", 0);
        BorderlessBackgroundStyle.Set("border_width_bottom", 0);
        SetStylebox("panel", "Panel", BorderlessBackgroundStyle);

        // Panel Container
        SetStylebox("panel", "PanelContainer", BorderlessBackgroundStyle);

        // Popup Dialog
        SetStylebox("panel", "PopupDialog", ForegroundStyle);

        // Popup Menu
        SetColor("font_color", "PopupMenu", PrimaryFontColor);
        SetColor("font_color_disabled", "PopupMenu", DisabledFontColor);
        SetColor("font_color_hover", "PopupMenu", HoverFontColor);
        SetColor("font_color_accel", "PopupMenu", DisabledFontColor);
        SetStylebox("disabled", "PopupMenu", BorderlessBackgroundStyle);
        SetStylebox("focus", "PopupMenu", BorderlessBackgroundStyle);
        SetStylebox("hover", "PopupMenu", BorderlessBackgroundStyle);
        SetStylebox("labeled_separator_left", "PopupMenu", SeparatorStyle);
        SetStylebox("labeled_separator_right", "PopupMenu", SeparatorStyle);
        SetStylebox("normal", "PopupMenu", BorderlessBackgroundStyle);
        SetStylebox("panel", "PopupMenu", ForegroundStyle);
        SetStylebox("pressed", "PopupMenu", BorderlessBackgroundStyle);
        SetStylebox("separator", "PopupMenu", SeparatorStyle);

        // Popup Panel
        SetStylebox("panel", "PopupPanel", ForegroundStyle);

        // Progress Bar
        SetColor("font_color", "ProgressBar", PrimaryFontColor);

        // Project Settings Editor
        // Passing for now

        // Rich Text Label
        SetColor("default_color", "RichTextLabel", PrimaryFontColor);
        SetStylebox("normal", "RichTextLabel", BackgroundStyle);

        // Spin Box
        // Only has icon

        // Tab Container
        SetColor("font_color_bg", "TabContainer", DisabledFontColor);
        SetColor("font_color_fg", "TabContainer", PrimaryFontColor);
        SetStylebox("panel", "TabContainer", ForegroundStyle);

        // All the bottom borders need to be zero
        StyleBoxFlat TabStyleBg = (BackgroundStyle.Duplicate() as StyleBoxFlat) ?? new StyleBoxFlat();
        TabifyStyle(ref TabStyleBg);
        SetStylebox("tab_bg", "TabContainer", TabStyleBg);

        StyleBoxFlat TabStyleDis = (MidgroundStyle.Duplicate() as StyleBoxFlat) ?? new StyleBoxFlat();
        TabifyStyle(ref TabStyleDis);
        SetStylebox("tab_disabled", "TabContainer", TabStyleDis);

        StyleBoxFlat TabStyleFg = (ForegroundStyle.Duplicate() as StyleBoxFlat) ?? new StyleBoxFlat();
        TabifyStyle(ref TabStyleFg);
        SetStylebox("tab_fg", "TabContainer", TabStyleFg);

        void TabifyStyle(ref StyleBoxFlat target)
        {
            target.Set("border_width_bottom", 0);
            target.Set("content_margin_left", 10);
            target.Set("content_margin_right", 10);
            target.Set("content_margin_top", 5);
            target.Set("content_margin_bottom", 5);
            target.Set("expand_margin_bottom", 1);
        }

        // Tabs
        SetColor("font_color_bg", "Tabs", DisabledFontColor);
        SetColor("font_color_fg", "Tabs", PrimaryFontColor);
        SetStylebox("button", "Tabs", BackgroundStyle);
        SetStylebox("button_pressed", "Tabs", BackgroundStyle);
        SetStylebox("tab_bg", "Tabs", TabStyleBg);
        SetStylebox("tab_disabled", "Tabs", TabStyleDis);
        SetStylebox("tab_fg", "Tabs", TabStyleFg);

        // Text Edit
        SetColor("caret_color", "TextEdit", PrimaryFontColor);
        SetColor("font_color", "TextEdit", PrimaryFontColor);
        //SetColor("selection_color", "TextEdit", PrimaryFontColor);
        SetStylebox("focus", "TextEdit", BackgroundStyle);
        SetStylebox("normal", "TextEdit", BackgroundStyle);
        SetStylebox("read_only", "TextEdit", MidgroundStyle);

        // Tool Button
        SetColor("font_color", "ToolButton", PrimaryFontColor);
        SetColor("font_color_hover", "ToolButton", HoverFontColor);
        SetColor("font_color_pressed", "ToolButton", PressedFontColor);
        SetStylebox("disabled", "ToolButton", BorderlessBackgroundStyle);
        SetStylebox("focus", "ToolButton", BorderlessBackgroundStyle);
        SetStylebox("hover", "ToolButton", BorderlessBackgroundStyle);
        SetStylebox("normal", "ToolButton", BorderlessBackgroundStyle);
        SetStylebox("pressed", "ToolButton", BorderlessBackgroundStyle);

        // Tooltip Label
        SetColor("font_color", "TooltipLabel", TooltipFontColor);
        SetColor("font_color_shadow", "TooltipLabel", TooltipFontShadowColor);

        // Tooltip Panel
        SetStylebox("panel", "TooltipPanel", ToolTipStyle);

        // Tree
        SetColor("custom_button_font_highlight", "Tree", HoverFontColor);
        SetColor("drop_position_color", "Tree", PressedFontColor);
        SetColor("font_color", "Tree", PrimaryFontColor);
        SetColor("font_color_selected", "Tree", SelectFontColor);
        Color lineColor = new Color(SelectFontColor);
        lineColor.a = 0.05f;
        SetColor("guide_color", "Tree", lineColor);
        lineColor = new Color(SelectFontColor);
        lineColor.a = 0.1f;
        SetColor("relationship_line_color", "Tree", lineColor);
        SetColor("title_button_color", "Tree", PrimaryFontColor);
        SetStylebox("bg", "Tree", BackgroundStyle);
        SetStylebox("bg_focus", "Tree", ForegroundStyle);
        SetStylebox("button_pressed", "Tree", HighlightStyle);
        SetStylebox("cursor", "Tree", ForegroundStyle);
        SetStylebox("cursor_unfocused", "Tree", ForegroundStyle);
        SetStylebox("custom_button", "Tree", new StyleBoxEmpty());
        SetStylebox("custom_button_hover", "Tree", BackgroundStyle);
        SetStylebox("custom_button_pressed", "Tree", new StyleBoxEmpty());
        //SetStylebox("hover", "Tree", ); // Doesn't have a standard stylebox
        //SetStylebox("selected", "Tree", ); // Also non-standard stylebox
        // SetStylebox("selected_focus", "Tree", ); // non-standard stylebox
        SetStylebox("title_button_hover", "Tree", HighlightContrastStyle);
        SetStylebox("title_button_normal", "Tree", HighlightContrastStyle);
        SetStylebox("title_button_pressed", "Tree", HighlightContrastStyle);

        // V Box Container
        // --
        // V SCroll Bar
        // --
        // V Separator
        SetStylebox("separator", "VSeparator", SeparatorStyle);

        // V Slider
        SetStylebox("grabber_area", "VSlider", HighlightStyle);
        SetStylebox("grabber_area_highlight", "VSlider", HighlightStyle);
        SetStylebox("slider", "VSlider", HighlightContrastStyle);

        // V Split Container
        // --
        // Window Dialog
        SetColor("title_color", "WindowDialog", PrimaryFontColor);
        StyleBoxFlat WindowDialogPanelStyle = (ForegroundStyle.Duplicate() as StyleBoxFlat) ?? new StyleBoxFlat();
        WindowDialogPanelStyle.Set("border_width_top", 24);
        WindowDialogPanelStyle.Set("expand_margin_top", 24);
        WindowDialogPanelStyle.Set("shadow_size", 4);
        WindowDialogPanelStyle.Set("content_margin_left", 8);
        WindowDialogPanelStyle.Set("content_margin_right", 8);
        WindowDialogPanelStyle.Set("content_margin_top", 8);
        WindowDialogPanelStyle.Set("content_margin_bottom", 8);
        SetStylebox("panel", "WindowDialog", WindowDialogPanelStyle);

        // Place custom after the fact...
    }

    public StyleBoxFlat CopyStyleBoxFlat(StyleBoxFlat source)
    {
        StyleBoxFlat target = new();
        target.Set("bg_color", source.Get("bg_color"));
        target.Set("draw_center", source.Get("draw_center"));
        target.Set("corner_detail", source.Get("corner_detail"));
        target.Set("border_width_left", source.Get("border_width_left"));
        target.Set("border_width_top", source.Get("border_width_top"));
        target.Set("border_width_right", source.Get("border_width_right"));
        target.Set("border_width_bottom", source.Get("border_width_bottom"));
        target.Set("border_color", source.Get("border_color"));
        target.Set("border_blend", source.Get("border_blend"));
        target.Set("corner_radius_top_left", source.Get("corner_radius_top_left"));
        target.Set("corner_radius_top_right", source.Get("corner_radius_top_right"));
        target.Set("corner_radius_bottom_right", ForegroundStyle.Get("corner_radius_bottom_right"));
        target.Set("corner_radius_bottom_left", source.Get("corner_radius_bottom_left"));
        target.Set("expand_margin_left", source.Get("expand_margin_left"));
        target.Set("expand_margin_right", source.Get("expand_margin_right"));
        target.Set("expand_margin_top", source.Get("expand_margin_top"));
        target.Set("expand_margin_bottom", source.Get("expand_margin_bottom"));
        target.Set("shadow_color", source.Get("shadow_color"));
        target.Set("shadow_size", source.Get("shadow_size"));
        target.Set("shadow_offset", source.Get("shadow_offset"));
        target.Set("anti_aliasing", source.Get("anti_aliasing"));
        target.Set("anti_aliasing_size", source.Get("anti_aliasing_size"));
        target.Set("content_margin_left", source.Get("content_margin_left"));
        target.Set("content_margin_right", source.Get("content_margin_right"));
        target.Set("content_margin_top", source.Get("content_margin_top"));
        target.Set("content_margin_bottom", source.Get("content_margin_bottom"));

        return target;
    }
}
