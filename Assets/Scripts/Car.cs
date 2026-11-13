using UnityEngine;

public class Car : MonoBehaviour
{
    private float vehicleSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * vehicleSpeed * Time.deltaTime);
    }

    public void setVehicleSpeed(float speed)
    {
        vehicleSpeed = speed;
    }
}
