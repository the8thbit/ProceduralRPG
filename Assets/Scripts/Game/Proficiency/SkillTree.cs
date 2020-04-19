using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
/// <summary>
/// Skill tree is an object that belongs to an entity
/// It contains all of their experience for each skill,
///  and each elemental mastery
/// </summary>
[System.Serializable]
public class SkillTree
{



    public HeavyArmour HeavyArmour;
    public SkillLightArmour LightArmour;
    public SharpWeapons SharpWeapons;
    public BluntWeapons BluntWeapons;
    public Archery Archery;
    public OffensiveMagic OffensiveMagic;
    public DefensiveMagic DefensiveMagic;
    public PassiveMagic PassiveMagic;



    public SkillTree()
    {
        HeavyArmour = new HeavyArmour();
        LightArmour = new SkillLightArmour();
        SharpWeapons = new SharpWeapons();
        BluntWeapons = new BluntWeapons();
        Archery = new Archery();
        OffensiveMagic = new OffensiveMagic();
        DefensiveMagic = new DefensiveMagic();
        PassiveMagic = new PassiveMagic();
    }



    /// <summary>
    /// Returns the required skill from this skill tree.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public Skill GetSkill(Skills s)
    {
        switch (s)
        {
            case Skills.Archery:
                return Archery;
            case Skills.BluntWeapons:
                return BluntWeapons;
            case Skills.DefenciveMagic:
                return DefensiveMagic;
            case Skills.HeavyArmour:
                return HeavyArmour;
            case Skills.LightArmour:
                return LightArmour;
            case Skills.OffensiveMagic:
                return OffensiveMagic;
            case Skills.PassiveMagic:
                return PassiveMagic;
            case Skills.SharpWeapons:
                return SharpWeapons;
        }
        return null;
            
    }

    public float GetXP(Skills skill)
    {
        return GetSkill(skill).GetXP();
    }

    public int GetLevel(Skills skill)
    {
        return GetSkill(skill).GetLevel();
    }





    /// <summary>
    /// TODO- choosen valid formula,
    /// for now, level = sqrt(XP)
    /// therefore, XP = Level^2
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float GetXPFromLevel(int level)
    {
        return level * level;
    }

    public static int GetLevelFromXP(float xp) {


        return Mathf.Clamp(Mathf.FloorToInt(Mathf.Sqrt(xp)),1, 100);
    }

}


public enum Skills
{
    HeavyArmour, LightArmour, SharpWeapons, BluntWeapons, Archery,
    OffensiveMagic,DefenciveMagic, PassiveMagic, 
}

