﻿using Leopotam.Ecs;

namespace Eran {
    class InSceneResult : IEcsAutoReset {
        public string ScenePath;

        public void Reset() {
            ScenePath = default;
        }
    }
}