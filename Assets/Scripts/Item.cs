using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        copperCoin,
        silverCoin,
        goldCoin,
        NONE
    };

    public Collider2D pickUpCollider;
    public Type type;
    public float onDropPreventPickUpDuration = 1f;
    bool blinking = false;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drop( Vector3 position )
    {
        transform.position = position;
        pickUpCollider.enabled = false;
        blinking = true;
        
        gameObject.SetActive(true);

        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(onDropPreventPickUpDuration);
        blinking = false;
        pickUpCollider.enabled = true;
    }   

}
