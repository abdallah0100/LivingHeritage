using System.Collections;
using TMPro;
using UnityEngine;

public class BeachMission : MonoBehaviour
{
    [Header("Mission Settings")]
    public float timeToFindFirstNail = 10f;
    public float timeToFindOtherNails = 15f;
    public int totalNails = 2;
    public int totalPlanks = 1;
    public float timeToFindPlank = 10f;
    public float maxObjectSpawnDistance = 20f;

    [Header("GameObject Assignments")]
    public Terrain terrain;
    public GameObject nail1;
    public GameObject nail2;
    public GameObject plank;
    public Transform player;
    public GameObject mound;
    public GameObject boat;
    public GameObject checkMarkPrefab;
    public GameObject conversationHandler;
    public GameObject uiController;

    [Header("NPC Controller")]
    public GameObject npcControllerObject;

    [Header("Score")]
    public GameObject scoreCanvas;
    public TextMeshProUGUI avgNailTime;
    public TextMeshProUGUI avgPlankTime;
    public TextMeshProUGUI overallTime;
    public TextMeshProUGUI fastestTime;

    private int nailsFound = 0;
    private int planksFound = 0;
    private float elapsedTime = 0; // used to count how long has passed
 
    private bool targetSpawned = false;
    private Vector3 spawnLoc;

    private double nailFindingTime = 0;
    private double plankFindingTime = 0;

    private ConversationHandler ch;
    private NPCController npcController;
    private BoatUIController boatUIController;
    private int timesNPCSFoundHint = 0;
    private AudioSequence sequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        GameStats.reset();
        ch = conversationHandler.GetComponent<ConversationHandler>();
        npcController = npcControllerObject.GetComponent<NPCController>();
        boatUIController = uiController.GetComponent<BoatUIController>();

        double? bestTime = await FirebaseRealtimeService.Instance.GetBestTime();
        if (bestTime.HasValue)
        {
            Debug.Log($"Best overall has been loaded(time: {bestTime.Value})");
            fastestTime.SetText(timeToString(bestTime.Value, 1));
            SessionVariables.fastestTime = bestTime.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) // game is paused
            return;
        if (npcController.wander)
            elapsedTime += Time.deltaTime;
        missionController();
        handlePlayerSearch();
        boatUIController.updateProgress(nailsFound, totalNails, planksFound, totalPlanks);
    }

    private void missionController() {
        if (nailsFound < totalNails)
            nailSearch();
        else if (planksFound < totalPlanks)
        {
            plankSearch();
        }

    }

    private void plankSearch() {
        if (!targetSpawned) {//spawn plank
            float xOffset = UnityEngine.Random.Range(0f, maxObjectSpawnDistance * 0.3f);
            float zOffset = maxObjectSpawnDistance - xOffset;

            xOffset = UnityEngine.Random.Range(0, 2) == 0 ? xOffset : -xOffset;
            zOffset = UnityEngine.Random.Range(0, 2) == 0 ? zOffset : -zOffset;

            Vector3 moundPos = mound.transform.position;
            spawnObject(plank, new Vector3(moundPos.x - xOffset, 0, moundPos.z - zOffset));

            targetSpawned = true;
            return;
        }
        if (elapsedTime >= timeToFindPlank && npcController.wander)
        {
            GameStats.wasAssisted = true;
            timesNPCSFoundHint++;
            ch.forceStop();
            StartCoroutine(guideNPCsToSpawnedObject(false));
            StartCoroutine(ch.playClip(ch.femaleFoundPlank));
        }
    }
    private void nailSearch() {
        if (!targetSpawned) {            //spawn nail here
            float xOffset = UnityEngine.Random.Range(0f, maxObjectSpawnDistance);
            float zOffset = maxObjectSpawnDistance - xOffset;

            xOffset = UnityEngine.Random.Range(0, 2) == 0 ? xOffset : -xOffset;
            zOffset = UnityEngine.Random.Range(0, 2) == 0 ? zOffset : -zOffset;

            GameObject nailVersion = UnityEngine.Random.Range(0, 2) == 0 ? nail1 : nail2;
            Vector3 playerPos = player.position;
            spawnObject(nailVersion, new Vector3(playerPos.x - xOffset, 0, playerPos.z - zOffset));

            targetSpawned = true;
            return;
        }
        if (nailsFound == 0) {
            //handle first nail look out
            if (elapsedTime >= timeToFindFirstNail && npcController.wander) {
                GameStats.wasAssisted = true;
                timesNPCSFoundHint++;
                StartCoroutine(guideNPCsToSpawnedObject(false));
                ch.forceStop();
                StartCoroutine(ch.playClip(ch.maleFoundNail1));
            }
        }
        else // not the first nail
        {
            //float requiredTime = timeToFindFirstNail + (nailsFound) * timeToFindOtherNails;
            if (elapsedTime >= timeToFindOtherNails && npcController.wander) {
                GameStats.wasAssisted = true;
                timesNPCSFoundHint++;
                StartCoroutine(guideNPCsToSpawnedObject(false));
                ch.forceStop();
                StartCoroutine(ch.playClip(ch.femaleFoundNail2));
            }
        }
    }

    private IEnumerator guideNPCsToSpawnedObject(bool finalClue) {
        yield return new WaitForSeconds(2f);

        Vector3 offset = new Vector3(2, 0, 0.5f);
        npcController.takeNPCstoLocation(spawnLoc - offset, spawnLoc, finalClue);
    }

    private void handlePlayerSearch()
    {
        if (player == null)
        {
            Debug.Log("player is null");
            return;
        }

        if (planksFound >= totalPlanks)
            return;

        Vector2 playerPos = new Vector2(player.position.x, player.position.z);
        Vector2 objPos = new Vector2(spawnLoc.x, spawnLoc.z);

        if (Vector2.Distance(playerPos, objPos) <= 3f && !(planksFound == totalPlanks)/*Not on the mound*/)
        {
            // @TODO: Add green check above found nail to show that this has been found
            Debug.Log("Object has been found!");
            if (nailsFound < totalNails)
            {
                if (nailsFound == 0)
                {
                    ch.forceStop();
                    StartCoroutine(ch.playNail1Dialogue());
                }
                else
                {
                    ch.forceStop();
                    StartCoroutine(ch.playNail2Dialogue());
                }
                nailsFound++;
                nailFindingTime += elapsedTime;
            }
            else
            {
                planksFound++;
                ch.forceStop();
                StartCoroutine(ch.plankFollowUp());
                plankFindingTime += elapsedTime;
            }

            addCheckMark(spawnLoc);

            targetSpawned = false;
            if (planksFound == totalPlanks) {
                spawnLoc = mound.transform.position;
                StartCoroutine(guideNPCsToSpawnedObject(true));
                return;
            }
            else
            {
                npcController.wander = true;
                elapsedTime = 0f;
            }

        }
    }


    private void spawnObject(GameObject obj, Vector3 position) {
        if (obj == null)
        {
            Debug.Log("Invalid object received");
            return;
        }
        float terrainHeight = terrain.SampleHeight(position);
        Vector3 spawnPos = new Vector3(position.x, terrainHeight + 0.1f, position.z);
        spawnLoc = spawnPos;
        Instantiate(obj, spawnPos, Quaternion.identity);
        Debug.Log("object has been spawned");
    }

    private void addCheckMark(Vector3 position) {
        Vector3 offset = new Vector3(0, 1.0f, 0); // offset for the check mark to appear above ground

        Vector3 checkPos = position + offset;
        Instantiate(checkMarkPrefab, checkPos, Quaternion.identity);
    }

    public async void loadScoreBoard() {
        GameStats.gameEnded = true;
        SessionVariables.completedBoat = true;
        if (!GameStats.storySeeker)
            GameStats.storySeeker = timesNPCSFoundHint == (totalNails + totalPlanks);
        Debug.Log("times: " + timesNPCSFoundHint +", totalN " + totalNails + " TotalP: "+ totalPlanks);
        GameStats.finishTime = nailFindingTime + plankFindingTime;
        scoreCanvas.SetActive(true);
        avgNailTime.SetText(timeToString(nailFindingTime, nailsFound));
        avgPlankTime.SetText(timeToString(plankFindingTime, planksFound));
        double t = nailFindingTime + plankFindingTime;

        overallTime.SetText(timeToString(t, 1));

        if (SessionVariables.user == null)
        {
            Debug.LogError("User is null");
            SessionVariables.user = new User("Guest", 25, true, "New");
        }

        SessionVariables.user.overAllTime = t;
        await FirebaseRealtimeService.Instance.AddUser(SessionVariables.user);

#if UNITY_EDITOR || UNITY_STANDALONE
       Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        
#endif
    }

    public static string timeToString(double time, int cnt) {

        double avg;
        if (cnt > 0)
            avg = time / cnt;
        else
            avg = 0;
        string result = "";
        int mins = (int)(avg) / 60;
        result += mins > 9 ? mins.ToString() : "0" + mins;
        result += ":";
        int secs = (int)(avg) % 60;

        result += secs > 9 ? secs.ToString() : "0" + secs;
        return result;
    }

    public void revealBoat() { 
        mound.gameObject.SetActive(false);
        boat.gameObject.SetActive(true);
    }
}
