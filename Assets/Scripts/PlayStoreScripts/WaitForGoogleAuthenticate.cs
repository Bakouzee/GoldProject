using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayStoreScripts
{
    public class WaitForGoogleAuthenticate : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button manualAuthenticateButton;
        [SerializeField] private UnityEngine.UI.Button skipButton;
        
        bool waitingForResult = true;
    
        public void Start()
        {
            manualAuthenticateButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            
            PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
            PlayGamesPlatform.DebugLogEnabled = false;
            PlayGamesPlatform.Activate();
        }

        private void OnSignInResult(SignInStatus status)
        {
            PlayServices.usePlayServices = status == SignInStatus.Success;
            Debug.Log($"usePlayServices = {PlayServices.usePlayServices}");

            if (PlayServices.usePlayServices)
            {
                LoadNextScene();
                return;
            }
            
            // Enable manual authentication button and skip button
                // Authenticate
            manualAuthenticateButton.gameObject.SetActive(true);
            manualAuthenticateButton.onClick.RemoveAllListeners();
            manualAuthenticateButton.onClick.AddListener(() => PlayGamesPlatform.Instance.ManuallyAuthenticate(OnSignInResult));
                // Skip
            skipButton.gameObject.SetActive(true);
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(LoadNextScene);
        }

        private void LoadNextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}