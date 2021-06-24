using Godot;
using System;
using System.Threading.Tasks;

namespace AutoThemes
{
    [Tool]
    public class AutoThemeSetter : Node, IAutoTheme
    {
        //Properties
        [Export]
        string AutoThemeGroupName = "AutoThemeNodes";

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

        internal FontIconSetting iconAndFontColor = FontIconSetting.Light;
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

        internal Color baseColor = new("3D3D3D");
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

        internal Color accentColor = new("b8e3ff");
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

        internal float contrast = 0.2f;
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

        internal float iconSaturation = 1.0f;
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

        internal float relationshipLineOpacity = 0.1f;
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


        internal int borderSize = 0;
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

        internal int cornerRadius = 3;
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

        internal float additionalSpacing = 0.0f;
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

        internal int focusBorderSize = 1;
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

        protected AutoThemeIcons iconSetLight = new();
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

        protected Theme themeResource = new();
        [Export]
        public Theme ThemeResource
        {
            get => themeResource;
            set => themeResource = value;
        }

        protected AutoThemeData themeData;
        public AutoThemeData ThemeData
        {
            get => themeData;
        }

        Control parentControl;

        public AutoThemeSetter()
        {
            themeData = new(this);
        }

        public override void _Ready()
        {
            Node parent = GetParent();
            if (parent is not null && parent is Control parentControl)
            {
                this.parentControl = parentControl;
                parent.Connect("ready", this, nameof(OnParentReady));
            }
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            var AutoThemeNodes = GetTree().GetNodesInGroup(AutoThemeGroupName);
            foreach (Node node in AutoThemeNodes)
            {
                if (node is IAutoThemeUser autoNode)
                {
                    if (ReferenceEquals(autoNode.ThemeSource, this)) ThemeResource.Disconnect("changed", node, nameof(autoNode._OnAutoThemeChanged));
                }
            }
            if (parentControl is not null)
            {
                parentControl.Disconnect("ready", this, nameof(OnParentReady));
            }
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }

        public void OnParentReady()
        {
            parentControl.Theme = ThemeResource;
            var AutoThemeNodes = GetTree().GetNodesInGroup(AutoThemeGroupName);
            foreach (Node node in AutoThemeNodes)
            {
                if (node is Control ctrlNode && node is IAutoThemeUser autoNode)
                {
                    if (ReferenceEquals(autoNode.ThemeSource, this)) continue;
                    autoNode.ThemeSource = this;
                    ctrlNode.Theme = ThemeResource;
                    ThemeResource.Connect("changed", node, nameof(autoNode._OnAutoThemeChanged));
                    autoNode._OnAutoThemeChanged();
                }
            }
        }

        public void Refresh()
        {
            themeData.ApplyToTheme();
            ThemeResource.EmitChanged();
            PropertyListChangedNotify();
        }

        protected Task? refreshHandle;
        public async Task RefreshAsync()
        {
            if (refreshHandle is null)
            {
                refreshHandle = Task.Run(() => themeData.ApplyToTheme());
                await refreshHandle;
                ThemeResource.EmitChanged();
                PropertyListChangedNotify();
            }
            else
            {
                await refreshHandle.ContinueWith((Task prev) =>
                {
                    themeData.ApplyToTheme();
                });
                ThemeResource.EmitChanged();
                PropertyListChangedNotify();
            }
        }

    }
}
