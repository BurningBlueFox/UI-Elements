using System;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public Image icon = default;
    public string itemGuid = "";

    public InventorySlot()
    {
        icon = new Image();
        Add(icon);

        icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");

        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        if (evt.button != 0 || itemGuid.Equals(""))
            return;

        icon.image = null;

        InventoryUiController.StartDrag(evt.position, this);
    }

    public void HoldItem(ItemDetails item)
    {
        icon.image = item.Icon.texture;
        itemGuid = item.GUID;
    }

    public void DropItem()
    {
        itemGuid = "";
        icon.image = null;
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}