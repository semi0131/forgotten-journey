using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetSceneName = "LastDungeonScene";
    // 새 씬에서 캐릭터가 나타날 정확한 좌표를 인스펙터에서 설정
    public Vector3 targetPosition = new Vector3(5f, 0f, 0f);

    private bool playerIsNear = false;

    // (OnTriggerEnter2D, OnTriggerExit2D 등 생략 - 이전과 동일)

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
            // 다음 씬의 목표 위치를 저장
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
        // 씬 로드 완료 후, 해당 씬이 목표 씬이라면 위치 조정
        if (scene.name == targetSceneName)
        {
            // 1. 저장된 위치 정보 로드
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");

            // 2. DontDestroyOnLoad로 파괴되지 않고 넘어온 플레이어를 찾음
            //    (PlayerPersist 스크립트 덕분에 미리 배치된 플레이어는 이미 파괴되었음)
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // 3. 플레이어의 위치를 목표 좌표로 이동
                player.transform.position = new Vector3(x, y, player.transform.position.z);

                // 위치 정보 초기화
                PlayerPrefs.DeleteKey("PlayerX");
                PlayerPrefs.DeleteKey("PlayerY");
            }
        }
    }
}