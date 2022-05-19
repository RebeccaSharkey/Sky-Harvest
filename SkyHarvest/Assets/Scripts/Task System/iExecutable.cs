using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interfact which the Task class inherits from
/// Sets the rule that the sub classes that use the Task class must use the functions within
/// </summary>
public interface iExecutable
{
    /// <summary>
    /// Executes the task in parts depending on how many parts it is made up of
    /// </summary>
    /// <param name="_part">Int which controls what section of the task is being executed</param>
    public void ExecuteTask(int _part);

    /// <summary>
    /// Returns the tasks icon which will then be displayed onto the UI
    /// </summary>
    /// <returns>Returns the sprite logo of the task</returns>
    public Sprite GetIcon();

    /// <summary>
    /// Returns an Item class to be used when needed
    /// </summary>
    public SCR_Items GetItem();

    /// <summary>
    /// Returns an Item class to be used when needed
    /// </summary>
    public ItemQuality GetQuality();

}
