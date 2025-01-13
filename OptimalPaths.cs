using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;
using System;
using static OptimalPaths.CustomFunctions;
using static OptimalPaths.Plugin;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Make sure your namespace is the same everywhere
namespace OptimalPaths
{

    [HarmonyPatch] //DO NOT REMOVE/CHANGE

    public class OptimalPaths
    {
        // To create a patch, you need to declare either a prefix or a postfix. 
        // Prefixes are executed before the original code, postfixes are executed after
        // Then you need to tell Harmony which method to patch.

        static string[] optimalPath = ["sen_0", "sen_1"];
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), nameof(AtOManager.AssignSingleGameNode))]
        public static void AssignSingleGameNodePrefix(ref AtOManager __instance, ref Node _node)
        {
            LogInfo("AssignSingleGameNode Prefix");
            if (!Plugin.EnableOptimalPaths.Value || _node == null || _node.nodeData == null)
            {
                return;
            }
            // forces all nodes to exist
            _node.nodeData.ExistsPercent = 100;

            // forces all combats to exist since they give more score than any event node
            if (_node.nodeData.CombatPercent != 0)
            {
                _node.nodeData.CombatPercent = 100;
                _node.nodeData.EventPercent = 0;
                return;
            }

            // forces all events with multiple nodes to choose the last event in the list. 
            // This happens to always be the highest scoring event
            int len = _node.nodeData.NodeEventPercent.Length;
            if (len > 1)
            {
                LogDebug(_node.nodeData.NodeId);
                int[] outputPercents = new int[len];
                // LogDebug("after creation");
                // LogDebug($"len = {len}");
                outputPercents[len - 1] = 100;
                // LogDebug("afterset");
                // LogDebug("NodeEvent Percents:" + string.Join(",", _node.nodeData.NodeEventPercent));
                // LogDebug("Output Percents:" + string.Join(",", outputPercents));

                _node.nodeData.NodeEventPercent = outputPercents;
            }
            return;
        }

        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(MapManager), nameof(MapManager.BeginMap))]
        // public static void BeginMapPostfix()
        // {
        //     LogInfo("BeginMap Postfix");
        //     Dictionary<string, Node> mapNodeDict = MapManager.Instance.GetMapNodeDict();

        // }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Node), nameof(Node.AssignNode))]

        public static void AssignNodePrefix(ref Node __instance)
        {
            LogInfo("AssignNodePrefix - Setting Optimal Nodes to Red");
            if (optimalPath.Contains(__instance.nodeData.NodeId))
            {
                ParticleSystem nodeImageParticlesSystem = Traverse.Create(__instance).Field("nodeImageParticlesSystem").GetValue<ParticleSystem>();
                if (nodeImageParticlesSystem == null)
                {
                    Transform nodeImageParticlesT = Traverse.Create(__instance).Field("nodeImageParticlesT").GetValue<Transform>();
                    if (nodeImageParticlesT == null)
                    {
                        LogError("null nodeImageParticlesSystem");
                        return;

                    }
                    nodeImageParticlesSystem = nodeImageParticlesT.GetComponent<ParticleSystem>();
                }

                ParticleSystem.MainModule main = nodeImageParticlesSystem.main;
                main.startColor = (ParticleSystem.MinMaxGradient)Color.red;
                Traverse.Create(__instance).Field("nodeImageParticlesSystem").SetValue(nodeImageParticlesSystem);
            }
        }
    }
}