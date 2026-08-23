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
		// GD.Print("[CommandHandler] Change portrait to ", portraitName);
		if (characterPortrait != null) 
		{
			var characterPortraitTexture = GD.Load<Texture2D>("res://Assets/Art/Portraits/" + portraitName + ".png");
			if (characterPortraitTexture != null) 
			{
				characterPortrait.Texture = characterPortraitTexture;
			}
			else
			{
				GD.Print("[CommandHandler] Portrait texture not found");
			}
		}
		else
		{
			GD.Print("[CommandHandler] node CharacterPortrait not found");
		}
	}
	
	[YarnCommand("change_character")]
	public static void ChangeCharacter(string characterName)
	{
		// GD.Print("[CommandHandler] Change character to ", characterName);
		if (characterImage != null)
		{
			if (characterName == "0" || characterName == "none")
			{
				characterImage.Visible = false;
			}
			else
			{
				characterImage.Visible = true;
				var characterTexture = GD.Load<Texture2D>("res://Assets/Art/Characters/" + characterName + ".png");
				if (characterTexture != null)
				{
					characterImage.Texture = characterTexture;
				}
				else
				{
					GD.Print("[CommandHandler] Character texture not found");
					return;
				}
			}
		}
		else
		{
			GD.Print("[CommandHandler] node CharacterImage not found");
		}
	}

	[YarnCommand("change_background")]
	public static void ChangeBackground(string backgroundName)
	{
		// GD.Print("[CommandHandler] Change background to ", backgroundName);
		if (background != null)
		{
			var backgroundTexture = GD.Load<Texture2D>("res://Assets/Art/Backgrounds/" + backgroundName + ".png");
			if (backgroundTexture != null)
			{
				background.Texture = backgroundTexture;
			}
			else
			{
				backgroundTexture = GD.Load<Texture2D>("res://Assets/Art/Backgrounds/" + backgroundName + ".jpg");
				if (backgroundTexture != null)
					{
						background.Texture = backgroundTexture;
					}
				else
				{
					GD.Print("[CommandHandler] Background texture not found");
				}
			}
		}
		else
		{
			GD.Print("[CommandHandler] node Background not found");
		}
	}
}
