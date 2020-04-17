using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class PlayerCamera : MonoBehaviour
{

    public Vector3 Dif { get; private set; }
    public Camera Camera { get; private set; }
    public PlayerManager PlayerManager { get; private set; }

    private List<GameObject> ViewBlock;

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        ViewBlock = new List<GameObject>();
        if (TestMain.TEST_MODE)
        {
            PlayerManager = GameObject.Find("PlayerManager").gameObject.GetComponent<PlayerManager>();
        }
        else
        {
            PlayerManager = transform.parent.gameObject.GetComponent<PlayerManager>();

        }
        Dif = new Vector3(0, 10, -10);
    }


    private void Update()
    {
        if(PlayerManager.Player != null)
        {
            transform.position = PlayerManager.LoadedPlayer.transform.position + new Vector3(0, 10, -10);
            transform.LookAt(PlayerManager.LoadedPlayer.transform);
        }
    }

    private List<Collider> colliders = new List<Collider>();
    public List<Collider> GetColliders() { return colliders; }

    private void OnTriggerEnter(Collider other)
    {

        LoadedEntity le = other.gameObject.GetComponent<LoadedEntity>();
        if(le == null)
        {
            if (other.gameObject.GetComponent<LoadedChunk>() != null)
                return;
            if (other.gameObject.GetComponent<LoadedProjectile>() != null)
                return;
            if (other.gameObject.GetComponent<LoadedBeam>() != null)
                return;
            if(other.gameObject.GetComponent<MeshRenderer>() != null)
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LoadedEntity le = other.gameObject.GetComponent<LoadedEntity>();
        if (le == null)
        {
            if (other.gameObject.GetComponent<LoadedChunk>() != null)
                return;
            if (other.gameObject.GetComponent<LoadedBeam>() != null)
                return;
            if (other.gameObject.GetComponent<MeshRenderer>() != null)
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}