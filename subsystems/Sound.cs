using System.Media;

namespace ChessGame.Subsystems
{
    internal class Sound
    {
        SoundPlayer moveSound = new("../../../data/move_sound.wav");
        SoundPlayer checkSound = new("../../../data/check_sound.wav");
        SoundPlayer checkmateSound = new("../../../data/checkmate_sound.wav");
        SoundPlayer castlingSound = new("../../../data/castling_sound.wav");
        SoundPlayer captureSound = new("../../../data/capture_sound.wav");
        SoundPlayer startSound = new("../../../data/game_start.wav");

        public void PlayMoveSound() { moveSound.Play(); }
        public void PlayCheckSound() { checkSound.Play(); }
        public void PlayCheckmateSound() { checkmateSound.Play(); }
        public void PlayCastlingSound() { castlingSound.Play(); }
        public void PlayCaptureSound() { captureSound.Play(); }
        public void PlayStartSound() { startSound.Play(); }
    }
}
