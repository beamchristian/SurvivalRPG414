using UnityEngine;
using System.Collections.Generic;

public class WaypointPathDrawer : MonoBehaviour
{
    public Color pathColor = Color.red;
    public float sphereRadius = 2f;
    private List<Transform> waypoints = new List<Transform>();

    private void OnDrawGizmos()
    {
        UpdateWaypointsList();

        if (waypoints.Count < 2) return;

        Gizmos.color = pathColor;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawSphere(waypoints[i].position, sphereRadius);

                if (i < waypoints.Count - 1 && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else if (i == waypoints.Count - 1 && waypoints[0] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
    }

    private void UpdateWaypointsList()
    {
        waypoints.Clear();

        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }
}
