using UnityEngine;
using UnityEditor;

public abstract class Projectile
{
    public float ProjectileSpeed { get; protected set; } //Speed of projectile
    public bool DestroyOnCollision { get; protected set; } //Should projectile autodestroy itself after 

    public LoadedProjectile LoadedProjectile { get; private set; }

    public abstract string ResourceCode { get; }
    public abstract GameObject GenerateProjectileObject();
    public abstract void InternalOnCollision(Collider other);
    public abstract void Update();

    /// <summary>
    /// Sets the LoadedProjectile assosciated with this instance of prjectile.
    /// </summary>
    /// <param name="loaded"></param>
    public void SetLoadedProjectile(LoadedProjectile loaded)
    {
        LoadedProjectile = loaded;
    }
    public static Entity GetHitEntity(Collider col)
    {
        LoadedEntity LoadedEntity = col.gameObject.GetComponent<LoadedEntity>();
        if (LoadedEntity == null)
            LoadedEntity = col.gameObject.GetComponentInParent<LoadedEntity>();

        if (LoadedEntity == null)
            return null;
        return LoadedEntity.Entity;
    }
}