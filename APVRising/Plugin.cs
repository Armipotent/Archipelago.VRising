using APVRising.Utils;
using APVRising.Archipelago;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using VampireCommandFramework;
using ProjectM.UI;
using ProjectM;
using Unity.Entities;
using ProjectM.Scripting;
using Stunlock.Core;

namespace APVRising;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency("gg.deca.VampireCommandFramework")]
//[BepInDependency("gg.deca.Bloodstone")]
public class Plugin : BasePlugin
{
    public const string PluginGUID = "APVRising";
    public const string PluginName = "Archipelago";
    public const string PluginVersion = "0.0.1";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
    public static ManualLogSource BepinLogger;
    public static ArchipelagoClient ArchipelagoClient;
    Harmony _harmony;
    private static World _serverWorld;

    public static bool IsServer => Application.productName == "VRisingServer";

    public override void Load()
    {
        // Plugin startup logic
        BepinLogger = Log;
        ArchipelagoClient = new ArchipelagoClient();
        ArchipelagoConsole.Awake();

        // Harmony patching
        _harmony = new Harmony(PluginGUID);
        _harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        if (IsServer)
        {
            ArchipelagoClient.Instance = new ArchipelagoClient();
        }

        // Register all commands in the assembly with VCF
        CommandRegistry.RegisterAll();

        ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly();
        _harmony?.UnpatchSelf();
        return true;
    }

    public static EntityManager EntityManager => Server.EntityManager;
    public static PrefabCollectionSystem PrefabCollectionSystem => Server.GetExistingSystemManaged<PrefabCollectionSystem>();
    public static GameDataSystem GameDataSystem => Server.GetExistingSystemManaged<GameDataSystem>();
    public static ManagedDataRegistry ManagedDataRegistry => GameDataSystem.ManagedDataRegistry;
    public static DebugEventsSystem DebugEventsSystem => Server.GetExistingSystemManaged<DebugEventsSystem>();
    public static UnitSpawnerUpdateSystem UnitSpawnerUpdateSystem => Server.GetExistingSystemManaged<UnitSpawnerUpdateSystem>();
    public static ServerScriptMapper ServerScriptMapper => Server.GetExistingSystemManaged<ServerScriptMapper>();

	/// <summary>
	/// Return the Unity ECS World instance used on the server build of VRising.
	/// </summary>
	public static World Server
	{
		get
		{
			if (_serverWorld != null) return _serverWorld;

			_serverWorld = GetWorld("Server")
				?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");
			return _serverWorld;
		}
	}

	private static World GetWorld(string name)
	{
		foreach (var world in World.s_AllWorlds)
		{
			if (world.Name == name)
			{
				_serverWorld = world;
				return world;
			}
		}

		return null;
	}
}