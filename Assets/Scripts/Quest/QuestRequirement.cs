using UnityEngine;
using UnityEditor;

/// <summary>
/// Holds onto information that defines minimum stats for a player to start a quest.
/// This includes previously completed quests, minimum skills, etc
/// </summary>
public class QuestRequirement
{



    /// <summary>
    /// TODO - checks if player reaches the given requirements.
    /// Currently just returns true for testing purposes.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool IsRequirementMet(Player player)
    {
        return true;
    }
}