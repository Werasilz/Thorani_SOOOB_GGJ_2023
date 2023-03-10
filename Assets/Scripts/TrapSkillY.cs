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

    public GameObject burningParticle;

    public float delayChange = 1f;

    void Start()
    {
        StartCoroutine(TrapActivate());
    }

    IEnumerator TrapActivate()
    {
        startModel.SetActive(true);
        finalModel.SetActive(false);

        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(delayChange);

        startModel.GetComponentInChildren<Animator>().SetTrigger("isActivate");

        yield return new WaitForSeconds(1f);

        startModel.SetActive(false);
        finalModel.SetActive(true);

        // Set color
        Renderer[] rootRenderers = finalModel.GetComponentsInChildren<Renderer>();

        foreach (var renderer in rootRenderers)
        {
            renderer.material = PlayerManager.instance.rootSpinColorMaterials[PlayerManager.instance.playerSelectIndex[playerInput.playerIndex]];
        }

        yield return new WaitForSeconds(1f);

        GetComponent<Collider>().enabled = true;
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
            PlayerManager.instance.AddPlayerScore(playerInput, target.GetComponent<EnemyController>().enemyScore);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (target == null)
            {
                target = other.gameObject;

                if (target.GetComponent<EnemyController>().holdingTouch)
                {
                    GetComponent<Collider>().enabled = false;
                    finalModel.GetComponentInChildren<Animator>().SetTrigger("goDown");
                    burningParticle.SetActive(true);
                    burningParticle.transform.SetParent(null);

                    Destroy(gameObject, 5f);
                    Invoke(nameof(StopParticle), 2f);
                    Destroy(gameObject, 2f);
                }
                else
                {
                    target.transform.position = transform.position;
                    target.GetComponent<EnemyController>().EnableAgent(false);
                    target.GetComponent<Rigidbody>().isKinematic = true;

                    StartCoroutine(KillEnemy());
                }
            }
        }

        if (other.gameObject.CompareTag("RootWall") || other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            finalModel.GetComponentInChildren<Animator>().SetTrigger("goDown");
            Destroy(gameObject, 1f);
        }
    }

    void StopParticle()
    {
        burningParticle.GetComponent<ParticleSystem>().Stop(true);
    }


}
