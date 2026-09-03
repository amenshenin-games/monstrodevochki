using Godot;
using System;
using YarnSpinnerGodot;
using System.Collections.Generic;

[GlobalClass]
public partial class CustomVariableStorage : VariableStorageBehaviour
{
    private Node playerData;
    private Node gameData;

    public override void _Ready()
    {
        playerData = GetNode("/root/PlayerData");
        gameData = GetNode("/root/GameData");
    }

    // ---- Чтение ----
    public override bool TryGetValue<T>(string variableName, out T result)
    {
        string key = variableName.TrimStart('$');

        // 1. Ищем в PlayerData
        Variant? value = playerData?.Call("get_value", key);
        if (value.HasValue && value.Value.VariantType != Variant.Type.Nil)
        {
            if (ConvertToType<T>(value.Value, out result))
                return true;
        }

        // 2. Ищем в GameData
        value = gameData?.Call("get_value", key);
        if (value.HasValue && value.Value.VariantType != Variant.Type.Nil)
        {
            if (ConvertToType<T>(value.Value, out result))
                return true;
        }

        result = default;
        return false;
    }

    // ---- Запись ----
    public override void SetValue(string variableName, float floatValue)
    {
        string key = variableName.TrimStart('$');
        if (playerData?.Call("has_key", key).AsBool() == true)
            playerData.Call("set_value", key, floatValue);
        else
            gameData?.Call("set_value", key, floatValue);
    }

    public override void SetValue(string variableName, string stringValue)
    {
        string key = variableName.TrimStart('$');
        if (playerData?.Call("has_key", key).AsBool() == true)
            playerData.Call("set_value", key, stringValue);
        else
            gameData?.Call("set_value", key, stringValue);
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        string key = variableName.TrimStart('$');
        if (playerData?.Call("has_key", key).AsBool() == true)
            playerData.Call("set_value", key, boolValue);
        else
            gameData?.Call("set_value", key, boolValue);
    }

    // ---- Обязательные методы ----
    public override void Clear()
    {
        gameData?.Call("clear");
    }

    public override bool Contains(string variableName)
    {
        string key = variableName.TrimStart('$');
        if (playerData?.Call("has_key", key).AsBool() == true)
            return true;
        return gameData?.Call("has_key", key).AsBool() == true;
    }

    public override (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables()
    {
        return (new Dictionary<string, float>(), new Dictionary<string, string>(), new Dictionary<string, bool>());
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings,
        Dictionary<string, bool> bools, bool clear = true)
    {
        // Заглушка – данные загружаются через SaveManager
    }

    // ---- Конвертер ----
    private bool ConvertToType<T>(Variant variant, out T result)
    {
        if (variant.VariantType == Variant.Type.Nil)
        {
            result = default;
            return false;
        }

        if (typeof(T) == typeof(float))
        {
            if (variant.VariantType == Variant.Type.Float || variant.VariantType == Variant.Type.Int)
            {
                result = (T)(object)variant.As<float>();
                return true;
            }
            result = default;
            return false;
        }
        if (typeof(T) == typeof(string))
        {
            if (variant.VariantType == Variant.Type.String)
            {
                result = (T)(object)variant.As<string>();
                return true;
            }
            result = default;
            return false;
        }
        if (typeof(T) == typeof(bool))
        {
            if (variant.VariantType == Variant.Type.Bool)
            {
                result = (T)(object)variant.As<bool>();
                return true;
            }
            result = default;
            return false;
        }
        if (typeof(T) == typeof(IConvertible))
        {
            if (variant.VariantType == Variant.Type.Float || variant.VariantType == Variant.Type.Int)
            {
                result = (T)(IConvertible)variant.As<float>();
                return true;
            }
            if (variant.VariantType == Variant.Type.String)
            {
                result = (T)(IConvertible)variant.As<string>();
                return true;
            }
            if (variant.VariantType == Variant.Type.Bool)
            {
                result = (T)(IConvertible)variant.As<bool>();
                return true;
            }
            result = default;
            return false;
        }

        result = default;
        return false;
    }
}