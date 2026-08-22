/*
Handles commands inside yarn files
USAGE: 
<<change_portrait NAME>> - changes character icon on the bottom left
<<change_image NAME>> - changes character image
<<change_background NAME>>
*/

using Godot;
using System;
using YarnSpinnerGodot;

public partial class CommandHandler : Node
{
	static TextureRect characterPortrait;
	static TextureRect characterImage;
	static TextureRect background;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		characterPortrait = GetNode<TextureRect>("../LinePresenter/PresenterControl/HBoxContainer/CharacterPortrait");
		characterImage = GetNode<TextureRect>("../CharacterImage");
		background = GetNode<TextureRect>("../Background");
	}

	[YarnCommand("change_portrait")]
	public static void ChangePortrait(string portraitName)
	{
		if (characterPortrait != null) 
		{
			var characterPortraitTexture = GD.Load<Texture2D>("res://Assets/Portraits/" + portraitName + ".png");
			characterPortrait.Texture = characterPortraitTexture;
		}
		else
		{
			GD.Print("[CommandHandler] node CharacterPortrait not found");
		}
	}
	
	[YarnCommand("change_image")]
	public static void ChangeImage(string imageName)
	{
		if (characterImage != null)
		{
			var characterImageTexture = GD.Load<Texture2D>("res://Assets/Portraits/" + imageName + ".png");
			characterImage.Texture = characterImageTexture;
		}
		else
		{
			GD.Print("[CommandHandler] node CharacterImage not found");
		}
	}

	[YarnCommand("change_background")]
	public static void ChangeBackground(string backgroundName)
	{
		if (background != null)
		{
			var backgroundTexture = GD.Load<Texture2D>("res://Assets/Portraits/" + backgroundName + ".png");
			background.Texture = backgroundTexture;
		}
		else
		{
			GD.Print("[CommandHandler] node Background not found");
		}
	}
}
