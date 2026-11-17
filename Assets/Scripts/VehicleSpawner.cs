using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject car;
    public GameObject bigRig;
    public GameObject truck;
    public GameObject motorcycle;
    public GameObject coin;
    public GameObject wrench;
    public GameObject upgrade;
    public GameObject[] spawnLocations;
    private GameObject objectToSpawn;
    [SerializeField]
    private float[] laneSpeeds;
    private int openLane = 2;
    private float playerY = -4;
    [SerializeField]
    private float playerMoveSpeed;
    [SerializeField]
    private float playerMoveDistance;
    private float playerMoveDuration;
    [SerializeField]
    private float buffer;
    [SerializeField]
    private int maxVehicleSpawn;
    private float arrivalTime;
    private float minWaveTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //The time it takes the player to switch lanes
        playerMoveDuration = playerMoveDistance / playerMoveSpeed;
        //The minimum time required for a player to be able to move for each wave
        minWaveTime = playerMoveDuration + buffer;

        //Distance between spawner and player
        float distance = spawnLocations[0].transform.position.y - playerY;
        //Longest travel time from object spawn to player from slowest lane
        float maxTravelTime = distance / laneSpeeds[0];

        //Time when all spawned objects should arrive at player
        arrivalTime = Time.time + maxTravelTime + 0.5f;

        StartCoroutine(planWaves(arrivalTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator planWaves(float arrivalTime)
    {
        while (true)
        {
            //Makes sure the coroutine only plans 5 seconds ahead
            if (arrivalTime - Time.time < 5f)
            {
                scheduleWave(arrivalTime);
                arrivalTime += minWaveTime;
            }
            yield return null;
        }
    }

    private void scheduleWave(float arrivalTime)
    {
        int randomNum;
        bool changeLane = false;
        //Random num to decide if next open lane should shift left, right, or stay the same
        int openLaneChange = UnityEngine.Random.Range(-1, 2);
        if ((openLane + openLaneChange) > -1 && (openLane + openLaneChange) < 5)
        {
            openLane += openLaneChange;
        }

        //List of lanes that an object is allowed to spawn in
        List<int> lanesToSpawn = new List<int> { 0, 1, 2, 3, 4 };
        lanesToSpawn.Remove(openLane);

        //Shuffle lanes for randomness
        for (int i = 0; i < lanesToSpawn.Count; i++)
        {
            int temp = lanesToSpawn[i];
            randomNum = UnityEngine.Random.Range(i, lanesToSpawn.Count);
            lanesToSpawn[i] = lanesToSpawn[randomNum];
            lanesToSpawn[randomNum] = temp;
        }

        randomNum = UnityEngine.Random.Range(1, 6);
        if (randomNum == 1)
        {
            changeLane = true;
        }

        //Determine the total number of vehicles to spawn across all open lanes
        int vehicleCount = UnityEngine.Random.Range(1, (maxVehicleSpawn + 1));
        //Schedule the vehicles to spawn in specific lanes at specific times
        for (int i = 0; i < vehicleCount; i++)
        {
            int lane = lanesToSpawn[i];
            if (changeLane == true)
            {
                if ((lane + 1) == openLane)
                {
                    StartCoroutine(spawnAtTime(lane, arrivalTime, false, true));
                }
                else if ((lane - 1) == openLane)
                {
                    StartCoroutine(spawnAtTime(lane, arrivalTime, true, false));
                }
                changeLane = false;
            }
            else
            {
                StartCoroutine(spawnAtTime(lane, arrivalTime, false, false));
            }
        }
        randomNum = UnityEngine.Random.Range(1, 6);
        if (randomNum < 4)
        {
            StartCoroutine(spawnColectableAtTime(openLane, arrivalTime));
        }
    }

    private IEnumerator spawnAtTime(int lane, float arrivalTime, bool changeLaneLeft, bool changeLaneRight)
    {
        //Distance between spawner and player
        float distance = spawnLocations[lane].transform.position.y - playerY;
        //Time for spawned object to reach player
        float travelTime = distance / laneSpeeds[lane];
        //Time for object to spawn to reach player at the specific time
        float spawnTime = arrivalTime - travelTime;
        //Length of time before object spawns 
        float waitDuration = spawnTime - Time.time;

        if (waitDuration > 0)
        {
            //This ensures the code after the yield will not execute until the wait time is passed and the object is ready to spawn
            yield return new WaitForSeconds(waitDuration);
        }

        int randomNum = UnityEngine.Random.Range(1, 11);
        if (lane > 2)
        {
            if (randomNum <= 5)
            {
                objectToSpawn = Instantiate(bigRig, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<BigRig>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<BigRig>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<BigRig>().setChangeLaneRight(changeLaneRight);
            }
            else if (randomNum < 8)
            {
                objectToSpawn = Instantiate(truck, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<Truck>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<Truck>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<Truck>().setChangeLaneRight(changeLaneRight);
            }
            else if (randomNum < 10)
            {
                objectToSpawn = Instantiate(car, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<Car>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<Car>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<Car>().setChangeLaneRight(changeLaneRight);
            }
            else
            {
                objectToSpawn = Instantiate(motorcycle, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<Motorcycle>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<Motorcycle>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<Motorcycle>().setChangeLaneRight(changeLaneRight);
            }
        }
        else
        {            
            if (randomNum < 5)
            {
                objectToSpawn = Instantiate(truck, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<Truck>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<Truck>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<Truck>().setChangeLaneRight(changeLaneRight);
            }
            else if (randomNum < 10)
            {
                objectToSpawn = Instantiate(car, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<Car>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<Car>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<Car>().setChangeLaneRight(changeLaneRight);
            }
            else
            {
                objectToSpawn = Instantiate(motorcycle, spawnLocations[lane].transform.position, Quaternion.identity);
                objectToSpawn.GetComponent<Motorcycle>().setVehicleSpeed(laneSpeeds[lane]);
                objectToSpawn.GetComponent<Motorcycle>().setChangeLaneLeft(changeLaneLeft);
                objectToSpawn.GetComponent<Motorcycle>().setChangeLaneRight(changeLaneRight);
            }
        }
    }

    private IEnumerator spawnColectableAtTime(int openLane, float arrivalTime)
    {
        //Distance between spawner and player
        float distance = spawnLocations[openLane].transform.position.y - playerY;
        //Time for spawned object to reach player
        float travelTime = distance / laneSpeeds[openLane];
        //Time for object to spawn to reach player at the specific time
        float spawnTime = arrivalTime - travelTime;
        //Length of time before object spawns 
        float waitDuration = spawnTime - Time.time;

        if (waitDuration > 0)
        {
            //This ensures the code after the yield will not execute until the wait time is passed and the object is ready to spawn
            yield return new WaitForSeconds(waitDuration);
        }
        int randomNum = UnityEngine.Random.Range(1, 101);
        if (randomNum < 91)
        {
            objectToSpawn = Instantiate(coin, spawnLocations[openLane].transform.position, Quaternion.identity);
            Debug.Log("Coin Spawned");
            objectToSpawn.GetComponent<Coin>().setSpeed(laneSpeeds[openLane]);
        }
        else if (randomNum < 96)
        {
            objectToSpawn = Instantiate(wrench, spawnLocations[openLane].transform.position, Quaternion.identity);
            Debug.Log("Wrench Spawned");
        }
        else
        {
            objectToSpawn = Instantiate(upgrade, spawnLocations[openLane].transform.position, Quaternion.identity);
            Debug.Log("Upgrade Spawned");
            objectToSpawn.GetComponent<Upgrade>().setSpeed(laneSpeeds[openLane]);
        }
    }
}
