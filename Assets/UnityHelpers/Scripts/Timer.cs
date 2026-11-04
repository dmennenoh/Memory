using UnityEngine;
using TMPro;

public class Timer:MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float elapsedTime;
    private bool isRunning;
      

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int tenths = Mathf.FloorToInt((elapsedTime * 10f) % 10f);

        timerText.text = $"{minutes:00}:{seconds:00}.{tenths}";
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "00:00.0";
    }
}