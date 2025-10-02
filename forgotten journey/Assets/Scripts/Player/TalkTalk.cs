using UnityEngine;

public class TalkTalk : MonoBehaviour
{
    // TalkManager 오브젝트를 인스펙터에서 연결 (혹은 FindObjectOfType으로 찾음)
    public TalkManager manager;

    // 이 변수는 이제 필요 없으므로 제거하거나 주석 처리
    // GameObject scanObject; 

    // 대화 시작/다음 대화 키

    private KeyCode interactionKey = KeyCode.Space;

    void Start()
    {
        // manager 변수가 인스펙터에 연결되지 않았다면 씬에서 찾아 연결합니다.
        if (manager == null)
        {
            manager = FindObjectOfType<TalkManager>();
        }
    }

    private void Update()
    {
        // 1. 스페이스바를 눌렀을 때
        if (Input.GetKeyDown(interactionKey))
        {
            // 2. TalkManager가 현재 대화 중인 상태인지 확인합니다.
            //    DialogueTrigger와 충돌하지 않은 상태에서 NextDialogue를 호출하지 않도록 방지
            if (manager != null && manager.dialoguePanel != null && manager.dialoguePanel.activeSelf)
            {
                // 대화 중이라면 다음 대화로 넘깁니다. (NextDialogue는 현재 대화 주체에 관계없이 작동)
                manager.NextDialogue();
            }
            // 💡 대화 중이 아니라면, 이 스크립트에서는 별다른 행동을 하지 않습니다
            //    (대화 시작은 DialogueTrigger.cs가 담당)
        }
    }
}
