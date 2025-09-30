using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    // Text (TMP) ������Ʈ
    [SerializeField] private TextMeshProUGUI abilityTextComponent;

    // **[����]** abilityPanel�� ���� Inspector���� ������ �ʿ䰡 ���ų�,
    // �ʿ��ϴٸ� �����ϵ� ��ũ��Ʈ���� .SetActive()�� ȣ������ �ʽ��ϴ�.
    // [SerializeField] private GameObject abilityPanel; // ���� �� ���� �����ϰų� �����մϴ�.

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

        // ������ '����'�� ��Ȱ��ȭ�մϴ�. �ǳ��� ���� ���·� �����˴ϴ�.
        HideTooltip();
    }

    // AbilityButton ��ũ��Ʈ���� ȣ��˴ϴ�.
    public void ShowTooltip(string content)
    {
        // �ؽ�Ʈ ������Ʈ�� Ȱ��ȭ�Ͽ� ������ ���̰� �մϴ�.
        abilityTextComponent.gameObject.SetActive(true);

        // �ؽ�Ʈ�� �����մϴ�.
        abilityTextComponent.text = content;
    }

    // AbilityButton ��ũ��Ʈ���� ȣ��˴ϴ�.
    public void HideTooltip()
    {
        // �ؽ�Ʈ ������Ʈ�� ��Ȱ��ȭ�Ͽ� ������ ����ϴ�.
        // abilityTextComponent�� ���� 'Text (TMP)' ������Ʈ�� ���ϴ�.
        abilityTextComponent.gameObject.SetActive(false);

        // �ؽ�Ʈ ������ ����ִ� �͵� �����ϴ�. (���� ���������� ����մϴ�.)
        abilityTextComponent.text = string.Empty;
    }
}