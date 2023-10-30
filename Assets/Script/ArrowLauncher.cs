using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject arrowPrefab;

    public void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, launchPoint.position, arrowPrefab.transform.rotation);

        Vector3 oriScale = arrow.transform.localScale;

        arrow.transform.localScale = new Vector3(
            oriScale.x * transform.localScale.x > 0 ? 1 : -1,
            oriScale.y,
            oriScale.z
            );
    }
}
