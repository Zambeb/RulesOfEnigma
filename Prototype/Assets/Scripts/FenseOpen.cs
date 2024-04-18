using System.Collections;
using UnityEngine;

public class FenseOpen : MonoBehaviour
{
    public GameObject fenseLeft;
    public GameObject fenseRight;

    private bool fenseOpen;
    private bool isOpenning = false;
    private bool isClosing = false;

    private Quaternion initialRotationLeft;
    private Quaternion initialRotationRight;

    public Quaternion targetRotationLeft;
    public Quaternion targetRotationRight;

    public float rotationSpeed = 2f;

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    void Start()
    {
        fenseOpen = false;
        initialRotationLeft = fenseLeft.transform.rotation;
        initialRotationRight = fenseRight.transform.rotation;
    }

    void Update()
    {
        if (fenseOpen && !isOpenning)
        {
            StartCoroutine(OpenFense());
        }

        if (!fenseOpen && !isClosing)
        {
            StartCoroutine(CloseFense());
        }
    }

    IEnumerator OpenFense()
    {
        isOpenning = true;

        while (Quaternion.Angle(fenseLeft.transform.rotation, targetRotationLeft) > 0.1f ||
               Quaternion.Angle(fenseRight.transform.rotation, targetRotationRight) > 0.1f)
        {
            fenseLeft.transform.rotation = Quaternion.Slerp(fenseLeft.transform.rotation, targetRotationLeft, Time.deltaTime * rotationSpeed);
            fenseRight.transform.rotation = Quaternion.Slerp(fenseRight.transform.rotation, targetRotationRight, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        isOpenning = false;
    }

    IEnumerator CloseFense()
    {
        isClosing = true;

        while (Quaternion.Angle(fenseLeft.transform.rotation, initialRotationLeft) > 0.1f ||
               Quaternion.Angle(fenseRight.transform.rotation, initialRotationRight) > 0.1f)
        {
            fenseLeft.transform.rotation = Quaternion.Slerp(fenseLeft.transform.rotation, initialRotationLeft, Time.deltaTime * rotationSpeed);
            fenseRight.transform.rotation = Quaternion.Slerp(fenseRight.transform.rotation, initialRotationRight, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        isClosing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(openSound);
            fenseOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(closeSound);
            fenseOpen = false;
        }
    }
}
