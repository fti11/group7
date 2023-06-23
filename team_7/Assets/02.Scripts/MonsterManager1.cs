using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager1 : MonoBehaviour
{
    public GameObject[] monsters;  // 1번 씬에 있는 몬스터들을 저장할 배열

    private void Update()
    {
        // 1번 씬에 있는 모든 몬스터가 죽었는지 확인
        bool allMonstersDead = true;

        foreach (GameObject monster in monsters)
        {
            if (monster != null)
            {
                // 몬스터가 아직 살아있으면 allMonstersDead를 false로 설정
                allMonstersDead = false;
                break;
            }
        }

        // 모든 몬스터가 죽었으면 2번 씬으로 전환
        if (allMonstersDead)
        {
            SceneManager.LoadScene("04.Ending");  // 2번 씬으로 전환 (씬 이름에 실제 씬의 이름을 입력해야 합니다)
        }
    }
}