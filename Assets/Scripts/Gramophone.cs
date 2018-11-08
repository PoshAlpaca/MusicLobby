using UnityEngine;
using VRTK;

[RequireComponent(typeof(AudioSource))]
public class Gramophone : MonoBehaviour {
    Transform snapDropZoneTransform;
    VRTK_SnapDropZone snapDropZone;
    AudioSource audioSource;
    Disk disk;

    bool playing;
    
    const float spinSpeed = 360f;
    int songIndex = 0;

    void OnEnable() {
        // Search within gramophone's children for SnapDropZone object.
        if (snapDropZoneTransform == null) {
            snapDropZoneTransform = gameObject.transform.Find("SnapDropZone");

            // If we didn't find it, return.
            if (snapDropZoneTransform == null) {
                return;
            }
        }

        // If we don't have them yet, access required components.
        if (snapDropZone == null) {
            snapDropZone = snapDropZoneTransform.GetComponent<VRTK_SnapDropZone>();
        }
        
        if (audioSource == null) {
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        // Register our delegate functions.
        snapDropZone.ObjectSnappedToDropZone += DiskSnappedToDropZone;
        snapDropZone.ObjectUnsnappedFromDropZone += DiskUnsnappedFromDropZone;
    }

    void OnDisable() {
        // Unregister our delegate functions.
        snapDropZone.ObjectSnappedToDropZone -= DiskSnappedToDropZone;
        snapDropZone.ObjectUnsnappedFromDropZone -= DiskUnsnappedFromDropZone;
    }

    void Update() {
        if (disk == null) {
            return;
        }

        if (playing)
            Spin();

        // Check if song has ended
        if (audioSource.time == audioSource.clip.length) {
            LoadNextSong();
            audioSource.Play();
            Debug.Log("End of song");
        }
    }

    // Gets called by GramophoneButton
    public void Used(object sender, InteractableObjectEventArgs e) {
        if (disk == null) {
            Debug.Log("Disk missing");
            return;
        }

        Debug.Log("Length: " + audioSource.clip.length);

        playing = true;
        audioSource.Play();
        Debug.Log("Play");
    }

    // Also gets called by GramophoneButton
    public void Unused(object sender, InteractableObjectEventArgs e) {
        playing = false;
        audioSource.Pause();
        Debug.Log("Pause");
    }

    void DiskSnappedToDropZone(object sender, SnapDropZoneEventArgs e) {
        disk = e.snappedObject.GetComponent<Disk>();
        LoadNextSong();
        Debug.Log("Snap");
    }

    void DiskUnsnappedFromDropZone(object sender, SnapDropZoneEventArgs e) {
        disk = null;
        playing = false;
        audioSource.Stop();
        Debug.Log("Unsnap");
    }

    void Spin() {
        disk.transform.Rotate(new Vector3(0f, 0f, spinSpeed * Time.deltaTime));
    }

    void LoadNextSong() {
        // Check if end of disk has been reached, if yes start from the beginning again
        if (songIndex == disk.storedSongs.Length) {
            songIndex = 0;
            Debug.Log("End of disk");
        }

        audioSource.clip = disk.storedSongs[songIndex];
        songIndex++;
        Debug.Log("Load song");
    }
}