using UnityEngine;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Left, Right }

    [SerializeField] private GameObject objectToMove;
    [SerializeField] private Direction moveDirection = Direction.Right;
    [SerializeField] private float moveSpeed = 3.5f; // Units per second
    private bool isMoving = false;
    void Start()
    {
        if (objectToMove == null)
        {
            objectToMove = GameObject.FindGameObjectWithTag("basket");
        }
    }

    void Update()
{
    if (objectToMove == null)
    {
        objectToMove = GameObject.FindGameObjectWithTag("basket");
        return; // wait until next frame to move
    }

    if (isMoving)
    {
        Vector3 directionVector = moveDirection == Direction.Right ? Vector3.right : Vector3.left;
        objectToMove.transform.position += directionVector * moveSpeed * Time.deltaTime;
    }
}


    public void OnPointerDown(PointerEventData eventData)
    {
        isMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMoving = false;
    }
}