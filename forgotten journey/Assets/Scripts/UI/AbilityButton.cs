using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ����Ƽ �ν����� â���� ��ư�� ������ ���� �Է��� �� �ֵ��� �մϴ�.
    [TextArea(3, 5)]
    [SerializeField] private string tooltipDescription = "���⿡ ��ų/������ ������ �Է��ϼ���.";

    private TurnManager turnManager;
    private Button buttonComponent;

    public void SetTooltipDescription(string newDescription)
    {
        tooltipDescription = newDescription;
    }

    // ���콺 Ŀ���� ��ư ���� ������ �� ȣ��˴ϴ�.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            // TooltipManager���� ����� ���� �ؽ�Ʈ�� ǥ���ϵ��� ��û�մϴ�.
            TooltipManager.Instance.ShowTooltip(tooltipDescription);
        }
    }

    // ���콺 Ŀ���� ��ư ������ ������ �� ȣ��˴ϴ�.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            // TooltipManager���� ������ ���⵵�� ��û�մϴ�.
            TooltipManager.Instance.HideTooltip();
        }
    }
}