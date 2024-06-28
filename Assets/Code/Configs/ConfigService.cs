using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace TN141.Configs
{
    public class ConfigService : IInitializable, IDisposable
    {
        [Inject]
        private readonly EventDispatcher _eventDispatcher;

        public bool IsLoaded => _config != null;
        
        // TODO
        private const string configName = "data";
        
        private readonly CancellationTokenSource _cts = new();

        private Data _config;
        
        public void Initialize()
        {
            LoadConfigs(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        public CameraModel GetCameraSettings()
        {
            return _config?.cameraSettings;
        }

        public Stat[] GetPlayerStats()
        {
            return _config?.stats;
        }

        public Buff[] GetBuffs()
        {
            return _config?.buffs;
        }

        public GameModel GetBuffSettings()
        {
            return _config?.settings;
        }

        private async UniTask LoadConfigs(CancellationToken token)
        {
            var textAsset = await Resources.LoadAsync<TextAsset>(configName)
                .ToUniTask(cancellationToken: token);
            if (textAsset == null)
                throw new FileNotFoundException($"Failed to load config: '{configName}' not found");

            string json = ((TextAsset) textAsset).text;
            _config = JsonConvert.DeserializeObject<Data>(json);
            
            _eventDispatcher.Trigger<OnConfigsLoaded>();
        }
    }
}