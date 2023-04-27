using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    private Transform mainCamera;

    private void Start() => mainCamera = GameObject.Find("MainCamera").transform;
    private void Update() => transform.LookAt(2 * transform.position - mainCamera.position);
}
