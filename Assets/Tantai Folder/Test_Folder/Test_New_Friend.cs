using UnityEngine;
using UnityEngine.EventSystems;

public class Test_New_Friend : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Player_Movement.Instance.FindFriend();
    }
}
