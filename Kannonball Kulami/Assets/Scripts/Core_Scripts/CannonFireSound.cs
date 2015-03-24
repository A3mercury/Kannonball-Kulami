using UnityEngine;

[RequireComponent(typeof(AudioSource))]
class CannonFireSound : MonoBehaviour 
{
    public static CannonFireSound Instance;

    public static float volume;
    public AudioSource CannonSound;

	void Awake()
    {
        // Register singleton
        if (Instance != null)
            Debug.LogError("Multiple instances of CannonFireSound");

        Instance = this;
        volume = 1f;
    }

    public void FireCannon()
    {
        AudioSource newAS = Instantiate(
            CannonSound,
            new Vector3(0, 0, 0),
            Quaternion.identity
        ) as AudioSource;

        newAS.transform.parent = GameObject.Find("AudioSounds").transform;
        newAS.volume = GetVolume();
        newAS.PlayOneShot(newAS.clip);

        //Destroy(newAS.gameObject);
    }

    public static void SetVolume(float v) 
    {
        volume = v;
    }

    public float GetVolume()
    {
        return volume;
    }
}
