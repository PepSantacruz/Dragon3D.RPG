namespace RPG.Core {
    public static class Constants {
        //Player
        public const string PLAYER_TAG = "Player";

        //Camera raycast
        public const int WALKABLE_LAYER = 9;
        public const float MAX_RAYCASTER_DEPTH = 100f;

        //Particle Effects
        public const float PARTICLE_CLEAN_UP_DELAY = 10.0f;
        public const float PARTICLE_Y_OFFSET = 1.2f;

        //Animation
        public const string ATTACK_TRIGGER = "Attack";
        public const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        public const string DEATH_TRIGGER = "Death";
        public const string DEFAULT_DEATH = "DEFAULT DEATH";

        public const float SLOW_MOTION_RATE = 0.05f;
        public const float SLOW_MOTION_TIME = 0.1f;
        public const float ANIMATION_HIT_OFFSET = 0.1f;
    }
}
