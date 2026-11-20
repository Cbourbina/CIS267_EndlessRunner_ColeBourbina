using System.Collections;
using System.ComponentModel.Design.Serialization;
using TMPro;
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
    private enum MoveState { Idle, Moving, Returning, Exploding}
    private MoveState moveState = MoveState.Idle;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float moveTimer;
    private float upgradeTimer;
    private float ghostTimer;
    private bool canOffRoad = false;
    private bool isGhosted = false;
    [SerializeField]
    private Sprite upgradedCar;
    [SerializeField]
    private Sprite playerCarDefault;
    private SpriteRenderer spriteRenderer;
    private bool isMoving = false;
    public Transform gameManager;
    private GameObject objectToSpawn;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject upgradeInfo;
    [SerializeField]
    private TMP_Text upgradeTimerDisplay;
    [SerializeField]
    private Sprite[] playerCarDamaged;
    [SerializeField]
    private GameObject ghostInfo;
    [SerializeField]
    private TMP_Text ghostTimerDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetComponent<GameManager>().getHealth() <= 0 && moveState != MoveState.Exploding)
        {
            gameManager.GetComponent<PlayerScore>().stopScore = true;
            moveState = MoveState.Exploding;
            objectToSpawn = Instantiate(explosion, this.transform.position, Quaternion.identity);
        }
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
                int timeRemaining = (int)(10 - upgradeTimer);
                upgradeTimerDisplay.text = timeRemaining.ToString();
                if (upgradeTimer >= 10)
                {
                    canOffRoad = false;
                    upgradeInfo.SetActive(false);
                    Debug.Log("Car Downgraded");
                    spriteRenderer.sprite = playerCarDefault;
                    if (transform.position.x < -3)
                    {
                        moveState = MoveState.Returning;
                        StartCoroutine(moveCellLateral(new Vector2(-4, -4)));
                    }
                    if (transform.position.x > 3)
                    {
                        moveState = MoveState.Returning;
                        StartCoroutine(moveCellLateral(new Vector2(4, -4)));
                    }
                    if (gameManager.GetComponent<GameManager>().getHealth() > 100)
                    {
                        gameManager.GetComponent<GameManager>().setHealth(100);
                    }
                }
            }
            if (isGhosted)
            {
                ghostTimer += Time.deltaTime;
                int timeRemaining = (int)(10 - ghostTimer);
                ghostTimerDisplay.text = timeRemaining.ToString();
                if (ghostTimer >= 10)
                {
                    isGhosted = false;
                    ghostInfo.SetActive(false);
                    Debug.Log("Car Unghosted");
                    Color spriteOpacity = spriteRenderer.color;
                    spriteOpacity.a = 1f;
                    spriteRenderer.color = spriteOpacity;
                    this.tag = "Player";

                }
            }
        }
        if (gameManager.GetComponent<GameManager>().getHealth() < 26 && !canOffRoad)
        {
            spriteRenderer.sprite = playerCarDamaged[2];
        }
        else if (gameManager.GetComponent<GameManager>().getHealth() < 51 && !canOffRoad)
        {
            spriteRenderer.sprite = playerCarDamaged[1];
        }
        else if (gameManager.GetComponent<GameManager>().getHealth() < 76 && !canOffRoad)
        {
            spriteRenderer.sprite = playerCarDamaged[0];
        }
        else if (!canOffRoad)
        {
            spriteRenderer.sprite = playerCarDefault;
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
        if (moveState == MoveState.Returning && collision.CompareTag("Vehicle"))
        {
            if (!isGhosted)
            {
                gameManager.GetComponent<GameManager>().setHealth(0);
            }
        }
        if (moveState == MoveState.Moving)
        {
            if (collision.CompareTag("Vehicle") || collision.CompareTag("Boundary"))
            {
                if (!isGhosted)
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
        gameManager.GetComponent<GameManager>().setHealth(200);
        upgradeInfo.SetActive(true);
    }

    public void ghostCar()
    {
        isGhosted = true;
        ghostTimer = 0f;
        Debug.Log("Car Ghosted");
        Color spriteOpacity = spriteRenderer.color;
        spriteOpacity.a = 0.5f;
        spriteRenderer.color = spriteOpacity;
        this.tag = "Ghost";
        ghostInfo.SetActive(true);
    }

    public void repairCar()
    {
        if (canOffRoad)
        {
            gameManager.GetComponent<GameManager>().setHealth(200);
        }
        else
        {
            gameManager.GetComponent<GameManager>().setHealth(100);
        }
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
