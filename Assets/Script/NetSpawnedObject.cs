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

    //���� ����
    public int QustionValue = 0;
    public Text Point_Text;
    public bool NextGame = false;
    [SerializeField] private int Point = 0;
   

    private void Start()
    {
        Animator_Player = GetComponent<Animator>();
        Debug.Log($"���� Ŭ���ΰ�? : {isClient}");
        Debug.Log($"���� �����ΰ�? : {isServer}");
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
        
        // ȸ��
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        // �̵�
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.Move(forward * Mathf.Max(vertical, 0) * NavAgent_Player.speed * _moveSpeed * Time.deltaTime);

        bool isMoving = horizontal != 0 || vertical != 0;
        Animator_Player.SetBool("isRun", isMoving);

        // �ִϸ��̼� ���¸� ������ ����
        CmdUpdateAnimation(isMoving);

    }

    [Command]
    void CmdUpdateAnimation(bool isMoving)
    {
        // �������� �ִϸ��̼� ���¸� �����ϰ� ��� Ŭ���̾�Ʈ�� ����
        RpcUpdateAnimation(isMoving);
    }

    [ClientRpc]
    void RpcUpdateAnimation(bool isMoving)
    {
        // ��� Ŭ���̾�Ʈ���� �ִϸ��̼� ���¸� ����
        if (!isLocalPlayer)
        {
            Animator_Player.SetBool("isRun", isMoving);
        }
    }

    // �г����� ����� �� ȣ��Ǵ� �Լ�
    private void OnNicknameChanged(string oldNickname, string newNickname)
    {
        TextMesh_Nickname.text = newNickname;
    }

    //ī�޶� ���̱�
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

            // ���� �÷��̾��� �г��� ����
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
                Debug.Log("���� ���� üũ ����~~~");

                // Ŭ�е� QuestionBox ����Ʈ�� ��ȸ�ϸ� ������Ʈ�� Ȱ�� ���¸� Ȯ���մϴ�.
                foreach (GameObject questionBox in gameManageMent.QuestionBox)
                {
                    if (questionBox.activeInHierarchy)
                    {
                        Question question = questionBox.GetComponent<Question>();
                        Debug.Log($"���� ��: {question.QuestionCurrent}, QustionValue: {QustionValue}");
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
        Win_Text.text = "���� �Դϴ� ^ �� ^";
        yield return new WaitForSeconds(3);
        Win_Text.text = " ";
        yield return new WaitForSeconds(3);
        SetResult_Win.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SetResult_Win.gameObject.SetActive(false);
        
        if (isLocalPlayer)
        {
            Point += 1;
            Point_Text.text = $"���� : {Point}";
            isWinText = true;

            if (gameManageMent.EndGame)
            {
                Win_Text.text = "��� ������ ������ϴ�!@!";
                yield return new WaitForSeconds(3);
                Win_Text.text = " ";
                yield return new WaitForSeconds(3);
                Exit_Room.text = "����� ���� ��Ż�մϴ�";
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
            Win_Text.text = "Ʋ�Ƚ��ϴ� �� �� ��";
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
        Exit_Room.text = "�� ���� ��Ż �մϴ�!";
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