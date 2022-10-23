using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource[] _bgm;
    [SerializeField] private AudioSource[] _sfx;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlaySFX(string audioName)
    {
        for (int i = 0; i < _sfx.Length; i++)
        {
            if (audioName == _sfx[i].name)
            {
                _sfx[i].Play();
            }
        }
    }
}
