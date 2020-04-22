using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTestScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }


    private Player player;
    private void Awake()
    {
        ResourceManager.LoadAllResources();

        EventManager.Instance = new EventManager();

        player = new Player();
        PlayerManager.SetPlayer(player);

        LoadedEntity.SetEntity(player);
    }

    public PlayerManager PlayerManager;
    public LoadedEntity LoadedEntity;

    public LoadedHumanoidAnimatorManager HumanAni;

    // Update is called once per frame
    void Update()
    {

    }
}
