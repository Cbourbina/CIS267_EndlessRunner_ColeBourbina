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
        //Random num to decide if next open lane should shift left, right, or stay the same
        int laneChange = UnityEngine.Random.Range(-1, 2);
        if ((openLane + laneChange) > -1 && (openLane + laneChange) < 5)
        {
            openLane += laneChange;
        }

        //List of lanes that an object is allowed to spawn in
        List<int> lanesToSpawn = new List<int> { 0, 1, 2, 3, 4 };
        lanesToSpawn.Remove(openLane);

        //Shuffle lanes for randomness
        for (int i = 0; i < lanesToSpawn.Count; i++)
        {
            int temp = lanesToSpawn[i];
            int randomNum = UnityEngine.Random.Range(i, lanesToSpawn.Count);
            lanesToSpawn[i] = lanesToSpawn[randomNum];
            lanesToSpawn[randomNum] = temp;
        }

        //Determine the total number of vehicles to spawn across all open lanes
        int vehicleCount = UnityEngine.Random.Range(1, (maxVehicleSpawn + 1));
        //Schedule the vehicles to spawn in specific lanes at specific times
        for (int i = 0; i < vehicleCount; i++)
        {
            int lane = lanesToSpawn[i];
            StartCoroutine(spawnAtTime(lane, arrivalTime));
        }
    }

    private IEnumerator spawnAtTime(int lane, float arrivalTime)
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

        objectToSpawn = Instantiate(car, spawnLocations[lane].transform.position, Quaternion.identity);
        objectToSpawn.GetComponent<Car>().setVehicleSpeed(laneSpeeds[lane]);
    }
}
