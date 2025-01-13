using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    const int framecount = 64;
    const float timeprframe = (3f + (25f / 60f)) / framecount;
    int activationtimes = 0;
    BoxCollider2D box;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        box = GetComponent<BoxCollider2D>();
        int pixelpositon = Mathf.FloorToInt(player.position.x * 32f);
        float spikePos = pixelpositon / 32f;
        transform.position = new Vector2(spikePos, transform.position.y);
    }

    public void hello()
    {
        Debug.Log("Spike Says hello :D");
    }

    public void changehitbox()
    {
        activationtimes++;
        switch (activationtimes) 
        { 
            case 1:
                box.enabled = true;
                break;
            case 2:
                box.offset = new Vector2(0, -0.8f);
                break;
            case 3:
                box.offset = new Vector2(0, 0);
                break;
            case 4:
                StartCoroutine(Framestuff());
                break;
            case 5:
                Destroy(gameObject);
                Destroy(this);
                break;
        }
    }
    IEnumerator Framestuff()
    {
        for (int i = 0; i < framecount; i++)
        {
            yield return new WaitForSeconds(timeprframe);
            box.offset = box.offset - new Vector2(0, 2f/32f);
        }
    }
}
