using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using System.Collections.Generic;
public class LoadedEntity : MonoBehaviour, INewDamageRegionEvent, IGamePauseEvent
{


   

    public static readonly float LOOK_ROTATION_SPEED = 5;
    public Entity Entity { get; private set; }
    public LoadedEntityAnimationManager AnimationManager { get; private set; }
    public Rigidbody RigidBody { get; private set; }

    private float VerticalVelocity;

    private EntityHealthBar EntityHealthBar;
    private Collider Collider;
    //private float targetRotation;
    private float DistToGround;

    private Vector3 LookTowards;

    private Vector2 MoveDirection;
    private Vector2 TargetPosition;

    //public WeaponController WeaponController { get; private set; }


    public bool IsIdle { get; private set; }

    //private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    public List<Entity> NearEntities;


    void OnDrawGizmos()
    {

        if(Entity.EntityAI != null)
        {
            if(Entity.EntityAI.EntityPath != null)
            {
                foreach(Vec2i v in Entity.EntityAI.EntityPath.Path)
                {
                    Gizmos.DrawCube(v.AsVector3(), Vector3.one * 0.2f);
                }
            }
        }

        if(NearEntities != null)
        {
            GameManager.DebugGUI.SetData("Entity_" + Entity.ID + "_near:", NearEntities.Count);
            foreach(Entity e in NearEntities)
            {
                if (e.Equals(Entity))
                    continue;
                Color c = Color.red;
                if (Entity.EntityAI.CombatAI.LineOfSight(e))
                {
                    c = Color.green;
                }
                Color oldC = Gizmos.color;
                Gizmos.color = c;
                Gizmos.DrawLine(Entity.Position, e.Position);
                Gizmos.color = oldC;               
  

            }
        }
    }

    public void SetEntity(Entity entity)
    {
    
        EventManager.Instance.AddListener(this);
        
        Entity = entity;
        AnimationManager = GetComponent<LoadedEntityAnimationManager>();

        DistToGround = 0;
        transform.position = new Vector3(entity.Position.x, transform.position.y, entity.Position.z);
        RigidBody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();


        GameObject entityhealthBar = Instantiate(ResourceManager.GetEntityGameObject("healthbar"));
        entityhealthBar.transform.parent = transform;
        entityhealthBar.transform.localPosition = new Vector3(0, 2, 0);

        EntityHealthBar = entityhealthBar.GetComponentInChildren<EntityHealthBar>();

       // AnimControll = GetComponentInChildren<AnimatorController>();

    }



    public void ResetPhysics()
    {
        VerticalVelocity = 0;

    }

    public void Idle()
    {
        IsIdle = true;
    }

    public bool OnGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, DistToGround + 0.1f);
    }
    private bool jump_;
    public void Jump()
    {
        if (!jump_)
            jump_ = true;
        IsIdle = false;
        if (OnGround())
        {
            VerticalVelocity = 20f;
        }
        else
        {

        }
    }

    private System.Diagnostics.Stopwatch Stopwatch;
    



    public void LookTowardsPoint(Vector3 v)
    {
        IsIdle = false;
        LookTowards = v;
    }
    public void SetLookBasedOnMovement(bool onMovement)
    {
        IsIdle = false;
    }

    public void MoveTowards(Vector3 position, bool chosenPoint=false)
    {
        

        Vector3 delta = position - transform.position;
        delta.Normalize();
        MoveDirection.x = delta.x;
        MoveDirection.y = delta.z;
        if (chosenPoint)
            TargetPosition = new Vector2(position.x, position.z);
        else
            TargetPosition = new Vector2(delta.x, delta.z) * 20f;
        IsIdle = false;
    }
    
    public void MoveTowards(Vector2 position)
    {
        MoveTowards(new Vector3(position.x, transform.position.y, position.y), true);
    }

    public void MoveInDirection(Vector2 direction)
    {
        MoveTowards(transform.position + new Vector3(direction.x, 0, direction.y).normalized);
    }
    /// <summary>
    /// Calculates the movement and rotation of the entity.
    /// </summary>
    private void FixedUpdate() {

        EntityHealthBar.SetHealthPct(Entity.CombatManager.CurrentHealth / Entity.CombatManager.MaxHealth);

        if (transform.position.y < 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Vec2i cPos = World.GetChunkPosition(transform.position);
        if (GameManager.WorldManager.InSubworld)
        {
            if (!GameManager.WorldManager.CRManager.SubworldChunks.ContainsKey(cPos))
            {
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                return;
            }
        }else if (!GameManager.WorldManager.CRManager.LoadedChunks.ContainsKey(cPos))
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            return;
        }

        RigidBody.angularVelocity = Vector3.zero;
        if (GameManager.Paused)
            return;
        if (IsIdle)
        {

            return;
        }

        if (MoveDirection != Vector2.zero)
        {


            float tileSpeed = 1; //TODO - get tile speed
            float entitySpeed = 20; //TODO - get entity speed

            Vector2 v2Pos = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetDisp = TargetPosition - v2Pos;
            float targetMag = targetDisp.magnitude;


            RigidBody.velocity = new Vector3(MoveDirection.x, 0, MoveDirection.y) * tileSpeed * entitySpeed;
            //If the distance the body will move in a frame (|velocity|*dt) is more than the desired amount (target mag)
            //Then we must scale the velocity down
            if(RigidBody.velocity.magnitude*Time.fixedDeltaTime > targetMag)
            {

                RigidBody.velocity = RigidBody.velocity * targetMag/(Time.fixedDeltaTime* RigidBody.velocity.magnitude);
            }

        }
        else
        {

        }
        if (LookTowards != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, LookTowards - transform.position, Vector3.up);
            Quaternion quat = Quaternion.Euler(new Vector3(0, angle, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.fixedDeltaTime * LOOK_ROTATION_SPEED);
            Entity.SetLookAngle(transform.rotation.eulerAngles.y);
        }
        LookTowards = Vector3.zero;
        MoveDirection = Vector2.zero;
        Entity.SetPosition(transform.position);
        return;


     
    }


    public void NewDamageRegionEvent(NewDamageRegion ev)
    {
        if (ev.Cause == Entity)
            return;
        if (ev.DamageRegion.CauseDamage(Collider))
        {
            Entity.CombatManager.DealDamageRegion(ev);
        }
        else
        {
            //Debug.Log("nope");
        }
    }

    public void GamePauseEvent(bool pause)
    {
      
    }
}