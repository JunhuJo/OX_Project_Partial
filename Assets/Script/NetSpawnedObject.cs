using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent NavAgent_Player;
    [SerializeField] private Animator Animator_Player;
    [SerializeField] private TextMesh TextMesh_Nickname;
    
    public GameObject SetResult_Win;
    public GameObject SetResult_Lose;
    private bool isWinText = false;
    private bool isLoseText = false;

    public NetworkManager NetworkManager;

    public GameManageMent gameManageMent;
    public GameObject Login_Window;
    public Text Win_Text;
    public Text Exit_Room;
    

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
    public Text Point_Text;
    public bool NextGame = false;
    [SerializeField] private int Point = 0;
   

    private void Start()
    {
        Animator_Player = GetComponent<Animator>();
        Debug.Log($"나는 클라인가? : {isClient}");
        Debug.Log($"나는 서버인가? : {isServer}");
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
                            StartCoroutine(Win());
                        }
                        else if (question.QuestionCurrent != QustionValue)
                        {
                            question.gameObject.SetActive(false);
                            isLoseText = true;
                            StartCoroutine(CloseQuestionBox());
                        }
                    }
                    oXZoneTrigger.check = false;
                }

            }
        }
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(3);
        Win_Text.text = "정답 입니다 ^ ㅇ ^";
        yield return new WaitForSeconds(3);
        Win_Text.text = " ";
        yield return new WaitForSeconds(3);
        SetResult_Win.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SetResult_Win.gameObject.SetActive(false);
        
        if (isLocalPlayer)
        {
            Point += 1;
            Point_Text.text = $"점수 : {Point}";
            isWinText = true;

            if (gameManageMent.EndGame)
            {
                Win_Text.text = "모든 문제를 맞췄습니다!@!";
                yield return new WaitForSeconds(3);
                Win_Text.text = " ";
                yield return new WaitForSeconds(3);
                Exit_Room.text = "잠시후 방을 이탈합니다";
                yield return new WaitForSeconds(3);

                if (isClient)
                {
                    NetworkManager.StopClient();
                }
                else if (isServer)
                {
                    NetworkManager.StopHost();
                }

                Login_Window.gameObject.SetActive(true);
            }
            else
            {
                gameManageMent.StartGames = true;
            }
        }
    }


    IEnumerator CloseQuestionBox()
    {
        QustionValue = 0;
        
        if(isLoseText)
        {
            yield return new WaitForSeconds(3);
            Win_Text.text = "틀렸습니다 ㅠ ㅇ ㅠ";
            yield return new WaitForSeconds(3);
            Win_Text.text = "";
            SetResult_Lose.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            SetResult_Lose.gameObject.SetActive(false);
            StartCoroutine(breakaway());
        }
    }

    IEnumerator breakaway()
    {
        yield return new WaitForSeconds(3);
        Exit_Room.text = "곧 방을 이탈 합니다!";
        yield return new WaitForSeconds(5);
        Exit_Room.text = "";

        if (isClient)
        {
            NetworkManager.StopClient();
        }
        else if (isServer)
        {
            NetworkManager.StopHost();
        }

        Login_Window.gameObject.SetActive(true);
    }
}