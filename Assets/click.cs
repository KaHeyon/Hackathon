using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; // 전환할 씬의 이름을 Inspector에서 설정합니다.

    private void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되었을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 씬을 전환합니다.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
