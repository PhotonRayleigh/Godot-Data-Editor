using Godot;
using System;
using AutoThemes;

[Tool]
public class testColor : Godot.ColorRect, IAutoThemeUser
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public AutoThemeSetter ThemeSource { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _OnAutoThemeChanged();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _OnAutoThemeChanged()
    {
        if (ThemeSource is not null)
        {
            Color = ThemeSource.ThemeData.BackdropColor;
        }
    }
}
