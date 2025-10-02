using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TalkManager : MonoBehaviour
{
    // 인스펙터에 연결할 대화창 텍스트 컴포넌트
    public Text talkText;

    // 대화창 전체 패널 (SetActive로 켜고 끄기 위함)
    public GameObject dialoguePanel;

    // 현재 대화 중인 오브젝트
    public GameObject currentTarget;

    // 💡 실제 대화 데이터를 저장할 딕셔너리
    private Dictionary<int, string[]> dialogueData;

    // 현재 대화 중인 문장 인덱스
    private int dialogueIndex = 0;

    // 현재 사용 중인 대화 ID
    private int currentDialogueID = 0;

    void Awake()
    {
        // 💡 대화 데이터 초기화 (ID에 따라 다중 몬스터 대화 저장)
        dialogueData = new Dictionary<int, string[]>
        {
            { 100, new string[] { "나는 이 던전의 슬라임이다.", "감히 내 영역을 침범하다니!", "각오해라!" } },
            { 200, new string[] { "여어~ 길드에서 온 용사님인가?", "내 초록색 물방울은 끈적하지.", "조심하는 게 좋을 걸." } },
            { 300, new string[] { "나.. 나는 화가 났어! (콰직)", "날 만지면 화상 입을걸!", "더 이상 대화는 없다!" } }
        };

        // 초기에는 대화창을 숨깁니다.
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void StartDialogue(GameObject targetObj, int dialogueID)
    {
        // 🚨 NullReferenceException 방지: 연결된 UI가 없으면 실행 중단
        if (dialoguePanel == null || talkText == null)
        {
            Debug.LogError("TalkManager: Dialogue Panel 또는 Talk Text가 인스펙터에 연결되지 않았습니다!");
            return;
        }

        currentTarget = targetObj;
        currentDialogueID = dialogueID;
        dialogueIndex = 0; // 대화 시작 시 인덱스 0으로 초기화

        dialoguePanel.SetActive(true);
        DisplayDialogue(); // 인덱스 0 (첫 번째 문장) 출력
        Debug.Log(targetObj.name + "과의 대화 시작. ID: " + dialogueID);
    }

    public void NextDialogue()
    {
        // 🚨 NullReferenceException 방지: 연결된 UI가 없으면 실행 중단
        if (dialoguePanel == null || talkText == null)
        {
            Debug.LogError("TalkManager: Dialogue Panel 또는 Talk Text가 인스펙터에 연결되지 않았습니다! (NextDialogue)");
            return;
        }

        dialogueIndex++; // 다음 문장으로 인덱스 증가

        if (dialogueData.ContainsKey(currentDialogueID))
        {
            int totalLines = dialogueData[currentDialogueID].Length;

            // ⭐ 강화된 로직: 인덱스가 배열의 길이를 초과하지 않았는지 확인
            if (dialogueIndex < totalLines)
            {
                DisplayDialogue();
            }
            else
            {
                // 인덱스가 배열 길이에 도달하거나 초과하면 대화 종료
                EndDialogue();
            }
        }
    }

    private void DisplayDialogue()
    {
        if (dialogueData.ContainsKey(currentDialogueID) && talkText != null)
        {
            // ⭐ 현재 증가된 인덱스의 문장을 출력
            talkText.text = dialogueData[currentDialogueID][dialogueIndex];
        }
    }

    private void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        currentTarget = null;
        currentDialogueID = 0;
        dialogueIndex = 0;
        Debug.Log("대화 종료");
    }
}