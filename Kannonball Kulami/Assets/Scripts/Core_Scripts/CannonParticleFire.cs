using UnityEngine;

class CannonParticleFire : MonoBehaviour
{
    public static CannonParticleFire Instance;

    public ParticleSystem CannonSmoke;

    void Awake()
    {
        // Register singleton
        if(Instance != null)
        {
            Debug.LogError("Multiple instances of singleton");
        }

        Instance = this;
    }

    public ParticleSystem CreateParticles()
    {
        ParticleSystem newPS = Instantiate(
            CannonSmoke,
            new Vector3(0, 0, 0),
            Quaternion.identity
            ) as ParticleSystem;

        newPS.transform.parent = GameObject.Find("ParticleObject").transform;
        newPS.transform.position = newPS.transform.parent.transform.position;

        Destroy(
            newPS.gameObject,
            newPS.startLifetime
            );

        return newPS;
    }

    public void Explosion(Vector3 position)
    {
        instantiate(CannonSmoke, position);
    }

    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newPS = Instantiate(
            prefab,
            position,
            Quaternion.identity
            ) as ParticleSystem;

        newPS.transform.parent = GameObject.Find("ParticleObject").transform;

        Destroy(
            newPS.gameObject,
            newPS.startLifetime
            );

        return newPS;
    }
}

