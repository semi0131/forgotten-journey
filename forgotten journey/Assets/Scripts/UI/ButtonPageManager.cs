using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; // Enum.GetValues 사용을 위해 추가될 수 있음

public class ButtonPageManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------
    // UI 및 외부 스크립트 연결
    // ----------------------------------------------------------------------------------

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private Button[] battleButtons = new Button[3];
    [SerializeField] private TextMeshProUGUI[] buttonTextComponents = new TextMeshProUGUI[3];
    [SerializeField] private AbilityButton[] abilityButtonScripts = new AbilityButton[3];

    [Header("페이지 전환 버튼")]
    [SerializeField] private Button magicButton; // Button(1)을 여기에 연결합니다. 

    [Header("외부 스크립트 연결")]
    [SerializeField] private PlayerAction playerAction; // 턴 소비 행동 실행을 위해 필요
    [SerializeField] private TurnManager turnManager;   // 도망가기 기능 실행을 위해 필요

    // ----------------------------------------------------------------------------------
    // 데이터 설정
    // ----------------------------------------------------------------------------------

    [Header("페이지 데이터")]
    public AbilityData[] battlePageData = new AbilityData[3]; // 공격, 마법, 도망가기
    public AbilityData[] magicPageData = new AbilityData[3];  // 마법 스킬 1, 2, 3 (혹은 돌아가기)

    // ----------------------------------------------------------------------------------
    // 내부 로직
    // ----------------------------------------------------------------------------------

    private bool isMagicPage = false;

    void Start()
    {
        if (playerAction == null) playerAction = FindFirstObjectByType<PlayerAction>();
        if (turnManager == null) turnManager = FindFirstObjectByType<TurnManager>();

        // 게임 시작 시 전투 페이지로 초기화합니다.
        SwitchPage(false);
    }

    /// <summary>
    /// 마법 페이지로 전환하는 함수입니다. (전투 페이지의 Button(1) 클릭 시 연결)
    /// </summary>
    public void GoToMagicPage()
    {
        if (!isMagicPage)
        {
            SwitchPage(true);
        }
    }

    // (선택 사항: 마법 페이지에서 전투 페이지로 돌아가는 버튼이 있다면 연결)
    public void GoToBattlePage()
    {
        if (isMagicPage)
        {
            SwitchPage(false);
        }
    }

    /// <summary>
    /// 실제 페이지 전환 및 Button(1)의 기능을 업데이트합니다.
    /// </summary>
    private void SwitchPage(bool toMagic)
    {
        isMagicPage = toMagic; // 상태 업데이트

        if (magicButton == null) return;

        // 1. 기존 Button(1)의 리스너를 모두 제거합니다.
        magicButton.onClick.RemoveAllListeners();

        // 2. 툴팁 텍스트 및 버튼 이름 업데이트
        ApplyPage(toMagic ? magicPageData : battlePageData);

        // 3. 페이지에 따라 Button(1)의 기능 설정 (스킬 사용 vs 페이지 전환)
        if (toMagic)
        {
            // 마법 페이지: Button(1)을 '턴 소비 스킬'로 연결합니다.
            magicButton.onClick.AddListener(() => playerAction.ExecutePlayerAction("MAGIC_M")); // MAGIC_M은 Button(1) 스킬을 의미
            Debug.Log("페이지 전환: 마법 스킬 페이지. Button(1) 기능이 스킬 사용으로 변경됨.");
        }
        else
        {
            // 전투 페이지: Button(1)을 '페이지 전환' 기능으로 연결합니다.
            magicButton.onClick.AddListener(GoToMagicPage);
            Debug.Log("페이지 전환: 기본 전투 페이지. Button(1) 기능이 마법 페이지로 가기로 변경됨.");
        }

        // 4. 나머지 버튼들 (Button(0), Button(2))의 리스너를 페이지에 맞게 설정합니다.
        SetOtherButtonListeners(toMagic);
    }

    /// <summary>
    /// Button(0)과 Button(2)의 OnClick 리스너를 페이지에 맞게 동적으로 설정합니다.
    /// </summary>
    private void SetOtherButtonListeners(bool isMagicPage)
    {
        for (int i = 0; i < battleButtons.Length; i++)
        {
            Button button = battleButtons[i];

            // Button(1)은 SwitchPage에서 처리했으므로 건너뜁니다.
            if (i == 1) continue;

            button.onClick.RemoveAllListeners(); // 기존 리스너 모두 제거!

            if (!isMagicPage) // 전투 페이지 (Button(0): 공격, Button(2): 도망가기)
            {
                if (i == 0 && playerAction != null) // Button(0) - 공격
                {
                    // 람다 표현식으로 "ATTACK" 문자열 인자를 전달합니다.
                    button.onClick.AddListener(() => playerAction.ExecutePlayerAction("ATTACK"));
                }
                else if (i == 2 && turnManager != null) // Button(2) - 도망가기
                {
                    button.onClick.AddListener(turnManager.RunFromBattle);
                }
            }
            else // 마법 페이지 (Button(0), Button(2) 모두 턴 소비 마법 스킬)
            {
                if (playerAction != null)
                {
                    string actionType = (i == 0) ? "MAGIC_A" : "MAGIC_B"; // Button(0), Button(2)를 구분합니다.
                    button.onClick.AddListener(() => playerAction.ExecutePlayerAction(actionType));
                }
            }
        }
    }

    /// <summary>
    /// 배열에 설정된 데이터를 UI 컴포넌트(이름, 툴팁)에 적용합니다.
    /// </summary>
    private void ApplyPage(AbilityData[] data)
    {
        if (data.Length < 3) return;

        for (int i = 0; i < 3; i++)
        {
            if (buttonTextComponents[i] != null)
            {
                buttonTextComponents[i].text = data[i].buttonName;
            }

            // AbilityButton 스크립트가 있다면 툴팁 데이터 업데이트
            if (abilityButtonScripts[i] != null)
            {
                // **주의:** AbilityButton.cs에 SetTooltipDescription 함수가 필요합니다.
                abilityButtonScripts[i].SetTooltipDescription(data[i].tooltipDescription);
            }
        }
    }
    public void ResetToBattlePage()
    {
        if (isMagicPage)
        {
            GoToBattlePage(); // GoToBattlePage()가 SwitchPage(false)를 호출하여 기본 페이지로 전환
        }
    }
}

// ----------------------------------------------------------------------------------
// 데이터 구조체 정의 (같은 파일에 넣거나 별도 파일로 만듭니다)
// ----------------------------------------------------------------------------------

[System.Serializable]
public struct AbilityData
{
    public string buttonName;
    [TextArea(3, 5)]
    public string tooltipDescription;
}