using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTracking : MonoBehaviour
{
    public static TimeTracking instance = null;

    [Header("UI References")]
    public Text timeText;
    public GameObject highscoreContainer;
    public Text highscoreText;
    public Text currentRoomText;
    public ClockAnimation clockAnimation;


    private float startingTime;
    private float bestTime;
    private Navigator navigator;
    private bool updateTime = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
            return;
        }

        bestTime = PlayerPrefs.GetFloat("Highscore", -1);
    }

    void Start()
    {
        if(highscoreContainer != null && highscoreText != null)
        {
            highscoreContainer?.SetActive(bestTime != -1);
            highscoreText.text = FormatTime(bestTime);
        }
    }

    public void OnNavigatorReady(Navigator nav)
    {
        navigator = nav;
        foreach (var room in navigator.Rooms)
        {
            room.Value.PlayerEntered.AddListener(
                () => OnLoadRoom(room.Key)
            );
        }
    }

    private void OnLoadRoom(int roomId)
    {
        if(!updateTime)
        {
            updateTime = true;
            startingTime = Time.time;
            clockAnimation.startedClock = true;
        }
        if(currentRoomText != null)
            currentRoomText.text = roomId.ToString();
    }

    void Update()
    {
        if(updateTime && timeText != null)
        {
            float currentTime = Time.time - startingTime;
            timeText.text = FormatTime(currentTime);
        }
    }

    public string FormatTime( float time )
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;
        int milliseconds = (int) (1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds );
    }

    public void FinishGame()
    {
        clockAnimation.startedClock = false;
        float score = Time.time - startingTime;
        
        updateTime = false;
        if(score < bestTime || bestTime == -1)
            PlayerPrefs.SetFloat("Highscore", score);
    }


}
