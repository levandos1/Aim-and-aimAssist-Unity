using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimDirTargetFinder : MonoBehaviour
{
    public Transform startPoint;
    public float detectionRadius = 10f;
    public LayerMask detectionLayer;

    private Collider[] colliders;
    public Collider closestCollider;

    private void Update()
    {
        colliders = Physics.OverlapSphere(startPoint.position, detectionRadius, detectionLayer);
        if (colliders.Length > 0)
        {
            closestCollider = colliders[0];

            float closestDistance = Vector3.Distance(startPoint.position, closestCollider.transform.position);

            for (int i = 1; i < colliders.Length; i++)
            {
                float distanceToCollider = Vector3.Distance(startPoint.position, colliders[i].transform.position);

                if (distanceToCollider < closestDistance)
                {
                    closestCollider = colliders[i];
                    closestDistance = distanceToCollider;

                }
            }
        }
    }
}
