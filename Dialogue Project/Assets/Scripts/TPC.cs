using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ChangeTargetHandler(Transform target);
public delegate void ChangeControllerHandler(int number);
public delegate void ChangeTip(string text);

[RequireComponent(typeof(CharacterController))]
public class TPC : MyTools, IAnimatedPlayer
{
    public Transform cam;
    public float speed;
    public Animator anim;

    public List<AudioClip> changeReplick = new List<AudioClip>();
 

    [HideInInspector]public bool active;


    private CameraContainer cameraContainer;
    private CharacterController characterController;
    private Vector3 camForward;
    private Vector3 moveVector;
    private Transform camTarget;
    private bool moveCam;

    public event ChangeControllerHandler ChangeController;
    public event ChangeTargetHandler ChangeCamTarget;
    public event ChangeTip ChangeTip;
    public event System.Action SimpleEvent;

    public bool RandomStay { get; set; }
    public int AnimNumber { get; set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if(active)
        {
            MoveCam();
            Move();
        }
    }

    private void Move()
    {
        float h, v;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(h != 0 || v != 0)
        {
            camForward = cam.forward;
            camForward.y = 0;

            moveVector = camForward.normalized * v + cam.right.normalized * h;
            if(moveVector != Vector3.zero)
            {
                anim.SetFloat("Zaxis", Mathf.Clamp01(Mathf.Abs(v) + Mathf.Abs(h)));
                transform.forward = moveVector;
                characterController.Move(moveVector * speed * Time.deltaTime);
            }
        }
        else
        {
            anim.SetFloat("Zaxis", 0);
            if (!RandomStay)
            {
                anim.SetInteger("RandomStay", 0);
            }
        }
    }
    private void MoveCam()
    {
        if (moveCam)
        {
            MoveCameraToTarget();
        }
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
    public void DefaultStay()
    {
        RandomStay = false;
    }
    public void SetCamTarget(Transform target)
    {
        camTarget = target;
        moveCam = true;
    }
    private void MoveCameraToTarget()
    {
        if(Vector3.Distance(cam.position, camTarget.position) > 0.1f)
        {
            cam.position = Vector3.Slerp(cam.position, camTarget.position, Time.deltaTime);
        }
        else
        {
            cam.position = camTarget.position;
            moveCam = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(active)
        {
            if (other.tag.Equals("CameraChanger"))
            {
                cameraContainer = other.GetComponent<CameraContainer>();
                SetCamTarget(cameraContainer.camTarget);
                ChangeController.Invoke(cameraContainer.controllerNumber);
                ChangeCamTarget.Invoke(cameraContainer.camTarget);
            }
            else if (other.tag.Equals("Scene"))
            {
                ChangeTip?.Invoke(other.GetComponent<DialogueScenePoint>().tip);
            }
            else if (other.tag.Equals("Module"))
            {
                UsingObject usingObject;
                if (MyGetComponent(other.gameObject, out usingObject))
                {
                    usingObject.Use();
                    if (usingObject is LocationReactor)
                    {
                        if (((LocationReactor)usingObject).once)
                        {
                            Destroy(other.gameObject);
                        }
                    }
                }
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && active && other.tag.Equals("Scene"))
        {
            other.GetComponent<DialogueScenePoint>().StartScene();
            active = false;
            SimpleEvent?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (active)
        {
            if (other.tag.Equals("Scene"))
            {
                SimpleEvent?.Invoke();
            }
        }
        else if (other.tag.Equals("Module"))
        {
            UsingObject usingObject;
            if (MyGetComponent(other.gameObject, out usingObject))
            {
                if (usingObject is LocationReactor)
                {
                    if (!((LocationReactor)usingObject).enterOnly)
                    {
                        usingObject.Use();
                    }
                }
            }
        }
    }

    public override void OnEvent()
    {
        throw new System.NotImplementedException();
    }
}
