using UnityEngine;
using System.Collections;

public class Symbol3DViewer : MonoBehaviour
{
    public static Symbol3DViewer Instance;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public float visibleDuration = 2.5f;
    public float rotationSpeed = 30f;

    private GameObject currentModel;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowSymbol(GameObject symbolPrefab)
    {
        // Stop previous hide coroutine
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        // Destroy previous model
        if (currentModel != null)
            Destroy(currentModel);

        // Spawn new model
        currentModel = Instantiate(symbolPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Spawning: " + symbolPrefab.name);
        currentModel.transform.SetParent(transform, true);

        // Start new hide coroutine
        hideCoroutine = StartCoroutine(AutoHide());
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(visibleDuration);

        if (currentModel != null)
            Destroy(currentModel);

        hideCoroutine = null;
    }

    private void Update()
    {
        if (currentModel != null)
        {
            currentModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
