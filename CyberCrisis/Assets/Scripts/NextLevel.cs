using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] string levelName;
    
    public void next() {
        SceneManager.LoadScene(levelName);
    }


}

