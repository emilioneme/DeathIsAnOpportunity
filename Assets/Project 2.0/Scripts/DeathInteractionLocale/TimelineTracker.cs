using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TimelineTracker : MonoBehaviour
{
    public static TimelineTracker Instance { get; private set; }

    private HashSet<string> completedEvents = new HashSet<string>();
    private Dictionary<string, bool> eventFlags = new Dictionary<string, bool>();
    private Dictionary<string, float> upgradeTracker = new Dictionary<string, float>();
    private string saveFilePath;



    [Serializable]
    public class SerializableFlag
    {
        public string key;
        public bool value;
    }
    [Serializable]
    public class SerializableUpgrade
    {
        public string key;
        public float fvalue;
    }
    [Serializable]
    public class SerializablePlayerUpgradeData
    {
        public float moveSpeed;
        public float maxHorizontalSpeed;
        public float groundAcceleration;
        public float airAcceleration;
        public float groundLinearDrag;
        public float airLinearDrag;
        public float jumpImpulse;
        public int maxAirJumps;
        public float coyoteTime;
        public float jumpBuffer;
        public float fallGravityMultiplier;
        public float lowJumpGravityMultiplier;
        public float fireRate;
        public float projectileSize;
        public float projectileDamage;
        public float projectileSpeed;
        public float projectileLife;
        public int projectilesPerShot;
        public float projectileSpreadRadius;
        public float projectileAngleVariance;
    }



    [Serializable]
    private class SaveData
    {
        public List<string> completedEvents;
        public List<SerializableFlag> eventFlags;
        public List<SerializableUpgrade> upgradeTracker;
        //public SerializablePlayerUpgradeData playerUpgradeData;
    }



    private void Awake()
    {
        // for testing purposes; otherwise it will got to appdata locallow
        saveFilePath = Path.Combine(Application.dataPath, "../saveFile.json");

        // saveFilePath = Path.Combine(Application.persistentDataPath, "timeline_progress.json");

        LoadProgress();

    }




    // completedEvents checks
    public bool IsEventCompleted(string eventId) 
    { 
        return completedEvents.Contains(eventId); 
    }
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






    // upgradeTracker checks and implementation
    public bool HasUpgrade(string eventId) => upgradeTracker.ContainsKey(eventId);
    public float GetUpgrade(string upgradeId)
    {
        if (upgradeTracker.TryGetValue(upgradeId, out float value))
        {
            return value;
        }

        Debug.Log($"Event '{upgradeTracker}' does not exist!");
        return 0f; // default
    }
    public void SetUpgrade(string upgradeId, float value)
    {
        if (upgradeTracker.ContainsKey(upgradeId))
        {
            upgradeTracker[upgradeId] = value; // update existing
        }
        else
        {
            upgradeTracker.Add(upgradeId, value); // add new
        }
    }

    public List<string> GetAllCompletedEvents() => new List<string>(completedEvents);





    private void SaveProgress(string saveFilePath)
    {
        var data = new SaveData
        {
            completedEvents = new List<string>(completedEvents),
            eventFlags = new List<SerializableFlag>(),
            upgradeTracker = new List<SerializableUpgrade>(),
            //playerUpgradeData = ConvertToSerializable(GameManager.Instance.upgradeData)
        };

        foreach (var flag in eventFlags)
        {
            data.eventFlags.Add(new SerializableFlag { key = flag.Key, value = flag.Value });
        }

        foreach (var upgrade in upgradeTracker)
        {
            data.upgradeTracker.Add(new SerializableUpgrade { key = upgrade.Key, fvalue = upgrade.Value });
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

            upgradeTracker.Clear();
            if (data.upgradeTracker != null)
            {
                foreach (var upgrade in data.upgradeTracker)
                {
                    this.SetUpgrade(upgrade.key, upgrade.fvalue);
                }
            }


            //if (data.playerUpgradeData != null)
            //{
            //    ApplyToUpgradeData(GameManager.Instance.upgradeData, data.playerUpgradeData);
            //}
            //Debug.Log("Timeline progress loaded successfully.");
            Debug.Log("Load Successful");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load timeline progress: {ex.Message}");
        }
    }


    private void ResetProgress()
    {
        completedEvents.Clear();
        eventFlags.Clear();
        upgradeTracker.Clear();
        SaveProgress(Path.Combine(saveFilePath, "../saveFile.json"));
        Debug.Log("[TimelineTracker] Progress reset.");
    }


    //private void ApplyToUpgradeData(PlayerUpgradeData target, SerializablePlayerUpgradeData saved)
    //{
    //    if (saved == null || target == null)
    //        return;

    //    target.moveSpeed = saved.moveSpeed;
    //    target.maxHorizontalSpeed = saved.maxHorizontalSpeed;
    //    target.groundAcceleration = saved.groundAcceleration;
    //    target.airAcceleration = saved.airAcceleration;
    //    target.groundLinearDrag = saved.groundLinearDrag;
    //    target.airLinearDrag = saved.airLinearDrag;
    //    target.jumpImpulse = saved.jumpImpulse;
    //    target.maxAirJumps = saved.maxAirJumps;
    //    target.coyoteTime = saved.coyoteTime;
    //    target.jumpBuffer = saved.jumpBuffer;
    //    target.fallGravityMultiplier = saved.fallGravityMultiplier;
    //    target.lowJumpGravityMultiplier = saved.lowJumpGravityMultiplier;
    //    target.fireRate = saved.fireRate;
    //    target.projectileSize = saved.projectileSize;
    //    target.projectileDamage = saved.projectileDamage;
    //    target.projectileSpeed = saved.projectileSpeed;
    //    target.projectileLife = saved.projectileLife;
    //    target.projectilesPerShot = saved.projectilesPerShot;
    //    target.projectileSpreadRadius = saved.projectileSpreadRadius;
    //    target.projectileAngleVariance = saved.projectileAngleVariance;

    //    target.NotifyChanged();
    //}


#if UNITY_EDITOR
    [ContextMenu("Reset Progress")]
        private void DebugReset()
        {
            completedEvents.Clear();
            eventFlags.Clear();
            upgradeTracker.Clear();
            SaveProgress(Path.Combine(Application.dataPath, "../saveFile.json"));
            Debug.Log("[TimelineTracker] Progress reset.");
        }
#endif
}
