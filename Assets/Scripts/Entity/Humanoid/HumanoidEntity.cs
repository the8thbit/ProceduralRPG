using UnityEngine;
using UnityEditor;

/// <summary>
/// A humanoid entity is one that has human form.
/// Humanoid entities can have equipted items 
/// </summary>
[System.Serializable]
public abstract class HumanoidEntity : Entity {


    public EquiptmentManager EquiptmentManager { get; private set; }
    public HumanoidEntity(EntityCombatAI combatAI, EntityTaskAI taskAI, EntityMovementData movementData,string name = "un-named_entity", bool isFixed = false) 
        : base(combatAI, taskAI, movementData, name, isFixed)
    {
        EquiptmentManager = new EquiptmentManager(this);
    }
}