using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance; // 게임 매니저 인스턴스

    private Dictionary<int, bool> monsterDeathStatus = new Dictionary<int, bool>(); // 몬스터 ID와 사망 상태 매핑

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 전환 시 실행되는 로직을 구현합니다.
        // 필요에 따라 스테이지를 초기화하거나 원하는 동작을 수행할 수 있습니다.
    }

    public void StartStage(string stageName)
    {
        // 스테이지 시작 로직을 구현합니다.
    }

    // 몬스터 사망 처리
    public void OnMonsterDeath(int monsterID)
    {
        monsterDeathStatus[monsterID] = true;

        // 게임 오버 조건 체크 또는 필요한 동작 수행
        CheckGameOverCondition();
        // 필요한 로직 구현
    }

    // 게임 오버 조건 체크
    private void CheckGameOverCondition()
    {
        bool allMonstersDead = true;
        foreach (bool deathStatus in monsterDeathStatus.Values)
        {
            if (!deathStatus)
            {
                allMonstersDead = false;
                break;
            }
        }

        if (allMonstersDead)
        {
            // 게임 오버 처리
        }
    }
}
