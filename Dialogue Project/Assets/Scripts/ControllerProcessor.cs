using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public delegate void AnswerHandler(int number);

[RequireComponent(typeof(AudioSource))]
public class ControllerProcessor : MonoBehaviour
{
    public List<TPC> players = new List<TPC>();
    [HideInInspector] public bool inDialog;
    public GameObject tipPanel;
    public Text tipText;
    public Transform obstacle;

    public event AnswerHandler OnChooseAnswer;

    private List<AgentsController> playerAgents = new List<AgentsController>();
    private AudioSource source;
    private Transform lookTarget;
    private Vector3 lookDirBufer;
    private Vector3 camPosBufer;
    private Quaternion camRotBufer;
    private int currentActivePlayer;
    private bool moveLook;


    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
        foreach (var item in players)
        {
            playerAgents.Add(item.GetComponent<AgentsController>());
            item.ChangeController += CheckPlayer;
            item.ChangeCamTarget += CheckCamTarget;
            item.ChangeTip += EnableTip;
            item.SimpleEvent += DisableTip;
        }
        CheckPlayer(0);
        CheckCamTarget(transform);
    }
    private void LateUpdate()
    {
        if(!inDialog)
        {
            CameraLook();
        }
    }

    private void Update()
    {
        RayDown();
    }

    public void ReturnFromDialogue()
    {
        transform.position = camPosBufer;
        transform.rotation = camRotBufer;
        CheckPlayer(0);
    }
    public void OnAnswerClick(int number)
    {
        OnChooseAnswer?.Invoke(number);
    }

    private void CheckPlayer(int number)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].active = false;
            players[i].enabled = false;
            playerAgents[i].enabled = true;
            playerAgents[i].enabled = true;

        }
        lookTarget = playerAgents[number].transform;
        moveLook = true;
        playerAgents[number].StopControl();
        playerAgents[number].enabled = false;
        players[number].enabled = true;
        players[number].active = true;
        if(currentActivePlayer != number)
        {
            StartReplica(players[number].changeReplick[Random.Range(0, players[number].changeReplick.Count)]);
            currentActivePlayer = number;
        }
    }
    private void CheckCamTarget(Transform target)
    {
        DisableTip();
        camPosBufer = target.position;
        camRotBufer = target.rotation;
        players[currentActivePlayer].SetCamTarget(target);
    }
    private void CameraLook()
    {
        if(moveLook)
        {
            lookDirBufer = (lookTarget.position + Vector3.up) - transform.position;

            transform.forward = Vector3.RotateTowards(transform.forward, lookDirBufer, Time.deltaTime, Time.deltaTime);

            if (Vector3.Angle(transform.forward, lookDirBufer) < 3)
            {
                moveLook = false;
            }
        }
        else
        {
            transform.LookAt(lookTarget.position + Vector3.up);
        }
    }

    public void EnableTip(string tipText)
    {
        tipPanel.SetActive(true);
        this.tipText.text = tipText;
    }
    public void DisableTip()
    {
        tipPanel.SetActive(false);
    }

    private void RayDown()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            obstacle.position = hit.point + Vector3.up * 0.5f;
        }
    }

    private void StartReplica(AudioClip clip)
    {
        if(!source.isPlaying)
        {
            source.clip = clip;
            source.Play();
        }
    }
}
