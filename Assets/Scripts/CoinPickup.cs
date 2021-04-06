using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinValue = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            PlayPickupSFX();
            FindObjectOfType<GameSession>().AddCoinToScore(coinValue);
            Destroy(gameObject);
        }
    }

    private void PlayPickupSFX()
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
    }
}
