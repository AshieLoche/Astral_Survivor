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
    private static KeyManager _instance;
    public static KeyManager Instance { get { return _instance; } }
    protected Color keyColor;

    [Header("Collected")]
    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject keyCollect;
    [SerializeField]
    private float rotationSpeed = 100;

    [Header("Collected")]
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
    [SerializeField]
    private GameObject door;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Material keyBodyMaterial = keyBody.GetComponent<Renderer>().material;
        keyBodyMaterial.color = Color.blue;
        Debug.Log(keyBodyMaterial.color);
        Material doorMaterial = keyBody.GetComponent<Renderer>().material;
        Debug.Log(doorMaterial.color);
        Debug.Log(keyBodyMaterial.color == doorMaterial.color);

        keyCollect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (collected)
        {
            // Calculate a position in front of the player, offset by the circle radius
            Vector3 targetPosition = cameraTransform.position + cameraTransform.transform.forward;

            // Rotate the key to face the player
            transform.LookAt(cameraTransform.transform);

            // Move the key smoothly towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
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
