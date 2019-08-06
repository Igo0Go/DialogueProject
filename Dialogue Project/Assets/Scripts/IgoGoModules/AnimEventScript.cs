using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventScript : UsingOrigin
{
    [Space(20)]
    public bool once;

    public override void Use()
    {
        foreach (var item in actionObjects)
        {
            item.Use();
        }
        if(once)
        {
            Destroy(gameObject);
        }
    }
}
