using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;


    public LoadedEntity LoadedPlayer { get; private set; }
    public Player Player { get; private set; }

    public Camera PlayerCamera { get; private set; }
    public PlayerCamera PlayerCameraScript { get; private set; }
    private NPC CurrentlySelected;
    private void Awake()
    {
        Instance = this;
        if (TestMain.TEST_MODE)
        {
            PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
            PlayerCameraScript = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
            //PlayerCamera = GetComponent<Camera>();
            //PlayerCameraScript = GetComponent<PlayerCamera>();
                
        }
        else
        {
            PlayerCamera = transform.Find("PlayerCamera").GetComponent<Camera>();
            PlayerCameraScript = transform.Find("PlayerCamera").GetComponent<PlayerCamera>();
        }
        
    }

    public void SetPlayer(Player player)
    {
        GameObject entityObject = Instantiate(player.GetEntityGameObject());
        entityObject.name = "Player";
        entityObject.transform.parent = transform;

        LoadedEntity loadedEntity = entityObject.GetComponent<LoadedEntity>();
        player.OnEntityLoad(loadedEntity, true);
        loadedEntity.SetEntity(player);
        entityObject.transform.position = player.Position;
        loadedEntity.SetLookBasedOnMovement(false);
        Player = player;
        //Player.CombatManager.AddSpell(new SpellFireball(), 0);
        Player.CombatManager.SpellManager.AddSpell(new SpellStoneWall(), 0);


        Player.Inventory.AddItem(new SteelLongSword());
        Player.Inventory.AddItem(new SimpleDungeonKey(0));

        LoadedPlayer = loadedEntity;
        if (TestMain.TEST_MODE)
        {

        }
        else
        {
        }




    }
    public void Tick(float time)
    {
        Player.CombatManager.Tick(time);
    }

    void Update()
    {

        if (Console.Instance != null && Console.Instance.Active)
            return;


        if (GameManager.GUIManager!=null && GameManager.GUIManager.DialogGUI.InConversation)
            return;

        if (Input.GetKey(KeyCode.E))
        {
            PlayerCameraScript.ClockwiseMove(20 * Time.deltaTime);
        }else if (Input.GetKey(KeyCode.Q))
        {
            PlayerCameraScript.ClockwiseMove(-20 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GUIManager2.Instance.SetVisible(!GUIManager2.Instance.OpenGUIVisible);

        }

        if (GameManager.Paused)
            return;

        Player.Update();
        Debug.BeginDeepProfile("PlayerManagerUpdate");
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //GameManager.DebugGUI.SetData("player_xy_input", new Vector2(x, z).ToString());

        float ud = x * Mathf.Cos(-PlayerCameraScript.Theta * Mathf.Deg2Rad) + z * Mathf.Sin(-PlayerCameraScript.Theta * Mathf.Deg2Rad);
        float lr = x * Mathf.Sin(-PlayerCameraScript.Theta * Mathf.Deg2Rad) - z * Mathf.Cos(-PlayerCameraScript.Theta * Mathf.Deg2Rad);

        x *= Mathf.Cos(PlayerCameraScript.Theta * Mathf.Deg2Rad) + Mathf.Sin(PlayerCameraScript.Theta * Mathf.Deg2Rad);
        z *= Mathf.Sin(PlayerCameraScript.Theta * Mathf.Deg2Rad);

        LoadedPlayer.MoveInDirection(new Vector2(lr, ud));


        Vector3 worldMousePos = GetWorldMousePosition();

        GameManager.DebugGUI.SetData("world_mouse_pos", worldMousePos.ToString());
        LoadedPlayer.LookTowardsPoint(worldMousePos);

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SpellCastData data = new SpellCastData();
            data.Source = Player;
            data.Target = GetWorldMousePosition();
            Player.CombatManager.SpellManager.CastSpell(0, data);
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButton();            
        }else if (Input.GetMouseButtonDown(1))
        {
            RightMouseButton();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadedPlayer.Jump();
        }

        Debug.EndDeepProfile("PlayerManagerUpdate");
    }
    
    public Vector3 GetWorldMousePosition()
    {
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        //We need to know the x and z components of the position when the z is 0.
        //So, the ray is a Point + float * direction
        Vector3 start = PlayerCamera.transform.position;
        float mult = start.y / ray.direction.y;
        float x = start.x  - ray.direction.x * mult;
        float z = start.z  - ray.direction.z * mult;
        //float x = start.x  + ray.direction.x * mult;
        //float z = start.z  + ray.direction.z * mult;
        return new Vector3(x, 0, z);
    }

 

    /// <summary>
    /// Called if the left mouse button pressed
    /// </summary>
    public void LeftMouseButton()
    {
        
        //Weapon in sheath but not in hand, so we put in hand
        if(Player.EquiptmentManager.HasWeapon && !Player.EquiptmentManager.WeaponReady)
        {
            Player.EquiptmentManager.UnsheathWeapon(LoadedEquiptmentPlacement.weaponSheath);
            return;
        }


        if (Player.CombatManager.CanAttack())
        {
            //LoadedPlayer.WeaponController.PlayWeaponAnimation();
            Player.CombatManager.UseEquiptWeapon();
        }

    }

    public void RightMouseButton()
    {
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        bool entSelect = false;
        Debug.Log("hmmm");
        RaycastHit[] raycasthit = Physics.RaycastAll(ray);
        foreach(RaycastHit hit in raycasthit)
        {
            Debug.Log(hit);
            LoadedEntity hitEnt = hit.transform.gameObject.GetComponent<LoadedEntity>();
            if (hitEnt != null)
            {
                entSelect = true;
                //GameManager.DebugGUI.SetSelectedEntity(hitEnt);
                Entity hitEntity = hitEnt.Entity;
                if (hitEntity is NPC)
                {
                    NPC npc = hitEntity as NPC;
                    //If we have selected a different entities
                    if (npc != CurrentlySelected)
                    {
                        Debug.Log(hitEntity.Name + " has been clicked by the player");
                        GameManager.EventManager.InvokeNewEvent(new PlayerTalkToNPC(npc));
                        CurrentlySelected = npc;

                        if (npc.HasDialog())
                        {
                            Debug.Log("TRUE DIALOG");
                            GameManager.GUIManager.StartDialog(npc);
                            if (!Player.InConversation())
                            {
                                if (Player.CurrentDialogNPC() != npc)
                                {
                                    NPCDialog dial = npc.Dialog;
                                    dial.StartDialog();
                                }
                            }
                        }
                    }




                }
            }
            else
            {
                CurrentlySelected = null;
            }
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.GetComponent<WorldObject>() != null)
            {
                WorldObject obj = hit.collider.gameObject.GetComponent<WorldObject>();
                Debug.Log(obj);
                Debug.Log(obj.Data);
                if (obj.Data is IOnEntityInteract)
                {
                    Debug.Log("here2");
                    (obj.Data as IOnEntityInteract).OnEntityInteract(Player);
                }
            }
        }

        if (!entSelect)
            GameManager.DebugGUI.SetSelectedEntity(null);
    }


    
}
