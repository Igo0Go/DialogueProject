using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueController : DialogueController
{
    public TPC tPC;
    public AgentsController agentsController;

    public override void ToDialogue(Transform positioning)
    {
        tPC.enabled = false;
        anim.SetInteger("TalkStatus", 1);
        agentsController.StopControl();
        agentsController.enabled = false;
        playerTransform.position = positioning.position;
        playerTransform.rotation = positioning.rotation;
    }
}
