using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AimAssist : MonoBehaviour
{
    Aim aim;
    public aimDirTargetFinder aimDirTargetFinder;
    public GameObject fovStartPoint;
    public GameObject aimAssistPoint;
    public float lookSpeed = 200;
    public float maxAngle = 45f;
    Quaternion targetRotation;
    Quaternion lookAt;
    [SerializeField] LayerMask layerMask;
    void Awake()
    {
        aim = GetComponent<Aim>();
    }
    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(aimAssistPoint.transform.position, aimAssistPoint.transform.TransformDirection(Vector3.forward));

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(aimAssistPoint.transform.position, aimAssistPoint.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.black);
            if (Input.GetButtonUp("Shoot") && hitInfo.collider.CompareTag("enemy"))
            {
                hitInfo.collider.gameObject.GetComponent<Target>().TakeDamage();
            }
        }
        if (aim.isShootPressed)
        {         
            if (EnemyInFieldOfView(fovStartPoint))
            {
                
                Vector3 direction = aimDirTargetFinder.closestCollider.transform.position - transform.position;
                targetRotation = Quaternion.LookRotation(direction);
                lookAt = Quaternion.RotateTowards(aimAssistPoint.transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
                aimAssistPoint.transform.rotation = lookAt;
            }
            
        }
    }
    bool EnemyInFieldOfView(GameObject looker)
    {
        Vector3 targetDir = aimDirTargetFinder.closestCollider.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, looker.transform.forward);
        if (angle < maxAngle)
            return true;
        else
            return false;
    }
}
