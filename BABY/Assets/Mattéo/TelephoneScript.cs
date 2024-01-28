using System.Transactions;
using UnityEngine;

public class TelephoneScript : MonoBehaviour, IInteractable
{
    public enum State{ Idle, Ringing, Taken, Answering, Victory, GameOver }
    public State currentState = State.Idle;

    public float ringTimer = 10f;
    public float idleTimermMin = 2f;
    public float idleTimermMax = 5f;
    float timer;

    public AudioSource player;

    //Sonnerie du téléphone
    public AudioClip ringAudio;

    public AudioClip[] dialogues;

    struct Call
    {
        public AudioClip dialogue;
        public bool hasToBeAnswered;
        public AudioClip answer;
    }
    Call nextCall;

    bool timerEnded = false;

    void Start() {
        chooseNextCall();
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

            case State.Taken:
                player.Stop();
                player.loop = false;
                player.clip = nextCall.dialogue;
                Debug.Log("Playing " + player.clip);
                player.Play();
            break;

            case State.Answering:
                player.Stop();
                player.loop = false;
                player.clip = nextCall.answer;
                Debug.Log("Playing " + player.clip);
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
                if (timerEnded && nextCall.hasToBeAnswered) { SwitchTo(State.GameOver); }
            break;

            case State.Taken:
                //Debug.Log("Taken " + timer);
                if (!player.isPlaying) SwitchTo(State.Answering);
            break;

            case State.Answering:
                if (!player.isPlaying) { SwitchTo(State.Idle); chooseNextCall();}
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
        if (currentState == State.Ringing) SwitchTo(State.Taken);
    }

    void chooseNextCall() {
        Call currentCall = nextCall;
         // Pour ne pas sélectionner le même appel 2 fois de suite
        do {
            nextCall.hasToBeAnswered = (Random.value > 0.5f);
            if(nextCall.hasToBeAnswered) {
                int i = Random.Range(1,4);
                nextCall.dialogue = dialogues[i];
                nextCall.answer = dialogues[i+4];
            } else {
                int i = Random.Range(9,13);
                nextCall.dialogue = dialogues[i];
                nextCall.answer = dialogues[i+5];
        }
        } while (currentCall.dialogue == nextCall.dialogue);
    }
}