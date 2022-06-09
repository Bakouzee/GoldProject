using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayStore_scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldProject.PlayStore_scripts
{
    public class WaitForGoogleAuthenticate : MonoBehaviour
    {
        public void Start()
        {
            //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            PlayServices.usePlayServices = status == SignInStatus.Success;
            StartCoroutine(GoToNextScene());
            
            
            // if (status == SignInStatus.Success)
            // {
            //     
            //     // Continue with Play Games Services
            // }
            // else
            // {
            //     // Disable your integration with Play Games Services or show a login button
            //     // to ask users to sign-in. Clicking it should call
            //     // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            // }
        }

        IEnumerator GoToNextScene()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}