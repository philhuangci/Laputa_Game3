using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class CountDownTimer : MonoBehaviour
{
    public Text _countDownText;
    public Image _cloudImage;
    public Image _clockImage;



    public float CountDownTime;
    private float GameTime;
    private float timer = 0;

    private bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        GameTime = CountDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            int M = (int)(GameTime / 60);
            float S = GameTime % 60;
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0;
                GameTime--;
                _countDownText.text = M + ": " + string.Format("{0:00}", S);
            }
        }

    }

    public void Reset()
    {
        CountDownTime = 0;
        DisableTimer();
    }

    public void StartNewTimer(int countDownTimer)
    {
        SetTimer(countDownTimer);
        
        AbleTimer();

    }

    public void SetTimer(int countDownSec)
    {
        CountDownTime = countDownSec;
        int M = (int)(CountDownTime / 60);
        float S = CountDownTime % 60;
        _countDownText.text = M + ": " + string.Format("{0:00}", S);
    }

    public void DisableTimer()
    {
        isActive = false;
        GameTime = 0;
        _countDownText.gameObject.SetActive(false);
        _cloudImage.gameObject.SetActive(false);
        _clockImage.gameObject.SetActive(false);
    }

    public void AbleTimer()
    {
        isActive = true;
        GameTime = CountDownTime;
        _countDownText.gameObject.SetActive(true);
        _cloudImage.gameObject.SetActive(true);
        _clockImage.gameObject.SetActive(true);
    }



}
