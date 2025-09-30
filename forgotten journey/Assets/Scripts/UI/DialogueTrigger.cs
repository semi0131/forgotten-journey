using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // TalkManager 오브젝트를 인스펙터에서 연결
    public TalkManager talkManager;

    // 이 몬스터가 가진 대화 데이터의 ID (인스펙터에서 100, 200, 300 등으로 설정)
    public int dialogueID = 100;

    // 플레이어가 대화 가능 범위에 있는지 확인
    private bool playerIsNear = false;

    // 대화 시작/다음 대화 키
    private KeyCode interactionKey = KeyCode.Space;

    void Start()
    {
        // 인스펙터 연결을 잊었을 경우를 대비하여 자동으로 찾음
        if (talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
        }
    }

    void Update()
    {
        // 1. 플레이어가 근처에 있고,
        // 2. 상호작용 키(Space)를 눌렀을 때
        if (playerIsNear && Input.GetKeyDown(interactionKey))
        {
            if (talkManager != null)
            {
                // 대화 중이고 현재 오브젝트가 대화의 타겟이라면 다음 문장으로
                if (talkManager.dialoguePanel != null && talkManager.dialoguePanel.activeSelf && talkManager.currentTarget == gameObject)
                {
                    talkManager.NextDialogue();
                }
                // 대화 중이 아니거나 다른 오브젝트와 대화 중이라면 새로운 대화 시작
                else if (talkManager.dialoguePanel != null && !talkManager.dialoguePanel.activeSelf)
                {
                    talkManager.StartDialogue(gameObject, dialogueID);
                }
            }
        }
    }

    // 플레이어가 대화 범위에 들어왔을 때 (몬스터에 Is Trigger 콜라이더 필요)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    // 플레이어가 대화 범위에서 나갔을 때
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}