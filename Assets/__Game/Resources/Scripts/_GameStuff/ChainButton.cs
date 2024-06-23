using Assets.__Game.Resources.Scripts.Enums;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class ChainButton : MonoBehaviour
  {
    public event Action<ChainEnums> ButtonCLicked;

    [SerializeField] private ChainEnums _type;

    private Button _button;

    private void Awake()
    {
      _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
      _button.onClick.AddListener(() =>
      {
        ButtonCLicked?.Invoke(_type);
      });
    }
  }
}