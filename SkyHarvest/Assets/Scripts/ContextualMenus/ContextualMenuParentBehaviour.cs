using UnityEngine;

/// <summary>
/// Monobehaviour that lives on the contextual menu parent object, enables object to listen for destroy event
/// </summary>
public class ContextualMenuParentBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        CustomEvents.UI.OnDestroyContextualMenu += DestroyContextualMenu;
    }

    private void OnDisable()
    {
        CustomEvents.UI.OnDestroyContextualMenu -= DestroyContextualMenu;
    }

    private void DestroyContextualMenu()
    {
        Destroy(gameObject);
    }
}
