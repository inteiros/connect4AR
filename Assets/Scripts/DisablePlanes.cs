using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARPlaneManager))]
public class DisablePlanes : MonoBehaviour
{
    private ARPlaneManager planeManager;

    [SerializeField]
    private Text toggleButtonText;
    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}