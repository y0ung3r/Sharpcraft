using System;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<Item> DragBeginned;

    public event Action<ItemType, int> ItemDropped;

    [SerializeField]
    private Player _player;

    [SerializeField] 
    private ItemFactory _itemFactory;

    [SerializeField]
    private CursorAccessor _cursorAccessor;

    [SerializeField]
    private ItemHolderFactory _holderFactory;

    [SerializeField] 
    private Backpack _backpack;

    [SerializeField] 
    private Toolbar _toolbar;

    public bool HasDraggableHolder
        => CurrentDragging != null && CurrentDragging.IsDraggingNow;

    public Player Player
        => _player;

    public ItemFactory ItemFactory 
        => _itemFactory;

    public ItemHolderFactory ItemHolderFactory
        => _holderFactory;
    
    public Backpack Backpack 
        => _backpack;

    public Toolbar Toolbar 
        => _toolbar;

    public ItemHolderDragging CurrentDragging { get; private set; }

    public bool IsOpen
    {
        get => gameObject.activeSelf;
        
        set
        {
            if (!value)
            {
                EndDragHolder();
            }
            
            _cursorAccessor.Shown = value;

            Player.FreezeRotations = 
                Player.StopBuilding = 
                    Player.StopBreaking = value;

            gameObject.SetActive(value);
        }
    }

    public void Start()
    {
        var apple = ItemFactory
            .Create(ItemType.Apple);

        var chainHelmet = ItemFactory
            .Create(ItemType.ChainHelmet);

        var glassBlock = ItemFactory
            .Create(ItemType.GlassBlock);

        var stoneBlock = ItemFactory
            .Create(ItemType.StoneBlock);

        var grassBlock = ItemFactory
            .Create(ItemType.GrassBlock);

        Backpack.GetHolder(20)
            .PutItem(glassBlock, 64);

        Backpack.GetHolder(21)
            .PutItem(stoneBlock, 64);

        Backpack.GetHolder(22)
            .PutItem(grassBlock, 64);

        Backpack.GetHolder(5)
            .PutItem(apple, 5);

        Backpack.GetHolder(6)
            .PutItem(chainHelmet, 4);
    }

    private void OnItemDropped(ItemType itemType, int amount)
        => ItemDropped?.Invoke(itemType, amount);

    public void BeginDragHolder(ItemHolder holderToDrag)
    {
        if (HasDraggableHolder)
        {
            EndDragHolder();
        }

        CurrentDragging = holderToDrag
            .AddComponent<ItemHolderDragging>();

        DragBeginned?
            .Invoke(holderToDrag.Item);

        CurrentDragging
            .BeginDrag();

        CurrentDragging.DraggableHolder
            .ItemDropped += OnItemDropped;
    }

    public void EndDragHolder()
    {
        if (!HasDraggableHolder)
        {
            return;
        }

        CurrentDragging.DraggableHolder
            .ItemDropped -= OnItemDropped;

        CurrentDragging
            .EndDrag();

        CurrentDragging = null;
    }
}