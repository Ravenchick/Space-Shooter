using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }
    public IEnumerator Shaking(float duration, float magnitude)
    {
        

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void RestarPosition()
    {
        transform.localPosition = originalPosition;
    }
}
