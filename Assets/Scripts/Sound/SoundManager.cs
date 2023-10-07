using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("InGame")]
    [SerializeField] private AudioSource die;
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioSource wing;
    [SerializeField] private AudioSource point;

    [Header("Bg")]
    [SerializeField] private AudioSource bgGame;
    [SerializeField] private AudioSource bgMenu;

    [Header("Other")]
    [SerializeField] private AudioSource bubble;
    [SerializeField] private AudioSource coin;
    [SerializeField] private AudioSource label;

    public AudioSource Die => die;
    public AudioSource Hit => hit;
    public AudioSource Wing => wing;
    public AudioSource Point => point;
    public AudioSource BgGame => bgGame;
    public AudioSource BgMenu => bgMenu;
    public AudioSource Bubble => bubble;
    public AudioSource Coin => coin;
    public AudioSource Label => label;

    private float _bgMenuCurrentTime;
    private float _bgGameCurrentTime;

    private void Awake()
    {
        _bgGameCurrentTime = Random.Range(25, 100);
        _bgMenuCurrentTime = Random.Range(19, 27);

        StartCoroutine(SoundFadeIn(bgMenu, 3));

        bgMenu.Play();
    }

    private void Update()
    {
        CheckMusicTimeOffset(bgMenu, 128, 10);
        CheckMusicTimeOffset(bgGame, 156, 8);
    }

    public void PlayDie() => die.Play();

    public void PlayHit() => hit.Play();

    public void PlayWing() => wing.Play();

    public void PlayPoint() => point.Play();

    public void PlayBubble() => bubble.Play();

    public void PlayCoin() => coin.Play();

    public void PlayLabel() => label.Play();

    public IEnumerator SoundFadeOut(AudioSource audio, float time)
    {
        float timeLeft = time;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            audio.volume = timeLeft / time;

            yield return null;
        }

        if (audio == bgGame) _bgGameCurrentTime = bgGame.time;
        if (audio == bgMenu) _bgMenuCurrentTime = bgMenu.time;

        audio.enabled = false;
    }

    public IEnumerator SoundFadeIn(AudioSource audio, float time)
    {
        audio.enabled = true;

        if (audio == bgGame) audio.time = _bgGameCurrentTime;
        if (audio == bgMenu) audio.time = _bgMenuCurrentTime;

        audio.Play();

        float timeLeft = 0;

        while (timeLeft < time)
        {
            timeLeft += Time.deltaTime;

            audio.volume = timeLeft / time;

            yield return null;
        }
    }

    private void CheckMusicTimeOffset(AudioSource audio, float endPosition, float startPosition)
    {
        if (audio.time >= endPosition)
        {
            audio.time = startPosition;
        }
    }
}
