using DG.Tweening;
using UnityEngine;

public class TelephoneScript : MonoBehaviour, IInteractable
{
    public enum State{ Idle, Ringing, Taken, Answering, Victory, GameOver }
    public State currentState = State.Idle;

    public float ringTimer = 10f;
    public float idleTimermMin = 5f;
    public float idleTimermMax = 10f;
    float timer = 15f;

    public AudioSource player;
    public Transform combine;
    Vector3 combineStartPos;
    Vector3 combineStartRot;
    Vector3 combineStartScale;


    //Sonnerie du téléphone
    public AudioClip ringAudio, hangUpAudio;

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
        combineStartPos = combine.localPosition;
        combineStartRot = combine.localEulerAngles;
        combineStartScale = combine.localScale;
        chooseNextCall();
        SwitchTo(State.Idle);

    }

    void StartTimer(float newTime) {
        timer = newTime;
    }

    void SwitchTo(State newState)
    {
        switch (currentState)
        {
            case State.Answering:
                Sound.access.PlayWithDelay(hangUpAudio,1f, 0.5f);
            break;
        }


        switch(newState)
        {
            case State.Idle:
                ringParticles.Stop();
                player.Stop();
                combine.DOKill();
                combine.DOLocalRotate(combineStartRot, 1);
                combine.DOLocalMove(combineStartPos, 1);
                combine.DOScale(combineStartScale,1f);
                float newTimer = Random.Range(idleTimermMin, idleTimermMax);
                StartTimer(newTimer);
                Debug.Log("sonnerie dans "+newTimer+" secondes");
            break;

            case State.Ringing:
                Debug.Log("LE TÉLÉPHONE SONNE");
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
                combine.DOKill();
                combine.DOLocalRotate(new Vector3(34f, -34f, 0f), 1f);
                combine.DOLocalMove(new Vector3(-6f,10f,0), 1f).SetEase(Ease.OutBack);
                combine.DOScale(6.7f,1f);
                ringParticles.Stop();
                player.Stop();
                player.loop = false;
                player.clip = nextCall.dialogue;
                Sound.access.Play(hangUpAudio,1f);
                Debug.Log("Playing " + player.clip);
                player.Play();
            break;

            case State.Answering:
                player.Stop();
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

        timerEnded = (timer <= 0);

        switch(currentState)
        {
            case State.Idle:
                //Debug.Log("Idle " + timer);
                if (timerEnded) SwitchTo(State.Ringing);
            break;

            case State.Ringing:
                //Debug.Log("Ringing " + timer);
                if (timerEnded && nextCall.hasToBeAnswered) SwitchTo(State.GameOver);
                else if (timerEnded && !nextCall.hasToBeAnswered) { chooseNextCall(); SwitchTo(State.Idle); }
                //Debug.Log("Timer " + timer);
            break;

            case State.Taken:
                //Debug.Log("Taken " + timer);
                if (!player.isPlaying) SwitchTo(State.Answering);
            break;

            case State.Answering:
                if (!player.isPlaying) { chooseNextCall(); SwitchTo(State.Idle); }
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