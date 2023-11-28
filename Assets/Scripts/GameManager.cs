using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject sceneCamera;
    public GameObject disconnectUi;
    public Text pingText;

    private bool off = false;

    public GameObject playerFeed; 
    public GameObject feedGrid; 

    private void Awake() => gameCanvas.SetActive(true);

    private void Update()
    {
        pingText.text = $"Ping: {PhotonNetwork.GetPing()}";

        CheckInput();
    }

    private void CheckInput()
    {
        if (off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUi.SetActive(false);
            off = false;
        }
        else if (!off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUi.SetActive(true);
            off = true;
        }
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(feedGrid.transform, false);

        obj.GetComponent<Text>().text = $"{player.name} joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(feedGrid.transform, false);

        obj.GetComponent<Text>().text = $"{player.name} left the game";
        obj.GetComponent<Text>().color = Color.red;
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f, 1f); // ??-+

        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(transform.position.x * randomValue, transform.position.y), Quaternion.identity, 0);
        
        gameCanvas.SetActive(false);
        sceneCamera.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(gameObject);
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
