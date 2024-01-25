using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingScript : MonoBehaviour
{

    [SerializeField] private GameObject path;
    [SerializeField] private Transform startingWaypoint;
    [SerializeField] private Transform endWaypoint;
    private float pathingSpeed = 5f;
    private Transform[] totalPathArr;
    private Transform[] pathArr;
    private int waypointsIndex = 0;
    private float rotationSpeed = 150f;

    private void Start()
    {
        totalPathArr = path.GetComponentsInChildren<Transform>();
        int startingIndex = Array.IndexOf(totalPathArr, startingWaypoint);
        int endingIndex = Array.IndexOf(totalPathArr, endWaypoint);

        if (startingIndex < endingIndex)
        {
            int count = 0;
            pathArr = new Transform[endingIndex - startingIndex + 1];

            for (int i = startingIndex; i <= endingIndex; i++)
            {
                pathArr[count] = totalPathArr[i];
                count++;
            }
        }
        else
        {
            int count = 0;
            pathArr = new Transform[startingIndex - endingIndex + 1];

            for (int i = startingIndex; i >= endingIndex; i--)
            {
                pathArr[count] = totalPathArr[i];
                count++;
            }
        }

    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position == pathArr[waypointsIndex].position)
        {
            waypointsIndex++;
            if (waypointsIndex >= pathArr.Length)
            {
                Array.Reverse(pathArr);
                waypointsIndex = 0;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pathArr[waypointsIndex].position, pathingSpeed * Time.deltaTime);
            float angle = Mathf.Atan2(pathArr[waypointsIndex].position.y - transform.position.y, pathArr[waypointsIndex].position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void SetPathingSpeed(float speed)
    {
        pathingSpeed = speed;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
