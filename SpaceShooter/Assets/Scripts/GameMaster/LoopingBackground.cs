using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [SerializeField] private List<GameObject> backgroundElements;
    [SerializeField] private float loopSpeed = 10;
    [SerializeField] private float travelDistance = 4;

    private float distanceTraveled = 0;
    private Vector3 startPosition;
    private int numberOfBackgroundElements;
    private int objectToMove = 0;

    private void Awake()
    {
        numberOfBackgroundElements = backgroundElements.Count - 1;
        startPosition = backgroundElements[numberOfBackgroundElements].transform.position;
    }

    private void Update()
    {
        if(distanceTraveled >= travelDistance)
        {
            backgroundElements[objectToMove % backgroundElements.Count].transform.position = startPosition;
            distanceTraveled = 0;
            objectToMove ++;
        }
        foreach(GameObject obj in backgroundElements)
        {
            distanceTraveled += loopSpeed * GameVariables.GameTime;
            obj.transform.position += obj.transform.forward * (loopSpeed * GameVariables.GameTime);
        }



    }
}
