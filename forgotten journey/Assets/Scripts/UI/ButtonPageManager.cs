using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonPageManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------
    // UI 및 컴포넌트 연결
    // ----------------------------------------------------------------------------------

    [Header("UI 연결")]
    [SerializeField] private Button[] battleButtons = new Button[3];
    [SerializeField] private TextMeshProUGUI[] buttonTextComponents = new TextMeshProUGUI[3];
    [SerializeField] private AbilityButton[] abilityButtonScripts = new AbilityButton[3];

    [Header("페이지 전환 버튼")]
    // Button(1) 컴포넌트를 연결합니다. (OnClick 리스너를 코드로 관리하기 위함)
    [SerializeField] private Button magicButton;

    // ----------------------------------------------------------------------------------
    // 데이터 설정
    // ----------------------------------------------------------------------------------

    [Header("페이지 데이터")]
    public AbilityData[] battlePageData = new AbilityData[3];
    public AbilityData[] magicPageData = new AbilityData[3];

    // ----------------------------------------------------------------------------------
    // 내부 로직
    // ----------------------------------------------------------------------------------

    private bool isMagicPage = false;

    void Start()
    {
        SwitchPage(false); // 시작 시 전투 페이지로 설정
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

    /// <summary>
    /// 마법 페이지에서 Button(1)이 눌렸을 때 실행될 함수입니다. (페이지 전환 없음)
    /// 나중에 실제 스킬 사용 로직을 여기에 추가하세요.
    /// </summary>
    public void UseMagicSkill()
    {
        // Debug.Log를 통해 버튼이 눌렸고, 페이지 전환은 일어나지 않았음을 확인합니다.
        // TODO: 나중에 여기에 실제 마법 사용 로직을 추가합니다.
    }

    // (선택 사항: 다른 버튼을 눌러 전투 페이지로 돌아가야 한다면 이 함수를 사용)
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

        if (magicButton == null)
        {
            Debug.LogError("Magic Button (Button(1))이 Inspector에 연결되지 않았습니다.");
            return;
        }

        // 기존 Button(1)에 연결된 모든 기능을 제거합니다. (필수)
        magicButton.onClick.RemoveAllListeners();

        if (toMagic)
        {
            // [1] 마법 페이지로 전환
            ApplyPage(magicPageData);

            // [2] Button(1)에 '스킬 사용' 기능을 연결합니다. (페이지 전환 방지)
            magicButton.onClick.AddListener(UseMagicSkill);
        }
        else
        {
            // [1] 전투 페이지로 전환
            ApplyPage(battlePageData);

            // [2] Button(1)에 '마법 페이지로 가기' 기능을 다시 연결합니다.
            magicButton.onClick.AddListener(GoToMagicPage);
        }
    }

    /// <summary>
    /// 배열에 설정된 데이터를 UI 컴포넌트에 적용합니다. (이전과 동일)
    /// </summary>
    private void ApplyPage(AbilityData[] data)
    {
        for (int i = 0; i < 3; i++)
        {
            if (buttonTextComponents[i] != null)
                buttonTextComponents[i].text = data[i].buttonName;

            if (abilityButtonScripts[i] != null)
                abilityButtonScripts[i].SetTooltipDescription(data[i].tooltipDescription);
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