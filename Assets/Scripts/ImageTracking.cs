using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] boards;

    private Dictionary<string, GameObject> spawnedBoard = new Dictionary<string, GameObject>();
    private ARTrackedImageManager imageManager;

    private void Awake()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject board in boards)
        {
            GameObject newBoard = Instantiate(board, Vector3.zero, Quaternion.identity);
            newBoard.name = board.name;
            spawnedBoard.Add(board.name, newBoard);
        }
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            Debug.Log($"Imagem adicionada: {trackedImage.referenceImage.name}");
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            Debug.Log($"Imagem atualizada: {trackedImage.referenceImage.name}");
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            Debug.Log($"Imagem removida: {trackedImage.referenceImage.name}");
            spawnedBoard[trackedImage.referenceImage.name].SetActive(false);
        }
    }


    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedBoard[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach(GameObject go in spawnedBoard.Values)
        {
            if(go.name != name)
            {
                go.SetActive(false);
            }
        }
    }
}

