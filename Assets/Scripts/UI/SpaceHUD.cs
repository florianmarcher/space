using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpaceHUD : MonoBehaviour
    {
        private Text prompt;
        
        public static SpaceHUD instance { get; private set; }

        private void Awake()
        {
            instance = this;
            prompt = transform.Find("Prompt").GetComponent<Text>();
            DisablePrompt();
        }

        public void SetPrompt(bool prompt_enabled, string text = null)
        {
            if(prompt_enabled)
                EnablePrompt(text);
            else
                DisablePrompt();
        }

        public void EnablePrompt(string text = null)
        {
            if (text != null)
                prompt.text = text;
            prompt.gameObject.SetActive(true);
        }

        public void DisablePrompt()
        {
            prompt.gameObject.SetActive(false);
        }
    }
}
