using UnityEngine;
using UnityEngine.EventSystems; // IPointerEnterHandler, IPointerExitHandler 사용을 위해 필요

// 마우스가 들어오고 나가는 이벤트를 처리하기 위한 인터페이스 상속
public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 유니티 인스펙터 창에서 버튼별 설명을 쉽게 입력할 수 있도록 합니다.
    [TextArea(3, 5)]
    [SerializeField] private string tooltipDescription = "여기에 스킬/아이템 설명을 입력하세요.";

    public void SetTooltipDescription(string newDescription)
    {
        tooltipDescription = newDescription;
    }

    // 마우스 커서가 버튼 위에 들어왔을 때 호출됩니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            // TooltipManager에게 저장된 설명 텍스트를 표시하도록 요청합니다.
            TooltipManager.Instance.ShowTooltip(tooltipDescription);
        }
    }

    // 마우스 커서가 버튼 밖으로 나갔을 때 호출됩니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            // TooltipManager에게 툴팁을 숨기도록 요청합니다.
            TooltipManager.Instance.HideTooltip();
        }
    }
}