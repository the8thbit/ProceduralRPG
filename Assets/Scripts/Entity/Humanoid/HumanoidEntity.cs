using UnityEngine;
using UnityEditor;

/// <summary>
/// A humanoid entity is one that has human form.
/// Humanoid entities can have equipted items 
/// </summary>
[System.Serializable]
public abstract class HumanoidEntity : Entity {


    public EquiptmentManager EquiptmentManager { get; private set; }
    public HumanoidEntity(EntityCombatAI combatAI, EntityTaskAI taskAI, string name = "un-named_entity", bool isFixed = false) 
        : base(combatAI, taskAI, name, isFixed)
    {
        EquiptmentManager = new EquiptmentManager(this);
    }
}