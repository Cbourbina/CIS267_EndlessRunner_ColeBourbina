using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D player_rb;
    [SerializeField]
    float movementSpeed;
    bool isMoving;
    [SerializeField]
    float moveDistance;

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
                Vector2 targetPos = (Vector2)transform.position + ((new Vector2(input, 0) * moveDistance));
                StartCoroutine(moveCellLateral(targetPos));
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

    private IEnumerator moveCellLateral(Vector2 targetPosition)
    {
        isMoving = true;

        while (Vector2.Distance(transform.position, targetPosition) > 0.001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, (movementSpeed * Time.deltaTime));
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
