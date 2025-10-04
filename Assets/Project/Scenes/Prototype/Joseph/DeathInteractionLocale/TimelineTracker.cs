using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class TimelineTracker : MonoBehaviour
{
    public static TimelineTracker Instance { get; private set; }

    private HashSet<string> completedEvents = new HashSet<string>();
    private Dictionary<string, bool> eventFlags = new Dictionary<string, bool>();
    private string saveFilePath;

    [Serializable]
    public class SerializableFlag
    {
        public string key;
        public bool value;
    }
    [Serializable]
    private class SaveData
    {
        public List<string> completedEvents;
        public List<SerializableFlag> eventFlags;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // for testing purposes; otherwise it will got to appdata locallow
            saveFilePath = Path.Combine(Application.dataPath, "../saveFile.json");
            Debug.Log(Application.dataPath);

            // saveFilePath = Path.Combine(Application.persistentDataPath, "timeline_progress.json");

            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // completedEvents checks
    public bool IsEventCompleted(string eventId) => completedEvents.Contains(eventId);
    public void MarkEventCompleted(string eventId)
    {
        Debug.Log("the Event is saving");
        if (!completedEvents.Contains(eventId))
        {
            completedEvents.Add(eventId);
            SaveProgress(saveFilePath);
        }
    }

    // eventFlags checks and implementation
    public bool HasFlag(string eventId) => eventFlags.ContainsKey(eventId);
    public bool GetEvent(string eventName)
    {
        if (eventFlags.TryGetValue(eventName, out bool value))
        {
            return value;
        }

        Debug.LogWarning($"Event '{eventName}' does not exist!");
        return false; // default
    }
    public void SetEvent(string eventName, bool value)
    {
        if (eventFlags.ContainsKey(eventName))
        {
            eventFlags[eventName] = value; // update existing
        }
        else
        {
            eventFlags.Add(eventName, value); // add new
        }
    }

    public List<string> GetAllCompletedEvents() => new List<string>(completedEvents);

    private void SaveProgress(string saveFilePath)
    {
        var data = new SaveData
        {
            completedEvents = new List<string>(completedEvents),
            eventFlags = new List<SerializableFlag>()
        };

        foreach (var flag in eventFlags)
        {
            data.eventFlags.Add(new SerializableFlag { key = flag.Key, value = flag.Value });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Progress saved to: {saveFilePath}");
    }

    private void LoadProgress()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("No timeline_progress.json found — starting fresh.");
            SaveProgress(saveFilePath);
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            var data = JsonUtility.FromJson<SaveData>(json);

            completedEvents = new HashSet<string>(data.completedEvents ?? new List<string>());

            eventFlags.Clear();
            if (data.eventFlags != null)
            {
                foreach (var flag in data.eventFlags)
                {
                    this.SetEvent(flag.key, flag.value);
                }
            }

            Debug.Log("Timeline progress loaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load timeline progress: {ex.Message}");
        }
    }

    [System.Serializable]
    private class SerializableProgress
    {
        public List<string> dialogueIds = new List<string>();
        public SerializableProgress(HashSet<string> set)
        {
            dialogueIds.AddRange(set);
        }
    }

    #if UNITY_EDITOR
        [ContextMenu("Reset Progress")]
        private void ResetProgress()
        {
            completedEvents.Clear();
            eventFlags.Clear();
            SaveProgress(Path.Combine(Application.dataPath, "../saveFile.json"));
            Debug.Log("[TimelineTracker] Progress reset.");
        }
    #endif
}
