using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 starterPosition = new Vector3(0, 7, -10);
    [SerializeField] private Vector3 actualPosition;
    [SerializeField] private float cameraTransitionVelocity = 2f;

    [SerializeField] private CinemachineVirtualCamera virtualCam;

    public static CameraController Instance;
    private void Awake() {
        Instance = this;
        actualPosition = starterPosition;
    }
    public void ChangeCameraPosition(int carringCount)
    {
        Vector3 newPosition = starterPosition + new Vector3(0, 1 * (carringCount / 2), -2 * (carringCount / 2));
        actualPosition = newPosition;
    }
    private void Update()
    {
        virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, actualPosition, Time.deltaTime * cameraTransitionVelocity);
    }
}
