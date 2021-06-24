using Godot;
using System;
using AutoThemes;


[Tool]
public class FSControls : HBoxContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    Button? BackButton;
    Button? ForwardButton;
    Button? UpButton;
    Button? RefreshButton;
    public override void _Ready()
    {
        BackButton = GetNode<Button>("./Back Button");
        ForwardButton = GetNode<Button>("./Forward Button");
        UpButton = GetNode<Button>("./Up Button");
        RefreshButton = GetNode<Button>("./RefreshButton");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void SetIcons(AutoThemeSetter themeSource)
    {
        BackButton.Icon = themeSource.ThemeData.IconSet.ArrowLeft;
        ForwardButton.Icon = themeSource.ThemeData.IconSet.ArrowRight;
        UpButton.Icon = themeSource.ThemeData.IconSet.ArrowUp;
        RefreshButton.Icon = themeSource.ThemeData.IconSet.Reload;
    }
}
