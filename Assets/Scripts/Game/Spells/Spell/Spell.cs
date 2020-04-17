using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class Spell
{
    public abstract float ManaCost { get; }
 

    public abstract string Description { get; }


}
[System.Serializable]
public abstract class SingleSpell : Spell
{
    public abstract float CoolDown { get; }
    public abstract void CastSpell(SpellCastData data);

}
[System.Serializable]
public abstract class HoldSpell : Spell
{

    public bool IsCast{ get; protected set; }


    public abstract void SpellStart(SpellCastData data);
    public abstract void SpellUpdate(SpellCastData data);
    public abstract void SpellEnd(SpellCastData data);
}
public struct SpellCastData
{
    public Entity Source;
    public Vector3 Target;
}
