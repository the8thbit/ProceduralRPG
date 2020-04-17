using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Bandit : HumanoidEntity
{

    public Bandit() :base (new BasicHumanoidCombatAI(), new CreatureTaskAI())
    {

    }



    public override string EntityGameObjectSource => "human";

    protected override void KillInternal()
    {
    }
}