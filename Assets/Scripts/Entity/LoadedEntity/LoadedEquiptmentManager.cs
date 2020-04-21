using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
public enum LoadedEquiptmentPlacement
{
    head, chest, legs, feet, handR, handL, weaponSheath, backSheath
}
public class LoadedEquiptmentManager : MonoBehaviour
{

    
    //references to the bone transforms for each equiptment placement
    public GameObject HEAD, CHEST, LEGS, FOOT_L, FOOT_R, HAND_L, HAND_L_END, HAND_R, HAND_R_END;
    public GameObject WEAPONSHEATH, WEAPONSHEATH_END, BACKSHEATH, BACKSHEATH_END;
    private Dictionary<LoadedEquiptmentPlacement, GameObject> EquiptObjects;


    private LoadedEntity LoadedEntity;
    private void OnEnable()
    {
        LoadedEntity = GetComponent<LoadedEntity>();
        EquiptObjects = new Dictionary<LoadedEquiptmentPlacement, GameObject>();
    }

    private void Start()
    {
        (LoadedEntity.Entity as HumanoidEntity).EquiptmentManager.SetLoadedEquiptmentManager(this);

    }

    public void SetEquiptmentItem(LoadedEquiptmentPlacement slot, Item item)
    {
        Debug.Log("[LoadedEquiptmentManager] Adding item " + item + " to slot " + slot);
        GameObject remove;
        EquiptObjects.TryGetValue(slot, out remove);
        if(remove != null)
        {
            DestroyImmediate(remove);
        }

        if (item == null)
            return;

        GameObject obj = Instantiate(item.GetEquiptItem());
        switch (slot)
        {
            case LoadedEquiptmentPlacement.weaponSheath:
                obj.transform.parent = WEAPONSHEATH.transform;
                break;
            case LoadedEquiptmentPlacement.handR:
                obj.transform.parent = HAND_R.transform;
                break;
        }
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        if (EquiptObjects.ContainsKey(slot))
            EquiptObjects[slot] = obj;
        else
            EquiptObjects.Add(slot, obj);
    }


    public void UnsheathWeapon(LoadedEquiptmentPlacement slot)
    {
        GameObject sheathed = GetObjectInSlot(slot);
        LoadedEntity.AnimationManager.HumanoidCast().DrawFromSheath(HAND_R, sheathed);
        StartCoroutine(WaitToUnsheath(slot));

    }

    private IEnumerator WaitToUnsheath(LoadedEquiptmentPlacement slot)
    {
        yield return new WaitForSeconds(LoadedHumanoidAnimatorManager.GRAB_SHEATHED_WEAPON_ANI_TIME);
        EquiptObjects[LoadedEquiptmentPlacement.handR] = EquiptObjects[slot];
        EquiptObjects[slot] = null;
    }

    public GameObject GetObjectInSlot(LoadedEquiptmentPlacement place)
    {
        if (EquiptObjects.ContainsKey(place))
            return EquiptObjects[place];
        return null;
    }
    public void AddObjectInSlot(LoadedEquiptmentPlacement place, GameObject obj)
    {
        if (EquiptObjects.ContainsKey(place))
            EquiptObjects[place] = obj;
        else
        {
            EquiptObjects.Add(place, obj);
        }
    }
    public void SwapObjectSlots(LoadedEquiptmentPlacement a, LoadedEquiptmentPlacement b)
    {
        GameObject A = GetObjectInSlot(a);
        GameObject B = GetObjectInSlot(b);
        AddObjectInSlot(a, B);
        AddObjectInSlot(b, A);
    }


    private void Update()
    {
        GameObject weaponObj;
        if(EquiptObjects.TryGetValue(LoadedEquiptmentPlacement.handR, out weaponObj))
        {
            weaponObj.transform.rotation = GetBoneRotation(HAND_R, HAND_R_END);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(HAND_R.transform.position, HAND_R_END.transform.position);
    }








    /// <summary>
    /// Returns the Quaternion rotation that defines the bone
    /// rotation starting at t1 and ending at t2
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public static Quaternion GetBoneRotation(GameObject t1, GameObject t2)
    {
        Vector3 dif = (t2.transform.position - t1.transform.position).normalized;

        return Quaternion.FromToRotation(Vector3.up, dif);
        Debug.Log(t2.transform.localPosition + "_" + t1.transform.localPosition + "_" + dif);
        return Quaternion.Euler(dif);
        return Quaternion.LookRotation(t2.transform.localPosition.normalized, Vector3.up);

    }

}