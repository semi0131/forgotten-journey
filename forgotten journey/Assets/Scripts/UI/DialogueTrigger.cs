using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // TalkManager ������Ʈ�� �ν����Ϳ��� ����
    public TalkManager talkManager;

    // �� ���Ͱ� ���� ��ȭ �������� ID (�ν����Ϳ��� 100, 200, 300 ������ ����)
    public int dialogueID = 100;

    // �÷��̾ ��ȭ ���� ������ �ִ��� Ȯ��
    private bool playerIsNear = false;

    // ��ȭ ����/���� ��ȭ Ű
    private KeyCode interactionKey = KeyCode.Space;

    void Start()
    {
        // �ν����� ������ �ؾ��� ��츦 ����Ͽ� �ڵ����� ã��
        if (talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
        }
    }

    void Update()
    {
        // 1. �÷��̾ ��ó�� �ְ�,
        // 2. ��ȣ�ۿ� Ű(Space)�� ������ ��
        if (playerIsNear && Input.GetKeyDown(interactionKey))
        {
            if (talkManager != null)
            {
                // ��ȭ ���̰� ���� ������Ʈ�� ��ȭ�� Ÿ���̶�� ���� ��������
                if (talkManager.dialoguePanel != null && talkManager.dialoguePanel.activeSelf && talkManager.currentTarget == gameObject)
                {
                    talkManager.NextDialogue();
                }
                // ��ȭ ���� �ƴϰų� �ٸ� ������Ʈ�� ��ȭ ���̶�� ���ο� ��ȭ ����
                else if (talkManager.dialoguePanel != null && !talkManager.dialoguePanel.activeSelf)
                {
                    talkManager.StartDialogue(gameObject, dialogueID);
                }
            }
        }
    }

    // �÷��̾ ��ȭ ������ ������ �� (���Ϳ� Is Trigger �ݶ��̴� �ʿ�)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    // �÷��̾ ��ȭ �������� ������ ��
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}