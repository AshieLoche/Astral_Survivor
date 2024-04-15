using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMangaer : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField]
    private GameObject keyPrefab;
    [SerializeField]
    private Transform keys;
    [SerializeField]
    private Transform[] cC_EPKS;
    [SerializeField]
    private Color[] keyColors;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newKey = Instantiate(keyPrefab, keys.transform) as GameObject;
        newKey.name = "Blue Key";
        int randomNumber = Random.Range(0, 4);
        newKey.transform.position = new Vector3(cC_EPKS[randomNumber].position.x, cC_EPKS[randomNumber].position.y + 1, cC_EPKS[randomNumber].position.z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
