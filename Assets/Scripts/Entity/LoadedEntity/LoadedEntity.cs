using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Collections;
public class LoadedEntity : MonoBehaviour, IGamePauseEvent
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

    //Used to check for entities, if Idle 
    public bool IsIdle { get; private set; }

    //private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    public List<Entity> NearEntities;


    void OnDrawGizmos()
    {
        Color prev = Gizmos.color;
        Color col = OnGround() ? Color.green : Color.red;
        Gizmos.color = col;
        Gizmos.DrawLine(transform.position+Vector3.up*1, transform.position - Vector3.up * (1+DistToGround + 0.1f));
        Gizmos.color = prev;
        if (Entity.EntityAI != null)
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
           // GameManager.DebugGUI.SetData("Entity_" + Entity.ID + "_near:", NearEntities.Count);
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
        return Physics.Raycast(transform.position+Vector3.one, -Vector3.up, 0.1f+1);
    }
    private bool IsJumping;
    private bool IsWaitingForJump;
    private bool IsFalling;
    private bool IsRunning;

    /// <summary>
    /// Causes the entity to jump. 
    /// Checks if the entity is grounded, and if
    /// the jump is possible we play the jump animation 
    /// </summary>
    public void Jump()
    {
        IsIdle = false;
        if (!OnGround() || IsJumping || IsWaitingForJump)
            return;
        IsWaitingForJump = true;
        StartCoroutine(WaitForJumpAnimation());
    }

    /// <summary>
    /// Internal method used for jumping.
    /// Waits for the Jump animation to play before setting variables
    /// such that jumping occurs
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForJumpAnimation()
    {
        AnimationManager.Jump();
        yield return new WaitForSeconds(AnimationManager.JumpAnimationTime);
        VerticalVelocity = 10;
        IsJumping = true;
        IsWaitingForJump = false;
    }

    /// <summary>
    /// Sets if the entity should currently be running or not
    /// </summary>
    /// <param name="running"></param>
    public void SetRunning(bool running)
    {
        IsIdle = false;

        IsRunning = running;
    }



    private System.Diagnostics.Stopwatch Stopwatch;
    


    /// <summary>
    /// Causes entity to look towards the specified point
    /// </summary>
    /// <param name="v">Point of interest to face towards</param>
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
    /// Physics update for the entity
    /// 
    /// </summary>
    private void FixedUpdate() {

        //RigidBody.angularVelocity = Vector3.zero;
        if (GameManager.Paused)
            return;
        if (IsIdle)
        {

            return;
        }
        Debug.BeginDeepProfile("le_fixed_update");

        if(!(Entity is Player))
        {
            DebugGUI.Instance.SetData("ent", GameManager.Paused + "_" + IsIdle + "_" + MoveDirection);
            if (MiscMaths.DistanceSqr(transform.position, GameManager.PlayerManager.Player.Position) > (World.ChunkSize * 3) * (World.ChunkSize * 3))
            {
                IsIdle = true;
            }
            
        }

        EntityHealthBar.SetHealthPct(Entity.CombatManager.CurrentHealth / Entity.CombatManager.MaxHealth);

        if (transform.position.y < 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Vec2i cPos = World.GetChunkPosition(transform.position);

        //Check if the current chunk is loaded
        if (!GameManager.WorldManager.CRManager.IsCurrentChunkPositionLoaded(cPos))
        {
            //If not, keep position dixed with +ve y value, to prevent falling through world.
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }



        //If we have a specified move direction
        if (MoveDirection != Vector2.zero)
        {
            float tileSpeed = 1; //TODO - get tile speed
            //Finds correct speed associated with running/walking for this entity
            float entitySpeed = IsRunning?Entity.MovementData.RunSpeed:Entity.MovementData.WalkSpeed;

            AnimationManager.SetSpeedPercentage(entitySpeed / Entity.MovementData.RunSpeed);

            Vector2 v2Pos = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetDisp = TargetPosition - v2Pos;
            float targetMag = targetDisp.magnitude;
            Vector3 vel = new Vector3(MoveDirection.x, 0, MoveDirection.y) * tileSpeed * entitySpeed;
            RigidBody.velocity = new Vector3(MoveDirection.x, 0, MoveDirection.y) * tileSpeed * entitySpeed;

            //If the distance the body will move in a frame (|velocity|*dt) is more than the desired amount (target mag)
            //Then we must scale the velocity down
            if(RigidBody.velocity.magnitude*Time.fixedDeltaTime > targetMag)
            {

               RigidBody.velocity = RigidBody.velocity * targetMag/(Time.fixedDeltaTime* RigidBody.velocity.magnitude);
            }


            if (Entity is Player)
            {
                DebugGUI.Instance.SetData("Vel", RigidBody.velocity + " | " + vel);
            }

        }
        else
        {
            AnimationManager.SetSpeedPercentage(0);
        }
        //If we have a specified look direction
        if (LookTowards != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, LookTowards - transform.position, Vector3.up);
            Quaternion quat = Quaternion.Euler(new Vector3(0, angle, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.fixedDeltaTime * LOOK_ROTATION_SPEED);
            Entity.SetLookAngle(transform.rotation.eulerAngles.y);
        }

        //Check if we are on the ground
        bool IsOnGround = OnGround();

        //Debug info for player Jumping
        if(Entity is Player)
        {
            DebugGUI.Instance.SetData("jump_vel", VerticalVelocity);
            DebugGUI.Instance.SetData("onGround", IsOnGround);
            DebugGUI.Instance.SetData("isJumping", IsJumping);
            DebugGUI.Instance.SetData("isFalling", IsFalling);

        }

        //Check if we are off the ground
        if (!IsOnGround)
        {
            //If so, update vertical position and velocity accordingly
            VerticalVelocity -= 9.81f * Time.fixedDeltaTime;
            transform.position += Vector3.up * VerticalVelocity * Time.fixedDeltaTime;
            //Next, if we are off the ground but not jumping, it must mean we're falling
            if (!IsJumping)
            {
                //If not currently falling, set it to true
                if (!IsFalling)
                {
                    IsFalling = true;
                    AnimationManager.SetFalling();
                }
            }
        }
        else if (VerticalVelocity > 0) {
            //If we are on the ground, but our velocity is positive, we must be jumping
            VerticalVelocity -= 9.81f * Time.fixedDeltaTime;
            transform.position += Vector3.up * VerticalVelocity * Time.fixedDeltaTime;

        }
        else if ((IsJumping || IsFalling) && !IsWaitingForJump)
        {
            //If we are on the ground, but still jumping or falling, set both to false. reset velocity
            IsJumping = false;
            VerticalVelocity = 0;
            IsFalling = false;
            //Then play the land animation
            AnimationManager.LandJump();
        }

        //Reset variables 
        LookTowards = Vector3.zero;
        MoveDirection = Vector2.zero;
        Entity.SetPosition(transform.position);

        Debug.EndDeepProfile("le_fixed_update");

        return;


     
    }


    public void GamePauseEvent(bool pause)
    {
      
    }
}