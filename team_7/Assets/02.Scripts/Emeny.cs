using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emeny : MonoBehaviour
{
    public int Health = 10;

    public GameObject DestroyedEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int attackpower)
    {
        Health -= attackpower;

        if(Health <= 0)
        {
            GameObject Temp = Instantiate(DestroyedEffect);
            Temp.transform.position = this.gameObject.transform.position;
            Destroy(Temp, 3.0f);
            Destroy(this.gameObject);

        }


    }
}
