using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallSeeCamera : MonoBehaviour
{

    public RawImage RenderTextureDisplayImage;

    private Camera MainCamera;
    private Camera ThisCamera;
    public RenderTexture RenderTexture;

    public bool ShouldShow { get; private set; }

    private int ScreenWidth, ScreenHeight;

    /// <summary>
    /// Collects instances 
    /// </summary>
    void Start()
    {
        ThisCamera = GetComponent<Camera>();
        MainCamera = GameManager.PlayerManager.PlayerCamera;
    }


    /// <summary>
    /// Updates FOV and enables/disables the see through wall render texture
    /// as required.
    /// </summary>
    void Update()
    {
        if (Screen.width != ScreenWidth || Screen.height != ScreenHeight)
            UpdateFOV();

        //Check for all objects blocking camera view to player
        ShouldShow = WorldObjectIntersect(); ;

        RenderTextureDisplayImage.enabled = ShouldShow;


    }

    /// <summary>
    /// Raycasts from the camera to a set of points surrounding the player
    /// Returns true if at least 1 world object intersects one of the rays
    /// </summary>
    /// <returns></returns>
    private bool WorldObjectIntersect()
    {
        //Define positions (relative to player) to raycast to
        Vector3[] toCheck = new Vector3[] { new Vector3(0,1,0), new Vector3(0, -1, 0),
                                             new Vector3(1,0,0),new Vector3(-1,0,0),
                                            new Vector3(0,0,1),new Vector3(0,0,-1)};
        //Get the player middle position
        Collider playerCol = GameManager.PlayerManager.Player.GetLoadedEntity().GetComponent<Collider>();
        Vector3 playerMid = playerCol.bounds.center;
        //iterate all points
        foreach (Vector3 v in toCheck)
        {
            //Define the ray directly
            Vector3 origin = transform.position;
            Vector3 target = playerMid + v;
            Vector3 direction = (target - origin).normalized;
            Ray ray = new Ray(origin, direction);

            float dist = Vector3.Distance(GameManager.PlayerManager.PlayerCameraScript.Dif, Vector3.zero);
            //Raycast the point,
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, dist))
            {
                //If the ray intersects an object, check if it is a WorldOjbect. If so, we return true
                if (hit.transform.gameObject != null && hit.transform.gameObject.layer == 8)
                    return true;
            }
        }
        //If no WorldObject intersection is found we return false
        return false;
    }

   
    private void UpdateFOV()
    {
         float wfov = CalculateFOVFromWidth();
         float hfov = CalculateFOVFromHeight();
         ScreenWidth = Screen.width;
         ScreenHeight = Screen.height;
         ThisCamera.fieldOfView = Mathf.Min(wfov, hfov);
        //ThisCamera.fieldOfView = 31;
    }

    private float CalculateFOVFromWidth()
    {
        float halfScreen = Screen.width / 2;
        float renderTextureHalf = RenderTexture.width / 2;
        float fov = Mathf.Asin((renderTextureHalf*2*Mathf.Sin(Mathf.Deg2Rad * MainCamera.fieldOfView/2))/halfScreen);
        return fov * Mathf.Rad2Deg*2;
    }
    private float CalculateFOVFromHeight()
    {
        float halfScreen = Screen.height / 2;
        float renderTextureHalf = RenderTexture.height / 2;
        float fov = Mathf.Asin((renderTextureHalf * 2 * Mathf.Sin(Mathf.Deg2Rad * MainCamera.fieldOfView / 2)) / halfScreen);
        return fov * Mathf.Rad2Deg * 2;
    }
}
