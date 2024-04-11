using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KeyManager : MonoBehaviour
{

    [Header("General")]
    [SerializeField]
    private bool collected = false;

    [Header("Collected")]
    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject keyCollect;
    [SerializeField]
    private float rotationSpeed = 100;

    [Header("Collected")]
    [SerializeField]
    private Transform key;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float circleRadius = 1f;
    [SerializeField]
    public Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float smoothTime = 0.1f;

    [Header("Unlock")]
    [SerializeField]
    private GameObject keyBody;

    // Start is called before the first frame update
    void Start()
    {
        Material keyBodyMaterial = keyBody.GetComponent<Renderer>().material;
        keyBodyMaterial.color = Color.blue;
        Debug.Log(keyBodyMaterial.color);

        keyCollect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        if (collected)
        {
            //key.position = new Vector3(0f, 1f, player.position.z + 1f);

            // Calculate a position in front of the player, offset by the circle radius
            Vector3 targetPosition = cameraTransform.position + cameraTransform.transform.forward * circleRadius;

            // Rotate the key to face the player
            key.LookAt(cameraTransform.transform);

            // Move the key smoothly towards the target position
            key.transform.position = Vector3.SmoothDamp(key.transform.position, targetPosition, ref velocity, smoothTime);
        }

        if (keyCollect.activeInHierarchy)
        {
            Quaternion targetRotation = Quaternion.LookRotation(keyCollect.transform.position - player.transform.position);
            keyCollect.transform.rotation = Quaternion.RotateTowards(
                keyCollect.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Input.GetKeyDown(KeyCode.E) && !collected)
            {
                keyCollect.SetActive(false);
                collected = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (!collected)
            {
                keyCollect.SetActive(true);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (!collected)
            {
                keyCollect.SetActive(false);
            }
        }

    }
}
