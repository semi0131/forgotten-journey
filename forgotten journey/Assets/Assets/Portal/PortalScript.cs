using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetSceneName = "LastDungeonScene";
    // �� ������ ĳ���Ͱ� ��Ÿ�� ��Ȯ�� ��ǥ�� �ν����Ϳ��� ����
    public Vector3 targetPosition = new Vector3(5f, 0f, 0f);

    private bool playerIsNear = false;

    // (OnTriggerEnter2D, OnTriggerExit2D �� ���� - ������ ����)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }


    private void Update()
    {
        if (playerIsNear && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            // ���� ���� ��ǥ ��ġ�� ����
            PlayerPrefs.SetFloat("PlayerX", targetPosition.x);
            PlayerPrefs.SetFloat("PlayerY", targetPosition.y);

            SceneManager.LoadScene(targetSceneName);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� �ε� �Ϸ� ��, �ش� ���� ��ǥ ���̶�� ��ġ ����
        if (scene.name == targetSceneName)
        {
            // 1. ����� ��ġ ���� �ε�
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");

            // 2. DontDestroyOnLoad�� �ı����� �ʰ� �Ѿ�� �÷��̾ ã��
            //    (PlayerPersist ��ũ��Ʈ ���п� �̸� ��ġ�� �÷��̾�� �̹� �ı��Ǿ���)
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // 3. �÷��̾��� ��ġ�� ��ǥ ��ǥ�� �̵�
                player.transform.position = new Vector3(x, y, player.transform.position.z);

                // ��ġ ���� �ʱ�ȭ
                PlayerPrefs.DeleteKey("PlayerX");
                PlayerPrefs.DeleteKey("PlayerY");
            }
        }
    }
}