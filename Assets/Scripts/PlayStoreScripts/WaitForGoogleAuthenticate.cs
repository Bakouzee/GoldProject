using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayStoreScripts
{
    public class WaitForGoogleAuthenticate : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button manualAuthenticateButton;
        
        bool waitingForResult = true;
    
        public void Start()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            
            PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
        }

        private void OnSignInResult(SignInStatus status)
        {
            PlayServices.usePlayServices = status == SignInStatus.Success;
            Debug.Log($"usePlayServices = {PlayServices.usePlayServices}");

            if (PlayServices.usePlayServices)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }
            
            manualAuthenticateButton.gameObject.SetActive(true);
            manualAuthenticateButton.onClick.RemoveAllListeners();
            manualAuthenticateButton.onClick.AddListener(() => PlayGamesPlatform.Instance.ManuallyAuthenticate(OnSignInResult));
        }
    }
}