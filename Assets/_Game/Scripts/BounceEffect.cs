using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float jumpHeight = 1f; // Độ cao mà GameObject sẽ nảy lên
    public float jumpSpeed = 1f; // Tốc độ của nảy lên
    public float scaleFactor = 0.01f;

    private Vector3 originalScale;
    private float originalZ; // Vị trí Z ban đầu của GameObject

    private void Start()
    {
        // Lưu vị trí Z ban đầu của GameObject
        originalZ = transform.position.z;
        originalScale = transform.localScale;
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        float targetZ = originalZ + jumpHeight; // Vị trí Z mục tiêu của nảy lên

        while (transform.position.z < targetZ)
        {
            // Di chuyển GameObject lên trên theo trục Z
            transform.position += new Vector3(0, 0, jumpSpeed * Time.deltaTime);
            transform.localScale += originalScale * scaleFactor * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}