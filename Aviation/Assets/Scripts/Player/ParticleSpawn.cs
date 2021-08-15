using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private HashSet<int> ids;
    // Start is called before the first frame update
    void Start()
    {
        ids = new HashSet<int>();
        AviationEventManagerGui.Instance.onItemPickup += itemPickup;
        AviationEventManagerGui.Instance.onBooster += spawnEffect;
    }

    private void itemPickup(int id)
    {
        if (!ids.Contains(id))
        {
            ids.Add(id);
            spawnEffect();
        }
    }

    private void spawnEffect()
    {
        GameObject obj = Instantiate(effect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
    }
}
