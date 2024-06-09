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

    //���� ����
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
                            Win_Text.text = "���� �Դϴ� ^ �� ^";
                            isWinText = true;
                            StartCoroutine(CloseQuestionBox());
                              
                        }
                        else if (question.QuestionCurrent != QustionValue)
                        {
                            question.gameObject.SetActive(false);
                            Win_Text.text = "Ʋ�Ƚ��ϴ� �� �� ��";
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