using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] bool hasBeenCaught;
    // [SerializeField] bool entered;
    // [SerializeField] bool exited;
    // [SerializeField] static int ballCount = 0;
    // [SerializeField] int ballID = 0;
    // Start is called before the first frame update
    void Start()
    {  
        // ballCount++;
        // ballID = ballCount;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hasBeenCaught = false;
    }

    // private void OnDrawGizmos()
    // {
    //     if (exited)
    //     {
    //         Gizmos.color = Color.black;
    //         Gizmos.DrawSphere(transform.position, 1);
    //     }
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bin"))
        {
            if (!hasBeenCaught)
            {
                // entered = true;
                // Debug.Log(gameObject.name + " " + ballID + " entered trigger at " + gameObject.transform.position);
                hasBeenCaught = true;
                gameManager.UpdateBallsCaught();
                StartCoroutine(DestroyBall(gameObject));
            }
        }
        else if (other.gameObject.CompareTag("Sensor"))
        {
            // if (entered)
            // {
            //     exited = true;
            //     Debug.Log(gameObject.name + " " + ballID + " was caught but fell through the platform or bin at " + gameObject.transform.position);
            // }
            Destroy(gameObject);
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Bin"))
    //     {
    //         Debug.Log(gameObject.name + " " + ballID + " left box trigger at " + gameObject.transform.position);
    //     }
    // }

    IEnumerator DestroyBall(GameObject ball)
    {
        yield return new WaitForSeconds(2);
        // Debug.Log(ball.name + " " + ballID + " has been caught");
        Destroy(ball);
    }

    void Update()
    {
        Physics.SyncTransforms();
    }
}
