using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeZoneBoundary : MonoBehaviour
{
    private EnemyAi enemyAi;

    // Start is called before the first frame update
    void Start()
    {
        enemyAi = EnemyAi.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyAi.isPlayerSafe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyAi.isPlayerSafe = false;
        }
    }
}
