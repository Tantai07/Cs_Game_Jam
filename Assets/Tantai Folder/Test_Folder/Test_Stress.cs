using UnityEngine;
using UnityEngine.EventSystems;

public class Test_Stress : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Player_Movement.Instance.AddStress(20);
    }
}
