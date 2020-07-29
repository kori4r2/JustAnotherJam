using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image;

    public bool startedClock = false;

    private float accumulator = 0;
    private float timeDelta;
    private int currentIndex = 0;

    void Awake()
    {
        timeDelta = (1.0f) / sprites.Length;
        image.sprite = sprites[0];
    }

    void Update()
    {
        if(!startedClock) return;

        accumulator += Time.deltaTime;
        int previousIndex = currentIndex;
        while (accumulator > timeDelta)
        {
            accumulator -= timeDelta;
            currentIndex++;
            if(currentIndex >= sprites.Length) currentIndex = 0;
        }
        if(previousIndex != currentIndex)
            image.sprite = sprites[currentIndex];
    }

    
}
