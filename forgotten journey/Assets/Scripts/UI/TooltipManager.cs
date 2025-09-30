using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    // Text (TMP) 컴포넌트
    [SerializeField] private TextMeshProUGUI abilityTextComponent;

    // **[주의]** abilityPanel은 이제 Inspector에서 연결할 필요가 없거나,
    // 필요하다면 연결하되 스크립트에서 .SetActive()를 호출하지 않습니다.
    // [SerializeField] private GameObject abilityPanel; // 이제 이 줄은 무시하거나 제거합니다.

    public static TooltipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 툴팁의 '내용'만 비활성화합니다. 판넬은 켜진 상태로 유지됩니다.
        HideTooltip();
    }

    // AbilityButton 스크립트에서 호출됩니다.
    public void ShowTooltip(string content)
    {
        // 텍스트 컴포넌트를 활성화하여 내용을 보이게 합니다.
        abilityTextComponent.gameObject.SetActive(true);

        // 텍스트를 설정합니다.
        abilityTextComponent.text = content;
    }

    // AbilityButton 스크립트에서 호출됩니다.
    public void HideTooltip()
    {
        // 텍스트 컴포넌트를 비활성화하여 내용을 숨깁니다.
        // abilityTextComponent가 붙은 'Text (TMP)' 오브젝트를 끕니다.
        abilityTextComponent.gameObject.SetActive(false);

        // 텍스트 내용을 비워주는 것도 좋습니다. (선택 사항이지만 깔끔합니다.)
        abilityTextComponent.text = string.Empty;
    }
}