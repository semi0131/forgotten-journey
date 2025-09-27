using UnityEngine;

public class PlayerPersist : MonoBehaviour
{
    // 정적 변수를 사용하여 씬을 넘나들어도 파괴되지 않는 유일한 인스턴스를 추적
    public static PlayerPersist instance;

    void Awake()
    {
        if (instance == null)
        {
            // 1. 현재 인스턴스가 첫 번째라면, 이 인스턴스를 유지하도록 설정
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // 2. 이미 다른 씬에서 넘어온 인스턴스(instance)가 있다면,
            //    현재 씬에 미리 배치된 플레이어(this.gameObject)는 파괴
            Destroy(gameObject);
        }
    }
}