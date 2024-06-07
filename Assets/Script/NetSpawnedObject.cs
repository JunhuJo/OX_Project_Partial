using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;

public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    public NavMeshAgent NavAgent_Player;
    public Animator Animator_Player;
    public Transform Transform_Player;

    [Header("Movement")]
    public float _rotationSpeed = 100.0f;
    public float _moveSpeed = 4.0f;



    [SerializeField] private Vector3 defaultInitialPlanePosition = new Vector3(-9.16621f, 0.036054f, -66.13957f); // Adjust as necessary
    private CinemachineVirtualCamera virtualCamera;
    private string MyObjectName;


    private void Update()
    {
        if (CheckIsFocusedOnUpdate() == false)
        {
            return;
        }

        CheckIsLocalPlayerOnUpdate();
    }

    private bool CheckIsFocusedOnUpdate()
    {
        return Application.isFocused;
    }

    private void CheckIsLocalPlayerOnUpdate()
    {
        if (isLocalPlayer == false)
            return;
        
        

        // 회전
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        // 이동
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.Move(forward * Mathf.Max(vertical, 0) * NavAgent_Player.speed * _moveSpeed * Time.deltaTime);

        if (horizontal != 0 || vertical != 0)
        {
            Animator_Player.SetBool("isRun", true);
        }
        else 
        {
            Animator_Player.SetBool("isRun", false);
        }
        //Animator_Player.SetBool("Moving", NavAgent_Player.velocity != Vector3.zero);

        //RotateLocalPlayer();
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Set the initial position of the player
        //transform.position = new Vector3(Random.Range(defaultInitialPlanePosition.x, defaultInitialPlanePosition.y), 1, -154);

        // Check if this is the local player
        if (isLocalPlayer)
        {
            // Get the name of the player object
            MyObjectName = gameObject.name;

            // Find the virtual camera in the scene
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                // Set the Follow and Look At fields of the virtual camera to the local player
                virtualCamera.Follow = transform;
                //virtualCamera.LookAt = transform;
            }
        }

    }

    //마우스 보는 방향으로 회전(이건안씀)
    void RotateLocalPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Vector3 lookRotate = new Vector3(hit.point.x, Transform_Player.position.y, hit.point.z);
            Transform_Player.LookAt(lookRotate);
        }
    }
}