using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;
using System;
using static OptimalPaths.CustomFunctions;

// Make sure your namespace is the same everywhere
namespace OptimalPaths{

    [HarmonyPatch] //DO NOT REMOVE/CHANGE

    public class OptimalPaths
    {
        // To create a patch, you need to declare either a prefix or a postfix. 
        // Prefixes are executed before the original code, postfixes are executed after
        // Then you need to tell Harmony which method to patch.

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager),nameof(AtOManager.AssignSingleGameNode))]
        public static void AssignSingleGameNodePrefix(ref AtOManager __instance, ref Node _node){
            Plugin.Log.LogInfo("AssignSingleGameNode Prefix");
            if(!Plugin.EnableOptimalPaths.Value||_node==null || _node.nodeData==null)
            {
                return;
            }
            // forces all nodes to exist
            _node.nodeData.ExistsPercent=100;

            // forces all combats to exist since they give more score than any event node
            if(_node.nodeData.CombatPercent!=0)
            {
                _node.nodeData.CombatPercent=100;
                _node.nodeData.EventPercent=0;
            }

            if (_node.nodeData.NodeEventPercent.Length>1)
            {
                int[] outputPercents = [_node.nodeData.NodeEventPercent.Length];
                for (int i = 0; i < _node.nodeData.NodeEventPercent.Length; i++)
                {
                    outputPercents[i] = i>=_node.nodeData.NodeEventPercent.Length-1 ? 100 : 0;
                }
                _node.nodeData.NodeEventPercent = outputPercents;
            }


            return;       
            
        }
        
        

    }
}