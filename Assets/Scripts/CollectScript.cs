using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectScript : MonoBehaviour
{
    public float rotationSpeed = 50f;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float delayBeforeDeactivate = 1f;

    private bool isPlayerNear = false;
    private GameObject currentPlayer;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !MenuKeybindingsScript.IsMenuOpen)
        {
            PlayPickupSound();

            if (currentPlayer != null)
            {
                CharacterScript character = currentPlayer.GetComponent<CharacterScript>();
                if (character != null)
                {
                    character.PlayGatheringAnimationTimed();
                }
            }

            StartCoroutine(DeactivateAfterDelay());
        }
    }

    void PlayPickupSound()
    {
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDeactivate);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            currentPlayer = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            currentPlayer = null;
        }
    }
}