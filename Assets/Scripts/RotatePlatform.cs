using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] float RotationSpeed = 30;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.ShouldRotate())
        {
            transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
            Physics.SyncTransforms();
        }
    }
}
