using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Transfer : MonoBehaviour
{
    [Header("Start Point")]
    [SerializeField] Transform start_player;

    [Space(5)]
    [Header("Transfer Player")]
    public Transform Transfer_Player_To_Point;

    [Space(5)]
    [Header("Transition scene")]
    public GameObject Group_Transition;
    [SerializeField] Animator anim_Transition;

    [Space(5)]
    [Header("Type Optional")]
    public bool stair;
    public bool last_Room;

    [Space(5)]
    public float dealy = 1.2f;

    private void Start()
    {
        Group_Transition = GameObject.Find("Group_Transition");
        anim_Transition = Group_Transition.GetComponentInChildren<Animator>();

        start_player = GameObject.Find("First_Point_Player").transform;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player.transform.position != start_player.position)
            player.transform.position = start_player.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (stair)
        {
            if (Player_Movement.Instance.check_go_up)
            {
                anim_Transition.SetTrigger("Start");
                StartCoroutine(TransferScene(collision.gameObject));
            }
        }
        else if (last_Room)
        {
            if (InventoryManager.Instance.HasItem("Key"))
            {
                InventoryManager.Instance.RemoveItemByName("Key");
                anim_Transition.SetTrigger("Start");
                StartCoroutine(TransferScene(collision.gameObject));
            }
        }
        else
        {
            anim_Transition.SetTrigger("Start");
            StartCoroutine(TransferScene(collision.gameObject));
        }
    }


    private IEnumerator TransferScene(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        Player_Movement.Instance.canMove = false;
        yield return new WaitForSeconds(dealy);

        if (Transfer_Player_To_Point != null)
        {
            player.transform.position = Transfer_Player_To_Point.position;
        }

        yield return new WaitForSeconds(0.4f);

        anim_Transition.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        Player_Movement.Instance.canMove = true;

    }
}
