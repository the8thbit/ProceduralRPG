using UnityEngine;
using UnityEditor;

/// <summary>

/// </summary>
public class LoadedEntityAnimationManager : MonoBehaviour, IGamePauseEvent
{

    protected LoadedEntity LoadedEntity;
    protected Animator Animator;
    protected virtual void Start()
    {
        LoadedEntity = GetComponent<LoadedEntity>();
        Animator = GetComponent<Animator>();
        EventManager.Instance.AddListener(this);
    }


    protected virtual void Update()
    {
        //float velocity = LoadedEntity.GetRigidBody().velocity.magnitude;
    }


    /// <summary>
    /// Triggers the animator to call attack left.
    /// </summary>
    public virtual void AttackLeft()
    {

    }
    /// <summary>
    /// Triggers the animator to call attack right.
    /// </summary>
    public virtual void AttackRight()
    {

    }

    public void GamePauseEvent(bool pause)
    {

        Animator.enabled = !pause;
    }


    public LoadedHumanoidAnimatorManager HumanoidCast()
    {
        return this as LoadedHumanoidAnimatorManager;
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener(this);
    }

}