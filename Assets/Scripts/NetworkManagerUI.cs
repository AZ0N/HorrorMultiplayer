using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;

    private void Awake() {
        hostButton.onClick.AddListener(() => {NetworkManager.Singleton.StartHost(); gameObject.SetActive(false);});
        serverButton.onClick.AddListener(() => {NetworkManager.Singleton.StartServer(); gameObject.SetActive(false);});
        clientButton.onClick.AddListener(() => {NetworkManager.Singleton.StartClient(); gameObject.SetActive(false);});
    }
}
