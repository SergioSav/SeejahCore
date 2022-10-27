using System.Collections;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Assets.Scripts.Core
{
    public interface ISceneLoader
    {
        IEnumerator LoadScene(string sceneName, IInstaller installer, LifetimeScope scope);
    }

    public class SceneLoader : ISceneLoader
    {
        public IEnumerator LoadScene(string sceneName, IInstaller installer, LifetimeScope scope)
        {
            using (LifetimeScope.EnqueueParent(scope))
            using (LifetimeScope.Enqueue(installer))
            {
                var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }
    }
}
