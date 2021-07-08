using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;
    GameObject go;

    public void FractureObject()
    {
        gameObject.TryGetComponent(out AudioSource audioSource);

        go = Instantiate(fractured, transform.position, transform.rotation); //Spawn in the broken version
        go.transform.SetParent(gameObject.transform.parent.transform);
        AudioSource subSound = go.AddComponent<AudioSource>();
        SoundManager.instance.SetSFXAudio(subSound);
        subSound.clip = audioSource.clip;
        subSound.time = subSound.clip.length * 0.1f;
        subSound.Play();

        go.transform.position += Vector3.down * Time.deltaTime;
        Destroy(gameObject); //Destroy the object to stop it getting in the way
        Invoke(nameof(ThisDestroy), 2f);
    }

    void ThisDestroy()
    {
        Destroy(go);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball") FractureObject();
    }
}
