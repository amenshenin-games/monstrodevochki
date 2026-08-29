/*
	КОМАНДЫ.
	(везде, где есть 'name', можно вставить 'none', чтобы очистить)
	
	0. Ресет
	reset() - убирает изображения персонажей, убирает фон, убирает музыку.

	1. Изображения персонажей (или предмета. короче, картинка)
	add_image('name') - добавляет изображение в текст.
	change_image('name') - убирает все изображения, добавляет новое.

	2. Бэкграунд - независим от персонажа, находится под ним.
	change_background('name') - устанавливает фон.

	3. Воспроизведение звука
	play_sound('name') - воспроизводит единичный звук, независим от музыки
	play_music('name') - устанавливает фоновую музыку

	4. Прокачка
	upgrade_stat('stat') - производит улучшение характиристики
	get_stat('stat') - возвращает значение характеристики
	set_stat('stat', value) - устанавливает значениие характеристики (скорее для отладки)

	5. Добавляет в систему диалога пункт выбора.
	check('check_id', difficulty, 'stat', disadvantage, advantage)
	пример: <<check('door_lock', 10, 'strength', $arm_broken, false)>>
	

	6. пассивная проверка, возвращает true или false
	passive_check('check_id', difficulty, 'stat', disadvantage, advantage)

	Потенциально: добавить персонажа в список доступных для добавления в отряд, добавить в отряд и т.д. связанные с отрядом.
*/


using Godot;
using System;
using YarnSpinnerGodot;

public partial class CommandHandler : Node
{

	static Node signalBus;
	// private InMemoryVariableStorage variableStorage;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		signalBus = GetNode<Node>("/root/SignalBus");
		// variableStorage = GetNode<InMemoryVariableStorage>("../InMemoryVariableStorage");
	}	


// reset
	[YarnCommand("reset")]
	public static void Reset(string Name)
	{
		signalBus.EmitSignal("reset", Name);
	}

// visual
	[YarnCommand("add_image")]
	public static void AddImage(string Name)
	{
		signalBus.EmitSignal("add_image", Name);
	}

	[YarnCommand("change_image")]
	public static void ChangeImage(string Name)
	{
		signalBus.EmitSignal("change_image", Name);
	}

	[YarnCommand("change_background")]
	public static void ChangeBackground(string Name)
	{
		signalBus.EmitSignal("change_background", Name);
	}

// sound
	[YarnCommand("play_sound")]
	public static void PlaySound(string Name)
	{
		signalBus.EmitSignal("play_sound", Name);
	}

	[YarnCommand("play_music")]
	public static void PlayMusic(string Name)
	{
		signalBus.EmitSignal("play_music", Name);
	}

// stats
	[YarnCommand("upgrade_stat")]
	public static void UpgradeStat(string Stat)
	{
		signalBus.EmitSignal("upgrade_stat", Stat);
	}

	[YarnCommand("get_stat")]
	public static void GetStat(string Stat)
	{
		signalBus.EmitSignal("get_stat", Stat);
	}

	[YarnCommand("set_stat")]
	public static void SetStat(string Stat, int Value)
	{
		signalBus.EmitSignal("set_stat", Stat, Value);
	}

// check
	[YarnCommand("check")]
	public static void Check(string CheckId, int Difficulty, string Stat, bool Disadvantage, bool Advantage)
	{
		signalBus.EmitSignal("check", CheckId, Difficulty, Stat, Disadvantage, Advantage);
	}

	[YarnCommand("passive_check")]
	public static void PassiveCheck(string CheckId, int Difficulty, string Stat, bool Disadvantage, bool Advantage)
	{
		signalBus.EmitSignal("passive_check", CheckId, Difficulty, Stat, Disadvantage, Advantage);
	}

}
