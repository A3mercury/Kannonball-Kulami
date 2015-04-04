using UnityEngine;

class CannonParticleFire : MonoBehaviour
{
    public static CannonParticleFire Instance;

    public ParticleSystem CannonSmoke;

    void Awake()
    {
        // Register singleton
        if(Instance != null)
            Debug.LogError("Multiple instances of CannonParticleSystem");

        Instance = this;
    }

    public ParticleSystem CreateParticles(string parent)
    {
        ParticleSystem newPS = Instantiate(
            CannonSmoke,
            new Vector3(0, 0, 0),
            Quaternion.identity
            ) as ParticleSystem;

        //if (parent == "PlayerParticleObject")
        //    newPS.transform.parent = GameObject.Find("PlayerParticleObject").transform;
        //else if (parent == "OpponentParticleObject")
        //    newPS.transform.parent = GameObject.Find("OpponentParticleObject").transform;

        //newPS.transform.position = newPS.transform.parent.transform.position;

        //Destroy(
        //    newPS.gameObject,
        //    newPS.startLifetime
        //    );

        return newPS;
    }
}

