using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D player_rb;
    private float movementSpeed = 5;
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
    private float moveTimer;
    private float upgradeTimer;
    private bool canOffRoad = false;
    [SerializeField]
    private Sprite upgradedCar;
    [SerializeField]
    private Sprite playerCarDefault;
    private SpriteRenderer spriteRenderer;
    private bool isMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
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
            if (canOffRoad)
            {
                upgradeTimer += Time.deltaTime;
                if (upgradeTimer >= 10)
                {
                    canOffRoad = false;
                    Debug.Log("Car Downgraded");
                    spriteRenderer.sprite = playerCarDefault;
                    if (transform.position.x < -3)
                    {
                        moveState = MoveState.Moving;
                        StartCoroutine(moveCellLateral(new Vector2(-4, -4)));
                    }
                    if (transform.position.x > 3)
                    {
                        moveState = MoveState.Moving;
                        StartCoroutine(moveCellLateral(new Vector2(4, -4)));
                    }
                }
            }
        }
    }

    private void getDirection()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            startPosition = transform.position;
            targetPosition = startPosition + new Vector2(input * moveDistance, 0f);
            moveTimer = 0f;
            moveState = MoveState.Moving;
        }
    }

    private void movePlayerLateral(Vector2 originalPosition, Vector2 targetPosition, float duration, bool isReturning)
    {
        moveTimer += Time.deltaTime;
        float progress = (moveTimer / duration);
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
                float progress = (moveTimer / moveDuration);
                if (progress > 1)
                {
                    progress = 1;
                }
                else if (progress < 0)
                {
                    progress = 0;
                }

                moveTimer = (1 - progress) * returnDuration;
            }
            if (!canOffRoad && collision.CompareTag("OffRoad"))
            {
                moveState = MoveState.Returning;
                float progress = (moveTimer / moveDuration);
                if (progress > 1)
                {
                    progress = 1;
                }
                else if (progress < 0)
                {
                    progress = 0;
                }

                moveTimer = (1 - progress) * returnDuration;
            }
        }
    }

    public void upgradeCar()
    {
        canOffRoad = true;
        upgradeTimer = 0f;
        Debug.Log("Car Upgraded");
        spriteRenderer.sprite = upgradedCar;
    }

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
        moveState = MoveState.Idle;
    }
}
