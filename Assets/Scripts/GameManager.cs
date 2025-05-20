using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event EventHandler<OnClickedOnGridPositionEventArgs> OnClickedOnGridPosition;
    public class OnClickedOnGridPositionEventArgs: EventArgs
    {
        public int x;
        public int y;
        public PlayerType playerType;
    }
    
    public enum PlayerType
    {
        None,
        Cross,
        Circle
    }

    private PlayerType _localPlayerType;
    private PlayerType _currentPlayablePlayerType;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public override void OnNetworkSpawn() {
        Debug.Log("OnNetworkSpawn: " + NetworkManager.Singleton.LocalClientId);

        if (NetworkManager.Singleton.LocalClientId == 0) {
            _localPlayerType = PlayerType.Cross;
        }
        else {
            _localPlayerType = PlayerType.Circle;
        }

        if (IsServer) {
            _currentPlayablePlayerType = PlayerType.Cross;
        }
    }

    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x, int y, PlayerType playerType) {
        Debug.Log("Clicked on grid position: " + x + ", " + y);

        if (playerType != _currentPlayablePlayerType) {
            return;
        }
        
        OnClickedOnGridPosition?.Invoke(this, new OnClickedOnGridPositionEventArgs {x = x, y = y, playerType = playerType});

        switch (_currentPlayablePlayerType) {
            default:
            case PlayerType.Cross:
                _currentPlayablePlayerType = PlayerType.Circle;
                break;
            case PlayerType.Circle:
                _currentPlayablePlayerType = PlayerType.Cross;
                break;
                
        }
    }
    
    public PlayerType GetLocalPlayerType() => _localPlayerType;
}
