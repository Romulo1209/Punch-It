using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Informations")]
    [SerializeField] private EnemyScriptable enemyData;
    [Space(5)]
    [SerializeField] private bool alive = true;
    [SerializeField] private int levelRequired;
    [SerializeField] private int xpToGive;

    [Header("References")]
    [SerializeField] GameObject LevelInformation;
    

    Rigidbody[] ragdolls;
    bool punched = false;
    Animator animator;

    //Getters & Setters
    public bool Alive { get { return alive; } set { alive = value; } }

    private void Awake() {
        Alive = true;
        ragdolls = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Setup();
        StartCoroutine(SelfDestroy());
    }
    void Setup()
    {
        levelRequired = enemyData.EnemyLevel;
        xpToGive = enemyData.EnemyXP;
    }

    #region Hit

    public bool GetHit(Transform hitPosition, int hitPower)
    {
        if (hitPower < levelRequired)
            return false;
        if (punched)
            return false;

        punched = true;
        animator.enabled = false;
        LevelInformation.SetActive(false);
        GetHitRagdoll(false, hitPosition);
        StartCoroutine(WaitDeath());

        return true;
    }

    void GetHitRagdoll(bool state, Transform hitPosition = null)
    {
        foreach (Rigidbody rb in ragdolls) {
            rb.isKinematic = state;
            rb.AddExplosionForce(20, hitPosition.position, 10, 2, ForceMode.Impulse);
        }
    }

    IEnumerator WaitDeath() {
        yield return new WaitForSeconds(0.5f);
        Alive = false;
    }

    #endregion

    #region Others Functions

    public void GetPoints()
    {
        GameController.Instance.GetReward(enemyData);
        Destroy(gameObject);
    }

    public void StackEnemy(bool state)
    {
        ragdolls[0].isKinematic = state;
        ragdolls[0].transform.localPosition = Vector3.zero;
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(30f);
        if (alive)
            Destroy(gameObject);
    }

    #endregion
}
