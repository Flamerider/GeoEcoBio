using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace geb
{
    public class checkIfCompleted : global
    {
        Image buttonImage;
        public string associatedLevel;

        void Start()
        {
            buttonImage = gameObject.GetComponent<Image>();

            if (DataLoad().completedLevels.Contains(associatedLevel))
                buttonImage.color = Color.yellow;
        }
    }
}