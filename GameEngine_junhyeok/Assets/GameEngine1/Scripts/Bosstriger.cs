using UnityEngine;
using Unity.Cinemachine;  // Cinemachine 3: namespace 변경

public class CameraSwitchTrigger : MonoBehaviour
{
    [Header("전환할 카메라")]
    public CinemachineCamera targetCamera;  // Cinemachine 3: CinemachineVirtualCamera → CinemachineCamera
    
    [Header("우선순위 설정")]
    public int highPriority = 15;
    public int lowPriority = 5;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetCamera.Priority = highPriority;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetCamera.Priority = lowPriority;
        }
    }
}
