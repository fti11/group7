using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance; // ���� �Ŵ��� �ν��Ͻ�

    public GameObject monsterObject;

    public Vector3[] monsterGenPoint_Stage_01 = new Vector3[6];
    public Vector3[] monsterGenPoint_Stage_02 = new Vector3[6];

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

        StartStage(scene.name);

    }

    public void StartStage(string stageName)
    {
        // �������� ���� ������ �����մϴ�.
        if(stageName == "03.GameScene")
        {
            for(int i = 0; i < monsterGenPoint_Stage_02.Length; i++)
            {
                GameObject temp = (GameObject)Instantiate(monsterObject);
                temp.transform.position = monsterGenPoint_Stage_02[i];
                temp.GetComponent<Enemy>().monsterID = i;

                monsterDeathStatus.Add(i, false);

                Debug.Log(" i  : " + i);
            }
        }
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
            Debug.Log("STAGE_END");
            SceneManager.LoadScene("00.Title");
        }
    }
}
