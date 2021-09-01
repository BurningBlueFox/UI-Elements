using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class InventoryUiController : MonoBehaviour
{
    private static VisualElement ghostIcon;
    private static bool isDragging;
    private static InventorySlot originalSlot;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private VisualElement root = default;
    private VisualElement slotContainer = default;

    public static void StartDrag(Vector2 position, InventorySlot inventorySlot)
    {
        isDragging = true;
        originalSlot = inventorySlot;

        ghostIcon.style.top = position.y - ghostIcon.layout.height / 2;
        ghostIcon.style.left = position.x - ghostIcon.layout.width / 2;

        ghostIcon.style.backgroundImage = GameController.GetItemByGuid(originalSlot.itemGuid).Icon.texture;

        ghostIcon.style.visibility = Visibility.Visible;
    }

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        slotContainer = root.Query<VisualElement>("SlotContainer").First();

        for (int i = 0; i < 20; i++)
        {
            InventorySlot item = new InventorySlot();

            slots.Add(item);
            slotContainer.Add(item);
        }
        ghostIcon = root.Query<VisualElement>("GhostIcon").First();

        GameController.OnInventoryChanged += GameController_OnInventoryChanged;
        ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }
    private void OnPointerMove(PointerMoveEvent evt)
    {
        //Only take action if the player is dragging an item around the screen
        if (!isDragging)
        {
            return;
        }
        //Set the new position
        ghostIcon.style.top = evt.position.y - ghostIcon.layout.height / 2;
        ghostIcon.style.left = evt.position.x - ghostIcon.layout.width / 2;
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!isDragging)
        {
            return;
        }
        //Check to see if they are dropping the ghost icon over any inventory slots.
        IEnumerable<InventorySlot> slotsCopy = slots.Where(x =>
               x.worldBound.Overlaps(ghostIcon.worldBound));
        //Found at least one
        if (slotsCopy.Count() != 0)
        {
            InventorySlot closestSlot = slotsCopy.OrderBy(x => Vector2.Distance
               (x.worldBound.position, ghostIcon.worldBound.position)).First();

            //Set the new inventory slot with the data
            closestSlot.HoldItem(GameController.GetItemByGuid(originalSlot.itemGuid));

            //Clear the original slot
            originalSlot.DropItem();
        }
        //Didn't find any (dragged off the window)
        else
        {
            originalSlot.icon.image =
                  GameController.GetItemByGuid(originalSlot.itemGuid).Icon.texture;
        }
        //Clear dragging related visuals and data
        isDragging = false;
        originalSlot = null;
        ghostIcon.style.visibility = Visibility.Hidden;
    }



    private void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
    {
        foreach (var item in itemGuid)
        {
            if(change == InventoryChangeType.Pickup)
            {
                var emptySlot = slots.FirstOrDefault(x => x.itemGuid.Equals(""));

                if (emptySlot != null)
                    emptySlot.HoldItem(GameController.GetItemByGuid(item));
            }
        }
    }
}
