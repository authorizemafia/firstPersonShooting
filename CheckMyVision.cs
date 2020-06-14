using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMyVision : MonoBehaviour
{
    public enum enumsensitivity { HIGH, LOW }

    public enumsensitivity sensitivity = enumsensitivity.HIGH;

    public bool TargetinSight = false;

    public float FOV = 45f;

    private Transform target = null;

    public Transform MyEyes = null;

    public Transform npcTransform = null;

    private SphereCollider sphereCollider = null;

    public Vector3 LastknowSight = Vector3.zero;

    public void Awake()
    {
        npcTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        LastknowSight = npcTransform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    bool InMyFOV()
    {
        Vector3 dirToTarget = target.position - MyEyes.position;
        float angle = Vector3.Angle(MyEyes.forward, dirToTarget);
        if (angle <= FOV)
        {
            return true;
        }
        else
            return false;

    }

    bool clearLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(MyEyes.position, (target.position - MyEyes.position).normalized, out hit, sphereCollider.radius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }

        }
        return false;
    }
    private void updateSight()
    {
        switch (sensitivity)
        {
            case enumsensitivity.HIGH:
                TargetinSight = InMyFOV() && clearLineOfSight();
                break;
            case enumsensitivity.LOW:
                TargetinSight = InMyFOV() || clearLineOfSight();
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        updateSight();
        if (TargetinSight)
            LastknowSight = target.position;

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        TargetinSight = false;
    }


}
