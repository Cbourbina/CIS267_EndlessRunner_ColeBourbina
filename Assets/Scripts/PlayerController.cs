using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D player_rb;
    [SerializeField]
    private float moveDuration;
    [SerializeField]
    private float returnDuration;
    [SerializeField]
    private float moveDistance;
    private enum MoveState { Idle, Moving, Returning}
    private MoveState moveState = MoveState.Idle;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moveState == MoveState.Idle)
        {
            getDirection();
        }
        else if (moveState == MoveState.Moving)
        {
            movePlayerLateral(startPosition, targetPosition, moveDuration, false);
        }
        else if (moveState == MoveState.Returning)
        {
            movePlayerLateral(startPosition, targetPosition, returnDuration, true);
        }
    }

    private void getDirection()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            startPosition = transform.position;
            targetPosition = startPosition + new Vector2(input * moveDistance, 0f);
            timer = 0f;
            moveState = MoveState.Moving;
        }
    }

    private void movePlayerLateral(Vector2 originalPosition, Vector2 targetPosition, float duration, bool isReturning)
    {
        timer += Time.deltaTime;
        float progress = (timer / duration);
        if (progress > 1)
        {
            progress = 1;
        }
        else if (progress < 0)
        {
            progress = 0;
        }

        float lerpPercent;

        if (!isReturning)
        {
            lerpPercent = progress;
        }
        else
        {
            lerpPercent = 1 - progress;
        }

        transform.position = Vector2.Lerp(originalPosition, targetPosition, lerpPercent);

        if (progress >= 1)
        {
            if (isReturning)
            {
                transform.position = originalPosition;
            }
            else
            {
                transform.position = targetPosition;
            }
            moveState = MoveState.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (moveState == MoveState.Moving)
        {
            if (collision.CompareTag("Vehicle") || collision.CompareTag("Boundary"))
            {
                moveState = MoveState.Returning;
                float progress = (timer / moveDuration);
                if (progress > 1)
                {
                    progress = 1;
                }
                else if (progress < 0)
                {
                    progress = 0;
                }

                timer = (1 - progress) * returnDuration;
            }
        }
    }
}
