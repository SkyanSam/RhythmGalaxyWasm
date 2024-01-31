using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System.IO;
using System;
using static Raylib_cs.Raylib;
using Raylib_cs;

public class SongManager
{
    public static SongManager Instance;
    public static Dictionary<NoteName, List<double>> noteTimestamps = new Dictionary<NoteName, List<double>>();
    public static Dictionary<NoteName, List<double>> noteLengths = new Dictionary<NoteName, List<double>>();
    public float songDelayInSeconds;
    public string audioLocation;
    public string midiLocation;

    public static MidiFile midiFile;
    public static Music music;
    public static int BPM;
    public static int step;
    private void ReadFromFile()
    {
        midiFile = MidiFile.Read($"{Directory.GetCurrentDirectory()}/Resources/Audio/{midiLocation}");
        music = LoadMusicStream($"Resources/{audioLocation}");
        GetDataFromMidi();
    }
    private void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);
        SetTimeStamps(array);
    }
    private void StartSong()
    {
        Task.Run(async () =>
        {
            await Task.Delay((int)(songDelayInSeconds * 1000));
            PlayMusicStream(music);
        });
    }
    public static float GetAudioSourceTime()
    {
        return GetMusicTimePlayed(music);
    }
    public static int GetCurrentStep()
    {
        return GetCurrentStep(GetAudioSourceTime());
    }
    public static int GetCurrentStep(float time)
    {
        return (int)(time / (60f / BPM));
    }
    public static float GetCurrentStepFloat()
    {
        return GetCurrentStepFloat(GetAudioSourceTime());
    }
    public static float GetCurrentStepFloat(float time)
    {
        return (time / (60f / BPM));
    }
    public static float GetCurrentStepRemainder()
    {
        return GetCurrentStepRemainder(GetAudioSourceTime());
    }
    public static float GetCurrentStepRemainder(float time)
    {
        return GetCurrentStepFloat(time) - GetCurrentStep(time);
    }
    public static bool IsStartNoteAtStep(int step, NoteName noteName)
    {
        foreach (var timeStamp in noteTimestamps[noteName])
            if (GetCurrentStep((float)timeStamp) == step) 
                return true;
        return false;
    }
    // use this info to get the step get the note and draw in ui
    // make custom lerp function if needed for tweening
    public static bool IsMidNoteAtStep(int step, NoteName noteName)
    {
        var stepTimeStamp = step * (60f / BPM);
        for (int i = 0; i < noteTimestamps[noteName].Count; i++)
        {
            var timeStamp = noteTimestamps[noteName][i];
            var length = noteLengths[noteName][i];
            if (timeStamp < stepTimeStamp && stepTimeStamp < timeStamp + length) 
                return true;
        }
        return false;
    }
    public static bool IsEndNoteAtStep(int step, NoteName noteName)
    {
        for (int i = 0; i < noteTimestamps[noteName].Count; i++)
        {
            var timeStamp = noteTimestamps[noteName][i];
            var length = noteLengths[noteName][i];
            if (GetCurrentStep((float)(timeStamp + length)) == step)
                return true;
        }
        return false;
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        MetricTimeSpan metricTimeSpan;
        foreach (var note in array)
        {
            if (noteTimestamps[note.NoteName] == null)
            {
                noteTimestamps[note.NoteName] = new List<double>();
                noteLengths[note.NoteName] = new List<double>();
            }
            metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, midiFile.GetTempoMap());
            noteTimestamps[note.NoteName].Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Length, midiFile.GetTempoMap());
            noteLengths[note.NoteName].Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
        }
    }
    public void Start(string musicLoc, string midiLoc, int bpm)
    {
        audioLocation = musicLoc;
        midiLocation = midiLoc;
        BPM = bpm;
        ReadFromFile();
        GetDataFromMidi();
        StartSong();
    }
    public void Update()
    {
        UpdateMusicStream(music);
    }
    
}
