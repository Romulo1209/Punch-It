using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float playerForce;
    [SerializeField] private float playerVelocity;
    [SerializeField] private float playerGravity = -9f;
    [SerializeField] private LayerMask interactLayers;

    [Header("States")]
    public bool holding;
    private bool moving;

    [Header("References")]
    [SerializeField] private Transform collisionChecker;
    [SerializeField] private Transform playerMesh;
    [SerializeField] private Material[] materials;

    private SkinnedMeshRenderer[] renderers;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    Vector3 gravityVelocity;

    CharacterController characterController;
    Animator animator;
    StackController stackController;
    public static PlayerController Instance;

    //Getters & Setters
    public float PlayerForce { get { return playerForce; } set { playerForce = value; } }
    public float PlayerVelocity { get { return playerVelocity; } set { playerVelocity = value; } }
    public Transform PlayerMesh { get { return playerMesh; } }

    private void Awake() {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>(); 
        Instance = this; 
    }

    private void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        stackController = GetComponentInChildren<StackController>();
    }

    private void Update()
    {
        Gravity();
        Animations();
        CheckCollisions();
    }
    public void Animations()
    {
        animator.SetBool("Moving", moving);
        animator.SetBool("Holding", holding);
        moving = false;
    }

    #region Movement

    public void Gravity()
    {
        gravityVelocity.y += playerGravity * Time.deltaTime;
        characterController.Move(gravityVelocity * Time.deltaTime);
    }
    public void Move(float movX, float movY)
    {
        Vector3 direction = new Vector3(movX, 0, movY).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(playerMesh.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        playerMesh.rotation = Quaternion.Euler(0, angle, 0);

        characterController.Move(direction * playerVelocity * Time.deltaTime);
        moving = true;
    }

    #endregion

    #region Collisions

    void CheckCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(collisionChecker.position, 1, interactLayers);
        foreach(Collider col in hitColliders) {
            

            var enemy = col.GetComponent<EnemyController>();
            if (enemy != null && enemy.Alive) {
                Punch(col);
                continue;
            }
            else if (col.gameObject.layer == LayerMask.NameToLayer("DropZone")) {
                if (holding) stackController.DropBody();
                else stackController.Dropping = false;
                continue;
            }
            else if (enemy == null) {
                enemy = col.GetComponentInParent<EnemyController>();
                if (!enemy.Alive) {
                    GameObject enemyObj = col.GetComponentInParent<EnemyController>().gameObject;
                    Stack(enemyObj);
                    continue;
                }
            }
        }
    }

    #endregion

    #region Other Functions

    void Stack(GameObject enemyBody)
    {
        stackController.StackBody(enemyBody);
    }
    void Punch(Collider col)
    {
        var canPunch = col.GetComponent<EnemyController>().GetHit(transform, (int)PlayerForce);
        if(canPunch)
            animator.SetTrigger("Punch");
    }

    public void ChangeColor(int index)
    {
        foreach(SkinnedMeshRenderer renderer in renderers) {
            renderer.material = materials[index];
        }
    }

    #endregion
}
