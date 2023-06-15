using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuesetObserver
{
    void NotifyQuestProgress(string questName, int progress);
    void NotifyQuestCompletion(string questName);
}

public class Quest
{
    List<IQuesetObserver> questObservers = new List<IQuesetObserver>();
    Dictionary<string, int> questProgress = new Dictionary<string, int>();

    public void RegisterObserver(IQuesetObserver observer)
    {
        questObservers.Add(observer);
    }

    public void UnregisterObserver(IQuesetObserver observer)
    {
        questObservers.Remove(observer);
    }

    // Notify observers about quest progress
    private void NotifyQuestProgress(string questName, int progress)
    {
        foreach (var observer in questObservers)
        {
            observer.NotifyQuestProgress(questName, progress);
        }
    }

    // Notify observers about quest completion
    public void NotifyQuestCompletion(string questName)
    {
        foreach (var observer in questObservers)
        {
            observer.NotifyQuestCompletion(questName);
        }
    }

    // Update quest progress and notify observers
    public void UpdateQuestProgress(string questName, int progress)
    {
        // Update the progress
        if(questProgress.ContainsKey(questName))
        {
            questProgress[questName] += progress;
        }
        else
        {
            questProgress.Add(questName, progress);
        }

        if(questName == "Catch 5 monsters")
        {
            int currentProgress = questProgress[questName];
            if (currentProgress >= 1)
                CompleteQuest(questName);
        }

        // Notify observers about the progress
        NotifyQuestProgress(questName, progress);
    }

    // Update quest completion and notify observers
    public void CompleteQuest(string questName)
    {
        // Mark the quest as completed

        // Notify observers about the completion
        NotifyQuestCompletion(questName);
    }
}
