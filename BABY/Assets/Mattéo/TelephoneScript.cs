using System.Transactions;
using DG.Tweening;
using UnityEngine;

public class TelephoneScript : MonoBehaviour, IInteractable
{
    public enum State{ Idle, Ringing, Taken, Answering, Victory, GameOver }
    public State currentState = State.Idle;

    public float ringTimer = 10f;
    public float idleTimermMin = 5f;
    public float idleTimermMax = 10f;
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

    public ParticleSystem ringParticles;

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
                ringParticles.Stop();
                player.Stop();
                gameObject.transform.GetChild(1).transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 1);
                gameObject.transform.GetChild(1).transform.DOLocalMove(new Vector3(0f,0.036f,0f), 1);
                StartTimer(Random.Range(idleTimermMin, idleTimermMax));
            break;

            case State.Ringing:
                StartTimer(ringTimer);
                var col = ringParticles.colorOverLifetime;
                col.enabled = true;
                Gradient grad = new Gradient();
                if (nextCall.hasToBeAnswered) 
                {
                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
                }
                else
                {
                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.yellow, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
                }
                col.color = grad;
                ringParticles.Play();
                player.Stop();
                player.loop = true;
                player.clip = ringAudio;
                player.Play();
            break;

            case State.Taken:
                gameObject.transform.GetChild(1).transform.DOLocalRotate(new Vector3(150f, 0f, 0f), 1);
                gameObject.transform.GetChild(1).transform.DOLocalMove(new Vector3(0.0560000017f,1.72099996f,0.574000001f), 1);
                ringParticles.Stop();
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
                if (timerEnded && nextCall.hasToBeAnswered) SwitchTo(State.GameOver);
                else if (timerEnded && !nextCall.hasToBeAnswered) SwitchTo(State.Idle);
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
                int i = Random.Range(9,12);
                nextCall.dialogue = dialogues[i];
                nextCall.answer = dialogues[i+4];
        }
        } while (currentCall.dialogue == nextCall.dialogue);
    }
}