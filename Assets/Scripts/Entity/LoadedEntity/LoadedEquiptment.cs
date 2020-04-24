using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic script, at current we only use this as a utility.
/// </summary>
public class LoadedEquiptment : MonoBehaviour
{

    public SkinnedMeshRenderer SMR { get; private set; }
    void Awake()
    {
        SMR = GetComponent<SkinnedMeshRenderer>();
        if (SMR == null)
            SMR = GetComponentInChildren<SkinnedMeshRenderer>();
        if (SMR == null)
            Debug.LogError("[Loading] Skinned Mesh Renderer could not be found");
    }

}
