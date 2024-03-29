using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string versionName = "0.1";
    [SerializeField] private GameObject usernameMenu;
    [SerializeField] private GameObject connectPanel;

    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField createGameInput;
    [SerializeField] private InputField joinGameInput;

    [SerializeField] private GameObject startButton;

    private void Awake() => PhotonNetwork.ConnectUsingSettings(versionName);

    private void Start() => usernameMenu.SetActive(true);

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        print("Connected");
    }

    public void ChangeUserNameInput()
    {
        if (usernameInput.text.Length >= 3)
            startButton.SetActive(true);
        else
            startButton.SetActive(false);
    }

    public void SetUserName()
    {
        usernameMenu.SetActive(false);

        PhotonNetwork.playerName = usernameInput.text;
    }

    /// Gameplay?
    
    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }
}
