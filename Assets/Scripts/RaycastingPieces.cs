using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class RaycastingPieces : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;

    [SerializeField]
    private GameObject redPrefab;

    [SerializeField]
    private GameObject bluePrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private bool isRedTurn = true;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        Debug.Log("Raycast Manager inicializado.");
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            Debug.Log("Toque detectado na posi��o: " + touchPosition);
            return true;
        }

        touchPosition = default;
        Debug.Log("Nenhum toque detectado.");
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (raycastManager.Raycast(touchPosition, s_Hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            Debug.Log("Raycast bem-sucedido. Posi��o: " + hitPose.position);

            GameObject prefabToSpawn = isRedTurn ? redPrefab : bluePrefab;
            Debug.Log(isRedTurn ? "Spawnando pe�a vermelha." : "Spawnando pe�a azul.");

            spawnedObject = Instantiate(prefabToSpawn, hitPose.position, hitPose.rotation);

            isRedTurn = !isRedTurn;
            Debug.Log("Alternando turno. Pr�ximo turno: " + (isRedTurn ? "Vermelho" : "Azul"));
        }
        else
        {
            Debug.Log("Raycast falhou.");
        }
    }
}
