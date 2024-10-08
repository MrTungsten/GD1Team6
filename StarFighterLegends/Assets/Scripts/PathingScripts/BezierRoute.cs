using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierRoute : MonoBehaviour
{

    [SerializeField] private Transform[] controlWaypoints;
    private Vector2 gizmosPosition;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlWaypoints[0].position + 
            3 * Mathf.Pow(1 - t, 2) * t * controlWaypoints[1].position + 
            3 * (1 - t) * Mathf.Pow(t, 2) * controlWaypoints[2].position + 
            Mathf.Pow(t, 3) * controlWaypoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(controlWaypoints[0].position.x, controlWaypoints[0].position.y),
            new Vector2(controlWaypoints[1].position.x, controlWaypoints[1].position.y));

        Gizmos.DrawLine(new Vector2(controlWaypoints[2].position.x, controlWaypoints[2].position.y),
            new Vector2(controlWaypoints[3].position.x, controlWaypoints[3].position.y));
    }

}