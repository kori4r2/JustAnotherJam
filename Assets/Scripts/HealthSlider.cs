using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider slider;
    private PlayerController player;

    void Start()
    {
        player = PlayerController.Instance;
        if(player != null)
        {
            player.OnHealthChange += UpdateBar;
        }
    }

    void UpdateBar(float percent)
    {
        if(slider)
        {
            slider.minValue = 0;
            slider.maxValue = 1f;
            slider.value = percent;
        }
    }
}
