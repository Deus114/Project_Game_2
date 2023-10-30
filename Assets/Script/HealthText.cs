using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 75, 0);
    RectTransform textTransform;

    public float timeToFade = 1f;
    private float timeElapsed;
    TextMeshProUGUI textMeshPro;
    private Color startColor;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeToFade)
        {
            float fadeA = startColor.a * (1-(timeElapsed/timeToFade));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeA);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
