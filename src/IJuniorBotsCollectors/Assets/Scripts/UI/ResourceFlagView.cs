using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourceFlagView : MonoBehaviour
    {
        private const string StartCount = "0";

        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Flag _resourceBase;

        private void Start()
        {
            //_canvas.worldCamera = Camera.main;
            _text.text = StartCount;
        }

        private void OnEnable() => 
            _resourceBase.CollectedResourcesCountChanged += UpdateText;

        private void OnDisable() => 
            _resourceBase.CollectedResourcesCountChanged -= UpdateText;

        private void UpdateText(int count) => 
            _text.text = count.ToString();
    }
}