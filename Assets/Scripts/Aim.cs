using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] public bool isGamePad;
    [SerializeField] LayerMask layerMask;
    public Camera mainCam;
    Vector2 aim;
    public LineRenderer laserLine;
    //input action asset
    PlayerControls playerControls;
    //component on aim 
    PlayerInput playerInput;
    
    public GameObject mouseCursor;
    public GameObject player;
    public bool isShootPressed = false;
    public GameObject aimDirTargetFinder;
    Ray ray;
    void Awake()
    {
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        playerControls.Character.Shoot.started += onShoot;
        playerControls.Character.Shoot.canceled += onShoot;
        playerControls.Character.Shoot.performed += onShoot;
    }
    void Update()
    {
        
        HandleInput();
        HandleRotation();
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, ray.GetPoint(7));
        aimDirTargetFinder.transform.position = laserLine.GetPosition(1);
        laserLine.transform.position = player.transform.position;
    }
    void HandleInput()
    {
        aim = playerControls.Character.Aim.ReadValue<Vector2>();
    }
    void HandleRotation()
    {
        
        RaycastHit hitInfo;
        ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
            
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.green);
        }

        if (isGamePad)
        {
            mouseCursor.SetActive(false);

            if (Mathf.Abs(aim.x) > controllerDeadZone || Mathf.Abs(aim.y) > controllerDeadZone)
            {
                Vector3 aimDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
                if (aimDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, gamepadRotateSmoothing * Time.deltaTime);
                }
            }

        }
        else
        {
            mouseCursor.SetActive(true);
            Vector3 direction = mouseCursor.transform.position - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
        
    }
    void onShoot(InputAction.CallbackContext context)
    {
        isShootPressed = context.ReadValueAsButton();
    }
    public void OnDeviceChange(PlayerInput pi)
    {
        isGamePad = pi.currentControlScheme.Equals("GamePad") ? true : false;
    }
    void OnEnable()
    {
        playerControls.Character.Enable();
    }
    void OnDisable()
    {
        playerControls.Character.Disable();
    }
}
