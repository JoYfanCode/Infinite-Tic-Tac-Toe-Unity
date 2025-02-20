using UnityEngine;
using UnityEngine.UI;
namespace I2.Loc
{
    [AddComponentMenu("I2/Localization/SetLanguage Button")]
    [RequireComponent(typeof(Button))]
    public class SetLanguage : MonoBehaviour
    {
        public string _Language;
        Button _button;

#if UNITY_EDITOR
        public LanguageSource mSource;
#endif

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        void OnClick()
        {
            ApplyLanguage();
        }

        public void ApplyLanguage()
        {
            if (LocalizationManager.HasLanguage(_Language))
            {
                LocalizationManager.CurrentLanguage = _Language;
            }
        }
    }
}