using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// EntitySpellManager is used to deal with 
/// the casting of spells.
/// Used to cast a spell by the Entity/Player
/// </summary>
[System.Serializable]
public class EntitySpellManager
{

    private Entity Entity;

    public float MaxMana { get; private set; }
    public float CurrentMana { get; private set; }
    public float ManaRegenerationRate { get; private set; }


    private List<Spell> AllSpells;
    private Spell[] EquiptSpells;
    private bool[] SpellKeyDown;
    [System.NonSerialized]
    private Stopwatch stopwatch;
    public EntitySpellManager(Entity entity)
    {
        Entity = entity;
        AllSpells = new List<Spell>();
        EquiptSpells = new Spell[2];
        SpellKeyDown = new bool[2];
        MaxMana = CurrentMana = 100;
        ManaRegenerationRate = 5;
        stopwatch = new Stopwatch();
    }

    /// <summary>
    /// Increases mana by the mana regeneration rate.
    /// </summary>
    /// <param name="time"></param>
    public void Tick(float time)
    {
        CurrentMana = Mathf.Clamp(CurrentMana + ManaRegenerationRate * time, 0, MaxMana);
    }
    /// <summary>
    /// Called for a loaded entity that can cast spells
    /// Checks if the Entity is currently casting a Continous Cast spell
    /// If they are, we update the spell casting, and change mana and XP values
    /// accordingly
    /// </summary>
    /// <param name="data"></param>
    public void Update(SpellCastData data)
    {
        //Check lots 1 and 2
        for(int i=0; i<2; i++)
        {
            //get the spell, continue if spell is null
            Spell s = EquiptSpells[i];
            if (s == null)
                continue;
            //If the spell is a hold spell
            if (s is HoldSpell)
            {
                HoldSpell holdSpell = s as HoldSpell;

                if (holdSpell.IsCast)
                {
                    //Check if the key is down
                    if (SpellKeyDown[i] == false)
                    {
                        holdSpell.SpellEnd(data); //If not, then end the spell
                        Debug.Log("Ending");
                    }                       
                    else
                    {
                        //Set to false to be updated
                        SpellKeyDown[i] = false;
                        //If we have enough mana, update the spell
                        if (CurrentMana > holdSpell.ManaCost * Time.deltaTime)
                        {
                            Debug.Log("Updating");
                            holdSpell.SpellUpdate(data);
                            AddXp(holdSpell);
                            CurrentMana -= holdSpell.ManaCost * Time.deltaTime;
                        }
                        else
                        {
                            Debug.Log("Ending");

                            //If not, stop the spell
                            holdSpell.SpellEnd(data);
                        }


                    }
                }

            }
        }

    }
    /// <summary>
    /// Casts the specified spell from the local spell inventory
    /// based on the relevent SpellCastData
    /// </summary>
    /// <param name="spell"></param>
    /// <param name="data"></param>
    public void CastSpell(int spell, SpellCastData data)
    {
        //Check if spell number is valid
        if(spell <= 0 && spell < 2)
        {
            SpellKeyDown[spell] = true; //Set the hold figure to tue

            Spell s = EquiptSpells[spell];
            if (s == null)
                return;
            if(s is SingleSpell)
            {
                //If the spell is a single cast, ensure the cool down and mana costs are valid. 
                SingleSpell singSpe = s as SingleSpell;
                if(CurrentMana > s.ManaCost && stopwatch.ElapsedMilliseconds > singSpe.CoolDown * 1000)
                {
                    singSpe.CastSpell(data);
                    AddXp(s);
                    CurrentMana -= s.ManaCost;
                    stopwatch.Restart();
                }
            }
            else
            {
                //If the spell is a hold spell, check if we've started casting it.
                HoldSpell holdSpel = s as HoldSpell;
                if (!holdSpel.IsCast)
                {
                    Debug.Log("Casting");
                    holdSpel.SpellStart(data);
                    AddXp(s);
                    CurrentMana -= s.ManaCost * Time.deltaTime;
                }
                //If we are casting, it is dealth with in the u[
                    
            }


        }
    }


    /// <summary>
    /// Checks the type of magic used for the spell (offensive, defencive, passive),
    /// and increases the XP by the relevent amount
    /// </summary>
    /// <param name="spell"></param>
    private void AddXp(Spell spell)
    {
        //Shouldn't happen but just incase
        if (spell == null)
            return;
        //Calculate the xp based on whether it is a single shot spell, or constant cast spell
        float xpGain = (spell is SingleSpell)?spell.XPGain:spell.XPGain* Time.deltaTime;


        //Check the combat type, and add XP to relevent skill
        switch (spell.SpellCombatType)
        {         

            case SpellCombatType.OFFENSIVE:
                Entity.SkillTree.OffensiveMagic.AddXP(xpGain);
                return;
            case SpellCombatType.DEFENSIVE:
                Entity.SkillTree.DefensiveMagic.AddXP(xpGain);
                return;
            case SpellCombatType.PASSIVE:
                Entity.SkillTree.PassiveMagic.AddXP(xpGain);
                return;
        }
    }

    public void AddSpell(Spell spell, int spellSlot = -1)
    {
        if (!AllSpells.Contains(spell))
            AllSpells.Add(spell);
        if (spellSlot == 0)
            EquiptSpells[0] = spell;
        else if (spellSlot == 1)
            EquiptSpells[1] = spell;
    }


}