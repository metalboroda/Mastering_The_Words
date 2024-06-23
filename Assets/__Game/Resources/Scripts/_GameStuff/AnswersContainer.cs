using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using UnityEngine;
using static __Game.Resources.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class AnswersContainer : MonoBehaviour
  {
    [SerializeField] private ChainsItem ChainsItem;

    private int _currentChainIndex = 0;

    private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      foreach (var chain in ChainsItem.Chains)
      {
        chain.Completed += OnChainCompleted;
      }

      if (ChainsItem.Chains.Length > 0)
      {
        ChainsItem.Chains[_currentChainIndex].gameObject.SetActive(true);
      }
    }

    private void OnDisable()
    {
      foreach (var chain in ChainsItem.Chains)
      {
        chain.Completed -= OnChainCompleted;
      }
    }

    private void Start()
    {
      EventBus<VariantsAssignedEvent>.Raise(new VariantsAssignedEvent());
    }

    private void OnChainCompleted()
    {
      if (_currentChainIndex < ChainsItem.Chains.Length - 1)
      {
        ChainsItem.Chains[_currentChainIndex].gameObject.SetActive(false);

        _currentChainIndex++;

        ChainsItem.Chains[_currentChainIndex].gameObject.SetActive(true);
      }
      else
      {
        _gameBootstrapper.StateMachine.ChangeStateWithDelay(new GameWinState(_gameBootstrapper), 0.5f, this);
      }
    }
  }
}