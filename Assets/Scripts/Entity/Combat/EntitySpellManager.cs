using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;

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
        AllSpells = new List<Spell>();
        EquiptSpells = new Spell[2];
        SpellKeyDown = new bool[2];
        MaxMana = CurrentMana = 100;
        ManaRegenerationRate = 5;
        stopwatch = new Stopwatch();
    }

    public void Tick(float time)
    {
        CurrentMana = Mathf.Clamp(CurrentMana + ManaRegenerationRate * time, 0, MaxMana);
    }

    public void Update(SpellCastData data)
    {
        for(int i=0; i<2; i++)
        {
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

    public void SpellButtonDown(int spell, SpellCastData data)
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
                    CurrentMana -= s.ManaCost * Time.deltaTime;
                }
                    
            }


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