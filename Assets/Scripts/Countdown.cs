using System;
using System.Collections;
using Events;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Countdown : EventListenerMono
{
    [SerializeField] private TMP_Text _countDownTMP;

    private float _countDownTimer = 3.0f;

    private void Start()
    {
        TimeEvents.CountdownTimer?.Invoke();
    }

    protected override void RegisterEvents()
    {
        TimeEvents.CountdownTimer += OnCountdownTimer;
    }

    private void OnCountdownTimer()
    {
        StartCoroutine(CountdownToStart());
    }

    private IEnumerator CountdownToStart()
    {
        Time.timeScale = 0f;
            
        while (_countDownTimer > 0)
        {
            _countDownTMP.text = _countDownTimer.ToString();

            yield return new WaitForSecondsRealtime(1f);

            _countDownTimer--;
        }

        _countDownTMP.text = "GO!";
            
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
            
        _countDownTMP.gameObject.SetActive(false);
            
    }

    protected override void UnRegisterEvents()
    {
        TimeEvents.CountdownTimer -= OnCountdownTimer;
    }
}
