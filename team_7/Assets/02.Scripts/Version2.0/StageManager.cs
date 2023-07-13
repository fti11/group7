using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance; // ���� �Ŵ��� �ν��Ͻ�

    private Dictionary<int, bool> monsterDeathStatus = new Dictionary<int, bool>(); // ���� ID�� ��� ���� ����

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
        // �� ��ȯ �� ����Ǵ� ������ �����մϴ�.
        // �ʿ信 ���� ���������� �ʱ�ȭ�ϰų� ���ϴ� ������ ������ �� �ֽ��ϴ�.
    }

    public void StartStage(string stageName)
    {
        // �������� ���� ������ �����մϴ�.
    }

    // ���� ��� ó��
    public void OnMonsterDeath(int monsterID)
    {
        monsterDeathStatus[monsterID] = true;

        // ���� ���� ���� üũ �Ǵ� �ʿ��� ���� ����
        CheckGameOverCondition();
        // �ʿ��� ���� ����
    }

    // ���� ���� ���� üũ
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
            // ���� ���� ó��
        }
    }
}
