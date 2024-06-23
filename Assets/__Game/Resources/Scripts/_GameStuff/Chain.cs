using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Enums;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static __Game.Resources.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Chain : MonoBehaviour
  {
    public event Action Completed;

    [Header("Parameters")]
    [SerializeField] private ChainEnums _variantType;
    [SerializeField] private Sprite _variantSprite;
    [SerializeField] private string _variantText;
    [SerializeField] private AudioClip _variantClip;
    [Header("References")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private ChainButton _yesButton;
    [SerializeField] private ChainButton _noButton;
    [Space]
    [SerializeField] private AudioClip _correctClip;
    [SerializeField] private AudioClip _incorrectClip;

    private Button _yesButtonUi;
    private Button _noButtonUi;

    private AudioSource _audioSource;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<StateChanged> _stateChangedEvent;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;

      _yesButtonUi = _yesButton.GetComponent<Button>();
      _noButtonUi = _noButton.GetComponent<Button>();

      _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
      _stateChangedEvent = new EventBinding<StateChanged>(OnGamePlayState);

      _yesButton.ButtonCLicked += OnButtonClicked;
      _noButton.ButtonCLicked += OnButtonClicked;
    }

    private void OnDisable()
    {
      _stateChangedEvent.Remove(OnGamePlayState);

      _yesButton.ButtonCLicked -= OnButtonClicked;
      _noButton.ButtonCLicked -= OnButtonClicked;
    }

    private void Start()
    {
      _image.sprite = _variantSprite;
      _textMesh.text = _variantText;

      PlayVariantAudioCLip();
    }

    public void PlayVariantAudioCLip()
    {
      if (_gameBootstrapper.StateMachine.CurrentState is GameplayState)
      {
        EventBus<VariantAudioClickedEvent>.Raise(new VariantAudioClickedEvent { AudioClip = _variantClip });

        _audioSource.PlayOneShot(_correctClip);
      }
    }

    private void OnGamePlayState(StateChanged stateChanged)
    {
      if (stateChanged.State is GameplayState)
        EventBus<VariantAudioClickedEvent>.Raise(new VariantAudioClickedEvent { AudioClip = _variantClip });
    }

    private void OnButtonClicked(ChainEnums type)
    {
      if (type == _variantType)
      {
        _audioSource.PlayOneShot(_correctClip);

        Completed?.Invoke();
      }
      else
      {
        _audioSource.PlayOneShot(_incorrectClip);

        _gameBootstrapper.StateMachine.ChangeStateWithDelay(new GameLoseState(_gameBootstrapper), 0.5f, this);
      }

      DisableAllButtons();
    }

    private void DisableAllButtons()
    {
      _yesButtonUi.interactable = false;
      _noButtonUi.interactable = false;
    }
  }
}