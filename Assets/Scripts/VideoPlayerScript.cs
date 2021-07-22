using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"Game Over.mp4");
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
