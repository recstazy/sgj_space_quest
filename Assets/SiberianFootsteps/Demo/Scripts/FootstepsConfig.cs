﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Recstazy.SiberianFootsteps.Demo
{
    public class FootstepsConfig : ScriptableObject
    {
        [System.Serializable]
        public class FootstepMapping
        {
            public PhysicMaterial Material;
            public RandomSound WalkSound;
            public RandomSound RunSound;
        }

        [SerializeField]
        private RandomSound _defaultWalkSound;

        [SerializeField]
        private RandomSound _defaultRunSound;

        [SerializeField]
        private FootstepMapping[] _mappings;

        private Dictionary<PhysicMaterial, FootstepMapping> _sounds;
        private FootstepMapping DefaultMapping { get; set; }
        public FootstepMapping[] Mappings => _mappings;

        public FootstepMapping GetDefaultMapping()
        {
            EnsureMappings();
            return DefaultMapping;
        }
        
        public FootstepMapping GetMapping(PhysicMaterial material)
        {
            EnsureMappings();
            if (_sounds.TryGetValue(material, out var mapping)) return mapping;
            else return DefaultMapping;
        }

        private void EnsureMappings()
        {
            if (_sounds == null) _sounds = new Dictionary<PhysicMaterial, FootstepMapping>();
            if (_sounds.Count != _mappings.Length) CreateMappings();
        }

        private void CreateMappings()
        {
            _sounds.Clear();
            DefaultMapping = new FootstepMapping() { WalkSound = _defaultWalkSound, RunSound = _defaultRunSound };

            foreach (var m in _mappings)
            {
                _sounds.Add(m.Material, m);
            }
        }
    }
}
