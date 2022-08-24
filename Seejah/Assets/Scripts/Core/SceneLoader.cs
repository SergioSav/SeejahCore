using System.Collections;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Assets.Scripts.Core
{
    public interface ISceneLoader
    {
        IEnumerator LoadScene(string sceneName, IInstaller installer);
    }

    public class SceneLoader : ISceneLoader
    {
        private readonly LifetimeScope _parentScope;

        public SceneLoader(LifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }

        public IEnumerator LoadScene(string sceneName, IInstaller installer)
        {
            using (LifetimeScope.EnqueueParent(_parentScope))
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
