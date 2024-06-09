using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;
using Org.BouncyCastle.Bcpg;

public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent NavAgent_Player;
    [SerializeField] private Animator Animator_Player;
    [SerializeField] private TextMesh TextMesh_Nickname;
    

    [SerializeField] private float Text_Speed = 5;
    public GameObject SetResult_Win;
    public GameObject SetResult_Lose;
    private bool isWinText = false;
    private bool isLoseText = false;


    public GameManageMent gameManageMent;
    public Text Win_Text;
    
    //private Transform Transform_Player;

    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 100.0f;
    public float _moveSpeed = 4.0f;


    [SerializeField] private Vector3 defaultInitialPlanePosition = new Vector3(-9.16621f, 0.036054f, -66.13957f);
    private CinemachineVirtualCamera virtualCamera;
    private string MyObjectName;


    [SyncVar(hook = nameof(OnNicknameChanged))]
    private string playerNickname;

    //문제 검증
    public int QustionValue = 0;
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer)
        {
            OXZoneTrigger oXZoneTrigger = other.GetComponent<OXZoneTrigger>();

            if (other.gameObject.name == "O_Zone")
            {
                QustionValue = 1;

            }
            else if (other.gameObject.name == "X_Zone")
            {
                QustionValue = 2;
            }
            oXZoneTrigger.check = true;


            if (oXZoneTrigger.check)
            {
                Debug.Log("이제 정답 체크 들어간다~~~");

                // 클론된 QuestionBox 리스트를 순회하며 오브젝트와 활성 상태를 확인합니다.
                foreach (GameObject questionBox in gameManageMent.QuestionBox)
                {
                    if (questionBox.activeInHierarchy)
                    {
                        Question question = questionBox.GetComponent<Question>();
                        Debug.Log($"문제 값: {question.QuestionCurrent}, QustionValue: {QustionValue}");
                        if (question.QuestionCurrent == QustionValue)
                        {
                            question.gameObject.SetActive(false);
                            Win_Text.text = "정답 입니다 ^ ㅇ ^";
                            isWinText = true;
                            StartCoroutine(CloseQuestionBox());
                              
                        }
                        else if (question.QuestionCurrent != QustionValue)
                        {
                            question.gameObject.SetActive(false);
                            Win_Text.text = "틀렸습니다 ㅠ ㅇ ㅠ";
                            isLoseText = true;
                            StartCoroutine(CloseQuestionBox());
                            
                        }
                    }
                    oXZoneTrigger.check = false;
                }

            }
        }
       
    }

    IEnumerator CloseQuestionBox()
    {
        yield return new WaitForSeconds(3);
        Win_Text.text = " ";
        QustionValue = 0;
        if (isWinText)
        {
            SetResult_Win.gameObject.SetActive(true);
        }
        else if(isLoseText)
        {
            SetResult_Lose.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(3);
    }
}