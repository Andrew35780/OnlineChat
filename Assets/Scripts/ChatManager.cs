using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public Player plMove;
    public PhotonView photonView;
    public GameObject bubleSpeechObject;
    public Text updatedText;

    private InputField chatInputField;
    private bool disableSend;

    private void Awake() => chatInputField = GameObject.Find("ChatInputField").GetComponent<InputField>();

    private void Update()
    {
        if (photonView.isMine) 
        {
            if (!disableSend && chatInputField.isFocused)
            {
                if(chatInputField.text != "" && chatInputField.text.Length > 0 && chatInputField.touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done) 
                {
                    photonView.RPC("SendMessage", PhotonTargets.AllBuffered, chatInputField.text);

                    bubleSpeechObject.SetActive(true);

                    chatInputField.text = string.Empty;
                    disableSend = true;
                }
            }
        }
    }

    [PunRPC] private void SendMessage(string message)
    {
        updatedText.text = message;

        StartCoroutine("Remove"); // Delete??
    }

    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(4f);

        bubleSpeechObject.SetActive(false);

        disableSend = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
            stream.SendNext(bubleSpeechObject.active);
        else if (stream.isReading)
            bubleSpeechObject.SetActive((bool)stream.ReceiveNext());
    }
}
