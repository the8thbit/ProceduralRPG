using UnityEngine;
using UnityEditor;

public class LoadedBeam : MonoBehaviour
{
    public GameObject BeamObject;
    private Beam Beam;
    private float CurrentMaxLength;
    private Entity Source;
    private Vector3 Target;
    private bool Collision;
    public void CreateBeam(Entity source, Vector3 target, Beam beam)
    {
        Beam = beam;
        Source = source;
        UpdateTarget(target);
        CurrentMaxLength = beam.MaxLength;
    }

    public void UpdateTarget(Vector3 target)
    {
        if (Target == target)
            return;
        Target = target;
        Vector3 displacement = Source.GetLoadedEntity().transform.position - target;
        transform.LookAt(target);

        BeamObject.transform.localScale = new Vector3(1, 1, Mathf.Max(1, Mathf.Min(displacement.magnitude, CurrentMaxLength)));
        CurrentMaxLength = Beam.MaxLength;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
            return;
        //Check if the collider is with an entity
        LoadedEntity LoadedEntity = other.gameObject.GetComponent<LoadedEntity>();
        if (LoadedEntity == null)
            LoadedEntity = other.gameObject.GetComponentInParent<LoadedEntity>();
        if (LoadedEntity != null)
        {
            if (LoadedEntity.Entity.Equals(Source))
                return;
            Beam.InternalOnCollision(other);
        }
        else
        {
            //here the collider is a world object
            Vector3 tempTarget = other.gameObject.transform.position;
            CurrentMaxLength = Mathf.Min(CurrentMaxLength, (tempTarget - Source.GetLoadedEntity().transform.position).magnitude);
        }
    }
}