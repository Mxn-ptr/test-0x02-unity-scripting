using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public float speed = 10.0f;
    private int score = 0;
    private GameObject[] teleporters;
    private GameObject destination;
    private bool canTeleport = true;
    // Start is called before the first frame update
    void Start()
    {
        teleporters = GameObject.FindGameObjectsWithTag("Teleporter");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.Normalize();
        transform.position += move * speed * Time.deltaTime;
        if (health <= 0) {
            health = 5;
            score = 0;
            Debug.Log("Game Over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Pickup") {
            score++;
            Debug.Log("Score: " + score);

            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Trap") {
            health--;
            Debug.Log("Health: " + health);

        }
        else if (other.gameObject.tag == "Goal") {
            Debug.Log("You win!");

        }
        else if (other.gameObject.tag == "Teleporter") {
            foreach (GameObject teleporter in teleporters) {
                if (teleporter != other.gameObject) {
                    destination = teleporter;
                    break;
                }
            }

            if (destination != null && canTeleport == true) {
                transform.position = destination.transform.position;
                foreach (GameObject teleporter in teleporters) {
                    teleporter.gameObject.SetActive(false);
                }
                canTeleport = false;
                StartCoroutine(TeleportCooldown());
            }
        }
    }
    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        foreach (GameObject teleporter in teleporters) {
            teleporter.gameObject.SetActive(true);
        }
        canTeleport = true;
    }
}
