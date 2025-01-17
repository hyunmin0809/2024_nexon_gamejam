using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparency : MonoBehaviour
{
    private Renderer rd;

    private void Start()
    {

        rd = GetComponent<Renderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Color color = rd.material.color;
            color.a = 0.5f;
            rd.material.color = color;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SoundManager.Instance.PlaySfx(SoundType.조작바충돌sfx);
        if (collision.gameObject.CompareTag("ball"))
        {
            Color color = rd.material.color;
            color.a = 1f;
            rd.material.color = color;
        }
    }
}
