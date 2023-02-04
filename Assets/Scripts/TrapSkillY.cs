using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrapSkillY : MonoBehaviour
{
    [Header("Owner")]
    public PlayerInput playerInput;

    public GameObject startModel;
    public GameObject finalModel;

    public GameObject target;

    public float delayChange = 1f;

    void Start()
    {
        StartCoroutine(TrapActivate());
    }

    IEnumerator TrapActivate()
    {
        startModel.SetActive(true);
        finalModel.SetActive(false);

        yield return new WaitForSeconds(delayChange);

        startModel.GetComponentInChildren<Animator>().SetTrigger("isActivate");

        yield return new WaitForSeconds(1f);

        startModel.SetActive(false);
        finalModel.SetActive(true);

        yield return new WaitForSeconds(1f);

        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (target == null)
            {
                target = other.gameObject;

                target.transform.position = transform.position;
                target.GetComponent<EnemyController>().EnableAgent(false);
                target.GetComponent<Rigidbody>().isKinematic = true;

                StartCoroutine(KillEnemy());
            }
        }
    }

    IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(1f);

        EnemyController enemy = target.GetComponent<EnemyController>();

        enemy.currentHP -= 1;
        if (enemy.currentHP <= 0)
        {
            // kill enemy
            enemy.transform.SetParent(finalModel.GetComponentInChildren<Animator>().transform);

            // add score
            PlayerManager.instance.AddPlayerScore(playerInput, 1);
        }
        else
        {
            // free enemy
            enemy.EnableAgent(true);
        }

        finalModel.GetComponentInChildren<Animator>().SetTrigger("goDown");

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
