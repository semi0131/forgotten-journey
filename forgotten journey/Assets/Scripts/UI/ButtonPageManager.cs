using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonPageManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------
    // UI �� ������Ʈ ����
    // ----------------------------------------------------------------------------------

    [Header("UI ����")]
    [SerializeField] private Button[] battleButtons = new Button[3];
    [SerializeField] private TextMeshProUGUI[] buttonTextComponents = new TextMeshProUGUI[3];
    [SerializeField] private AbilityButton[] abilityButtonScripts = new AbilityButton[3];

    [Header("������ ��ȯ ��ư")]
    // Button(1) ������Ʈ�� �����մϴ�. (OnClick �����ʸ� �ڵ�� �����ϱ� ����)
    [SerializeField] private Button magicButton;

    // ----------------------------------------------------------------------------------
    // ������ ����
    // ----------------------------------------------------------------------------------

    [Header("������ ������")]
    public AbilityData[] battlePageData = new AbilityData[3];
    public AbilityData[] magicPageData = new AbilityData[3];

    // ----------------------------------------------------------------------------------
    // ���� ����
    // ----------------------------------------------------------------------------------

    private bool isMagicPage = false;

    void Start()
    {
        SwitchPage(false); // ���� �� ���� �������� ����
    }

    /// <summary>
    /// ���� �������� ��ȯ�ϴ� �Լ��Դϴ�. (���� �������� Button(1) Ŭ�� �� ����)
    /// </summary>
    public void GoToMagicPage()
    {
        if (!isMagicPage)
        {
            SwitchPage(true);
        }
    }

    /// <summary>
    /// ���� ���������� Button(1)�� ������ �� ����� �Լ��Դϴ�. (������ ��ȯ ����)
    /// ���߿� ���� ��ų ��� ������ ���⿡ �߰��ϼ���.
    /// </summary>
    public void UseMagicSkill()
    {
        // Debug.Log�� ���� ��ư�� ���Ȱ�, ������ ��ȯ�� �Ͼ�� �ʾ����� Ȯ���մϴ�.
        // TODO: ���߿� ���⿡ ���� ���� ��� ������ �߰��մϴ�.
    }

    // (���� ����: �ٸ� ��ư�� ���� ���� �������� ���ư��� �Ѵٸ� �� �Լ��� ���)
    public void GoToBattlePage()
    {
        if (isMagicPage)
        {
            SwitchPage(false);
        }
    }


    /// <summary>
    /// ���� ������ ��ȯ �� Button(1)�� ����� ������Ʈ�մϴ�.
    /// </summary>
    private void SwitchPage(bool toMagic)
    {
        isMagicPage = toMagic; // ���� ������Ʈ

        if (magicButton == null)
        {
            Debug.LogError("Magic Button (Button(1))�� Inspector�� ������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� Button(1)�� ����� ��� ����� �����մϴ�. (�ʼ�)
        magicButton.onClick.RemoveAllListeners();

        if (toMagic)
        {
            // [1] ���� �������� ��ȯ
            ApplyPage(magicPageData);

            // [2] Button(1)�� '��ų ���' ����� �����մϴ�. (������ ��ȯ ����)
            magicButton.onClick.AddListener(UseMagicSkill);
        }
        else
        {
            // [1] ���� �������� ��ȯ
            ApplyPage(battlePageData);

            // [2] Button(1)�� '���� �������� ����' ����� �ٽ� �����մϴ�.
            magicButton.onClick.AddListener(GoToMagicPage);
        }
    }

    /// <summary>
    /// �迭�� ������ �����͸� UI ������Ʈ�� �����մϴ�. (������ ����)
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
// ������ ����ü ���� (���� ���Ͽ� �ְų� ���� ���Ϸ� ����ϴ�)
// ----------------------------------------------------------------------------------

[System.Serializable]
public struct AbilityData
{
    public string buttonName;
    [TextArea(3, 5)]
    public string tooltipDescription;
}