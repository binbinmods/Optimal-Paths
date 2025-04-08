// These are your imports, mostly you'll be needing these 5 for every plugin. Some will need more.

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;


// The Plugin csharp file is used to 


// Make sure all your files have the same namespace and this namespace matches the RootNamespace in the .csproj file
namespace OptimalPaths{
    // These are used to create the actual plugin. If you don't need Obeliskial Essentials for your mod, 
    // delete the BepInDependency and the associated code "RegisterMod()" below.

    // If you have other dependencies, such as obeliskial content, make sure to include them here.
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    // [BepInDependency("com.stiffmeds.obeliskialessentials")] // this is the name of the .dll in the !libs folder.
    [BepInProcess("AcrossTheObelisk.exe")] //Don't change this

    // If PluginInfo isn't working, you are either:
    // 1. Using BepInEx v6
    // 2. Have an issue with your csproj file (not loading the analyzer or BepInEx appropriately)
    // 3. You have an issue with your solution file (not referencing the correct csproj file)


    public class Plugin : BaseUnityPlugin
    {
        
        // If desired, you can create configs for users by creating a ConfigEntry object here, 
        // and then use config = Config.Bind() to set the title, default value, and description of the config.
        // It automatically creates the appropriate configs.
        
        public static ConfigEntry<bool> EnableOptimalPaths { get; set; }
        public static ConfigEntry<bool> HighlightPath { get; set; }
        public static ConfigEntry<bool> ForceAllNodes { get; set; }
        public static ConfigEntry<bool> ForceHighScoring { get; set; }
        public static ConfigEntry<bool> ForceRareEvents { get; set; }
        public static ConfigEntry<bool> EnableHighScoreLog { get; set; }
        

        internal int ModDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;

        public static string debugBase = $"{PluginInfo.PLUGIN_GUID} ";

        private void Awake()
        {

            // The Logger will allow you to print things to the LogOutput (found in the BepInEx directory)
            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");
            
            // Sets the title, default values, and descriptions
            EnableOptimalPaths = Config.Bind(new ConfigDefinition("Optimal Paths", "Enable Optimal Paths"), true, new ConfigDescription("If false, disables the mod. Restart the game upon changing this or any setting."));
            ForceHighScoring = Config.Bind(new ConfigDefinition("Optimal Paths", "Force Highscoring"), false, new ConfigDescription("If true, forces nodes to be in their highscoring variant. This overrides other settings. Restart the game after changing this."));
            HighlightPath = Config.Bind(new ConfigDefinition("Optimal Paths", "Highlight Path (WIP)"), false, new ConfigDescription("If true, highlights the optimal path in adventure mode with a red flare for each node. (Not implemented yet)"));
            ForceAllNodes = Config.Bind(new ConfigDefinition("Optimal Paths", "Force All Nodes"), true, new ConfigDescription("If true, forces all nodes to spawn. Restart the game after changing this."));            
            ForceRareEvents = Config.Bind(new ConfigDefinition("Optimal Paths", "Force Rare Events"), true, new ConfigDescription("If true, forces all nodes to have their rarest form. Also, nodes will prioritize events over combats. Restart the game after changing this."));
            EnableHighScoreLog = Config.Bind(new ConfigDefinition("Optimal Paths", "Enable High Score Log"), false, new ConfigDescription("If true, logs some additional high scoring info."));
            

            // Register with Obeliskial Essentials, delete this if you don't need it.
            // RegisterMod(
            //     _name: PluginInfo.PLUGIN_NAME,
            //     _author: "binbin",
            //     _description: "Sample Plugin",
            //     _version: PluginInfo.PLUGIN_VERSION,
            //     _date: ModDate,
            //     _link: @"https://github.com/binbinmods/SampleCSharpWorkspace"
            // );

            // apply patches
            // NodePathCode.PathScoreCalc();
            harmony.PatchAll();
            
        }

        internal static void LogDebug(string msg)
        {
            Log.LogDebug(debugBase + msg);
        }
        internal static void LogInfo(string msg)
        {
            Log.LogInfo(debugBase + msg);
        }
        internal static void LogError(string msg)
        {
            Log.LogError(debugBase + msg);
        }
    }
}