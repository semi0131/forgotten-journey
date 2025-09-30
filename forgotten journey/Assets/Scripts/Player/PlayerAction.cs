using UnityEditor.Build.Content;
using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    public TalkManager manager;
    GameObject scanObject;

    [SerializeField] private SPUM_Prefabs spumPrefabs;
    [SerializeField] private TurnManager turnManager; // �� ���� ��ũ��Ʈ (���� ����)

    void Start()
    {
        // ���� Inspector���� �������� �ʾҴٸ�, ���⼭ ã�Ƽ� �����մϴ�.
        if (spumPrefabs == null)
        {
            spumPrefabs = GetComponent<SPUM_Prefabs>();
        }
        if (turnManager == null)
        {
            turnManager = FindObjectOfType<TurnManager>();
        }
    }

    /// <summary>
    /// ���� ��ư�� OnClick �̺�Ʈ�� ����� �Լ�. 
    /// �ִϸ��̼��� �����ϰ� �� �Ŵ������� �ൿ �ϷḦ �˸��ϴ�.
    /// </summary>
    public void ExecuteAttackAction()
    {
        if (turnManager == null || spumPrefabs == null) return;

        // 1. [�ٽ�] �� �Ŵ������� '�ൿ ����'�� �˸��ϴ�. (�� ���� �ڷ�ƾ�� ���۵��� ����)
        // �� �Լ��� state = BattleState.Action ���� �����մϴ�.
        turnManager.StartPlayerAction("ATTACK");

        // 2. ���� �ִϸ��̼� ���
        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0);

        // 3. �ִϸ��̼� ����� ���� ������ ��ٸ��ϴ�.
        // SPUM_Prefabs�� �ִϸ��̼� ���̸� �˾Ƴ��ų�, �̺�Ʈ �Լ��� ���� ó���ؾ� �մϴ�.
        float animationDuration = 1.0f; // �ִϸ��̼� ���̿� �°� �����ϼ���!
        StartCoroutine(WaitForActionEnd(animationDuration));
    }

    IEnumerator WaitForActionEnd(float duration)
    {
        // �ִϸ��̼� ��� �ð���ŭ ��ٸ��ϴ�.
        yield return new WaitForSeconds(duration);

        // 4. �ִϸ��̼��� �������� �� �Ŵ������� �˸��� ���� �����մϴ�.
        if (turnManager != null)
        {
            turnManager.FinishPlayerAction();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
            manager.Action(scanObject);
    }
}
