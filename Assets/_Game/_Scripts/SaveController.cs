using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;

    // Lifecycle method
    void Start()
    {
        InitializeSaveController();
        LoadGame();
    }

    // Sets up initial references and save path
    private void InitializeSaveController()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
    }

    // Saves the current game state to a JSON file
    public void SaveGame()
    {
        SaveData saveData = CollectSaveData();
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveLocation, json);
    }

    // Collects all data necessary for saving
    private SaveData CollectSaveData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,
            mapBoundary = confiner.m_BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems()
        };

        return saveData;
    }

    // Loads the game state from the save file
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            ApplySaveData(saveData);
        }
        else
        {
            // If no save exists, create one with default state
            SaveGame();
        }
    }

    // Applies loaded save data to the scene
    private void ApplySaveData(SaveData saveData)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        player.transform.position = saveData.playerPosition;

        // Set map boundary using saved PolygonCollider2D
        confiner.m_BoundingShape2D = GameObject.Find(saveData.mapBoundary)
            .GetComponent<PolygonCollider2D>();

        // Restore inventory
        inventoryController.SetInventoryItems(saveData.inventorySaveData);
    }
}
