using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentsController : MonoBehaviour, IAnimatedPlayer
{
    public Transform friendTransform;
    public Animator anim;
    [Range(5,50)]
    public float friendZone;

    private NavMeshAgent agent;
    private bool walk;
    private int alone;


    public void StopControl()
    {
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.isStopped = true;
        //agent.enabled = false;
    }

    public bool RandomStay { get; set; }
    public int AnimNumber { get; set; }

    public void DefaultStay()
    {
        RandomStay = false;
    }
    public void RandomAnim()
    {
        RandomStay = true;
        AnimNumber = Random.Range(-1, 1);
        if (AnimNumber < 0)
        {
            AnimNumber = -1;
        }
        else
        {
            AnimNumber = 1;
        }
        anim.SetInteger("RandomStay", AnimNumber);
    }


    private void OnEnable()
    {
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        //agent.enabled = true;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        AgentAction();
    }

    private void AgentAction()
    {
        alone = Vector3.Distance(transform.position, friendTransform.position) > friendZone ? 1 : 0;

        if (alone == 1)
        {
            walk = true;
        }
        else if(alone == 0)
        {
            Invoke("ReturnWalk", 1);
        }

        walk = Vector3.Distance(transform.position, friendTransform.position) > friendZone;

        if (walk)
        {
            agent.destination = friendTransform.position;
            agent.isStopped = false;
            anim.SetFloat("Zaxis", 1, 10 * Time.deltaTime, Time.deltaTime);
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Zaxis", 0, 10 * Time.deltaTime, Time.deltaTime);
            if (!RandomStay)
            {
                anim.SetInteger("RandomStay", 0);
            }
        }
    }

    private void ReturnWalk()
    {
        walk = false;
    }
}
