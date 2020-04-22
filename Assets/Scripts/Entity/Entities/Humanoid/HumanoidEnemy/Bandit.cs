using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Bandit : HumanoidEntity
{

    public Bandit() :base (new BasicHumanoidCombatAI(), new CreatureTaskAI(), new EntityMovementData())
    {

    }



    public override string EntityGameObjectSource => "human";

    protected override void KillInternal()
    {
    }
}