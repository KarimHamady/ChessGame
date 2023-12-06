using System.Media;

namespace ChessGame.Subsystems
{
    internal class Sound
    {
        SoundPlayer moveSound = new("../../../data/move_sound.wav");
        SoundPlayer checkSound = new("../../../data/check_sound.wav");
        SoundPlayer checkmateSound = new("../../../data/checkmate_sound.wav");

        public void PlayMoveSound() { moveSound.Play(); }
        public void PlayCheckSound() { checkSound.Play(); }
        public void PlayCheckmateSound() { checkmateSound.Play(); }
    }
}
