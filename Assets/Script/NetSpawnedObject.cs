using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;

public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent NavAgent_Player;
    [SerializeField] private Animator Animator_Player;
    [SerializeField] private TextMesh TextMesh_Nickname;
    private Transform Transform_Player;

    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 100.0f;
    public float _moveSpeed = 4.0f;

    [SerializeField] private Vector3 defaultInitialPlanePosition = new Vector3(-9.16621f, 0.036054f, -66.13957f);
    private CinemachineVirtualCamera virtualCamera;
    private string MyObjectName;

    [SyncVar(hook = nameof(OnNicknameChanged))]
    private string playerNickname;


    //문제 정답 검증을 위한 변수 -> 1. 정답, 2.오답
    public int QustionValue = 0;
    public int GetPoint = 0;

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

    
    private void CheckIsLocalPlayerOnUpdate()
    {
        //TextMesh_NetType.text = this.isLocalPlayer ? "로컬" : "로컬 아님";

        if (isLocalPlayer == false)
            return;
        
        // 회전
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        // 이동
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.Move(forward * Mathf.Max(vertical, 0) * NavAgent_Player.speed * _moveSpeed * Time.deltaTime);

        bool isMoving = horizontal != 0 || vertical != 0;
        Animator_Player.SetBool("isRun", isMoving);

        // 애니메이션 상태를 서버에 전송
        CmdUpdateAnimation(isMoving);

    }

    [Command]
    void CmdUpdateAnimation(bool isMoving)
    {
        // 서버에서 애니메이션 상태를 설정하고 모든 클라이언트에 전송
        RpcUpdateAnimation(isMoving);
    }

    [ClientRpc]
    void RpcUpdateAnimation(bool isMoving)
    {
        // 모든 클라이언트에서 애니메이션 상태를 설정
        if (!isLocalPlayer)
        {
            Animator_Player.SetBool("isRun", isMoving);
        }
    }

    // 닉네임이 변경될 때 호출되는 함수
    private void OnNicknameChanged(string oldNickname, string newNickname)
    {
        TextMesh_Nickname.text = newNickname;
    }

    //카메라 붙이기
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

            // 로컬 플레이어의 닉네임 설정
            CmdSetNickname(PlayerPrefs.GetString("PlayerName"));
        }
    }

    [Command]
    void CmdSetNickname(string nickname)
    {
        playerNickname = nickname;
    }
}