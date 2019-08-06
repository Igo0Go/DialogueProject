using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAnimatedPlayer
{
    bool RandomStay { get; set; }
    int AnimNumber { get; set; }
    void RandomAnim();
    void DefaultStay();
}

public class AnimTranslator : MonoBehaviour
{
    public TPC tPC;
    public AgentsController agent;

    [HideInInspector]public bool activePlayer;

    private void StartRandomPos()
    {
        tPC.RandomAnim();
        agent.RandomAnim();
    }
    private void StartDefaultPos()
    {
        tPC.DefaultStay();
        agent.DefaultStay();
    }
}
