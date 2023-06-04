using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTileMove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    private void FixedUpdate()
    {
        //시간대비 이동 량 float 값으로 선언
        float moveAmount = 10 * Time.fixedDeltaTime;
        //launchDirection 방향으로 발사체 이동 (Translate) 이동 시키는 함수
        transform.Translate(Vector3.forward * moveAmount);
    }
}
