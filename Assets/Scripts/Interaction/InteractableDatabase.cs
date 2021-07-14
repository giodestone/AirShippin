using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// For accessing all interactables in a scene.
/// </summary>
public static class InteractableDatabase
{
    private static Dictionary<GameObject, Interactable> interactableDatabase = new Dictionary<GameObject, Interactable>();
    
    /// <summary>
    /// Register an interactable to the database.
    /// </summary>
    /// <param name="inter"></param>
    public static void RegisterInteractable(Interactable inter)
    {
        if (!interactableDatabase.ContainsKey(inter.gameObject))
            interactableDatabase.Add(inter.gameObject, inter);
    }

    public static void UnregiserInteractable(GameObject gameObject)
    {
        if (interactableDatabase.ContainsKey(gameObject))
            interactableDatabase.Remove(gameObject);
    }

    public static void UnregiserInteractable(Interactable inter)
    {
        UnregiserInteractable(inter.gameObject);
    }

    /// <summary>
    /// Get interactable if it is present.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="inter"></param>
    /// <returns></returns>
    public static bool GetInteractableIfPresent(GameObject go, out Interactable inter)
    {
        inter = null;

        if (interactableDatabase.ContainsKey(go))
        {
            inter = interactableDatabase[go];
            return true;
        }

        return false;
    }
}