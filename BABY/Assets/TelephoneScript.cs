using UnityEngine;

public class TelephoneScript : MonoBehaviour, IInteractable
{
    public enum State{ Idle, Ringing, Answered }
    public State currentState = State.Idle;

    public float ringTimer = 10f;
    public float idleTimermMin = 10f;
    public float idleTimermMax = 60f;
    float timer;

    public AudioSource ringAudio;
    public AudioSource answeredAudio;

    bool timerEnded = false;

    void Start() {
        SwitchTo(State.Idle);
    }

    void StartTimer(float newTime) {
        timer = newTime;
    }

    void SwitchTo(State newState)
    {
        switch(newState)
        {
            case State.Idle:
                StartTimer(Random.Range(idleTimermMin, idleTimermMax));
            break;

            case State.Ringing:
                StartTimer(ringTimer);
                ringAudio.Play();
            break;

            case State.Answered:
                answeredAudio.Play();
            break;
        }
        currentState = newState;
    }

    void Update() {
        timer -= Time.deltaTime;

        timerEnded = timer <= 0;

        switch(currentState)
        {
            case State.Idle:
                Debug.Log("Idle " + timer);
                if (timerEnded) SwitchTo(State.Ringing);
            break;

            case State.Ringing:
                Debug.Log("Ringing " + timer);
                if (timerEnded) { /*GAME OVER*/ }
            break;

            case State.Answered:
                Debug.Log("Answered " + timer);
                if (!answeredAudio.isPlaying) SwitchTo(State.Idle);
            break;
        }
    }

    public void MouseHover() {
        
    }

    public void MouseUnhover() {
        
    }

    public void MouseClicDown() {
        Debug.Log("test");
        if (currentState == State.Ringing) SwitchTo(State.Answered);
    }
}
