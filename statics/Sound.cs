using System.Media;

namespace ChessGame.Statics
{
    internal class Sound
    {
        static SoundPlayer moveSound = new("../../../data/move_sound.wav");
        static SoundPlayer checkSound = new("../../../data/check_sound.wav");
        static SoundPlayer checkmateSound = new("../../../data/checkmate_sound.wav");

        public static void PlayMoveSound() { moveSound.Play(); }
        public static void PlayCheckSound() {  checkSound.Play(); }
        public static void PlayCheckmateSound() {  checkmateSound.Play(); }
    }
}
