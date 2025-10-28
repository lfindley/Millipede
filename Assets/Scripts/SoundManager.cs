using UnityEngine;

public enum SoundEvent
{
    Shoot,
    EnemyHit,
    PlayerHit,
    MushroomBreak,
    MushroomHit,
    GameOver,
    LevelUp
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager I { get; private set; }

    [Header("Assign 1+ clips for each event (randomized)")]
    public AudioClip[] shoot;
    public AudioClip[] enemyHit;
    public AudioClip[] playerHit;
    public AudioClip[] mushroomBreak;
    public AudioClip[] mushroomHit;
    public AudioClip[] gameOver;
    public AudioClip[] levelUp;

    private AudioSource source; // 2D/UI one-shot player

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D
    }

    public void Play(SoundEvent evt)
    {
        var clip = GetClip(evt);
        if (clip != null)
            source.PlayOneShot(clip);
    }

    public void PlayAtPosition(SoundEvent evt, Vector3 worldPos)
    {
        var clip = GetClip(evt);
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, worldPos); // simple 3D one-shot
    }

    // --- helpers ---
    AudioClip GetClip(SoundEvent evt)
    {
        switch (evt)
        {
            case SoundEvent.Shoot: return Pick(shoot);
            case SoundEvent.EnemyHit: return Pick(enemyHit);
            case SoundEvent.PlayerHit: return Pick(playerHit);
            case SoundEvent.MushroomBreak: return Pick(mushroomBreak);
            case SoundEvent.MushroomHit: return Pick(mushroomHit);
            case SoundEvent.GameOver: return Pick(gameOver);
            case SoundEvent.LevelUp: return Pick(levelUp);
            default: return null;
        }
    }

    AudioClip Pick(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        if (clips.Length == 1) return clips[0];
        return clips[Random.Range(0, clips.Length)];
    }
}
