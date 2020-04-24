using UnityEngine;
using UnityEditor;

public abstract class CameraController : MonoBehaviour
{

    protected PlayerManager PlayerManager;
    protected Camera Camera;

    private void Awake()
    {
        PlayerManager = GetComponentInParent<PlayerManager>();
        Camera = GetComponent<Camera>();
    }

    public abstract void Update();
}

public class FirstPersonCC : CameraController, IGamePauseEvent
{


    private float Theta, Phi;
    private float Sensitivity=0.5f;

    private bool Pause;

    private void Start()
    {
        Theta = PlayerManager.Player.LookAngle;
        Phi = 0;
        EventManager.Instance.AddListener(this);
        Cursor.lockState = CursorLockMode.Locked;

    }

    public override void Update()
    {
        if (Pause)
            return;
        Theta += Input.GetAxis("Mouse X") * Sensitivity;
        Phi   += Input.GetAxis("Mouse Y") * Sensitivity;
        transform.position = PlayerManager.LoadedPlayer.GetComponent<LoadedEquiptmentManager>().HEAD.transform.position;
        //transform.position = PlayerManager.LoadedPlayer.transform.position + Vector3.up*1.5f;
        transform.rotation = Quaternion.Euler(0, Theta, 0);
        PlayerManager.Player.SetLookAngle(Theta);
        PlayerManager.LoadedPlayer.transform.rotation = Quaternion.Euler(0, Theta, 0);
    }

    public void GamePauseEvent(bool pause)
    {
        Debug.Log("test");
        Pause = pause;
        if (pause)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener(this);
    }
}