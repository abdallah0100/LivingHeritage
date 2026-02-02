using UnityEngine;
using Vuforia;
using TMPro;
using System.Collections;

public class YigalMosaicHandler : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject yigalPrefab;

    [Header("UI")]
    public TextMeshProUGUI statusText;
    public GameObject skipPanel;
    public TextMeshProUGUI subtitlesText;

    [Header("Sand Overlay (Task 1)")]
    public GameObject sandOverlay;
    [Range(0f, 2f)] public float sandPaddingPercent = 1.0f;
    public float sandYOffset = -0.002f;
    public bool forceFlatRotation = true;

    [Header("Rubble Spawning (Task 2)")]
    public GameObject[] rubblePrefabs; // sand pile + rock_branch
    public int totalRubbleCount = 5;   // TOTAL, not per prefab
    public float rubbleY = 0.006f;
    [Range(0.1f, 0.49f)] public float safeMargin = 0.45f;
    public Vector2 rubbleScaleRange = new Vector2(0.07f, 0.12f);

    private bool hasTriggered = false;
    private Transform rubbleRoot;

    public bool gameStarted = false;
    private bool targetListenerBound = false;

    [Header("Instances")]
    public GameObject uiInstance;
    private UIController mosaicUIController;

    [Header("Completion Flags (Task 3)")]
    public bool allRubbleCleared = false;
    public bool sandCleared = false;
    public bool areaFullyCleaned = false;

    private int rubbleRemaining = 0;


    void Start()
    {
        
        mosaicUIController = uiInstance.GetComponent<UIController>();

        var observerEventHandler = GetComponent<DefaultObserverEventHandler>();
        if (observerEventHandler != null && gameStarted)
        {
            observerEventHandler.OnTargetFound.AddListener(OnTargetFound);
        }

        if (sandOverlay != null)
            sandOverlay.SetActive(false);
    }

    void Update()
    {
       if (gameStarted && !targetListenerBound)
        {
            var observerEventHandler = GetComponent<DefaultObserverEventHandler>();
            if (observerEventHandler != null)
            {
                observerEventHandler.OnTargetFound.AddListener(OnTargetFound);
                targetListenerBound = true;
            }
        } 
    }

    void OnTargetFound()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        Debug.Log("Mosaic detected");
        mosaicUIController.stage = Stage.PostScan;
        mosaicUIController.toggleTools(true);

        if (statusText != null)
            statusText.text = "Mosaic located";

        // --- Task 1: Sand ---
        if (sandOverlay != null)
        {
            SetupSandOverlay();
            sandOverlay.SetActive(true);
        }

        // --- Task 2: Rubble ---
        SpawnRubble();
    }

    // =========================
    // TASK 1 — Sand
    // =========================
    private void SetupSandOverlay()
    {
        sandOverlay.transform.SetParent(transform, false);

        var imageTarget = GetComponent<ImageTargetBehaviour>();
        if (imageTarget == null) return;

        Vector2 size = imageTarget.GetSize();

        float w = size.x * (1f + sandPaddingPercent);
        float h = size.y * (1f + sandPaddingPercent);

        sandOverlay.transform.localScale = new Vector3(w, h, 1f);
        sandOverlay.transform.localPosition = new Vector3(0f, sandYOffset, 0f);

        if (forceFlatRotation)
            sandOverlay.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        // Resize collider to match overlay size
        BoxCollider col = sandOverlay.GetComponent<BoxCollider>();
        if (col != null)
        {
            col.center = Vector3.zero;
            col.size = new Vector3(1f, 1f, 0.01f);
        }

    }

    // =========================
    // TASK 2 — Rubble
    // =========================
    private void EnsureRubbleRoot()
    {
        if (rubbleRoot != null) return;

        GameObject root = new GameObject("RubbleRoot");
        root.transform.SetParent(transform, false);
        rubbleRoot = root.transform;
    }

    private void SpawnRubble()
    {
        if (rubblePrefabs == null || rubblePrefabs.Length == 0)
        {
            Debug.LogWarning("No rubble prefabs assigned.");
            return;
        }

        EnsureRubbleRoot();

        // Clear old rubble (safety)
        for (int i = rubbleRoot.childCount - 1; i >= 0; i--)
            Destroy(rubbleRoot.GetChild(i).gameObject);

        // Reset completion state for a fresh run
        rubbleRemaining = 0;
        allRubbleCleared = false;
        areaFullyCleaned = false;

        var imageTarget = GetComponent<ImageTargetBehaviour>();
        if (imageTarget == null) return;

        Vector2 size = imageTarget.GetSize();
        float halfW = size.x * 0.5f * safeMargin;
        float halfH = size.y * 0.5f * safeMargin;

        for (int i = 0; i < totalRubbleCount; i++)
        {
            GameObject prefab = rubblePrefabs[Random.Range(0, rubblePrefabs.Length)];
            GameObject piece = Instantiate(prefab, rubbleRoot);

            float x = Random.Range(-halfW, halfW);
            float z = Random.Range(-halfH, halfH);

            piece.transform.localPosition = new Vector3(x, rubbleY, z);
            piece.transform.localRotation = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);

            float s = Random.Range(rubbleScaleRange.x, rubbleScaleRange.y);
            piece.transform.localScale = Vector3.one * s;

            // Hook into cleaning
            var cleanable = piece.GetComponent<RubbleCleanable>();
            if (cleanable != null)
            {
                rubbleRemaining++;
                cleanable.onCleaned = OnRubblePieceCleaned;
            }
            else
            {
                Debug.LogWarning($"Rubble prefab '{prefab.name}' is missing RubbleCleanable!");
            }
        }

        // Edge case: if none had RubbleCleanable
        if (rubbleRemaining == 0)
            OnRubblePieceCleaned(null);
    }


    // =========================
    // Yigal spawn (unchanged)
    // =========================
    private IEnumerator DelayAndSpawnYigal(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (statusText != null)
            statusText.gameObject.SetActive(false);

        if (skipPanel != null)
            skipPanel.SetActive(true);

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 mosaicPosition = transform.position;

        Vector3 lateralOffset = transform.right * 0.25f;
        Vector3 backwardOffset = -transform.forward * 0.25f;
        Vector3 verticalOffset = Vector3.up * 0.05f;

        Vector3 yigalPosition = mosaicPosition + lateralOffset + backwardOffset + verticalOffset;

        Vector3 lookDirection = cameraPosition - yigalPosition;
        lookDirection.y = 0;
        Quaternion yigalRotation = Quaternion.LookRotation(lookDirection);

        GameObject yigal = Instantiate(yigalPrefab, yigalPosition, yigalRotation);
        yigal.transform.localScale = new Vector3(0.25f, 0.2f, 0.25f);

        var speech = yigal.GetComponent<YigalSpeech>();
        if (speech != null)
        {
            speech.setSubTitleComp(subtitlesText);
            speech.mosaicTransform = transform;
            speech.StartYigalSpeech();
        }
    }

    private void OnRubblePieceCleaned(RubbleCleanable cleanedPiece)
    {
        rubbleRemaining--;

        if (rubbleRemaining <= 0 && !allRubbleCleared)
        {
            allRubbleCleared = true;
            Debug.Log("All rubble cleared");
            UpdateAreaFullyCleaned();
        }
    }


    public void NotifySandCleared()
    {
        if (sandCleared) return;
        sandCleared = true;
        Debug.Log(" Sand cleared");
        UpdateAreaFullyCleaned();
    }

    private void UpdateAreaFullyCleaned()
    {
        areaFullyCleaned = allRubbleCleared && sandCleared;

        if (areaFullyCleaned)
        {
            Debug.Log(" areaFullyCleaned = TRUE (rubble + sand cleared)");

            StartCoroutine(DelayAndSpawnYigal(3f));
        }
    }


}
