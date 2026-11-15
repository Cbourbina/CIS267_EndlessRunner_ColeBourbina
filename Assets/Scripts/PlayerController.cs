using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D player_rb;
    [SerializeField]
    private float movementSpeed;
    private bool isMoving;
    [SerializeField]
    private float moveDistance;
    [SerializeField]
    private LayerMask collisions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        //movePlayer();
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (!isMoving)
        {
            float input = Input.GetAxisRaw("Horizontal");
            if (input != 0)
            {
                Vector2 targetPos = new Vector2(transform.position.x, -4) + ((new Vector2(input, 0) * moveDistance));
                Vector2 targetPosHalf = new Vector2(transform.position.x, -4) + ((new Vector2(input, 0) * (moveDistance / 2)));
                Vector2 originalPos = new Vector2(transform.position.x, -4);
                StartCoroutine(movePlayerLateral(targetPos, targetPosHalf, originalPos));
            }
        }
    }

    //private void movePlayer()
    //{
    //    float inputHorizontal = Input.GetAxisRaw("Horizontal");
    //    float inputVertical = Input.GetAxisRaw("Vertical");
    //    player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, (inputVertical * movementSpeed));
    //    player_rb.linearVelocity = new Vector2 ((inputHorizontal * movementSpeed), player_rb.linearVelocityY);
    //    //Debug.Log("Movement Speed: " + movementSpeed);
    //}

    private IEnumerator movePlayerLateral(Vector2 targetPosition, Vector2 halfTargetPosition, Vector2 originalPos)
    {
        isMoving = true;
        Collider2D collision = Physics2D.OverlapCircle(targetPosition, 0.4f, collisions);
        if (collision == null)
        {
            while (Vector2.Distance(transform.position, halfTargetPosition) > 0.001f)
            {
                collision = Physics2D.OverlapCircle(targetPosition, 0.4f, collisions);
                transform.position = Vector2.MoveTowards(transform.position, halfTargetPosition, (movementSpeed * Time.deltaTime));
                yield return null;
            }
        }
        if (collision != null)
        {
            while (Vector2.Distance(transform.position, halfTargetPosition) > 0.001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, halfTargetPosition, ((movementSpeed) * Time.deltaTime));
                yield return null;
            }
        }
        if (collision == null)
        {
            while (Vector2.Distance(transform.position, targetPosition) > 0.001f)
            {
                collision = Physics2D.OverlapCircle(targetPosition, 0.4f, collisions);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, (movementSpeed * Time.deltaTime));
                yield return null;
            }
            transform.position = targetPosition;
        }
        else 
        {
            while (Vector2.Distance(transform.position, originalPos) > 0.001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, originalPos, ((movementSpeed) * Time.deltaTime));
                yield return null;
            }
            transform.position = originalPos;
        }
        isMoving = false;
    }
}
