using UnityEngine;
using UnityEngine.Rendering;

public class TelephoneScript : MonoBehaviour, IInteractable
{
    public enum State{ Idle, Ringing, Answered, Victory, GameOver }
    public State currentState = State.Idle;

    public float ringTimer = 10f;
    public float idleTimermMin = 2f;
    public float idleTimermMax = 5f;
    float timer;

    public AudioSource player;
    public AudioClip ringAudio;
    public AudioClip answeredAudio;

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
                player.Stop();
                player.loop = true;
                player.clip = ringAudio;
                player.Play();
            break;

            case State.Answered:
                player.Stop();
                player.loop = false;
                player.clip = answeredAudio;
                player.Play();
            break;

            case State.Victory:
                GAMEMANAGER.access.Victoire();
                player.Stop();
            break;

            case State.GameOver:
                GAMEMANAGER.access.GameOver();
                player.Stop();
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
                //Debug.Log("Idle " + timer);
                if (timerEnded) SwitchTo(State.Ringing);
            break;

            case State.Ringing:
                //Debug.Log("Ringing " + timer);
                if (timerEnded) { SwitchTo(State.GameOver); }
            break;

            case State.Answered:
                //Debug.Log("Answered " + timer);
                if (!player.isPlaying) SwitchTo(State.Idle);
            break;

            case State.Victory:

            break;

            case State.GameOver:

            break;
        }
    }

    public void MouseHover() {
        
    }

    public void MouseUnhover() {
        
    }

    public void MouseClicDown() {
        Debug.Log("Click téléphone");
        if (currentState == State.Ringing) SwitchTo(State.Answered);
    }
}