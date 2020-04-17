using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldMapLocationButton : MonoBehaviour, IPointerClickHandler
{
    private WorldMapGUI Parent;
    public WorldMapLocation WorldMapLocation { get; private set; }
    public Text Text;
    public Image Image;

    private bool IsActive;

    public void SetLocation(WorldMapGUI parent, WorldMapLocation location)
    {
        Text.text = location.Name;
        GetComponent<RectTransform>().anchoredPosition = location.ChunkPosition.AsVector2()*4;
        Parent = parent;
        WorldMapLocation = location;
    }

    public void SetActiveQuestButton(bool set)
    {

        if (set)
        {
            Debug.Log("LOOOOL");
        }

        if (IsActive == set)
            return;
        if(set && !IsActive)
        {
            IsActive = set;
            Image.color = Color.yellow;
        }
        else
        {
            Image.color = Color.white;
            IsActive = set;
        }
    }


    public void OnLeftClick()
    {
        Parent.TeleportToLocation(WorldMapLocation);


    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }

}