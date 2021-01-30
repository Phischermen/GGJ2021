using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float minSpawnDistance;
    public float maxSpawnDistance;
    public GameObject wavePrefab;
    public WaitForSeconds waveTimer = new WaitForSeconds(0.1f);
    public float minWaveDuration;
    public float maxWaveDuration;
    public float minWaveSpeed;
    public float maxWaveSpeed;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            yield return waveTimer;
            var offset = Random.insideUnitCircle * Random.Range(minSpawnDistance, maxSpawnDistance);
            var waveObject = Instantiate(wavePrefab, transform.position + new Vector3(offset.x, offset.y, 0f), Quaternion.identity);
            var waveComponent = waveObject.GetComponent<Wave>();
            waveComponent.waveDuration = Random.Range(minWaveDuration, maxWaveDuration);
            waveComponent.velocity = Random.onUnitSphere * Random.Range(minWaveSpeed, maxWaveSpeed);
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
