using System;
using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    [Header("Player Object Assigning")]
    public Transform player;

    [Header("NPC Assignings")]
    public Transform[] characters; // array to hold all npcs 

    [Header("Controllers")]
    public GameObject missionController;
    public GameObject convHandler;

    [Header("NPC Settings")]
    public float maxDistanceFromPlayer = 4f;
    public float maxWalkDuarion = 3f;
    public float idleDuaration = 2f;
    public float defaultSpeed = 1f;
    public bool wander = true;

    private float gravity = -9.81f;
    private Npc[] npcs;
    private float moveSpeed;
    private ConversationHandler ch;
    private bool clipsPlayed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
            player = transform;
        ch = convHandler.GetComponent<ConversationHandler>();
        npcs = new Npc[characters.Length];
        for(int i=0; i < characters.Length; i++) {
            npcs[i] = new Npc(characters[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Time.timeScale > 0)) // game is paused
            return;

        adjustHeights(); // making sure npcs stick to the ground regardless of terrain height

        if (!wander)
            return;

        foreach (Npc n in npcs)
        {
            if (n != null && n.getTransform() != null && !n.isBusy())
            {
               StartCoroutine(handleNpcWalk(n));
            }
        }
    }

    private void adjustHeights()
    {
        bool grounded;
        CharacterController controller;
        if (npcs == null || npcs.Length == 0)
            return;
        foreach (Npc n in npcs) {
            if (n == null) continue;
            controller = n.getTransform().GetComponent<CharacterController>();
            grounded = controller.isGrounded;
            if (grounded && n.velocity.y < 0)
            {
                n.velocity.y = -2f; // sticking the character to the ground
            }
            n.velocity.y += gravity * Time.deltaTime; // gravity pulling the character down
            controller.Move(n.velocity * Time.deltaTime);
        }
    }

    private IEnumerator handleNpcWalk(Npc n) {
        if (n == null || n.getTransform() == null || n.isBusy()) {
            Debug.Log("[handleNpcWalk] - is Null: " + (n == null) + ", Null transform: " + (n.getTransform() == null) + ", Busy: " + n.isBusy());
            yield break;
        }
        if (!wander) {
            yield break;//stop characters
        }
        float timer = 0f;
        n.setBusy(true);
        n.setWalking(true);

        moveSpeed = defaultSpeed;

        Animator animator = n.getTransform().GetComponent<Animator>();
        animator.SetFloat("Speed", moveSpeed);

        CharacterController controller = n.getTransform().GetComponent<CharacterController>();

        int directionRolls = 0;
        int changeDirectionChance = 5; // 5% chance to change direction

        Vector3 move = GetRandomDirection();
        float walkDuration = UnityEngine.Random.Range(2f, maxWalkDuarion + 1);

        while (timer < walkDuration) {
            if (controller.isGrounded && n.velocity.y < 0) {
                n.velocity.y = -2f;
            }
            n.velocity.y += gravity * Time.deltaTime;// used to control the character's vertical speed

            float distance = Vector3.Distance(player.position, n.getTransform().position);

            if (directionRolls < 2 && UnityEngine.Random.Range(0, 100) < changeDirectionChance)
            {
                move = GetRandomDirection();
                directionRolls++;
            }

            if (distance > maxDistanceFromPlayer)
            {
                moveSpeed *= 1.25f;
                move = (player.position - n.getTransform().position).normalized;
            }
            else {
                moveSpeed = defaultSpeed;
            }
                controller.Move((move + n.velocity) * moveSpeed * Time.deltaTime);

            if (move != Vector3.zero)
            {
                n.getTransform().rotation = Quaternion.LookRotation(move);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetFloat("Speed", 0f);
        n.setWalking(false);

        timer = 0f;
        while (timer < idleDuaration) {
            timer += Time.deltaTime;
            yield return null;
        }
        n.setBusy(false);
    }
    private Vector3 GetRandomDirection()
    {
        // Pick a random angle in degrees
        float angle = UnityEngine.Random.Range(0f, 360f);

        // Convert to direction vector on the XZ plane
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));

        return dir.normalized;
    }

    public void takeNPCstoLocation(Vector3 location, Vector3 objLoc, bool finalClue)
    {
        // stop wandering
        wander = false;
        Debug.Log("NPCs stopped wandering");
        Vector3 standOffset = new Vector3(0, 0, 0);

        foreach (Npc n in npcs)
        {
            if (n == null || n.getTransform() == null) continue;
            StartCoroutine(MoveNpcToLocation(n, location - standOffset, objLoc, finalClue));
            standOffset += new Vector3(-2.5f, 0, 2);
        }
    }
    private IEnumerator MoveNpcToLocation(Npc n, Vector3 target, Vector3 objectPos, bool finalClue)
    {
        n.setBusy(true);
        n.setWalking(true);

        Animator animator = n.getTransform().GetComponent<Animator>();
        CharacterController controller = n.getTransform().GetComponent<CharacterController>();

        animator.SetFloat("Speed", defaultSpeed);

        float distanceThreshold = 0.5f;

        while (true)
        {
            Vector3 npcPos = n.getTransform().position;
            Vector3 flatTarget = new Vector3(target.x, npcPos.y, target.z);

            Vector3 direction = (flatTarget - npcPos).normalized;

            if (direction.sqrMagnitude > 0.0001f)
            {
                n.getTransform().rotation = Quaternion.LookRotation(direction);
            }

            // Move NPC
            Vector3 motion = (direction * defaultSpeed + n.velocity) * Time.deltaTime;

            // Stop if reached target
            float distance = Vector3.Distance(n.getTransform().position, flatTarget);
            if (distance <= distanceThreshold)
                break;

            animator.SetFloat("Speed", defaultSpeed);
            controller.Move(motion);
            yield return null;
        }

        // Stop animation
        animator.SetFloat("Speed", 0f);
        n.setWalking(false);
        n.getTransform().rotation = Quaternion.LookRotation(objectPos);
        if (finalClue && !clipsPlayed)
        {
            clipsPlayed = true;
            ch.forceStop();
            StartCoroutine(ch.playMoundClips());
            StartCoroutine(moundAction());
        }else
            n.setBusy(false);

        yield return null;
    }

    private IEnumerator moundAction()
    {
        ch.forceStop();
        dig();
        StartCoroutine(ch.playClip(ch.male_dig));
        yield return new WaitForSeconds(8f);   // wait for seconds
        missionController.GetComponent<BeachMission>().revealBoat();
        cheer();
        StartCoroutine(ch.playClip(ch.femaleOnBoat));
        yield return new WaitForSeconds(3f);
        StartCoroutine(ch.playClip(ch.ending_female));
        yield return new WaitForSeconds(5f);
        missionController.GetComponent<BeachMission>().loadScoreBoard();
    }


    public void dig() {
        foreach (Npc n in npcs)
        {
            Animator animator = n.getTransform().GetComponent<Animator>();
            animator.SetFloat("Speed", 0);
            animator.SetBool("cheer", false);
            animator.SetBool("dig", true);
        }

    }
    public void cheer()
    {
        foreach (Npc n in npcs)
        {
            Animator animator = n.getTransform().GetComponent<Animator>();
            animator.SetFloat("Speed", 0);
            animator.SetBool("cheer", true);
            animator.SetBool("dig", false);
        }
    }

    public bool idleNpcs() {
        foreach (Npc n in npcs)
            if (n.isBusy())
                return false;
        return true;
    }

}
