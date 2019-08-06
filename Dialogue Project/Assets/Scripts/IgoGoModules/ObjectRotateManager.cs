﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectRotateManager : UsingObject
{
    [Space(20)]
    [Tooltip("Смещение от стартового вращения")] public Vector3 rotVector;
    [Tooltip("Скорость движения")] public float speed;
    [Tooltip("Задержка перед запуском")] public float delay;
    [Tooltip("Цикличные движения туда-обратно")] public bool reverce;
    [Tooltip("Задержка между циклами")] public float pauseTime;
    [Tooltip("Активно сразу")] [Space(20)] public bool active;

    [Space(20)]
    [Header("Настройки дебага")]
    [Tooltip("Нужен для дебага. Создайте пустышку дочерним объектом.")] public Transform helper;
    [Tooltip("Дальность линии от центра"), Range(1,10)] public float range = 1;

    #region Служебные
    private SimpleHandler rotHandler;
    private Quaternion rot1;
    private Quaternion rot2;
    private Quaternion currentTargetRot;
    Vector3[] points = new Vector3[4];

    private bool pause;
    #endregion

    private bool Conclude
    {
        get
        {
            if (Quaternion.Angle(transform.rotation, currentTargetRot) > 1)
            {
                return false;
            }
            return true;
        }
    }

    void Start()
    {
        rot1 = transform.rotation;
        rot2 = rot1 * Quaternion.Euler(rotVector);
        pause = false;
        if (reverce)
        {
            rotHandler = ReverceRotate;
            currentTargetRot = rot2;
        }
        else
        {
            rotHandler = ForwardRotate;
            currentTargetRot = rot1;
        }
    }

    void Update()
    {
        rotHandler();
    }

    public override void Use()
    {
        Invoke("Action", delay);
    }
    private void ForwardRotate()
    {
        if (active && !pause)
        {
            if (Conclude)
            {
                transform.rotation = currentTargetRot;
                active = false;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, currentTargetRot, speed * Time.deltaTime);
            }
        }
    }
    private void ReverceRotate()
    {
        if (active && !pause)
        {
            if (Conclude)
            {
                pause = true;
                Invoke("ChangeTarget", pauseTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, currentTargetRot, speed * Time.deltaTime);
            }
        }
    }
    private void ChangeTarget()
    {
        if (currentTargetRot == rot1)
        {
            currentTargetRot = rot2;
        }
        else
        {
            currentTargetRot = rot1;
        }
        rotVector *= -1;
        pause = false;
    }
    private void Action()
    {
        if (reverce)
        {
            active = !active;
        }
        else
        {
            active = true;
        }
        ChangeTarget();
        pause = false;
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            rot1 = transform.rotation;
            rot2 = rot1 * Quaternion.Euler(rotVector);
            helper.position = transform.position;
            Gizmos.color = Color.cyan;

            currentTargetRot = rot1;
            helper.rotation = rot1;
            points[0] = helper.position + helper.forward * range;

            currentTargetRot = Quaternion.Lerp(rot1, rot2, 0.25f);
            helper.rotation = currentTargetRot;
            points[1] = helper.position + helper.forward * range;

            currentTargetRot = Quaternion.Lerp(rot1, rot2, 0.75f);
            helper.rotation = currentTargetRot;
            points[2] = helper.position + helper.forward * range;

            currentTargetRot = rot2;
            helper.rotation = currentTargetRot;
            points[3] = helper.position + helper.forward * range;

            Gizmos.DrawSphere(points[0], 0.3f);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.cyan, null, 3f);
        }
    }

}