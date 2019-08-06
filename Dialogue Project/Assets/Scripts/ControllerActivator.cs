using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerActivator : MonoBehaviour
{
    public void ControllerOn()
    {
        Invoke("Action", 3);
    }

    private void Action()
    {
        GetComponent<ControllerProcessor>().enabled = true;
    }
}
