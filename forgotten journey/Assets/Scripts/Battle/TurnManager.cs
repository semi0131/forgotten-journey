using UnityEngine;
using System.Collections; // Coroutine�� ����ϱ� ���� �ʿ��մϴ�.

public class TurnManager : MonoBehaviour
{
    public BattleState state; // ���� ���� ����

    // ������ �����ϴ� �÷��̾� �� �� ���� ��ũ��Ʈ (���߿� ����)
    // [SerializeField] private Unit playerUnit; 
    // [SerializeField] private Unit enemyUnit;

    void Start()
    {
        // ���� ����
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// ������ �ʱ�ȭ�ϰ� ù ���� �����մϴ�.
    /// </summary>
    IEnumerator SetupBattle()
    {
        // 1. UI ���� (��: ���� ���� �޽��� ���)
        // battleMessageText.text = "���� ����!";
        Debug.Log("������ �����մϴ�.");

        // 2. ĳ���� ���� �ε� �� �غ� (HP, ���� ��)
        yield return new WaitForSeconds(1f); // 1�� ���

        // 3. ù �� ���� (�÷��̾ �����Ѵٰ� ����)
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// �÷��̾��� ���� ���۵� �� ȣ��˴ϴ�.
    /// </summary>
    void PlayerTurn()
    {
        // UI�� Ȱ��ȭ�Ͽ� �÷��̾�� �ൿ ���� ��ȸ�� �����մϴ�.
        // UIManager.Instance.ShowActionPanel(); 
        Debug.Log("�÷��̾� ��: �ൿ�� �����ϼ���.");

        // �� ���¿��� �÷��̾��� ��ư �Է�(����, ���� ��)�� ��ٸ��ϴ�.
    }

    /// <summary>
    /// �÷��̾��� �ൿ(��: ���� ��ư Ŭ��)�� ������ �� �ܺο��� ȣ��˴ϴ�.
    /// </summary>
    public void OnPlayerActionSelected()
    {
        if (state != BattleState.PlayerTurn)
            return;

        state = BattleState.Action; // ���¸� �ൿ �������� ����
        StartCoroutine(PlayerActionCoroutine());
    }

    /// <summary>
    /// �÷��̾��� �ൿ�� �����ϴ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    IEnumerator PlayerActionCoroutine()
    {
        // 1. �÷��̾��� �ൿ(�ִϸ��̼�, ������ ���) ����
        // (��: playerUnit.Attack(enemyUnit);)
        Debug.Log("�÷��̾ �ൿ�� �����մϴ�.");

        yield return new WaitForSeconds(2f); // �ൿ �ִϸ��̼� �ð� ���

        // 2. ���� ���� Ȯ��
        // if (enemyUnit.isDead)
        // {
        //     state = BattleState.Won;
        //     EndBattle();
        //     yield break;
        // }

        // 3. ���� ������ ��ȯ
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurnCoroutine());
    }

    /// <summary>
    /// ���� ���� ó���ϴ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    IEnumerator EnemyTurnCoroutine()
    {
        Debug.Log("���� ���� ���۵˴ϴ�.");

        // 1. ��� ��� (�����ϴ� �ð�)
        yield return new WaitForSeconds(1f);

        // 2. ���� �ൿ(AI) ���� �� ����
        // (��: enemyUnit.DecideAndAct(playerUnit);)
        Debug.Log("���� �ൿ�� �����մϴ�.");

        yield return new WaitForSeconds(2f); // �� �ൿ �ð� ���

        // 3. �÷��̾��� ���� Ȯ��
        // if (playerUnit.isDead)
        // {
        //     state = BattleState.Lost;
        //     EndBattle();
        //     yield break;
        // }

        // 4. ���� ��(�÷��̾� ��)���� ��ȯ
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// ���� ���Ḧ ó���մϴ�.
    /// </summary>
    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            Debug.Log("�������� �¸��߽��ϴ�!");
        }
        else if (state == BattleState.Lost)
        {
            Debug.Log("�������� �й��߽��ϴ�.");
        }
    }

    public void StartPlayerAction(string actionType)
    {
        if (state != BattleState.PlayerTurn)
        {
            Debug.Log("������ �÷��̾� ���� �ƴմϴ�. �ൿ ���.");
            return;
        }

        // ���� Action ���·� �����Ͽ� �ٸ� �Է��� �����ϴ�.
        state = BattleState.Action;
        Debug.Log($"�� �Һ� �ൿ: {actionType} ����. ����: Action");

        // **[�ٽ�]** �ڷ�ƾ�� ���� �������� �ʽ��ϴ�. PlayerAction�� �ִϸ��̼��� ����� �� ȣ���� �̴ϴ�.
    }

    public void FinishPlayerAction()
    {
        // �ִϸ��̼��� �������� ���� ���� ������ �Ѿ �� �ֽ��ϴ�.
        StartCoroutine(PlayerActionCoroutine());
    }

    public enum BattleState
    {
        Start,          // ���� ���� (�ʱ�ȭ �ܰ�)
        PlayerTurn,     // �÷��̾ �ൿ�� ������ ����
        EnemyTurn,      // ���� �ൿ�� �ϴ� ����
        Action,         // �÷��̾ ���� �ൿ�� ������ ����Ǵ� ��
        Won,            // ���� �¸�
        Lost            // ���� �й�
    }
}