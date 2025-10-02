using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; // Enum.GetValues ����� ���� �߰��� �� ����

public class ButtonPageManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------
    // UI �� �ܺ� ��ũ��Ʈ ����
    // ----------------------------------------------------------------------------------

    [Header("UI ������Ʈ ����")]
    [SerializeField] private Button[] battleButtons = new Button[3];
    [SerializeField] private TextMeshProUGUI[] buttonTextComponents = new TextMeshProUGUI[3];
    [SerializeField] private AbilityButton[] abilityButtonScripts = new AbilityButton[3];

    [Header("������ ��ȯ ��ư")]
    [SerializeField] private Button magicButton; // Button(1)�� ���⿡ �����մϴ�. 

    [Header("�ܺ� ��ũ��Ʈ ����")]
    [SerializeField] private PlayerAction playerAction; // �� �Һ� �ൿ ������ ���� �ʿ�
    [SerializeField] private TurnManager turnManager;   // �������� ��� ������ ���� �ʿ�

    // ----------------------------------------------------------------------------------
    // ������ ����
    // ----------------------------------------------------------------------------------

    [Header("������ ������")]
    public AbilityData[] battlePageData = new AbilityData[3]; // ����, ����, ��������
    public AbilityData[] magicPageData = new AbilityData[3];  // ���� ��ų 1, 2, 3 (Ȥ�� ���ư���)

    // ----------------------------------------------------------------------------------
    // ���� ����
    // ----------------------------------------------------------------------------------

    private bool isMagicPage = false;

    void Start()
    {
        if (playerAction == null) playerAction = FindFirstObjectByType<PlayerAction>();
        if (turnManager == null) turnManager = FindFirstObjectByType<TurnManager>();

        // ���� ���� �� ���� �������� �ʱ�ȭ�մϴ�.
        SwitchPage(false);
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

    // (���� ����: ���� ���������� ���� �������� ���ư��� ��ư�� �ִٸ� ����)
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

        if (magicButton == null) return;

        // 1. ���� Button(1)�� �����ʸ� ��� �����մϴ�.
        magicButton.onClick.RemoveAllListeners();

        // 2. ���� �ؽ�Ʈ �� ��ư �̸� ������Ʈ
        ApplyPage(toMagic ? magicPageData : battlePageData);

        // 3. �������� ���� Button(1)�� ��� ���� (��ų ��� vs ������ ��ȯ)
        if (toMagic)
        {
            // ���� ������: Button(1)�� '�� �Һ� ��ų'�� �����մϴ�.
            magicButton.onClick.AddListener(() => playerAction.ExecutePlayerAction("MAGIC_M")); // MAGIC_M�� Button(1) ��ų�� �ǹ�
            Debug.Log("������ ��ȯ: ���� ��ų ������. Button(1) ����� ��ų ������� �����.");
        }
        else
        {
            // ���� ������: Button(1)�� '������ ��ȯ' ������� �����մϴ�.
            magicButton.onClick.AddListener(GoToMagicPage);
            Debug.Log("������ ��ȯ: �⺻ ���� ������. Button(1) ����� ���� �������� ����� �����.");
        }

        // 4. ������ ��ư�� (Button(0), Button(2))�� �����ʸ� �������� �°� �����մϴ�.
        SetOtherButtonListeners(toMagic);
    }

    /// <summary>
    /// Button(0)�� Button(2)�� OnClick �����ʸ� �������� �°� �������� �����մϴ�.
    /// </summary>
    private void SetOtherButtonListeners(bool isMagicPage)
    {
        for (int i = 0; i < battleButtons.Length; i++)
        {
            Button button = battleButtons[i];

            // Button(1)�� SwitchPage���� ó�������Ƿ� �ǳʶݴϴ�.
            if (i == 1) continue;

            button.onClick.RemoveAllListeners(); // ���� ������ ��� ����!

            if (!isMagicPage) // ���� ������ (Button(0): ����, Button(2): ��������)
            {
                if (i == 0 && playerAction != null) // Button(0) - ����
                {
                    // ���� ǥ�������� "ATTACK" ���ڿ� ���ڸ� �����մϴ�.
                    button.onClick.AddListener(() => playerAction.ExecutePlayerAction("ATTACK"));
                }
                else if (i == 2 && turnManager != null) // Button(2) - ��������
                {
                    button.onClick.AddListener(turnManager.RunFromBattle);
                }
            }
            else // ���� ������ (Button(0), Button(2) ��� �� �Һ� ���� ��ų)
            {
                if (playerAction != null)
                {
                    string actionType = (i == 0) ? "MAGIC_A" : "MAGIC_B"; // Button(0), Button(2)�� �����մϴ�.
                    button.onClick.AddListener(() => playerAction.ExecutePlayerAction(actionType));
                }
            }
        }
    }

    /// <summary>
    /// �迭�� ������ �����͸� UI ������Ʈ(�̸�, ����)�� �����մϴ�.
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

            // AbilityButton ��ũ��Ʈ�� �ִٸ� ���� ������ ������Ʈ
            if (abilityButtonScripts[i] != null)
            {
                // **����:** AbilityButton.cs�� SetTooltipDescription �Լ��� �ʿ��մϴ�.
                abilityButtonScripts[i].SetTooltipDescription(data[i].tooltipDescription);
            }
        }
    }
    public void ResetToBattlePage()
    {
        if (isMagicPage)
        {
            GoToBattlePage(); // GoToBattlePage()�� SwitchPage(false)�� ȣ���Ͽ� �⺻ �������� ��ȯ
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