﻿using UnityEngine;

namespace OutFoxeed.CustomHierarchy
{
    [CreateAssetMenu(fileName = "Custom Hierarchy Parameters", menuName = "Hierarchy Parameters", order = 0)]
    public class HierarchyRulesSO : ScriptableObject
    {
        public HierarchyRule[] startsWithHierarchyRules;
        public HierarchyRule[] endsWithHierarchyRules;
        public HierarchyRule[] containsHierarchyRules;

        public ComponentHierarchyRule[] componentHierarchyRules;
    }
}