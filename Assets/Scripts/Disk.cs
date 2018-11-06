using UnityEngine;

public class Disk : MonoBehaviour {
    public AudioClip[] storedSongs;

    void OnEnable () {
        Debug.Assert(storedSongs != null);
    }
}