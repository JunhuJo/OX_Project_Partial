using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;

public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent NavAgent_Player;
    [SerializeField] private Animator Animator_Player;
    [SerializeField] private TextMesh TextMesh_NetType;
    private Transform Transform_Player;

    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 100.0f;
    public float _moveSpeed = 4.0f;

    [SerializeField] private Vector3 defaultInitialPlanePosition = new Vector3(-9.16621f, 0.036054f, -66.13957f);
    private CinemachineVirtualCamera virtualCamera;
    private string MyObjectName;

    private void Start()
    {
        Animator_Player = GetComponent<Animator>();
    }

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

    [Command]
    private void CheckIsLocalPlayerOnUpdate()
    {
        TextMesh_NetType.text = this.isLocalPlayer ? "로컬" : "로컬 아님";

        if (isLocalPlayer == false)
            return;
        
        // 회전
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        // 이동
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.Move(forward * Mathf.Max(vertical, 0) * NavAgent_Player.speed * _moveSpeed * Time.deltaTime);

        if (horizontal == 0 || vertical == 0)
        {
            Animator_Player.SetBool("isRun", true);
        }
        else 
        {
            Animator_Player.SetBool("isRun", false);
        }

    }

    [ClientRpc]
    void RpcUpdateAnimation()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            if (transform.position != Vector3.zero)
            {
                Animator_Player.SetBool("isRun", false);
            }
            else 
            {
                Animator_Player.SetBool("isRun", true);
            }
        }
           
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (isLocalPlayer)
        {
            MyObjectName = gameObject.name;

            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                virtualCamera.Follow = transform;
            }
        }

    }
}