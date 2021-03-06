﻿//  Copyright (C) 2020 Evercoast Inc. All Rights Reserved.

//
// Simplest form of Evercoast's playback controller, recommend for getting started. This script will "Play on awake" and not need an
// animation trigger. This scrip will also loop playback, according to the setting of the audio source.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlaybackControllerSimple : MonoBehaviour
{
    int frame_index = 0;
    int total_capture_frames = 0;
    GameObject[] mesh_parents;
    AudioSource performance_audio;

    System.DateTime start_time;

    void Start()
    {
        if (Application.isPlaying)
        {
            frame_index = 0;

            List<GameObject> meshList = new List<GameObject>();
            foreach (Component comp in this.gameObject.GetComponentsInChildren<Component>(true))
            {
                if (comp.CompareTag("EVERCOAST VOLUMETRIC"))
                    meshList.Add(comp.gameObject);
            }
            mesh_parents = meshList.ToArray();
            System.Array.Sort(mesh_parents, CompareObjNames);
            total_capture_frames = mesh_parents.Length;

            foreach (GameObject meshParent in mesh_parents)
                meshParent.SetActive(false);
            mesh_parents[0].SetActive(true);

            performance_audio = GetComponent<AudioSource>();
            performance_audio.Play();

            start_time = System.DateTime.Now;

            Debug.Log("# EVETRCOAST VOLUMETRIC FRAMES: " + total_capture_frames);
            Debug.Log("AUDIO CLIP LENGTH: " + performance_audio.clip.length);
        }
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            System.TimeSpan elapsed_time = System.DateTime.Now - start_time;

            mesh_parents[frame_index].SetActive(false);
            var t = performance_audio.timeSamples / ((float) performance_audio.clip.samples);
            var previous_frame_index = frame_index;
            frame_index = Mathf.FloorToInt(t * total_capture_frames);
            if (frame_index >= mesh_parents.Length)
                frame_index = previous_frame_index;

            mesh_parents[frame_index].SetActive(true);

            Debug.Log(
                "[" + elapsed_time.ToString(@"hh\:mm\:ss") + "]  |  INDEX: " + frame_index + "  |  AUDIO: " + 100 * t + "%" + System.Environment.NewLine +
                "[ " + mesh_parents[frame_index].name + "]"
            );
        }
    }

    void OnDisable()
    {
        List<GameObject> meshList = new List<GameObject>();
        foreach (Component comp in this.gameObject.GetComponentsInChildren<Component>(true))
        {
            if (comp.CompareTag("EVERCOAST VOLUMETRIC"))
                meshList.Add(comp.gameObject);
        }
        mesh_parents = meshList.ToArray();
        System.Array.Sort(mesh_parents, CompareObjNames);

        foreach (GameObject meshParent in mesh_parents)
            meshParent.SetActive(false);
        mesh_parents[0].SetActive(true);
    }

    int CompareObjNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }
}
