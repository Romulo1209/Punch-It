using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private bool holding;
    [SerializeField] private bool dropping;

    [Header("Inertia Values")]
    [SerializeField] private float inertiaVelocity = 5;
    [SerializeField] private float inertiaIncrease = .5f;

    [Header("Stack Informations")]
    [SerializeField] private int stackCount;
    [SerializeField] private int maxStackCount;
    [SerializeField] private List<GameObject> bodies;

    [SerializeField] private Transform bodiesStartPostionTransform;
    [SerializeField] private Transform bodiesDropPosition;

    public static StackController Instance;

    //Getters & Setters

    public int MaxStackCount { get { return maxStackCount; } set { maxStackCount = value; } }
    public bool Dropping { get { return dropping; } set { dropping = value; } }

    private void Awake() => Instance = this;

    private void Update() {
        if(!dropping)
            UpdatePositions();
        holding = CheckIfHoldingBodies();
    }

    #region Body Control

    bool CheckIfCanStackBody() {
        if (bodies.Count < maxStackCount)
            return true;
        return false;
    }
    Vector3 GetBodyPosition(int bodyIndex) {
        Vector3 position = new Vector3(bodiesStartPostionTransform.position.x, bodiesStartPostionTransform.position.y + (1f * bodyIndex), bodiesStartPostionTransform.position.z);
        return position;
    }
    bool CheckDupped(GameObject body) {
        foreach(GameObject bodyCheck in bodies) {
            if (bodyCheck == body)
                return true;
        }
        return false;
    }
    public void StackBody(GameObject TouchedBody)
    {
        if (CheckIfCanStackBody()) {
            if (CheckDupped(TouchedBody))
                return;

            TouchedBody.GetComponent<EnemyController>().StackEnemy(true);
            inertiaVelocity += inertiaIncrease;
            bodies.Add(TouchedBody);
            TouchedBody.transform.position = GetBodyPosition(bodies.Count - 1);
            CameraController.Instance.ChangeCameraPosition(bodies.Count);
        }
        else
            return;
    }
    void UpdatePositions()
    {
        if (bodies.Count > 0) {
            
            int index = 0;
            float parts = inertiaVelocity / bodies.Count;
            foreach (GameObject body in bodies) {
                var targetRotation = GetComponent<PlayerController>().PlayerMesh.transform.rotation;
                body.transform.rotation = Quaternion.Lerp(body.transform.rotation, targetRotation, Time.deltaTime * inertiaVelocity);
                body.transform.position = Vector3.Lerp(body.transform.position, GetBodyPosition(index), Time.deltaTime * (inertiaVelocity - (parts * index)));
                index++;
            }
        }
        else
            GetComponent<PlayerController>().holding = false;
    }

    #endregion

    bool CheckIfHoldingBodies() {
        if (bodies.Count != 0) {
            GetComponent<PlayerController>().holding = true;
            return true;
        }
        else {
            GetComponent<PlayerController>().holding = false;
            return false;
        }
    }

    public void DropBody()
    {
        dropping = true;
        foreach(GameObject body in bodies) {
            body.transform.position = Vector3.MoveTowards(body.transform.position, bodiesDropPosition.position, 25 * Time.deltaTime);
            if(body.transform.position == bodiesDropPosition.position) {
                body.GetComponent<EnemyController>().StackEnemy(false);
                bodies.Remove(body);
                break;
            }
                
        }
    }
}
