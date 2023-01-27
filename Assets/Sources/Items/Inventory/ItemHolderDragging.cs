using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderDragging : MonoBehaviour, ICleanup
{
    private ItemHolder _proxy;

    private Image ProxyImage
        => Proxy.GetComponent<Image>();

    public ItemHolderContainer LinkedContainer
        => SourceHolder.OwningContainer;

    public Inventory LinkedInventory
        => LinkedContainer.Inventory;

    public ItemHolder SourceHolder
        => GetComponent<InventorySlot>();

    public bool IsDraggingNow
        => _proxy != null;

    public ItemHolder DraggableHolder
        => Proxy;

    private ItemHolder Proxy
    {
        get
        {
            if (IsDraggingNow)
            {
                return _proxy;
            }

            _proxy = LinkedInventory.ItemHolderFactory
                .Create("Draggable Slot", LinkedContainer);

            Proxy.Released
                += OnProxyReleased;

            SourceHolder.MoveItemTo(_proxy);

            return _proxy;
        }
    }

    public void BeginDrag()
    {
        if (IsDraggingNow && !SourceHolder.HasItem)
        {
            return;
        }

        ProxyImage
            .raycastTarget = false;
    }

    private void ContinueDrag()
    {
        if (!IsDraggingNow)
        {
            return;
        }

        Proxy.transform
            .position = Input.mousePosition;
    }

    public void EndDrag()
    {
        if (!IsDraggingNow)
        {
            return;
        }

        ProxyImage
            .raycastTarget = true;

        Release();
    }

    public void Release()
    {
        if (Proxy.HasItem)
        {
            Proxy.MoveItemTo(SourceHolder);
        }

        Proxy.Released
            -= OnProxyReleased;

        Proxy.Release();

        Destroy(Proxy.gameObject);
        Destroy(this);
    }

    public void Update()
        => ContinueDrag();

    private void OnProxyReleased()
        => EndDrag();
}