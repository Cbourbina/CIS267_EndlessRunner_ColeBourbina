using System.Collections;
using UnityEngine;

public class Motorcycle : MonoBehaviour
{
    private float vehicleSpeed;
    private bool changeLaneLeft;
    private bool changeLaneRight;
    private bool isMoving;
    Vector2 targetPos;
    Vector2 newPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
        if (changeLaneLeft)
        {
            targetPos = (Vector2)transform.position + ((new Vector2(-1, 0) * 2));
        }
        else if (changeLaneRight)
        {
            targetPos = (Vector2)transform.position + ((new Vector2(1, 0) * 2));
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * vehicleSpeed * Time.deltaTime);
        if (!isMoving && transform.position.y <= 2)
        {
            if (changeLaneLeft || changeLaneRight)
            {
                newPos = transform.position;
                StartCoroutine(changeLane(targetPos));
                Debug.Log("Lane Changing, Left: " + changeLaneLeft + ", Right: " + changeLaneRight);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                isMoving = true;
            }
        }
        newPos = transform.position;
    }

    public void setVehicleSpeed(float speed)
    {
        vehicleSpeed = speed;
    }

    private IEnumerator changeLane(Vector2 targetPosition)
    {
        Debug.Log("Lane Changing, Left: " + changeLaneLeft + ", Right: " + changeLaneRight);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.001f)
        {
            newPos.x = Mathf.MoveTowards(transform.position.x, targetPosition.x, (vehicleSpeed * Time.deltaTime));
            transform.position = newPos;
            yield return null;
        }

        //transform.position = targetPosition;
        if (changeLaneLeft == true)
        {
            changeLaneLeft = false;
        }
        if (changeLaneRight == true)
        {
            changeLaneRight = false;
        }
    }

    public void setChangeLaneLeft(bool changeLeft)
    {
        changeLaneLeft = changeLeft;
    }

    public void setChangeLaneRight(bool changeRight)
    {
        changeLaneRight = changeRight;
    }

    public void honkHorn()
    {
    }

    private void OnTriggerEnter2D(Collider2D triggerCollider)
    {
        // Calculate distance and surface normal between the normal collider and the trigger
        ColliderDistance2D distanceInfo = Physics2D.Distance(GetComponent<BoxCollider2D>(), triggerCollider);

        // distanceInfo.normal points outward from myNormalCollider toward the trigger
        Vector2 hitSideNormal = distanceInfo.normal;

        // Check which side of the normal collider was touched
        if (hitSideNormal.y > 0.5f)
        {
            Debug.Log("Trigger touched the TOP side!");
        }
        else if (hitSideNormal.y < -0.5f)
        {
            Debug.Log("Trigger touched the BOTTOM side!");
            Time.timeScale = 0f;
        }
        else if (hitSideNormal.x > 0.5f)
        {
            Debug.Log("Trigger touched the RIGHT side!");
        }
        else if (hitSideNormal.x < -0.5f)
        {
            Debug.Log("Trigger touched the LEFT side!");
        }
    }
}