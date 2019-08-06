using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DialogueController : MonoBehaviour
{
    public Transform playerTransform;
    public DialogueCharacter dialogueCharacter;

    protected Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void ToDialogue(Transform positioning)
    {
        playerTransform.position = positioning.position;
        playerTransform.rotation = positioning.rotation;
        anim.SetInteger("TalkType", 0);
        anim.SetInteger("TalkStatus", 1);
    }
    public void ToDefault()
    {
        anim.SetInteger("TalkType", 0);
        anim.SetInteger("TalkStatus", 0);
    }
    public void StopReplic()
    {
        anim.SetInteger("TalkType", 0);
        anim.SetInteger("TalkStatus", 1);
    }
    public void SetTalkType(DialogueAnimType type)
    {
        anim.SetInteger("TalkStatus", 2);
        anim.SetInteger("TalkType", InverseTypeToInt(type));
    }
    protected int InverseTypeToInt(DialogueAnimType type)
    {
        switch (type)
        {
            case DialogueAnimType.Talk:
                return 1;
            case DialogueAnimType.Yes:
                return 2;
            case DialogueAnimType.No:
                return 3;
            case DialogueAnimType.Quastion:
                return 4;
            case DialogueAnimType.InSurprise:
                return 5;
            case DialogueAnimType.Nervously:
                return 6;
            case DialogueAnimType.Fear:
                return 7;
        }
        return 0;
    }
}
