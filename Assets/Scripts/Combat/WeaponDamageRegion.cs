using UnityEngine;
using UnityEditor;

public class WeaponDamageRegion
{

    public Vector3 Bot { get; private set; }
    public Vector3 Top { get; private set; }
    public float Radius { get; private set; }
    /// <summary>
    /// TODO - Add in damage amount, damage type, etc
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="top"></param>
    /// <param name="radius"></param>
    public WeaponDamageRegion(Vector3 bot, Vector3 top, float radius)
    {
        Bot = bot;
        Top = top;
        Radius = radius;
    }



    public bool CauseDamage(Collider rb)
    {
        Vector3 pos = rb.attachedRigidbody.transform.position;
        //Debug.Log(pos);
        if (QuickDistWithin(pos, Bot, Radius) || QuickDistWithin(pos, Top, Radius))
            return true;
        return false;
    }

    private bool QuickDistWithin(Vector3 a, Vector3 b, float r)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y * b.y) + (a.z - b.z) * (a.z - b.z) < r*r;
    }
}