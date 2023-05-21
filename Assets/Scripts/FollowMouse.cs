using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public Camera mainCam;
    Vector3 screenPos;
    Vector3 worldPos;
    void Update()
    {
        screenPos = Input.mousePosition;
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        if(Physics.Raycast(ray, out RaycastHit hitData))
        {
            worldPos = hitData.point;
        }
        transform.position = worldPos;
    }
}
