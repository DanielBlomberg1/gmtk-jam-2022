using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    float pullForce;

    [SerializeField]
    float rotationForce;

    public int face;

    bool thrown;
    bool stationary;

    Camera mainCamera;

    Rigidbody rigidBody;

    GameManager gameManager;

    void Start() {
        mainCamera = Camera.main;

        rigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)){
            thrown = true;
        }
    }

    void FixedUpdate(){
        // holding dice
        if (Input.GetMouseButton(0) && !thrown){
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
        yield return new WaitForSeconds(3);

        thrown = false;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
