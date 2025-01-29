using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FartCensor : MonoBehaviour
{
    [SerializeField] private GameObject censorObject;

    public void ActivateCensor()
    {
        censorObject.SetActive(true);
    }

    public void DeactivateCensor()
    {
        StartCoroutine(DeactivateCensorAfterDelay());
    }

    private IEnumerator DeactivateCensorAfterDelay()
    {
        yield return new WaitForSeconds(15f);
        censorObject.SetActive(false);
    }
}
