using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    float pullForce;

    [SerializeField]
    float rotationForce;

    [SerializeField]
    float holdDelay;

    float holdStart = -1f;

    public int face;

    bool thrown;
    bool stationary;
    bool holding;

    AudioSource audioSource;
    
    Camera mainCamera;

    Rigidbody rigidBody;

    GameManager gameManager;

    void Start() {
        mainCamera = Camera.main;

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update() {
        // start holding
        if (Input.GetMouseButtonDown(0) && !holding){
            holdStart = Time.time;

            holding = true;
        }

        else if (Input.GetMouseButtonUp(0) && Time.time - holdStart > holdDelay){
            holding = false;
            thrown = true;
            StartCoroutine(PlayDiceSound());
        }
    }

    void FixedUpdate(){

        // holding dice
        if (!thrown && holding){
            stationary = false;

            rigidBody.drag = 10f;
            rigidBody.angularDrag = 2f;

            Vector3 mousePos = Input.mousePosition;

            mousePos.z = 1f;

            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

            int layerMask = 1 << 8;

            RaycastHit hit;

            if (Physics.Raycast(mainCamera.transform.position, mouseWorldPos - mainCamera.transform.position, out hit, Mathf.Infinity, layerMask))
            {
                Vector3 dir = hit.point - rigidBody.position;
                Vector3 rotationAxis = Quaternion.AngleAxis(90, Vector3.up) * dir.normalized;

                rigidBody.AddForce(dir * pullForce);
                rigidBody.AddTorque(rotationAxis * -dir.magnitude * rotationForce);
            }
        }

        // dice not held
        else {
            rigidBody.drag = 1f;
            rigidBody.angularDrag = 0.05f;
        }

        // finished rolling
        if (rigidBody.angularVelocity.magnitude < 0.01f && rigidBody.velocity.magnitude < 0.01f && thrown && !stationary){
            stationary = true;

            float smallestAngle = 180f;

            Vector3[] directions = new Vector3[] {-transform.up, -transform.right, transform.forward, -transform.forward, transform.right, transform.up};

            for (int i = 0; i < directions.Length; i++)
            {
                if (Vector3.Angle(directions[i], Vector3.up) < smallestAngle){
                    smallestAngle = Vector3.Angle(directions[i], Vector3.up);
                    face = i + 1;
                }
            }

            Debug.Log(gameObject.name + " finished rolling on face " + face);
            
            gameManager.DiceHasBeenRolled(face);

            // start the dissaperance sequence 
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    private IEnumerator PlayDiceSound()
    {
        yield return new WaitForSeconds(1f);

        audioSource.Play();
    }
}
