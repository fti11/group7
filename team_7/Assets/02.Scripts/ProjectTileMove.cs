using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTileMove : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Monster")
        {
            other.gameObject.GetComponent<Enemy>().Damage(1);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    private void FixedUpdate()
    {
        //�ð���� �̵� �� float ������ ����
        float moveAmount = 75 * Time.fixedDeltaTime;
        //launchDirection �������� �߻�ü �̵� (Translate) �̵� ��Ű�� �Լ�
        transform.Translate(Vector3.forward * moveAmount);
    }
}
