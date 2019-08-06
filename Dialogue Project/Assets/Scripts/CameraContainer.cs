using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour
{
    public Transform camTarget;
    public int controllerNumber;

    private Vector3 bufer;

    private void OnDrawGizmos()
    {
        if(camTarget != null)
        {
            Gizmos.color = Color.cyan;
            bufer = transform.position + Vector3.up;
            Gizmos.DrawLine(camTarget.transform.position, bufer);
            bufer = transform.position + camTarget.transform.right;
            Gizmos.DrawLine(camTarget.transform.position, bufer);
            bufer = transform.position - camTarget.transform.right;
            Gizmos.DrawLine(camTarget.transform.position, bufer);
        }
    }
}
