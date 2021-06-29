using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
   public void GameStart()
   {
     SceneLoader.Instance.SceneChange();
   }

   public void GameExit()
   {
#if UNITY_EDITOR
       UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
   }
}
