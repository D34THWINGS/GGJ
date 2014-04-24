using System.Collections.Generic;

namespace XRay.UI {
    public static class StaticVariables {
        public enum Powers {
            Vision,
            ReshapeCircle,
            Reveal,
            ChangeWeight,
            ChangeMaterial
        }

        private static readonly List<Powers> EnabledPowers = new List<Powers>() {
            Powers.Vision,
            Powers.ChangeWeight,
            Powers.ReshapeCircle,
            Powers.Reveal,
            Powers.ChangeMaterial
        };

        public static float LightWeight = 1.2f;
        public static float HeavyWeight = 3f;
        public static bool IsOnTuto = false;

        public static bool HasPower(Powers power) {
            return EnabledPowers.Contains(power);
        }

        public static void AddPower(Powers power) {
            EnabledPowers.Add(power);
        }
    }
}