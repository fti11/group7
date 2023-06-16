using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveTarget : MonoBehaviour
{
    public enum AGENTTYPE
    {
        ENEMY,
        PLAYER,
        PET
    }


    public Vector3 genPoint;

    public float searchTimer = 0.0f;
    public float searchLoop = 2.0f;

    public NavMeshAgent agent;

    public AGENTTYPE agenttype = AGENTTYPE.ENEMY;

    // Start is called before the first frame update
    void Start()
    {
        genPoint = this.gameObject.transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = genPoint;
    }

    // Update is called once per frame
    void Update()
    {
        DoSearchLoop();
       
    }

    void DoSearchLoop()
    {
        searchTimer += 1;
        if (searchLoop <= searchTimer)
        {
            searchTimer = 0;

            if(agenttype == AGENTTYPE.ENEMY)
            {
                GameObject temp = GameObject.FindGameObjectWithTag("Player");

                if (temp != null)
                {
                    if(Vector3.Distance(temp.transform.position , this.gameObject.transform.position) < 50.0f)
                    {
                        agent.destination = temp.transform.position;
                    }
                    else
                    {
                        agent.destination = genPoint;
                    }
                  
                }
            }
          
        }
    }
}
