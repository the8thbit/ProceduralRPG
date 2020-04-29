using UnityEngine;
using UnityEditor;
using Unity.Profiling;
using System.Collections.Generic;
public class Debug : ScriptableObject
{
    public static bool DEBUG = true;
    private static Dictionary<string, ProfilerMarker> ProfileMarkers;

    public static int NORMAL = 0;
    public static int WORLD_GENERATION = 1;
    public static int SETTLEMENT_GENERATION = 2;
    public static int CHUNK_STRUCTURE_GENERATION = 3;
    public static int DUNGEON_GENERATION = 4;
    public static int QUEST_GENERATION = 5;
    public static int ENTITY_GENERATION = 6;
    
    
    public static int ENTITY = 7;
    public static int ENTITY_TEST = 8;

    public static int CHUNK_LOADING = 9;

    public static int CURRENT_TAG= NORMAL;


    public static void Error(string str)
    {
        UnityEngine.Debug.LogError(str);
    }
    public static void Error(object obj)
    {
        UnityEngine.Debug.LogError(obj);
    }

    public static void LogError(object obj)
    {
        UnityEngine.Debug.LogError(obj);
    }
    public static void Log(string str, int TAG=-1)
    {
        if(TAG==-1 || TAG==CURRENT_TAG)
            UnityEngine.Debug.Log(str);
    }
    public static void Log(object obj, int TAG = -1)
    {
        if (TAG == -1 || TAG == CURRENT_TAG)
            UnityEngine.Debug.Log(obj);
    }

    public static void BeginDeepProfile(string tag)
    {
        if (!DEBUG)
            return;
        if (ProfileMarkers == null)
            ProfileMarkers = new Dictionary<string, ProfilerMarker>();
        if (ProfileMarkers.ContainsKey(tag))
        {
            ProfileMarkers[tag].Begin();
        }
        else
        {
            ProfileMarkers.Add(tag, new ProfilerMarker(tag));
            ProfileMarkers[tag].Begin();
        }
    }

    public static void EndDeepProfile(string tag)
    {
        if (!DEBUG)
            return;
        if (ProfileMarkers != null)
        {
            if (ProfileMarkers.ContainsKey(tag))
                ProfileMarkers[tag].End();
        }
    }
    public static void BeginProfile(string tag)
    {
        UnityEngine.Profiling.Profiler.BeginSample(tag);
    }

    public static void EndProfile()
    {
        UnityEngine.Profiling.Profiler.EndSample();

    }
}