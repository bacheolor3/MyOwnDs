using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace TSG
{
    public class UI_Match_Scroll_Wheel_To_Selected_Button : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected;
        [SerializeField] GameObject previouslySelected;
        [SerializeField] RectTransform currentSelectedTransform;
        [SerializeField] RectTransform contentPanel;
        [SerializeField] ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if(currentSelected != null)
            {
                if(currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = 
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            // Y축 방향으로 락을 걸길 바라는 것이기 때문에 조치를 해야 함
            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }    
}
