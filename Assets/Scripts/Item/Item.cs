using UnityEngine;
public class Item : MonoBehaviour
{
  [SerializeField]
  [ItemIdDescription]
  private string _itemId;
  private SpriteRenderer _spriteRenderer;

  public string ItemId
  {
    get { return _itemId; }
    set { _itemId = value; }
  }

  private void Awake()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    if (_itemId != null)
    {
      Init(ItemId);
    }
  }

  public void Init(string itemId)
  {
    ItemId = itemId;

  }
}