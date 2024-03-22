using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] int isMirror;
    Rigidbody2D rgbd;
    MirrorMovement player;
    float xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MirrorMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {   
        if (isMirror == 1) {
            rgbd.velocity = new Vector2(-xSpeed, 0f);
        }
        else {
            rgbd.velocity = new Vector2(xSpeed, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
